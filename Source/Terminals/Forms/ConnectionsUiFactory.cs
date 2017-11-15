using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using Terminals.CaptureManager;
using Terminals.Configuration;
using Terminals.Connections;
using Terminals.Data;
using Terminals.Data.Credentials;
using Terminals.Forms.Controls;
using Terminals.Network;
using Terminals.Services;

namespace Terminals.Forms
{
    /// <summary>
    /// Responsible to create and connect connections user interface
    /// </summary>
    internal class ConnectionsUiFactory
    {
        private readonly MainForm mainForm;
        private readonly TerminalTabsSelectionControler terminalsControler;

        private readonly IPersistence persistence;

        private readonly Settings settings = Settings.Instance;

        private readonly GuardedCredentialFactory guardedCredentialFactory;

        private readonly ConnectionManager connectionManager;

        private readonly FavoriteIcons favoriteIcons;

        internal ConnectionsUiFactory(MainForm mainForm, TerminalTabsSelectionControler terminalsControler,
            IPersistence persistence, ConnectionManager connectionManager, FavoriteIcons favoriteIcons)
        {
            this.mainForm = mainForm;
            this.terminalsControler = terminalsControler;
            this.persistence = persistence;
            this.connectionManager = connectionManager;
            this.favoriteIcons = favoriteIcons;
            this.guardedCredentialFactory = new GuardedCredentialFactory(this.persistence);
        }

        internal void CreateCaptureManagerTab()
        {
            Action<CaptureManagerLayout> executeExtra = control => { };
            this.OpenTabControl("CaptureManager", CaptureManagerLayout.ControlName,
                "Error loading the Capture Manager Tab Page", executeExtra);
        }

        internal void OpenNetworkingTools(NettworkingTools action, string host)
        {
            Action<NetworkingToolsLayout> executeExtra = control => { control.Execute(action, host, this.persistence); };
            this.OpenTabControl("NetworkingTools", "NetworkingTools", "Open Networking Tools Failure", executeExtra);
        }

        private void OpenTabControl<TControl>(string titleResourceKey,  string controlName,
            string openErrorMessage, Action<TControl> executeExtra)
            where TControl : UserControl
        {
            string title = Program.Resources.GetString(titleResourceKey);
            var terminalTabPage = new TerminalTabControlItem(title);
            try
            {
                this.ConfigureTabPage(title, controlName, executeExtra, terminalTabPage);
            }
            catch (Exception exc)
            {
                Logging.Error(openErrorMessage, exc);
                this.terminalsControler.RemoveAndUnSelect(terminalTabPage);
                terminalTabPage.Dispose();
            }
        }

        private void ConfigureTabPage<TControl>(string title, string controlName,
            Action<TControl> executeExtra, TerminalTabControlItem terminalTabPage)
            where TControl : UserControl
        {
            this.ConfigureTabPage(terminalTabPage, title);
            var control = Activator.CreateInstance<TControl>();
            control.Name = controlName;
            control.Dock = DockStyle.Fill;
            control.Parent = terminalTabPage;
            executeExtra(control);
            this.BringToFrontOnMainForm(control);
        }

        internal void ConnectByFavoriteNames(IEnumerable<string> favoriteNames, bool forceConsole = false, bool forceNewWindow = false, ICredentialSet credentials = null)
        {
            if (favoriteNames == null || favoriteNames.Count() < 1)
                return;

            var targets = this.persistence.Favorites
                .Where(favorite => favoriteNames.Contains(favorite.Name, StringComparer.InvariantCultureIgnoreCase));
            var definition = new ConnectionDefinition(targets, forceConsole, forceNewWindow, credentials, targets.Any<IFavorite>() ? string.Empty : favoriteNames.First());
            this.Connect(definition);
        }

        internal void Connect(IFavorite favorite)
        {
            var definition = new ConnectionDefinition(favorite);
            this.Connect(definition);
        }

