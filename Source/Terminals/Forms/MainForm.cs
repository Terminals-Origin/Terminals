using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using System.Windows.Forms;
using TabControl;
using Terminals.Data;
using Terminals.Forms;
using Terminals.Forms.Controls;
using Terminals.Forms.Rendering;
using Terminals.CommandLine;
using Terminals.Configuration;
using Terminals.Connections;
using Terminals.Credentials;
using Terminals.Native;
using Terminals.Network.Servers;
using Terminals.Updates;
using Settings = Terminals.Configuration.Settings;

namespace Terminals
{
    internal partial class MainForm : Form
    {
        private readonly IPersistence persistence;

        private readonly Settings settings = Settings.Instance;

        #region Declarations

        private const String FULLSCREEN_ERROR_MSG = "Screen properties not available for RDP";

        private FavsList favsList1;

        private readonly FormSettings formSettings;
        private TabControlItem currentToolTipItem;
        private ToolTip currentToolTip;
        private Boolean allScreens;
        private readonly TerminalTabsSelectionControler terminalsControler;
        private readonly FavoritesMenuLoader menuLoader;
        private readonly MainFormFullScreenSwitch fullScreenSwitch;

        private readonly ConnectionsUiFactory connectionsUiFactory;

        #endregion

        #region Properties

        private IFavorites PersistedFavorites
        {
            get { return this.persistence.Favorites; }
        }

        private IConnection CurrentConnection
        {
            get
            {
                if (this.terminalsControler.HasSelected)
                    return this.terminalsControler.Selected.Connection;

                return null;
            }
        }

        /// <summary>
        /// Get or set wether the MainForm window is in fullscreen mode.
        /// </summary>
        internal bool FullScreen
        { 
            private get { return this.fullScreenSwitch.FullScreen; }
            set { this.fullScreenSwitch.FullScreen = value; }
        }

        /// <summary>
        /// Gets wether the MainForm window is switching to or from fullscreen mode.
        /// </summary>
        internal Boolean SwitchingFullScreen
        {
            get { return this.fullScreenSwitch.SwitchingFullScreen; } 
        }

        private IConnectionExtra CurrentTerminal
        {
            get
            {
                return this.CurrentConnection as IConnectionExtra;
            }
        }

        #endregion

        #region Protected overrides

        protected override void SetVisibleCore(Boolean value)
        {
            this.formSettings.LoadFormSize();
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
                        if (r.Contains(MousePosition))
                        {
                            SetGrabInput(true);
                        }
                        else
                        {
                            TabControlItem item = tcTerminals.GetTabItemByPoint(tcTerminals.PointToClient(MousePosition));
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
                else if (msg.Msg == Methods.WM_LEAVING_FULLSCREEN)
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
                Logging.Info("WnProc Failure", e);
            }
        }

        #endregion

        #region Constuctors

        public MainForm(IPersistence persistence)
        {
            try
            {
                this.persistence = persistence;
                settings.StartDelayedUpdate();

                // Set default font type by Windows theme to use for all controls on form
                this.Font = SystemFonts.IconTitleFont;

                InitializeComponent(); // main designer procedure

                this.formSettings = new FormSettings(this);

                this.terminalsControler = new TerminalTabsSelectionControler(this.tcTerminals, this.persistence);
                this.connectionsUiFactory = new ConnectionsUiFactory(this, this.terminalsControler, this.persistence);
                this.terminalsControler.AssingUiFactory(this.connectionsUiFactory);

                // Initialize FavsList outside of InitializeComponent
                // Inside InitializeComponent it sometimes caused the design view in VS to return errors
                this.InitializeFavsListControl();

                // Set notifyicon icon from embedded png image
                this.MainWindowNotifyIcon.Icon = Icon.FromHandle(Properties.Resources.terminalsicon.GetHicon());
                this.menuLoader = new FavoritesMenuLoader(this, this.persistence);
                this.favoriteToolBar.Visible = this.toolStripMenuItemShowHideFavoriteToolbar.Checked;
                this.fullScreenSwitch = new MainFormFullScreenSwitch(this);
                this.favsList1.Persistence = this.persistence;
                this.AssignToolStripsToContainer();
                this.ApplyControlsEnableAndVisibleState();
                
                this.menuLoader.LoadGroups();
                this.UpdateControls();
                this.LoadWindowState();                
                this.CheckForMultiMonitorUse();

                this.tcTerminals.TabControlItemDetach += new TabControlItemChangedHandler(this.TcTerminals_TabDetach);
                this.tcTerminals.MouseClick += new MouseEventHandler(this.TcTerminals_MouseClick);

                this.QuickContextMenu.ItemClicked += new ToolStripItemClickedEventHandler(QuickContextMenu_ItemClicked);
                this.LoadSpecialCommands();

                ProtocolHandler.Register();
                this.persistence.AssignSynchronizationObject(this);
            }
            catch (Exception exc)
            {
                Logging.Error("Error loading the Main Form", exc);
                throw;
            }
        }

        private void InitializeFavsListControl()
        {
            this.favsList1 = new FavsList();
            this.pnlTagsFavorites.Controls.Add(this.favsList1);
            this.favsList1.Dock = DockStyle.Fill;
            this.favsList1.Location = new Point(5, 0);
            this.favsList1.Padding = new Padding(4, 4, 4, 4);
            this.favsList1.Name = "favsList1";
            this.favsList1.Size = new Size(200, 497);
            this.favsList1.TabIndex = 2;
            this.favsList1.ConnectionsUiFactory = this.connectionsUiFactory;
        }

        private void ApplyControlsEnableAndVisibleState()
        {
            this.MainWindowNotifyIcon.Visible = settings.MinimizeToTray;
            if (!settings.MinimizeToTray && !this.Visible)
                this.Visible = true;

            this.lockToolbarsToolStripMenuItem.Checked = settings.ToolbarsLocked;
            this.MainMenuStrip.GripStyle = settings.ToolbarsLocked ? ToolStripGripStyle.Hidden : ToolStripGripStyle.Visible;

            this.tcTerminals.ShowToolTipOnTitle = settings.ShowInformationToolTips;
            if (this.terminalsControler.HasSelected)
                this.terminalsControler.Selected.ToolTipText = this.terminalsControler.Selected.Favorite.GetToolTipText();

            this.groupsToolStripMenuItem.Visible = settings.EnableGroupsMenu;
            this.tsbTags.Checked = settings.ShowFavoritePanel;
            this.pnlTagsFavorites.Width = 7;

            this.HideShowFavoritesPanel(settings.ShowFavoritePanel);
            this.UpdateCaptureButtonEnabled();
            this.ApplyTheme();
        }

