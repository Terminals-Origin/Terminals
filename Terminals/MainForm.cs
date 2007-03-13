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

namespace Terminals
{
    public partial class MainForm : Form
    {
        const int WM_LEAVING_FULLSCREEN = 0x4ff;

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
        }

        void SingleInstanceApplication_NewInstanceMessage(object sender, object message)
        {
            if (WindowState == FormWindowState.Minimized)
                NativeApi.ShowWindow(new HandleRef(this, this.Handle), 9);
            Activate();
            string[] commandLine = (string[])message;
            if (commandLine != null)
                ParseCommandline(commandLine);
        }

        protected override void SetVisibleCore(bool value)
        {
            _formSettings.LoadFormSize();
            base.SetVisibleCore(value);
        }

        public AxMsRdpClient2 CurrentTerminal
        {
            get
            {
                if (tcTerminals.SelectedItem != null)
                    return ((TerminalTabControlItem)(tcTerminals.SelectedItem)).TerminalControl;
                else
                    return null;
            }
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void OpenSavedConnections()
        {
            foreach (string name in Settings.SavedConnections)
            {
                Connect(name);
            }
            Settings.ClearSavedConnectionsList();
        }

        private void UpdateControls()
        {
            tcTerminals.ShowToolTipOnTitle = Settings.ShowInformationToolTips;
            addTerminalToGroupToolStripMenuItem.Enabled = (tcTerminals.SelectedItem != null);
            tsbGrabInput.Enabled = (tcTerminals.SelectedItem != null);
            tsbFullScreen.Enabled = (tcTerminals.SelectedItem != null);
            grabInputToolStripMenuItem.Enabled = tcTerminals.SelectedItem != null;
            tsbGrabInput.Checked = tsbGrabInput.Enabled && (CurrentTerminal != null) && CurrentTerminal.FullScreen;
            grabInputToolStripMenuItem.Checked = tsbGrabInput.Checked;
            tsbConnect.Enabled = (tscConnectTo.Text != String.Empty);
            saveTerminalsAsGroupToolStripMenuItem.Enabled = (tcTerminals.Items.Count > 0);
        }

        private void LoadFavorites()
        {
            FavoriteConfigurationElementCollection favorites = Settings.GetFavorites();
            int seperatorIndex = favoritesToolStripMenuItem.DropDownItems.IndexOf(favoritesSeparator);
            for (int i = favoritesToolStripMenuItem.DropDownItems.Count - 1; i > seperatorIndex; i--)
            {
                favoritesToolStripMenuItem.DropDownItems.RemoveAt(i);
            }
            tscConnectTo.Items.Clear();
            foreach (FavoriteConfigurationElement favorite in favorites)
            {
                AddFavorite(favorite);
            }
            LoadFavoritesToolbar();
        }

        private void LoadFavoritesToolbar()
        {
            for (int i = toolbarStd.Items.Count - 1; i >= 0; i--)
            {
                ToolStripItem item = toolbarStd.Items[i];
                if (item.Tag is FavoriteConfigurationElement)
                    toolbarStd.Items.Remove(item);
            }
            foreach (string favoriteButton in Settings.FavoritesToolbarButtons)
            {
                FavoriteConfigurationElementCollection favorites = Settings.GetFavorites();
                FavoriteConfigurationElement favorite = favorites[favoriteButton];
                ToolStripButton favoriteBtn = new ToolStripButton(favorite.Name,
                    Terminals.Properties.Resources.smallterm, serverToolStripMenuItem_Click);
                favoriteBtn.Tag = favorite;
                toolbarStd.Items.Add(favoriteBtn);
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
            foreach (FavoriteAliasConfigurationElement favoriteAlias in serversGroup.FavoriteAliases)
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
            string toolTip =
                "Computer: " + favorite.ServerName + Environment.NewLine +
                "User: " + Functions.UserDisplayName(favorite.DomainName, favorite.UserName) + Environment.NewLine;

            if (Settings.ShowFullInformationToolTips)
            {
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

        private void CreateTerminalTab(FavoriteConfigurationElement favorite)
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
            terminalTabPage.AllowDrop = true;
            terminalTabPage.DragOver += terminalTabPage_DragOver;
            terminalTabPage.DragEnter += new DragEventHandler(terminalTabPage_DragEnter);
            terminalTabPage.ToolTipText = GetToolTipText(favorite);
            terminalTabPage.Favorite = favorite;
            terminalTabPage.DoubleClick += new EventHandler(terminalTabPage_DoubleClick);
            tcTerminals.Items.Add(terminalTabPage);
            tcTerminals.SelectedItem = terminalTabPage;
            tcTerminals_SelectedIndexChanged(this, EventArgs.Empty);
            AxMsRdpClient2 axMsRdpClient2 = new AxMsRdpClient2();
            Controls.Add(axMsRdpClient2);
            axMsRdpClient2.Parent = terminalTabPage;
            axMsRdpClient2.AllowDrop = true;

            ((Control)axMsRdpClient2).DragEnter += new DragEventHandler(axMsRdpClient2_DragEnter);
            ((Control)axMsRdpClient2).DragDrop += new DragEventHandler(axMsRdpClient2_DragDrop);
            axMsRdpClient2.Dock = DockStyle.Fill;
            int height = 0, width = 0;
            switch (favorite.DesktopSize)
            {
                case DesktopSize.x640:
                    width = 640;
                    height = 480;
                    break;
                case DesktopSize.x800:
                    width = 800;
                    height = 600;
                    break;
                case DesktopSize.x1024:
                    width = 1024;
                    height = 768;
                    break;
                case DesktopSize.FitToWindow:
                    width = terminalTabPage.Width;
                    height = terminalTabPage.Height;
                    break;
                case DesktopSize.FullScreen:
                    width = Screen.FromControl(this).Bounds.Width;
                    height = Screen.FromControl(this).Bounds.Height - 1;
                    break;
                case DesktopSize.AutoScale:
                    axMsRdpClient2.AdvancedSettings3.SmartSizing = true;
                    width = Screen.FromControl(this).Bounds.Width;
                    height = Screen.FromControl(this).Bounds.Height - 1;
                    break;
            }
            int maxWidth = Settings.SupportsRDP6 ? 4096 : 1600;
            int maxHeight = Settings.SupportsRDP6 ? 2048 : 1200;

            axMsRdpClient2.DesktopWidth = Math.Min(maxWidth, width);
            axMsRdpClient2.DesktopHeight = Math.Min(maxHeight, height); ;

            switch (favorite.Colors)
            {
                case Colors.Bits8:
                    axMsRdpClient2.ColorDepth = 8;
                    break;
                case Colors.Bit16:
                    axMsRdpClient2.ColorDepth = 16;
                    break;
                case Colors.Bits24:
                    axMsRdpClient2.ColorDepth = 24;
                    break;
                case Colors.Bits32:
                    if (Settings.SupportsRDP6)
                        axMsRdpClient2.ColorDepth = 32;
                    else
                        axMsRdpClient2.ColorDepth = 24;
                    break;
            }
            axMsRdpClient2.ConnectingText = "Connecting. Please wait...";
            axMsRdpClient2.DisconnectedText = "Disconnecting...";
            axMsRdpClient2.AdvancedSettings3.RedirectDrives = favorite.RedirectDrives;
            axMsRdpClient2.AdvancedSettings3.RedirectPorts = favorite.RedirectPorts;
            axMsRdpClient2.AdvancedSettings3.RedirectPrinters = favorite.RedirectPrinters;
            axMsRdpClient2.AdvancedSettings3.RedirectSmartCards = favorite.RedirectSmartCards;
            axMsRdpClient2.AdvancedSettings3.PerformanceFlags = favorite.PerformanceFlags;
            if (Settings.SupportsRDP6)
            {
                MSTSCLib6.IMsRdpClientAdvancedSettings5 advancedSettings5 = (axMsRdpClient2.AdvancedSettings3 as MSTSCLib6.IMsRdpClientAdvancedSettings5);
                if (advancedSettings5 != null)
                {
                    advancedSettings5.RedirectClipboard = favorite.RedirectClipboard;
                    advancedSettings5.RedirectDevices = favorite.RedirectDevices;
                    advancedSettings5.ConnectionBarShowMinimizeButton = false;
                    advancedSettings5.ConnectionBarShowPinButton = false;
                    advancedSettings5.ConnectionBarShowRestoreButton = false;
                }
            }
            axMsRdpClient2.SecuredSettings2.AudioRedirectionMode = (int)favorite.Sounds;
            axMsRdpClient2.Domain = favorite.DomainName;
            axMsRdpClient2.Server = favorite.ServerName;
            axMsRdpClient2.AdvancedSettings3.RDPPort = favorite.Port;
            axMsRdpClient2.UserName = favorite.UserName;
            axMsRdpClient2.AdvancedSettings3.ContainerHandledFullScreen = -1;
            axMsRdpClient2.AdvancedSettings3.DisplayConnectionBar = false;
            axMsRdpClient2.AdvancedSettings3.ConnectToServerConsole = favorite.ConnectToConsole;
            axMsRdpClient2.OnRequestGoFullScreen += new EventHandler(axMsTscAx_OnRequestGoFullScreen);
            axMsRdpClient2.OnRequestLeaveFullScreen += new EventHandler(axMsTscAx_OnRequestLeaveFullScreen);
            axMsRdpClient2.OnDisconnected += new IMsTscAxEvents_OnDisconnectedEventHandler(axMsTscAx_OnDisconnected);
            axMsRdpClient2.OnWarning += new IMsTscAxEvents_OnWarningEventHandler(axMsRdpClient2_OnWarning);
            axMsRdpClient2.OnFatalError += new IMsTscAxEvents_OnFatalErrorEventHandler(axMsRdpClient2_OnFatalError);

            if (!String.IsNullOrEmpty(favorite.Password))
            {
                MSTSC.IMsTscNonScriptable nonScriptable = (MSTSC.IMsTscNonScriptable)axMsRdpClient2.GetOcx();
                nonScriptable.ClearTextPassword = favorite.Password;
            }

            axMsRdpClient2.FullScreen = true;
            axMsRdpClient2.Connect();
            terminalTabPage.TerminalControl = axMsRdpClient2;
            if (favorite.DesktopSize == DesktopSize.FullScreen)
                FullScreen = true;
        }

        void axMsRdpClient2_OnFatalError(object sender, IMsTscAxEvents_OnFatalErrorEvent e)
        {
            //throw new Exception("The method or operation is not implemented.");
        }

        void axMsRdpClient2_OnWarning(object sender, IMsTscAxEvents_OnWarningEvent e)
        {
            //throw new Exception("The method or operation is not implemented.");
        }

        void terminalTabPage_DragEnter(object sender, DragEventArgs e)
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

        private void SHCopyFiles(string[] sourceFiles, string destinationFolder)
        {
            SHFileOperationWrapper fo = new SHFileOperationWrapper();
            List<string> destinationFiles = new List<string>();

            foreach (string sourceFile in sourceFiles)
            {
                destinationFiles.Add(Path.Combine(destinationFolder, Path.GetFileName(sourceFile)));
            }

            fo.Operation = SHFileOperationWrapper.FileOperations.FO_COPY;
            fo.OwnerWindow = this.Handle;
            fo.SourceFiles = sourceFiles;
            fo.DestFiles = destinationFiles.ToArray();

            fo.DoOperation();
        }

        private string GetDesktopShare()
        {
            string desktopShare = ((TerminalTabControlItem)(tcTerminals.SelectedItem)).Favorite.DesktopShare;
            if (String.IsNullOrEmpty(desktopShare))
            {
                desktopShare = Settings.DefaultDesktopShare.Replace("%SERVER%", CurrentTerminal.Server).Replace(
                    "%USER%", CurrentTerminal.UserName);
            }
            return desktopShare;
        }

        void axMsRdpClient2_DragDrop(object sender, DragEventArgs e)
        {
            string[] files = (string[])e.Data.GetData(DataFormats.FileDrop);
            string desktopShare = GetDesktopShare();
            if (String.IsNullOrEmpty(desktopShare))
            {
                MessageBox.Show(this, "A Desktop Share was not defined for this connection.\n" +
                    "Please define a share in the connection properties window (under the Local Resources tab)."
                    , "Terminals", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
            else
                SHCopyFiles(files, desktopShare);
        }

        void axMsRdpClient2_DragEnter(object sender, DragEventArgs e)
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

        void terminalTabPage_DoubleClick(object sender, EventArgs e)
        {
            if (tcTerminals.SelectedItem != null)
            {
                tsbDisconnect.PerformClick();
            }
        }

        protected override void WndProc(ref Message msg)
        {
            try
            {
                if (msg.Msg == 0x21)
                {
                    TerminalTabControlItem selectedTab = (TerminalTabControlItem)tcTerminals.SelectedItem;
                    if (selectedTab != null)
                    {
                        Rectangle r = selectedTab.RectangleToScreen(selectedTab.ClientRectangle);
                        if (r.Contains(Form.MousePosition))
                        {
                            SetGrabInput(true);
                        }
                        else
                        {
                            TabControlItem item = tcTerminals.GetTabItemByPoint(tcTerminals.PointToClient(Form.MousePosition));
                            if (item == null)
                                SetGrabInput(false);
                            else if (item == selectedTab)
                                SetGrabInput(true); //Grab input if clicking on currently selected tab
                        }
                    }
                    else
                        SetGrabInput(false);
                }
                else if (msg.Msg == WM_LEAVING_FULLSCREEN)
                {
                    if (CurrentTerminal != null)
                    {
                        if (CurrentTerminal.ContainsFocus)
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
            catch (Exception e)
            {
                MessageBox.Show(this, e.Message);
            }
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

        void axMsTscAx_OnRequestLeaveFullScreen(object sender, EventArgs e)
        {
            tsbGrabInput.Checked = false;
            UpdateControls();
            NativeApi.PostMessage(new HandleRef(this, this.Handle), WM_LEAVING_FULLSCREEN, IntPtr.Zero, IntPtr.Zero);
        }

        void axMsTscAx_OnRequestGoFullScreen(object sender, EventArgs e)
        {
            tsbGrabInput.Checked = true;
            UpdateControls();
        }

        void axMsTscAx_OnDisconnected(object sender, IMsTscAxEvents_OnDisconnectedEvent e)
        {
            AxMsRdpClient2 client = (AxMsRdpClient2)sender;

            string error = Functions.GetErrorMessage(e.discReason);
            if (error != null)
            {
                MessageBox.Show(this, String.Format("error connecting to {0} ({1})", client.Server, error), "Terminals",
                    MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
            TabControlItem selectedTabPage = (TabControlItem)(client.Parent);
            bool wasSelected = selectedTabPage.Selected;
            tcTerminals.RemoveTab(selectedTabPage);
            tcTerminals_TabControlItemClosed(null, EventArgs.Empty);
            if (wasSelected)
                NativeApi.PostMessage(new HandleRef(this, this.Handle), WM_LEAVING_FULLSCREEN, IntPtr.Zero, IntPtr.Zero);
            UpdateControls();
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
                    LoadFavorites(txtSearchFavorites.Text);
                    LoadTags(txtSearchTags.Text);
                    tscConnectTo.SelectedIndex = tscConnectTo.Items.IndexOf(frmNewTerminal.Favorite.Name);
                    CreateTerminalTab(frmNewTerminal.Favorite);
                }
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
                Connect(connectionName);
            }
        }

        internal void Connect(string connectionName)
        {
            FavoriteConfigurationElementCollection favorites = Settings.GetFavorites();
            FavoriteConfigurationElement favorite = favorites[connectionName];
            if (favorite == null)
                CreateNewTerminal(connectionName);
            else
                CreateTerminalTab(favorite);
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
            tcTerminals.CloseTab(tcTerminals.SelectedItem);
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

        private void ToggleGrabInput()
        {
            if (CurrentTerminal != null)
            {
                CurrentTerminal.FullScreen = !CurrentTerminal.FullScreen;
            }
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            ToolStripManager.LoadSettings(this);
            tscConnectTo.Focus();
            OpenSavedConnections();
        }

        private void MainForm_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyValue == 3)
            {
                ToggleGrabInput();
            }
        }

        private void SetFullScreen(bool fullScreen)
        {
            tcTerminals.ShowTabs = !fullScreen;
            tcTerminals.ShowBorder = !fullScreen;
            if (fullScreen)
            {
                toolbarStd.Visible = false;
                menuStrip.Visible = false;
                this.lastLocation = this.Location;
                this.lastSize = this.RestoreBounds.Size;
                if (this.WindowState == FormWindowState.Minimized)
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
                if (lastState != FormWindowState.Minimized)
                {
                    if (lastState == FormWindowState.Normal)
                        this.Location = this.lastLocation;
                    this.Size = this.lastSize;
                }
                menuStrip.Visible = true;
                toolbarStd.Visible = true;
            }
            this.fullScreen = fullScreen;
            this.PerformLayout();
        }

        private void tcTerminals_TabControlItemClosing(TabControlItemClosingEventArgs e)
        {
            if (MessageBox.Show(this, "Are you sure that you want to disconnect from the active terminal?",
               "Terminals", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                CurrentTerminal.Disconnect();
                e.Cancel = false;
            }
            else
            {
                e.Cancel = true;
            }
        }

        private void ParseCommandline()
        {
            string[] cmdLineArgs = Environment.GetCommandLineArgs();
            ParseCommandline(cmdLineArgs);
        }

        private void ParseCommandline(string[] cmdLineArgs)
        {
            if (cmdLineArgs.Length > 1)
            {
                for (int i = 1; i < cmdLineArgs.Length; i++)
                {
                    string arg = cmdLineArgs[i];
                    if (arg[0] != '/')
                        QuickConnect(arg, 0);
                    else if (arg.StartsWith("/url:"))
                    {
                        string server; int port;
                        ProtocolHandler.Parse(arg, out server, out port);
                        QuickConnect(server, port);
                    }
                }
            }
        }

        private void QuickConnect(string server, int port)
        {
            FavoriteConfigurationElementCollection favorites = Settings.GetFavorites();
            FavoriteConfigurationElement favorite = favorites[server];
            if (favorite != null)
                CreateTerminalTab(favorite);
            else
            {
                //create a temporaty favorite and connect to it
                favorite = new FavoriteConfigurationElement();
                favorite.ServerName = server;
                favorite.Name = server;
                if (port != 0)
                    favorite.Port = port;
                CreateTerminalTab(favorite);
            }
        }

        private void MainForm_Shown(object sender, EventArgs e)
        {
            ParseCommandline();
        }

        private void tcTerminals_TabControlItemSelectionChanged(TabControlItemChangedEventArgs e)
        {
            UpdateControls();
            if (tcTerminals.Items.Count > 0)
            {
                tsbDisconnect.Enabled = e.Item != null;
                disconnectToolStripMenuItem.Enabled = e.Item != null;
                SetGrabInput(true);
            }
        }

        private void tscConnectTo_TextChanged(object sender, EventArgs e)
        {
            UpdateControls();
        }

        private void tcTerminals_MouseHover(object sender, EventArgs e)
        {
            if (!tcTerminals.ShowTabs)
            {
                timerHover.Enabled = true;
                //tcTerminals.ShowTabs = true;
            }
        }

        private void tcTerminals_MouseLeave(object sender, EventArgs e)
        {
            timerHover.Enabled = false;
            if (FullScreen && tcTerminals.ShowTabs && !tcTerminals.MenuOpen)
            {
                tcTerminals.ShowTabs = false;
            }
            if (currentToolTipItem != null)
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
                if (FullScreen != value)
                    SetFullScreen(value);
            }
        }

        private void tcTerminals_TabControlItemClosed(object sender, EventArgs e)
        {
            if (tcTerminals.Items.Count == 0)
                FullScreen = false;
        }

        private void tcTerminals_DoubleClick(object sender, EventArgs e)
        {
            FullScreen = false;
        }

        private void tsbFullScreen_Click(object sender, EventArgs e)
        {
            if (CurrentTerminal != null)
            {
                FullScreen = true;
            }
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
                ToolStripMenuItem item = new ToolStripMenuItem("Restore", null, tcTerminals_DoubleClick);
                tcTerminals.Menu.Items.Add(item);
                item = new ToolStripMenuItem("Minimize", null, Minimize);
                tcTerminals.Menu.Items.Add(item);
            }
        }

        private void Minimize(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Minimized;
        }

        private void MainForm_Activated(object sender, EventArgs e)
        {
            if (FullScreen)
                tcTerminals.ShowTabs = false;
        }

        private void manageConnectionsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            using (OrganizeFavoritesForm conMgr = new OrganizeFavoritesForm())
            {
                conMgr.ShowDialog();
                LoadFavorites();
            }
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

        private void SaveActiveConnections()
        {
            List<String> activeConnections = new List<string>();
            foreach (TabControlItem item in tcTerminals.Items)
            {
                activeConnections.Add(item.Title);
            }
            Settings.CreateSavedConnectionsList(activeConnections.ToArray());
        }

        private void MainForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (tcTerminals.Items.Count > 0)
            {
                if (Settings.ShowConfirmDialog)
                {
                    SaveActiveConnectionsForm frmSaveActiveConnections = new SaveActiveConnectionsForm();
                    if (frmSaveActiveConnections.ShowDialog() == DialogResult.OK)
                    {
                        Settings.ShowConfirmDialog = !frmSaveActiveConnections.chkDontShowDialog.Checked;
                        if (frmSaveActiveConnections.chkOpenOnNextTime.Checked)
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
                else if (Settings.SaveConnectionsOnClose)
                {
                    SaveActiveConnections();
                }                
            }
            ToolStripManager.SaveSettings(this);
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
            if (frmOrganizeFavoritesToolbar.ShowDialog() == DialogResult.OK)
            {
                LoadFavoritesToolbar();
            }
        }

        private void optionsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OptionsForm frmOptions = new OptionsForm(CurrentTerminal);
            if (frmOptions.ShowDialog() == DialogResult.OK)
            {
                tcTerminals.ShowToolTipOnTitle = Settings.ShowInformationToolTips;
                if (tcTerminals.SelectedItem != null)
                {
                    tcTerminals.SelectedItem.ToolTipText = GetToolTipText(((TerminalTabControlItem)tcTerminals.SelectedItem).Favorite);
                }
            }
        }

        private void aboutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            AboutForm frmAbout = new AboutForm();
            frmAbout.ShowDialog();
        }

        private TabControlItem currentToolTipItem = null;
        private ToolTip currentToolTip = new ToolTip();

        private void tcTerminals_TabControlMouseOnTitle(TabControlMouseOnTitleEventArgs e)
        {
            if (Settings.ShowInformationToolTips)
            {
                //ToolTip
                if ((currentToolTipItem != null) && (currentToolTipItem != e.Item))
                {
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

        private void tcTerminals_TabControlMouseLeftTitle(TabControlMouseOnTitleEventArgs e)
        {
            if (currentToolTipItem != null)
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

        private void tsbTags_Click(object sender, EventArgs e)
        {
            if (tsbTags.Checked)
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
            lvFavorites.Items.Clear();
            FavoriteConfigurationElementCollection favorites = Settings.GetFavorites();
            foreach (FavoriteConfigurationElement favorite in favorites)
            {
                if ((String.IsNullOrEmpty(filter) || (favorite.Name.ToUpper().StartsWith(filter.ToUpper()))))
                {
                    ListViewItem item = new ListViewItem();
                    item.ImageIndex = 0;
                    item.StateImageIndex = 0;
                    item.Tag = favorite;
                    item.Text = favorite.Name;
                    lvFavorites.Items.Add(item);
                }
            }
        }

        private void LoadTags(string filter)
        {
            lvTags.Items.Clear();
            ListViewItem unTaggedListViewItem = new ListViewItem();
            unTaggedListViewItem.ImageIndex = 0;
            unTaggedListViewItem.StateImageIndex = 0;
            List<FavoriteConfigurationElement> unTaggedFavorites = new List<FavoriteConfigurationElement>();
            foreach (string tag in Settings.Tags)
            {
                if ((String.IsNullOrEmpty(filter) || (tag.ToUpper().StartsWith(filter.ToUpper()))))
                {
                    ListViewItem item = new ListViewItem();
                    item.ImageIndex = 0;
                    item.StateImageIndex = 0;
                    FavoriteConfigurationElementCollection favorites = Settings.GetFavorites();
                    List<FavoriteConfigurationElement> tagFavorites = new List<FavoriteConfigurationElement>();
                    foreach (FavoriteConfigurationElement favorite in favorites)
                    {
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
                    lvTags.Items.Add(item);
                }
            }
            unTaggedListViewItem.Tag = unTaggedFavorites;
            unTaggedListViewItem.Text = "UnTagged (" + unTaggedFavorites.Count.ToString() + ")";
            lvTags.Items.Add(unTaggedListViewItem);
        }

        private void ShowTagsFavorites(TabControlItem activeTab)
        {
            pnlTagsFavorites.Width = 300;
            tcTagsFavorites.SelectedItem = activeTab;
            pnlHideTagsFavorites.Show();
            pnlShowTagsFavorites.Hide();
            txtSearchTags.Focus();
        }

        private void ShowTags()
        {
            ShowTagsFavorites(tciTags);
            tsbFavorites.Checked = false;
        }

        private void HideTagsFavorites()
        {
            pnlTagsFavorites.Width = 7;
            pnlHideTagsFavorites.Hide();
            pnlShowTagsFavorites.Show();
            tsbTags.Checked = false;
            tsbFavorites.Checked = false;
        }

        private void ShowFavorites()
        {
            ShowTagsFavorites(tciFavorites);
            tsbTags.Checked = false;
        }

        private void lvTags_SelectedIndexChanged(object sender, EventArgs e)
        {
            connectToolStripMenuItem.Enabled = lvTags.SelectedItems.Count > 0;
            lvTagConnections.Items.Clear();
            if (lvTags.SelectedItems.Count > 0)
            {
                List<FavoriteConfigurationElement> tagFavorites = (List<FavoriteConfigurationElement>)lvTags.SelectedItems[0].Tag;
                foreach (FavoriteConfigurationElement favorite in tagFavorites)
                {
                    ListViewItem item = lvTagConnections.Items.Add(favorite.Name);
                    item.ImageIndex = 0;
                    item.StateImageIndex = 0;
                    item.Tag = favorite;
                }
            }
        }

        private void lvTagConnections_SelectedIndexChanged(object sender, EventArgs e)
        {
            connectToolStripMenuItem.Enabled = lvTagConnections.SelectedItems.Count > 0;
        }

        private void connectToolStripMenuItem_Click(object sender, EventArgs e)
        {
            foreach (ListViewItem item in lvTagConnections.SelectedItems)
            {
                FavoriteConfigurationElement favorite = (FavoriteConfigurationElement)item.Tag;
                HideTagsFavorites();
                Connect(favorite.Name);
            }
        }

        private void connectToAllToolStripMenuItem_Click(object sender, EventArgs e)
        {
            foreach (ListViewItem item in lvTagConnections.Items)
            {
                FavoriteConfigurationElement favorite = (FavoriteConfigurationElement)item.Tag;
                HideTagsFavorites();
                Connect(favorite.Name);
            }
        }

        private void txtSearchTags_TextChanged(object sender, EventArgs e)
        {
            LoadTags(txtSearchTags.Text);
        }

        private void lvTags_KeyDown(object sender, KeyEventArgs e)
        {
            if ((e.KeyCode == Keys.Enter) && (lvTags.SelectedItems.Count == 1))
            {
                connectToAllToolStripMenuItem.PerformClick();
            }
        }

        private void lvTagConnections_KeyDown(object sender, KeyEventArgs e)
        {
            if ((e.KeyCode == Keys.Enter) && (lvTagConnections.SelectedItems.Count == 1))
            {
                connectToolStripMenuItem.PerformClick();
            }
        }

        private void lvTags_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            if (lvTags.SelectedItems.Count == 1)
            {
                connectToAllToolStripMenuItem.PerformClick();
            }
        }

        private void lvTagConnections_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            if (lvTagConnections.SelectedItems.Count == 1)
            {
                connectToolStripMenuItem.PerformClick();
            }
        }

        private void tsbFavorites_Click(object sender, EventArgs e)
        {
            if (tsbFavorites.Checked)
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
            LoadFavorites(txtSearchFavorites.Text);
        }

        private void lvFavorites_KeyDown(object sender, KeyEventArgs e)
        {
            if ((e.KeyCode == Keys.Enter) && (lvFavorites.SelectedItems.Count == 1))
            {
                connectToolStripMenuItem1.PerformClick();
            }
        }

        private void lvFavorites_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            if (lvFavorites.SelectedItems.Count == 1)
            {
                connectToolStripMenuItem1.PerformClick();
            }
        }

        private void connectToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            foreach (ListViewItem item in lvFavorites.SelectedItems)
            {
                FavoriteConfigurationElement favorite = (FavoriteConfigurationElement)item.Tag;
                HideTagsFavorites();
                Connect(favorite.Name);
            }
        }

        private void lvFavorites_SelectedIndexChanged(object sender, EventArgs e)
        {
            connectToolStripMenuItem1.Enabled = lvFavorites.SelectedItems.Count > 0;
        }
    }

    public class TerminalTabControlItem : TabControlItem
    {
        public TerminalTabControlItem(string caption)
            : base(caption, null)
        {
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
            get { return favorite; }
            set { favorite = value; }
        }
    }
}
