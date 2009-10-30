using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.IO;

using AxMSTSCLib;
using MSTSC = MSTSCLib;
using Terminals.Properties;
using Terminals.CommandLine;
using TabControl;
using Unified.Rss;

namespace Terminals {
    public partial class MainForm : Form
    {
        public delegate void ReleaseIsAvailable(FavoriteConfigurationElement ReleaseFavorite);
        public static event ReleaseIsAvailable OnReleaseIsAvailable;

        public const int WM_LEAVING_FULLSCREEN = 0x4ff;        

        private static TerminalsCA _commandLineArgs = new TerminalsCA();

        private static bool _releaseAvailable = false;
        private static string _terminalsReleasesFavoriteName = Program.Resources.GetString("TerminalsNews");
        private static RssItem _releaseDescription = null;                

        private MethodInvoker _specialCommandsMIV;
        private MethodInvoker _resetMethodInvoker;
        private MethodInvoker _releaseMIV;
        private Point _lastLocation;
        private Size _lastSize;
        private FormWindowState _lastState;
        private FormSettings _formSettings;
        private ImageFormatHandler _imageFormatHandler;
        private int _currentToolBarCount = 0;
        private FormWindowState _originalFormWindowState;
        private TabControlItem _currentToolTipItem = null;
        private ToolTip _currentToolTip = new ToolTip();
        private bool _fullScreen;
        private bool _allScreens = false;
        private bool _stdToolbarState = true;
        private bool _specialToolbarState = true;
        private bool _favToolbarState = true;
        private static MainForm _mainForm = null;

        #region protected
        protected override void SetVisibleCore(bool value)
        {
            _formSettings.LoadFormSize();
            base.SetVisibleCore(value);
        }

        protected override void WndProc(ref Message msg)
        {
            try
            {
                if(msg.Msg == 0x21)  // mouse click
                {
                    TerminalTabControlItem selectedTab = (TerminalTabControlItem)tcTerminals.SelectedItem;
                    if(selectedTab != null)
                    {
                        Rectangle r = selectedTab.RectangleToScreen(selectedTab.ClientRectangle);
                        if(r.Contains(Form.MousePosition))
                        {
                            SetGrabInput(true);
                        }
                        else
                        {
                            TabControlItem item = tcTerminals.GetTabItemByPoint(tcTerminals.PointToClient(Form.MousePosition));
                            if(item == null)
                                SetGrabInput(false);
                            else if(item == selectedTab)
                                SetGrabInput(true); //Grab input if clicking on currently selected tab
                        }
                    }
                    else
                        SetGrabInput(false);
                } else if(msg.Msg == WM_LEAVING_FULLSCREEN) {
                    if(CurrentTerminal != null)
                    {
                        if(CurrentTerminal.ContainsFocus)
                            tscConnectTo.Focus();
                    }
                    else
                        this.BringToFront();
                }
                base.WndProc(ref msg);
            }
            catch(Exception e)
            {
                Terminals.Logging.Log.Info("", e);
            }
        }
        #endregion

        #region public
        public static TerminalsCA CommandLineArgs
        {
            get { return MainForm._commandLineArgs; }
            set { MainForm._commandLineArgs = value; }
        }
        public static MainForm GetMainForm()
        {
            return _mainForm;
        }

        public void CloseTabControlItem()
        {
            tcTerminals_TabControlItemClosed(null, EventArgs.Empty);
        }
        public string GetDesktopShare()
        {
            string desktopShare = ((TerminalTabControlItem)(tcTerminals.SelectedItem)).Favorite.DesktopShare;
            if (String.IsNullOrEmpty(desktopShare))
            {
                desktopShare = Settings.DefaultDesktopShare.Replace("%SERVER%", CurrentTerminal.Server).Replace("%USER%", CurrentTerminal.UserName);
            }
            return desktopShare;
        }
        public void Connect(string connectionName, bool ForceConsole, bool ForceNewWindow)
        {
            FavoriteConfigurationElementCollection favorites = Settings.GetFavorites();
            FavoriteConfigurationElement favorite = (FavoriteConfigurationElement)favorites[connectionName].Clone();
            favsList1.RecordHistoryItem(connectionName);
            if (ForceConsole) favorite.ConnectToConsole = true;
            if (ForceNewWindow) favorite.NewWindow = true;

            if (favorite == null)
                CreateNewTerminal(connectionName);
            else
                CreateTerminalTab(favorite);
        }
        public void ToggleGrabInput()
        {
            if (CurrentTerminal != null)
            {
                CurrentTerminal.FullScreen = !CurrentTerminal.FullScreen;
            }
        }
        public void ShowManagedConnections()
        {
            using (OrganizeFavoritesForm conMgr = new OrganizeFavoritesForm())
            {
                conMgr.ShowDialog();
            }
            LoadFavorites();
        }
        public bool FullScreen
        {
            get
            {
                return _fullScreen;
            }
            set
            {
                if (_fullScreen != value) 
                    SetFullScreen(value);
                
                if (!_fullScreen)
                    ResetToolbars();
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
                tcTerminals.Items.Add(terminalTabPage);
                tcTerminals.SelectedItem = terminalTabPage;
                tcTerminals_SelectedIndexChanged(this, EventArgs.Empty);
                Terminals.Connections.NetworkingToolsConnection conn = new Terminals.Connections.NetworkingToolsConnection();
                conn.TerminalTabPage = terminalTabPage;
                conn.ParentForm = this;
                conn.Connect();
                (conn as Control).BringToFront();
                (conn as Control).Update();
                UpdateControls();
                conn.Execute(Action, Host);
            }
            catch (Exception exc)
            {
                Terminals.Logging.Log.Info("", exc);
                tcTerminals.Items.Remove(terminalTabPage);
                tcTerminals.SelectedItem = null;
                terminalTabPage.Dispose();
            }
        }

        public void ShowManageTerminalForm(FavoriteConfigurationElement Favorite)
        {
            using (NewTerminalForm frmNewTerminal = new NewTerminalForm(Favorite))
            {
                if (frmNewTerminal.ShowDialog() == DialogResult.OK)
                    LoadFavorites();
            }
        }        

        public Connections.IConnection CurrentConnection
        {
            get
            {
                if (tcTerminals.SelectedItem != null)
                    return ((TerminalTabControlItem)(tcTerminals.SelectedItem)).Connection;
                else
                    return null;
            }
        }

        public AxMsRdpClient2 CurrentTerminal
        {
            get
            {
                if (tcTerminals.SelectedItem != null)
                {
                    if (((TerminalTabControlItem)(tcTerminals.SelectedItem)).TerminalControl == null)
                    {
                        if (CurrentConnection != null)
                        {
                            if ((CurrentConnection as Connections.RDPConnection) != null)
                                return (CurrentConnection as Connections.RDPConnection).AxMsRdpClient2;
                            else
                                return null;
                        }
                        else
                            return null;
                    }
                    return null;
                }
                else
                {
                    if (CurrentConnection != null)
                    {
                        if ((CurrentConnection as Connections.RDPConnection) != null)
                            return (CurrentConnection as Connections.RDPConnection).AxMsRdpClient2;
                        else
                            return null;
                    }
                    else
                        return null;
                }
            }
        }

        public MainForm()
        {
            try
            {                
                _specialCommandsMIV = new MethodInvoker(LoadSpecialCommands);
                _resetMethodInvoker = new MethodInvoker(LoadWindowState);

                //check for wizard
                if (Settings.ShowWizard)
                {
                    //settings file doesnt exist, wizard!
                    FirstRunWizard wzrd = new FirstRunWizard();
                    wzrd.ShowDialog(this);
                    Settings.ShowWizard = false;
                }

                _imageFormatHandler = new ImageFormatHandler();
                _formSettings = new FormSettings(this);
                InitializeComponent();

                if (Settings.Office2007BlueFeel)
                    ToolStripManager.Renderer = Office2007Renderer.Office2007Renderer.GetRenderer(Office2007Renderer.RenderColors.Blue);
                else if (Settings.Office2007BlackFeel)
                    ToolStripManager.Renderer = Office2007Renderer.Office2007Renderer.GetRenderer(Office2007Renderer.RenderColors.Black);
                else
                    ToolStripManager.Renderer = new System.Windows.Forms.ToolStripProfessionalRenderer();

                tsbTags.Checked = Settings.ShowFavoritePanel;

                LoadFavorites();
                System.Threading.ThreadPool.QueueUserWorkItem(new System.Threading.WaitCallback(ReloadSpecialCommands), null);
                LoadGroups();
                UpdateControls();
                pnlTagsFavorites.Width = 7;
                LoadTags("");
                ProtocolHandler.Register();
                SingleInstanceApplication.NewInstanceMessage += new NewInstanceMessageEventHandler(SingleInstanceApplication_NewInstanceMessage);
                tcTerminals.MouseClick += new MouseEventHandler(tcTerminals_MouseClick);
                QuickContextMenu.ItemClicked += new ToolStripItemClickedEventHandler(QuickContextMenu_ItemClicked);
                SystemTrayQuickConnectToolStripMenuItem.DropDownItemClicked += new ToolStripItemClickedEventHandler(SystemTrayQuickConnectToolStripMenuItem_DropDownItemClicked);

                LoadWindowState();

                this.MainWindowNotifyIcon.Visible = Settings.MinimizeToTray;
                this.lockToolbarsToolStripMenuItem.Checked = Settings.ToolbarsLocked;
                this.groupsToolStripMenuItem.Visible = Settings.EnableGroupsMenu;


                if (Settings.ToolbarsLocked)
                    MainMenuStrip.GripStyle = ToolStripGripStyle.Hidden;
                else
                    MainMenuStrip.GripStyle = ToolStripGripStyle.Visible;

                _mainForm = this;

                if (Screen.AllScreens.Length > 1)
                {
                    showInDualScreensToolStripMenuItem.Enabled = true;

                    //Lazy check to see if we are using dual screens
                    int w = this.Width / Screen.PrimaryScreen.Bounds.Width;
                    if (w > 2)
                    {
                        _allScreens = true;
                        showInDualScreensToolStripMenuItem.Text = "Show in Singel Screens";
                    }
                }
                else
                {
                    showInDualScreensToolStripMenuItem.ToolTipText = "You only have one Screen";
                    showInDualScreensToolStripMenuItem.Enabled = false;
                }
            }
            catch (Exception exc)
            {
                Terminals.Logging.Log.Info("", exc);
            }
        }