        private void ApplyTheme()
        {
            if (settings.Office2007BlueFeel)
                ToolStripManager.Renderer = Office2007Renderer.GetRenderer(RenderColors.Blue);
            else if (settings.Office2007BlackFeel)
                ToolStripManager.Renderer = Office2007Renderer.GetRenderer(RenderColors.Black);
            else
                ToolStripManager.Renderer = new ToolStripProfessionalRenderer();

            // Update the old treeview theme to the new theme from Win Vista and up
            Methods.SetWindowTheme(this.menuStrip.Handle, "Explorer", null);
        }

        /// <summary>
        /// Assignes toolbars and menu items to toolstrip container.
        /// They arent moved to the container because of designer
        /// </summary>
        private void AssignToolStripsToContainer()
        {
            this.toolStripContainer.ToolbarStd = this.toolbarStd;
            this.toolStripContainer.StandardToolbarToolStripMenuItem = this.standardToolbarToolStripMenuItem;
            this.toolStripContainer.FavoriteToolBar = this.favoriteToolBar;
            this.toolStripContainer.ToolStripMenuItemShowHideFavoriteToolbar = this.toolStripMenuItemShowHideFavoriteToolbar;
            this.toolStripContainer.SpecialCommandsToolStrip = this.SpecialCommandsToolStrip;
            this.toolStripContainer.ShortcutsToolStripMenuItem = this.shortcutsToolStripMenuItem;
            this.toolStripContainer.MenuStrip = this.menuStrip;
            this.toolStripContainer.TsRemoteToolbar = this.tsRemoteToolbar;
            this.toolStripContainer.AssignToolStripsLocationChangedEventHandler();
        }

        #endregion

        #region Public methods

        private void LoadWindowState()
        {
            this.AssingTitle();
            this.HideShowFavoritesPanel(settings.ShowFavoritePanel);
            this.toolStripContainer.LoadToolStripsState();
        }

        internal void UpdateControls()
        {
            tcTerminals.ShowToolTipOnTitle = settings.ShowInformationToolTips;
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
                Logging.Error(FULLSCREEN_ERROR_MSG, exc);
            }

            grabInputToolStripMenuItem.Checked = tsbGrabInput.Checked;
            tsbConnect.Enabled = (tscConnectTo.Text != String.Empty);
            tsbConnectToConsole.Enabled = (tscConnectTo.Text != String.Empty);
            tsbConnectAs.Enabled = (tscConnectTo.Text != String.Empty);
            saveTerminalsAsGroupToolStripMenuItem.Enabled = (tcTerminals.Items.Count > 0);

            this.TerminalServerMenuButton.Visible = false;
            vncActionButton.Visible = false;
            VMRCAdminSwitchButton.Visible = false;
            VMRCViewOnlyButton.Visible = false;

            if (CurrentConnection != null)
            {
                var vmrc = this.CurrentConnection as VMRCConnection;
                if (vmrc != null)
                {
                    VMRCAdminSwitchButton.Visible = true;
                    VMRCViewOnlyButton.Visible = true;
                }

                var vnc = this.CurrentConnection as VNCConnection;
                if (vnc != null)
                {
                    vncActionButton.Visible = true;
                }

                this.TerminalServerMenuButton.Visible = this.CurrentConnection.IsTerminalServer;
            }
        }

        public String GetDesktopShare()
        {
            String desktopShare = this.terminalsControler.Selected.Favorite.DesktopShare;
            if (String.IsNullOrEmpty(desktopShare))
            {
                desktopShare = settings.DefaultDesktopShare.Replace("%SERVER%", CurrentTerminal.Server)
                                                                    .Replace("%USER%", CurrentTerminal.UserName)
                                                                    .Replace("%server%", CurrentTerminal.Server)
                                                                    .Replace("%user%", CurrentTerminal.UserName);
            }

            return desktopShare;
        }

        internal void SendNativeMessageToFocus()
        {
            if (!this.Visible)
            {
                this.Show();
                if (this.WindowState == FormWindowState.Minimized)
                    Methods.ShowWindow(new HandleRef(this, this.Handle), 9);

                Methods.SetForegroundWindow(new HandleRef(this, this.Handle));
            }
        }

        private void ToggleGrabInput()
        {
            if (CurrentTerminal != null)
            {
                CurrentTerminal.FullScreen = !CurrentTerminal.FullScreen;
            }
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
                    this.allScreens = true;
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
            if (settings.RestoreWindowOnLastTerminalDisconnect)
            {
                if (this.tcTerminals.Items.Count == 0)
                    this.FullScreen = false;
            }
        }

        internal void RemoveTabPage(TabControlItem tabControlToRemove)
        {
            this.tcTerminals.RemoveTab(tabControlToRemove);
            this.CloseTabControlItem();
        }

        internal void AssignEventsToConnectionTab(TerminalTabControlItem terminalTabPage)
        {
            terminalTabPage.DragOver += this.TerminalTabPage_DragOver;
            terminalTabPage.DragEnter += new DragEventHandler(this.terminalTabPage_DragEnter);
            terminalTabPage.Resize += new EventHandler(terminalTabPage_Resize);
        }

        internal void AssingDoubleClickEventHandler(TerminalTabControlItem terminalTabPage)
        {
            terminalTabPage.DoubleClick += new EventHandler(this.TerminalTabPage_DoubleClick);
        }

