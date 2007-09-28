using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using AxMSTSCLib;
using MSTSC = MSTSCLib;
using Terminals.Properties;
using System.Diagnostics;
using System.Runtime.InteropServices;
using TabControl;
using System.IO;
using rpaulo.toolbar;


namespace Terminals
{
    public partial class MainForm : Form {
        public const int WM_LEAVING_FULLSCREEN = 0x4ff;
        private bool fullScreen;
        private Point lastLocation;
        private Size lastSize;
        private FormWindowState lastState;
        FormSettings _formSettings;
        private ImageFormatHandler imageFormatHandler;
        /*private ScreenCapture screenCapture;
        private PictureBox previewPictureBox;*/

        ToolBarManager toolBarManager;

        public MainForm() {
            try {
                imageFormatHandler = new ImageFormatHandler();
                //screenCapture = new ScreenCapture(imageFormatHandler);
                _formSettings = new FormSettings(this);
                InitializeComponent();
                LoadFavorites();
                LoadFavorites("");
                LoadGroups();
                UpdateControls();
                pnlTagsFavorites.Width = 7;
                LoadTags("");
                ProtocolHandler.Register();
                SingleInstanceApplication.NewInstanceMessage += new NewInstanceMessageEventHandler(SingleInstanceApplication_NewInstanceMessage);
                tcTerminals.MouseClick += new MouseEventHandler(tcTerminals_MouseClick);
                QuickContextMenu.ItemClicked += new ToolStripItemClickedEventHandler(QuickContextMenu_ItemClicked);
                SystemTrayQuickConnectToolStripMenuItem.DropDownItemClicked += new ToolStripItemClickedEventHandler(SystemTrayQuickConnectToolStripMenuItem_DropDownItemClicked);
                toolBarManager = new ToolBarManager(this.toolStripContainer.ContentPanel, this);
                toolBarManager.AddControl(this.toolbarStd);
                toolBarManager.MainForm.BackColor = this.toolStripContainer.ContentPanel.BackColor;
            } catch(Exception exc) {
                System.Windows.Forms.MessageBox.Show(exc.ToString());
            }
            

        }


        void SystemTrayQuickConnectToolStripMenuItem_DropDownItemClicked(object sender, ToolStripItemClickedEventArgs e) {
            Connect(e.ClickedItem.Text);
        }


        void tcTerminals_MouseClick(object sender, MouseEventArgs e) {
            if(e.Button == MouseButtons.Right) {

                QuickContextMenu.Items.Clear();
                if(this.FullScreen) 
                    QuickContextMenu.Items.Add("&Restore Screen");
                else
                    QuickContextMenu.Items.Add("&Full Screen");

                QuickContextMenu.Items.Add("-");

                FavoriteConfigurationElementCollection favorites = Settings.GetFavorites();
                foreach(FavoriteConfigurationElement favorite in favorites) {
                    if(favorite.ToolBarIcon != null && System.IO.File.Exists(favorite.ToolBarIcon))
                    {
                        QuickContextMenu.Items.Add(favorite.Name, Image.FromFile(favorite.ToolBarIcon));
                    }
                    else
                    {
                        QuickContextMenu.Items.Add(favorite.Name);
                    }
                }

                QuickContextMenu.Items.Add("-");
                QuickContextMenu.Items.Add("&Exit");

                QuickContextMenu.Show(tcTerminals, e.Location);
            }
        }

        void QuickContextMenu_ItemClicked(object sender, ToolStripItemClickedEventArgs e) {

            switch(e.ClickedItem.Text) {
                case "&Restore":
                case "&Restore Screen":
                case "&Full Screen":
                    this.FullScreen = !this.FullScreen;
                    break;
                case "&Exit":
                    Close();
                    break;
                default:
                    Connect(e.ClickedItem.Text);
                    break;
            }
                
        }

        private void MainForm_Load(object sender, EventArgs e) {

        }

        void SingleInstanceApplication_NewInstanceMessage(object sender, object message) {
            if(WindowState == FormWindowState.Minimized)
                NativeApi.ShowWindow(new HandleRef(this, this.Handle), 9);
            Activate();
            string[] commandLine = (string[])message;
            if(commandLine != null)
                ParseCommandline(commandLine);
        }

        protected override void SetVisibleCore(bool value) {
            _formSettings.LoadFormSize();
            base.SetVisibleCore(value);
        }

        public Connections.IConnection CurrentConnection {
            get {
                if(tcTerminals.SelectedItem != null)
                    return ((TerminalTabControlItem)(tcTerminals.SelectedItem)).Connection;
                else
                    return null;
            }
        }

        public AxMsRdpClient2 CurrentTerminal {
            get {
                if(tcTerminals.SelectedItem != null) {

                    if(((TerminalTabControlItem)(tcTerminals.SelectedItem)).TerminalControl == null) {
                        if(CurrentConnection != null) {
                            if((CurrentConnection as Connections.RDPConnection) != null) {
                                return (CurrentConnection as Connections.RDPConnection).axMsRdpClient2;
                            } else {
                                return null;
                            }
                        } else {
                            return null;
                        }

                    }
                    return null;
                } else {
                    if(CurrentConnection != null) {
                        if((CurrentConnection as Connections.RDPConnection) != null) {
                            return (CurrentConnection as Connections.RDPConnection).axMsRdpClient2;
                        } else {
                            return null;
                        }
                    } else {
                        return null;
                    }
                }
            }
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e) {
            Close();
        }

        private void OpenSavedConnections() {
            foreach(string name in Settings.SavedConnections) {
                Connect(name);
            }
            Settings.ClearSavedConnectionsList();
        }