        /// <summary>
        /// Connects to all favorites required by definition.
        /// </summary>
        /// <param name="definition">not null definition of the connection behavior</param>
        internal void Connect(ConnectionDefinition definition)
        {
            if (string.IsNullOrEmpty(definition.NewFavorite)) // only one in this case
                this.ConnectToAll(definition);
            else
                this.CreateFavorite(definition.NewFavorite);
        }

        private void ConnectToAll(ConnectionDefinition connectionDefinition)
        {
            foreach (IFavorite favorite in connectionDefinition.Favorites)
            {
                this.Connect(favorite, connectionDefinition);
            }
        }

        private void Connect(IFavorite favorite, ConnectionDefinition definition)
        {
            IFavorite configured = GetFavoriteUpdatedCopy(favorite, definition);
            this.persistence.ConnectionHistory.RecordHistoryItem(favorite);
            this.mainForm.SendNativeMessageToFocus();
            this.CreateTerminalTab(favorite, configured);
        }

        private IFavorite GetFavoriteUpdatedCopy(IFavorite favorite, ConnectionDefinition definition)
        {
            IFavorite favoriteCopy = favorite.Copy();
            UpdateForceConsole(favoriteCopy, definition);
            
            if (definition.ForceNewWindow.HasValue)
                favoriteCopy.NewWindow = definition.ForceNewWindow.Value;

            var guarded = this.guardedCredentialFactory.CreateSecurityOptoins(favoriteCopy.Security);
            guarded.UpdateFromCredential(definition.Credentials);
            return favoriteCopy;
        }

        private static void UpdateForceConsole(IFavorite favorite, ConnectionDefinition definition)
        {
            if (!definition.ForceConsole.HasValue)
                return;

            var rdpOptions = favorite.ProtocolProperties as IForceConsoleOptions;
            if (rdpOptions != null)
                rdpOptions.ConnectToConsole = definition.ForceConsole.Value;
        }

        private void CreateTerminalTab(IFavorite origin, IFavorite configured)
        {
            ExternalLinks.CallExecuteBeforeConnected(this.settings);
            ExternalLinks.CallExecuteBeforeConnected(configured.ExecuteBeforeConnect);
            TerminalTabControlItem terminalTabPage = CreateTerminalTabPageByFavoriteName(configured);
            this.TryConnectTabPage(origin, configured, terminalTabPage);
        }

        private TerminalTabControlItem CreateTerminalTabPageByFavoriteName(IFavorite favorite)
        {
            String terminalTabTitle = favorite.Name;
            if (settings.ShowUserNameInTitle)
            {
                var security = this.guardedCredentialFactory.CreateCredential(favorite.Security);
                string title = HelperFunctions.UserDisplayName(security.Domain, security.UserName);
                terminalTabTitle += String.Format(" ({0})", title);
            }

            return new TerminalTabControlItem(terminalTabTitle);
        }

        private void TryConnectTabPage(IFavorite origin, IFavorite configured, TerminalTabControlItem terminalTabPage)
        {
            try
            {
                this.mainForm.AssignEventsToConnectionTab(terminalTabPage);
                var toolTipBuilder = new ToolTipBuilder(this.persistence.Security);
                string toolTipText = toolTipBuilder.BuildTooTip(configured);
                this.ConfigureTabPage(terminalTabPage, toolTipText, true);

                Connection conn = CreateConnection(origin, configured, terminalTabPage);
                this.UpdateConnectionTabPageByConnectionState(configured, terminalTabPage, conn);

                if (conn.Connected && configured.NewWindow)
                {
                    this.terminalsControler.DetachTabToNewWindow(terminalTabPage);
                }
            }
            catch (Exception exc)
            {
                Logging.Error("Error Creating A Terminal Tab", exc);
                this.terminalsControler.UnSelect();
            }
        }

        private Connection CreateConnection(IFavorite origin, IFavorite configured, TerminalTabControlItem terminalTabPage)
        {
            Connection conn = this.connectionManager.CreateConnection(configured);
            conn.Favorite = configured;
            conn.OriginFavorite = origin;

            var consumer = conn as ISettingsConsumer;
            if (consumer != null)
                consumer.Settings = this.settings;

            AssignControls(conn, terminalTabPage, this.mainForm);
            return conn;
        }