        private void BuildTerminalServerButtonMenu()
        {
            TerminalServerMenuButton.DropDownItems.Clear();

            if (this.CurrentConnection != null && this.CurrentConnection.IsTerminalServer)
            {
                var sessions = new ToolStripMenuItem(Program.Resources.GetString("Sessions"));
                sessions.Tag = this.CurrentConnection.Server;
                TerminalServerMenuButton.DropDownItems.Add(sessions);
                var svr = new ToolStripMenuItem(Program.Resources.GetString("Server"));
                svr.Tag = this.CurrentConnection.Server;
                TerminalServerMenuButton.DropDownItems.Add(svr);
                var sd = new ToolStripMenuItem(Program.Resources.GetString("Shutdown"));
                sd.Click += new EventHandler(sd_Click);
                sd.Tag = this.CurrentConnection.Server;
                svr.DropDownItems.Add(sd);
                var rb = new ToolStripMenuItem(Program.Resources.GetString("Reboot"));
                rb.Click += new EventHandler(sd_Click);
                rb.Tag = this.CurrentConnection.Server;
                svr.DropDownItems.Add(rb);

                if (this.CurrentConnection.Server.Sessions != null)
                {
                    foreach (TerminalServices.Session session in this.CurrentConnection.Server.Sessions)
                    {
                        if (session.Client.ClientName != "")
                        {
                            var sess = new ToolStripMenuItem(String.Format("{1} - {2} ({0})", session.State.ToString().Replace("WTS", ""), session.Client.ClientName, session.Client.UserName));
                            sess.Tag = session;
                            sessions.DropDownItems.Add(sess);
                            var msg = new ToolStripMenuItem(Program.Resources.GetString("SendMessage"));
                            msg.Click += new EventHandler(sd_Click);
                            msg.Tag = session;
                            sess.DropDownItems.Add(msg);

                            var lo = new ToolStripMenuItem(Program.Resources.GetString("Logoff"));
                            lo.Click += new EventHandler(sd_Click);
                            lo.Tag = session;
                            sess.DropDownItems.Add(lo);

                            if (session.IsTheActiveSession)
                            {
                                var lo1 = new ToolStripMenuItem(Program.Resources.GetString("Logoff"));
                                lo1.Click += new EventHandler(sd_Click);
                                lo1.Tag = session;
                                svr.DropDownItems.Add(lo1);
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

            foreach (SpecialCommandConfigurationElement cmd in settings.SpecialCommands)
            {
                var mi = new ToolStripMenuItem(cmd.Name);
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
            using (var mgr = new CredentialManager(this.persistence))
                mgr.ShowDialog();
        }

        private void OpenSavedConnections()
        {
            this.connectionsUiFactory.ConnectByFavoriteNames(settings.SavedConnections);
            settings.ClearSavedConnectionsList();
        }

        private void HideShowFavoritesPanel(bool show)
        {
            if (settings.EnableFavoritesPanel)
            {
                if (show)
                {
                    splitContainer1.Panel1MinSize = 10;
                    splitContainer1.SplitterDistance = settings.FavoritePanelWidth;
                    splitContainer1.Panel1Collapsed = false;
                    splitContainer1.IsSplitterFixed = false;
                    pnlHideTagsFavorites.Show();
                    pnlShowTagsFavorites.Hide();
                }
                else
                {
                    // noticed performance issue when set to 6 and Terminals.config file is empty
                    splitContainer1.Panel1MinSize = 9;
                    splitContainer1.SplitterDistance = 9;
                    splitContainer1.IsSplitterFixed = true;
                    pnlHideTagsFavorites.Hide();
                    pnlShowTagsFavorites.Show();
                }

                settings.ShowFavoritePanel = show;
                tsbTags.Checked = show;
            }
            else
            {
                //just hide it completely
                splitContainer1.Panel1Collapsed = true;
                splitContainer1.Panel1MinSize = 0;
                splitContainer1.SplitterDistance = 0;
            }
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
                catch (Exception exc)
                {
                    Logging.Error(FULLSCREEN_ERROR_MSG, exc);
                }
            }
        }

        internal void FocusFavoriteInQuickConnectCombobox(string favoriteName)
        {
            this.tscConnectTo.SelectedIndex = this.tscConnectTo.Items.IndexOf(favoriteName);
        }

        private void QuickConnect(String server, Int32 port, Boolean connectToConsole)
        {
            IFavorite favorite = FavoritesFactory.GetOrCreateQuickConnectFavorite(server, connectToConsole, port);
            this.connectionsUiFactory.CreateTerminalTab(favorite);
        }

        internal void HandleCommandLineActions(CommandLineArgs commandLineArgs)
        {
            Boolean connectToConsole = commandLineArgs.console;
            this.FullScreen = commandLineArgs.fullscreen;
            if (commandLineArgs.HasUrlDefined)
                QuickConnect(commandLineArgs.UrlServer, commandLineArgs.UrlPort, connectToConsole);

            if (commandLineArgs.HasMachineDefined)
                QuickConnect(commandLineArgs.MachineName, commandLineArgs.Port, connectToConsole);

            this.ConnectToFavorites(commandLineArgs, connectToConsole);
        }

        private void ConnectToFavorites(CommandLineArgs commandLineArgs, bool connectToConsole)
        {
            if (commandLineArgs.Favorites.Length > 0)
            {
                this.connectionsUiFactory.ConnectByFavoriteNames(commandLineArgs.Favorites, connectToConsole);
            }
        }

        private void SaveActiveConnections()
        {
            var activeConnections = new List<string>();
            foreach (TabControlItem item in tcTerminals.Items)
            {
                activeConnections.Add(item.Title);
            }

            settings.CreateSavedConnectionsList(activeConnections.ToArray());
        }

        private void CheckForNewRelease()
        {
            Task<ReleaseInfo> downloadTask = UpdateManager.CheckForUpdates(false);
            downloadTask.ContinueWith(this.CheckForNewRelease, TaskScheduler.FromCurrentSynchronizationContext());
        }

        private void CheckForNewRelease(Task<ReleaseInfo> downloadTask)
        {
            ReleaseInfo downloaded = downloadTask.Result;
            if (downloaded.NewAvailable && !settings.NeverShowTerminalsWindow)
                this.AskIfShowReleasePage(downloaded);

            this.UpdateReleaseToolStripItem(downloaded);
        }

        private void AskIfShowReleasePage(ReleaseInfo releaseInfo)
        {
            string message = string.Format("Version:{0}\r\nPublished:{1}\r\nDo you want to show the Terminals home page?",
                                            releaseInfo.Version, releaseInfo.Published);
            YesNoDisableResult answer = YesNoDisableForm.ShowDialog("New release is available", message);
            if (answer.Result == DialogResult.Yes)
                this.connectionsUiFactory.CreateReleaseTab();

            if (answer.Disable)
                settings.NeverShowTerminalsWindow = true;
        }

        #endregion

        #region Mainform events

        private void MainForm_Load(object sender, EventArgs e)
        {
            this.AssingTitle();
            this.CheckForNewRelease();
            this.OpenSavedConnections();
        }

        private void MainForm_Shown(object sender, EventArgs e)
        {
            // Get initial window state, location and after the form has finished loading
            this.SetWindowState();
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
            else if (e.KeyCode == Keys.F3)
            {
                this.ShowQuickConnect();
            }
        }

        private void ShowQuickConnect()
        {
            using (var qc = new QuickConnect(this.persistence))
            {
                if (qc.ShowDialog(this) == DialogResult.OK && !string.IsNullOrEmpty(qc.ConnectionName))
                    this.connectionsUiFactory.ConnectByFavoriteNames(new List<string>(){ qc.ConnectionName} );
            }
        }

        private void MainForm_Activated(object sender, EventArgs e)
        {
            this.formSettings.EnsureVisibleScreenArea();

            if (this.FullScreen)
                this.tcTerminals.ShowTabs = false;
        }

        private void MainForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            favsList1.SaveState();

            if (this.FullScreen)
                this.FullScreen = false;

            this.MainWindowNotifyIcon.Visible = false;
            CloseOpenedConnections(e);
            this.toolStripContainer.SaveLayout();

            if (!e.Cancel)
                SingleInstanceApplication.Instance.Close();
        }

        private void CloseOpenedConnections(FormClosingEventArgs args)
        {
            if (this.tcTerminals.Items.Count > 0)
            {
                if (settings.ShowConfirmDialog)
                    SaveConnectonsIfRequested(args);

                if (settings.SaveConnectionsOnClose)
                    this.SaveActiveConnections();
            }
        }

        private void SaveConnectonsIfRequested(FormClosingEventArgs args)
        {
            var frmSaveActiveConnections = new SaveActiveConnectionsForm();
            if (frmSaveActiveConnections.ShowDialog() == DialogResult.OK)
            {
                settings.ShowConfirmDialog = frmSaveActiveConnections.PromptNextTime;
                if (frmSaveActiveConnections.OpenConnectionsNextTime)
                    this.SaveActiveConnections();
            }
            else
                args.Cancel = true;
        }

        private void MainForm_Resize(object sender, EventArgs e)
        {
            if (this.WindowState == FormWindowState.Minimized)
            {
                if (settings.MinimizeToTray)
                    this.Visible = false;
            }
            else
            {
                SetWindowState();
            }
        }

        private void MainForm_Move(object sender, EventArgs e)
        {
            if (!fullScreenSwitch.SwitchingFullScreen && this.WindowState == FormWindowState.Normal)
                fullScreenSwitch.LastWindowStateNormalLocation = this.Location;
        }

        #endregion

        #region Private events

        /// <summary>
        /// Save the MainForm windowstate, location and size for reference,
        /// when restoring from fullscreen mode.
        /// </summary>
        private void SetWindowState()
        {
            // In a higher DPI mode, the form resize event is called before
            // the fullScreenSwitch class is initialized.
            if (fullScreenSwitch == null)
                return;

            // Save window state only when not switching to and from fullscreen mode
            if (fullScreenSwitch.SwitchingFullScreen)
                return;

            this.fullScreenSwitch.LastWindowState = this.WindowState;

            if (this.WindowState == FormWindowState.Normal)
            {
                this.fullScreenSwitch.LastWindowStateNormalLocation = this.Location;
                this.fullScreenSwitch.LastWindowStateNormalSize = this.Size;
            }
        }

        private void TcTerminals_TabDetach(TabControlItemChangedEventArgs args)
        {
            this.tcTerminals.SelectedItem = args.Item;
            this.terminalsControler.DetachTabToNewWindow();
        }

        private void TcTerminals_MouseClick(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                if (tcTerminals != null && sender != null)
                    this.QuickContextMenu.Show(tcTerminals, e.Location);
            }
        }

        private void QuickContextMenu_Opening(object sender, CancelEventArgs e)
        {
            this.TcTerminals_MouseClick(null, new MouseEventArgs(MouseButtons.Right, 1, 0, 0, 0));
            e.Cancel = false;
        }

        private void QuickContextMenu_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {
            ToolStripItem clickedItem = e.ClickedItem;
            if (clickedItem.Text == Program.Resources.GetString("Restore") ||
                clickedItem.Name == FavoritesMenuLoader.COMMAND_RESTORESCREEN ||
                clickedItem.Name == FavoritesMenuLoader.COMMAND_FULLSCREEN)
            {
                this.FullScreen = !this.FullScreen;
            }
            else switch (clickedItem.Name)
            {
                case FavoritesMenuLoader.COMMAND_CREDENTIALMANAGER:
                    this.ShowCredentialsManager();
                    break;
                case FavoritesMenuLoader.COMMAND_ORGANIZEFAVORITES:
                    this.ManageConnectionsToolStripMenuItem_Click(null, null);
                    break;
                case FavoritesMenuLoader.COMMAND_OPTIONS:
                    this.OptionsToolStripMenuItem_Click(null, null);
                    break;
                case FavoritesMenuLoader.COMMAND_NETTOOLS:
                    this.ToolStripButton2_Click(null, null);
                    break;
                case FavoritesMenuLoader.COMMAND_CAPTUREMANAGER:
                    this.terminalsControler.RefreshCaptureManagerAndCreateItsTab(true);
                    break;
                case FavoritesMenuLoader.COMMAND_EXIT:
                    this.Close();
                    break;
                case FavoritesMenuLoader.QUICK_CONNECT:
                    this.ShowQuickConnect();
                    break;
                case FavoritesMenuLoader.COMMAND_SHOWMENU:
                    {
                        Boolean visible = !this.menuStrip.Visible;
                        this.menuStrip.Visible = visible;
                        this.menubarToolStripMenuItem.Checked = visible;
                    }
                    break;
                case FavoritesMenuLoader.COMMAND_SPECIAL:
                    return;
                default:
                    this.OnFavoriteTrayToolsStripClick(e);
                    break;
            }

            this.QuickContextMenu.Hide();
        }

        private void OnFavoriteTrayToolsStripClick(ToolStripItemClickedEventArgs e)
        {
            var tag = e.ClickedItem.Tag as String;

            if (tag != null)
            {
                String itemName = e.ClickedItem.Text;
                if (tag == FavoritesMenuLoader.FAVORITE)
                    this.connectionsUiFactory.ConnectByFavoriteNames(new List<string>(){itemName});

                if (tag == GroupMenuItem.TAG)
                {
                    var parent = e.ClickedItem as ToolStripMenuItem;
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
                    IEnumerable<string> connectionNames = parent.DropDownItems.Cast<ToolStripMenuItem>()
                                                                              .Select(menuItem => menuItem.Text);
                    this.connectionsUiFactory.ConnectByFavoriteNames(connectionNames);
                }
            }
        }

        private DialogResult AskUserIfWantsConnectToAll(ToolStripMenuItem parent)
        {
            String localizedMessageFormat = Program.Resources.GetString("Areyousureyouwanttoconnecttoalltheseterminals");
            String message = String.Format(localizedMessageFormat, parent.DropDownItems.Count);
            String confirmatioTitle = Program.Resources.GetString("Confirmation");
            return MessageBox.Show(message, confirmatioTitle, MessageBoxButtons.OKCancel);
        }

        private void ExitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void GroupAddToolStripMenuItem_Click(object sender, EventArgs e)
        {
            IGroup selectedGroup = ((GroupMenuItem)sender).Group;
            if (selectedGroup != null)
            {
                IFavorite selectedFavorite = this.terminalsControler.Selected.Favorite;
                selectedGroup.AddFavorite(selectedFavorite);
            }
        }

        private void GroupToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var groupMenuItem = (GroupMenuItem)sender;
            foreach (IFavorite favorite in groupMenuItem.Favorites)
            {
                this.connectionsUiFactory.CreateTerminalTab(favorite);
            }
        }

        private void ServerToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string connectionName = ((ToolStripItem)sender).Text;
            IFavorite favorite = PersistedFavorites[connectionName];
            this.connectionsUiFactory.CreateTerminalTab(favorite);
        }

        private void sd_Click(object sender, EventArgs e)
        {
            var menu = sender as ToolStripMenuItem;
            if (menu != null)
            {
                if (menu.Text == Program.Resources.GetString("Shutdown"))
                {
                    var server = menu.Tag as TerminalServices.TerminalServer;
                    if (server != null && MessageBox.Show(Program.Resources.GetString("Areyousureyouwanttoshutthismachineoff"), Program.Resources.GetString("Confirmation"), MessageBoxButtons.OKCancel) == DialogResult.OK)
                        TerminalServices.TerminalServicesAPI.ShutdownSystem(server, false);
                }
                else if (menu.Text == Program.Resources.GetString("Reboot"))
                {
                    var server = menu.Tag as TerminalServices.TerminalServer;
                    if (server != null && MessageBox.Show(Program.Resources.GetString("Areyousureyouwanttorebootthismachine"), Program.Resources.GetString("Confirmation"), MessageBoxButtons.OKCancel) == DialogResult.OK)
                        TerminalServices.TerminalServicesAPI.ShutdownSystem(server, true);
                }
                else if (menu.Text == Program.Resources.GetString("Logoff"))
                {
                    var session = menu.Tag as TerminalServices.Session;
                    if (session != null && MessageBox.Show(Program.Resources.GetString("Areyousureyouwanttologthissessionoff"), Program.Resources.GetString("Confirmation"), MessageBoxButtons.OKCancel) == DialogResult.OK)
                        TerminalServices.TerminalServicesAPI.LogOffSession(session, false);
                }
                else if (menu.Text == Program.Resources.GetString("SendMessage"))
                {
                    var session = menu.Tag as TerminalServices.Session;
                    TerminalServerManager.SendMessageToSession(session);
                }
            }
        }

        private void terminalTabPage_Resize(object sender, EventArgs e)
        {
            var terminalTabControlItem = sender as TerminalTabControlItem;
            if (terminalTabControlItem != null)
            {
                // TODO Fix the smart sizing issue added on 7/27/2011
                //var rdpConnection = terminalTabControlItem.Connection as RDPConnection;
                //if (rdpConnection != null &&
                //    !rdpConnection.AxMsRdpClient.AdvancedSettings3.SmartSizing)
                //{
                //    //rdpConnection.AxMsRdpClient.DesktopWidth = terminalTabControlItem.Width;
                //    //rdpConnection.AxMsRdpClient.DesktopHeight = terminalTabControlItem.Height;
                //    //Debug.WriteLine("Tab size:" + terminalTabControlItem.Size.ToString() + ";" +
                //    //  rdpConnection.AxMsRdpClient.DesktopHeight.ToString() + "," +
                //    //  rdpConnection.AxMsRdpClient.DesktopWidth.ToString());
                //}
            }
        }

        private void terminalTabPage_DragEnter(object sender, DragEventArgs e)
        {
            e.Effect = e.Data.GetDataPresent(DataFormats.FileDrop, false) ? DragDropEffects.Copy : DragDropEffects.None;
        }

        private void TerminalTabPage_DragOver(object sender, DragEventArgs e)
        {
            this.terminalsControler.Select(sender as TerminalTabControlItem);
        }

        private void TerminalTabPage_DoubleClick(object sender, EventArgs e)
        {
            if (this.terminalsControler.HasSelected)
            {
                this.tsbDisconnect.PerformClick();
            }
        }

        private void NewTerminalToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.connectionsUiFactory.CreateNewTerminal();
        }