        public void UpdateControls() {
            tcTerminals.ShowToolTipOnTitle = Settings.ShowInformationToolTips;
            addTerminalToGroupToolStripMenuItem.Enabled = (tcTerminals.SelectedItem != null);
            tsbGrabInput.Enabled = (tcTerminals.SelectedItem != null);
            //tsbFullScreen.Enabled = (tcTerminals.SelectedItem != null);
            grabInputToolStripMenuItem.Enabled = tcTerminals.SelectedItem != null;
            tsbGrabInput.Checked = tsbGrabInput.Enabled && (CurrentTerminal != null) && CurrentTerminal.FullScreen;
            grabInputToolStripMenuItem.Checked = tsbGrabInput.Checked;
            tsbConnect.Enabled = (tscConnectTo.Text != String.Empty);
            saveTerminalsAsGroupToolStripMenuItem.Enabled = (tcTerminals.Items.Count > 0);

            VMRCAdminSwitchButton.Visible = false;
            VMRCViewOnlyButton.Visible = false;

            if(CurrentConnection != null) {
                Connections.VMRCConnection vmrc;
                vmrc = (this.CurrentConnection as Connections.VMRCConnection);
                if(vmrc != null) {
                    VMRCAdminSwitchButton.Visible = true;
                    VMRCViewOnlyButton.Visible = true;
                }
            }
        }

        private void LoadFavorites() {
            FavoriteConfigurationElementCollection favorites = Settings.GetFavorites();
            int seperatorIndex = favoritesToolStripMenuItem.DropDownItems.IndexOf(favoritesSeparator);
            for(int i = favoritesToolStripMenuItem.DropDownItems.Count - 1; i > seperatorIndex; i--) {
                favoritesToolStripMenuItem.DropDownItems.RemoveAt(i);
            }
            tscConnectTo.Items.Clear();

            string longestFav = "";

            foreach(FavoriteConfigurationElement favorite in favorites) {
                AddFavorite(favorite);
                if(favorite.Name.Length > longestFav.Length) longestFav = favorite.Name;
            }

            using(System.Drawing.Graphics g = System.Drawing.Graphics.FromHwnd(this.Handle))
            {
                int Width = (int)g.MeasureString(longestFav, lvFavorites.Font).Width;
                Width = Width + 1;
                if(Width > lvFavorites.Size.Width)
                {
                    //dynamically expand the size of the control to accomodate for longer names
                    tscConnectTo.Size = new Size(Width, tscConnectTo.Height);
                }
            }
            LoadFavoritesToolbar();
        }

        private void LoadFavoritesToolbar() {
            try {
                for(int i = toolbarStd.Items.Count - 1; i >= 0; i--) {
                    ToolStripItem item = toolbarStd.Items[i];
                    if(item.Tag is FavoriteConfigurationElement)
                        toolbarStd.Items.Remove(item);
                }
                foreach(string favoriteButton in Settings.FavoritesToolbarButtons) {
                    FavoriteConfigurationElementCollection favorites = Settings.GetFavorites();
                    FavoriteConfigurationElement favorite = favorites[favoriteButton];
                    Bitmap button = Terminals.Properties.Resources.smallterm;
                    if(favorite.ToolBarIcon != null && favorite.ToolBarIcon != "" && System.IO.File.Exists(favorite.ToolBarIcon))
                    {
                        try
                        {
                            button = (Bitmap)Bitmap.FromFile(favorite.ToolBarIcon);
                        }
                        catch(Exception)
                        {
                            if(button != Terminals.Properties.Resources.smallterm) button = Terminals.Properties.Resources.smallterm;
                        }
                    }
                    ToolStripButton favoriteBtn = new ToolStripButton(favorite.Name, button, serverToolStripMenuItem_Click);
                    favoriteBtn.Tag = favorite;
                    toolbarStd.Items.Add(favoriteBtn);
                }
            } catch(Exception exc) {
                System.Windows.Forms.MessageBox.Show("LoadFavoritesToolbar:" + exc.ToString());
            }
        }

        private void AddFavorite(FavoriteConfigurationElement favorite) {
            tscConnectTo.Items.Add(favorite.Name);
            ToolStripMenuItem serverToolStripMenuItem = new ToolStripMenuItem(favorite.Name);
            serverToolStripMenuItem.Name = favorite.Name;
            serverToolStripMenuItem.Click += serverToolStripMenuItem_Click;
            favoritesToolStripMenuItem.DropDownItems.Add(serverToolStripMenuItem);
        }

        private void LoadGroups() {
            GroupConfigurationElementCollection serversGroups = Settings.GetGroups();
            int seperatorIndex = groupsToolStripMenuItem.DropDownItems.IndexOf(groupsSeparator);
            for(int i = groupsToolStripMenuItem.DropDownItems.Count - 1; i > seperatorIndex; i--) {
                groupsToolStripMenuItem.DropDownItems.RemoveAt(i);
            }
            addTerminalToGroupToolStripMenuItem.DropDownItems.Clear();
            foreach(GroupConfigurationElement serversGroup in serversGroups) {
                AddGroup(serversGroup);
            }
            addTerminalToGroupToolStripMenuItem.Enabled = false;
            saveTerminalsAsGroupToolStripMenuItem.Enabled = false;
        }

        private void AddGroup(GroupConfigurationElement group) {
            ToolStripMenuItem groupToolStripMenuItem = new ToolStripMenuItem(group.Name);
            groupToolStripMenuItem.Name = group.Name;
            groupToolStripMenuItem.Click += new EventHandler(groupToolStripMenuItem_Click);
            groupsToolStripMenuItem.DropDownItems.Add(groupToolStripMenuItem);
            ToolStripMenuItem groupAddToolStripMenuItem = new ToolStripMenuItem(group.Name);
            groupAddToolStripMenuItem.Name = group.Name;
            groupAddToolStripMenuItem.Click += new EventHandler(groupAddToolStripMenuItem_Click);
            addTerminalToGroupToolStripMenuItem.DropDownItems.Add(groupAddToolStripMenuItem);
        }