        private void AssignControls(Connection conn, TerminalTabControlItem terminalTabPage, MainForm parentForm)
        {
            terminalTabPage.Connection = conn;
            conn.Parent = terminalTabPage;
            conn.ParentForm = parentForm;
            conn.CredentialFactory = this.guardedCredentialFactory;
            conn.OnDisconnected += parentForm.OnDisconnected;
        }

        private void ConfigureTabPage(TerminalTabControlItem terminalTabPage, string captureTitle,
            bool allowDrop = false)
        {
            terminalTabPage.AllowDrop = allowDrop;
            terminalTabPage.ToolTipText = captureTitle;
            this.mainForm.AssingDoubleClickEventHandler(terminalTabPage);
            this.terminalsControler.AddAndSelect(terminalTabPage);
            this.mainForm.UpdateControls();
        }

        private void UpdateConnectionTabPageByConnectionState(IFavorite favorite, TerminalTabControlItem terminalTabPage, Connection conn)
        {
            if (conn.Connect())
            {
                this.BringToFrontOnMainForm(conn);
                if (favorite.Display.DesktopSize == DesktopSize.FullScreen)
                    this.mainForm.FullScreen = true;
            }
            else
            {
                String msg = Program.Resources.GetString("SorryTerminalswasunabletoconnecttotheremotemachineTryagainorcheckthelogformoreinformation");
                if (!string.IsNullOrEmpty(conn.LastError))
                    msg = msg + "\r\n\r\nDetails:\r\n" + conn.LastError;
                MessageBox.Show(msg);
                this.terminalsControler.RemoveAndUnSelect(terminalTabPage);
            }
        }

        private void BringToFrontOnMainForm(Control tabContentControl)
        {
            tabContentControl.BringToFront();
            tabContentControl.Update();
            this.mainForm.UpdateControls();
        }

        internal void CreateFavorite(string favoriteName = null, GroupTreeNode groupNode = null)
        {
            this.CreateFavorite(dr => { }, favoriteName, groupNode);
        }

        internal void CreateFavorite(Action<TerminalFormDialogResult> callback,
            string favoriteName = null, GroupTreeNode groupNode = null)
        {
            using (NewTerminalForm createForm = this.CreateFavoriteForm(favoriteName))
            {
                if (groupNode != null)
                    createForm.AssingSelectedGroup(groupNode.Group);

                // TODO add adhoc connection option in case the dialog result is connect only
                this.ShowFavoriteForm(createForm, dr => this.FinishCreateFavorie(dr, createForm, callback));
            }
        }

        private void FinishCreateFavorie(TerminalFormDialogResult dialogResult, NewTerminalForm createForm,
            Action<TerminalFormDialogResult> callback)
        {
            if (dialogResult != TerminalFormDialogResult.Cancel)
            {
                string newFavoriteName = createForm.Favorite.Name;
                this.mainForm.FocusFavoriteInQuickConnectCombobox(newFavoriteName);
            }

            callback(dialogResult);
        }

        internal void EditFavorite(IFavorite favorite, Action<TerminalFormDialogResult> callback)
        {
            using (NewTerminalForm editForm = this.CreateFavoriteForm(favorite))
            {
                this.ShowFavoriteForm(editForm, callback);
            }
        }

        private void ShowFavoriteForm(NewTerminalForm editForm, Action<TerminalFormDialogResult> callback)
        {
            TerminalFormDialogResult dialogResult = editForm.ShowDialog();

            if (dialogResult == TerminalFormDialogResult.SaveAndConnect)
                this.Connect(editForm.Favorite);

            callback(dialogResult);
        }

        private NewTerminalForm CreateFavoriteForm(string favoriteName)
        {
            return new NewTerminalForm(this.persistence, this.connectionManager,
                this.favoriteIcons, favoriteName);
        }

        private NewTerminalForm CreateFavoriteForm(IFavorite favorite)
        {
            return new NewTerminalForm(this.persistence, this.connectionManager,
                this.favoriteIcons, favorite);
        }
    }
}