        private void TsbConnect_Click(object sender, EventArgs e)
        {
            this.ConnectFromQuickCombobox(false);
        }

        private void TsbConnectToConsole_Click(object sender, EventArgs e)
        {
            this.ConnectFromQuickCombobox(true);
        }

        private void TsbConnectAs_Click(object sender, EventArgs e)
        {
            using (var usrForm = new ConnectExtraForm(this.persistence))
            {
                if (usrForm.ShowDialog() != DialogResult.OK)
                    return;

                this.ConnectFromQuickCombobox(usrForm.Console, usrForm.NewWindow, usrForm.Credentials);
            }
        }

        private void ConnectFromQuickCombobox(bool forceConsole, bool forceNewWindow = false, ICredentialSet credentials = null)
        {
            string connectionName = this.tscConnectTo.Text;
            if (!string.IsNullOrEmpty(connectionName))
            {
                this.connectionsUiFactory.ConnectByFavoriteNames(new List<string>() { connectionName }, forceConsole, forceNewWindow, credentials);
            }
        }

        private void TscConnectTo_KeyDown(object sender, KeyEventArgs e)
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

        private void DeleteFavorite(string name)
        {
            tscConnectTo.Items.Remove(name);
            var favorite = PersistedFavorites[name];
            PersistedFavorites.Delete(favorite);
            favoritesToolStripMenuItem.DropDownItems.RemoveByKey(name);
        }