        void groupAddToolStripMenuItem_Click(object sender, EventArgs e) {
            GroupConfigurationElement group = Settings.GetGroups()[((ToolStripMenuItem)sender).Text];
            group.FavoriteAliases.Add(new FavoriteAliasConfigurationElement(tcTerminals.SelectedItem.Title));
            Settings.DeleteGroup(group.Name);
            Settings.AddGroup(group);
        }

        void groupToolStripMenuItem_Click(object sender, EventArgs e) {
            FavoriteConfigurationElementCollection favorites = Settings.GetFavorites();
            GroupConfigurationElement serversGroup = Settings.GetGroups()[((ToolStripItem)(sender)).Text];
            foreach(FavoriteAliasConfigurationElement favoriteAlias in serversGroup.FavoriteAliases) {
                FavoriteConfigurationElement favorite = favorites[favoriteAlias.Name];
                CreateTerminalTab(favorite);
            }
        }

        private void DeleteFavorite(string name) {
            tscConnectTo.Items.Remove(name);
            Settings.DeleteFavorite(name);
            favoritesToolStripMenuItem.DropDownItems.RemoveByKey(name);
        }

        void serverToolStripMenuItem_Click(object sender, EventArgs e) {
            FavoriteConfigurationElement favorite = Settings.GetFavorites()[((ToolStripItem)(sender)).Text];
            CreateTerminalTab(favorite);
        }

        private string GetToolTipText(FavoriteConfigurationElement favorite) {
            string toolTip =
                "Computer: " + favorite.ServerName + Environment.NewLine +
                "User: " + Functions.UserDisplayName(favorite.DomainName, favorite.UserName) + Environment.NewLine;

            if(Settings.ShowFullInformationToolTips) {
                toolTip +=
                "Port: " + favorite.Port + Environment.NewLine +
                "Colors: " + favorite.Colors.ToString() + Environment.NewLine +
                "Connect to Console: " + favorite.ConnectToConsole.ToString() + Environment.NewLine +
                "Desktop size: " + favorite.DesktopSize.ToString() + Environment.NewLine +
                "Redirect drives: " + favorite.RedirectDrives.ToString() + Environment.NewLine +
                "Redirect ports: " + favorite.RedirectPorts.ToString() + Environment.NewLine +
                "Redirect printers: " + favorite.RedirectPrinters.ToString() + Environment.NewLine +
                "Sounds: " + favorite.Sounds.ToString() + Environment.NewLine;
            }
            return toolTip;
        }

        private void CreateTerminalTab(FavoriteConfigurationElement favorite) {
            if(Settings.ExecuteBeforeConnect) {
                ProcessStartInfo processStartInfo = new ProcessStartInfo(Settings.ExecuteBeforeConnectCommand,
                    Settings.ExecuteBeforeConnectArgs);
                processStartInfo.WorkingDirectory = Settings.ExecuteBeforeConnectInitialDirectory;
                Process process = Process.Start(processStartInfo);
                if(Settings.ExecuteBeforeConnectWaitForExit) {
                    process.WaitForExit();
                }
            }

            if(favorite.ExecuteBeforeConnect) {
                ProcessStartInfo processStartInfo = new ProcessStartInfo(favorite.ExecuteBeforeConnectCommand,
                    favorite.ExecuteBeforeConnectArgs);
                processStartInfo.WorkingDirectory = favorite.ExecuteBeforeConnectInitialDirectory;
                Process process = Process.Start(processStartInfo);
                if(favorite.ExecuteBeforeConnectWaitForExit) {
                    process.WaitForExit();
                }
            }

            string terminalTabTitle = favorite.Name;
            if(Settings.ShowUserNameInTitle) {
                terminalTabTitle += " (" + Functions.UserDisplayName(favorite.DomainName, favorite.UserName) + ")";
            }
            TerminalTabControlItem terminalTabPage = new TerminalTabControlItem(terminalTabTitle);
            try {
                terminalTabPage.AllowDrop = true;
                terminalTabPage.DragOver += terminalTabPage_DragOver;
                terminalTabPage.DragEnter += new DragEventHandler(terminalTabPage_DragEnter);
                terminalTabPage.ToolTipText = GetToolTipText(favorite);
                terminalTabPage.Favorite = favorite;
                terminalTabPage.DoubleClick += new EventHandler(terminalTabPage_DoubleClick);
                tcTerminals.Items.Add(terminalTabPage);
                tcTerminals.SelectedItem = terminalTabPage;
                tcTerminals_SelectedIndexChanged(this, EventArgs.Empty);
                Connections.IConnection conn = Connections.ConnectionManager.CreateConnection(favorite, terminalTabPage, this);
                conn.Connect();
                (conn as Control).BringToFront();
                (conn as Control).Update();
                UpdateControls();
                if(favorite.DesktopSize == DesktopSize.FullScreen)
                    FullScreen = true;
            } catch(Exception exc) {
                tcTerminals.Items.Remove(terminalTabPage);
                tcTerminals.SelectedItem = null;
                terminalTabPage.Dispose();
                System.Windows.Forms.MessageBox.Show(exc.Message);
            }
        }

        //private void tcTerminals_Click(object sender, EventArgs e) {
        //    System.Windows.Forms.MouseEventArgs args = (System.Windows.Forms.MouseEventArgs)e;
        //    if(args.Button == MouseButtons.Right) {
        //        TabControl.TabControl panel = (sender as TabControl.TabControl);
        //        if(panel != null && panel.SelectedItem != null) {
        //            tcTerminals.CloseTab(panel.SelectedItem);
        //            string f = "";
        //        }
        //    }
        //}


        void terminalTabPage_DragEnter(object sender, DragEventArgs e) {
            if(e.Data.GetDataPresent(DataFormats.FileDrop, false)) {
                e.Effect = DragDropEffects.Copy;
            } else {
                e.Effect = DragDropEffects.None;
            }
        }

        private void terminalTabPage_DragOver(object sender, DragEventArgs e) {
            /*TabControlItem item = tcTerminals.GetTabItemByPoint(new Point(e.X, e.Y));
            if (item != null)
            {
                tcTerminals.SelectedItem = item;
            }*/
            tcTerminals.SelectedItem = (TerminalTabControlItem)sender;
        }


