using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Threading;
using System.Windows.Forms;
using AxMSTSCLib;
using TabControl;
using Terminals.Forms;
using Terminals.Forms.Rendering;
using Terminals.CommandLine;
using Terminals.Configuration;
using Terminals.Connections;
using Terminals.Credentials;
using Terminals.Properties;
using Unified.Rss;
using Settings = Terminals.Configuration.Settings;

namespace Terminals
{
    internal partial class MainForm : Form
    {
        #region Declarations
        
        public delegate void ReleaseIsAvailable(FavoriteConfigurationElement ReleaseFavorite);
        public static event ReleaseIsAvailable OnReleaseIsAvailable;

        public const Int32 WM_LEAVING_FULLSCREEN = 0x4ff;
        private const String FULLSCREEN_ERROR_MSG = "Screen properties not available for RDP";
        
        private static TerminalsCA _commandLineArgs = new TerminalsCA();
        private static Boolean _releaseAvailable = false;
        private static MainForm _mainForm = null;

        private MethodInvoker _specialCommandsMIV;
        private MethodInvoker _resetMethodInvoker;
        private MethodInvoker _releaseMIV;
        private Point _lastLocation;
        private Size _lastSize;
        private FormWindowState _lastState;
        private FormSettings _formSettings;
        private Int32 _currentToolBarCount = 0;
        private FormWindowState _originalFormWindowState;
        private TabControlItem _currentToolTipItem = null;
        private ToolTip _currentToolTip = null;
        private Boolean _fullScreen;
        private Boolean _allScreens = false;
        private Boolean _stdToolbarState = true;
        private Boolean _specialToolbarState = true;
        private Boolean _favToolbarState = true;
        private TerminalTabsSelectionControler terminalsControler;
        private Int32 MouseBreakThreshold = 200;
        private FavoritesMenuLoader menuLoader;

        #endregion

        #region Protected overrides

        protected override void SetVisibleCore(Boolean value)
        {
            _formSettings.LoadFormSize();
            base.SetVisibleCore(value);
        }

        protected override void WndProc(ref Message msg)
        {
            try
            {
                if (msg.Msg == 0x21)  // mouse click
                {
                    TerminalTabControlItem selectedTab = this.terminalsControler.Selected;
                    if (selectedTab != null)
                    {
                        Rectangle r = selectedTab.RectangleToScreen(selectedTab.ClientRectangle);
                        if (r.Contains(Control.MousePosition))
                        {
                            SetGrabInput(true);
                        }
                        else
                        {
                            TabControlItem item = tcTerminals.GetTabItemByPoint(tcTerminals.PointToClient(Control.MousePosition));
                            if (item == null)
                                SetGrabInput(false);
                            else if (item == selectedTab)
                                SetGrabInput(true); //Grab input if clicking on currently selected tab
                        }
                    }
                    else
                    {
                        SetGrabInput(false);
                    }
                } 
                else if (msg.Msg == WM_LEAVING_FULLSCREEN) 
                {
                    if (CurrentTerminal != null)
                    {
                        if (CurrentTerminal.ContainsFocus)
                            tscConnectTo.Focus();
                    }
                    else
                    {
                        this.BringToFront();
                    }
                }

                base.WndProc(ref msg);
            }
            catch (Exception e)
            {
                Logging.Log.Info("WnProc Failure", e);
            }
        }

        #endregion

        #region Constuctors

        public MainForm()
        {
            try
            {
                _mainForm = this;

                _specialCommandsMIV = new MethodInvoker(LoadSpecialCommands);
                _resetMethodInvoker = new MethodInvoker(LoadWindowState);

                ShowWizardAndReloadSpecialCommands();

                // Set default font type by Windows theme to use for all controls on form
                this.Font = SystemFonts.IconTitleFont;
                _formSettings = new FormSettings(this);
                
                InitializeComponent(); // main designer procedure

                ApplyControlsEnableAndVisibleState();
                ApplyTheme();

                this.terminalsControler = new TerminalTabsSelectionControler(this, this.tcTerminals);
                this.menuLoader = new FavoritesMenuLoader(this.favoritesToolStripMenuItem,
                    this.favoritesSeparator, this.tscConnectTo, this.serverToolStripMenuItem_Click,
                    this.favoriteToolBar, this.toolStripMenuItemShowHideFavoriteToolbar,
                    this.QuickContextMenu, this.QuickContextMenu_ItemClicked);

                this.LoadFavorites();
                this.LoadGroups();
                this.UpdateControls();
                this.LoadWindowState();
                this.UpdateCaptureButtonEnabled();
                this.CheckForMultiMonitorUse();

                this.tcTerminals.MouseDown += new MouseEventHandler(tcTerminals_MouseDown);
                this.tcTerminals.MouseUp += new MouseEventHandler(tcTerminals_MouseUp);
                this.tcTerminals.MouseClick += new MouseEventHandler(tcTerminals_MouseClick);
                this.QuickContextMenu.ItemClicked += new ToolStripItemClickedEventHandler(QuickContextMenu_ItemClicked);

                ProtocolHandler.Register();
                SingleInstanceApplication.NewInstanceMessage += new NewInstanceMessageEventHandler(SingleInstanceApplication_NewInstanceMessage);
            }
            catch (Exception exc)
            {
                Logging.Log.Error("Error loading the Main Form", exc);
            }
        }

        private void ApplyControlsEnableAndVisibleState()
        {
            this.MainWindowNotifyIcon.Visible = Settings.MinimizeToTray;
            this.lockToolbarsToolStripMenuItem.Checked = Settings.ToolbarsLocked;
            this.groupsToolStripMenuItem.Visible = Settings.EnableGroupsMenu;
            this.tsbTags.Checked = Settings.ShowFavoritePanel;
            this.pnlTagsFavorites.Width = 7;

            if (Settings.ToolbarsLocked)
                this.MainMenuStrip.GripStyle = ToolStripGripStyle.Hidden;
            else
                this.MainMenuStrip.GripStyle = ToolStripGripStyle.Visible;
        }

        private void ApplyTheme()
        {
            if (Settings.Office2007BlueFeel)
                ToolStripManager.Renderer = Office2007Renderer.GetRenderer(RenderColors.Blue);
            else if (Settings.Office2007BlackFeel)
                ToolStripManager.Renderer = Office2007Renderer.GetRenderer(RenderColors.Black);
            else
                ToolStripManager.Renderer = new ToolStripProfessionalRenderer();

            // Update the old treeview theme to the new theme from Win Vista and up
            NativeApi.SetWindowTheme(this.menuStrip.Handle, "Explorer", null);
        }

        private void ShowWizardAndReloadSpecialCommands()
        {
            if (Settings.ShowWizard)
            {
                //settings file doesnt exist, wizard!
                using (FirstRunWizard wzrd = new FirstRunWizard())
                    wzrd.ShowDialog(this);

                Settings.ShowWizard = false;
            }
            else
            {
                ThreadPool.QueueUserWorkItem(new WaitCallback(this.ReloadSpecialCommands), null);
            }
        }

        #endregion

        #region Properties
        
        private Boolean IsMouseDown { get; set; }
        private Point MouseDownLocation { get; set; }

        public static TerminalsCA CommandLineArgs
        {
            get
            {
                return _commandLineArgs;
            }

            set
            {
                _commandLineArgs = value;
            }
        }

        public bool FullScreen
        {
            get
            {
                return this._fullScreen;
            }

            set
            {
                if (value)
                    SaveWindowState(); //Save windows state before we do a fullscreen so we can restore it

                if (_fullScreen != value)
                    this.SetFullScreen(value);

                if (!_fullScreen)
                    this.ResetToolbars();
            }
        }

        public IConnection CurrentConnection
        {
            get
            {
                if (this.terminalsControler.HasSelected)
                    return this.terminalsControler.Selected.Connection;
                else
                    return null;
            }
        }

        public AxMsRdpClient6 CurrentTerminal
        {
            get
            {
                if (this.CurrentConnection != null)
                {
                    if (this.CurrentConnection is RDPConnection)
                        return (this.CurrentConnection as RDPConnection).AxMsRdpClient;
                }
                
                return null;
            }
        }

        public static Boolean ReleaseAvailable
        {
            get
            {
                return _releaseAvailable;
            }

            set
            {
                _releaseAvailable = value;
                if (_releaseAvailable)
                {
                    FavoriteConfigurationElement release = FavoritesFactory.GetOrCreateReleaseFavorite();

                    Thread.Sleep(5000);
                    if (OnReleaseIsAvailable != null)
                        OnReleaseIsAvailable(release);
                }
            }
        }

        public static RssItem ReleaseDescription { get; set; }

        #endregion

        #region Public methods