        private void TsbDisconnect_Click(object sender, EventArgs e)
        {
            try
            {
                TerminalTabControlItem tabToClose = this.terminalsControler.Selected;
                if (this.tcTerminals.Items.Contains(tabToClose))
                    this.tcTerminals.CloseTab(tabToClose);
            }
            catch (Exception exc)
            {
                Logging.Error("Disconnecting a tab threw an exception", exc);
            }
        }

        // TODO Assing missing event handler
        private void tcTerminals_SelectedIndexChanged(object sender, EventArgs e)
        {
            this.UpdateControls();
        }

        private void NewTerminalToolStripMenuItem_Click_1(object sender, EventArgs e)
        {
            this.connectionsUiFactory.CreateNewTerminal();
        }

        private void TsbGrabInput_Click(object sender, EventArgs e)
        {
            this.ToggleGrabInput();
        }

        private void TcTerminals_TabControlItemClosing(TabControlItemClosingEventArgs e)
        {
            Boolean cancel = false;
            if (this.CurrentConnection != null && this.CurrentConnection.Connected)
            {
                Boolean close = AskToClose();

                if (close)
                {
                    if (CurrentConnection != null)
                    {
                        CurrentConnection.Disconnect();
                        // Close tabitem functions handled under each connection disconnect methods.
                        cancel = true;
                    }
                }
                else
                {
                    cancel = true;
                }
            }

            e.Cancel = cancel;
        }

