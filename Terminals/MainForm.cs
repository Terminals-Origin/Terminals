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


namespace Terminals {
    public partial class MainForm : Form
    {


        MethodInvoker specialCommandsMIV;
        public static Terminals.CommandLine.TerminalsCA CommandLineArgs = new Terminals.CommandLine.TerminalsCA();

        public const int WM_LEAVING_FULLSCREEN = 0x4ff;
        private bool fullScreen;
        private Point lastLocation;
        private Size lastSize;
        private FormWindowState lastState;
        FormSettings _formSettings;
        private ImageFormatHandler imageFormatHandler;
        /*private ScreenCapture screenCapture;
        private PictureBox previewPictureBox;*/
        
        public MainForm()
        {
            try
            {
                specialCommandsMIV = new MethodInvoker(LoadSpecialCommands);
                resetMethodInvoker = new MethodInvoker(LoadWindowState);

                //check for wizard
                if(Settings.ShowWizard)
                {
                    //settings file doesnt exist, wizard!
                    //this.Hide();
                    FirstRunWizard wzrd = new FirstRunWizard();
                    wzrd.ShowDialog(this);
                    Settings.ShowWizard = false;
                }

                imageFormatHandler = new ImageFormatHandler();
                //screenCapture = new ScreenCapture(imageFormatHandler);
                _formSettings = new FormSettings(this);
                InitializeComponent();

                if(Settings.Office2007BlueFeel)
                    ToolStripManager.Renderer = Office2007Renderer.Office2007Renderer.GetRenderer(Office2007Renderer.RenderColors.Blue);
                else if(Settings.Office2007BlackFeel)
                    ToolStripManager.Renderer = Office2007Renderer.Office2007Renderer.GetRenderer(Office2007Renderer.RenderColors.Black);
                else
                    ToolStripManager.Renderer = new System.Windows.Forms.ToolStripProfessionalRenderer();

                //Settings.RebuildTagIndex();


                LoadFavorites();
                LoadFavorites("");
                System.Threading.ThreadPool.QueueUserWorkItem(new System.Threading.WaitCallback(ReloadSpecialCommands), null);
                //LoadSpecialCommands();
                LoadGroups();
                UpdateControls();
                pnlTagsFavorites.Width = 7;
                LoadTags("");
                ProtocolHandler.Register();
                SingleInstanceApplication.NewInstanceMessage += new NewInstanceMessageEventHandler(SingleInstanceApplication_NewInstanceMessage);
                tcTerminals.MouseClick += new MouseEventHandler(tcTerminals_MouseClick);
                QuickContextMenu.ItemClicked += new ToolStripItemClickedEventHandler(QuickContextMenu_ItemClicked);
                SystemTrayQuickConnectToolStripMenuItem.DropDownItemClicked += new ToolStripItemClickedEventHandler(SystemTrayQuickConnectToolStripMenuItem_DropDownItemClicked);
                //System.Net.NetworkInformation.NetworkChange.NetworkAvailabilityChanged += new System.Net.NetworkInformation.NetworkAvailabilityChangedEventHandler(NetworkChange_NetworkAvailabilityChanged);

                LoadWindowState();

                this.MainWindowNotifyIcon.Visible = Settings.MinimizeToTray;
                this.lockToolbarsToolStripMenuItem.Checked = Settings.ToolbarsLocked;
                this.groupsToolStripMenuItem.Visible = Settings.EnableGroupsMenu;
                

                if(Settings.ToolbarsLocked)
                    MainMenuStrip.GripStyle = ToolStripGripStyle.Hidden;
                else
                    MainMenuStrip.GripStyle = ToolStripGripStyle.Visible;

                //be sure to turn off the visual studio hosting process for this to work
                //right click the Terminals project, debug, bottom checkbox
                //try {
                //    gma.System.Windows.UserActivityHook hook = new gma.System.Windows.UserActivityHook(false, true);
                //    hook.KeyUp += new KeyEventHandler(hook_KeyUp);
                //} catch(Exception exc) {
                //    Terminals.Logging.Log.Info("Failed to set keyboard hook for global event handling.", exc);
                //}
                

                

            }
            catch(Exception exc)
            {
                Terminals.Logging.Log.Info("", exc);
                //System.Windows.Forms.MessageBox.Show(exc.ToString());
            }
        }
        //Keys lastKey = Keys.Escape;
        //void hook_KeyUp(object sender, KeyEventArgs e) {
        //    if((int)e.KeyData != 255) {
        //        lastKey = e.KeyCode;
        //        if(e.Alt && lastKey == Keys.F1) {
        //            string f = "";
        //        }
        //    }
        //}
        
        
        void LoadSpecialCommands()
        {
            SpecialCommandsToolStrip.Items.Clear();
            SpecialCommandConfigurationElementCollection cmdList = Settings.SpecialCommands;

            //if (cmdList == null || cmdList.Count == 0)
            //{
            //    //add the command prompt
            //    SpecialCommandConfigurationElement elm = new SpecialCommandConfigurationElement("Command Shell");
            //    elm.Executable = @"%systemroot%\system32\cmd.exe";
            //    cmdList.Add(elm);
            //    Settings.SpecialCommands = cmdList;
            //}
            foreach(SpecialCommandConfigurationElement cmd in Settings.SpecialCommands)
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
        void SystemTrayQuickConnectToolStripMenuItem_DropDownItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {
            Connect(e.ClickedItem.Text, false, false);
        }

        void tcTerminals_MouseClick(object sender, MouseEventArgs e)
        {
            if(e.Button == MouseButtons.Right)
            {
                QuickContextMenu.Items.Clear();

                if(this.FullScreen)
                    QuickContextMenu.Items.Add(Program.Resources.GetString("RestoreScreen"), Resources.arrow_in);
                else
                    QuickContextMenu.Items.Add(Program.Resources.GetString("FullScreen"), Resources.arrow_out);

                QuickContextMenu.Items.Add("-");
                QuickContextMenu.Items.Add(Program.Resources.GetString("ScreenCaptureManager"), Resources.screen_capture_box);
                QuickContextMenu.Items.Add(Program.Resources.GetString("NetworkingTools"), Resources.computer_link);
                QuickContextMenu.Items.Add("-");
                QuickContextMenu.Items.Add(Program.Resources.GetString("OrganizeFavorites"), Resources.star);
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

                SortedDictionary<string, FavoriteConfigurationElement>  favorites = Settings.GetSortedFavorites(Settings.DefaultSortProperty);

                Dictionary<string, ToolStripMenuItem> tagTools = new Dictionary<string, ToolStripMenuItem>();
                SortedDictionary<string, ToolStripMenuItem> sortedList = new SortedDictionary<string, ToolStripMenuItem>();
                ToolStripMenuItem sortedMenu = new ToolStripMenuItem(Program.Resources.GetString("Alphabetical"));
                sortedMenu.DropDownItemClicked += new ToolStripItemClickedEventHandler(QuickContextMenu_ItemClicked);

                foreach(string key in favorites.Keys)
                {
                    FavoriteConfigurationElement favorite = favorites[key];

                    System.Windows.Forms.ToolStripMenuItem sortedItem = new ToolStripMenuItem();
                    sortedItem.Text = favorite.Name;
                    sortedItem.Tag = "favorite";
                    if(favorite.ToolBarIcon != null && System.IO.File.Exists(favorite.ToolBarIcon))
                        sortedItem.Image = Image.FromFile(favorite.ToolBarIcon);

                    sortedList.Add(favorite.Name, sortedItem);


                    if(favorite.TagList != null && favorite.TagList.Count > 0)
                    {
                        foreach(string tag in favorite.TagList)
                        {
                            System.Windows.Forms.ToolStripMenuItem parent;
                            if(tagTools.ContainsKey(tag))
                            {
                                parent = tagTools[tag];
                            }
                            else
                            {
                                parent = new ToolStripMenuItem();
                                parent.DropDownItemClicked += new ToolStripItemClickedEventHandler(QuickContextMenu_ItemClicked);
                                //parent.MouseUp += new MouseEventHandler(parent_MouseUp);
                                parent.Tag = "tag";
                                parent.Text = tag;
                                tagTools.Add(tag, parent);
                                QuickContextMenu.Items.Add(parent);
                            }
                            System.Windows.Forms.ToolStripMenuItem item = new ToolStripMenuItem();
                            item.Text = favorite.Name;
                            item.Tag = "favorite";
                            if(favorite.ToolBarIcon != null && System.IO.File.Exists(favorite.ToolBarIcon))
                                item.Image = Image.FromFile(favorite.ToolBarIcon);

                            parent.DropDown.Items.Add(item);


                        }
                    }
                    else
                    {
                        ToolStripMenuItem item = new ToolStripMenuItem(favorite.Name);
                        item.Tag = "favorite";
                        if(favorite.ToolBarIcon != null && System.IO.File.Exists(favorite.ToolBarIcon))
                            item.Image = Image.FromFile(favorite.ToolBarIcon);

                        QuickContextMenu.Items.Add(item);

                    }
                }
                if(sortedList != null && sortedList.Count > 0)
                {
                    QuickContextMenu.Items.Add(sortedMenu);
                    sortedMenu.Image = Terminals.Properties.Resources.atoz;
                    foreach(string name in sortedList.Keys)
                    {
                        sortedMenu.DropDownItems.Add(sortedList[name]);
                    }
                }

                QuickContextMenu.Items.Add("-");
                QuickContextMenu.Items.Add(Program.Resources.GetString("Exit"));
                if(tcTerminals != null && sender != null) QuickContextMenu.Show(tcTerminals, e.Location);
            }
        }

        void specialItem_Click(object sender, EventArgs e)
        {
            ToolStripItem specialItem = (ToolStripItem)sender;
            SpecialCommandConfigurationElement elm = (SpecialCommandConfigurationElement)specialItem.Tag;
            elm.Launch();
        }

        private void QuickContextMenu_Opening(object sender, CancelEventArgs e)
        {

            if(QuickContextMenu.Items.Count <= 0)
            {
                tcTerminals_MouseClick(null, new MouseEventArgs(MouseButtons.Right, 1, 0, 0, 0));
                e.Cancel = false;
            }
        }

        void QuickContextMenu_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {
            QuickContextMenu.Hide();

            if (
                e.ClickedItem.Text == Program.Resources.GetString("Restore") ||
                e.ClickedItem.Text == Program.Resources.GetString("RestoreScreen") ||
                e.ClickedItem.Text == Program.Resources.GetString("FullScreen"))
            {
                this.FullScreen = !this.FullScreen;
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

    

        //void NetworkChange_NetworkAvailabilityChanged(object sender, System.Net.NetworkInformation.NetworkAvailabilityEventArgs e)
        //{
        //    MainWindowNotifyIcon.BalloonTipText = (e.IsAvailable ? "Connected" : "Not Connected");
        //    MainWindowNotifyIcon.BalloonTipTitle = "Network Availability Changed";
        //    MainWindowNotifyIcon.BalloonTipIcon = ToolTipIcon.Warning;
        //    MainWindowNotifyIcon.ShowBalloonTip(5000);

        //}

        void SingleInstanceApplication_NewInstanceMessage(object sender, object message)
        {
            if(WindowState == FormWindowState.Minimized)
                NativeApi.ShowWindow(new HandleRef(this, this.Handle), 9);
            Activate();
        }

        protected override void SetVisibleCore(bool value)
        {
            _formSettings.LoadFormSize();
            base.SetVisibleCore(value);
        }

        public Connections.IConnection CurrentConnection
        {
            get
            {
                if(tcTerminals.SelectedItem != null)
                    return ((TerminalTabControlItem)(tcTerminals.SelectedItem)).Connection;
                else
                    return null;
            }
        }

        public AxMsRdpClient2 CurrentTerminal
        {
            get
            {
                if(tcTerminals.SelectedItem != null)
                {

                    if(((TerminalTabControlItem)(tcTerminals.SelectedItem)).TerminalControl == null)
                    {
                        if(CurrentConnection != null)
                        {
                            if((CurrentConnection as Connections.RDPConnection) != null)
                            {
                                return (CurrentConnection as Connections.RDPConnection).axMsRdpClient2;
                            }
                            else
                            {
                                return null;
                            }
                        }
                        else
                        {
                            return null;
                        }

                    }
                    return null;
                }
                else
                {
                    if(CurrentConnection != null)
                    {
                        if((CurrentConnection as Connections.RDPConnection) != null)
                        {
                            return (CurrentConnection as Connections.RDPConnection).axMsRdpClient2;
                        }
                        else
                        {
                            return null;
                        }
                    }
                    else
                    {
                        return null;
                    }
                }
            }
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void OpenSavedConnections()
        {
            foreach(string name in Settings.SavedConnections)
            {
                Connect(name, false, false);
            }
            Settings.ClearSavedConnectionsList();
        }

        private void SaveToolStripPanel(ToolStripPanel Panel, string Position, ToolStripSettings newSettings)
        {
            int rowIndex = 0;
            foreach(ToolStripPanelRow row in Panel.Rows)
            {
                SaveToolStripRow(row, newSettings, Position, rowIndex);
                rowIndex++;
            }
        }

        int currentToolBarCount = 0;
        private void SaveToolStripRow(ToolStripPanelRow Row, ToolStripSettings newSettings, string Position, int rowIndex)
        {
            foreach(ToolStrip strip in Row.Controls)
            {
                //if(strip != menuStrip) {
                ToolStripSetting setting = new ToolStripSetting();
                setting.Dock = Position;
                setting.Row = rowIndex;
                setting.Left = strip.Left;
                setting.Top = strip.Top;
                setting.Name = strip.Name;
                setting.Visible = strip.Visible;
                newSettings.Add(currentToolBarCount, setting);
                currentToolBarCount++;
                //}
            }
        }

        private void SaveWindowState()
        {
            currentToolBarCount = 0;
            if(!Settings.ToolbarsLocked)
            {
                //ToolStripManager.SaveSettings(this);
                ToolStripSettings newSettings = new ToolStripSettings();
                SaveToolStripPanel(this.toolStripContainer.TopToolStripPanel, "Top", newSettings);
                SaveToolStripPanel(this.toolStripContainer.LeftToolStripPanel, "Left", newSettings);
                SaveToolStripPanel(this.toolStripContainer.RightToolStripPanel, "Right", newSettings);
                SaveToolStripPanel(this.toolStripContainer.BottomToolStripPanel, "Bottom", newSettings);
                Settings.ToolbarSettings = newSettings;
            }
        }
        private void HideShowFavoritesPanel(bool Show) {

            if(Show) {
                //splitContainer1.Panel1.Show();
                splitContainer1.SplitterDistance = 0;
                splitContainer1.Panel1Collapsed = false;
                splitContainer1.Panel1MinSize = 7;
                //pnlHideTagsFavorites.Visible = true;
            } else {
                //splitContainer1.Panel1.Hide();
                //splitContainer1.SplitterDistance = 0;
                splitContainer1.Panel1Collapsed = true;
                splitContainer1.Panel1MinSize = 0;
                splitContainer1.SplitterDistance = 0;
                //pnlHideTagsFavorites.Visible = false;
            }
        }

        System.Windows.Forms.MethodInvoker resetMethodInvoker;
        private void ResetToolbars()
        {
            System.Threading.ThreadPool.QueueUserWorkItem(new System.Threading.WaitCallback(ToolbarResetThread));
        }
        private void ToolbarResetThread(object state)
        {
            //System.Threading.Thread.Sleep(500);
            this.Invoke(resetMethodInvoker);
            //System.Threading.Thread.Sleep(500);
            this.Invoke(resetMethodInvoker);
        }

        public void LoadWindowState()
        {
            this.Text = Program.AboutText;

            HideShowFavoritesPanel(Settings.EnableFavoritesPanel);

            //ToolStripManager.LoadSettings(this);
            ToolStripSettings newSettings = Settings.ToolbarSettings;
            ToolStripMenuItem menuItem = null;
            if(newSettings != null && newSettings.Count > 0)
            {
                foreach(int rowIndex in newSettings.Keys)
                {
                    ToolStripSetting setting = newSettings[rowIndex];
                    menuItem = null;
                    ToolStrip strip = null;
                    if(setting.Name == toolbarStd.Name)
                    {
                        strip = toolbarStd;
                        menuItem = standardToolbarToolStripMenuItem;
                    }
                    else if(setting.Name == favoriteToolBar.Name)
                    {
                        strip = favoriteToolBar;
                        menuItem = toolStripMenuItem4;
                    }
                    else if(setting.Name == SpecialCommandsToolStrip.Name)
                    {
                        strip = SpecialCommandsToolStrip;
                        menuItem = shortcutsToolStripMenuItem;
                    }
                    else if(setting.Name == menuStrip.Name)
                    {
                        strip = menuStrip;
                    }
                    else if(setting.Name == tsRemoteToolbar.Name)
                    {
                        strip = tsRemoteToolbar;
                    }

                    if(menuItem != null)
                    {
                        menuItem.Checked = setting.Visible;
                    }
                    if(strip != null)
                    {
                        Point p;
                        int row = setting.Row + 1;
                        //p = new Point(setting.Left, (setting.Row * row) + 1);
                        p = new Point(setting.Left, setting.Top);
                        strip.Location = p;
                        if(setting.Dock == "Top")
                        {
                            this.toolStripContainer.TopToolStripPanel.Join(strip, p);
                        }
                        else if(setting.Dock == "Left")
                        {
                            this.toolStripContainer.LeftToolStripPanel.Join(strip, p);
                        }
                        else if(setting.Dock == "Right")
                        {
                            this.toolStripContainer.RightToolStripPanel.Join(strip, p);
                        }
                        else if(setting.Dock == "Bottom")
                        {
                            this.toolStripContainer.BottomToolStripPanel.Join(strip, p);
                        }
                        //strip.Left = setting.Left;
                        //strip.Top = setting.Top;
                        strip.Visible = setting.Visible;
                        if(Settings.ToolbarsLocked) 
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
            //tsbFullScreen.Enabled = (tcTerminals.SelectedItem != null);
            grabInputToolStripMenuItem.Enabled = tcTerminals.SelectedItem != null;
            tsbGrabInput.Checked = tsbGrabInput.Enabled && (CurrentTerminal != null) && CurrentTerminal.FullScreen;
            grabInputToolStripMenuItem.Checked = tsbGrabInput.Checked;
            tsbConnect.Enabled = (tscConnectTo.Text != String.Empty);
            saveTerminalsAsGroupToolStripMenuItem.Enabled = (tcTerminals.Items.Count > 0);

            this.TerminalServerMenuButton.Visible = false;
            vncActionButton.Visible = false;
            VMRCAdminSwitchButton.Visible = false;
            VMRCViewOnlyButton.Visible = false;

            if(CurrentConnection != null)
            {
                Connections.VMRCConnection vmrc;
                vmrc = (this.CurrentConnection as Connections.VMRCConnection);
                if(vmrc != null)
                {
                    VMRCAdminSwitchButton.Visible = true;
                    VMRCViewOnlyButton.Visible = true;
                }
                Connections.VNCConnection vnc;
                vnc = (this.CurrentConnection as Connections.VNCConnection);
                if(vnc != null)
                {
                    vncActionButton.Visible = true;
                }
                this.TerminalServerMenuButton.Visible = this.CurrentConnection.IsTerminalServer;
                //this.TerminalServerMenuButton.Visible = true;
            }
        }

        private void LoadFavorites()
        {
            //FavoriteConfigurationElementCollection favorites = Settings.GetFavorites();
            SortedDictionary<string, FavoriteConfigurationElement> favorites = Settings.GetSortedFavorites(Settings.DefaultSortProperty);
            int seperatorIndex = favoritesToolStripMenuItem.DropDownItems.IndexOf(favoritesSeparator);
            for(int i = favoritesToolStripMenuItem.DropDownItems.Count - 1; i > seperatorIndex; i--)
            {
                favoritesToolStripMenuItem.DropDownItems.RemoveAt(i);
            }
            tscConnectTo.Items.Clear();

            string longestFav = "";

            foreach(string key in favorites.Keys)
            {
                FavoriteConfigurationElement favorite = favorites[key];
                AddFavorite(favorite);
                if(favorite.Name.Length > longestFav.Length) longestFav = favorite.Name;
            }
            this.favsList1.LoadFavs();
            //using(System.Drawing.Graphics g = System.Drawing.Graphics.FromHwnd(this.Handle))
            //{
            //    int Width = (int)g.MeasureString(longestFav, lvFavorites.Font).Width;
            //    Width = Width + 1;
            //    if(Width > lvFavorites.Size.Width)
            //    {
            //        //dynamically expand the size of the control to accomodate for longer names
            //        tscConnectTo.Size = new Size(Width, tscConnectTo.Height);
            //    }
            //}
            LoadFavoritesToolbar();
        }

        private void LoadFavoritesToolbar()
        {
            try
            {
                bool favvisible = false;
                favoriteToolBar.Items.Clear();
                if (Settings.FavoritesToolbarButtons != null)
                {
                    foreach (string favoriteButton in Settings.FavoritesToolbarButtons)
                    {
                        FavoriteConfigurationElementCollection favorites = Settings.GetFavorites();
                        FavoriteConfigurationElement favorite = favorites[favoriteButton];
                        Bitmap button = Terminals.Properties.Resources.smallterm;
                        if (favorite != null)
                        {
                            if (favorite.ToolBarIcon != null && favorite.ToolBarIcon != "" && System.IO.File.Exists(favorite.ToolBarIcon))
                            {
                                try
                                {
                                    button = (Bitmap)Bitmap.FromFile(favorite.ToolBarIcon);
                                }
                                catch (Exception ex)
                                {
                                    Terminals.Logging.Log.Info("", ex);
                                    if (button != Terminals.Properties.Resources.smallterm) button = Terminals.Properties.Resources.smallterm;
                                }
                            }
                            ToolStripButton favoriteBtn = new ToolStripButton(favorite.Name, button, serverToolStripMenuItem_Click);
                            favoriteBtn.Tag = favorite;
                            favoriteBtn.Overflow = ToolStripItemOverflow.AsNeeded;
                            favoriteToolBar.Items.Add(favoriteBtn);
                            favvisible = true;
                        }
                    }
                }
                favoriteToolBar.Visible = favvisible;
                this.favsList1.LoadFavs();
            }
            catch(Exception exc)
            {
                Terminals.Logging.Log.Info("", exc);
                //System.Windows.Forms.MessageBox.Show("LoadFavoritesToolbar:" + exc.ToString());
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
            for(int i = groupsToolStripMenuItem.DropDownItems.Count - 1; i > seperatorIndex; i--)
            {
                groupsToolStripMenuItem.DropDownItems.RemoveAt(i);
            }
            addTerminalToGroupToolStripMenuItem.DropDownItems.Clear();
            foreach(GroupConfigurationElement serversGroup in serversGroups)
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

        void groupAddToolStripMenuItem_Click(object sender, EventArgs e)
        {
            GroupConfigurationElement group = Settings.GetGroups()[((ToolStripMenuItem)sender).Text];
            group.FavoriteAliases.Add(new FavoriteAliasConfigurationElement(tcTerminals.SelectedItem.Title));
            Settings.DeleteGroup(group.Name);
            Settings.AddGroup(group);
        }

        void groupToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FavoriteConfigurationElementCollection favorites = Settings.GetFavorites();
            GroupConfigurationElement serversGroup = Settings.GetGroups()[((ToolStripItem)(sender)).Text];
            foreach(FavoriteAliasConfigurationElement favoriteAlias in serversGroup.FavoriteAliases)
            {
                FavoriteConfigurationElement favorite = favorites[favoriteAlias.Name];
                CreateTerminalTab(favorite);
            }
        }

        private void DeleteFavorite(string name)
        {
            tscConnectTo.Items.Remove(name);
            Settings.DeleteFavorite(name);
            favoritesToolStripMenuItem.DropDownItems.RemoveByKey(name);
        }

        void serverToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FavoriteConfigurationElement favorite = Settings.GetFavorites()[((ToolStripItem)(sender)).Text];
            CreateTerminalTab(favorite);
        }

        private string GetToolTipText(FavoriteConfigurationElement favorite)
        {
            string toolTip = "";
            if(favorite != null)
            {
                toolTip =
                    "Computer: " + favorite.ServerName + Environment.NewLine +
                    "User: " + Functions.UserDisplayName(favorite.DomainName, favorite.UserName) + Environment.NewLine;

                if(Settings.ShowFullInformationToolTips)
                {
                    toolTip +=
                    "Port: " + favorite.Port + Environment.NewLine +
                    "Connect to Console: " + favorite.ConnectToConsole.ToString() + Environment.NewLine +
                    "Notes: " + favorite.Notes + Environment.NewLine;

                    //"Desktop size: " + favorite.DesktopSize.ToString() + Environment.NewLine +
                    //"Colors: " + favorite.Colors.ToString() + Environment.NewLine +
                    //"Redirect drives: " + favorite.RedirectDrives.ToString() + Environment.NewLine +
                    //"Redirect ports: " + favorite.RedirectPorts.ToString() + Environment.NewLine +
                    //"Redirect printers: " + favorite.RedirectPrinters.ToString() + Environment.NewLine +
                    //"Sounds: " + favorite.Sounds.ToString() + Environment.NewLine;
                }
            }
            return toolTip;
        }

        private void CreateTerminalTab(FavoriteConfigurationElement favorite)
        {
            if(Settings.ExecuteBeforeConnect)
            {
                ProcessStartInfo processStartInfo = new ProcessStartInfo(Settings.ExecuteBeforeConnectCommand,
                    Settings.ExecuteBeforeConnectArgs);
                processStartInfo.WorkingDirectory = Settings.ExecuteBeforeConnectInitialDirectory;
                Process process = Process.Start(processStartInfo);
                if(Settings.ExecuteBeforeConnectWaitForExit)
                {
                    process.WaitForExit();
                }
            }

            if(favorite.ExecuteBeforeConnect)
            {
                ProcessStartInfo processStartInfo = new ProcessStartInfo(favorite.ExecuteBeforeConnectCommand,
                    favorite.ExecuteBeforeConnectArgs);
                processStartInfo.WorkingDirectory = favorite.ExecuteBeforeConnectInitialDirectory;
                Process process = Process.Start(processStartInfo);
                if(favorite.ExecuteBeforeConnectWaitForExit)
                {
                    process.WaitForExit();
                }
            }

            string terminalTabTitle = favorite.Name;
            if(Settings.ShowUserNameInTitle)
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
                if(conn.Connect())
                {
                    (conn as Control).BringToFront();
                    (conn as Control).Update();
                    UpdateControls();
                    if(favorite.DesktopSize == DesktopSize.FullScreen)
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

                if(conn.Connected) {
                    if(favorite.NewWindow) {
                        OpenConnectionInNewWindow(conn);
                    }
                }
            }
            catch(Exception exc)
            {
                Terminals.Logging.Log.Info("", exc);
                tcTerminals.SelectedItem = null;
                //tcTerminals = null;
                //System.Windows.Forms.MessageBox.Show(exc.Message);
            }
        }

        void BuildTerminalServerButtonMenu()
        {
            TerminalServerMenuButton.DropDownItems.Clear();

            if(this.CurrentConnection != null && this.CurrentConnection.IsTerminalServer)
            {

                //if(IsTerminalServer) {
                ToolStripMenuItem Sessions = new ToolStripMenuItem(Program.Resources.GetString("Sessions"));
                Sessions.Tag = this.CurrentConnection.Server;
                TerminalServerMenuButton.DropDownItems.Add(Sessions);
                ToolStripMenuItem Svr = new ToolStripMenuItem(Program.Resources.GetString("Server"));
                Svr.Tag = this.CurrentConnection.Server;
                TerminalServerMenuButton.DropDownItems.Add(Svr);
                //TerminalServices.TerminalServicesAPI.ShutdownSystem(
                ToolStripMenuItem sd = new ToolStripMenuItem(Program.Resources.GetString("Shutdown"));
                sd.Click += new EventHandler(sd_Click);
                sd.Tag = this.CurrentConnection.Server;
                Svr.DropDownItems.Add(sd);
                ToolStripMenuItem rb = new ToolStripMenuItem(Program.Resources.GetString("Reboot"));
                rb.Click += new EventHandler(sd_Click);
                rb.Tag = this.CurrentConnection.Server;
                Svr.DropDownItems.Add(rb);


                if(this.CurrentConnection.Server.Sessions != null)
                {
                    foreach(TerminalServices.Session session in this.CurrentConnection.Server.Sessions)
                    {
                        if(session.Client.ClientName != "")
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

                            if(session.IsTheActiveSession)
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

        void sd_Click(object sender, EventArgs e)
        {
            ToolStripMenuItem menu = (sender as ToolStripMenuItem);
            if(menu != null)
            {
                if(menu.Text == Program.Resources.GetString("Shutdown"))
                {
                    TerminalServices.TerminalServer server = (menu.Tag as TerminalServices.TerminalServer);
                    if (server != null && System.Windows.Forms.MessageBox.Show(Program.Resources.GetString("Areyousureyouwanttoshutthismachineoff"), Program.Resources.GetString("Confirmation"), MessageBoxButtons.OKCancel) == DialogResult.OK)
                        TerminalServices.TerminalServicesAPI.ShutdownSystem(server, false);
                }
                else if(menu.Text == Program.Resources.GetString("Reboot"))
                {
                    TerminalServices.TerminalServer server = (menu.Tag as TerminalServices.TerminalServer);
                    if (server != null && System.Windows.Forms.MessageBox.Show(Program.Resources.GetString("Areyousureyouwanttorebootthismachine"), Program.Resources.GetString("Confirmation"), MessageBoxButtons.OKCancel) == DialogResult.OK)
                        TerminalServices.TerminalServicesAPI.ShutdownSystem(server, true);
                }
                else if(menu.Text == Program.Resources.GetString("Logoff"))
                {
                    TerminalServices.Session session = (menu.Tag as TerminalServices.Session);
                    if (session != null && System.Windows.Forms.MessageBox.Show(Program.Resources.GetString("Areyousureyouwanttologthissessionoff"), Program.Resources.GetString("Confirmation"), MessageBoxButtons.OKCancel) == DialogResult.OK)
                        TerminalServices.TerminalServicesAPI.LogOffSession(session, false);

                }
                else if(menu.Text == Program.Resources.GetString("Send Message"))
                {
                    TerminalServices.Session session = (menu.Tag as TerminalServices.Session);
                    if(session != null)
                    {
                        Terminals.InputBoxResult result = Terminals.InputBox.Show(Program.Resources.GetString("Pleaseenterthemessagetosend"));
                        if(result.ReturnCode == DialogResult.OK && result.Text.Trim() != null)
                        {
                            TerminalServices.TerminalServicesAPI.SendMessage(session, Program.Resources.GetString("MessagefromyourAdministrator"), result.Text.Trim(), 0, 10, false);
                        }

                    }
                }

            }
        }
        void b_OnTerminalServerStateDiscovery(FavoriteConfigurationElement Favorite, bool IsTerminalServer, TerminalServices.TerminalServer Server)
        {
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


        void terminalTabPage_DragEnter(object sender, DragEventArgs e)
        {
            if(e.Data.GetDataPresent(DataFormats.FileDrop, false))
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


        public string GetDesktopShare()
        {
            string desktopShare = ((TerminalTabControlItem)(tcTerminals.SelectedItem)).Favorite.DesktopShare;
            if(String.IsNullOrEmpty(desktopShare))
            {
                desktopShare = Settings.DefaultDesktopShare.Replace("%SERVER%", CurrentTerminal.Server).Replace("%USER%", CurrentTerminal.UserName);
            }
            return desktopShare;
        }

        void terminalTabPage_DoubleClick(object sender, EventArgs e)
        {
            if(tcTerminals.SelectedItem != null)
            {
                tsbDisconnect.PerformClick();
            }
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
            }
            catch(Exception e)
            {
                Terminals.Logging.Log.Info("", e);
                //MessageBox.Show(this, e.Message);
            }
        }

        private void SetGrabInput(bool grab)
        {
            if(CurrentTerminal != null)
            {
                if(grab && !CurrentTerminal.ContainsFocus)
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
            using(NewTerminalForm frmNewTerminal = new NewTerminalForm(name, true))
            {
                if(frmNewTerminal.ShowDialog() == DialogResult.OK)
                {
                    Settings.AddFavorite(frmNewTerminal.Favorite, frmNewTerminal.ShowOnToolbar);
                    LoadFavorites();
                    //LoadFavorites(txtSearchFavorites.Text);
                    //LoadTags(txtSearchTags.Text);
                    tscConnectTo.SelectedIndex = tscConnectTo.Items.IndexOf(frmNewTerminal.Favorite.Name);
                    CreateTerminalTab(frmNewTerminal.Favorite);
                }
            }
            LoadFavorites();
        }

        private void newTerminalToolStripMenuItem_Click(object sender, EventArgs e)
        {
            CreateNewTerminal();
        }

        private void tsbConnect_Click(object sender, EventArgs e)
        {
            string connectionName = tscConnectTo.Text;
            if(connectionName != "")
            {
                Connect(connectionName, false, false);
            }
        }

        public void Connect(string connectionName, bool ForceConsole, bool ForceNewWindow)
        {
            FavoriteConfigurationElementCollection favorites = Settings.GetFavorites();
            FavoriteConfigurationElement favorite = favorites[connectionName];

            if(ForceConsole) favorite.ConnectToConsole = true;
            if(ForceNewWindow) favorite.NewWindow = true;

            if(favorite == null)
                CreateNewTerminal(connectionName);
            else
                CreateTerminalTab(favorite);
        }

        private void tscConnectTo_KeyDown(object sender, KeyEventArgs e)
        {
            if(e.KeyCode == Keys.Enter)
            {
                tsbConnect.PerformClick();
            }
            if(e.KeyCode == Keys.Delete && tscConnectTo.DroppedDown &&
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
            catch(Exception exc)
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

        public void tsbGrabInput_Click(object sender, EventArgs e)
        {
            ToggleGrabInput();
        }

        public void ToggleGrabInput()
        {
            if(CurrentTerminal != null)
            {
                CurrentTerminal.FullScreen = !CurrentTerminal.FullScreen;
            }
        }



        private void MainForm_KeyDown(object sender, KeyEventArgs e)
        {
            if(e.KeyValue == 3)
            {
                ToggleGrabInput();
            }
        }

        bool stdToolbarState = true;
        bool specialToolbarState = true;
        bool favToolbarState = true;

        private void HideToolBar(bool fullScreen)
        {
            if(!fullScreen)
            {
                toolbarStd.Visible = stdToolbarState;
                SpecialCommandsToolStrip.Visible = specialToolbarState;
                favoriteToolBar.Visible = favToolbarState;
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
            tcTerminals.ShowTabs = !fullScreen;
            tcTerminals.ShowBorder = !fullScreen;

            if(fullScreen)
            {
                stdToolbarState = toolbarStd.Visible;
                specialToolbarState = SpecialCommandsToolStrip.Visible;
                favToolbarState = favoriteToolBar.Visible;
            }
            HideToolBar(fullScreen);

            if(fullScreen)
            {
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
            }
            else
            {
                this.TopMost = false;
                this.FormBorderStyle = FormBorderStyle.Sizable;
                this.WindowState = this.lastState;
                if(lastState != FormWindowState.Minimized)
                {
                    if(lastState == FormWindowState.Normal)
                        this.Location = this.lastLocation;
                    this.Size = this.lastSize;
                }
                menuStrip.Visible = true;
            }
            this.fullScreen = fullScreen;
            //if(CurrentConnection != null) this.CurrentConnection.ChangeDesktopSize(DesktopSize.FullScreen);
            this.PerformLayout();
        }

        private void tcTerminals_TabControlItemClosing(TabControlItemClosingEventArgs e)
        {

            if(CurrentConnection != null && CurrentConnection.Connected)
            {
                bool close = false;
                if(Settings.WarnOnConnectionClose)
                {
                    close = (MessageBox.Show(this, Program.Resources.GetString("Areyousurethatyouwanttodisconnectfromtheactiveterminal"),
                   Program.Resources.GetString("Terminals"), MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes);
                }
                else
                {
                    close = true;
                }
                if(close)
                {
                    if(CurrentTerminal != null) CurrentTerminal.Disconnect();
                    if(CurrentConnection != null) CurrentConnection.Disconnect();
                    this.Text = Program.AboutText;
                    e.Cancel = false;
                }
                else
                {
                    e.Cancel = true;
                }
            }
            else
            {
                e.Cancel = false;
            }
        }


        private void QuickConnect(string server, int port, bool ConnectToConsole)
        {
            FavoriteConfigurationElementCollection favorites = Settings.GetFavorites();
            FavoriteConfigurationElement favorite = favorites[server];
            if(favorite != null) {
                if(favorite.ConnectToConsole != ConnectToConsole) favorite.ConnectToConsole = ConnectToConsole;
                CreateTerminalTab(favorite);
            } else {
                //create a temporaty favorite and connect to it
                favorite = new FavoriteConfigurationElement();
                favorite.ConnectToConsole = ConnectToConsole;
                favorite.ServerName = server;
                favorite.Name = server;
                if(port != 0)
                    favorite.Port = port;
                CreateTerminalTab(favorite);
            }
        }

        private void MainForm_Shown(object sender, EventArgs e)
        {
            HandleCommandLineActions();
        }
        private void HandleCommandLineActions()
        {
            
            bool ConnectToConsole = Terminals.MainForm.CommandLineArgs.console;
            this.FullScreen = Terminals.MainForm.CommandLineArgs.fullscreen;
            if(Terminals.MainForm.CommandLineArgs.url != null && Terminals.MainForm.CommandLineArgs.url != "")
            {
                string server; int port;
                ProtocolHandler.Parse(Terminals.MainForm.CommandLineArgs.url, out server, out port);
                QuickConnect(server, port, ConnectToConsole);
            }
            if(Terminals.MainForm.CommandLineArgs.machine != null && Terminals.MainForm.CommandLineArgs.machine != "") {
                string server=""; int port=3389;
                server = Terminals.MainForm.CommandLineArgs.machine;
                int index = Terminals.MainForm.CommandLineArgs.machine.IndexOf(":");
                if(index>0) {
                    server = Terminals.MainForm.CommandLineArgs.machine.Substring(0, index);
                    string p = Terminals.MainForm.CommandLineArgs.machine.Substring(index+1);
                    if(!int.TryParse(p, out port)) {
                        port = 3389;
                    }
                }
                QuickConnect(server, port, ConnectToConsole);
            } if(Terminals.MainForm.CommandLineArgs.favs != null && Terminals.MainForm.CommandLineArgs.favs != "")
            {
                string favs = Terminals.MainForm.CommandLineArgs.favs;
                if(favs.Contains(","))
                {
                    string[] favlist = favs.Split(',');
                    foreach(string fav in favlist)
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

        private void tcTerminals_TabControlItemSelectionChanged(TabControlItemChangedEventArgs e)
        {
            UpdateControls();
            if(tcTerminals.Items.Count > 0)
            {
                tsbDisconnect.Enabled = e.Item != null;
                disconnectToolStripMenuItem.Enabled = e.Item != null;
                SetGrabInput(true);
                if(e.Item.Selected && Settings.ShowInformationToolTips) this.Text = e.Item.ToolTipText.Replace("\r\n", "; ");
            }
        }

        private void tscConnectTo_TextChanged(object sender, EventArgs e)
        {
            UpdateControls();
        }

        private void tcTerminals_MouseHover(object sender, EventArgs e)
        {
            if(tcTerminals != null)
            {
                if(!tcTerminals.ShowTabs)
                {
                    timerHover.Enabled = true;
                    //tcTerminals.ShowTabs = true;
                }
            }
        }

        private void tcTerminals_MouseLeave(object sender, EventArgs e)
        {
            timerHover.Enabled = false;
            if(FullScreen && tcTerminals.ShowTabs && !tcTerminals.MenuOpen)
            {
                tcTerminals.ShowTabs = false;
            }
            if(currentToolTipItem != null)
            {
                currentToolTip.Hide(currentToolTipItem);
            }
            /*if (previewPictureBox != null)
            {
                previewPictureBox.Hide();
            }*/
        }
        
        public bool FullScreen
        {
            get
            {
                return fullScreen;
            }
            set
            {
                if(FullScreen != value) SetFullScreen(value);
                if (!FullScreen)
                {
                    ResetToolbars();  //try to restore the toolbars
                }
            }
        }


        public void tcTerminals_TabControlItemClosed(object sender, EventArgs e)
        {
            this.Text = Program.AboutText;
            if(tcTerminals.Items.Count == 0)
                FullScreen = false;
        }

        private void tcTerminals_DoubleClick(object sender, EventArgs e)
        {
            FullScreen = !fullScreen;
            // UpdateControls();
        }

        private void tsbFullScreen_Click(object sender, EventArgs e)
        {
            FullScreen = !FullScreen;
            UpdateControls();
        }

        private void tcTerminals_MenuItemsLoaded(object sender, EventArgs e)
        {
            foreach(ToolStripItem item in tcTerminals.Menu.Items)
            {
                item.Image = Terminals.Properties.Resources.smallterm;
            }
            string f = "";
            if(FullScreen)
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
            Screen farRightScreen=null;
            foreach(Screen screen in System.Windows.Forms.Screen.AllScreens) {
                if(farRightScreen == null)
                    farRightScreen = screen;
                else
                    if(screen.Bounds.X > farRightScreen.Bounds.X) farRightScreen = screen;
            }
            if(this.Location.X > farRightScreen.Bounds.X + farRightScreen.Bounds.Width) this.Location = new Point(0, 0);


            if(FullScreen)
                tcTerminals.ShowTabs = false;
        }
        public void ShowManagedConnections() {
            using(OrganizeFavoritesForm conMgr = new OrganizeFavoritesForm()) {
                conMgr.ShowDialog();
                //LoadFavorites(txtSearchFavorites.Text);
                //LoadTags(txtSearchTags.Text);
            }
            LoadFavorites();
        }
        private void manageConnectionsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ShowManagedConnections();
        }

        private void saveTerminalsAsGroupToolStripMenuItem_Click(object sender, EventArgs e)
        {
            using(NewGroupForm frmNewGroup = new NewGroupForm())
            {
                if(frmNewGroup.ShowDialog() == DialogResult.OK)
                {
                    GroupConfigurationElement serversGroup = new GroupConfigurationElement();
                    serversGroup.Name = frmNewGroup.txtGroupName.Text;
                    foreach(TabControlItem tabControlItem in tcTerminals.Items)
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
            using(OrganizeGroupsForm frmOrganizeGroups = new OrganizeGroupsForm())
            {
                frmOrganizeGroups.ShowDialog();
                LoadGroups();
            }
        }

        private void SaveActiveConnections()
        {
            List<String> activeConnections = new List<string>();
            foreach(TabControlItem item in tcTerminals.Items)
            {
                activeConnections.Add(item.Title);
            }
            Settings.CreateSavedConnectionsList(activeConnections.ToArray());
        }

        private void MainForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            if(FullScreen) FullScreen = false;
            this.MainWindowNotifyIcon.Visible = false;

            if(tcTerminals.Items.Count > 0)
            {
                if(Settings.ShowConfirmDialog)
                {
                    SaveActiveConnectionsForm frmSaveActiveConnections = new SaveActiveConnectionsForm();
                    if(frmSaveActiveConnections.ShowDialog() == DialogResult.OK)
                    {
                        Settings.ShowConfirmDialog = !frmSaveActiveConnections.chkDontShowDialog.Checked;
                        if(frmSaveActiveConnections.chkOpenOnNextTime.Checked)
                        {
                            SaveActiveConnections();
                        }
                        e.Cancel = false;
                    }
                    else
                    {
                        e.Cancel = true;
                    }
                }
                else if(Settings.SaveConnectionsOnClose)
                {
                    SaveActiveConnections();
                }
            }
            SaveWindowState();
        }

        private void timerHover_Tick(object sender, EventArgs e)
        {
            if(timerHover.Enabled)
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
            if(frmOptions.ShowDialog() == DialogResult.OK)
            {
                this.groupsToolStripMenuItem.Visible = Settings.EnableGroupsMenu;
                HideShowFavoritesPanel(Settings.EnableFavoritesPanel);


                this.MainWindowNotifyIcon.Visible = Settings.MinimizeToTray;

                if(Settings.Office2007BlueFeel)
                    ToolStripManager.Renderer = Office2007Renderer.Office2007Renderer.GetRenderer(Office2007Renderer.RenderColors.Blue);
                else if(Settings.Office2007BlackFeel)
                    ToolStripManager.Renderer = Office2007Renderer.Office2007Renderer.GetRenderer(Office2007Renderer.RenderColors.Black);
                else
                    ToolStripManager.Renderer = new System.Windows.Forms.ToolStripProfessionalRenderer();

                tcTerminals.ShowToolTipOnTitle = Settings.ShowInformationToolTips;
                if(tcTerminals.SelectedItem != null)
                {
                    tcTerminals.SelectedItem.ToolTipText = GetToolTipText(((TerminalTabControlItem)tcTerminals.SelectedItem).Favorite);
                }
            }
        }

        private void aboutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            using(AboutForm frmAbout = new AboutForm()) {
                frmAbout.ShowDialog();
            }
        }

        private TabControlItem currentToolTipItem = null;
        private ToolTip currentToolTip = new ToolTip();

        private void tcTerminals_TabControlMouseOnTitle(TabControlMouseOnTitleEventArgs e)
        {
            if(Settings.ShowInformationToolTips)
            {
                //ToolTip
                if((currentToolTipItem != null) && (currentToolTipItem != e.Item))
                {
                    currentToolTip.Hide(currentToolTipItem);
                }
                currentToolTip.ToolTipTitle = Program.Resources.GetString("Connectioninformation");
                currentToolTip.ToolTipIcon = ToolTipIcon.Info;
                currentToolTip.UseFading = true;
                currentToolTip.UseAnimation = true;
                currentToolTip.IsBalloon = false;
                currentToolTip.Show(e.Item.ToolTipText, e.Item, (int)e.Item.StripRect.X, 2);
                currentToolTipItem = e.Item;
            }
        }

        private void tcTerminals_TabControlMouseLeftTitle(TabControlMouseOnTitleEventArgs e)
        {
            if(currentToolTipItem != null)
            {
                currentToolTip.Hide(currentToolTipItem);
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
            if(Settings.ShowInformationToolTips)
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

        private void tsbTags_Click(object sender, EventArgs e)
        {
            if(tsbTags.Checked)
            {
                ShowTags();
            }
            else
            {
                HideTagsFavorites();
            }
        }

        private void pbShowTags_Click(object sender, EventArgs e)
        {
            ShowTags();
        }

        private void pbHideTags_Click(object sender, EventArgs e)
        {
            HideTagsFavorites();
        }

        private void LoadFavorites(string filter)
        {
            //lvFavorites.Items.Clear();
            //SortedDictionary<string, FavoriteConfigurationElement> favorites = Settings.GetSortedFavorites(Settings.SortProperties.ConnectionName);
            //foreach(string key in favorites.Keys)
            //{
            //    FavoriteConfigurationElement favorite = favorites[key];
            //    if((String.IsNullOrEmpty(filter) || (favorite.Name.ToUpper().StartsWith(filter.ToUpper()))))
            //    {
            //        ListViewItem item = new ListViewItem();
            //        item.ImageIndex = 0;
            //        item.StateImageIndex = 0;
            //        item.Tag = favorite;
            //        item.Text = favorite.Name;
            //        lvFavorites.Items.Add(item);
            //    }
            //}
        }


        private void LoadTags(string filter)
        {
            //lvTags.Items.Clear();

            ListViewItem unTaggedListViewItem = new ListViewItem();
            unTaggedListViewItem.ImageIndex = 0;
            unTaggedListViewItem.StateImageIndex = 0;
            unTaggedListViewItem.ToolTipText = Program.Resources.GetString("UnTagged");
            List<FavoriteConfigurationElement> unTaggedFavorites = new List<FavoriteConfigurationElement>();
            foreach(string tag in Settings.Tags)
            {
                if((String.IsNullOrEmpty(filter) || (tag.ToUpper().StartsWith(filter.ToUpper()))))
                {
                    ListViewItem item = new ListViewItem();
                    item.ImageIndex = 0;
                    item.StateImageIndex = 0;
                    SortedDictionary<string, FavoriteConfigurationElement> favorites = Settings.GetSortedFavorites(Settings.SortProperties.ConnectionName);

                    List<FavoriteConfigurationElement> tagFavorites = new List<FavoriteConfigurationElement>();
                    foreach(string key in favorites.Keys)
                    {
                        FavoriteConfigurationElement favorite = favorites[key];
                        if(favorite.TagList.IndexOf(tag) >= 0)
                        {
                            tagFavorites.Add(favorite);
                        }
                        else if(favorite.TagList.Count == 0)
                        {
                            if(unTaggedFavorites.IndexOf(favorite) < 0)
                            {
                                unTaggedFavorites.Add(favorite);
                            }
                        }
                    }
                    item.Tag = tagFavorites;
                    item.Text = tag + " (" + tagFavorites.Count.ToString() + ")";
                    item.ToolTipText = tag;
                    //lvTags.Items.Add(item);
                }
            }
            if(Settings.Tags.Length == 0)
            {
                FavoriteConfigurationElementCollection favorites = Settings.GetFavorites();
                List<FavoriteConfigurationElement> tagFavorites = new List<FavoriteConfigurationElement>();
                foreach(FavoriteConfigurationElement favorite in favorites)
                {
                    if(unTaggedFavorites.IndexOf(favorite) < 0)
                    {
                        unTaggedFavorites.Add(favorite);
                    }
                }
            }
            unTaggedListViewItem.Tag = unTaggedFavorites;
            unTaggedListViewItem.Text = Program.Resources.GetString("UnTagged") + " (" + unTaggedFavorites.Count.ToString() + ")";
            //lvTags.Items.Add(unTaggedListViewItem);
            //lvTags.AutoResizeColumns(ColumnHeaderAutoResizeStyle.ColumnContent);
        }

        private void ShowTagsFavorites(/*TabControlItem activeTab*/)
        {
            splitContainer1.SplitterDistance = Settings.FavoritePanelWidth;
            //pnlTagsFavorites.Width = 300;
            //tcTagsFavorites.SelectedItem = activeTab;
            pnlHideTagsFavorites.Show();
            pnlShowTagsFavorites.Hide();
            //txtSearchTags.Focus();
        }

        private void ShowTags()
        {
            ShowTagsFavorites(/*tciTags*/);
            tsbFavorites.Checked = false;
        }

        private void HideTagsFavorites()
        {
            if(Settings.EnableFavoritesPanel) {
                if(Settings.FavoritePanelWidth != splitContainer1.SplitterDistance) Settings.FavoritePanelWidth = splitContainer1.SplitterDistance;
                splitContainer1.SplitterDistance = 7;
                pnlHideTagsFavorites.Hide();
                pnlShowTagsFavorites.Show();
                tsbTags.Checked = false;
                tsbFavorites.Checked = false;
            } else {
                splitContainer1.SplitterDistance = 0;
                pnlHideTagsFavorites.Visible = false;
            }
            if(Settings.FavoritePanelWidth <= 150) Settings.FavoritePanelWidth = 150;
        }

        private void ShowFavorites()
        {
            //ShowTagsFavorites(tciFavorites);
            tsbTags.Checked = false;
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
            if(tsbFavorites.Checked)
            {
                ShowFavorites();
            }
            else
            {
                HideTagsFavorites();
            }
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
                originalFormWindowState = this.WindowState;
            }
        }

        FormWindowState originalFormWindowState;
        private void Minimize(object sender, EventArgs e)
        {
            originalFormWindowState = this.WindowState;
            this.WindowState = FormWindowState.Minimized;
        }

        private void MainWindowNotifyIcon_MouseClick(object sender, MouseEventArgs e)
        {
            if(e.Button == MouseButtons.Left)
            {                
                if(Settings.MinimizeToTray)
                {
                    this.Visible = !this.Visible;
                    if(this.Visible && this.WindowState == FormWindowState.Minimized)
                        this.WindowState = originalFormWindowState;

                }
                else
                {
                    if (this.WindowState == FormWindowState.Normal)
                    {
                        originalFormWindowState = this.WindowState;
                        this.WindowState = FormWindowState.Minimized;
                    }
                    else
                    {
                        this.WindowState = originalFormWindowState;
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
            //SystemTracyQuickConnectToolStripMenuItem
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
            if(CurrentConnection != null)
            {
                Connections.VMRCConnection vmrc;
                vmrc = (CurrentConnection as Connections.VMRCConnection);
                if(vmrc != null)
                {
                    vmrc.AdminDisplay();
                }
            }
        }

        private void VMRCViewOnlyButton_Click(object sender, EventArgs e)
        {
            if(CurrentConnection != null)
            {
                Connections.VMRCConnection vmrc;
                vmrc = (CurrentConnection as Connections.VMRCConnection);
                if(vmrc != null)
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

            foreach(string key in favorites.Keys)
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
                if(!CurrentTerminal.AdvancedSettings3.ConnectToServerConsole)
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
                if(!CurrentTerminal.AdvancedSettings3.ConnectToServerConsole)
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
            if(e.Button == MouseButtons.Left)
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

        //private void mainMenuToolStripMenuItem_Click(object sender, EventArgs e)
        //{

        //    MainMenuHolder.Visible = !MainMenuHolder.Visible;
        //    mainMenuToolStripMenuItem.Checked = MainMenuHolder.Visible;
        //}

        private void standardToolbarToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if(!Settings.ToolbarsLocked)
            {
                toolbarStd.Visible = !toolbarStd.Visible;
                standardToolbarToolStripMenuItem.Checked = toolbarStd.Visible;
            }
            else
            {
                System.Windows.Forms.MessageBox.Show(Program.Resources.GetString("Inordertochangethetoolbarsyoumustfirstunlockthem"));
            }
        }

        private void toolStripMenuItem4_Click(object sender, EventArgs e)
        {
            if(!Settings.ToolbarsLocked)
            {

                favoriteToolBar.Visible = !favoriteToolBar.Visible;
                toolStripMenuItem4.Checked = favoriteToolBar.Visible;
            }
            else
            {
                System.Windows.Forms.MessageBox.Show(Program.Resources.GetString("Inordertochangethetoolbarsyoumustfirstunlockthem"));
            }
        }

        private void shortcutsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if(!Settings.ToolbarsLocked)
            {
                SpecialCommandsToolStrip.Visible = !SpecialCommandsToolStrip.Visible;
                shortcutsToolStripMenuItem.Checked = SpecialCommandsToolStrip.Visible;
            }
            else
            {
                System.Windows.Forms.MessageBox.Show(Program.Resources.GetString("Inordertochangethetoolbarsyoumustfirstunlockthem"));
            } 
        }

        private void toolsToolStripMenuItem_DropDownOpening(object sender, EventArgs e)
        {
            this.shortcutsToolStripMenuItem.Checked = this.SpecialCommandsToolStrip.Visible;
            //this.mainMenuToolStripMenuItem.Checked = this.MainMenuHolder.Visible;
            this.toolStripMenuItem4.Checked = this.favoriteToolBar.Visible;
            this.standardToolbarToolStripMenuItem.Checked = this.toolbarStd.Visible;

        }

        private void toolStripMenuItem3_Click(object sender, EventArgs e)
        {
            OrganizeShortcuts org = new OrganizeShortcuts();
            org.ShowDialog(this);
            if(Settings.EnableFavoritesPanel) LoadTags(null);
            this.Invoke(specialCommandsMIV);
        }



        private void ShortcutsContextMenu_MouseClick(object sender, MouseEventArgs e)
        {
            toolStripMenuItem3_Click(null, null);
        }

        private void SpecialCommandsToolStrip_MouseClick(object sender, MouseEventArgs e)
        {
            if(e.Button == MouseButtons.Right) ShortcutsContextMenu.Show(e.X, e.Y);
        }

        private void SpecialCommandsToolStrip_ItemClicked_1(object sender, ToolStripItemClickedEventArgs e)
        {
            SpecialCommandConfigurationElement elm = (e.ClickedItem.Tag as SpecialCommandConfigurationElement);
            if(elm != null) elm.Launch();
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
            catch(Exception exc)
            {
                Terminals.Logging.Log.Info("", exc);
                tcTerminals.Items.Remove(terminalTabPage);
                tcTerminals.SelectedItem = null;
                terminalTabPage.Dispose();
                //System.Windows.Forms.MessageBox.Show(exc.Message);
            }
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
            foreach(TerminalTabControlItem tab in tcTerminals.Items)
            {
                if(tab.Title == Program.Resources.GetString("CaptureManager"))
                {
                    Terminals.Connections.CaptureManagerConnection conn = (tab.Connection as Terminals.Connections.CaptureManagerConnection);
                    conn.RefreshView();
                    if(Settings.AutoSwitchOnCapture) tcTerminals.SelectedItem = tab;
                    createNew = false;
                    break;
                }
            }
            if(createNew)
            {
                if(sender == null && !Settings.AutoSwitchOnCapture) createNew = false;
            }
            if(createNew)
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
                catch(Exception exc)
                {
                    Terminals.Logging.Log.Info("", exc);
                    tcTerminals.Items.Remove(terminalTabPage);
                    tcTerminals.SelectedItem = null;
                    terminalTabPage.Dispose();
                    //System.Windows.Forms.MessageBox.Show(exc.Message);
                }
            }
        }

        private void toolStripButton3_Click(object sender, EventArgs e)
        {
            bool origval = Settings.AutoSwitchOnCapture;
            if(!Settings.AutoSwitchOnCapture)
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

            if(sender != null && (sender as ToolStripMenuItem) != null)
            {
                string key = (sender as ToolStripMenuItem).Text;
                Connections.VNCConnection vnc;
                if(CurrentConnection != null)
                {
                    vnc = (this.CurrentConnection as Connections.VNCConnection);
                    if(vnc != null)
                    {
                        if(key == sendALTF4KeyToolStripMenuItem.Text)
                        {
                            vnc.SendSpecialKeys(VncSharp.SpecialKeys.AltF4);
                        }
                        else if(key == sendALTKeyToolStripMenuItem.Text)
                        {
                            vnc.SendSpecialKeys(VncSharp.SpecialKeys.Alt);

                        }
                        else if(key == sendCTRLESCKeysToolStripMenuItem.Text)
                        {
                            vnc.SendSpecialKeys(VncSharp.SpecialKeys.CtrlEsc);

                        }
                        else if(key == sendCTRLKeyToolStripMenuItem.Text)
                        {
                            vnc.SendSpecialKeys(VncSharp.SpecialKeys.Ctrl);

                        }
                        else if(key == sentCTRLALTDELETEKeysToolStripMenuItem.Text)
                        {
                            vnc.SendSpecialKeys(VncSharp.SpecialKeys.CtrlAltDel);
                        }

                    }
                }

            }
        }

        public void ShowManageTerminalForm(FavoriteConfigurationElement Favorite) {
            using(NewTerminalForm frmNewTerminal = new NewTerminalForm(Favorite)) {
                frmNewTerminal.ShowDialog();
                LoadFavorites();
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
            if(tcTerminals.SelectedItem != null)
            {
                TerminalTabControlItem terminalTabPage = (TerminalTabControlItem)tcTerminals.SelectedItem;
                if(terminalTabPage.Connection != null)
                {
                    terminalTabPage.Connection.ChangeDesktopSize(terminalTabPage.Connection.Favorite.DesktopSize);
                }
            }
        }

        private void pbShowTagsFavorites_MouseMove(object sender, MouseEventArgs e)
        {
            if(Settings.AutoExapandTagsPanel) ShowTags();
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

            foreach(string format in e.Data.GetFormats())
            {
                if(format.Equals("System.Windows.Forms.ListView+SelectedListViewItemCollection"))
                {
                    e.Effect = DragDropEffects.Move;
                }
            }
        }

        private void AddTagToFavorite(FavoriteConfigurationElement Favorite, string Tag)
        {
            List<string> tagList = new List<string>();
            foreach(string tag in Favorite.TagList)
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
            foreach(string tag in Favorite.TagList)
            {
                if(tag.Trim().ToUpper() != t) tagList.Add(tag);
            }
            Favorite.Tags = String.Join(",", tagList.ToArray());
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
            foreach(ToolStripPanelRow row in toolStripContainer.TopToolStripPanel.Rows) {
                foreach(Control c in row.Controls) {
                    ToolStrip item = (c as ToolStrip);
                    if(item != null) {
                        if(GripVisible) 
                            item.GripStyle = ToolStripGripStyle.Visible;
                        else
                            item.GripStyle = ToolStripGripStyle.Hidden;
                    }                    
                }
            }

            //if(Settings.ToolbarsLocked)
            //    strip.GripStyle = ToolStripGripStyle.Hidden;
            //else
            //    strip.GripStyle = ToolStripGripStyle.Hidden;

        }

        private static bool releaseAvailable = false;
        public static bool ReleaseAvailable
        {
            get
            {
                return releaseAvailable;
            }
            set
            {
                releaseAvailable = value;
                if(releaseAvailable) {
                    FavoriteConfigurationElementCollection favs = Settings.GetFavorites();
                    FavoriteConfigurationElement release = null;
                    foreach(FavoriteConfigurationElement fav in favs) {
                        if(fav.Name == TerminalsReleasesFavoriteName) {
                            release = fav;
                            break;
                        }
                    }
                    if(release == null) {
                        release = new FavoriteConfigurationElement(TerminalsReleasesFavoriteName);
                        release.Url = "http://www.codeplex.com/Terminals/Wiki/View.aspx?title=Welcome%20To%20Terminals";
                        release.Tags = Program.Resources.GetString("Terminals");
                        release.Protocol = "HTTP";
                        Settings.AddFavorite(release, false);
                    }
                    System.Threading.Thread.Sleep(5000);
                    if(OnReleaseIsAvailable != null) OnReleaseIsAvailable(release);
                }
            }
        }
        private static string TerminalsReleasesFavoriteName = Program.Resources.GetString("TerminalsNews");
        private static Unified.Rss.RssItem releaseDescription = null;
        public static Unified.Rss.RssItem ReleaseDescription
        {
            get
            {
                return releaseDescription;
            }
            set
            {
                releaseDescription = value;

            }
        }

        public delegate void ReleaseIsAvailable(FavoriteConfigurationElement ReleaseFavorite);
        public static event ReleaseIsAvailable OnReleaseIsAvailable;

        MethodInvoker releaseMIV;
        private void OpenReleasePage() {
            Connect(TerminalsReleasesFavoriteName, false, false);
        }
        private void MainForm_Load(object sender, EventArgs e)
        {
            releaseMIV = new MethodInvoker(OpenReleasePage);
            this.Text = Program.AboutText;
            MainForm.OnReleaseIsAvailable += new ReleaseIsAvailable(MainForm_OnReleaseIsAvailable);
        }

        void MainForm_OnReleaseIsAvailable(FavoriteConfigurationElement ReleaseFavorite) {
            this.Invoke(releaseMIV);            
        }

        private void updateToolStripItem_Click(object sender, EventArgs e)
        {
        }

        private void helpToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if(!updateToolStripItem.Visible)
            {
                if(ReleaseAvailable && updateToolStripItem != null)
                {
                    updateToolStripItem.Visible = ReleaseAvailable;
                    if(ReleaseDescription != null)
                    {
                        updateToolStripItem.Text = string.Format("{0} - {1}", updateToolStripItem.Text, ReleaseDescription.Title);
                    }
                }
            }
        }

        private void MainForm_KeyUp(object sender, KeyEventArgs e) {
            //handle global keyup events
            if(e.Control && e.KeyCode == Keys.F12) {
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
            LoadFavorites("");
            LoadGroups();
            UpdateControls();
            LoadTags("");
        }

        public void OpenConnectionInNewWindow(Terminals.Connections.IConnection Connection) {
            if(Connection != null) {
                PopupTerminal pop = new PopupTerminal();
                Terminals.Connections.IConnection conn = CurrentConnection;
                tcTerminals.Items.SuspendEvents();
                tcTerminals.RemoveTab(CurrentConnection.TerminalTabPage);
                pop.AddTerminal(conn.TerminalTabPage);
                pop.MainForm = this;
                //pop.Controls.Add(conn.TerminalTabPage);
                //pop.WindowState = FormWindowState.Maximized;
                //pop.FormBorderStyle = FormBorderStyle.None;
                tcTerminals.Items.ResumeEvents();
                pop.Show();
                //conn.TerminalTabPage.Focus();
            }
        }
        private void viewInNewWindowToolStripMenuItem_Click(object sender, EventArgs e) {
            if(this.CurrentConnection != null) {
                OpenConnectionInNewWindow(this.CurrentConnection);
            }
        }
        public void AddTerminal(TerminalTabControlItem TabControlItem) {
            this.tcTerminals.AddTab(TabControlItem);
        }

        private void ReloadSpecialCommands(object state)
        {
            while(!this.Created) System.Threading.Thread.Sleep(500);
            rebuildShortcutsToolStripMenuItem_Click(null, null);
        }
        private void rebuildShortcutsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Settings.SpecialCommands.Clear();
            Settings.SpecialCommands = Terminals.Wizard.SpecialCommandsWizard.LoadSpecialCommands();            
            this.Invoke(specialCommandsMIV);
        }

        private void rebuildToolbarsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            LoadWindowState();
        }

    }

    public class TerminalTabControlItem : TabControlItem {
        public TerminalTabControlItem(string caption)
            : base(caption, null) {
        }
        private Connections.IConnection connection;

        public Connections.IConnection Connection {
            get {
                return connection;
            }
            set {
                connection = value;
            }
        }


        private AxMsRdpClient2 terminalControl;

        public AxMsRdpClient2 TerminalControl {
            get {
                return terminalControl;
            }
            set {
                terminalControl = value;
            }
        }

        private FavoriteConfigurationElement favorite;

        public FavoriteConfigurationElement Favorite {
            get {
                return favorite;
            }
            set {
                favorite = value;
            }
        }
    }
}