        private void LoadWindowState()
        {
            this.Text = Program.Info.AboutText.ToString();

            HideShowFavoritesPanel(Settings.ShowFavoritePanel);

            ToolStripSettings newSettings = Settings.ToolbarSettings;
            if (newSettings != null && newSettings.Count > 0)
            {
                ToolStripMenuItem menuItem = null;
                foreach (int rowIndex in newSettings.Keys)
                {
                    ToolStripSetting setting = newSettings[rowIndex];
                    menuItem = null;
                    ToolStrip strip = null;
                    if (setting.Name == toolbarStd.Name)
                    {
                        strip = toolbarStd;
                        menuItem = standardToolbarToolStripMenuItem;
                    }
                    else if (setting.Name == favoriteToolBar.Name)
                    {
                        strip = favoriteToolBar;
                        menuItem = toolStripMenuItemShowHideFavoriteToolbar;
                    }
                    else if (setting.Name == SpecialCommandsToolStrip.Name)
                    {
                        strip = SpecialCommandsToolStrip;
                        menuItem = shortcutsToolStripMenuItem;
                    }
                    else if (setting.Name == menuStrip.Name)
                    {
                        strip = menuStrip;
                    }
                    else if (setting.Name == tsRemoteToolbar.Name)
                    {
                        strip = tsRemoteToolbar;
                    }

                    if (menuItem != null)
                    {
                        menuItem.Checked = setting.Visible;
                    }

                    if (strip != null)
                    {
                        Point p = new Point(setting.Left, setting.Top);
                        switch (setting.Dock)
                        {
                            case "Top":
                                this.toolStripContainer.TopToolStripPanel.Join(strip, p);
                                break;
                            case "Left":
                                this.toolStripContainer.LeftToolStripPanel.Join(strip, p);
                                break;
                            case "Right":
                                this.toolStripContainer.RightToolStripPanel.Join(strip, p);
                                break;
                            case "Bottom":
                                this.toolStripContainer.BottomToolStripPanel.Join(strip, p);
                                break;
                        }

                        strip.Location = p;
                        strip.Visible = setting.Visible;
                        if (Settings.ToolbarsLocked)
                            strip.GripStyle = ToolStripGripStyle.Hidden;
                        else
                            strip.GripStyle = ToolStripGripStyle.Visible;
                    }
                }
            }
        }

        internal void UpdateControls()
        {
            tcTerminals.ShowToolTipOnTitle = Settings.ShowInformationToolTips;
            bool hasSelectedTerminal = this.terminalsControler.HasSelected;
            addTerminalToGroupToolStripMenuItem.Enabled = hasSelectedTerminal;
            tsbGrabInput.Enabled = hasSelectedTerminal;
            grabInputToolStripMenuItem.Enabled = hasSelectedTerminal;

            try
            {
                tsbGrabInput.Checked = tsbGrabInput.Enabled && (CurrentTerminal != null) && CurrentTerminal.FullScreen;
            }
            catch (Exception exc)
            {
                Logging.Log.Error(FULLSCREEN_ERROR_MSG, exc);
            }

            grabInputToolStripMenuItem.Checked = tsbGrabInput.Checked;
            tsbConnect.Enabled = (tscConnectTo.Text != String.Empty);
            tsbConnectToConsole.Enabled = (tscConnectTo.Text != String.Empty);
            saveTerminalsAsGroupToolStripMenuItem.Enabled = (tcTerminals.Items.Count > 0);

            this.TerminalServerMenuButton.Visible = false;
            vncActionButton.Visible = false;
            VMRCAdminSwitchButton.Visible = false;
            VMRCViewOnlyButton.Visible = false;

            if (CurrentConnection != null)
            {
                VMRCConnection vmrc = this.CurrentConnection as VMRCConnection;
                if (vmrc != null)
                {
                    VMRCAdminSwitchButton.Visible = true;
                    VMRCViewOnlyButton.Visible = true;
                }

                VNCConnection vnc = this.CurrentConnection as VNCConnection;
                if (vnc != null)
                {
                    vncActionButton.Visible = true;
                }

                this.TerminalServerMenuButton.Visible = this.CurrentConnection.IsTerminalServer;
            }
        }

        public static MainForm GetMainForm()
        {
            return _mainForm;
        }

        public String GetDesktopShare()
        {
            String desktopShare = this.terminalsControler.Selected.Favorite.DesktopShare;
            if (String.IsNullOrEmpty(desktopShare))
            {
                desktopShare = Settings.DefaultDesktopShare.Replace("%SERVER%", CurrentTerminal.Server)
                                                                    .Replace("%USER%", CurrentTerminal.UserName);
            }

            return desktopShare;
        }

        public void Connect(String connectionName, Boolean forceConsole, Boolean forceNewWindow, CredentialSet credential = null)
        {
            FavoriteConfigurationElement favorite = FavoritesFactory.GetFavoriteUpdatedCopy(connectionName, 
                forceConsole, forceNewWindow, credential);
            
            if (favorite == null)
                return;
            
            this.favsList1.RecordHistoryItem(connectionName);
            if (!this.Visible)
            {
                this.Show();
                if (WindowState == FormWindowState.Minimized)
                    NativeApi.ShowWindow(new HandleRef(this, this.Handle), 9);
                NativeApi.SetForegroundWindow(new HandleRef(this, this.Handle));
            }

            CreateTerminalTab(favorite);
        }

        public void ToggleGrabInput()
        {
            if (CurrentTerminal != null)
            {
                CurrentTerminal.FullScreen = !CurrentTerminal.FullScreen;
            }
        }

        public void OpenNetworkingTools(string Action, string Host)
        {
            TerminalTabControlItem terminalTabPage = new TerminalTabControlItem(Program.Resources.GetString("NetworkingTools"));
            try
            {
                terminalTabPage.AllowDrop = false;
                terminalTabPage.ToolTipText = Program.Resources.GetString("NetworkingTools");
                terminalTabPage.Favorite = null;
                terminalTabPage.DoubleClick += new EventHandler(terminalTabPage_DoubleClick);
                this.terminalsControler.AddAndSelect(terminalTabPage);
                tcTerminals_SelectedIndexChanged(this, EventArgs.Empty);
                NetworkingToolsConnection conn = new NetworkingToolsConnection();
                conn.TerminalTabPage = terminalTabPage;
                conn.ParentForm = this;
                conn.Connect();
                conn.BringToFront();
                conn.Update();
                UpdateControls();
                conn.Execute(Action, Host);
            }
            catch (Exception exc)
            {
                Logging.Log.Error("Open Networking Tools Failure", exc);
                this.terminalsControler.RemoveAndUnSelect(terminalTabPage);
                terminalTabPage.Dispose();
            }
        }

        public void ShowManageTerminalForm(FavoriteConfigurationElement favorite)
        {
            using (NewTerminalForm frmNewTerminal = new NewTerminalForm(favorite))
            {
                TerminalFormDialogResult result = frmNewTerminal.ShowDialog();

                if (result != TerminalFormDialogResult.Cancel)
                {
                    this.LoadFavorites();

                    if (result == TerminalFormDialogResult.SaveAndConnect)
                        this.CreateTerminalTab(frmNewTerminal.Favorite);
                }
            }
        }

        public void CreateTerminalTab(FavoriteConfigurationElement favorite)
        {
            this.CallExecuteBeforeConnectedFromSettings();
            this.CallExecuteFeforeConnectedFromFavorite(favorite);

            TerminalTabControlItem terminalTabPage = this.CreateTerminalTabPageByFavoriteName(favorite);
            this.TryConnectTabPage(favorite, terminalTabPage);
        }
                
        #endregion

        #region Private methods

        private void CheckForMultiMonitorUse()
        {
            if (Screen.AllScreens.Length > 1)
            {
                this.showInDualScreensToolStripMenuItem.Enabled = true;

                //Lazy check to see if we are using dual screens
                int w = this.Width / Screen.PrimaryScreen.Bounds.Width;
                if (w > 2)
                {
                    this._allScreens = true;
                    this.showInDualScreensToolStripMenuItem.Text = "Show in single screens";
                }
            }
            else
            {
                this.showInDualScreensToolStripMenuItem.ToolTipText = "You only have one screen";
                this.showInDualScreensToolStripMenuItem.Enabled = false;
            }
        }

        private void CloseTabControlItem()
        {
            this.Text = Program.Info.AboutText;
            if (this.tcTerminals.Items.Count == 0)
                this.FullScreen = false;
        }

        internal void RemoveTabPage(TabControlItem tabControlToRemove)
        {
            this.tcTerminals.RemoveTab(tabControlToRemove);
            this.CloseTabControlItem();
        }