        public string GetDesktopShare() {
            string desktopShare = ((TerminalTabControlItem)(tcTerminals.SelectedItem)).Favorite.DesktopShare;
            if(String.IsNullOrEmpty(desktopShare)) {
                desktopShare = Settings.DefaultDesktopShare.Replace("%SERVER%", CurrentTerminal.Server).Replace("%USER%", CurrentTerminal.UserName);
            }
            return desktopShare;
        }

        void terminalTabPage_DoubleClick(object sender, EventArgs e) {
            if(tcTerminals.SelectedItem != null) {
                tsbDisconnect.PerformClick();
            }
        }

        protected override void WndProc(ref Message msg) {
            try {
                if(msg.Msg == 0x21) {
                    TerminalTabControlItem selectedTab = (TerminalTabControlItem)tcTerminals.SelectedItem;
                    if(selectedTab != null) {
                        Rectangle r = selectedTab.RectangleToScreen(selectedTab.ClientRectangle);
                        if(r.Contains(Form.MousePosition)) {
                            SetGrabInput(true);
                        } else {
                            TabControlItem item = tcTerminals.GetTabItemByPoint(tcTerminals.PointToClient(Form.MousePosition));
                            if(item == null)
                                SetGrabInput(false);
                            else if(item == selectedTab)
                                SetGrabInput(true); //Grab input if clicking on currently selected tab
                        }
                    } else
                        SetGrabInput(false);
                } else if(msg.Msg == WM_LEAVING_FULLSCREEN) {
                    if(CurrentTerminal != null) {
                        if(CurrentTerminal.ContainsFocus)
                            tscConnectTo.Focus();
                    } else
                        this.BringToFront();
                }
                //else if (msg.Msg == NativeApi.WM_COPYDATA)
                //{
                //    NativeApi.COPYDATASTRUCT data = (NativeApi.COPYDATASTRUCT)Marshal.PtrToStructure(msg.LParam, typeof(NativeApi.COPYDATASTRUCT));
                //    byte[] buffer = new byte[data.cbData];
                //    Marshal.Copy(data.lpData, buffer, 0, buffer.Length);
                //    string args = Encoding.Unicode.GetString(buffer);
                //    if (WindowState == FormWindowState.Minimized)
                //        NativeApi.ShowWindow(new HandleRef(this, this.Handle), 9);
                //        //WindowState = FormWindowState.Normal;
                //    Activate();
                //    ParseCommandline(args.Split('>'));
                //}
                base.WndProc(ref msg);
            } catch(Exception e) {
                MessageBox.Show(this, e.Message);
            }
        }

        private void SetGrabInput(bool grab) {
            if(CurrentTerminal != null) {
                if(grab && !CurrentTerminal.ContainsFocus)
                    CurrentTerminal.Focus();
                CurrentTerminal.FullScreen = grab;
            }
        }


        private void CreateNewTerminal() {
            CreateNewTerminal(null);
        }

        private void CreateNewTerminal(string name) {
            using(NewTerminalForm frmNewTerminal = new NewTerminalForm(name, true)) {
                if(frmNewTerminal.ShowDialog() == DialogResult.OK) {
                    Settings.AddFavorite(frmNewTerminal.Favorite, frmNewTerminal.ShowOnToolbar);
                    LoadFavorites();
                    LoadFavorites(txtSearchFavorites.Text);
                    LoadTags(txtSearchTags.Text);
                    tscConnectTo.SelectedIndex = tscConnectTo.Items.IndexOf(frmNewTerminal.Favorite.Name);
                    CreateTerminalTab(frmNewTerminal.Favorite);
                }
            }
        }

        private void newTerminalToolStripMenuItem_Click(object sender, EventArgs e) {
            CreateNewTerminal();
        }

        private void tsbConnect_Click(object sender, EventArgs e) {
            string connectionName = tscConnectTo.Text;
            if(connectionName != "") {
                Connect(connectionName);
            }
        }

        internal void Connect(string connectionName) {
            FavoriteConfigurationElementCollection favorites = Settings.GetFavorites();
            FavoriteConfigurationElement favorite = favorites[connectionName];
            if(favorite == null)
                CreateNewTerminal(connectionName);
            else
                CreateTerminalTab(favorite);
        }

        private void tscConnectTo_KeyDown(object sender, KeyEventArgs e) {
            if(e.KeyCode == Keys.Enter) {
                tsbConnect.PerformClick();
            }
            if(e.KeyCode == Keys.Delete && tscConnectTo.DroppedDown &&
                tscConnectTo.SelectedIndex != -1) {
                string connectionName = (string)tscConnectTo.Items[tscConnectTo.SelectedIndex];
                DeleteFavorite(connectionName);
            }
        }

        private void tsbDisconnect_Click(object sender, EventArgs e) {
            tcTerminals.CloseTab(tcTerminals.SelectedItem);
        }

        private void tcTerminals_SelectedIndexChanged(object sender, EventArgs e) {
            UpdateControls();
        }

        private void newTerminalToolStripMenuItem_Click_1(object sender, EventArgs e) {
            CreateNewTerminal();
        }

        public void tsbGrabInput_Click(object sender, EventArgs e) {
            ToggleGrabInput();
        }

        public void ToggleGrabInput() {
            if(CurrentTerminal != null) {
                CurrentTerminal.FullScreen = !CurrentTerminal.FullScreen;
            }
        }



        private void MainForm_KeyDown(object sender, KeyEventArgs e) {
            if(e.KeyValue == 3) {
                ToggleGrabInput();
            }
        }

        private void HideToolBar(bool fullScreen)
        {
            this.toolBarManager.Top.Visible = !fullScreen;
            this.toolBarManager.Left.Visible = !fullScreen;
            this.toolBarManager.Right.Visible = !fullScreen;
            this.toolBarManager.Bottom.Visible = !fullScreen;
        }

