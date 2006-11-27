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

namespace Terminals
{
    public partial class MainForm : Form
    {
        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        public static extern int PostMessage(HandleRef hwnd, int msg, IntPtr wparam, IntPtr lparam);
        const int WM_LEAVING_FULLSCREEN = 0x4ff;

        private bool fullScreen;
        private Point lastLocation;
        private Size lastSize;
        private FormWindowState lastState;

        public MainForm()
        {
            InitializeComponent();
            LoadFavorites();
            LoadGroups();
            UpdateControls();
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
            return "Computer: " + favorite.ServerName + Environment.NewLine +
                "User Name: " + favorite.UserName + Environment.NewLine +
                "Domain: " + favorite.DomainName + Environment.NewLine +
                "Port: " + favorite.Port + Environment.NewLine +
                "Colors: " + favorite.Colors.ToString() + Environment.NewLine +
                "Connect To Console: " + favorite.ConnectToConsole.ToString() + Environment.NewLine +
                "Desktop Size: " + favorite.DesktopSize.ToString() + Environment.NewLine +
                "Redirect Drives: " + favorite.RedirectDrives.ToString() + Environment.NewLine +
                "Redirect Ports: " + favorite.RedirectPorts.ToString() + Environment.NewLine +
                "Redirect Printers: " + favorite.RedirectPrinters.ToString() + Environment.NewLine +
                "Sounds: " + favorite.Sounds.ToString() + Environment.NewLine;
        }

        private void CreateTerminalTab(FavoriteConfigurationElement favorite)
        {
            string terminalTabTitle = favorite.Name;
            if (Settings.ShowUserNameInTitle)
            {
                terminalTabTitle += " (" + favorite.UserName + ")";
            }
            TerminalTabControlItem terminalTabPage = new TerminalTabControlItem(terminalTabTitle);
            terminalTabPage.ToolTipText = GetToolTipText(favorite);
            terminalTabPage.DoubleClick += new EventHandler(terminalTabPage_DoubleClick);
            tcTerminals.Items.Add(terminalTabPage);
            tcTerminals.SelectedItem = terminalTabPage;
            tcTerminals_SelectedIndexChanged(this, EventArgs.Empty);
            AxMsRdpClient2 axMsRdpClient2 = new AxMsRdpClient2();
            Controls.Add(axMsRdpClient2);
            axMsRdpClient2.Parent = terminalTabPage;
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
            axMsRdpClient2.DesktopWidth = Math.Min(1600, width);
            axMsRdpClient2.DesktopHeight = Math.Min(1200, height); ;

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
            }
            axMsRdpClient2.AdvancedSettings3.RedirectDrives = favorite.RedirectDrives;
            axMsRdpClient2.AdvancedSettings3.RedirectPorts = favorite.RedirectPorts;
            axMsRdpClient2.AdvancedSettings3.RedirectPrinters = favorite.RedirectPrinters;
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
                            SetGrabInput(false);
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
                base.WndProc(ref msg);
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
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
            PostMessage(new HandleRef(this, this.Handle), WM_LEAVING_FULLSCREEN, IntPtr.Zero, IntPtr.Zero);
        }

        void axMsTscAx_OnRequestGoFullScreen(object sender, EventArgs e)
        {
            tsbGrabInput.Checked = true;
            UpdateControls();
        }

        void axMsTscAx_OnDisconnected(object sender, IMsTscAxEvents_OnDisconnectedEvent e)
        {
            TabControlItem selectedTabPage = (TabControlItem)((AxMsRdpClient2)sender).Parent;
            tcTerminals.RemoveTab(selectedTabPage);
            tcTerminals_TabControlItemClosed(null, EventArgs.Empty);
            PostMessage(new HandleRef(this, this.Handle), WM_LEAVING_FULLSCREEN, IntPtr.Zero, IntPtr.Zero);
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

        private void Connect(string connectionName)
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
            //ToolStripManager.LoadSettings(this);
            tscConnectTo.Focus();
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
            if (MessageBox.Show("Are you sure that you want to disconnect from the active terminal?",
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
            if (cmdLineArgs.Length > 1)
            {
                for (int i = 1; i < cmdLineArgs.Length; i++)
                {
                    Connect(cmdLineArgs[i]);
                }
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

        private void MainForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            //ToolStripManager.SaveSettings(this);
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
            OptionsForm frmOptions = new OptionsForm();
            if (frmOptions.ShowDialog() == DialogResult.OK)
            {
                tcTerminals.ShowToolTipOnTitle = Settings.ShowInformationToolTips;
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
                if ((currentToolTipItem != null) && (currentToolTipItem != e.Item))
                {
                    currentToolTip.Hide(currentToolTipItem);
                }
                currentToolTip.ToolTipTitle = e.Item.Title;
                currentToolTip.ToolTipIcon = ToolTipIcon.Info;
                currentToolTip.UseFading = true;
                currentToolTip.UseAnimation = true;
                currentToolTip.IsBalloon = false;
                currentToolTip.Show(e.Item.ToolTipText, e.Item, PointToClient(e.Item.Location));
                currentToolTipItem = e.Item;
            }
        }

        private void tcTerminals_TabControlMouseLeftTitle(EventArgs e)
        {
            if (currentToolTipItem != null)
            {
                currentToolTip.Hide(currentToolTipItem);
            }
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
    }
}