        private void TryConnectTabPage(FavoriteConfigurationElement favorite, TerminalTabControlItem terminalTabPage)
        {
            try
            {
                this.AssignEventsToConnectionTab(favorite, terminalTabPage);
                IConnection conn = ConnectionManager.CreateConnection(favorite, terminalTabPage, this);
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

        private void AssignEventsToConnectionTab(FavoriteConfigurationElement favorite, TerminalTabControlItem terminalTabPage)
        {
            terminalTabPage.AllowDrop = true;
            terminalTabPage.DragOver += this.terminalTabPage_DragOver;
            terminalTabPage.DragEnter += new DragEventHandler(this.terminalTabPage_DragEnter);
            terminalTabPage.Resize += new EventHandler(terminalTabPage_Resize);
            terminalTabPage.ToolTipText = favorite.GetToolTipText();
            terminalTabPage.Favorite = favorite;
            terminalTabPage.DoubleClick += new EventHandler(this.terminalTabPage_DoubleClick);
            this.terminalsControler.AddAndSelect(terminalTabPage);
            this.UpdateControls();
        }

        private void UpdateConnectionTabPageByConnectionState(FavoriteConfigurationElement favorite, TerminalTabControlItem terminalTabPage, IConnection conn)
        {
            if (conn.Connect())
            {
                (conn as Control).BringToFront();
                (conn as Control).Update();
                this.UpdateControls();
                if (favorite.DesktopSize == DesktopSize.FullScreen)
                    this.FullScreen = true;

                Connection b = (conn as Connection);
                //b.OnTerminalServerStateDiscovery += new Connection.TerminalServerStateDiscovery(this.b_OnTerminalServerStateDiscovery);
                b.CheckForTerminalServer(favorite);
            }
            else
            {
                String msg = Program.Resources.GetString("SorryTerminalswasunabletoconnecttotheremotemachineTryagainorcheckthelogformoreinformation");
                MessageBox.Show(msg);
                this.terminalsControler.RemoveAndUnSelect(terminalTabPage);
            }
        }

        private TerminalTabControlItem CreateTerminalTabPageByFavoriteName(FavoriteConfigurationElement favorite)
        {
            String terminalTabTitle = favorite.Name;
            if (Settings.ShowUserNameInTitle)
            {
                terminalTabTitle += String.Format(" ({0})", Functions.UserDisplayName(favorite.DomainName, favorite.UserName));
            }

            return new TerminalTabControlItem(terminalTabTitle);
        }

        private void CallExecuteFeforeConnectedFromFavorite(FavoriteConfigurationElement favorite)
        {
            if (favorite.ExecuteBeforeConnect && !string.IsNullOrEmpty(favorite.ExecuteBeforeConnectCommand))
            {
                ProcessStartInfo processStartInfo = new ProcessStartInfo(favorite.ExecuteBeforeConnectCommand, favorite.ExecuteBeforeConnectArgs);
                processStartInfo.WorkingDirectory = favorite.ExecuteBeforeConnectInitialDirectory;
                Process process = Process.Start(processStartInfo);
                if (favorite.ExecuteBeforeConnectWaitForExit)
                {
                    process.WaitForExit();
                }
            }
        }

        private void CallExecuteBeforeConnectedFromSettings()
        {
            if (Settings.ExecuteBeforeConnect && !string.IsNullOrEmpty(Settings.ExecuteBeforeConnectCommand))
            {
                ProcessStartInfo processStartInfo = new ProcessStartInfo(Settings.ExecuteBeforeConnectCommand, Settings.ExecuteBeforeConnectArgs);
                processStartInfo.WorkingDirectory = Settings.ExecuteBeforeConnectInitialDirectory;
                Process process = Process.Start(processStartInfo);
                if (Settings.ExecuteBeforeConnectWaitForExit)
                {
                    process.WaitForExit();
                }
            }
        }

        private void DeleteFavorite(string name)
        {
            tscConnectTo.Items.Remove(name);
            Settings.DeleteFavorite(name);
            favoritesToolStripMenuItem.DropDownItems.RemoveByKey(name);
        }

        private void BuildTerminalServerButtonMenu()
        {
            TerminalServerMenuButton.DropDownItems.Clear();

            if (this.CurrentConnection != null && this.CurrentConnection.IsTerminalServer)
            {
                ToolStripMenuItem Sessions = new ToolStripMenuItem(Program.Resources.GetString("Sessions"));
                Sessions.Tag = this.CurrentConnection.Server;
                TerminalServerMenuButton.DropDownItems.Add(Sessions);
                ToolStripMenuItem Svr = new ToolStripMenuItem(Program.Resources.GetString("Server"));
                Svr.Tag = this.CurrentConnection.Server;
                TerminalServerMenuButton.DropDownItems.Add(Svr);
                ToolStripMenuItem sd = new ToolStripMenuItem(Program.Resources.GetString("Shutdown"));
                sd.Click += new EventHandler(sd_Click);
                sd.Tag = this.CurrentConnection.Server;
                Svr.DropDownItems.Add(sd);
                ToolStripMenuItem rb = new ToolStripMenuItem(Program.Resources.GetString("Reboot"));
                rb.Click += new EventHandler(sd_Click);
                rb.Tag = this.CurrentConnection.Server;
                Svr.DropDownItems.Add(rb);


                if (this.CurrentConnection.Server.Sessions != null)
                {
                    foreach (TerminalServices.Session session in this.CurrentConnection.Server.Sessions)
                    {
                        if (session.Client.ClientName != "")
                        {
                            ToolStripMenuItem sess = new ToolStripMenuItem(String.Format("{1} - {2} ({0})", session.State.ToString().Replace("WTS", ""), session.Client.ClientName, session.Client.UserName));
                            sess.Tag = session;
                            Sessions.DropDownItems.Add(sess);
                            ToolStripMenuItem msg = new ToolStripMenuItem(Program.Resources.GetString("Send Message"));
                            msg.Click += new EventHandler(sd_Click);
                            msg.Tag = session;
                            sess.DropDownItems.Add(msg);

                            ToolStripMenuItem lo = new ToolStripMenuItem(Program.Resources.GetString("Logoff"));
                            lo.Click += new EventHandler(sd_Click);
                            lo.Tag = session;
                            sess.DropDownItems.Add(lo);

                            if (session.IsTheActiveSession)
                            {
                                ToolStripMenuItem lo1 = new ToolStripMenuItem(Program.Resources.GetString("Logoff"));
                                lo1.Click += new EventHandler(sd_Click);
                                lo1.Tag = session;
                                Svr.DropDownItems.Add(lo1);
                            }
                        }
                    }
                }
            }
            else
            {
                TerminalServerMenuButton.Visible = false;
            }
        }

        private void LoadSpecialCommands()
        {
            SpecialCommandsToolStrip.Items.Clear();

            foreach (SpecialCommandConfigurationElement cmd in Settings.SpecialCommands)
            {
                ToolStripMenuItem mi = new ToolStripMenuItem(cmd.Name);
                mi.DisplayStyle = ToolStripItemDisplayStyle.Image;
                mi.ToolTipText = cmd.Name;
                mi.Text = cmd.Name;
                mi.Tag = cmd;
                mi.Image = cmd.LoadThumbnail();
                mi.ImageTransparentColor = Color.Magenta;
                mi.Overflow = ToolStripItemOverflow.AsNeeded;
                SpecialCommandsToolStrip.Items.Add(mi);
            }
        }

        private void ShowCredentialsManager()
        {
            CredentialManager mgr = new CredentialManager();
            mgr.ShowDialog();
        }

        private void OpenSavedConnections()
        {
            foreach (string name in Settings.SavedConnections)
            {
                Connect(name, false, false);
            }

            Settings.ClearSavedConnectionsList();
        }

        private void SaveToolStripPanel(ToolStripPanel Panel, string Position, ToolStripSettings newSettings)
        {
            Int32 rowIndex = 0;
            foreach (ToolStripPanelRow row in Panel.Rows)
            {
                SaveToolStripRow(row, newSettings, Position, rowIndex);
                rowIndex++;
            }
        }

        private void SaveToolStripRow(ToolStripPanelRow Row, ToolStripSettings newSettings, string Position, int rowIndex)
        {
            foreach (ToolStrip strip in Row.Controls)
            {
                ToolStripSetting setting = new ToolStripSetting();
                setting.Dock = Position;
                setting.Row = rowIndex;
                setting.Left = strip.Left;
                setting.Top = strip.Top;
                setting.Name = strip.Name;
                setting.Visible = strip.Visible;
                newSettings.Add(_currentToolBarCount, setting);
                _currentToolBarCount++;
            }
        }

        private void SaveWindowState()
        {
            _currentToolBarCount = 0;
            if (!Settings.ToolbarsLocked)
            {
                ToolStripSettings newSettings = new ToolStripSettings();
                SaveToolStripPanel(this.toolStripContainer.TopToolStripPanel, "Top", newSettings);
                SaveToolStripPanel(this.toolStripContainer.LeftToolStripPanel, "Left", newSettings);
                SaveToolStripPanel(this.toolStripContainer.RightToolStripPanel, "Right", newSettings);
                SaveToolStripPanel(this.toolStripContainer.BottomToolStripPanel, "Bottom", newSettings);
                Settings.ToolbarSettings = newSettings;
            }
        }

        private void HideShowFavoritesPanel(bool Show)
        {
            if (Settings.EnableFavoritesPanel)
            {
                if (Show)
                {
                    splitContainer1.Panel1MinSize = 15;
                    splitContainer1.SplitterDistance = Settings.FavoritePanelWidth;
                    splitContainer1.Panel1Collapsed = false;
                    splitContainer1.IsSplitterFixed = false;
                    pnlHideTagsFavorites.Show();
                    pnlShowTagsFavorites.Hide();
                }
                else
                {
                    splitContainer1.Panel1MinSize = 9;
                    splitContainer1.SplitterDistance = 9;
                    splitContainer1.IsSplitterFixed = true;
                    pnlHideTagsFavorites.Hide();
                    pnlShowTagsFavorites.Show();
                }

                Settings.ShowFavoritePanel = Show;
                tsbTags.Checked = Show;
            }
            else
            {
                //just hide it completely
                splitContainer1.Panel1Collapsed = true;
                splitContainer1.Panel1MinSize = 0;
                splitContainer1.SplitterDistance = 0;
            }
        }

        private void ResetToolbars()
        {
            ThreadPool.QueueUserWorkItem(new WaitCallback(ToolbarResetThread));
        }

        private void ToolbarResetThread(object state)
        {
            this.Invoke(_resetMethodInvoker);
        }

        public void LoadFavorites()
        {
            Debug.WriteLine(DateTime.Now.ToString("hh:mm:ss,fff") + ": Load tree");
            this.favsList1.LoadFavs();
            menuLoader.FillMenu();
        }

        private void LoadGroups()
        {
            GroupConfigurationElementCollection serversGroups = Settings.GetGroups();
            Int32 seperatorIndex = groupsToolStripMenuItem.DropDownItems.IndexOf(groupsSeparator);
            for (Int32 i = groupsToolStripMenuItem.DropDownItems.Count - 1; i > seperatorIndex; i--)
            {
                groupsToolStripMenuItem.DropDownItems.RemoveAt(i);
            }

            addTerminalToGroupToolStripMenuItem.DropDownItems.Clear();
            foreach (GroupConfigurationElement serversGroup in serversGroups)
            {
                AddGroup(serversGroup);
            }

            addTerminalToGroupToolStripMenuItem.Enabled = false;
            saveTerminalsAsGroupToolStripMenuItem.Enabled = false;
        }

        private void AddGroup(GroupConfigurationElement group)
        {
            ToolStripMenuItem groupToolStripMenuItem = new ToolStripMenuItem(group.Name);
            groupToolStripMenuItem.Name = group.Name;
            groupToolStripMenuItem.Click += new EventHandler(groupToolStripMenuItem_Click);
            groupsToolStripMenuItem.DropDownItems.Add(groupToolStripMenuItem);
            ToolStripMenuItem groupAddToolStripMenuItem = new ToolStripMenuItem(group.Name);
            groupAddToolStripMenuItem.Name = group.Name;
            groupAddToolStripMenuItem.Click += new EventHandler(groupAddToolStripMenuItem_Click);
            addTerminalToGroupToolStripMenuItem.DropDownItems.Add(groupAddToolStripMenuItem);
        }

        private void SetGrabInput(Boolean grab)
        {
            if (CurrentTerminal != null)
            {
                if (grab && !CurrentTerminal.ContainsFocus)
                    CurrentTerminal.Focus();

                try
                {
                    CurrentTerminal.FullScreen = grab;
                }
                catch(Exception exc)
                {
                  Logging.Log.Error(FULLSCREEN_ERROR_MSG, exc);
                }
            }
        }

        private void CreateNewTerminal(String name = null)
        {
            using (NewTerminalForm frmNewTerminal = new NewTerminalForm(name))
            {
                TerminalFormDialogResult result = frmNewTerminal.ShowDialog();
                if (result != TerminalFormDialogResult.Cancel)
                {
                    this.LoadFavorites();
                    this.tscConnectTo.SelectedIndex = this.tscConnectTo.Items.IndexOf(frmNewTerminal.Favorite.Name);

                    if (result == TerminalFormDialogResult.SaveAndConnect)
                        this.CreateTerminalTab(frmNewTerminal.Favorite);
                }
            }
        }

        private void HideToolBar(Boolean fullScreen)
        {
            if (!fullScreen)
            {
                this.toolbarStd.Visible = _stdToolbarState;
                this.SpecialCommandsToolStrip.Visible = _specialToolbarState;
                this.favoriteToolBar.Visible = _favToolbarState;
            }
            else
            {
                this.toolbarStd.Visible = false;
                this.SpecialCommandsToolStrip.Visible = false;
                this.favoriteToolBar.Visible = false;
            }

        }

        private void SetFullScreen(Boolean fullScreen)
        {
            this.Visible = false;

            if (fullScreen)
            {
                this._stdToolbarState = this.toolbarStd.Visible;
                this._specialToolbarState = this.SpecialCommandsToolStrip.Visible;
                this._favToolbarState = this.favoriteToolBar.Visible;
            }

            this.HideToolBar(fullScreen);

            if (fullScreen)
            {
                this.menuStrip.Visible = false;
                this._lastLocation = this.Location;
                this._lastSize = this.RestoreBounds.Size;
                
                if (this.WindowState == FormWindowState.Minimized)
                    this._lastState = FormWindowState.Normal;
                else
                    this._lastState = this.WindowState;
                
                this.FormBorderStyle = FormBorderStyle.None;
                this.WindowState = FormWindowState.Normal;
                if (this._allScreens)
                {
                    Screen[] screenArr = Screen.AllScreens;
                    Int32 with = 0;
                    if (this._allScreens)
                    {
                        foreach (Screen screen in screenArr)
                        {
                            with += screen.Bounds.Width;
                        }
                    }

                    this.Width = with;
                    this.Location = new Point(0,0);
                }
                else
                {
                    this.Width = Screen.FromControl(tcTerminals).Bounds.Width;
                    this.Location = Screen.FromControl(tcTerminals).Bounds.Location;
                }

                this.Height = Screen.FromControl(tcTerminals).Bounds.Height;
                this.SetGrabInput(true);
                this.BringToFront();
            }
            else
            {
                this.TopMost = false;
                this.FormBorderStyle = FormBorderStyle.Sizable;
                this.WindowState = this._lastState;
                if (this._lastState != FormWindowState.Minimized)
                {
                    if (_lastState == FormWindowState.Normal)
                        this.Location = this._lastLocation;

                    this.Size = this._lastSize;
                }

                this.menuStrip.Visible = true;
            }
            
            this._fullScreen = fullScreen;

            this.tcTerminals.ShowTabs = !fullScreen;
            this.tcTerminals.ShowBorder = !fullScreen;

            this.Visible = true;
            this.PerformLayout();
        }

        private void QuickConnect(String server, Int32 port, Boolean ConnectToConsole)
        {
            FavoriteConfigurationElement favorite = FavoritesFactory
                .GetOrCreateQuickConnectFavorite(server, ConnectToConsole, port);

            this.CreateTerminalTab(favorite);
        }

        private void HandleCommandLineActions()
        {
            Boolean ConnectToConsole = _commandLineArgs.console;
            this.FullScreen = _commandLineArgs.fullscreen;
            if (!String.IsNullOrEmpty(_commandLineArgs.url))
            {
                String server; 
                Int32 port;
                ProtocolHandler.Parse(_commandLineArgs.url, out server, out port);
                QuickConnect(server, port, ConnectToConsole);
            }

            if (!String.IsNullOrEmpty(_commandLineArgs.machine))
            {
                String server = String.Empty;
                Int32 port = 3389;

                server = _commandLineArgs.machine;
                Int32 index = _commandLineArgs.machine.IndexOf(":");
                if (index > 0)
                {
                    server = _commandLineArgs.machine.Substring(0, index);
                    String p = _commandLineArgs.machine.Substring(index + 1);
                    if (!Int32.TryParse(p, out port))
                    {
                        port = 3389;
                    }
                }

                QuickConnect(server, port, ConnectToConsole);
            } 
            
            if (!String.IsNullOrEmpty(_commandLineArgs.favs))
            {
                String favs = _commandLineArgs.favs;
                if (favs.Contains(","))
                {
                    String[] favlist = favs.Split(',');
                    foreach (String fav in favlist)
                    {
                        this.Connect(fav, false, false);
                    }
                }
                else
                {
                    this.Connect(favs, false, false);
                }
            }
        }

        private void SaveActiveConnections()
        {
            List<String> activeConnections = new List<string>();
            foreach (TabControlItem item in tcTerminals.Items)
            {
                activeConnections.Add(item.Title);
            }

            Settings.CreateSavedConnectionsList(activeConnections.ToArray());
        }

        private void OpenReleasePage()
        {
            if (!Settings.NeverShowTerminalsWindow) 
                this.Connect(FavoritesFactory.TerminalsReleasesFavoriteName, false, false);
        }

        private void ReloadSpecialCommands(Object state)
        {
            while (!this.Created) // start creating icons after the form is created
            {
                Thread.Sleep(50);
            }

            this.rebuildShortcutsToolStripMenuItem_Click(null, null);
        }

        #endregion

        #region Mainform events

        private void MainForm_Load(object sender, EventArgs e)
        {
            this._releaseMIV = new MethodInvoker(this.OpenReleasePage);
            this.Text = Program.Info.AboutText;
            OnReleaseIsAvailable += new ReleaseIsAvailable(this.MainForm_OnReleaseIsAvailable);
        }

        private void MainForm_Shown(object sender, EventArgs e)
        {
            this.HandleCommandLineActions();
        }

        private void MainForm_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Cancel)
                this.ToggleGrabInput();
        }

