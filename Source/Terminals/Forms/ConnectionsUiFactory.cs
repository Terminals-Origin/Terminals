using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Windows.Forms;
using Terminals.CaptureManager;
using Terminals.Configuration;
using Terminals.Connections;
using Terminals.Data;
using Terminals.Data.Credentials;
using Terminals.Network;

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

        internal ConnectionsUiFactory(MainForm mainForm, TerminalTabsSelectionControler terminalsControler,
            IPersistence persistence, ConnectionManager connectionManager)
        {
            this.mainForm = mainForm;
            this.terminalsControler = terminalsControler;
            this.persistence = persistence;
            this.connectionManager = connectionManager;
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

        /// <summary>
        /// Connects to all favorites required by definition.
        /// </summary>
        /// <param name="definition">not null definition of the connection behavior</param>
        internal void Connect(ConnectionDefinition definition)
        {
            if (string.IsNullOrEmpty(definition.NewFavorite)) // only one in this case
                this.ConnectToAll(definition);
            else
                this.CreateNewTerminal(definition.NewFavorite);
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
            IFavorite favoriteCopy = GetFavoriteUpdatedCopy(favorite, definition);
            this.persistence.ConnectionHistory.RecordHistoryItem(favorite);
            this.mainForm.SendNativeMessageToFocus();
            this.CreateTerminalTab(favoriteCopy);
        }

        private IFavorite GetFavoriteUpdatedCopy(IFavorite favorite, ConnectionDefinition definition)
        {
            // TODO ensure the ID was copied, otherwise the tabControl can never communicate 
            // with rest of the app, because it will never find it by ID.
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

        internal void CreateNewTerminal(String name = null)
        {
            using (var frmNewTerminal = new NewTerminalForm(this.persistence, this.connectionManager, name))
            {
                TerminalFormDialogResult result = frmNewTerminal.ShowDialog();
                if (result != TerminalFormDialogResult.Cancel)
                {
                    string favoriteName = frmNewTerminal.Favorite.Name;
                    this.mainForm.FocusFavoriteInQuickConnectCombobox(favoriteName);

                    if (result == TerminalFormDialogResult.SaveAndConnect)
                        this.CreateTerminalTab(frmNewTerminal.Favorite);
                }
            }
        }

        internal void CreateReleaseTab()
        {
            this.CreateTerminalTab(FavoritesFactory.CreateReleaseFavorite(this.persistence.Factory, this.connectionManager));
        }

        internal void CreateTerminalTab(IFavorite favorite)
        {
            CallExecuteBeforeConnectedFromSettings();
            CallExecuteFeforeConnectedFromFavorite(favorite);

            TerminalTabControlItem terminalTabPage = CreateTerminalTabPageByFavoriteName(favorite);
            this.TryConnectTabPage(favorite, terminalTabPage);
        }

        private void CallExecuteBeforeConnectedFromSettings()
        {
            if (settings.ExecuteBeforeConnect && !string.IsNullOrEmpty(settings.ExecuteBeforeConnectCommand))
            {
                var processStartInfo = new ProcessStartInfo(settings.ExecuteBeforeConnectCommand, settings.ExecuteBeforeConnectArgs);
                processStartInfo.WorkingDirectory = settings.ExecuteBeforeConnectInitialDirectory;
                Process process = Process.Start(processStartInfo);
                if (settings.ExecuteBeforeConnectWaitForExit)
                {
                    process.WaitForExit();
                }
            }
        }

        private static void CallExecuteFeforeConnectedFromFavorite(IFavorite favorite)
        {
            IBeforeConnectExecuteOptions executeOptions = favorite.ExecuteBeforeConnect;
            if (executeOptions.Execute && !string.IsNullOrEmpty(executeOptions.Command))
            {
                var processStartInfo = new ProcessStartInfo(executeOptions.Command, executeOptions.CommandArguments);
                processStartInfo.WorkingDirectory = executeOptions.InitialDirectory;
                Process process = Process.Start(processStartInfo);
                if (executeOptions.WaitForExit)
                {
                    process.WaitForExit();
                }
            }
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

        private void TryConnectTabPage(IFavorite favorite, TerminalTabControlItem terminalTabPage)
        {
            try
            {
                this.mainForm.AssignEventsToConnectionTab(terminalTabPage);
                var toolTipBuilder = new ToolTipBuilder(this.persistence.Security);
                string toolTipText = toolTipBuilder.BuildTooTip(favorite);
                this.ConfigureTabPage(terminalTabPage, toolTipText, true, favorite);

                Connection conn = CreateConnection(favorite, terminalTabPage, this.mainForm);
                this.UpdateConnectionTabPageByConnectionState(favorite, terminalTabPage, conn);

                if (conn.Connected && favorite.NewWindow)
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

        private Connection CreateConnection(IFavorite favorite, TerminalTabControlItem terminalTabPage, MainForm parentForm)
        {
            Connection conn = this.connectionManager.CreateConnection(favorite);
            conn.Favorite = favorite;

            var consumer = conn as ISettingsConsumer;
            if (consumer != null)
                consumer.Settings = this.settings;

            AssignControls(conn, terminalTabPage, parentForm);
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
            bool allowDrop = false, IFavorite favorite = null)
        {
            terminalTabPage.AllowDrop = allowDrop;
            terminalTabPage.ToolTipText = captureTitle;
            terminalTabPage.Favorite = favorite;
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
    }
}