        public void LoadWindowState()
        {
            this.Text = Program.AboutText;

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
                        menuItem = toolStripMenuItem4;
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
                        int row = setting.Row + 1;
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
        public void UpdateControls()
        {
            tcTerminals.ShowToolTipOnTitle = Settings.ShowInformationToolTips;
            addTerminalToGroupToolStripMenuItem.Enabled = (tcTerminals.SelectedItem != null);
            tsbGrabInput.Enabled = (tcTerminals.SelectedItem != null);
            grabInputToolStripMenuItem.Enabled = tcTerminals.SelectedItem != null;
            tsbGrabInput.Checked = tsbGrabInput.Enabled && (CurrentTerminal != null) && CurrentTerminal.FullScreen;
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
                Connections.VMRCConnection vmrc;
                vmrc = (this.CurrentConnection as Connections.VMRCConnection);
                if (vmrc != null)
                {
                    VMRCAdminSwitchButton.Visible = true;
                    VMRCViewOnlyButton.Visible = true;
                }
                Connections.VNCConnection vnc;
                vnc = (this.CurrentConnection as Connections.VNCConnection);
                if (vnc != null)
                {
                    vncActionButton.Visible = true;
                }
                this.TerminalServerMenuButton.Visible = this.CurrentConnection.IsTerminalServer;
            }
        }
        public static bool ReleaseAvailable
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
                    FavoriteConfigurationElementCollection favs = Settings.GetFavorites();
                    FavoriteConfigurationElement release = null;
                    foreach (FavoriteConfigurationElement fav in favs)
                    {
                        if (fav.Name == _terminalsReleasesFavoriteName)
                        {
                            release = fav;
                            break;
                        }
                    }
                    if (release == null)
                    {
                        release = new FavoriteConfigurationElement(_terminalsReleasesFavoriteName);
                        release.Url = "http://www.codeplex.com/Terminals/Wiki/View.aspx?title=Welcome%20To%20Terminals";
                        release.Tags = Program.Resources.GetString("Terminals");
                        release.Protocol = "HTTP";
                        Settings.AddFavorite(release, false);
                    }
                    System.Threading.Thread.Sleep(5000);
                    if (OnReleaseIsAvailable != null) OnReleaseIsAvailable(release);
                }
            }
        }
        public static Unified.Rss.RssItem ReleaseDescription
        {
            get
            {
                return _releaseDescription;
            }
            set
            {
                _releaseDescription = value;

            }
        }

        public void OpenConnectionInNewWindow(Terminals.Connections.IConnection Connection)
        {
            if (Connection != null)
            {
                PopupTerminal pop = new PopupTerminal();
                Terminals.Connections.IConnection conn = CurrentConnection;
                tcTerminals.Items.SuspendEvents();
                tcTerminals.RemoveTab(CurrentConnection.TerminalTabPage);
                pop.AddTerminal(conn.TerminalTabPage);
                pop.MainForm = this;
                tcTerminals.Items.ResumeEvents();
                pop.Show();
            }
        }

        public void AddTerminal(TerminalTabControlItem TabControlItem) {
            this.tcTerminals.AddTab(TabControlItem);
        }
        #endregion

        #region private made by developer
        private string GetToolTipText(FavoriteConfigurationElement favorite)
        {
            string toolTip = "";
            if (favorite != null)
            {
                toolTip =
                    "Computer: " + favorite.ServerName + Environment.NewLine +
                    "User: " + Functions.UserDisplayName(favorite.DomainName, favorite.UserName) + Environment.NewLine;

                if (Settings.ShowFullInformationToolTips)
                {
                    toolTip +=
                    "Port: " + favorite.Port + Environment.NewLine +
                    "Connect to Console: " + favorite.ConnectToConsole.ToString() + Environment.NewLine +
                    "Notes: " + favorite.Notes + Environment.NewLine;
                }
            }
            return toolTip;
        }

        public void CreateTerminalTab(FavoriteConfigurationElement favorite)
        {
            if (Settings.ExecuteBeforeConnect)
            {
                ProcessStartInfo processStartInfo = new ProcessStartInfo(Settings.ExecuteBeforeConnectCommand,
                    Settings.ExecuteBeforeConnectArgs);
                processStartInfo.WorkingDirectory = Settings.ExecuteBeforeConnectInitialDirectory;
                Process process = Process.Start(processStartInfo);
                if (Settings.ExecuteBeforeConnectWaitForExit)
                {
                    process.WaitForExit();
                }
            }

            if (favorite.ExecuteBeforeConnect)
            {
                ProcessStartInfo processStartInfo = new ProcessStartInfo(favorite.ExecuteBeforeConnectCommand,
                    favorite.ExecuteBeforeConnectArgs);
                processStartInfo.WorkingDirectory = favorite.ExecuteBeforeConnectInitialDirectory;
                Process process = Process.Start(processStartInfo);
                if (favorite.ExecuteBeforeConnectWaitForExit)
                {
                    process.WaitForExit();
                }
            }

            string terminalTabTitle = favorite.Name;
            if (Settings.ShowUserNameInTitle)
            {
                terminalTabTitle += " (" + Functions.UserDisplayName(favorite.DomainName, favorite.UserName) + ")";
            }
            TerminalTabControlItem terminalTabPage = new TerminalTabControlItem(terminalTabTitle);
            try
            {
                terminalTabPage.AllowDrop = true;
                terminalTabPage.DragOver += terminalTabPage_DragOver;
                terminalTabPage.DragEnter += new DragEventHandler(terminalTabPage_DragEnter);
                this.Resize += new EventHandler(MainForm_Resize);
                terminalTabPage.ToolTipText = GetToolTipText(favorite);
                terminalTabPage.Favorite = favorite;
                terminalTabPage.DoubleClick += new EventHandler(terminalTabPage_DoubleClick);
                tcTerminals.Items.Add(terminalTabPage);
                tcTerminals.SelectedItem = terminalTabPage;
                tcTerminals_SelectedIndexChanged(this, EventArgs.Empty);
                Connections.IConnection conn = Connections.ConnectionManager.CreateConnection(favorite, terminalTabPage, this);
                if (conn.Connect())
                {
                    (conn as Control).BringToFront();
                    (conn as Control).Update();
                    UpdateControls();
                    if (favorite.DesktopSize == DesktopSize.FullScreen)
                        FullScreen = true;

                    Terminals.Connections.Connection b = (conn as Terminals.Connections.Connection);
                    b.OnTerminalServerStateDiscovery += new Terminals.Connections.Connection.TerminalServerStateDiscovery(b_OnTerminalServerStateDiscovery);
                    b.CheckForTerminalServer(favorite);

                }
                else
                {
                    string msg = Program.Resources.GetString("SorryTerminalswasunabletoconnecttotheremotemachineTryagainorcheckthelogformoreinformation");
                    System.Windows.Forms.MessageBox.Show(msg);
                    tcTerminals.Items.Remove(terminalTabPage);
                    tcTerminals.SelectedItem = null;
                }

                if (conn.Connected)
                {
                    if (favorite.NewWindow)
                    {
                        OpenConnectionInNewWindow(conn);
                    }
                }
            }
            catch (Exception exc)
            {
                Terminals.Logging.Log.Info("", exc);
                tcTerminals.SelectedItem = null;
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
                            ToolStripMenuItem sess = new ToolStripMenuItem(string.Format("{1} - {2} ({0})", session.State.ToString().Replace("WTS", ""), session.Client.ClientName, session.Client.UserName));
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

        private void b_OnTerminalServerStateDiscovery(FavoriteConfigurationElement Favorite, bool IsTerminalServer, TerminalServices.TerminalServer Server)
        {
        }        

        private void LoadSpecialCommands()
        {
            SpecialCommandsToolStrip.Items.Clear();
            SpecialCommandConfigurationElementCollection cmdList = Settings.SpecialCommands;

            foreach (SpecialCommandConfigurationElement cmd in Settings.SpecialCommands)
            {
                ToolStripMenuItem mi = new ToolStripMenuItem(cmd.Name);
                mi.DisplayStyle = ToolStripItemDisplayStyle.Image;
                mi.ToolTipText = cmd.Name;
                mi.Text = cmd.Name;
                mi.Tag = cmd;
                mi.Image = cmd.LoadThumbnail();
                mi.Overflow = ToolStripItemOverflow.AsNeeded;
                SpecialCommandsToolStrip.Items.Add(mi);
            }
        }
        private void ShowCredentialsManager()
        {
            Credentials.CredentialManager mgr = new Terminals.Credentials.CredentialManager();
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
            int rowIndex = 0;
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
            System.Threading.ThreadPool.QueueUserWorkItem(new System.Threading.WaitCallback(ToolbarResetThread));
        }
        private void ToolbarResetThread(object state)
        {
            this.Invoke(_resetMethodInvoker);
            this.Invoke(_resetMethodInvoker);
        }

        public void LoadFavorites()
        {
            SortedDictionary<string, FavoriteConfigurationElement> favorites = Settings.GetSortedFavorites(Settings.DefaultSortProperty);
            int seperatorIndex = favoritesToolStripMenuItem.DropDownItems.IndexOf(favoritesSeparator);
            for (int i = favoritesToolStripMenuItem.DropDownItems.Count - 1; i > seperatorIndex; i--)
            {
                favoritesToolStripMenuItem.DropDownItems.RemoveAt(i);
            }

            tscConnectTo.Items.Clear();
            foreach (string key in favorites.Keys)
            {
                FavoriteConfigurationElement favorite = favorites[key];
                SystemTrayQuickConnectToolStripMenuItem.DropDownItems.Add(favorite.Name);
            }

            Dictionary<string, ToolStripMenuItem> tagTools = new Dictionary<string, ToolStripMenuItem>();

            foreach (string key in favorites.Keys)
            {
                FavoriteConfigurationElement favorite = favorites[key];
                ToolStripMenuItem sortedItem = new ToolStripMenuItem();
                sortedItem.Text = favorite.Name;
                sortedItem.Tag = "favorite";
                tscConnectTo.Items.Add(favorite.Name);

                if (!string.IsNullOrEmpty(favorite.ToolBarIcon) && File.Exists(favorite.ToolBarIcon))
                    sortedItem.Image = Image.FromFile(favorite.ToolBarIcon);

                if (favorite.TagList != null && favorite.TagList.Count > 0)
                {
                    foreach (string tag in favorite.TagList)
                    {
                        ToolStripMenuItem parent = null;
                        if (tagTools.ContainsKey(tag))
                            parent = tagTools[tag];
                        else if (!tag.Contains("Terminals"))
                        {
                            parent = new ToolStripMenuItem(tag);
                            parent.Name = tag;
                            tagTools.Add(tag, parent);
                        }

                        if (parent != null)
                        {
                            ToolStripMenuItem item = new ToolStripMenuItem(favorite.Name);
                            item.Click += serverToolStripMenuItem_Click;
                            item.Name = favorite.Name;
                            item.Tag = "favorite";

                            if (favorite.ToolBarIcon != null && System.IO.File.Exists(favorite.ToolBarIcon))
                                item.Image = Image.FromFile(favorite.ToolBarIcon);

                            parent.DropDown.Items.Add(item);
                            favoritesToolStripMenuItem.DropDown.Items.Add(parent);
                        }
                    }
                }
                else
                {
                    ToolStripMenuItem item = new ToolStripMenuItem(favorite.Name);
                    item.Click += serverToolStripMenuItem_Click;
                    item.Name = favorite.Name;
                    item.Tag = "favorite";

                    if (favorite.ToolBarIcon != null && System.IO.File.Exists(favorite.ToolBarIcon))
                        item.Image = Image.FromFile(favorite.ToolBarIcon);

                    favoritesToolStripMenuItem.DropDown.Items.Add(item);
                }
            }

            this.favsList1.LoadFavs();
            LoadFavoritesToolbar();
        }

        private void LoadFavoritesToolbar()
        {
            try
            {
                favoriteToolBar.Items.Clear();
                if (Settings.FavoritesToolbarButtons != null)
                {
                    foreach (string favoriteButton in Settings.FavoritesToolbarButtons)
                    {
                        FavoriteConfigurationElementCollection favorites = Settings.GetFavorites();
                        FavoriteConfigurationElement favorite = favorites[favoriteButton];
                        Bitmap button = Resources.smallterm;
                        if (favorite != null)
                        {
                            if (!string.IsNullOrEmpty(favorite.ToolBarIcon) && File.Exists(favorite.ToolBarIcon))
                            {
                                try
                                {
                                    button = (Bitmap)Bitmap.FromFile(favorite.ToolBarIcon);
                                }
                                catch (Exception ex)
                                {
                                    Terminals.Logging.Log.Info("", ex);
                                    if (button != Resources.smallterm) 
                                        button = Resources.smallterm;
                                }
                            }
                            ToolStripButton favoriteBtn = new ToolStripButton(favorite.Name, button, serverToolStripMenuItem_Click);
                            favoriteBtn.Tag = favorite;
                            favoriteBtn.Overflow = ToolStripItemOverflow.AsNeeded;
                            favoriteToolBar.Items.Add(favoriteBtn);
                        }
                    }
                }
                favoriteToolBar.Visible = toolStripMenuItem4.Checked;
                this.favsList1.LoadFavs();
            }
            catch (Exception exc)
            {
                Terminals.Logging.Log.Info("", exc);
            }
        }

        private void AddFavorite(FavoriteConfigurationElement favorite)
        {
            tscConnectTo.Items.Add(favorite.Name);
            ToolStripMenuItem serverToolStripMenuItem = new ToolStripMenuItem(favorite.Name);
            serverToolStripMenuItem.Name = favorite.Name;
            serverToolStripMenuItem.Click += serverToolStripMenuItem_Click;
            favoritesToolStripMenuItem.DropDownItems.Add(serverToolStripMenuItem);
        }

        private void LoadGroups()
        {
            GroupConfigurationElementCollection serversGroups = Settings.GetGroups();
            int seperatorIndex = groupsToolStripMenuItem.DropDownItems.IndexOf(groupsSeparator);
            for (int i = groupsToolStripMenuItem.DropDownItems.Count - 1; i > seperatorIndex; i--)
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

        private void SetGrabInput(bool grab)
        {
            if (CurrentTerminal != null)
            {
                if (grab && !CurrentTerminal.ContainsFocus)
                    CurrentTerminal.Focus();
                CurrentTerminal.FullScreen = grab;
            }
        }

        private void CreateNewTerminal()
        {
            CreateNewTerminal(null);
        }

        private void CreateNewTerminal(string name)
        {
            using (NewTerminalForm frmNewTerminal = new NewTerminalForm(name, true))
            {
                if (frmNewTerminal.ShowDialog() == DialogResult.OK)
                {
                    Settings.AddFavorite(frmNewTerminal.Favorite, frmNewTerminal.ShowOnToolbar);
                    LoadFavorites();
                    tscConnectTo.SelectedIndex = tscConnectTo.Items.IndexOf(frmNewTerminal.Favorite.Name);
                    CreateTerminalTab(frmNewTerminal.Favorite);
                }
            }
            LoadFavorites();
        }

        private void HideToolBar(bool fullScreen)
        {
            if (!fullScreen)
            {
                toolbarStd.Visible = _stdToolbarState;
                SpecialCommandsToolStrip.Visible = _specialToolbarState;
                favoriteToolBar.Visible = _favToolbarState;
            }
            else
            {
                toolbarStd.Visible = false;
                SpecialCommandsToolStrip.Visible = false;
                favoriteToolBar.Visible = false;
            }

        }

        private void SetFullScreen(bool fullScreen)
        {
            this.Visible = false;

            if (fullScreen)
            {
                _stdToolbarState = toolbarStd.Visible;
                _specialToolbarState = SpecialCommandsToolStrip.Visible;
                _favToolbarState = favoriteToolBar.Visible;
            }
            HideToolBar(fullScreen);

            if (fullScreen)
            {
                menuStrip.Visible = false;
                this._lastLocation = this.Location;
                this._lastSize = this.RestoreBounds.Size;
                
                if (this.WindowState == FormWindowState.Minimized)
                    this._lastState = FormWindowState.Normal;
                else
                    this._lastState = this.WindowState;
                
                this.FormBorderStyle = FormBorderStyle.None;
                this.WindowState = FormWindowState.Normal;
                if (_allScreens)
                {
                    Screen[] screenArr = Screen.AllScreens;
                    int with = 0;
                    if (_allScreens)
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
                SetGrabInput(true);
                this.BringToFront();
            }
            else
            {
                this.TopMost = false;
                this.FormBorderStyle = FormBorderStyle.Sizable;
                this.WindowState = this._lastState;
                if (_lastState != FormWindowState.Minimized)
                {
                    if (_lastState == FormWindowState.Normal)
                        this.Location = this._lastLocation;
                    this.Size = this._lastSize;
                }
                menuStrip.Visible = true;
            }
            
            this._fullScreen = fullScreen;

            tcTerminals.ShowTabs = !fullScreen;
            tcTerminals.ShowBorder = !fullScreen;

            this.Visible = true;
            this.PerformLayout();
        }

        private void QuickConnect(string server, int port, bool ConnectToConsole)
        {
            FavoriteConfigurationElementCollection favorites = Settings.GetFavorites();
            FavoriteConfigurationElement favorite = favorites[server];
            if (favorite != null)
            {
                if (favorite.ConnectToConsole != ConnectToConsole) favorite.ConnectToConsole = ConnectToConsole;
                CreateTerminalTab(favorite);
            }
            else
            {
                //create a temporaty favorite and connect to it
                favorite = new FavoriteConfigurationElement();
                favorite.ConnectToConsole = ConnectToConsole;
                favorite.ServerName = server;
                favorite.Name = server;
                if (port != 0)
                    favorite.Port = port;
                CreateTerminalTab(favorite);
            }
        }

        private void HandleCommandLineActions()
        {

            bool ConnectToConsole = Terminals.MainForm._commandLineArgs.console;
            this.FullScreen = Terminals.MainForm._commandLineArgs.fullscreen;
            if (Terminals.MainForm._commandLineArgs.url != null && Terminals.MainForm._commandLineArgs.url != "")
            {
                string server; int port;
                ProtocolHandler.Parse(Terminals.MainForm._commandLineArgs.url, out server, out port);
                QuickConnect(server, port, ConnectToConsole);
            }
            if (Terminals.MainForm._commandLineArgs.machine != null && Terminals.MainForm._commandLineArgs.machine != "")
            {
                string server = ""; int port = 3389;
                server = Terminals.MainForm._commandLineArgs.machine;
                int index = Terminals.MainForm._commandLineArgs.machine.IndexOf(":");
                if (index > 0)
                {
                    server = Terminals.MainForm._commandLineArgs.machine.Substring(0, index);
                    string p = Terminals.MainForm._commandLineArgs.machine.Substring(index + 1);
                    if (!int.TryParse(p, out port))
                    {
                        port = 3389;
                    }
                }
                QuickConnect(server, port, ConnectToConsole);
            } if (Terminals.MainForm._commandLineArgs.favs != null && Terminals.MainForm._commandLineArgs.favs != "")
            {
                string favs = Terminals.MainForm._commandLineArgs.favs;
                if (favs.Contains(","))
                {
                    string[] favlist = favs.Split(',');
                    foreach (string fav in favlist)
                    {
                        Connect(fav, false, false);
                    }
                }
                else
                {
                    Connect(favs, false, false);
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

        private void LoadTags(string filter)
        {
            ListViewItem unTaggedListViewItem = new ListViewItem();
            unTaggedListViewItem.ImageIndex = 0;
            unTaggedListViewItem.StateImageIndex = 0;
            unTaggedListViewItem.ToolTipText = Program.Resources.GetString("UnTagged");
            List<FavoriteConfigurationElement> unTaggedFavorites = new List<FavoriteConfigurationElement>();
            foreach (string tag in Settings.Tags)
            {
                if ((String.IsNullOrEmpty(filter) || (tag.ToUpper().StartsWith(filter.ToUpper()))))
                {
                    ListViewItem item = new ListViewItem();
                    item.ImageIndex = 0;
                    item.StateImageIndex = 0;
                    SortedDictionary<string, FavoriteConfigurationElement> favorites = Settings.GetSortedFavorites(Settings.SortProperties.ConnectionName);

                    List<FavoriteConfigurationElement> tagFavorites = new List<FavoriteConfigurationElement>();
                    foreach (string key in favorites.Keys)
                    {
                        FavoriteConfigurationElement favorite = favorites[key];
                        if (favorite.TagList.IndexOf(tag) >= 0)
                        {
                            tagFavorites.Add(favorite);
                        }
                        else if (favorite.TagList.Count == 0)
                        {
                            if (unTaggedFavorites.IndexOf(favorite) < 0)
                            {
                                unTaggedFavorites.Add(favorite);
                            }
                        }
                    }
                    item.Tag = tagFavorites;
                    item.Text = tag + " (" + tagFavorites.Count.ToString() + ")";
                    item.ToolTipText = tag;
                }
            }
            if (Settings.Tags.Length == 0)
            {
                FavoriteConfigurationElementCollection favorites = Settings.GetFavorites();
                List<FavoriteConfigurationElement> tagFavorites = new List<FavoriteConfigurationElement>();
                foreach (FavoriteConfigurationElement favorite in favorites)
                {
                    if (unTaggedFavorites.IndexOf(favorite) < 0)
                    {
                        unTaggedFavorites.Add(favorite);
                    }
                }
            }
            unTaggedListViewItem.Tag = unTaggedFavorites;
            unTaggedListViewItem.Text = Program.Resources.GetString("UnTagged") + " (" + unTaggedFavorites.Count.ToString() + ")";
        }

        private void AddTagToFavorite(FavoriteConfigurationElement Favorite, string Tag)
        {
            List<string> tagList = new List<string>();
            foreach (string tag in Favorite.TagList)
            {
                tagList.Add(tag);
            }
            tagList.Add(Tag);
            Favorite.Tags = String.Join(",", tagList.ToArray());
        }
        private void RemoveTagFromFavorite(FavoriteConfigurationElement Favorite, string Tag)
        {
            List<string> tagList = new List<string>();
            string t = Tag.Trim().ToUpper();
            foreach (string tag in Favorite.TagList)
            {
                if (tag.Trim().ToUpper() != t) tagList.Add(tag);
            }
            Favorite.Tags = String.Join(",", tagList.ToArray());
        }

        private void OpenReleasePage()
        {
            Connect(_terminalsReleasesFavoriteName, false, false);
        }
        private void ReloadSpecialCommands(object state)
        {
            while(!this.Created) System.Threading.Thread.Sleep(500);
            rebuildShortcutsToolStripMenuItem_Click(null, null);
        }
        #endregion

        #region private event
        private void SystemTrayQuickConnectToolStripMenuItem_DropDownItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {
            Connect(e.ClickedItem.Text, false, false);
        }
        private void tcTerminals_MouseClick(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                QuickContextMenu.Items.Clear();

                if (this.FullScreen)
                    QuickContextMenu.Items.Add(Program.Resources.GetString("RestoreScreen"), Resources.arrow_in);
                else
                    QuickContextMenu.Items.Add(Program.Resources.GetString("FullScreen"), Resources.arrow_out);
                
                QuickContextMenu.Items.Add("-");
                QuickContextMenu.Items.Add(Program.Resources.GetString("ShowMenu"));

                QuickContextMenu.Items.Add("-");
                QuickContextMenu.Items.Add(Program.Resources.GetString("ScreenCaptureManager"), Resources.screen_capture_box);
                QuickContextMenu.Items.Add(Program.Resources.GetString("NetworkingTools"), Resources.computer_link);
                QuickContextMenu.Items.Add("-");
                QuickContextMenu.Items.Add(Program.Resources.GetString("CredentialsManager"), Resources.computer_security);
                QuickContextMenu.Items.Add(Program.Resources.GetString("OrganizeFavorites"), Resources.application_edit);
                QuickContextMenu.Items.Add(Program.Resources.GetString("Options"), Resources.options);
                QuickContextMenu.Items.Add("-");

                ToolStripMenuItem special = new ToolStripMenuItem(Program.Resources.GetString("SpecialCommands"), Resources.computer_link);
                ToolStripMenuItem mgmt = new ToolStripMenuItem(Program.Resources.GetString("Management"), Resources.CompMgmt);
                ToolStripMenuItem cpl = new ToolStripMenuItem(Program.Resources.GetString("ControlPanel"), Resources.ControlPanel);
                ToolStripMenuItem other = new ToolStripMenuItem(Program.Resources.GetString("Other"), Resources.star);

                QuickContextMenu.Items.Add(special);
                special.DropDown.Items.Add(mgmt);
                special.DropDown.Items.Add(cpl);
                special.DropDown.Items.Add(other);

                foreach (SpecialCommandConfigurationElement elm in Settings.SpecialCommands)
                {
                    Image img = null;
                    if (elm.Thumbnail != null && elm.Thumbnail != "" && System.IO.File.Exists(elm.Thumbnail))
                    {
                        img = Image.FromFile(elm.Thumbnail);
                    }
                    else
                    {
                        img = Resources.server_administrator_icon;
                    }
                    ToolStripItem specialItem;
                    if (elm.Executable.ToLower().EndsWith("cpl"))
                    {
                        specialItem = cpl.DropDown.Items.Add(elm.Name, img);
                    }
                    else if (elm.Executable.ToLower().EndsWith("msc"))
                    {
                        specialItem = mgmt.DropDown.Items.Add(elm.Name, img);
                    }
                    else
                    {
                        specialItem = other.DropDown.Items.Add(elm.Name, img);
                    }
                    specialItem.Click += new EventHandler(specialItem_Click);
                    specialItem.Tag = elm;
                }

                QuickContextMenu.Items.Add("-");

                SortedDictionary<string, FavoriteConfigurationElement> favorites = Settings.GetSortedFavorites(Settings.DefaultSortProperty);

                Dictionary<string, ToolStripMenuItem> tagTools = new Dictionary<string, ToolStripMenuItem>();
                SortedDictionary<string, ToolStripMenuItem> sortedList = new SortedDictionary<string, ToolStripMenuItem>();
                ToolStripMenuItem sortedMenu = new ToolStripMenuItem(Program.Resources.GetString("Alphabetical"));
                sortedMenu.DropDownItemClicked += new ToolStripItemClickedEventHandler(QuickContextMenu_ItemClicked);

                foreach (string key in favorites.Keys)
                {
                    FavoriteConfigurationElement favorite = favorites[key];

                    System.Windows.Forms.ToolStripMenuItem sortedItem = new ToolStripMenuItem();
                    sortedItem.Text = favorite.Name;
                    sortedItem.Tag = "favorite";
                    if (favorite.ToolBarIcon != null && System.IO.File.Exists(favorite.ToolBarIcon))
                        sortedItem.Image = Image.FromFile(favorite.ToolBarIcon);

                    sortedList.Add(favorite.Name, sortedItem);


                    if (favorite.TagList != null && favorite.TagList.Count > 0)
                    {
                        foreach (string tag in favorite.TagList)
                        {
                            System.Windows.Forms.ToolStripMenuItem parent;
                            if (tagTools.ContainsKey(tag))
                            {
                                parent = tagTools[tag];
                            }
                            else
                            {
                                parent = new ToolStripMenuItem();
                                parent.DropDownItemClicked += new ToolStripItemClickedEventHandler(QuickContextMenu_ItemClicked);
                                parent.Tag = "tag";
                                parent.Text = tag;
                                tagTools.Add(tag, parent);
                                QuickContextMenu.Items.Add(parent);
                            }
                            System.Windows.Forms.ToolStripMenuItem item = new ToolStripMenuItem();
                            item.Text = favorite.Name;
                            item.Tag = "favorite";
                            if (favorite.ToolBarIcon != null && System.IO.File.Exists(favorite.ToolBarIcon))
                                item.Image = Image.FromFile(favorite.ToolBarIcon);

                            parent.DropDown.Items.Add(item);


                        }
                    }
                    else
                    {
                        ToolStripMenuItem item = new ToolStripMenuItem(favorite.Name);
                        item.Tag = "favorite";
                        if (favorite.ToolBarIcon != null && System.IO.File.Exists(favorite.ToolBarIcon))
                            item.Image = Image.FromFile(favorite.ToolBarIcon);

                        QuickContextMenu.Items.Add(item);

                    }
                }
                if (sortedList != null && sortedList.Count > 0)
                {
                    QuickContextMenu.Items.Add(sortedMenu);
                    sortedMenu.Image = Terminals.Properties.Resources.atoz;
                    foreach (string name in sortedList.Keys)
                    {
                        sortedMenu.DropDownItems.Add(sortedList[name]);
                    }
                }

                QuickContextMenu.Items.Add("-");
                QuickContextMenu.Items.Add(Program.Resources.GetString("Exit"));
                if (tcTerminals != null && sender != null) QuickContextMenu.Show(tcTerminals, e.Location);
            }
        }

        private void specialItem_Click(object sender, EventArgs e)
        {
            ToolStripItem specialItem = (ToolStripItem)sender;
            SpecialCommandConfigurationElement elm = (SpecialCommandConfigurationElement)specialItem.Tag;
            elm.Launch();
        }

        private void QuickContextMenu_Opening(object sender, CancelEventArgs e)
        {

            if (QuickContextMenu.Items.Count <= 0)
            {
                tcTerminals_MouseClick(null, new MouseEventArgs(MouseButtons.Right, 1, 0, 0, 0));
                e.Cancel = false;
            }
        }

        private void QuickContextMenu_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {
            QuickContextMenu.Hide();

            if (
                e.ClickedItem.Text == Program.Resources.GetString("Restore") ||
                e.ClickedItem.Text == Program.Resources.GetString("RestoreScreen") ||
                e.ClickedItem.Text == Program.Resources.GetString("FullScreen"))
            {
                this.FullScreen = !this.FullScreen;
            }
            else if (e.ClickedItem.Text == Program.Resources.GetString("CredentialsManager"))
            {
                ShowCredentialsManager();
            }
            else if (e.ClickedItem.Text == Program.Resources.GetString("OrganizeFavorites"))
            {
                manageConnectionsToolStripMenuItem_Click(null, null);
            }
            else if (e.ClickedItem.Text == Program.Resources.GetString("Options"))
            {
                optionsToolStripMenuItem_Click(null, null);
            }
            else if (e.ClickedItem.Text == Program.Resources.GetString("NetworkingTools"))
            {
                toolStripButton2_Click(null, null);
            }
            else if (e.ClickedItem.Text == Program.Resources.GetString("ScreenCaptureManager"))
            {
                toolStripMenuItem5_Click(new Object(), null);
            }
            else if (e.ClickedItem.Text == Program.Resources.GetString("Exit"))
            {
                Close();
            }
            else if (e.ClickedItem.Text == Program.Resources.GetString("ShowMenu"))
            {
                AddShowStrip(menuStrip, menubarToolStripMenuItem, !menuStrip.Visible);
            }
            else
            {
                string tag = (e.ClickedItem.Tag as string);

                if (tag != null)
                {
                    string itemName = e.ClickedItem.Text;
                    if (tag == "favorite") Connect(itemName, false, false);
                    if (tag == "tag")
                    {
                        System.Windows.Forms.ToolStripMenuItem parent = (e.ClickedItem as System.Windows.Forms.ToolStripMenuItem);
                        if (parent.DropDownItems.Count > 0)
                        {
                            if (System.Windows.Forms.MessageBox.Show(string.Format(Program.Resources.GetString("Areyousureyouwanttoconnecttoalltheseterminals"), parent.DropDownItems.Count), Program.Resources.GetString(Program.Resources.GetString("Confirmation")), MessageBoxButtons.OKCancel) == DialogResult.OK)
                            {
                                foreach (ToolStripMenuItem button in parent.DropDownItems)
                                {
                                    Connect(button.Text, false, false);
                                }
                            }
                        }
                    }
                }
            }
        }

        private void SingleInstanceApplication_NewInstanceMessage(object sender, object message)
        {
            if (WindowState == FormWindowState.Minimized)
                NativeApi.ShowWindow(new HandleRef(this, this.Handle), 9);
            Activate();
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void groupAddToolStripMenuItem_Click(object sender, EventArgs e)
        {
            GroupConfigurationElement group = Settings.GetGroups()[((ToolStripMenuItem)sender).Text];
            group.FavoriteAliases.Add(new FavoriteAliasConfigurationElement(tcTerminals.SelectedItem.Title));
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
                CreateTerminalTab(favorite);
            }
        }

        private void serverToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FavoriteConfigurationElement favorite = Settings.GetFavorites()[((ToolStripItem)(sender)).Text];
            CreateTerminalTab(favorite);
        }

        private void sd_Click(object sender, EventArgs e)
        {
            ToolStripMenuItem menu = (sender as ToolStripMenuItem);
            if (menu != null)
            {
                if (menu.Text == Program.Resources.GetString("Shutdown"))
                {
                    TerminalServices.TerminalServer server = (menu.Tag as TerminalServices.TerminalServer);
                    if (server != null && System.Windows.Forms.MessageBox.Show(Program.Resources.GetString("Areyousureyouwanttoshutthismachineoff"), Program.Resources.GetString("Confirmation"), MessageBoxButtons.OKCancel) == DialogResult.OK)
                        TerminalServices.TerminalServicesAPI.ShutdownSystem(server, false);
                }
                else if (menu.Text == Program.Resources.GetString("Reboot"))
                {
                    TerminalServices.TerminalServer server = (menu.Tag as TerminalServices.TerminalServer);
                    if (server != null && System.Windows.Forms.MessageBox.Show(Program.Resources.GetString("Areyousureyouwanttorebootthismachine"), Program.Resources.GetString("Confirmation"), MessageBoxButtons.OKCancel) == DialogResult.OK)
                        TerminalServices.TerminalServicesAPI.ShutdownSystem(server, true);
                }
                else if (menu.Text == Program.Resources.GetString("Logoff"))
                {
                    TerminalServices.Session session = (menu.Tag as TerminalServices.Session);
                    if (session != null && System.Windows.Forms.MessageBox.Show(Program.Resources.GetString("Areyousureyouwanttologthissessionoff"), Program.Resources.GetString("Confirmation"), MessageBoxButtons.OKCancel) == DialogResult.OK)
                        TerminalServices.TerminalServicesAPI.LogOffSession(session, false);

                }
                else if (menu.Text == Program.Resources.GetString("Send Message"))
                {
                    TerminalServices.Session session = (menu.Tag as TerminalServices.Session);
                    if (session != null)
                    {
                        Terminals.InputBoxResult result = Terminals.InputBox.Show(Program.Resources.GetString("Pleaseenterthemessagetosend"));
                        if (result.ReturnCode == DialogResult.OK && result.Text.Trim() != null)
                        {
                            TerminalServices.TerminalServicesAPI.SendMessage(session, Program.Resources.GetString("MessagefromyourAdministrator"), result.Text.Trim(), 0, 10, false);
                        }

                    }
                }

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
            /*TabControlItem item = tcTerminals.GetTabItemByPoint(new Point(e.X, e.Y));
            if (item != null)
            {
                tcTerminals.SelectedItem = item;
            }*/
            tcTerminals.SelectedItem = (TerminalTabControlItem)sender;
        }

        private void terminalTabPage_DoubleClick(object sender, EventArgs e)
        {
            if (tcTerminals.SelectedItem != null)
            {
                tsbDisconnect.PerformClick();
            }
        }

        private void newTerminalToolStripMenuItem_Click(object sender, EventArgs e)
        {
            CreateNewTerminal();
        }

        private void tsbConnect_Click(object sender, EventArgs e)
        {
            string connectionName = tscConnectTo.Text;
            if (connectionName != "")
            {
                Connect(connectionName, false, false);
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
                tsbConnect.PerformClick();
            }
            if (e.KeyCode == Keys.Delete && tscConnectTo.DroppedDown &&
                tscConnectTo.SelectedIndex != -1)
            {
                string connectionName = (string)tscConnectTo.Items[tscConnectTo.SelectedIndex];
                DeleteFavorite(connectionName);
            }
        }

        private void tsbDisconnect_Click(object sender, EventArgs e)
        {
            try
            {
                tcTerminals.CloseTab(tcTerminals.SelectedItem);
            }
            catch (Exception exc)
            {
                Terminals.Logging.Log.Error("Disconnecting a tab threw an exception", exc);
            }
        }

        private void tcTerminals_SelectedIndexChanged(object sender, EventArgs e)
        {
            UpdateControls();
        }

        private void newTerminalToolStripMenuItem_Click_1(object sender, EventArgs e)
        {
            CreateNewTerminal();
        }

        private void tsbGrabInput_Click(object sender, EventArgs e)
        {
            ToggleGrabInput();
        }

        private void MainForm_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyValue == 3)
                ToggleGrabInput();
        }

        private void tcTerminals_TabControlItemClosing(TabControlItemClosingEventArgs e)
        {
            bool cancel = false; 
            if (CurrentConnection != null && CurrentConnection.Connected)
            {
                bool close = false;
                if (Settings.WarnOnConnectionClose)
                {
                    close = (MessageBox.Show(this, Program.Resources.GetString("Areyousurethatyouwanttodisconnectfromtheactiveterminal"),
                    Program.Resources.GetString("Terminals"), MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes);
                }
                else
                    close = true;

                if (close)
                {
                    if (CurrentTerminal != null)
                        CurrentTerminal.Disconnect();
                    
                    if (CurrentConnection != null)
                        CurrentConnection.Disconnect();
                    
                    this.Text = Program.AboutText;
                }
                else
                    cancel = true;
            }
            
            e.Cancel = cancel;
        }
        private void MainForm_Shown(object sender, EventArgs e)
        {
            HandleCommandLineActions();
        }

        private void tcTerminals_TabControlItemSelectionChanged(TabControlItemChangedEventArgs e)
        {
            UpdateControls();
            if (tcTerminals.Items.Count > 0)
            {
                tsbDisconnect.Enabled = e.Item != null;
                disconnectToolStripMenuItem.Enabled = e.Item != null;
                SetGrabInput(true);
                if (e.Item.Selected && Settings.ShowInformationToolTips) this.Text = e.Item.ToolTipText.Replace("\r\n", "; ");
            }
        }

        private void tscConnectTo_TextChanged(object sender, EventArgs e)
        {
            UpdateControls();
        }

        private void tcTerminals_MouseHover(object sender, EventArgs e)
        {
            if (tcTerminals != null && !tcTerminals.ShowTabs)
                timerHover.Enabled = true;
        }

        private void tcTerminals_MouseLeave(object sender, EventArgs e)
        {
            timerHover.Enabled = false;
            if (FullScreen && tcTerminals.ShowTabs && !tcTerminals.MenuOpen)
                tcTerminals.ShowTabs = false;

            if (_currentToolTipItem != null)
                _currentToolTip.Hide(_currentToolTipItem);
        }

        private void tcTerminals_TabControlItemClosed(object sender, EventArgs e)
        {
            this.Text = Program.AboutText;
            if (tcTerminals.Items.Count == 0)
                FullScreen = false;
        }

        private void tcTerminals_DoubleClick(object sender, EventArgs e)
        {
            FullScreen = !_fullScreen;
        }

        private void tsbFullScreen_Click(object sender, EventArgs e)
        {
            FullScreen = !FullScreen;
            UpdateControls();
        }

        private void tcTerminals_MenuItemsLoaded(object sender, EventArgs e)
        {
            foreach (ToolStripItem item in tcTerminals.Menu.Items)
            {
                item.Image = Terminals.Properties.Resources.smallterm;
            }

            if (FullScreen)
            {
                ToolStripSeparator sep = new ToolStripSeparator();
                tcTerminals.Menu.Items.Add(sep);
                ToolStripMenuItem item = new ToolStripMenuItem(Program.Resources.GetString("Restore"), null, tcTerminals_DoubleClick);
                tcTerminals.Menu.Items.Add(item);
                item = new ToolStripMenuItem(Program.Resources.GetString("Minimize"), null, Minimize);
                tcTerminals.Menu.Items.Add(item);
            }
        }

        private void MainForm_Activated(object sender, EventArgs e)
        {
            //put in a check to see if terminals is off the viewing area
            Screen farRightScreen = null;
            foreach (Screen screen in System.Windows.Forms.Screen.AllScreens)
            {
                if (farRightScreen == null)
                    farRightScreen = screen;
                else
                    if (screen.Bounds.X > farRightScreen.Bounds.X) farRightScreen = screen;
            }
            if (this.Location.X > farRightScreen.Bounds.X + farRightScreen.Bounds.Width) this.Location = new Point(0, 0);


            if (FullScreen)
                tcTerminals.ShowTabs = false;
        }
        private void manageConnectionsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ShowManagedConnections();
        }

        private void saveTerminalsAsGroupToolStripMenuItem_Click(object sender, EventArgs e)
        {
            using (NewGroupForm frmNewGroup = new NewGroupForm())
            {
                if (frmNewGroup.ShowDialog() == DialogResult.OK)
                {
                    GroupConfigurationElement serversGroup = new GroupConfigurationElement();
                    serversGroup.Name = frmNewGroup.txtGroupName.Text;
                    foreach (TabControlItem tabControlItem in tcTerminals.Items)
                    {
                        serversGroup.FavoriteAliases.Add(new FavoriteAliasConfigurationElement(tabControlItem.Title));
                    }
                    Settings.AddGroup(serversGroup);
                    LoadGroups();
                }
            }
        }

        private void organizeGroupsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            using (OrganizeGroupsForm frmOrganizeGroups = new OrganizeGroupsForm())
            {
                frmOrganizeGroups.ShowDialog();
                LoadGroups();
            }
        }

        private void tcTerminals_TabControlMouseOnTitle(TabControlMouseOnTitleEventArgs e)
        {
            if (Settings.ShowInformationToolTips)
            {
                //ToolTip
                if ((_currentToolTipItem != null) && (_currentToolTipItem != e.Item))
                {
                    _currentToolTip.Hide(_currentToolTipItem);
                }
                _currentToolTip.ToolTipTitle = Program.Resources.GetString("Connectioninformation");
                _currentToolTip.ToolTipIcon = ToolTipIcon.Info;
                _currentToolTip.UseFading = true;
                _currentToolTip.UseAnimation = true;
                _currentToolTip.IsBalloon = false;
                _currentToolTip.Show(e.Item.ToolTipText, e.Item, (int)e.Item.StripRect.X, 2);
                _currentToolTipItem = e.Item;
            }
        }

        private void tcTerminals_TabControlMouseLeftTitle(TabControlMouseOnTitleEventArgs e)
        {
            if (_currentToolTipItem != null)
            {
                _currentToolTip.Hide(_currentToolTipItem);
            }
            /*if (previewPictureBox != null)
            {
                previewPictureBox.Image.Dispose();
                previewPictureBox.Dispose();
                previewPictureBox = null;
            }*/
        }

        private void tcTerminals_TabControlMouseEnteredTitle(TabControlMouseOnTitleEventArgs e)
        {
            //Picture
            /*previewPictureBox = new PictureBox();
            previewPictureBox.BorderStyle = BorderStyle.FixedSingle;
            previewPictureBox.SizeMode = PictureBoxSizeMode.StretchImage;
            previewPictureBox.Size = new Size(320, 240);
            Image capturedImage = screenCapture.CaptureControl(((TerminalTabControlItem)e.Item).TerminalControl);
            previewPictureBox.Image = capturedImage;
            previewPictureBox.Parent = tcTerminals.SelectedItem;
            previewPictureBox.Location = new Point((int)e.Item.StripRect.X, 2);
            previewPictureBox.BringToFront();
            previewPictureBox.Show();*/
            if (Settings.ShowInformationToolTips)
            {
                /*TerminalTabControlItem item = (TerminalTabControlItem)e.Item;
                previewPictureBox = new PictureBox();
                previewPictureBox.BorderStyle = BorderStyle.FixedSingle;
                previewPictureBox.SizeMode = PictureBoxSizeMode.StretchImage;
                previewPictureBox.Size = new Size(320, 240);
                string fileName = Path.GetTempPath() + e.Item.Title;
                screenCapture.CaptureControl(((TerminalTabControlItem)e.Item).TerminalControl, fileName, ImageFormatHandler.ImageFormatTypes.imgPNG);
                FileStream fileStream = new FileStream(fileName + ".PNG", FileMode.Open, FileAccess.Read);
                previewPictureBox.Image = Image.FromStream(fileStream);
                fileStream.Close();
                previewPictureBox.Parent = item;
                previewPictureBox.Location = new Point((int)e.Item.StripRect.X, 2);
                previewPictureBox.BringToFront();
                previewPictureBox.Show();*/
                /*if ((currentToolTipItem != null) && (currentToolTipItem != e.Item))
                {
                    currentToolTip.Hide(currentToolTipItem);
                }
                currentToolTip.ToolTipTitle = "Connection information";
                currentToolTip.ToolTipIcon = ToolTipIcon.Info;
                currentToolTip.UseFading = true;
                currentToolTip.UseAnimation = true;
                currentToolTip.IsBalloon = false;
                currentToolTip.Show(e.Item.ToolTipText, e.Item, (int)e.Item.StripRect.X, 2);
                currentToolTipItem = e.Item;*/
            }
        }

        private void MainForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (FullScreen) 
                FullScreen = false;
            this.MainWindowNotifyIcon.Visible = false;

            if (tcTerminals.Items.Count > 0)
            {
                if (Settings.ShowConfirmDialog)
                {
                    SaveActiveConnectionsForm frmSaveActiveConnections = new SaveActiveConnectionsForm();
                    if (frmSaveActiveConnections.ShowDialog() == DialogResult.OK)
                    {
                        Settings.ShowConfirmDialog = !frmSaveActiveConnections.chkDontShowDialog.Checked;
                        if (frmSaveActiveConnections.chkOpenOnNextTime.Checked)
                            SaveActiveConnections();
                        e.Cancel = false;
                    }
                    else
                        e.Cancel = true;
                }
                else if (Settings.SaveConnectionsOnClose)
                    SaveActiveConnections();
            }
            SaveWindowState();
        }

        private void timerHover_Tick(object sender, EventArgs e)
        {
            if (timerHover.Enabled)
            {
                timerHover.Enabled = false;
                tcTerminals.ShowTabs = true;
            }
        }

        private void organizeFavoritesToolbarToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OrganizeFavoritesToolbarForm frmOrganizeFavoritesToolbar = new OrganizeFavoritesToolbarForm();
            LoadFavoritesToolbar();
        }

        private void optionsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OptionsForm frmOptions = new OptionsForm(CurrentTerminal);
            if (frmOptions.ShowDialog() == DialogResult.OK)
            {
                this.groupsToolStripMenuItem.Visible = Settings.EnableGroupsMenu;
                HideShowFavoritesPanel(Settings.ShowFavoritePanel);


                this.MainWindowNotifyIcon.Visible = Settings.MinimizeToTray;
                if (!Settings.MinimizeToTray && !this.Visible) this.Visible = true;

                if (Settings.Office2007BlueFeel)
                    ToolStripManager.Renderer = Office2007Renderer.Office2007Renderer.GetRenderer(Office2007Renderer.RenderColors.Blue);
                else if (Settings.Office2007BlackFeel)
                    ToolStripManager.Renderer = Office2007Renderer.Office2007Renderer.GetRenderer(Office2007Renderer.RenderColors.Black);
                else
                    ToolStripManager.Renderer = new System.Windows.Forms.ToolStripProfessionalRenderer();

                tcTerminals.ShowToolTipOnTitle = Settings.ShowInformationToolTips;
                if (tcTerminals.SelectedItem != null)
                {
                    tcTerminals.SelectedItem.ToolTipText = GetToolTipText(((TerminalTabControlItem)tcTerminals.SelectedItem).Favorite);
                }
            }
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
            HideShowFavoritesPanel(tsbTags.Checked);
        }

        private void pbShowTags_Click(object sender, EventArgs e)
        {
            HideShowFavoritesPanel(true);
        }

        private void pbHideTags_Click(object sender, EventArgs e)
        {
            HideShowFavoritesPanel(false);
        }
        private void lvTags_SelectedIndexChanged(object sender, EventArgs e)
        {
            //connectToolStripMenuItem.Enabled = lvTags.SelectedItems.Count > 0;
            //lvTagConnections.Items.Clear();
            //lvTagConnections.ShowItemToolTips = true;
            //if(lvTags.SelectedItems.Count > 0)
            //{
            //    List<FavoriteConfigurationElement> tagFavorites = (List<FavoriteConfigurationElement>)lvTags.SelectedItems[0].Tag;
            //    foreach(FavoriteConfigurationElement favorite in tagFavorites)
            //    {
            //        ListViewItem item = lvTagConnections.Items.Add(favorite.Name);
            //        item.ImageIndex = 0;
            //        item.StateImageIndex = 0;
            //        item.Tag = favorite;                    
            //        item.ToolTipText = favorite.Notes;
            //    }
            //}
            //lvTagConnections.AutoResizeColumns(ColumnHeaderAutoResizeStyle.ColumnContent);
        }

        private void lvTagConnections_SelectedIndexChanged(object sender, EventArgs e)
        {
            //connectToolStripMenuItem.Enabled = lvTagConnections.SelectedItems.Count > 0;
        }

        private void connectToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //foreach(ListViewItem item in lvTagConnections.SelectedItems)
            //{
            //    FavoriteConfigurationElement favorite = (FavoriteConfigurationElement)item.Tag;
            //    HideTagsFavorites();
            //    Connect(favorite.Name, false);
            //}
        }

        private void connectToAllToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //foreach(ListViewItem item in lvTagConnections.Items)
            //{
            //    FavoriteConfigurationElement favorite = (FavoriteConfigurationElement)item.Tag;
            //    HideTagsFavorites();
            //    Connect(favorite.Name, false);
            //}
        }

        private void txtSearchTags_TextChanged(object sender, EventArgs e)
        {
            //LoadTags(txtSearchTags.Text);
        }

        private void lvTags_KeyDown(object sender, KeyEventArgs e)
        {
            //if((e.KeyCode == Keys.Enter) && (lvTags.SelectedItems.Count == 1))
            //{
            //    connectToAllToolStripMenuItem.PerformClick();
            //}
        }

        private void lvTagConnections_KeyDown(object sender, KeyEventArgs e)
        {
            //if((e.KeyCode == Keys.Enter) && (lvTagConnections.SelectedItems.Count == 1))
            //{
            //    connectToolStripMenuItem.PerformClick();
            //}
        }

        private void lvTags_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            //if(lvTags.SelectedItems.Count == 1)
            //{
            //    connectToAllToolStripMenuItem.PerformClick();
            //}
        }

        private void lvTagConnections_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            //if(lvTagConnections.SelectedItems.Count == 1)
            //{
            //    connectToolStripMenuItem.PerformClick();
            //}
        }

        private void tsbFavorites_Click(object sender, EventArgs e)
        {
            Settings.EnableFavoritesPanel = tsbFavorites.Checked;
            HideShowFavoritesPanel(Settings.ShowFavoritePanel);
        }

        private void txtSearchFavorites_TextChanged(object sender, EventArgs e)
        {
            //LoadFavorites(txtSearchFavorites.Text);
        }

        private void lvFavorites_KeyDown(object sender, KeyEventArgs e)
        {
            //if((e.KeyCode == Keys.Enter) && (lvFavorites.SelectedItems.Count == 1))
            //{
            //    connectToolStripMenuItem1.PerformClick();
            //}
        }

        private void lvFavorites_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            //if(lvFavorites.SelectedItems.Count == 1)
            //{
            //    connectToolStripMenuItem1.PerformClick();
            //}
        }

        private void connectToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            //foreach(ListViewItem item in lvFavorites.SelectedItems)
            //{
            //    FavoriteConfigurationElement favorite = (FavoriteConfigurationElement)item.Tag;
            //    HideTagsFavorites();
            //    Connect(favorite.Name, false);
            //}
        }

        private void lvFavorites_SelectedIndexChanged(object sender, EventArgs e)
        {
            //connectToolStripMenuItem1.Enabled = lvFavorites.SelectedItems.Count > 0;
        }
        private void MainForm_Resize(object sender, EventArgs e)
        {
            if (this.WindowState == FormWindowState.Minimized)
            {
                if (Settings.MinimizeToTray) this.Visible = false;
            }
            else
            {
                _originalFormWindowState = this.WindowState;
            }
        }
        private void Minimize(object sender, EventArgs e)
        {
            _originalFormWindowState = this.WindowState;
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
                        _originalFormWindowState = this.WindowState;
                        this.WindowState = FormWindowState.Minimized;
                    }
                    else
                    {
                        this.WindowState = _originalFormWindowState;
                    }
                }
            }
        }

        private void toolStripMenuItem2_Click(object sender, EventArgs e)
        {
            this.Visible = true;
            Close();
        }

        private void newConnectionToolStripMenuItem_Click(object sender, EventArgs e)
        {
            manageConnectionsToolStripMenuItem_Click(null, null);
        }

        private void showToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Visible = !this.Visible;
            showToolStripMenuItem.Text = Program.Resources.GetString("HideShow");
        }

        private void CaptureScreenToolStripButton_Click(object sender, EventArgs e)
        {
            Terminals.CaptureManager.Capture cap = Terminals.CaptureManager.CaptureManager.PerformScreenCapture(this.tcTerminals);

            toolStripMenuItem5_Click(null, null);

        }

        private void captureTerminalScreenToolStripMenuItem_Click(object sender, EventArgs e)
        {
            CaptureScreenToolStripButton_Click(null, null);
        }

        private void VMRCAdminSwitchButton_Click(object sender, EventArgs e)
        {
            if (CurrentConnection != null)
            {
                Connections.VMRCConnection vmrc;
                vmrc = (CurrentConnection as Connections.VMRCConnection);
                if (vmrc != null)
                {
                    vmrc.AdminDisplay();
                }
            }
        }

        private void VMRCViewOnlyButton_Click(object sender, EventArgs e)
        {
            if (CurrentConnection != null)
            {
                Connections.VMRCConnection vmrc;
                vmrc = (CurrentConnection as Connections.VMRCConnection);
                if (vmrc != null)
                {
                    vmrc.ViewOnlyMode = !vmrc.ViewOnlyMode;
                }
            }
            UpdateControls();
        }

        private void SystemTrayContextMenuStrip_Opening(object sender, CancelEventArgs e)
        {

            SystemTrayQuickConnectToolStripMenuItem.DropDownItems.Clear();
            SortedDictionary<string, FavoriteConfigurationElement> favorites = Settings.GetSortedFavorites(Settings.DefaultSortProperty);

            foreach (string key in favorites.Keys)
            {
                FavoriteConfigurationElement favorite = favorites[key];
                SystemTrayQuickConnectToolStripMenuItem.DropDownItems.Add(favorite.Name);
            }
        }

        private void toolStripButton1_Click(object sender, EventArgs e)
        {
            Cursor = Cursors.WaitCursor;
            try
            {
                string sessionId = String.Empty;
                if (!CurrentTerminal.AdvancedSettings3.ConnectToServerConsole)
                {
                    sessionId = TSManager.GetCurrentSession(CurrentTerminal.Server,
                    CurrentTerminal.UserName, CurrentTerminal.Domain,
                    Environment.MachineName).Id.ToString();
                }
                Process process = new Process();
                string args = " \\\\" + CurrentTerminal.Server +
                    " -i " + sessionId + " -d notepad";
                ProcessStartInfo startInfo = new ProcessStartInfo(Settings.PsexecLocation,
                    args);
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
                string sessionId = String.Empty;
                if (!CurrentTerminal.AdvancedSettings3.ConnectToServerConsole)
                {
                    sessionId = TSManager.GetCurrentSession(CurrentTerminal.Server,
                    CurrentTerminal.UserName, CurrentTerminal.Domain,
                    Environment.MachineName).Id.ToString();
                }
                Process process = new Process();
                string args = " \\\\" + CurrentTerminal.Server +
                    " -i " + sessionId + " -d cmd";
                ProcessStartInfo startInfo = new ProcessStartInfo(Settings.PsexecLocation,
                    args);
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

        private void toolStripContainer_TopToolStripPanel_MouseClick(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                //contextMenuStrip1.Show(toolStripContainer, e.X, e.Y);
            }
        }

        private void manageToolbarToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ManageToolStrip mgr = new ManageToolStrip();
            mgr.ShowDialog(this);
            this.favsList1.LoadFavs();
        }

        private void standardToolbarToolStripMenuItem_Click(object sender, EventArgs e)
        {
            AddShowStrip(toolbarStd, standardToolbarToolStripMenuItem, !toolbarStd.Visible);
        }

        private void toolStripMenuItem4_Click(object sender, EventArgs e)
        {
            AddShowStrip(favoriteToolBar, toolStripMenuItem4, !favoriteToolBar.Visible);
        }

        private void shortcutsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            AddShowStrip(SpecialCommandsToolStrip, shortcutsToolStripMenuItem,  !SpecialCommandsToolStrip.Visible);
        }

        private void menubarToolStripMenuItem_Click(object sender, EventArgs e)
        {
            AddShowStrip(menuStrip, menubarToolStripMenuItem, !menuStrip.Visible);
        }

        private void AddShowStrip(ToolStrip strip, ToolStripMenuItem menu, bool visible)
        {
            if (!Settings.ToolbarsLocked)
            {
                strip.Visible = visible;
                menu.Checked = visible;
            }
            else
                System.Windows.Forms.MessageBox.Show(Program.Resources.GetString("Inordertochangethetoolbarsyoumustfirstunlockthem"));                        
        }

        private void toolsToolStripMenuItem_DropDownOpening(object sender, EventArgs e)
        {
            this.shortcutsToolStripMenuItem.Checked = this.SpecialCommandsToolStrip.Visible;
            this.toolStripMenuItem4.Checked = this.favoriteToolBar.Visible;
            this.standardToolbarToolStripMenuItem.Checked = this.toolbarStd.Visible;

        }

        private void toolStripMenuItem3_Click(object sender, EventArgs e)
        {
            OrganizeShortcuts org = new OrganizeShortcuts();
            org.ShowDialog(this);
            if (Settings.EnableFavoritesPanel) LoadTags(null);
            this.Invoke(_specialCommandsMIV);
        }

        private void ShortcutsContextMenu_MouseClick(object sender, MouseEventArgs e)
        {
            toolStripMenuItem3_Click(null, null);
        }

        private void SpecialCommandsToolStrip_MouseClick(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right) ShortcutsContextMenu.Show(e.X, e.Y);
        }

        private void SpecialCommandsToolStrip_ItemClicked_1(object sender, ToolStripItemClickedEventArgs e)
        {
            SpecialCommandConfigurationElement elm = (e.ClickedItem.Tag as SpecialCommandConfigurationElement);
            if (elm != null) elm.Launch();
        }

        private void toolStripButton2_Click(object sender, EventArgs e)
        {
            OpenNetworkingTools(null, null);
        }

        private void networkingToolsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            toolStripButton2_Click(null, null);
        }

        private void toolStripMenuItem5_Click(object sender, EventArgs e)
        {
            bool createNew = true;
            foreach (TerminalTabControlItem tab in tcTerminals.Items)
            {
                if (tab.Title == Program.Resources.GetString("CaptureManager"))
                {
                    Terminals.Connections.CaptureManagerConnection conn = (tab.Connection as Terminals.Connections.CaptureManagerConnection);
                    conn.RefreshView();
                    if (Settings.AutoSwitchOnCapture) tcTerminals.SelectedItem = tab;
                    createNew = false;
                    break;
                }
            }
            if (createNew)
            {
                if (sender == null && !Settings.AutoSwitchOnCapture) createNew = false;
            }
            if (createNew)
            {
                TerminalTabControlItem terminalTabPage = new TerminalTabControlItem(Program.Resources.GetString("CaptureManager"));
                try
                {
                    terminalTabPage.AllowDrop = false;
                    terminalTabPage.ToolTipText = Program.Resources.GetString("CaptureManager");
                    terminalTabPage.Favorite = null;
                    terminalTabPage.DoubleClick += new EventHandler(terminalTabPage_DoubleClick);
                    tcTerminals.Items.Add(terminalTabPage);
                    tcTerminals.SelectedItem = terminalTabPage;
                    tcTerminals_SelectedIndexChanged(this, EventArgs.Empty);
                    Connections.IConnection conn = new Terminals.Connections.CaptureManagerConnection();
                    conn.TerminalTabPage = terminalTabPage;
                    conn.ParentForm = this;
                    conn.Connect();
                    (conn as Control).BringToFront();
                    (conn as Control).Update();
                    UpdateControls();
                }
                catch (Exception exc)
                {
                    Terminals.Logging.Log.Info("", exc);
                    tcTerminals.Items.Remove(terminalTabPage);
                    tcTerminals.SelectedItem = null;
                    terminalTabPage.Dispose();
                }
            }
        }

        private void toolStripButton3_Click(object sender, EventArgs e)
        {
            bool origval = Settings.AutoSwitchOnCapture;
            if (!Settings.AutoSwitchOnCapture)
            {
                Settings.AutoSwitchOnCapture = true;
            }
            toolStripMenuItem5_Click(new object(), null);
            Settings.AutoSwitchOnCapture = origval;
        }

        private void pingToolStripMenuItem_Click_1(object sender, EventArgs e)
        {
            //if(lvTagConnections.SelectedItems != null && lvTagConnections.SelectedItems.Count > 0)
            //{
            //    FavoriteConfigurationElement fav = (lvTagConnections.SelectedItems[0].Tag as FavoriteConfigurationElement);
            //    if(fav != null)
            //    {
            //        string host = fav.ServerName;
            //        string action = "Ping";
            //        this.OpenNetworkingTools(action, host);
            //    }
            //}
        }

        private void cmsTagConnections_Opening(object sender, CancelEventArgs e)
        {
            //bool itemSelected = (lvTagConnections.SelectedItems != null && lvTagConnections.SelectedItems.Count > 0);
            //pingToolStripMenuItem.Visible = itemSelected;
            //dNSToolStripMenuItem.Visible = itemSelected;
            //traceRouteToolStripMenuItem.Visible = itemSelected;
            //tSAdminToolStripMenuItem.Visible = itemSelected;
        }

        private void dNSToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //if(lvTagConnections.SelectedItems != null && lvTagConnections.SelectedItems.Count > 0)
            //{
            //    FavoriteConfigurationElement fav = (lvTagConnections.SelectedItems[0].Tag as FavoriteConfigurationElement);
            //    if(fav != null)
            //    {
            //        string host = fav.ServerName;
            //        if(host.ToLower().StartsWith("www."))
            //        {
            //            host = host.Substring(4);
            //        }
            //        string action = "DNS";
            //        this.OpenNetworkingTools(action, host);
            //    }
            //}
        }

        private void traceRouteToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //if(lvTagConnections.SelectedItems != null && lvTagConnections.SelectedItems.Count > 0)
            //{
            //    FavoriteConfigurationElement fav = (lvTagConnections.SelectedItems[0].Tag as FavoriteConfigurationElement);
            //    if(fav != null)
            //    {
            //        string host = fav.ServerName;
            //        string action = "Trace";
            //        this.OpenNetworkingTools(action, host);
            //    }
            //}
        }

        private void tSAdminToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //if(lvTagConnections.SelectedItems != null && lvTagConnections.SelectedItems.Count > 0)
            //{
            //    FavoriteConfigurationElement fav = (lvTagConnections.SelectedItems[0].Tag as FavoriteConfigurationElement);
            //    if(fav != null)
            //    {
            //        string host = fav.ServerName;
            //        string action = "TSAdmin";
            //        this.OpenNetworkingTools(action, host);
            //    }
            //}
        }

        private void sendALTKeyToolStripMenuItem_Click(object sender, EventArgs e)
        {

            if (sender != null && (sender as ToolStripMenuItem) != null)
            {
                string key = (sender as ToolStripMenuItem).Text;
                Connections.VNCConnection vnc;
                if (CurrentConnection != null)
                {
                    vnc = (this.CurrentConnection as Connections.VNCConnection);
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

        private void propertiesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //if(lvTagConnections.SelectedItems != null && lvTagConnections.SelectedItems.Count > 0)
            //{
            //    FavoriteConfigurationElement fav = (lvTagConnections.SelectedItems[0].Tag as FavoriteConfigurationElement);
            //    if(fav != null)
            //    {
            //        using(NewTerminalForm frmNewTerminal = new NewTerminalForm(fav))
            //        {
            //            frmNewTerminal.ShowDialog();
            //            LoadFavorites();
            //        }
            //    }
            //}
        }

        private void toolStripButton4_Click(object sender, EventArgs e)
        {
            if (tcTerminals.SelectedItem != null)
            {
                TerminalTabControlItem terminalTabPage = (TerminalTabControlItem)tcTerminals.SelectedItem;
                if (terminalTabPage.Connection != null)
                {
                    terminalTabPage.Connection.ChangeDesktopSize(terminalTabPage.Connection.Favorite.DesktopSize);
                }
            }
        }

        private void pbShowTagsFavorites_MouseMove(object sender, MouseEventArgs e)
        {
            if (Settings.AutoExapandTagsPanel) HideShowFavoritesPanel(true);
        }

        private void TerminalServerMenuButton_DropDownOpening(object sender, EventArgs e)
        {
            BuildTerminalServerButtonMenu();
        }

        private void lvTags_ItemDrag(object sender, ItemDragEventArgs e)
        {

        }

        private void lvTagConnections_ItemDrag(object sender, ItemDragEventArgs e)
        {
            //lvTagConnections.DoDragDrop(lvTagConnections.SelectedItems, DragDropEffects.Move);
        }

        private void lvTags_DragEnter(object sender, DragEventArgs e)
        {

            foreach (string format in e.Data.GetFormats())
            {
                if (format.Equals("System.Windows.Forms.ListView+SelectedListViewItemCollection"))
                {
                    e.Effect = DragDropEffects.Move;
                }
            }
        }

        private void lvTags_DragDrop(object sender, DragEventArgs e)
        {

            //bool needsUpdate = false;
            ////Returns the location of the mouse pointer in the ListView control.
            //Point p = lvTags.PointToClient(new Point(e.X, e.Y));
            ////Obtain the item that is located at the specified location of the mouse pointer.
            ////dragItem is the lvi upon which the drop is made
            //ListViewItem dragToItem = lvTags.GetItemAt(p.X, p.Y);
            //if(dragToItem != null && !String.IsNullOrEmpty(dragToItem.ToolTipText))
            //{

            //    foreach(ListViewItem item in lvTagConnections.SelectedItems)
            //    {
            //        FavoriteConfigurationElement fav = (item.Tag as FavoriteConfigurationElement);
            //        if(fav != null)
            //        {
            //            if(lvTags.SelectedItems != null && lvTags.SelectedItems.Count > 0)
            //            {
            //                string tag = lvTags.SelectedItems[0].ToolTipText;
            //                RemoveTagFromFavorite(fav, tag);
            //            }
            //            if(dragToItem.ToolTipText != "UnTagged") AddTagToFavorite(fav, dragToItem.ToolTipText);
            //            Settings.DeleteFavorite(fav.Name);
            //            Settings.AddFavorite(fav, false);
            //            needsUpdate = true;
            //        }
            //    }
            //}
            //if(needsUpdate)
            //{
            //    ListViewItem sel = null;
            //    if(lvTags.SelectedItems != null && lvTags.SelectedItems.Count > 0)
            //    {
            //        sel = lvTags.SelectedItems[0];
            //    }
            //    LoadTags(null);
            //    foreach(ListViewItem item in lvTags.Items)
            //    {
            //        if(item.ToolTipText == sel.ToolTipText)
            //        {
            //            item.Selected = true;
            //        }
            //    }
            //}
        }

        private void lockToolbarsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SaveWindowState();
            Settings.ToolbarsLocked = !lockToolbarsToolStripMenuItem.Checked;
            lockToolbarsToolStripMenuItem.Checked = Settings.ToolbarsLocked;

            bool GripVisible = !Settings.ToolbarsLocked;
            foreach (ToolStripPanelRow row in toolStripContainer.TopToolStripPanel.Rows)
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
        private void MainForm_Load(object sender, EventArgs e)
        {
            _releaseMIV = new MethodInvoker(OpenReleasePage);
            this.Text = Program.AboutText;
            MainForm.OnReleaseIsAvailable += new ReleaseIsAvailable(MainForm_OnReleaseIsAvailable);
        }

        private void MainForm_OnReleaseIsAvailable(FavoriteConfigurationElement ReleaseFavorite)
        {
            this.Invoke(_releaseMIV);
        }

        private void updateToolStripItem_Click(object sender, EventArgs e)
        {
        }

        private void helpToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (!updateToolStripItem.Visible)
            {
                if (ReleaseAvailable && updateToolStripItem != null)
                {
                    updateToolStripItem.Visible = ReleaseAvailable;
                    if (ReleaseDescription != null)
                    {
                        updateToolStripItem.Text = string.Format("{0} - {1}", updateToolStripItem.Text, ReleaseDescription.Title);
                    }
                }
            }
        }

        private void MainForm_KeyUp(object sender, KeyEventArgs e)
        {
            //handle global keyup events
            if (e.Control && e.KeyCode == Keys.F12)
            {
                Terminals.CaptureManager.Capture cap = Terminals.CaptureManager.CaptureManager.PerformScreenCapture(this.tcTerminals);
                toolStripMenuItem5_Click(null, null);
            }
            else if (e.KeyCode == Keys.F4)
            {
                tscConnectTo.Focus();
            }
        }

        private void toolStripButton5_Click(object sender, EventArgs e)
        {
            System.Diagnostics.Process.Start("mmc.exe", "compmgmt.msc /a /computer=.");
        }

        private void rebuildTagsIndexToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Settings.RebuildTagIndex();
            LoadFavorites();
            LoadGroups();
            UpdateControls();
            LoadTags("");
        }

        private void viewInNewWindowToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (this.CurrentConnection != null)
            {
                OpenConnectionInNewWindow(this.CurrentConnection);
            }
        }
        private void rebuildShortcutsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Settings.SpecialCommands.Clear();
            Settings.SpecialCommands = Terminals.Wizard.SpecialCommandsWizard.LoadSpecialCommands();            
            this.Invoke(_specialCommandsMIV);
        }

        private void rebuildToolbarsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            LoadWindowState();
        }

        private void openLogFolderToolStripMenuItem_Click(object sender, EventArgs e)
        {
            System.Diagnostics.Process.Start(System.IO.Path.Combine(System.IO.Path.GetDirectoryName(Application.ExecutablePath),"Logs"));
            
        }

        private void splitContainer1_SplitterMoved(object sender, SplitterEventArgs e)
        {
            if(splitContainer1.Panel1.Width>15) Settings.FavoritePanelWidth = splitContainer1.Panel1.Width;            
        }

        private void credentialManagementToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ShowCredentialsManager();
        }

        private void credentialsManagerToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ShowCredentialsManager();
        }

        private void CredentialManagementToolStripButton_Click(object sender, EventArgs e)
        {
            ShowCredentialsManager();
        }        

        private void closeSelectedConnectionToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void exportImportConnectionsListToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Export ei = new Export();
            ei.Show();
        }

        private void showInDualScreensToolStripMenuItem_Click(object sender, EventArgs e)
        {                        
            Screen[] screenArr = Screen.AllScreens;
            int with = 0;
            if (!_allScreens)
            {
                if (this.WindowState == FormWindowState.Maximized)
                    this.WindowState = FormWindowState.Normal;
                foreach (Screen screen in screenArr)
                {
                    with += screen.Bounds.Width;
                }
                showInDualScreensToolStripMenuItem.Text = "Show in Single Screen";
                this.BringToFront();
            }
            else
            {
                with = Screen.PrimaryScreen.Bounds.Width;
                showInDualScreensToolStripMenuItem.Text = "Show In Multi Screens";
            }

            this.Top = 0;
            this.Left = 0;
            this.Height = Screen.PrimaryScreen.Bounds.Height;
            this.Width = with;
            _allScreens = !_allScreens;
        }
        #endregion
    }

    public class TerminalTabControlItem : TabControlItem {
        public TerminalTabControlItem(string caption)
            : base(caption, null) {
        }
        private Connections.IConnection _connection;
        private AxMsRdpClient2 _terminalControl;
        private FavoriteConfigurationElement _favorite;

        public Connections.IConnection Connection {
            get {
                return _connection;
            }
            set {
                _connection = value;
            }
        }
        
        public AxMsRdpClient2 TerminalControl {
            get {
                return _terminalControl;
            }
            set {
                _terminalControl = value;
            }
        }        

        public FavoriteConfigurationElement Favorite {
            get {
                return _favorite;
            }
            set {
                _favorite = value;
            }
        }
    }
}