        private void MainForm_KeyUp(object sender, KeyEventArgs e)
        {
            //handle global keyup events
            if (e.Control && e.KeyCode == Keys.F12)
            {
                CaptureManager.CaptureManager.PerformScreenCapture(this.tcTerminals);
                this.terminalsControler.RefreshCaptureManagerAndCreateItsTab(false);
            }
            else if (e.KeyCode == Keys.F4)
            {
                if (!this.tscConnectTo.Focused)
                    this.tscConnectTo.Focus();
            }
        }

        private void MainForm_Activated(object sender, EventArgs e)
        {
            _formSettings.EnsureVisibleScreenArrea();

            if (this.FullScreen)
                this.tcTerminals.ShowTabs = false;
        }

        private void MainForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (this.FullScreen)
                this.FullScreen = false;

            this.MainWindowNotifyIcon.Visible = false;

            if (this.tcTerminals.Items.Count > 0)
            {
                if (Settings.ShowConfirmDialog)
                {
                    SaveActiveConnectionsForm frmSaveActiveConnections = new SaveActiveConnectionsForm();
                    if (frmSaveActiveConnections.ShowDialog() == DialogResult.OK)
                    {
                        Settings.ShowConfirmDialog = !frmSaveActiveConnections.chkDontShowDialog.Checked;
                        if (frmSaveActiveConnections.chkOpenOnNextTime.Checked)
                            this.SaveActiveConnections();

                        e.Cancel = false;
                    }
                    else
                    {
                        e.Cancel = true;
                    }
                }
                else if (Settings.SaveConnectionsOnClose)
                {
                    this.SaveActiveConnections();
                }
            }