        private void SetFullScreen(bool fullScreen) {
            tcTerminals.ShowTabs = !fullScreen;
            tcTerminals.ShowBorder = !fullScreen;

            HideToolBar(fullScreen);

            if(fullScreen) {
                toolbarStd.Visible = false;
                menuStrip.Visible = false;
                this.lastLocation = this.Location;
                this.lastSize = this.RestoreBounds.Size;
                if(this.WindowState == FormWindowState.Minimized)
                    this.lastState = FormWindowState.Normal;
                else
                    this.lastState = this.WindowState;
                this.FormBorderStyle = FormBorderStyle.None;
                this.WindowState = FormWindowState.Normal;
                this.Width = Screen.FromControl(tcTerminals).Bounds.Width;
                this.Height = Screen.FromControl(tcTerminals).Bounds.Height;
                this.Location = Screen.FromControl(tcTerminals).Bounds.Location;
                //this.TopMost = true;
                SetGrabInput(true);
                this.BringToFront();
            } else {                
                this.TopMost = false;
                this.FormBorderStyle = FormBorderStyle.Sizable;
                this.WindowState = this.lastState;
                if(lastState != FormWindowState.Minimized) {
                    if(lastState == FormWindowState.Normal)
                        this.Location = this.lastLocation;
                    this.Size = this.lastSize;
                }
                menuStrip.Visible = true;
                toolbarStd.Visible = true;
            }
            this.fullScreen = fullScreen;
            if(CurrentConnection != null) this.CurrentConnection.ChangeDesktopSize(DesktopSize.FullScreen);
            this.PerformLayout();
        }