        private bool AskToClose()
        {
            if (settings.WarnOnConnectionClose)
            {
                string message = Program.Resources.GetString("Areyousurethatyouwanttodisconnectfromtheactiveterminal");
                string title = Program.Resources.GetString("Terminals");
                YesNoDisableResult answer = YesNoDisableForm.ShowDialog(title, message);
                if (answer.Disable)
                    settings.WarnOnConnectionClose = false;

                return answer.Result == DialogResult.Yes;
            }

            return true;
        }

        private void TcTerminals_TabControlItemSelectionChanged(TabControlItemChangedEventArgs e)
        {
            this.UpdateControls();
            this.AssingTitle();

            if (this.tcTerminals.Items.Count > 0)
            {
                this.tsbDisconnect.Enabled = e.Item != null;
                this.disconnectToolStripMenuItem.Enabled = e.Item != null;
                this.SetGrabInput(true);
            }
        }

        private void AssingTitle()
        {
            TabControlItem selectedTab = this.tcTerminals.SelectedItem;
            if (settings.ShowInformationToolTips && selectedTab != null)
                this.Text = selectedTab.ToolTipText.Replace("\r\n", "; ");
            else
                this.Text = Program.Info.GetAboutText(this.persistence.Name);
        }

        private void TscConnectTo_TextChanged(object sender, EventArgs e)
        {
            this.UpdateControls();
        }

        private void TcTerminals_MouseHover(object sender, EventArgs e)
        {
            if (this.tcTerminals != null && !this.tcTerminals.ShowTabs)
                this.timerHover.Enabled = true;
        }

        private void TcTerminals_MouseLeave(object sender, EventArgs e)
        {
            this.timerHover.Enabled = false;
            if (this.FullScreen && this.tcTerminals.ShowTabs && !this.tcTerminals.MenuOpen)
                this.tcTerminals.ShowTabs = false;

            if (this.currentToolTipItem != null)
            {
                this.currentToolTip.Hide(this.currentToolTipItem);
                this.currentToolTip.Active = false;
            }
        }

        private void TcTerminals_TabControlItemClosed(object sender, EventArgs e)
        {
            CloseTabControlItem();
        }

        private void TcTerminals_DoubleClick(object sender, EventArgs e)
        {
            this.FullScreen = !this.FullScreen;
        }

        private void TsbFullScreen_Click(object sender, EventArgs e)
        {
            this.FullScreen = !this.FullScreen;
            this.UpdateControls();
        }

        private void TcTerminals_MenuItemsLoaded(object sender, EventArgs e)
        {
            this.UpdateTabControlMenuItemIcons();

            if (this.FullScreen)
            {
                var sep = new ToolStripSeparator();
                this.tcTerminals.Menu.Items.Add(sep);
                var item = new ToolStripMenuItem(Program.Resources.GetString("Restore"), null, this.TcTerminals_DoubleClick);
                this.tcTerminals.Menu.Items.Add(item);
                item = new ToolStripMenuItem(Program.Resources.GetString("Minimize"), null, this.Minimize);
                this.tcTerminals.Menu.Items.Add(item);
            }
        }

        private void UpdateTabControlMenuItemIcons()
        {
            foreach (ToolStripItem menuItem in this.tcTerminals.Menu.Items)
            {
                var tab = this.FindTabControlItemByTitle(menuItem);

                if (tab != null)
                    menuItem.Image = tab.Favorite.ToolBarIconImage;
            }
        }

        private TerminalTabControlItem FindTabControlItemByTitle(ToolStripItem item)
        {
            return this.tcTerminals.Items.Cast<TabControlItem>()
                   .FirstOrDefault(candidate => candidate is TerminalTabControlItem && candidate.Title == item.Text)
                   as TerminalTabControlItem;
        }

        private void SaveTerminalsAsGroupToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string newGroupName = NewGroupForm.AskFroGroupName(this.persistence);
            if (string.IsNullOrEmpty(newGroupName))
                return;

            IGroup group = FavoritesFactory.GetOrAddNewGroup(this.persistence, newGroupName);
            foreach (TerminalTabControlItem tabControlItem in this.tcTerminals.Items)
            {
                group.AddFavorite(tabControlItem.Favorite);
            }