            this.SaveWindowState();
        }

        private void MainForm_Resize(object sender, EventArgs e)
        {
            if (this.WindowState == FormWindowState.Minimized)
            {
                if (Settings.MinimizeToTray) this.Visible = false;
            }
            else
            {
                this._originalFormWindowState = this.WindowState;
            }
        }

        #endregion

        #region Private events
        
        private void tcTerminals_MouseUp(object sender, MouseEventArgs e)
        {
            Cursor = Cursors.Default;
            this.IsMouseDown = false;
            Int32 mouseLeft = MousePosition.X;
            Int32 downLeft = MouseDownLocation.X;

            Int32 mouseTop = MousePosition.Y;
            Int32 downTop = MouseDownLocation.Y;

            if ((Math.Abs(mouseLeft - downLeft) >= MouseBreakThreshold) || (Math.Abs(mouseTop - downTop) >= MouseBreakThreshold))
            {
                this.terminalsControler.DetachTabToNewWindow();
            }
        }

        private void tcTerminals_MouseDown(object sender, MouseEventArgs e)
        {
            MouseDownLocation = MousePosition;
            // TODO: only show arrow when mousedown over connection tab and mouse is moving
            // Maybe also use another cursor?
            // Cursor = Cursors.UpArrow; 
            this.IsMouseDown = true;
        }