        private void tcTerminals_TabControlItemClosing(TabControlItemClosingEventArgs e) {
            if(CurrentConnection != null && CurrentConnection.Connected) {
                bool close = false;
                if(Settings.WarnOnConnectionClose)
                {
                    close = (MessageBox.Show(this, "Are you sure that you want to disconnect from the active terminal?",
                   "Terminals", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes);
                }
                else
                {
                    close = true;
                }
                if(close) {
                    if(CurrentTerminal != null) CurrentTerminal.Disconnect();
                    if(CurrentConnection != null) CurrentConnection.Disconnect();
                    e.Cancel = false;
                } else {
                    e.Cancel = true;
                }
            } else {
                e.Cancel = false;
            }
        }

        private void ParseCommandline() {
            string[] cmdLineArgs = Environment.GetCommandLineArgs();
            ParseCommandline(cmdLineArgs);
        }

        private void ParseCommandline(string[] cmdLineArgs)
        {

            Terminals.CommandLine.TerminalsCA args = new Terminals.CommandLine.TerminalsCA();

            Terminals.CommandLine.Parser.ParseArguments(cmdLineArgs, args);
            if(args.config != null && args.config != "")
            {
                string f = "";
            }
            if(args.url != null && args.url != "")
            {
                string server; int port;
                ProtocolHandler.Parse(args.url, out server, out port);
                QuickConnect(server, port);
            }
        }

        private void QuickConnect(string server, int port) {
            FavoriteConfigurationElementCollection favorites = Settings.GetFavorites();
            FavoriteConfigurationElement favorite = favorites[server];
            if(favorite != null)
                CreateTerminalTab(favorite);
            else {
                //create a temporaty favorite and connect to it
                favorite = new FavoriteConfigurationElement();
                favorite.ServerName = server;
                favorite.Name = server;
                if(port != 0)
                    favorite.Port = port;
                CreateTerminalTab(favorite);
            }
        }

        private void MainForm_Shown(object sender, EventArgs e) {
            ParseCommandline();
        }

        private void tcTerminals_TabControlItemSelectionChanged(TabControlItemChangedEventArgs e) {
            UpdateControls();
            if(tcTerminals.Items.Count > 0) {
                tsbDisconnect.Enabled = e.Item != null;
                disconnectToolStripMenuItem.Enabled = e.Item != null;
                SetGrabInput(true);
            }
        }

        private void tscConnectTo_TextChanged(object sender, EventArgs e) {
            UpdateControls();
        }

        private void tcTerminals_MouseHover(object sender, EventArgs e) {
            if(!tcTerminals.ShowTabs) {
                timerHover.Enabled = true;
                //tcTerminals.ShowTabs = true;
            }
        }

        private void tcTerminals_MouseLeave(object sender, EventArgs e) {
            timerHover.Enabled = false;
            if(FullScreen && tcTerminals.ShowTabs && !tcTerminals.MenuOpen) {
                tcTerminals.ShowTabs = false;
            }
            if(currentToolTipItem != null) {
                currentToolTip.Hide(currentToolTipItem);
            }
            /*if (previewPictureBox != null)
            {
                previewPictureBox.Hide();
            }*/
        }

        public bool FullScreen {
            get {
                return fullScreen;
            }
            set {
                if(FullScreen != value)
                    SetFullScreen(value);
            }
        }

        public void tcTerminals_TabControlItemClosed(object sender, EventArgs e) {
            if(tcTerminals.Items.Count == 0)
                FullScreen = false;
        }

        private void tcTerminals_DoubleClick(object sender, EventArgs e) {
            FullScreen = !fullScreen;
           // UpdateControls();
        }

        private void tsbFullScreen_Click(object sender, EventArgs e) {
            FullScreen = !FullScreen;
            UpdateControls();
        }

        private void tcTerminals_MenuItemsLoaded(object sender, EventArgs e) {
            foreach(ToolStripItem item in tcTerminals.Menu.Items) {
                item.Image = Terminals.Properties.Resources.smallterm;
            }
            if(FullScreen) {
                ToolStripSeparator sep = new ToolStripSeparator();
                tcTerminals.Menu.Items.Add(sep);
                ToolStripMenuItem item = new ToolStripMenuItem("Restore", null, tcTerminals_DoubleClick);
                tcTerminals.Menu.Items.Add(item);
                item = new ToolStripMenuItem("Minimize", null, Minimize);
                tcTerminals.Menu.Items.Add(item);
            }
        }

        private void Minimize(object sender, EventArgs e) {
            this.WindowState = FormWindowState.Minimized;
        }

        private void MainForm_Activated(object sender, EventArgs e) {
            if(FullScreen)
                tcTerminals.ShowTabs = false;
        }

        private void manageConnectionsToolStripMenuItem_Click(object sender, EventArgs e) {
            using(OrganizeFavoritesForm conMgr = new OrganizeFavoritesForm()) {
                conMgr.ShowDialog();
                LoadFavorites();
                LoadFavorites(txtSearchFavorites.Text);
                LoadTags(txtSearchTags.Text);
            }
        }

        private void saveTerminalsAsGroupToolStripMenuItem_Click(object sender, EventArgs e) {
            using(NewGroupForm frmNewGroup = new NewGroupForm()) {
                if(frmNewGroup.ShowDialog() == DialogResult.OK) {
                    GroupConfigurationElement serversGroup = new GroupConfigurationElement();
                    serversGroup.Name = frmNewGroup.txtGroupName.Text;
                    foreach(TabControlItem tabControlItem in tcTerminals.Items) {
                        serversGroup.FavoriteAliases.Add(new FavoriteAliasConfigurationElement(tabControlItem.Title));
                    }
                    Settings.AddGroup(serversGroup);
                    LoadGroups();
                }
            }
        }

        private void organizeGroupsToolStripMenuItem_Click(object sender, EventArgs e) {
            using(OrganizeGroupsForm frmOrganizeGroups = new OrganizeGroupsForm()) {
                frmOrganizeGroups.ShowDialog();
                LoadGroups();
            }
        }

        private void SaveActiveConnections() {
            List<String> activeConnections = new List<string>();
            foreach(TabControlItem item in tcTerminals.Items) {
                activeConnections.Add(item.Title);
            }
            Settings.CreateSavedConnectionsList(activeConnections.ToArray());
        }

        private void MainForm_FormClosing(object sender, FormClosingEventArgs e) {
            if(tcTerminals.Items.Count > 0) {
                if(Settings.ShowConfirmDialog) {
                    SaveActiveConnectionsForm frmSaveActiveConnections = new SaveActiveConnectionsForm();
                    if(frmSaveActiveConnections.ShowDialog() == DialogResult.OK) {
                        Settings.ShowConfirmDialog = !frmSaveActiveConnections.chkDontShowDialog.Checked;
                        if(frmSaveActiveConnections.chkOpenOnNextTime.Checked) {
                            SaveActiveConnections();
                        }
                        e.Cancel = false;
                    } else {
                        e.Cancel = true;
                    }
                } else if(Settings.SaveConnectionsOnClose) {
                    SaveActiveConnections();
                }
            }
            ToolStripManager.SaveSettings(this);
        }

        private void timerHover_Tick(object sender, EventArgs e) {
            if(timerHover.Enabled) {
                timerHover.Enabled = false;
                tcTerminals.ShowTabs = true;
            }
        }

        private void organizeFavoritesToolbarToolStripMenuItem_Click(object sender, EventArgs e) {
            OrganizeFavoritesToolbarForm frmOrganizeFavoritesToolbar = new OrganizeFavoritesToolbarForm();
            if(frmOrganizeFavoritesToolbar.ShowDialog() == DialogResult.OK) {
                LoadFavoritesToolbar();
            }
        }

        private void optionsToolStripMenuItem_Click(object sender, EventArgs e) {
            OptionsForm frmOptions = new OptionsForm(CurrentTerminal);
            if(frmOptions.ShowDialog() == DialogResult.OK) {
                tcTerminals.ShowToolTipOnTitle = Settings.ShowInformationToolTips;
                if(tcTerminals.SelectedItem != null) {
                    tcTerminals.SelectedItem.ToolTipText = GetToolTipText(((TerminalTabControlItem)tcTerminals.SelectedItem).Favorite);
                }
            }
        }

        private void aboutToolStripMenuItem_Click(object sender, EventArgs e) {
            AboutForm frmAbout = new AboutForm();
            frmAbout.ShowDialog();
        }

        private TabControlItem currentToolTipItem = null;
        private ToolTip currentToolTip = new ToolTip();

        private void tcTerminals_TabControlMouseOnTitle(TabControlMouseOnTitleEventArgs e) {
            if(Settings.ShowInformationToolTips) {
                //ToolTip
                if((currentToolTipItem != null) && (currentToolTipItem != e.Item)) {
                    currentToolTip.Hide(currentToolTipItem);
                }
                currentToolTip.ToolTipTitle = "Connection information";
                currentToolTip.ToolTipIcon = ToolTipIcon.Info;
                currentToolTip.UseFading = true;
                currentToolTip.UseAnimation = true;
                currentToolTip.IsBalloon = false;
                currentToolTip.Show(e.Item.ToolTipText, e.Item, (int)e.Item.StripRect.X, 2);
                currentToolTipItem = e.Item;
            }
        }

        private void tcTerminals_TabControlMouseLeftTitle(TabControlMouseOnTitleEventArgs e) {
            if(currentToolTipItem != null) {
                currentToolTip.Hide(currentToolTipItem);
            }
            /*if (previewPictureBox != null)
            {
                previewPictureBox.Image.Dispose();
                previewPictureBox.Dispose();
                previewPictureBox = null;
            }*/
        }

        private void tcTerminals_TabControlMouseEnteredTitle(TabControlMouseOnTitleEventArgs e) {
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
            if(Settings.ShowInformationToolTips) {
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

        private void tsbTags_Click(object sender, EventArgs e) {
            if(tsbTags.Checked) {
                ShowTags();
            } else {
                HideTagsFavorites();
            }
        }

        private void pbShowTags_Click(object sender, EventArgs e) {
            ShowTags();
        }

        private void pbHideTags_Click(object sender, EventArgs e) {
            HideTagsFavorites();
        }

        private void LoadFavorites(string filter) {
            lvFavorites.Items.Clear();
            FavoriteConfigurationElementCollection favorites = Settings.GetFavorites();
            foreach(FavoriteConfigurationElement favorite in favorites) {
                if((String.IsNullOrEmpty(filter) || (favorite.Name.ToUpper().StartsWith(filter.ToUpper())))) {
                    ListViewItem item = new ListViewItem();
                    item.ImageIndex = 0;
                    item.StateImageIndex = 0;
                    item.Tag = favorite;
                    item.Text = favorite.Name;
                    lvFavorites.Items.Add(item);
                }
            }
        }


        private void LoadTags(string filter) {
            lvTags.Items.Clear();
            ListViewItem unTaggedListViewItem = new ListViewItem();
            unTaggedListViewItem.ImageIndex = 0;
            unTaggedListViewItem.StateImageIndex = 0;
            List<FavoriteConfigurationElement> unTaggedFavorites = new List<FavoriteConfigurationElement>();
            foreach(string tag in Settings.Tags) {
                if((String.IsNullOrEmpty(filter) || (tag.ToUpper().StartsWith(filter.ToUpper())))) {
                    ListViewItem item = new ListViewItem();
                    item.ImageIndex = 0;
                    item.StateImageIndex = 0;
                    FavoriteConfigurationElementCollection favorites = Settings.GetFavorites();
                    List<FavoriteConfigurationElement> tagFavorites = new List<FavoriteConfigurationElement>();
                    foreach(FavoriteConfigurationElement favorite in favorites) {
                        if(favorite.TagList.IndexOf(tag) >= 0) {
                            tagFavorites.Add(favorite);
                        } else if(favorite.TagList.Count == 0) {
                            if(unTaggedFavorites.IndexOf(favorite) < 0) {
                                unTaggedFavorites.Add(favorite);
                            }
                        }
                    }
                    item.Tag = tagFavorites;
                    item.Text = tag + " (" + tagFavorites.Count.ToString() + ")";
                    lvTags.Items.Add(item);
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
            unTaggedListViewItem.Text = "UnTagged (" + unTaggedFavorites.Count.ToString() + ")";
            lvTags.Items.Add(unTaggedListViewItem);
        }

        private void ShowTagsFavorites(TabControlItem activeTab) {
            pnlTagsFavorites.Width = 300;
            tcTagsFavorites.SelectedItem = activeTab;
            pnlHideTagsFavorites.Show();
            pnlShowTagsFavorites.Hide();
            txtSearchTags.Focus();
        }

        private void ShowTags() {
            ShowTagsFavorites(tciTags);
            tsbFavorites.Checked = false;
        }

        private void HideTagsFavorites() {
            pnlTagsFavorites.Width = 7;
            pnlHideTagsFavorites.Hide();
            pnlShowTagsFavorites.Show();
            tsbTags.Checked = false;
            tsbFavorites.Checked = false;
        }

        private void ShowFavorites() {
            ShowTagsFavorites(tciFavorites);
            tsbTags.Checked = false;
        }

        private void lvTags_SelectedIndexChanged(object sender, EventArgs e) {
            connectToolStripMenuItem.Enabled = lvTags.SelectedItems.Count > 0;
            lvTagConnections.Items.Clear();
            if(lvTags.SelectedItems.Count > 0) {
                List<FavoriteConfigurationElement> tagFavorites = (List<FavoriteConfigurationElement>)lvTags.SelectedItems[0].Tag;
                foreach(FavoriteConfigurationElement favorite in tagFavorites) {
                    ListViewItem item = lvTagConnections.Items.Add(favorite.Name);
                    item.ImageIndex = 0;
                    item.StateImageIndex = 0;
                    item.Tag = favorite;
                }
            }
        }

        private void lvTagConnections_SelectedIndexChanged(object sender, EventArgs e) {
            connectToolStripMenuItem.Enabled = lvTagConnections.SelectedItems.Count > 0;
        }

        private void connectToolStripMenuItem_Click(object sender, EventArgs e) {
            foreach(ListViewItem item in lvTagConnections.SelectedItems) {
                FavoriteConfigurationElement favorite = (FavoriteConfigurationElement)item.Tag;
                HideTagsFavorites();
                Connect(favorite.Name);
            }
        }

        private void connectToAllToolStripMenuItem_Click(object sender, EventArgs e) {
            foreach(ListViewItem item in lvTagConnections.Items) {
                FavoriteConfigurationElement favorite = (FavoriteConfigurationElement)item.Tag;
                HideTagsFavorites();
                Connect(favorite.Name);
            }
        }

        private void txtSearchTags_TextChanged(object sender, EventArgs e) {
            LoadTags(txtSearchTags.Text);
        }

        private void lvTags_KeyDown(object sender, KeyEventArgs e) {
            if((e.KeyCode == Keys.Enter) && (lvTags.SelectedItems.Count == 1)) {
                connectToAllToolStripMenuItem.PerformClick();
            }
        }

        private void lvTagConnections_KeyDown(object sender, KeyEventArgs e) {
            if((e.KeyCode == Keys.Enter) && (lvTagConnections.SelectedItems.Count == 1)) {
                connectToolStripMenuItem.PerformClick();
            }
        }

        private void lvTags_MouseDoubleClick(object sender, MouseEventArgs e) {
            if(lvTags.SelectedItems.Count == 1) {
                connectToAllToolStripMenuItem.PerformClick();
            }
        }

        private void lvTagConnections_MouseDoubleClick(object sender, MouseEventArgs e) {
            if(lvTagConnections.SelectedItems.Count == 1) {
                connectToolStripMenuItem.PerformClick();
            }
        }

        private void tsbFavorites_Click(object sender, EventArgs e) {
            if(tsbFavorites.Checked) {
                ShowFavorites();
            } else {
                HideTagsFavorites();
            }
        }

        private void txtSearchFavorites_TextChanged(object sender, EventArgs e) {
            LoadFavorites(txtSearchFavorites.Text);
        }

        private void lvFavorites_KeyDown(object sender, KeyEventArgs e) {
            if((e.KeyCode == Keys.Enter) && (lvFavorites.SelectedItems.Count == 1)) {
                connectToolStripMenuItem1.PerformClick();
            }
        }

        private void lvFavorites_MouseDoubleClick(object sender, MouseEventArgs e) {
            if(lvFavorites.SelectedItems.Count == 1) {
                connectToolStripMenuItem1.PerformClick();
            }
        }

        private void connectToolStripMenuItem1_Click(object sender, EventArgs e) {
            foreach(ListViewItem item in lvFavorites.SelectedItems) {
                FavoriteConfigurationElement favorite = (FavoriteConfigurationElement)item.Tag;
                HideTagsFavorites();
                Connect(favorite.Name);
            }
        }

        private void lvFavorites_SelectedIndexChanged(object sender, EventArgs e) {
            connectToolStripMenuItem1.Enabled = lvFavorites.SelectedItems.Count > 0;
        }
        private void MainForm_Resize(object sender, EventArgs e) {
            if(this.WindowState == FormWindowState.Minimized) //{
                if(Settings.MinimizeToTray) this.Visible = false;
        }

        private void MainWindowNotifyIcon_MouseClick(object sender, MouseEventArgs e) {
            if(e.Button == MouseButtons.Left) {
                if(Settings.MinimizeToTray) {
                    this.Visible = !this.Visible;
                    if(this.Visible && this.WindowState == FormWindowState.Minimized) this.WindowState = FormWindowState.Normal;
                } else {
                    if(this.WindowState == FormWindowState.Normal)
                        this.WindowState = FormWindowState.Minimized;
                    else
                        this.WindowState = FormWindowState.Normal;
                }
            }
        }

        private void toolStripMenuItem2_Click(object sender, EventArgs e) {
            Close();
        }

        private void newConnectionToolStripMenuItem_Click(object sender, EventArgs e) {
            manageConnectionsToolStripMenuItem_Click(null, null);
        }

        private void showToolStripMenuItem_Click(object sender, EventArgs e) {
            //SystemTracyQuickConnectToolStripMenuItem
            this.Visible = !this.Visible;
            showToolStripMenuItem.Text = "&Hide / Show";
        }

        private void CaptureScreenToolStripButton_Click(object sender, EventArgs e) {
            ScreenCapture sc = new ScreenCapture();
            string tempFile = System.IO.Path.GetTempFileName() + ".jpg";

            using(sc.CaptureControl(this.tcTerminals, tempFile, ImageFormatHandler.ImageFormatTypes.imgJPEG)) ;
            System.Diagnostics.Process.Start(tempFile);

        }

        private void captureTerminalScreenToolStripMenuItem_Click(object sender, EventArgs e) {
            CaptureScreenToolStripButton_Click(null, null);
        }

        private void VMRCAdminSwitchButton_Click(object sender, EventArgs e) {
            if(CurrentConnection != null) {
                Connections.VMRCConnection vmrc;
                vmrc = (CurrentConnection as Connections.VMRCConnection);
                if(vmrc != null) {
                    vmrc.AdminDisplay();
                }
            }
        }

        private void VMRCViewOnlyButton_Click(object sender, EventArgs e) {
            if(CurrentConnection != null) {
                Connections.VMRCConnection vmrc;
                vmrc = (CurrentConnection as Connections.VMRCConnection);
                if(vmrc != null) {
                    vmrc.ViewOnlyMode = !vmrc.ViewOnlyMode;
                }
            }
            UpdateControls();
        }



        private void SystemTrayContextMenuStrip_Opening(object sender, CancelEventArgs e) {

            SystemTrayQuickConnectToolStripMenuItem.DropDownItems.Clear();
            FavoriteConfigurationElementCollection favorites = Settings.GetFavorites();
            foreach(FavoriteConfigurationElement favorite in favorites) {
                SystemTrayQuickConnectToolStripMenuItem.DropDownItems.Add(favorite.Name);
            }
        }

        private void toolStripButton1_Click(object sender, EventArgs e) {
            Cursor = Cursors.WaitCursor;
            try {
                string sessionId = String.Empty;
                if(!CurrentTerminal.AdvancedSettings3.ConnectToServerConsole) {
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
            } finally {
                Cursor = Cursors.Default;
            }
        }

        private void tsbCMD_Click(object sender, EventArgs e) {
            Cursor = Cursors.WaitCursor;
            try {
                string sessionId = String.Empty;
                if(!CurrentTerminal.AdvancedSettings3.ConnectToServerConsole) {
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
            } finally {
                Cursor = Cursors.Default;
            }
        }

        private void toolStripContainer_TopToolStripPanel_MouseClick(object sender, MouseEventArgs e)
        {
            if(e.Button == MouseButtons.Right)
            {
                //contextMenuStrip1.Show(toolStripContainer, e.X, e.Y);
            }
        }

        private void manageToolbarToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ManageToolStrip mgr = new ManageToolStrip();
            mgr.ShowDialog(this);
        }


    }

    public class TerminalTabControlItem : TabControlItem
    {
        public TerminalTabControlItem(string caption)
            : base(caption, null)
        {
        }
        private Connections.IConnection connection;

        public Connections.IConnection Connection
        {
            get
            {
                return connection;
            }
            set
            {
                connection = value;
            }
        }


        private AxMsRdpClient2 terminalControl;

        public AxMsRdpClient2 TerminalControl
        {
            get
            {
                return terminalControl;
            }
            set
            {
                terminalControl = value;
            }
        }

        private FavoriteConfigurationElement favorite;

        public FavoriteConfigurationElement Favorite
        {
            get
            {
                return favorite;
            }
            set
            {
                favorite = value;
            }
        }
    }
}