            this.menuLoader.LoadGroups();
        }

        private void OrganizeGroupsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            using (var frmOrganizeGroups = new OrganizeGroupsForm(this.persistence))
            {
                frmOrganizeGroups.ShowDialog();
                this.menuLoader.LoadGroups();
            }
        }

        private void TcTerminals_TabControlMouseOnTitle(TabControlMouseOnTitleEventArgs e)
        {
            if (settings.ShowInformationToolTips)
            {
                if (this.currentToolTip == null)
                {
                    this.currentToolTip = new ToolTip();
                    this.currentToolTip.Active = false;
                }
                else if ((this.currentToolTipItem != null) && (this.currentToolTipItem != e.Item))
                {
                    this.currentToolTip.Hide(this.currentToolTipItem);
                    this.currentToolTip.Active = false;
                }

                if (!this.currentToolTip.Active)
                {
                    this.currentToolTip = new ToolTip();
                    this.currentToolTip.ToolTipTitle = Program.Resources.GetString("ConnectionInformation");
                    this.currentToolTip.ToolTipIcon = ToolTipIcon.Info;
                    this.currentToolTip.UseFading = true;
                    this.currentToolTip.UseAnimation = true;
                    this.currentToolTip.IsBalloon = false;
                    this.currentToolTip.Show(e.Item.ToolTipText, e.Item, (int)e.Item.StripRect.X, 2);
                    this.currentToolTipItem = e.Item;
                    this.currentToolTip.Active = true;
                }
            }
        }

        private void TcTerminals_TabControlMouseLeftTitle(TabControlMouseOnTitleEventArgs e)
        {
            if (this.currentToolTipItem != null)
            {
                this.currentToolTip.Hide(this.currentToolTipItem);
                this.currentToolTip.Active = false;
            }
            /*if (previewPictureBox != null)
            {
                previewPictureBox.Image.Dispose();
                previewPictureBox.Dispose();
                previewPictureBox = null;
            }*/
        }

        private void TimerHover_Tick(object sender, EventArgs e)
        {
            if (this.timerHover.Enabled)
            {
                this.timerHover.Enabled = false;
                this.tcTerminals.ShowTabs = true;
            }
        }

        private void OrganizeFavoritesToolbarToolStripMenuItem_Click(object sender, EventArgs e)
        {
            using (var frmOrganizeFavoritesToolbar = new OrganizeFavoritesToolbarForm(this.persistence))
            {
                frmOrganizeFavoritesToolbar.ShowDialog();
            }
        }

        private void OptionsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            using (var frmOptions = new OptionDialog(CurrentTerminal, this.persistence))
            {
                if (frmOptions.ShowDialog() == DialogResult.OK)
                {
                    this.ApplyControlsEnableAndVisibleState();
                }
            }
        }

        /// <summary>
        /// Disable capture button when function is disabled in options
        /// </summary>
        private void UpdateCaptureButtonEnabled()
        {
            Boolean enableCapture = settings.EnabledCaptureToFolderAndClipBoard;
            this.CaptureScreenToolStripButton.Enabled = enableCapture;
            this.captureTerminalScreenToolStripMenuItem.Enabled = enableCapture;
            this.terminalsControler.UpdateCaptureButtonOnDetachedPopUps();
        }

        private void AboutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            using (var frmAbout = new AboutForm(this.persistence.Name))
            {
                frmAbout.ShowDialog();
            }
        }

        private void TsbTags_Click(object sender, EventArgs e)
        {
            this.HideShowFavoritesPanel(this.tsbTags.Checked);
        }

        private void PbShowTags_Click(object sender, EventArgs e)
        {
            this.HideShowFavoritesPanel(true);
        }

        private void PbHideTags_Click(object sender, EventArgs e)
        {
            this.HideShowFavoritesPanel(false);
        }

        private void TsbFavorites_Click(object sender, EventArgs e)
        {
            settings.EnableFavoritesPanel = this.tsbFavorites.Checked;
            this.HideShowFavoritesPanel(settings.ShowFavoritePanel);
        }

        private void Minimize(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Minimized;
        }

        private void MainWindowNotifyIcon_MouseClick(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                if (settings.MinimizeToTray)
                {
                    this.Visible = !this.Visible;
                    if (this.Visible && this.WindowState == FormWindowState.Minimized)
                        this.WindowState = fullScreenSwitch.LastWindowState;
                }
                else
                {
                    if (this.WindowState == FormWindowState.Normal)
                        this.WindowState = FormWindowState.Minimized;
                    else
                        this.WindowState = fullScreenSwitch.LastWindowState;
                }
            }
        }

        private void CaptureScreenToolStripButton_Click(object sender, EventArgs e)
        {
            CaptureManager.CaptureManager.PerformScreenCapture(this.tcTerminals);
            this.terminalsControler.RefreshCaptureManager(false);

            if (settings.EnableCaptureToFolder && settings.AutoSwitchOnCapture)
                this.terminalsControler.RefreshCaptureManagerAndCreateItsTab(false);
        }

        private void CaptureTerminalScreenToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.CaptureScreenToolStripButton_Click(null, null);
        }

        private void VMRCAdminSwitchButton_Click(object sender, EventArgs e)
        {
            if (this.CurrentConnection != null)
            {
                var vmrc = this.CurrentConnection as VMRCConnection;
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
                var vmrc = this.CurrentConnection as VMRCConnection;
                if (vmrc != null)
                {
                    vmrc.ViewOnlyMode = !vmrc.ViewOnlyMode;
                }
            }

            this.UpdateControls();
        }

        private void ToolStripButton1_Click(object sender, EventArgs e)
        {
            Cursor = Cursors.WaitCursor;
            try
            {
                String sessionId = String.Empty;
                if (!this.CurrentTerminal.ConnectToConsole)
                {
                    sessionId = TSManager.GetCurrentSession(this.CurrentTerminal.Server,
                        this.CurrentTerminal.UserName,
                        this.CurrentTerminal.Domain,
                        Environment.MachineName).Id.ToString();
                }

                var process = new Process();
                String args = String.Format(" \\\\{0} -i {1} -d notepad", CurrentTerminal.Server, sessionId);
                var startInfo = new ProcessStartInfo(settings.PsexecLocation, args);
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

        private void TsbCmd_Click(object sender, EventArgs e)
        {
            Cursor = Cursors.WaitCursor;
            try
            {
                String sessionId = String.Empty;
                if (!this.CurrentTerminal.ConnectToConsole)
                {
                    sessionId = TSManager.GetCurrentSession(this.CurrentTerminal.Server,
                        this.CurrentTerminal.UserName,
                        this.CurrentTerminal.Domain,
                        Environment.MachineName).Id.ToString();
                }

                var process = new Process();
                String args = String.Format(" \\\\{0} -i {1} -d cmd", CurrentTerminal.Server, sessionId);
                var startInfo = new ProcessStartInfo(settings.PsexecLocation, args);
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

        private void StandardToolbarToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.AddShowStrip(this.toolbarStd, this.standardToolbarToolStripMenuItem, !this.toolbarStd.Visible);
        }

        private void ToolStripMenuItemShowHideFavoriteToolbar_Click(object sender, EventArgs e)
        {
            this.AddShowStrip(this.favoriteToolBar, this.toolStripMenuItemShowHideFavoriteToolbar, !this.favoriteToolBar.Visible);
        }

        private void ShortcutsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.AddShowStrip(this.SpecialCommandsToolStrip, this.shortcutsToolStripMenuItem, !this.SpecialCommandsToolStrip.Visible);
        }

        private void MenubarToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.AddShowStrip(this.menuStrip, this.menubarToolStripMenuItem, !this.menuStrip.Visible);
        }

        private void AddShowStrip(ToolStrip strip, ToolStripMenuItem menu, Boolean visible)
        {
            strip.Visible = visible;
            menu.Checked = visible;
            this.toolStripContainer.SaveLayout();
        }

        private void ToolsToolStripMenuItem_DropDownOpening(object sender, EventArgs e)
        {
            this.shortcutsToolStripMenuItem.Checked = this.SpecialCommandsToolStrip.Visible;
            this.toolStripMenuItemShowHideFavoriteToolbar.Checked = this.favoriteToolBar.Visible;
            this.standardToolbarToolStripMenuItem.Checked = this.toolbarStd.Visible;
        }

        private void ToolStripMenuItem3_Click(object sender, EventArgs e)
        {
            using (var org = new OrganizeShortcuts())
            {
                org.ShowDialog(this);
            }

            this.LoadSpecialCommands();
        }

        private void ShortcutsContextMenu_MouseClick(object sender, MouseEventArgs e)
        {
            this.ToolStripMenuItem3_Click(null, null);
        }

        // todo assign missing SpecialCommandsToolStrip_MouseClick to context menu for Organize shortcuts command
        private void SpecialCommandsToolStrip_MouseClick(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
                this.ShortcutsContextMenu.Show(e.X, e.Y);
        }

        private void SpecialCommandsToolStrip_ItemClicked_1(object sender, ToolStripItemClickedEventArgs e)
        {
            var elm = e.ClickedItem.Tag as SpecialCommandConfigurationElement;
            if (elm != null)
                elm.Launch();
        }

        private void ToolStripButton2_Click(object sender, EventArgs e)
        {
            this.connectionsUiFactory.OpenNetworkingTools(NettworkingTools.None, string.Empty);
        }

        private void NetworkingToolsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.ToolStripButton2_Click(null, null);
        }

        private void ToolStripMenuItemCaptureManager_Click(object sender, EventArgs e)
        {
            this.terminalsControler.RefreshCaptureManagerAndCreateItsTab(true);
        }

        private void ToolStripButtonCaptureManager_Click(object sender, EventArgs e)
        {
            Boolean origval = settings.AutoSwitchOnCapture;
            if (!settings.EnableCaptureToFolder || !settings.AutoSwitchOnCapture)
            {
                settings.AutoSwitchOnCapture = true;
            }

            this.terminalsControler.RefreshCaptureManagerAndCreateItsTab(true);
            settings.AutoSwitchOnCapture = origval;
        }

        private void SendAltKeyToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (sender != null && (sender as ToolStripMenuItem) != null)
            {
                String key = (sender as ToolStripMenuItem).Text;
                if (this.CurrentConnection != null)
                {
                    var vnc = this.CurrentConnection as VNCConnection;
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

        private void ToolStripButton4_Click(object sender, EventArgs e)
        {
            if (this.terminalsControler.HasSelected)
            {
                TerminalTabControlItem terminalTabPage = this.terminalsControler.Selected;
                if (terminalTabPage.Connection != null)
                {
                    DesktopSize desktop = terminalTabPage.Connection.Favorite.Display.DesktopSize;
                    terminalTabPage.Connection.ChangeDesktopSize(desktop);
                }
            }
        }

        private void PbShowTagsFavorites_MouseMove(object sender, MouseEventArgs e)
        {
            if (settings.AutoExapandTagsPanel)
                this.HideShowFavoritesPanel(true);
        }

        private void TerminalServerMenuButton_DropDownOpening(object sender, EventArgs e)
        {
            this.BuildTerminalServerButtonMenu();
        }

        private void LockToolbarsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.toolStripContainer.SaveLayout();
            this.lockToolbarsToolStripMenuItem.Checked = !this.lockToolbarsToolStripMenuItem.Checked;
            settings.ToolbarsLocked = this.lockToolbarsToolStripMenuItem.Checked;
            this.toolStripContainer.ChangeLockState();
        }

        private void UpdateReleaseToolStripItem(ReleaseInfo downloaded)
        {
            if (this.updateToolStripItem != null && !this.updateToolStripItem.Visible)
            {
                if (downloaded.NewAvailable)
                {
                    this.updateToolStripItem.Visible = true;
                    string newText = String.Format("{0} - {1}", this.updateToolStripItem.Text, downloaded.Version);
                    this.updateToolStripItem.Text = newText;
                }
            }
        }

        private void ToolStripButton5_Click(object sender, EventArgs e)
        {
            Process.Start("mmc.exe", "compmgmt.msc /a /computer=.");
        }

        private void RebuildTagsIndexToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.persistence.Groups.Rebuild();
            this.menuLoader.LoadGroups();
            this.UpdateControls();
        }

        private void ViewInNewWindowToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (this.CurrentConnection != null)
            {
                this.terminalsControler.DetachTabToNewWindow();
            }
        }

        private void RebuildShortcutsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            settings.SpecialCommands.Clear();
            settings.SpecialCommands = Wizard.SpecialCommandsWizard.LoadSpecialCommands();
            this.LoadSpecialCommands();
        }

        private void RebuildToolbarsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.LoadWindowState();
        }

        private void OpenLogFolderToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Task.Factory.StartNew(this.OpenLogsFolder);
        }

        private void OpenLogsFolder()
        {
            try
            {
                Process.Start(FileLocations.LogDirectory);
            }
            catch (Exception)
            {
                string message = string.Format("Unable to open logs directory:\r\n{0}", FileLocations.LogDirectory);
                MessageBox.Show(message, "Terminals", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void SplitContainer1_SplitterMoved(object sender, SplitterEventArgs e)
        {
            if (splitContainer1.Panel1.Width > 15)
                settings.FavoritePanelWidth = splitContainer1.Panel1.Width;
        }

        private void CredentialManagementToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.ShowCredentialsManager();
        }

        private void CredentialManagementToolStripButton_Click(object sender, EventArgs e)
        {
            this.ShowCredentialsManager();
        }

        private void ExportConnectionsListToolStripMenuItem_Click(object sender, EventArgs e)
        {
            using (var frm = new ExportForm(this.persistence))
                frm.ShowDialog();
        }

        private void ManageConnectionsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            using (OrganizeFavoritesForm conMgr = this.CreateOrganizeFavoritesForm())
            {
                conMgr.ShowDialog();
            }
        }

        private void ToolStripMenuItemImport_Click(object sender, EventArgs e)
        {
            using (OrganizeFavoritesForm conMgr = this.CreateOrganizeFavoritesForm())
            {
                conMgr.CallImport();
                conMgr.ShowDialog();
            }
        }

        private OrganizeFavoritesForm CreateOrganizeFavoritesForm()
        {
            var organizeForm = new OrganizeFavoritesForm(this.persistence);
            organizeForm.AssignConnectionsUiFactory(this.connectionsUiFactory);
            return organizeForm;
        }

        private void ShowInDualScreensToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Screen[] screenArr = Screen.AllScreens;
            Int32 with = 0;
            if (!this.allScreens)
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
            this.allScreens = !this.allScreens;
        }

        private void UpdateToolStripItem_Click(object sender, EventArgs e)
        {
            this.connectionsUiFactory.CreateReleaseTab();
        }

        private void ClearHistoryToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Cursor = Cursors.WaitCursor;
            this.persistence.ConnectionHistory.Clear();
            this.Cursor = Cursors.Default;
        }

        #endregion
    }
}