        private void tcTerminals_MouseClick(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                menuLoader.FillTrayContextMenu(this.FullScreen);

                if (tcTerminals != null && sender != null)
                    this.QuickContextMenu.Show(tcTerminals, e.Location);
            }
        }

        private void QuickContextMenu_Opening(object sender, CancelEventArgs e)
        {
            if (this.QuickContextMenu.Items.Count <= 0)
            {
                tcTerminals_MouseClick(null, new MouseEventArgs(MouseButtons.Right, 1, 0, 0, 0));
                e.Cancel = false;
            }
        }

        private void QuickContextMenu_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {
            if (e.ClickedItem.Text == Program.Resources.GetString("Restore") ||
                e.ClickedItem.Text == Program.Resources.GetString("RestoreScreen") ||
                e.ClickedItem.Text == Program.Resources.GetString("FullScreen"))
            {
                this.FullScreen = !this.FullScreen;
            }
            else if (e.ClickedItem.Text == Program.Resources.GetString("CredentialsManager"))
            {
                this.ShowCredentialsManager();
            }
            else if (e.ClickedItem.Text == Program.Resources.GetString("OrganizeFavorites"))
            {
                this.manageConnectionsToolStripMenuItem_Click(null, null);
            }
            else if (e.ClickedItem.Text == Program.Resources.GetString("Options"))
            {
                this.optionsToolStripMenuItem_Click(null, null);
            }
            else if (e.ClickedItem.Text == Program.Resources.GetString("NetworkingTools"))
            {
                this.toolStripButton2_Click(null, null);
            }
            else if (e.ClickedItem.Text == Program.Resources.GetString("ScreenCaptureManager"))
            {
                this.terminalsControler.RefreshCaptureManagerAndCreateItsTab(true);
            }
            else if (e.ClickedItem.Text == Program.Resources.GetString("Exit"))
            {
                this.Close();
            }
            else if (e.ClickedItem.Text == Program.Resources.GetString("ShowMenu"))
            {
                Boolean visible = !this.menuStrip.Visible;
                this.menuStrip.Visible = visible;
                this.menubarToolStripMenuItem.Checked = visible;
            }
            else if (e.ClickedItem.Text == Program.Resources.GetString("SpecialCommands"))
            {
                return;
            }

            else
            {
                OnFavoriteTrayToolsStripClick(e);
            }

            this.QuickContextMenu.Hide();
        }

        private void OnFavoriteTrayToolsStripClick(ToolStripItemClickedEventArgs e)
        {
            String tag = (e.ClickedItem.Tag as String);

            if (tag != null)
            {
                String itemName = e.ClickedItem.Text;
                if (tag == "favorite")
                    this.Connect(itemName, false, false);

                if (tag == "tag")
                {
                    ToolStripMenuItem parent = e.ClickedItem as ToolStripMenuItem;
                    ConnectToAllFavoritesUnderTag(parent);
                }
            }
        }

        private void ConnectToAllFavoritesUnderTag(ToolStripMenuItem parent)
        {
            if (parent.DropDownItems.Count > 0)
            {
                DialogResult result = this.AskUserIfWantsConnectToAll(parent);
                if (result == DialogResult.OK)
                {
                    foreach (ToolStripMenuItem button in parent.DropDownItems)
                    {
                        this.Connect(button.Text, false, false);
                    }
                }
            }
        }

        private DialogResult AskUserIfWantsConnectToAll(ToolStripMenuItem parent)
        {
            String localizedMessageFormat = Program.Resources.GetString("Areyousureyouwanttoconnecttoalltheseterminals");
            String message = String.Format(localizedMessageFormat, parent.DropDownItems.Count);
            String confirmatioTitle = Program.Resources.GetString(Program.Resources.GetString("Confirmation"));
            return MessageBox.Show(message, confirmatioTitle, MessageBoxButtons.OKCancel);
        }

        private void SingleInstanceApplication_NewInstanceMessage(object sender, object message)
        {
            if (this.WindowState == FormWindowState.Minimized)
                NativeApi.ShowWindow(new HandleRef(this, this.Handle), 9);
            NativeApi.SetForegroundWindow(new HandleRef(this, this.Handle));
            this.Activate();
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void groupAddToolStripMenuItem_Click(object sender, EventArgs e)
        {
            GroupConfigurationElement group = Settings.GetGroups()[((ToolStripMenuItem)sender).Text];
          String selectedTitle = this.terminalsControler.Selected.Title;
            group.FavoriteAliases.Add(new FavoriteAliasConfigurationElement(selectedTitle));
            Settings.DeleteGroup(group.Name);
            Settings.AddGroup(group);
        }

        private void groupToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FavoriteConfigurationElementCollection favorites = Settings.GetFavorites();
            GroupConfigurationElement serversGroup = Settings.GetGroups()[((ToolStripItem)(sender)).Text];
            foreach (FavoriteAliasConfigurationElement favoriteAlias in serversGroup.FavoriteAliases)
            {
                FavoriteConfigurationElement favorite = favorites[favoriteAlias.Name];
                this.CreateTerminalTab(favorite);
            }
        }

        private void serverToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FavoriteConfigurationElement favorite = Settings.GetFavorites()[((ToolStripItem)(sender)).Text];
            this.CreateTerminalTab(favorite);
        }

        private void sd_Click(object sender, EventArgs e)
        {
            ToolStripMenuItem menu = (sender as ToolStripMenuItem);
            if (menu != null)
            {
                if (menu.Text == Program.Resources.GetString("Shutdown"))
                {
                    TerminalServices.TerminalServer server = (menu.Tag as TerminalServices.TerminalServer);
                    if (server != null && MessageBox.Show(Program.Resources.GetString("Areyousureyouwanttoshutthismachineoff"), Program.Resources.GetString("Confirmation"), MessageBoxButtons.OKCancel) == DialogResult.OK)
                        TerminalServices.TerminalServicesAPI.ShutdownSystem(server, false);
                }
                else if (menu.Text == Program.Resources.GetString("Reboot"))
                {
                    TerminalServices.TerminalServer server = (menu.Tag as TerminalServices.TerminalServer);
                    if (server != null && MessageBox.Show(Program.Resources.GetString("Areyousureyouwanttorebootthismachine"), Program.Resources.GetString("Confirmation"), MessageBoxButtons.OKCancel) == DialogResult.OK)
                        TerminalServices.TerminalServicesAPI.ShutdownSystem(server, true);
                }
                else if (menu.Text == Program.Resources.GetString("Logoff"))
                {
                    TerminalServices.Session session = (menu.Tag as TerminalServices.Session);
                    if (session != null && MessageBox.Show(Program.Resources.GetString("Areyousureyouwanttologthissessionoff"), Program.Resources.GetString("Confirmation"), MessageBoxButtons.OKCancel) == DialogResult.OK)
                        TerminalServices.TerminalServicesAPI.LogOffSession(session, false);
                }
                else if (menu.Text == Program.Resources.GetString("Send Message"))
                {
                    TerminalServices.Session session = (menu.Tag as TerminalServices.Session);
                    if (session != null)
                    {
                        InputBoxResult result = InputBox.Show(Program.Resources.GetString("Pleaseenterthemessagetosend"));
                        if (result.ReturnCode == DialogResult.OK && result.Text.Trim() != null)
                        {
                            TerminalServices.TerminalServicesAPI.SendMessage(session, Program.Resources.GetString("MessagefromyourAdministrator"), result.Text.Trim(), 0, 10, false);
                        }
                    }
                }
            }
        }

        private void terminalTabPage_Resize(object sender, EventArgs e)
        {
            TerminalTabControlItem terminalTabControlItem = sender as TerminalTabControlItem;
            RDPConnection rdpConnection = terminalTabControlItem.Connection as RDPConnection;
            if (rdpConnection != null &&
                !rdpConnection.AxMsRdpClient.AdvancedSettings3.SmartSizing)
            {
                //rdpConnection.AxMsRdpClient.DesktopWidth = terminalTabControlItem.Width;
                //rdpConnection.AxMsRdpClient.DesktopHeight = terminalTabControlItem.Height;
                //Debug.WriteLine("Tab size:" + terminalTabControlItem.Size.ToString() + ";" +
                //  rdpConnection.AxMsRdpClient.DesktopHeight.ToString() + "," +
                //  rdpConnection.AxMsRdpClient.DesktopWidth.ToString());
            }
        }

        private void terminalTabPage_DragEnter(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.FileDrop, false))
            {
                e.Effect = DragDropEffects.Copy;
            }
            else
            {
                e.Effect = DragDropEffects.None;
            }
        }

        private void terminalTabPage_DragOver(object sender, DragEventArgs e)
        {
            this.terminalsControler.Select(sender as TerminalTabControlItem);
        }

        internal void terminalTabPage_DoubleClick(object sender, EventArgs e)
        {
            if (this.terminalsControler.HasSelected)
            {
                this.tsbDisconnect.PerformClick();
            }
        }

        private void newTerminalToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.CreateNewTerminal();
        }

        private void tsbConnect_Click(object sender, EventArgs e)
        {
            string connectionName = this.tscConnectTo.Text;
            if (connectionName != String.Empty)
            {
                this.Connect(connectionName, false, false);
            }
        }

        private void tsbConnectToConsole_Click(object sender, EventArgs e)
        {
            string connectionName = tscConnectTo.Text;
            if (connectionName != "")
            {
                Connect(connectionName, true, false);
            }
        }

        private void tscConnectTo_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                this.tsbConnect.PerformClick();
            }
            if (e.KeyCode == Keys.Delete && this.tscConnectTo.DroppedDown &&
                this.tscConnectTo.SelectedIndex != -1)
            {
                String connectionName = tscConnectTo.Items[tscConnectTo.SelectedIndex].ToString();
                this.DeleteFavorite(connectionName);
            }
        }

        private void tsbDisconnect_Click(object sender, EventArgs e)
        {
            try
            {
              TerminalTabControlItem tabToClose = this.terminalsControler.Selected;
              if(this.tcTerminals.Items.Contains(tabToClose))
                this.tcTerminals.CloseTab(tabToClose);
            }
            catch (Exception exc)
            {
                Logging.Log.Error("Disconnecting a tab threw an exception", exc);
            }
        }

        private void tcTerminals_SelectedIndexChanged(object sender, EventArgs e)
        {
            this.UpdateControls();
        }

        private void newTerminalToolStripMenuItem_Click_1(object sender, EventArgs e)
        {
            this.CreateNewTerminal();
        }

        private void tsbGrabInput_Click(object sender, EventArgs e)
        {
            this.ToggleGrabInput();
        }

        private void tcTerminals_TabControlItemClosing(TabControlItemClosingEventArgs e)
        {
            Boolean cancel = false;
            if (this.CurrentConnection != null && this.CurrentConnection.Connected)
            {
                Boolean close = false;
                if (Settings.WarnOnConnectionClose)
                {
                    close = (MessageBox.Show(this, Program.Resources.GetString("Areyousurethatyouwanttodisconnectfromtheactiveterminal"),
                    Program.Resources.GetString("Terminals"), MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes);
                }
                else
                {
                    close = true;
                }

                if (close)
                {
                    if (CurrentTerminal != null)
                        CurrentTerminal.Disconnect();

                    if (CurrentConnection != null)
                    {
                        CurrentConnection.Disconnect();
                        // Close tabitem functions handled under each connection disconnect methods.
                        cancel = true;
                    }

                    this.Text = Program.Info.AboutText;
                }
                else
                {
                    cancel = true;
                }
            }

            e.Cancel = cancel;
        }

        private void tcTerminals_TabControlItemSelectionChanged(TabControlItemChangedEventArgs e)
        {
            this.UpdateControls();
            if (this.tcTerminals.Items.Count > 0)
            {
                this.tsbDisconnect.Enabled = e.Item != null;
                this.disconnectToolStripMenuItem.Enabled = e.Item != null;
                this.SetGrabInput(true);

                if (e.Item.Selected && Settings.ShowInformationToolTips)
                    this.Text = e.Item.ToolTipText.Replace("\r\n", "; ");
            }
        }

        private void tscConnectTo_TextChanged(object sender, EventArgs e)
        {
            this.UpdateControls();
        }

        private void tcTerminals_MouseHover(object sender, EventArgs e)
        {
            if (this.tcTerminals != null && !this.tcTerminals.ShowTabs)
                this.timerHover.Enabled = true;
        }

        private void tcTerminals_MouseLeave(object sender, EventArgs e)
        {
            this.timerHover.Enabled = false;
            if (this.FullScreen && this.tcTerminals.ShowTabs && !this.tcTerminals.MenuOpen)
                this.tcTerminals.ShowTabs = false;

            if (this._currentToolTipItem != null)
            {
                this._currentToolTip.Hide(this._currentToolTipItem);
                this._currentToolTip.Active = false;
            }
        }

        private void tcTerminals_TabControlItemClosed(object sender, EventArgs e)
        {
          CloseTabControlItem();
        }

        private void tcTerminals_DoubleClick(object sender, EventArgs e)
        {
            this.FullScreen = !this._fullScreen;
        }

        private void tsbFullScreen_Click(object sender, EventArgs e)
        {
            this.FullScreen = !this.FullScreen;
            this.UpdateControls();
        }

        private void tcTerminals_MenuItemsLoaded(object sender, EventArgs e)
        {
            foreach (ToolStripItem item in this.tcTerminals.Menu.Items)
            {
                item.Image = Resources.smallterm;
            }

            if (this.FullScreen)
            {
                ToolStripSeparator sep = new ToolStripSeparator();
                this.tcTerminals.Menu.Items.Add(sep);
                ToolStripMenuItem item = new ToolStripMenuItem(Program.Resources.GetString("Restore"), null, this.tcTerminals_DoubleClick);
                this.tcTerminals.Menu.Items.Add(item);
                item = new ToolStripMenuItem(Program.Resources.GetString("Minimize"), null, this.Minimize);
                this.tcTerminals.Menu.Items.Add(item);
            }
        }

        private void manageConnectionsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OrganizeFavoritesForm conMgr = new OrganizeFavoritesForm();
            conMgr.ShowDialog();
            LoadFavorites();
        }

        private void saveTerminalsAsGroupToolStripMenuItem_Click(object sender, EventArgs e)
        {
            using (NewGroupForm frmNewGroup = new NewGroupForm())
            {
                if (frmNewGroup.ShowDialog() == DialogResult.OK)
                {
                    GroupConfigurationElement serversGroup = new GroupConfigurationElement();
                    serversGroup.Name = frmNewGroup.txtGroupName.Text;
                    foreach (TabControlItem tabControlItem in this.tcTerminals.Items)
                    {
                        serversGroup.FavoriteAliases.Add(new FavoriteAliasConfigurationElement(tabControlItem.Title));
                    }

                    Settings.AddGroup(serversGroup);
                    this.LoadGroups();
                }
            }
        }

        private void organizeGroupsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            using (OrganizeGroupsForm frmOrganizeGroups = new OrganizeGroupsForm())
            {
                frmOrganizeGroups.ShowDialog();
                this.LoadGroups();
            }
        }

        private void tcTerminals_TabControlMouseOnTitle(TabControlMouseOnTitleEventArgs e)
        {
            if (Settings.ShowInformationToolTips)
            {
                if (this._currentToolTip == null)
                {
                    this._currentToolTip = new ToolTip();
                    this._currentToolTip.Active = false;
                }
                else if ((this._currentToolTipItem != null) && (this._currentToolTipItem != e.Item))
                {
                    this._currentToolTip.Hide(this._currentToolTipItem);
                    this._currentToolTip.Active = false;
                }

                if (!this._currentToolTip.Active)
                {
                    this._currentToolTip = new ToolTip();
                    this._currentToolTip.ToolTipTitle = Program.Resources.GetString("ConnectionInformation");
                    this._currentToolTip.ToolTipIcon = ToolTipIcon.Info;
                    this._currentToolTip.UseFading = true;
                    this._currentToolTip.UseAnimation = true;
                    this._currentToolTip.IsBalloon = false;
                    this._currentToolTip.Show(e.Item.ToolTipText, e.Item, (int)e.Item.StripRect.X, 2);
                    this._currentToolTipItem = e.Item;
                    this._currentToolTip.Active = true;
                }
            }
        }

        private void tcTerminals_TabControlMouseLeftTitle(TabControlMouseOnTitleEventArgs e)
        {
            if (this._currentToolTipItem != null)
            {
                this._currentToolTip.Hide(this._currentToolTipItem);
                this._currentToolTip.Active = false;
            }
            /*if (previewPictureBox != null)
            {
                previewPictureBox.Image.Dispose();
                previewPictureBox.Dispose();
                previewPictureBox = null;
            }*/
        }

        private void timerHover_Tick(object sender, EventArgs e)
        {
            if (this.timerHover.Enabled)
            {
                this.timerHover.Enabled = false;
                this.tcTerminals.ShowTabs = true;
            }
        }

        private void organizeFavoritesToolbarToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OrganizeFavoritesToolbarForm frmOrganizeFavoritesToolbar = new OrganizeFavoritesToolbarForm();
            frmOrganizeFavoritesToolbar.ShowDialog();
            //todo FavoritesMenuLoader.LoadFavoritesToolbar();
        }

        private void optionsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            using (OptionDialog frmOptions = new OptionDialog(CurrentTerminal))
            {
                if (frmOptions.ShowDialog() == DialogResult.OK)
                {
                    this.groupsToolStripMenuItem.Visible = Settings.EnableGroupsMenu;
                    this.HideShowFavoritesPanel(Settings.ShowFavoritePanel);
                    this.MainWindowNotifyIcon.Visible = Settings.MinimizeToTray;
                    if (!Settings.MinimizeToTray && !this.Visible)
                        this.Visible = true;

                    if (Settings.Office2007BlueFeel)
                    {
                        ToolStripManager.Renderer = Office2007Renderer.GetRenderer(RenderColors.Blue);
                    }
                    else
                    {
                        if (Settings.Office2007BlackFeel)
                            ToolStripManager.Renderer = Office2007Renderer.GetRenderer(RenderColors.Black);
                        else
                            ToolStripManager.Renderer = new ToolStripProfessionalRenderer();
                    }

                    this.tcTerminals.ShowToolTipOnTitle = Settings.ShowInformationToolTips;
                    if (this.terminalsControler.HasSelected)
                        this.terminalsControler.Selected.ToolTipText = this.terminalsControler.Selected.Favorite.GetToolTipText();

                    UpdateCaptureButtonEnabled();
                }
            }
        }

        /// <summary>
        /// Disable capture button when function is disabled in options
        /// </summary>
        private void UpdateCaptureButtonEnabled()
        {
            Boolean enableCapture = Settings.EnabledCaptureToFolderAndClipBoard;
            this.CaptureScreenToolStripButton.Enabled = enableCapture;
            this.captureTerminalScreenToolStripMenuItem.Enabled = enableCapture;
            this.terminalsControler.UpdateCaptureButtonOnDetachedPopUps();
        }

        private void aboutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            using (AboutForm frmAbout = new AboutForm())
            {
                frmAbout.ShowDialog();
            }
        }

        private void tsbTags_Click(object sender, EventArgs e)
        {
            this.HideShowFavoritesPanel(this.tsbTags.Checked);
        }

        private void pbShowTags_Click(object sender, EventArgs e)
        {
            this.HideShowFavoritesPanel(true);
        }

        private void pbHideTags_Click(object sender, EventArgs e)
        {
            this.HideShowFavoritesPanel(false);
        }

        private void tsbFavorites_Click(object sender, EventArgs e)
        {
            Settings.EnableFavoritesPanel = this.tsbFavorites.Checked;
            this.HideShowFavoritesPanel(Settings.ShowFavoritePanel);
        }

        private void Minimize(object sender, EventArgs e)
        {
            this._originalFormWindowState = this.WindowState;
            this.WindowState = FormWindowState.Minimized;
        }

        private void MainWindowNotifyIcon_MouseClick(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                if (Settings.MinimizeToTray)
                {
                    this.Visible = !this.Visible;
                    if (this.Visible && this.WindowState == FormWindowState.Minimized)
                        this.WindowState = _originalFormWindowState;
                }
                else
                {
                    if (this.WindowState == FormWindowState.Normal)
                    {
                        this._originalFormWindowState = this.WindowState;
                        this.WindowState = FormWindowState.Minimized;
                    }
                    else
                    {
                        this.WindowState = _originalFormWindowState;
                    }
                }
            }
        }

        private void CaptureScreenToolStripButton_Click(object sender, EventArgs e)
        {
            CaptureManager.CaptureManager.PerformScreenCapture(this.tcTerminals);
            this.terminalsControler.RefreshCaptureManager(false);

            if (Settings.EnableCaptureToFolder && Settings.AutoSwitchOnCapture)
                this.terminalsControler.RefreshCaptureManagerAndCreateItsTab(false);
        }

        private void captureTerminalScreenToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.CaptureScreenToolStripButton_Click(null, null);
        }

        private void VMRCAdminSwitchButton_Click(object sender, EventArgs e)
        {
            if (this.CurrentConnection != null)
            {
                VMRCConnection vmrc = this.CurrentConnection as VMRCConnection;
                if (vmrc != null)
                {
                    vmrc.AdminDisplay();
                }
            }
        }

        private void VMRCViewOnlyButton_Click(object sender, EventArgs e)
        {
            if (this.CurrentConnection != null)
            {
                VMRCConnection vmrc = this.CurrentConnection as VMRCConnection;
                if (vmrc != null)
                {
                    vmrc.ViewOnlyMode = !vmrc.ViewOnlyMode;
                }
            }

            this.UpdateControls();
        }

        private void toolStripButton1_Click(object sender, EventArgs e)
        {
            Cursor = Cursors.WaitCursor;
            try
            {
                String sessionId = String.Empty;
                if (!this.CurrentTerminal.AdvancedSettings3.ConnectToServerConsole)
                {
                    sessionId = TSManager.GetCurrentSession(this.CurrentTerminal.Server,
                        this.CurrentTerminal.UserName, 
                        this.CurrentTerminal.Domain,
                        Environment.MachineName).Id.ToString();
                }

                Process process = new Process();
                String args = String.Format(" \\\\{0} -i {1} -d notepad", CurrentTerminal.Server, sessionId);
                ProcessStartInfo startInfo = new ProcessStartInfo(Settings.PsexecLocation, args);
                startInfo.UseShellExecute = false;
                startInfo.CreateNoWindow = true;
                process.StartInfo = startInfo;
                process.Start();
            }
            finally
            {
                Cursor = Cursors.Default;
            }
        }

        private void tsbCMD_Click(object sender, EventArgs e)
        {
            Cursor = Cursors.WaitCursor;
            try
            {
                String sessionId = String.Empty;
                if (!this.CurrentTerminal.AdvancedSettings3.ConnectToServerConsole)
                {
                    sessionId = TSManager.GetCurrentSession(this.CurrentTerminal.Server,
                        this.CurrentTerminal.UserName, 
                        this.CurrentTerminal.Domain,
                        Environment.MachineName).Id.ToString();
                }

                Process process = new Process();
                String args = String.Format(" \\\\{0} -i {1} -d cmd", CurrentTerminal.Server, sessionId);
                ProcessStartInfo startInfo = new ProcessStartInfo(Settings.PsexecLocation, args);
                startInfo.UseShellExecute = false;
                startInfo.CreateNoWindow = true;
                process.StartInfo = startInfo;
                process.Start();
            }
            finally
            {
                Cursor = Cursors.Default;
            }
        }

        private void standardToolbarToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.AddShowStrip(this.toolbarStd, this.standardToolbarToolStripMenuItem, !this.toolbarStd.Visible);
        }

        private void toolStripMenuItemShowHideFavoriteToolbar_Click(object sender, EventArgs e)
        {
            this.AddShowStrip(this.favoriteToolBar, this.toolStripMenuItemShowHideFavoriteToolbar, !this.favoriteToolBar.Visible);
        }

        private void shortcutsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.AddShowStrip(this.SpecialCommandsToolStrip, this.shortcutsToolStripMenuItem, !this.SpecialCommandsToolStrip.Visible);
        }

        private void menubarToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.AddShowStrip(this.menuStrip, this.menubarToolStripMenuItem, !this.menuStrip.Visible);
        }

        private void AddShowStrip(ToolStrip strip, ToolStripMenuItem menu, Boolean visible)
        {
            if (!Settings.ToolbarsLocked)
            {
                strip.Visible = visible;
                menu.Checked = visible;
            }
            else
            {
                MessageBox.Show(Program.Resources.GetString("Inordertochangethetoolbarsyoumustfirstunlockthem"));
            }
        }

        private void toolsToolStripMenuItem_DropDownOpening(object sender, EventArgs e)
        {
            this.shortcutsToolStripMenuItem.Checked = this.SpecialCommandsToolStrip.Visible;
            this.toolStripMenuItemShowHideFavoriteToolbar.Checked = this.favoriteToolBar.Visible;
            this.standardToolbarToolStripMenuItem.Checked = this.toolbarStd.Visible;
        }

        private void toolStripMenuItem3_Click(object sender, EventArgs e)
        {
            using (OrganizeShortcuts org = new OrganizeShortcuts())
            {
                org.ShowDialog(this);
            }

            this.Invoke(this._specialCommandsMIV);
        }

        private void ShortcutsContextMenu_MouseClick(object sender, MouseEventArgs e)
        {
            this.toolStripMenuItem3_Click(null, null);
        }

        // todo assign missing SpecialCommandsToolStrip_MouseClick
        private void SpecialCommandsToolStrip_MouseClick(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
                this.ShortcutsContextMenu.Show(e.X, e.Y);
        }

        private void SpecialCommandsToolStrip_ItemClicked_1(object sender, ToolStripItemClickedEventArgs e)
        {
            SpecialCommandConfigurationElement elm = (e.ClickedItem.Tag as SpecialCommandConfigurationElement);
            if (elm != null) 
                elm.Launch();
        }

        private void toolStripButton2_Click(object sender, EventArgs e)
        {
            this.OpenNetworkingTools(null, null);
        }

        private void networkingToolsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.toolStripButton2_Click(null, null);
        }

        private void toolStripMenuItemCaptureManager_Click(object sender, EventArgs e)
        {
          this.terminalsControler.RefreshCaptureManagerAndCreateItsTab(true);
        }

        private void toolStripButtonCaptureManager_Click(object sender, EventArgs e)
        {
            Boolean origval = Settings.AutoSwitchOnCapture;
            if (!Settings.EnableCaptureToFolder || !Settings.AutoSwitchOnCapture)
            {
                Settings.AutoSwitchOnCapture = true;
            }

            this.terminalsControler.RefreshCaptureManagerAndCreateItsTab(true);
            Settings.AutoSwitchOnCapture = origval;
        }

        private void sendALTKeyToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (sender != null && (sender as ToolStripMenuItem) != null)
            {
                String key = (sender as ToolStripMenuItem).Text;
                VNCConnection vnc;
                if (this.CurrentConnection != null)
                {
                    vnc = (this.CurrentConnection as VNCConnection);
                    if (vnc != null)
                    {
                        if (key == sendALTF4KeyToolStripMenuItem.Text)
                        {
                            vnc.SendSpecialKeys(VncSharp.SpecialKeys.AltF4);
                        }
                        else if (key == sendALTKeyToolStripMenuItem.Text)
                        {
                            vnc.SendSpecialKeys(VncSharp.SpecialKeys.Alt);
                        }
                        else if (key == sendCTRLESCKeysToolStripMenuItem.Text)
                        {
                            vnc.SendSpecialKeys(VncSharp.SpecialKeys.CtrlEsc);
                        }
                        else if (key == sendCTRLKeyToolStripMenuItem.Text)
                        {
                            vnc.SendSpecialKeys(VncSharp.SpecialKeys.Ctrl);
                        }
                        else if (key == sentCTRLALTDELETEKeysToolStripMenuItem.Text)
                        {
                            vnc.SendSpecialKeys(VncSharp.SpecialKeys.CtrlAltDel);
                        }
                    }
                }
            }
        }

        private void toolStripButton4_Click(object sender, EventArgs e)
        {
            if (this.terminalsControler.HasSelected)
            {
                TerminalTabControlItem terminalTabPage = this.terminalsControler.Selected;
                if (terminalTabPage.Connection != null)
                {
                    terminalTabPage.Connection.ChangeDesktopSize(terminalTabPage.Connection.Favorite.DesktopSize);
                }
            }
        }

        private void pbShowTagsFavorites_MouseMove(object sender, MouseEventArgs e)
        {
            if (Settings.AutoExapandTagsPanel)
                this.HideShowFavoritesPanel(true);
        }

        private void TerminalServerMenuButton_DropDownOpening(object sender, EventArgs e)
        {
            this.BuildTerminalServerButtonMenu();
        }

        private void lockToolbarsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.SaveWindowState();
            this.lockToolbarsToolStripMenuItem.Checked = !this.lockToolbarsToolStripMenuItem.Checked;
            Settings.ToolbarsLocked = this.lockToolbarsToolStripMenuItem.Checked;

            Boolean GripVisible = !Settings.ToolbarsLocked;
            foreach (ToolStripPanelRow row in this.toolStripContainer.TopToolStripPanel.Rows)
            {
                foreach (Control c in row.Controls)
                {
                    ToolStrip item = (c as ToolStrip);
                    if (item != null)
                    {
                        if (GripVisible)
                            item.GripStyle = ToolStripGripStyle.Visible;
                        else
                            item.GripStyle = ToolStripGripStyle.Hidden;
                    }
                }
            }
        }

        private void MainForm_OnReleaseIsAvailable(FavoriteConfigurationElement ReleaseFavorite)
        {
            this.Invoke(this._releaseMIV);
        }

        private void helpToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (!this.updateToolStripItem.Visible)
            {
                if (ReleaseAvailable && this.updateToolStripItem != null)
                {
                    this.updateToolStripItem.Visible = ReleaseAvailable;
                    if (ReleaseDescription != null)
                    {
                        this.updateToolStripItem.Text = String.Format("{0} - {1}", this.updateToolStripItem.Text, ReleaseDescription.Title);
                    }
                }
            }
        }

        private void toolStripButton5_Click(object sender, EventArgs e)
        {
            Process.Start("mmc.exe", "compmgmt.msc /a /computer=.");
        }

        private void rebuildTagsIndexToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Settings.RebuildTagIndex();
            this.LoadFavorites();
            this.LoadGroups();
            this.UpdateControls();
        }

        private void viewInNewWindowToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (this.CurrentConnection != null)
            {
              this.terminalsControler.DetachTabToNewWindow();
            }
        }

        private void rebuildShortcutsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Settings.SpecialCommands.Clear();
            Settings.SpecialCommands = Wizard.SpecialCommandsWizard.LoadSpecialCommands();
            this.Invoke(this._specialCommandsMIV);
        }

        private void rebuildToolbarsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.LoadWindowState();
        }

        private void openLogFolderToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Process.Start(Path.Combine(Path.GetDirectoryName(Application.ExecutablePath),"Logs"));
        }

        private void splitContainer1_SplitterMoved(object sender, SplitterEventArgs e)
        {
            if (splitContainer1.Panel1.Width>15)
                Settings.FavoritePanelWidth = splitContainer1.Panel1.Width;            
        }

        private void credentialManagementToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.ShowCredentialsManager();
        }

        private void CredentialManagementToolStripButton_Click(object sender, EventArgs e)
        {
            this.ShowCredentialsManager();
        }        

        private void exportConnectionsListToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ExportFrom ei = new ExportFrom();
            ei.Show();
        }

        private void toolStripMenuItemImport_Click(object sender, EventArgs e)
        {
            OrganizeFavoritesForm conMgr = new OrganizeFavoritesForm();
            conMgr.CallImport();
            conMgr.ShowDialog();
            LoadFavorites();
        }

        private void showInDualScreensToolStripMenuItem_Click(object sender, EventArgs e)
        {                        
            Screen[] screenArr = Screen.AllScreens;
            Int32 with = 0;
            if (!this._allScreens)
            {
                if (this.WindowState == FormWindowState.Maximized)
                    this.WindowState = FormWindowState.Normal;

                foreach (Screen screen in screenArr)
                {
                    with += screen.Bounds.Width;
                }

                this.showInDualScreensToolStripMenuItem.Text = "Show in Single Screen";
                this.BringToFront();
            }
            else
            {
                with = Screen.PrimaryScreen.Bounds.Width;
                this.showInDualScreensToolStripMenuItem.Text = "Show In Multi Screens";
            }

            this.Top = 0;
            this.Left = 0;
            this.Height = Screen.PrimaryScreen.Bounds.Height;
            this.Width = with;
            this._allScreens = !this._allScreens;
        }

        #endregion
    }
}
