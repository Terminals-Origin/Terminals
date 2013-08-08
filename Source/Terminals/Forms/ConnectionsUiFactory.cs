using System;
using System.Diagnostics;
using System.Windows.Forms;
using Terminals.Configuration;
using Terminals.Connections;
using Terminals.Data;
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

        internal ConnectionsUiFactory(MainForm mainForm, TerminalTabsSelectionControler terminalsControler)
        {
            this.mainForm = mainForm;
            this.terminalsControler = terminalsControler;
        }

        internal void CreateCaptureManagerTab()
        {
            Action<Connection> executeExtra = (connection) => { };
            this.OpenConenction<CaptureManagerConnection>("CaptureManager", "Error loading the Capture Manager Tab Page", executeExtra);
        }

        internal void OpenNetworkingTools(NettworkingTools action, string host)
        {
            Action<NetworkingToolsConnection> executeExtra = (connection) => connection.Execute(action, host);
            this.OpenConenction("NetworkingTools", "Open Networking Tools Failure", executeExtra);
        }

        private void OpenConenction<TConnection>(string titleResourceKey, string openErrorMessage, Action<TConnection> executeExtra)
            where TConnection : Connection
        {
            string title = Program.Resources.GetString(titleResourceKey);
            var terminalTabPage = new TerminalTabControlItem(title);
            try
            {
                this.ConfigureTabPage(terminalTabPage, title);
                var conn = Activator.CreateInstance<TConnection>();
                this.ConfigureConnection(conn, terminalTabPage);
                executeExtra(conn);
            }
            catch (Exception exc)
            {
                Logging.Log.Error(openErrorMessage, exc);
                this.terminalsControler.RemoveAndUnSelect(terminalTabPage);
                terminalTabPage.Dispose();
            }
        }

        private void ConfigureConnection(Connection conn, TerminalTabControlItem terminalTabPage)
        {
            conn.TerminalTabPage = terminalTabPage;
            conn.ParentForm = this.mainForm;
            conn.Connect();
            this.BringToFrontOnMainForm(conn);
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
            Persistence.Instance.ConnectionHistory.RecordHistoryItem(favorite);
            this.mainForm.SendNativeMessageToFocus();
            this.CreateTerminalTab(favoriteCopy);
        }

        private static IFavorite GetFavoriteUpdatedCopy(IFavorite favorite, ConnectionDefinition definition)
        {
            IFavorite favoriteCopy = favorite.Copy();
            UpdateForceConsole(favoriteCopy, definition);
            
            if (definition.ForceNewWindow.HasValue)
                favoriteCopy.NewWindow = definition.ForceNewWindow.Value;
            
            favoriteCopy.Security.UpdateFromCredential(definition.Credentials);
            return favoriteCopy;
        }

        private static void UpdateForceConsole(IFavorite favorite, ConnectionDefinition definition)
        {
            if (!definition.ForceConsole.HasValue)
                return;

            var rdpOptions = favorite.ProtocolProperties as RdpOptions;
            if (rdpOptions != null)
                rdpOptions.ConnectToConsole = definition.ForceConsole.Value;
        }

        internal void CreateNewTerminal(String name = null)
        {
            using (var frmNewTerminal = new NewTerminalForm(name))
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
            this.CreateTerminalTab(FavoritesFactory.CreateReleaseFavorite());
        }

        internal void CreateTerminalTab(IFavorite favorite)
        {
            CallExecuteBeforeConnectedFromSettings();
            CallExecuteFeforeConnectedFromFavorite(favorite);

            TerminalTabControlItem terminalTabPage = CreateTerminalTabPageByFavoriteName(favorite);
            this.TryConnectTabPage(favorite, terminalTabPage);
        }

        private static void CallExecuteBeforeConnectedFromSettings()
        {
            if (Settings.ExecuteBeforeConnect && !string.IsNullOrEmpty(Settings.ExecuteBeforeConnectCommand))
            {
                var processStartInfo = new ProcessStartInfo(Settings.ExecuteBeforeConnectCommand, Settings.ExecuteBeforeConnectArgs);
                processStartInfo.WorkingDirectory = Settings.ExecuteBeforeConnectInitialDirectory;
                Process process = Process.Start(processStartInfo);
                if (Settings.ExecuteBeforeConnectWaitForExit)
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

        private static TerminalTabControlItem CreateTerminalTabPageByFavoriteName(IFavorite favorite)
        {
            String terminalTabTitle = favorite.Name;
            if (Settings.ShowUserNameInTitle)
            {
                var security = favorite.Security;
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
                this.ConfigureTabPage(terminalTabPage, favorite.GetToolTipText(), true, favorite);

                Connection conn = CreateConnection(favorite, terminalTabPage, this.mainForm);
                this.UpdateConnectionTabPageByConnectionState(favorite, terminalTabPage, conn);

                if (conn.Connected && favorite.NewWindow)
                {
                    this.terminalsControler.DetachTabToNewWindow(terminalTabPage);
                }
            }
            catch (Exception exc)
            {
                Logging.Log.Error("Error Creating A Terminal Tab", exc);
                this.terminalsControler.UnSelect();
            }
        }

        private static Connection CreateConnection(IFavorite favorite, TerminalTabControlItem terminalTabPage, MainForm parentForm)
        {
            Connection conn = ConnectionManager.CreateConnection(favorite);
            conn.Favorite = favorite;
            terminalTabPage.Connection = conn;
            conn.TerminalTabPage = terminalTabPage;
            conn.ParentForm = parentForm;
            return conn;
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

                 conn.CheckForTerminalServer(favorite);
            }
            else
            {
                String msg = Program.Resources.GetString("SorryTerminalswasunabletoconnecttotheremotemachineTryagainorcheckthelogformoreinformation");
                MessageBox.Show(msg);
                this.terminalsControler.RemoveAndUnSelect(terminalTabPage);
            }
        }

        private void BringToFrontOnMainForm(Control conn)
        {
            conn.BringToFront();
            conn.Update();
            this.mainForm.UpdateControls();
        }
    }
}
