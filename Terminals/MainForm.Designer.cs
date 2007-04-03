namespace Terminals {
    partial class MainForm {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing) {
            if(disposing && (components != null)) {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent() {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
            this.menuStrip = new System.Windows.Forms.MenuStrip();
            this.fileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.newTerminalToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem1 = new System.Windows.Forms.ToolStripSeparator();
            this.exitToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.viewToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.fullScreenToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.favoritesToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.manageFavoritesToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.organizeFavoritesToolbarToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.favoritesSeparator = new System.Windows.Forms.ToolStripSeparator();
            this.terminalToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.grabInputToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator5 = new System.Windows.Forms.ToolStripSeparator();
            this.disconnectToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.groupsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.organizeGroupsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.saveTerminalsAsGroupToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.addTerminalToGroupToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.groupsSeparator = new System.Windows.Forms.ToolStripSeparator();
            this.toolsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.captureTerminalScreenToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.optionsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.helpToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.aboutToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolbarStd = new System.Windows.Forms.ToolStrip();
            this.tsbNewTerminal = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.toolStripLabel1 = new System.Windows.Forms.ToolStripLabel();
            this.tscConnectTo = new System.Windows.Forms.ToolStripComboBox();
            this.tsbConnect = new System.Windows.Forms.ToolStripButton();
            this.tsbDisconnect = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator3 = new System.Windows.Forms.ToolStripSeparator();
            this.tsbManageConnections = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this.tsbGrabInput = new System.Windows.Forms.ToolStripButton();
            this.tsbFullScreen = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator6 = new System.Windows.Forms.ToolStripSeparator();
            this.tsbTags = new System.Windows.Forms.ToolStripButton();
            this.tsbFavorites = new System.Windows.Forms.ToolStripButton();
            this.CaptureScreenToolStripButton = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator4 = new System.Windows.Forms.ToolStripSeparator();
            this.VMRCAdminSwitchButton = new System.Windows.Forms.ToolStripButton();
            this.VMRCViewOnlyButton = new System.Windows.Forms.ToolStripButton();
            this.toolStripContainer = new System.Windows.Forms.ToolStripContainer();
            this.tcTerminals = new TabControl.TabControl();
            this.pnlTagsFavorites = new System.Windows.Forms.Panel();
            this.tcTagsFavorites = new TabControl.TabControl();
            this.tciTags = new TabControl.TabControlItem();
            this.lvTagConnections = new System.Windows.Forms.ListView();
            this.chConnection = new System.Windows.Forms.ColumnHeader();
            this.cmsTagConnections = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.connectToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.ilTagConnections = new System.Windows.Forms.ImageList(this.components);
            this.lvTags = new System.Windows.Forms.ListView();
            this.chTag = new System.Windows.Forms.ColumnHeader();
            this.cmsTags = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.connectToAllToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.ilTags = new System.Windows.Forms.ImageList(this.components);
            this.txtSearchTags = new System.Windows.Forms.TextBox();
            this.tciFavorites = new TabControl.TabControlItem();
            this.lvFavorites = new System.Windows.Forms.ListView();
            this.columnHeader1 = new System.Windows.Forms.ColumnHeader();
            this.cmsFavorites = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.connectToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.txtSearchFavorites = new System.Windows.Forms.TextBox();
            this.pnlHideTagsFavorites = new System.Windows.Forms.Panel();
            this.pbHideTagsFavorites = new System.Windows.Forms.PictureBox();
            this.pnlShowTagsFavorites = new System.Windows.Forms.Panel();
            this.pbShowTagsFavorites = new System.Windows.Forms.PictureBox();
            this.timerHover = new System.Windows.Forms.Timer(this.components);
            this.MainWindowNotifyIcon = new System.Windows.Forms.NotifyIcon(this.components);
            this.SystemTrayContextMenuStrip = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.toolStripMenuItem2 = new System.Windows.Forms.ToolStripMenuItem();
            this.newConnectionToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.showToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.remoteDesktop1 = new VncSharp.RemoteDesktop();
            this.menuStrip.SuspendLayout();
            this.toolbarStd.SuspendLayout();
            this.toolStripContainer.ContentPanel.SuspendLayout();
            this.toolStripContainer.TopToolStripPanel.SuspendLayout();
            this.toolStripContainer.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.tcTerminals)).BeginInit();
            this.pnlTagsFavorites.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.tcTagsFavorites)).BeginInit();
            this.tcTagsFavorites.SuspendLayout();
            this.tciTags.SuspendLayout();
            this.cmsTagConnections.SuspendLayout();
            this.cmsTags.SuspendLayout();
            this.tciFavorites.SuspendLayout();
            this.cmsFavorites.SuspendLayout();
            this.pnlHideTagsFavorites.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pbHideTagsFavorites)).BeginInit();
            this.pnlShowTagsFavorites.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pbShowTagsFavorites)).BeginInit();
            this.SystemTrayContextMenuStrip.SuspendLayout();
            this.SuspendLayout();
            // 
            // menuStrip
            // 
            this.menuStrip.Dock = System.Windows.Forms.DockStyle.None;
            this.menuStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.fileToolStripMenuItem,
            this.viewToolStripMenuItem,
            this.favoritesToolStripMenuItem,
            this.terminalToolStripMenuItem,
            this.groupsToolStripMenuItem,
            this.toolsToolStripMenuItem,
            this.helpToolStripMenuItem});
            this.menuStrip.Location = new System.Drawing.Point(3, 0);
            this.menuStrip.Name = "menuStrip";
            this.menuStrip.Size = new System.Drawing.Size(344, 24);
            this.menuStrip.Stretch = false;
            this.menuStrip.TabIndex = 1;
            this.menuStrip.Text = "menuStrip1";
            // 
            // fileToolStripMenuItem
            // 
            this.fileToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.newTerminalToolStripMenuItem,
            this.toolStripMenuItem1,
            this.exitToolStripMenuItem});
            this.fileToolStripMenuItem.Name = "fileToolStripMenuItem";
            this.fileToolStripMenuItem.Size = new System.Drawing.Size(35, 20);
            this.fileToolStripMenuItem.Text = "&File";
            // 
            // newTerminalToolStripMenuItem
            // 
            this.newTerminalToolStripMenuItem.Name = "newTerminalToolStripMenuItem";
            this.newTerminalToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.N)));
            this.newTerminalToolStripMenuItem.Size = new System.Drawing.Size(214, 22);
            this.newTerminalToolStripMenuItem.Text = "&New Connection...";
            this.newTerminalToolStripMenuItem.Click += new System.EventHandler(this.newTerminalToolStripMenuItem_Click_1);
            // 
            // toolStripMenuItem1
            // 
            this.toolStripMenuItem1.Name = "toolStripMenuItem1";
            this.toolStripMenuItem1.Size = new System.Drawing.Size(211, 6);
            // 
            // exitToolStripMenuItem
            // 
            this.exitToolStripMenuItem.Name = "exitToolStripMenuItem";
            this.exitToolStripMenuItem.Size = new System.Drawing.Size(214, 22);
            this.exitToolStripMenuItem.Text = "E&xit";
            this.exitToolStripMenuItem.Click += new System.EventHandler(this.exitToolStripMenuItem_Click);
            // 
            // viewToolStripMenuItem
            // 
            this.viewToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.fullScreenToolStripMenuItem});
            this.viewToolStripMenuItem.Name = "viewToolStripMenuItem";
            this.viewToolStripMenuItem.Size = new System.Drawing.Size(41, 20);
            this.viewToolStripMenuItem.Text = "&View";
            // 
            // fullScreenToolStripMenuItem
            // 
            this.fullScreenToolStripMenuItem.Name = "fullScreenToolStripMenuItem";
            this.fullScreenToolStripMenuItem.ShortcutKeys = System.Windows.Forms.Keys.F11;
            this.fullScreenToolStripMenuItem.Size = new System.Drawing.Size(162, 22);
            this.fullScreenToolStripMenuItem.Text = "&Full Screen";
            this.fullScreenToolStripMenuItem.Click += new System.EventHandler(this.tsbFullScreen_Click);
            // 
            // favoritesToolStripMenuItem
            // 
            this.favoritesToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.manageFavoritesToolStripMenuItem,
            this.organizeFavoritesToolbarToolStripMenuItem,
            this.favoritesSeparator});
            this.favoritesToolStripMenuItem.Name = "favoritesToolStripMenuItem";
            this.favoritesToolStripMenuItem.Size = new System.Drawing.Size(64, 20);
            this.favoritesToolStripMenuItem.Text = "F&avorites";
            // 
            // manageFavoritesToolStripMenuItem
            // 
            this.manageFavoritesToolStripMenuItem.Name = "manageFavoritesToolStripMenuItem";
            this.manageFavoritesToolStripMenuItem.Size = new System.Drawing.Size(227, 22);
            this.manageFavoritesToolStripMenuItem.Text = "&Organize Favorites...";
            this.manageFavoritesToolStripMenuItem.Click += new System.EventHandler(this.manageConnectionsToolStripMenuItem_Click);
            // 
            // organizeFavoritesToolbarToolStripMenuItem
            // 
            this.organizeFavoritesToolbarToolStripMenuItem.Name = "organizeFavoritesToolbarToolStripMenuItem";
            this.organizeFavoritesToolbarToolStripMenuItem.Size = new System.Drawing.Size(227, 22);
            this.organizeFavoritesToolbarToolStripMenuItem.Text = "Organize &Favorites Toolbar...";
            this.organizeFavoritesToolbarToolStripMenuItem.Click += new System.EventHandler(this.organizeFavoritesToolbarToolStripMenuItem_Click);
            // 
            // favoritesSeparator
            // 
            this.favoritesSeparator.Name = "favoritesSeparator";
            this.favoritesSeparator.Size = new System.Drawing.Size(224, 6);
            // 
            // terminalToolStripMenuItem
            // 
            this.terminalToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.grabInputToolStripMenuItem,
            this.toolStripSeparator5,
            this.disconnectToolStripMenuItem});
            this.terminalToolStripMenuItem.Name = "terminalToolStripMenuItem";
            this.terminalToolStripMenuItem.Size = new System.Drawing.Size(59, 20);
            this.terminalToolStripMenuItem.Text = "Te&rminal";
            // 
            // grabInputToolStripMenuItem
            // 
            this.grabInputToolStripMenuItem.Enabled = false;
            this.grabInputToolStripMenuItem.Image = global::Terminals.Properties.Resources.arrow_out;
            this.grabInputToolStripMenuItem.Name = "grabInputToolStripMenuItem";
            this.grabInputToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.G)));
            this.grabInputToolStripMenuItem.Size = new System.Drawing.Size(176, 22);
            this.grabInputToolStripMenuItem.Text = "&Grab Input";
            this.grabInputToolStripMenuItem.Click += new System.EventHandler(this.tsbGrabInput_Click);
            // 
            // toolStripSeparator5
            // 
            this.toolStripSeparator5.Name = "toolStripSeparator5";
            this.toolStripSeparator5.Size = new System.Drawing.Size(173, 6);
            // 
            // disconnectToolStripMenuItem
            // 
            this.disconnectToolStripMenuItem.Enabled = false;
            this.disconnectToolStripMenuItem.Name = "disconnectToolStripMenuItem";
            this.disconnectToolStripMenuItem.Size = new System.Drawing.Size(176, 22);
            this.disconnectToolStripMenuItem.Text = "&Disconnect";
            this.disconnectToolStripMenuItem.Click += new System.EventHandler(this.tsbDisconnect_Click);
            // 
            // groupsToolStripMenuItem
            // 
            this.groupsToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.organizeGroupsToolStripMenuItem,
            this.saveTerminalsAsGroupToolStripMenuItem,
            this.addTerminalToGroupToolStripMenuItem,
            this.groupsSeparator});
            this.groupsToolStripMenuItem.Name = "groupsToolStripMenuItem";
            this.groupsToolStripMenuItem.Size = new System.Drawing.Size(53, 20);
            this.groupsToolStripMenuItem.Text = "&Groups";
            // 
            // organizeGroupsToolStripMenuItem
            // 
            this.organizeGroupsToolStripMenuItem.Name = "organizeGroupsToolStripMenuItem";
            this.organizeGroupsToolStripMenuItem.Size = new System.Drawing.Size(272, 22);
            this.organizeGroupsToolStripMenuItem.Text = "&Organize Groups...";
            this.organizeGroupsToolStripMenuItem.Click += new System.EventHandler(this.organizeGroupsToolStripMenuItem_Click);
            // 
            // saveTerminalsAsGroupToolStripMenuItem
            // 
            this.saveTerminalsAsGroupToolStripMenuItem.Name = "saveTerminalsAsGroupToolStripMenuItem";
            this.saveTerminalsAsGroupToolStripMenuItem.Size = new System.Drawing.Size(272, 22);
            this.saveTerminalsAsGroupToolStripMenuItem.Text = "&Create Group From Active Connections";
            this.saveTerminalsAsGroupToolStripMenuItem.Click += new System.EventHandler(this.saveTerminalsAsGroupToolStripMenuItem_Click);
            // 
            // addTerminalToGroupToolStripMenuItem
            // 
            this.addTerminalToGroupToolStripMenuItem.Name = "addTerminalToGroupToolStripMenuItem";
            this.addTerminalToGroupToolStripMenuItem.Size = new System.Drawing.Size(272, 22);
            this.addTerminalToGroupToolStripMenuItem.Text = "&Add Current Connection To ";
            // 
            // groupsSeparator
            // 
            this.groupsSeparator.Name = "groupsSeparator";
            this.groupsSeparator.Size = new System.Drawing.Size(269, 6);
            // 
            // toolsToolStripMenuItem
            // 
            this.toolsToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.captureTerminalScreenToolStripMenuItem,
            this.optionsToolStripMenuItem});
            this.toolsToolStripMenuItem.Name = "toolsToolStripMenuItem";
            this.toolsToolStripMenuItem.Size = new System.Drawing.Size(44, 20);
            this.toolsToolStripMenuItem.Text = "&Tools";
            this.toolsToolStripMenuItem.Click += new System.EventHandler(this.toolsToolStripMenuItem_Click);
            // 
            // captureTerminalScreenToolStripMenuItem
            // 
            this.captureTerminalScreenToolStripMenuItem.Name = "captureTerminalScreenToolStripMenuItem";
            this.captureTerminalScreenToolStripMenuItem.Size = new System.Drawing.Size(203, 22);
            this.captureTerminalScreenToolStripMenuItem.Text = "&Capture Terminal Screen";
            this.captureTerminalScreenToolStripMenuItem.Click += new System.EventHandler(this.captureTerminalScreenToolStripMenuItem_Click);
            // 
            // optionsToolStripMenuItem
            // 
            this.optionsToolStripMenuItem.Name = "optionsToolStripMenuItem";
            this.optionsToolStripMenuItem.Size = new System.Drawing.Size(203, 22);
            this.optionsToolStripMenuItem.Text = "&Options";
            this.optionsToolStripMenuItem.Click += new System.EventHandler(this.optionsToolStripMenuItem_Click);
            // 
            // helpToolStripMenuItem
            // 
            this.helpToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.aboutToolStripMenuItem});
            this.helpToolStripMenuItem.Name = "helpToolStripMenuItem";
            this.helpToolStripMenuItem.Size = new System.Drawing.Size(40, 20);
            this.helpToolStripMenuItem.Text = "&Help";
            // 
            // aboutToolStripMenuItem
            // 
            this.aboutToolStripMenuItem.Name = "aboutToolStripMenuItem";
            this.aboutToolStripMenuItem.Size = new System.Drawing.Size(114, 22);
            this.aboutToolStripMenuItem.Text = "&About";
            this.aboutToolStripMenuItem.Click += new System.EventHandler(this.aboutToolStripMenuItem_Click);
            // 
            // toolbarStd
            // 
            this.toolbarStd.Dock = System.Windows.Forms.DockStyle.None;
            this.toolbarStd.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tsbNewTerminal,
            this.toolStripSeparator1,
            this.toolStripLabel1,
            this.tscConnectTo,
            this.tsbConnect,
            this.tsbDisconnect,
            this.toolStripSeparator3,
            this.tsbManageConnections,
            this.toolStripSeparator2,
            this.tsbGrabInput,
            this.tsbFullScreen,
            this.toolStripSeparator6,
            this.tsbTags,
            this.tsbFavorites,
            this.CaptureScreenToolStripButton,
            this.toolStripSeparator4,
            this.VMRCAdminSwitchButton,
            this.VMRCViewOnlyButton});
            this.toolbarStd.Location = new System.Drawing.Point(3, 24);
            this.toolbarStd.Name = "toolbarStd";
            this.toolbarStd.Size = new System.Drawing.Size(561, 25);
            this.toolbarStd.TabIndex = 2;
            // 
            // tsbNewTerminal
            // 
            this.tsbNewTerminal.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.tsbNewTerminal.Image = ((System.Drawing.Image)(resources.GetObject("tsbNewTerminal.Image")));
            this.tsbNewTerminal.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsbNewTerminal.Name = "tsbNewTerminal";
            this.tsbNewTerminal.Size = new System.Drawing.Size(23, 22);
            this.tsbNewTerminal.ToolTipText = "New Connection";
            this.tsbNewTerminal.Click += new System.EventHandler(this.newTerminalToolStripMenuItem_Click);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(6, 25);
            // 
            // toolStripLabel1
            // 
            this.toolStripLabel1.Name = "toolStripLabel1";
            this.toolStripLabel1.Size = new System.Drawing.Size(66, 22);
            this.toolStripLabel1.Text = "&Connect To:";
            // 
            // tscConnectTo
            // 
            this.tscConnectTo.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.Suggest;
            this.tscConnectTo.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
            this.tscConnectTo.Name = "tscConnectTo";
            this.tscConnectTo.Size = new System.Drawing.Size(200, 25);
            this.tscConnectTo.Sorted = true;
            this.tscConnectTo.KeyDown += new System.Windows.Forms.KeyEventHandler(this.tscConnectTo_KeyDown);
            this.tscConnectTo.TextChanged += new System.EventHandler(this.tscConnectTo_TextChanged);
            // 
            // tsbConnect
            // 
            this.tsbConnect.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.tsbConnect.Enabled = false;
            this.tsbConnect.Image = global::Terminals.Properties.Resources.application_lightning;
            this.tsbConnect.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsbConnect.Name = "tsbConnect";
            this.tsbConnect.Size = new System.Drawing.Size(23, 22);
            this.tsbConnect.ToolTipText = "Connect to server";
            this.tsbConnect.Click += new System.EventHandler(this.tsbConnect_Click);
            // 
            // tsbDisconnect
            // 
            this.tsbDisconnect.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.tsbDisconnect.Enabled = false;
            this.tsbDisconnect.Image = global::Terminals.Properties.Resources.disconnect;
            this.tsbDisconnect.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsbDisconnect.Name = "tsbDisconnect";
            this.tsbDisconnect.Size = new System.Drawing.Size(23, 22);
            this.tsbDisconnect.ToolTipText = "Disconnect From Server";
            this.tsbDisconnect.Click += new System.EventHandler(this.tsbDisconnect_Click);
            // 
            // toolStripSeparator3
            // 
            this.toolStripSeparator3.Name = "toolStripSeparator3";
            this.toolStripSeparator3.Size = new System.Drawing.Size(6, 25);
            // 
            // tsbManageConnections
            // 
            this.tsbManageConnections.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.tsbManageConnections.Image = global::Terminals.Properties.Resources.application_edit;
            this.tsbManageConnections.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsbManageConnections.Name = "tsbManageConnections";
            this.tsbManageConnections.Size = new System.Drawing.Size(23, 22);
            this.tsbManageConnections.ToolTipText = "Manage Connections";
            this.tsbManageConnections.Click += new System.EventHandler(this.manageConnectionsToolStripMenuItem_Click);
            // 
            // toolStripSeparator2
            // 
            this.toolStripSeparator2.Name = "toolStripSeparator2";
            this.toolStripSeparator2.Size = new System.Drawing.Size(6, 25);
            // 
            // tsbGrabInput
            // 
            this.tsbGrabInput.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.tsbGrabInput.Enabled = false;
            this.tsbGrabInput.Image = global::Terminals.Properties.Resources.keyboard;
            this.tsbGrabInput.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsbGrabInput.Name = "tsbGrabInput";
            this.tsbGrabInput.Size = new System.Drawing.Size(23, 22);
            this.tsbGrabInput.ToolTipText = "Grab Input";
            this.tsbGrabInput.Click += new System.EventHandler(this.tsbGrabInput_Click);
            // 
            // tsbFullScreen
            // 
            this.tsbFullScreen.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.tsbFullScreen.Image = global::Terminals.Properties.Resources.arrow_out;
            this.tsbFullScreen.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsbFullScreen.Name = "tsbFullScreen";
            this.tsbFullScreen.Size = new System.Drawing.Size(23, 22);
            this.tsbFullScreen.ToolTipText = "Full Screen (F11)";
            this.tsbFullScreen.Click += new System.EventHandler(this.tsbFullScreen_Click);
            // 
            // toolStripSeparator6
            // 
            this.toolStripSeparator6.Name = "toolStripSeparator6";
            this.toolStripSeparator6.Size = new System.Drawing.Size(6, 25);
            // 
            // tsbTags
            // 
            this.tsbTags.CheckOnClick = true;
            this.tsbTags.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.tsbTags.Image = global::Terminals.Properties.Resources.tag;
            this.tsbTags.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsbTags.Name = "tsbTags";
            this.tsbTags.Size = new System.Drawing.Size(23, 22);
            this.tsbTags.ToolTipText = "Tags";
            this.tsbTags.Click += new System.EventHandler(this.tsbTags_Click);
            // 
            // tsbFavorites
            // 
            this.tsbFavorites.CheckOnClick = true;
            this.tsbFavorites.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.tsbFavorites.Image = global::Terminals.Properties.Resources.star;
            this.tsbFavorites.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsbFavorites.Name = "tsbFavorites";
            this.tsbFavorites.Size = new System.Drawing.Size(23, 22);
            this.tsbFavorites.ToolTipText = "Favorites";
            this.tsbFavorites.Click += new System.EventHandler(this.tsbFavorites_Click);
            // 
            // CaptureScreenToolStripButton
            // 
            this.CaptureScreenToolStripButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.CaptureScreenToolStripButton.Image = global::Terminals.Properties.Resources.ico_scapture;
            this.CaptureScreenToolStripButton.ImageTransparentColor = System.Drawing.Color.FromArgb(((int)(((byte)(254)))), ((int)(((byte)(254)))), ((int)(((byte)(254)))));
            this.CaptureScreenToolStripButton.Name = "CaptureScreenToolStripButton";
            this.CaptureScreenToolStripButton.Size = new System.Drawing.Size(23, 22);
            this.CaptureScreenToolStripButton.Text = "Capture Terminal Screen";
            this.CaptureScreenToolStripButton.ToolTipText = "Capture Terminal Screen";
            this.CaptureScreenToolStripButton.Click += new System.EventHandler(this.CaptureScreenToolStripButton_Click);
            // 
            // toolStripSeparator4
            // 
            this.toolStripSeparator4.Name = "toolStripSeparator4";
            this.toolStripSeparator4.Size = new System.Drawing.Size(6, 25);
            // 
            // VMRCAdminSwitchButton
            // 
            this.VMRCAdminSwitchButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.VMRCAdminSwitchButton.Image = global::Terminals.Properties.Resources.server_administrator_icon;
            this.VMRCAdminSwitchButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.VMRCAdminSwitchButton.Name = "VMRCAdminSwitchButton";
            this.VMRCAdminSwitchButton.Size = new System.Drawing.Size(23, 22);
            this.VMRCAdminSwitchButton.Text = "VMRC: Switch to Administrator View.";
            this.VMRCAdminSwitchButton.Click += new System.EventHandler(this.VMRCAdminSwitchButton_Click);
            // 
            // VMRCViewOnlyButton
            // 
            this.VMRCViewOnlyButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.VMRCViewOnlyButton.Image = global::Terminals.Properties.Resources.polarized_glasses;
            this.VMRCViewOnlyButton.ImageTransparentColor = System.Drawing.Color.White;
            this.VMRCViewOnlyButton.Name = "VMRCViewOnlyButton";
            this.VMRCViewOnlyButton.Size = new System.Drawing.Size(23, 22);
            this.VMRCViewOnlyButton.Text = "VMRC: View Only Mode";
            this.VMRCViewOnlyButton.Click += new System.EventHandler(this.VMRCViewOnlyButton_Click);
            // 
            // toolStripContainer
            // 
            // 
            // toolStripContainer.ContentPanel
            // 
            this.toolStripContainer.ContentPanel.AutoScroll = true;
            this.toolStripContainer.ContentPanel.Controls.Add(this.tcTerminals);
            this.toolStripContainer.ContentPanel.Controls.Add(this.pnlTagsFavorites);
            this.toolStripContainer.ContentPanel.Size = new System.Drawing.Size(792, 524);
            this.toolStripContainer.Dock = System.Windows.Forms.DockStyle.Fill;
            this.toolStripContainer.Location = new System.Drawing.Point(0, 0);
            this.toolStripContainer.Name = "toolStripContainer";
            this.toolStripContainer.Size = new System.Drawing.Size(792, 573);
            this.toolStripContainer.TabIndex = 0;
            this.toolStripContainer.Text = "toolStripContainer1";
            // 
            // toolStripContainer.TopToolStripPanel
            // 
            this.toolStripContainer.TopToolStripPanel.Controls.Add(this.menuStrip);
            this.toolStripContainer.TopToolStripPanel.Controls.Add(this.toolbarStd);
            // 
            // tcTerminals
            // 
            this.tcTerminals.AllowDrop = true;
            this.tcTerminals.AlwaysShowClose = false;
            this.tcTerminals.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tcTerminals.Font = new System.Drawing.Font("Tahoma", 8.25F);
            this.tcTerminals.Location = new System.Drawing.Point(300, 0);
            this.tcTerminals.Name = "tcTerminals";
            this.tcTerminals.ShowToolTipOnTitle = false;
            this.tcTerminals.Size = new System.Drawing.Size(492, 524);
            this.tcTerminals.TabIndex = 3;
            this.tcTerminals.DoubleClick += new System.EventHandler(this.tcTerminals_DoubleClick);
            this.tcTerminals.TabControlMouseOnTitle += new TabControl.TabControlMouseOnTitleHandler(this.tcTerminals_TabControlMouseOnTitle);
            this.tcTerminals.TabControlItemSelectionChanged += new TabControl.TabControlItemChangedHandler(this.tcTerminals_TabControlItemSelectionChanged);
            this.tcTerminals.MenuItemsLoaded += new System.EventHandler(this.tcTerminals_MenuItemsLoaded);
            this.tcTerminals.TabControlItemClosing += new TabControl.TabControlItemClosingHandler(this.tcTerminals_TabControlItemClosing);
            this.tcTerminals.MouseLeave += new System.EventHandler(this.tcTerminals_MouseLeave);
            this.tcTerminals.TabControlItemClosed += new System.EventHandler(this.tcTerminals_TabControlItemClosed);
            this.tcTerminals.TabControlMouseLeftTitle += new TabControl.TabControlMouseLeftTitleHandler(this.tcTerminals_TabControlMouseLeftTitle);
            this.tcTerminals.MouseHover += new System.EventHandler(this.tcTerminals_MouseHover);
            this.tcTerminals.TabControlMouseEnteredTitle += new TabControl.TabControlMouseEnteredTitleHandler(this.tcTerminals_TabControlMouseEnteredTitle);
            // 
            // pnlTagsFavorites
            // 
            this.pnlTagsFavorites.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pnlTagsFavorites.Controls.Add(this.tcTagsFavorites);
            this.pnlTagsFavorites.Controls.Add(this.pnlHideTagsFavorites);
            this.pnlTagsFavorites.Controls.Add(this.pnlShowTagsFavorites);
            this.pnlTagsFavorites.Dock = System.Windows.Forms.DockStyle.Left;
            this.pnlTagsFavorites.Location = new System.Drawing.Point(0, 0);
            this.pnlTagsFavorites.Name = "pnlTagsFavorites";
            this.pnlTagsFavorites.Size = new System.Drawing.Size(300, 524);
            this.pnlTagsFavorites.TabIndex = 6;
            // 
            // tcTagsFavorites
            // 
            this.tcTagsFavorites.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tcTagsFavorites.Font = new System.Drawing.Font("Tahoma", 8.25F);
            this.tcTagsFavorites.Items.AddRange(new TabControl.TabControlItem[] {
            this.tciTags,
            this.tciFavorites});
            this.tcTagsFavorites.Location = new System.Drawing.Point(5, 0);
            this.tcTagsFavorites.Name = "tcTagsFavorites";
            this.tcTagsFavorites.SelectedItem = this.tciTags;
            this.tcTagsFavorites.ShowToolTipOnTitle = false;
            this.tcTagsFavorites.Size = new System.Drawing.Size(288, 522);
            this.tcTagsFavorites.TabIndex = 9;
            // 
            // tciTags
            // 
            this.tciTags.Controls.Add(this.lvTagConnections);
            this.tciTags.Controls.Add(this.lvTags);
            this.tciTags.Controls.Add(this.txtSearchTags);
            this.tciTags.IsDrawn = true;
            this.tciTags.Name = "tciTags";
            this.tciTags.Selected = true;
            this.tciTags.TabIndex = 0;
            this.tciTags.Title = "Tags";
            this.tciTags.ToolTipText = "";
            // 
            // lvTagConnections
            // 
            this.lvTagConnections.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.chConnection});
            this.lvTagConnections.ContextMenuStrip = this.cmsTagConnections;
            this.lvTagConnections.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lvTagConnections.HideSelection = false;
            this.lvTagConnections.Location = new System.Drawing.Point(0, 308);
            this.lvTagConnections.Name = "lvTagConnections";
            this.lvTagConnections.Size = new System.Drawing.Size(286, 193);
            this.lvTagConnections.SmallImageList = this.ilTagConnections;
            this.lvTagConnections.TabIndex = 7;
            this.lvTagConnections.UseCompatibleStateImageBehavior = false;
            this.lvTagConnections.View = System.Windows.Forms.View.Details;
            this.lvTagConnections.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.lvTagConnections_MouseDoubleClick);
            this.lvTagConnections.SelectedIndexChanged += new System.EventHandler(this.lvTagConnections_SelectedIndexChanged);
            this.lvTagConnections.KeyDown += new System.Windows.Forms.KeyEventHandler(this.lvTagConnections_KeyDown);
            // 
            // chConnection
            // 
            this.chConnection.Text = "Connection";
            this.chConnection.Width = 263;
            // 
            // cmsTagConnections
            // 
            this.cmsTagConnections.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.connectToolStripMenuItem});
            this.cmsTagConnections.Name = "cmsTagConnections";
            this.cmsTagConnections.Size = new System.Drawing.Size(126, 26);
            // 
            // connectToolStripMenuItem
            // 
            this.connectToolStripMenuItem.Enabled = false;
            this.connectToolStripMenuItem.Image = global::Terminals.Properties.Resources.application_lightning;
            this.connectToolStripMenuItem.Name = "connectToolStripMenuItem";
            this.connectToolStripMenuItem.Size = new System.Drawing.Size(125, 22);
            this.connectToolStripMenuItem.Text = "&Connect";
            this.connectToolStripMenuItem.Click += new System.EventHandler(this.connectToolStripMenuItem_Click);
            // 
            // ilTagConnections
            // 
            this.ilTagConnections.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("ilTagConnections.ImageStream")));
            this.ilTagConnections.TransparentColor = System.Drawing.Color.Transparent;
            this.ilTagConnections.Images.SetKeyName(0, "");
            // 
            // lvTags
            // 
            this.lvTags.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.chTag});
            this.lvTags.ContextMenuStrip = this.cmsTags;
            this.lvTags.Dock = System.Windows.Forms.DockStyle.Top;
            this.lvTags.HideSelection = false;
            this.lvTags.Location = new System.Drawing.Point(0, 21);
            this.lvTags.MultiSelect = false;
            this.lvTags.Name = "lvTags";
            this.lvTags.Size = new System.Drawing.Size(286, 287);
            this.lvTags.SmallImageList = this.ilTags;
            this.lvTags.TabIndex = 6;
            this.lvTags.UseCompatibleStateImageBehavior = false;
            this.lvTags.View = System.Windows.Forms.View.Details;
            this.lvTags.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.lvTags_MouseDoubleClick);
            this.lvTags.SelectedIndexChanged += new System.EventHandler(this.lvTags_SelectedIndexChanged);
            this.lvTags.KeyDown += new System.Windows.Forms.KeyEventHandler(this.lvTags_KeyDown);
            // 
            // chTag
            // 
            this.chTag.Text = "Tag";
            this.chTag.Width = 259;
            // 
            // cmsTags
            // 
            this.cmsTags.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.connectToAllToolStripMenuItem});
            this.cmsTags.Name = "cmsTags";
            this.cmsTags.Size = new System.Drawing.Size(155, 26);
            // 
            // connectToAllToolStripMenuItem
            // 
            this.connectToAllToolStripMenuItem.Image = global::Terminals.Properties.Resources.application_lightning;
            this.connectToAllToolStripMenuItem.Name = "connectToAllToolStripMenuItem";
            this.connectToAllToolStripMenuItem.Size = new System.Drawing.Size(154, 22);
            this.connectToAllToolStripMenuItem.Text = "&Connect To All";
            this.connectToAllToolStripMenuItem.Click += new System.EventHandler(this.connectToAllToolStripMenuItem_Click);
            // 
            // ilTags
            // 
            this.ilTags.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("ilTags.ImageStream")));
            this.ilTags.TransparentColor = System.Drawing.Color.Transparent;
            this.ilTags.Images.SetKeyName(0, "tag.png");
            // 
            // txtSearchTags
            // 
            this.txtSearchTags.Dock = System.Windows.Forms.DockStyle.Top;
            this.txtSearchTags.Location = new System.Drawing.Point(0, 0);
            this.txtSearchTags.Name = "txtSearchTags";
            this.txtSearchTags.Size = new System.Drawing.Size(286, 21);
            this.txtSearchTags.TabIndex = 8;
            this.txtSearchTags.TextChanged += new System.EventHandler(this.txtSearchTags_TextChanged);
            // 
            // tciFavorites
            // 
            this.tciFavorites.Controls.Add(this.lvFavorites);
            this.tciFavorites.Controls.Add(this.txtSearchFavorites);
            this.tciFavorites.IsDrawn = true;
            this.tciFavorites.Name = "tciFavorites";
            this.tciFavorites.TabIndex = 1;
            this.tciFavorites.Title = "Favorites";
            this.tciFavorites.ToolTipText = "";
            // 
            // lvFavorites
            // 
            this.lvFavorites.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeader1});
            this.lvFavorites.ContextMenuStrip = this.cmsFavorites;
            this.lvFavorites.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lvFavorites.HideSelection = false;
            this.lvFavorites.Location = new System.Drawing.Point(0, 21);
            this.lvFavorites.Name = "lvFavorites";
            this.lvFavorites.Size = new System.Drawing.Size(200, 79);
            this.lvFavorites.SmallImageList = this.ilTagConnections;
            this.lvFavorites.TabIndex = 10;
            this.lvFavorites.UseCompatibleStateImageBehavior = false;
            this.lvFavorites.View = System.Windows.Forms.View.Details;
            this.lvFavorites.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.lvFavorites_MouseDoubleClick);
            this.lvFavorites.SelectedIndexChanged += new System.EventHandler(this.lvFavorites_SelectedIndexChanged);
            this.lvFavorites.KeyDown += new System.Windows.Forms.KeyEventHandler(this.lvFavorites_KeyDown);
            // 
            // columnHeader1
            // 
            this.columnHeader1.Text = "Connection";
            this.columnHeader1.Width = 263;
            // 
            // cmsFavorites
            // 
            this.cmsFavorites.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.connectToolStripMenuItem1});
            this.cmsFavorites.Name = "cmsFavorites";
            this.cmsFavorites.Size = new System.Drawing.Size(126, 26);
            // 
            // connectToolStripMenuItem1
            // 
            this.connectToolStripMenuItem1.Enabled = false;
            this.connectToolStripMenuItem1.Image = global::Terminals.Properties.Resources.application_lightning;
            this.connectToolStripMenuItem1.Name = "connectToolStripMenuItem1";
            this.connectToolStripMenuItem1.Size = new System.Drawing.Size(125, 22);
            this.connectToolStripMenuItem1.Text = "&Connect";
            this.connectToolStripMenuItem1.Click += new System.EventHandler(this.connectToolStripMenuItem1_Click);
            // 
            // txtSearchFavorites
            // 
            this.txtSearchFavorites.Dock = System.Windows.Forms.DockStyle.Top;
            this.txtSearchFavorites.Location = new System.Drawing.Point(0, 0);
            this.txtSearchFavorites.Name = "txtSearchFavorites";
            this.txtSearchFavorites.Size = new System.Drawing.Size(200, 21);
            this.txtSearchFavorites.TabIndex = 9;
            this.txtSearchFavorites.TextChanged += new System.EventHandler(this.txtSearchFavorites_TextChanged);
            // 
            // pnlHideTagsFavorites
            // 
            this.pnlHideTagsFavorites.BackColor = System.Drawing.Color.Gray;
            this.pnlHideTagsFavorites.Controls.Add(this.pbHideTagsFavorites);
            this.pnlHideTagsFavorites.Dock = System.Windows.Forms.DockStyle.Right;
            this.pnlHideTagsFavorites.Location = new System.Drawing.Point(293, 0);
            this.pnlHideTagsFavorites.Name = "pnlHideTagsFavorites";
            this.pnlHideTagsFavorites.Size = new System.Drawing.Size(5, 522);
            this.pnlHideTagsFavorites.TabIndex = 1;
            this.pnlHideTagsFavorites.Visible = false;
            // 
            // pbHideTagsFavorites
            // 
            this.pbHideTagsFavorites.Cursor = System.Windows.Forms.Cursors.Hand;
            this.pbHideTagsFavorites.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pbHideTagsFavorites.Image = global::Terminals.Properties.Resources.HidePanel;
            this.pbHideTagsFavorites.Location = new System.Drawing.Point(0, 0);
            this.pbHideTagsFavorites.Name = "pbHideTagsFavorites";
            this.pbHideTagsFavorites.Size = new System.Drawing.Size(5, 522);
            this.pbHideTagsFavorites.SizeMode = System.Windows.Forms.PictureBoxSizeMode.CenterImage;
            this.pbHideTagsFavorites.TabIndex = 2;
            this.pbHideTagsFavorites.TabStop = false;
            this.pbHideTagsFavorites.Click += new System.EventHandler(this.pbHideTags_Click);
            // 
            // pnlShowTagsFavorites
            // 
            this.pnlShowTagsFavorites.BackColor = System.Drawing.Color.Gray;
            this.pnlShowTagsFavorites.Controls.Add(this.pbShowTagsFavorites);
            this.pnlShowTagsFavorites.Dock = System.Windows.Forms.DockStyle.Left;
            this.pnlShowTagsFavorites.Location = new System.Drawing.Point(0, 0);
            this.pnlShowTagsFavorites.Name = "pnlShowTagsFavorites";
            this.pnlShowTagsFavorites.Size = new System.Drawing.Size(5, 522);
            this.pnlShowTagsFavorites.TabIndex = 0;
            // 
            // pbShowTagsFavorites
            // 
            this.pbShowTagsFavorites.Cursor = System.Windows.Forms.Cursors.Hand;
            this.pbShowTagsFavorites.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pbShowTagsFavorites.Image = global::Terminals.Properties.Resources.ShowPanel;
            this.pbShowTagsFavorites.Location = new System.Drawing.Point(0, 0);
            this.pbShowTagsFavorites.Name = "pbShowTagsFavorites";
            this.pbShowTagsFavorites.Size = new System.Drawing.Size(5, 522);
            this.pbShowTagsFavorites.SizeMode = System.Windows.Forms.PictureBoxSizeMode.CenterImage;
            this.pbShowTagsFavorites.TabIndex = 0;
            this.pbShowTagsFavorites.TabStop = false;
            this.pbShowTagsFavorites.Click += new System.EventHandler(this.pbShowTags_Click);
            // 
            // timerHover
            // 
            this.timerHover.Interval = 200;
            this.timerHover.Tick += new System.EventHandler(this.timerHover_Tick);
            // 
            // MainWindowNotifyIcon
            // 
            this.MainWindowNotifyIcon.BalloonTipIcon = System.Windows.Forms.ToolTipIcon.Info;
            this.MainWindowNotifyIcon.BalloonTipText = "Click to Show or Hide Terminals Main Window";
            this.MainWindowNotifyIcon.BalloonTipTitle = "Terminals";
            this.MainWindowNotifyIcon.ContextMenuStrip = this.SystemTrayContextMenuStrip;
            this.MainWindowNotifyIcon.Icon = ((System.Drawing.Icon)(resources.GetObject("MainWindowNotifyIcon.Icon")));
            this.MainWindowNotifyIcon.Text = "Click to Show or Hide Terminals Main Window";
            this.MainWindowNotifyIcon.Visible = true;
            this.MainWindowNotifyIcon.MouseClick += new System.Windows.Forms.MouseEventHandler(this.MainWindowNotifyIcon_MouseClick);
            // 
            // SystemTrayContextMenuStrip
            // 
            this.SystemTrayContextMenuStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripMenuItem2,
            this.newConnectionToolStripMenuItem,
            this.showToolStripMenuItem});
            this.SystemTrayContextMenuStrip.Name = "SystemTrayContextMenuStrip";
            this.SystemTrayContextMenuStrip.Size = new System.Drawing.Size(177, 70);
            this.SystemTrayContextMenuStrip.Text = "Terminals";
            // 
            // toolStripMenuItem2
            // 
            this.toolStripMenuItem2.Name = "toolStripMenuItem2";
            this.toolStripMenuItem2.Size = new System.Drawing.Size(176, 22);
            this.toolStripMenuItem2.Text = "E&xit";
            this.toolStripMenuItem2.Click += new System.EventHandler(this.toolStripMenuItem2_Click);
            // 
            // newConnectionToolStripMenuItem
            // 
            this.newConnectionToolStripMenuItem.Name = "newConnectionToolStripMenuItem";
            this.newConnectionToolStripMenuItem.Size = new System.Drawing.Size(176, 22);
            this.newConnectionToolStripMenuItem.Text = "&Organize Favorites";
            this.newConnectionToolStripMenuItem.Click += new System.EventHandler(this.newConnectionToolStripMenuItem_Click);
            // 
            // showToolStripMenuItem
            // 
            this.showToolStripMenuItem.Name = "showToolStripMenuItem";
            this.showToolStripMenuItem.Size = new System.Drawing.Size(176, 22);
            this.showToolStripMenuItem.Text = "Show";
            this.showToolStripMenuItem.Click += new System.EventHandler(this.showToolStripMenuItem_Click);
            // 
            // remoteDesktop1
            // 
            this.remoteDesktop1.AutoScroll = true;
            this.remoteDesktop1.AutoScrollMinSize = new System.Drawing.Size(608, 427);
            this.remoteDesktop1.Location = new System.Drawing.Point(131, 79);
            this.remoteDesktop1.Name = "remoteDesktop1";
            this.remoteDesktop1.Size = new System.Drawing.Size(400, 200);
            this.remoteDesktop1.TabIndex = 0;
            this.remoteDesktop1.Text = "remoteDesktop1";
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(792, 573);
            this.Controls.Add(this.toolStripContainer);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.KeyPreview = true;
            this.Location = new System.Drawing.Point(100, 100);
            this.MainMenuStrip = this.menuStrip;
            this.Name = "MainForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.Manual;
            this.Text = "Terminals 1.6 (RDP, VNC, VMRC, RAS, Telnet, SSH)";
            this.Shown += new System.EventHandler(this.MainForm_Shown);
            this.Activated += new System.EventHandler(this.MainForm_Activated);
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.MainForm_FormClosing);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.MainForm_KeyDown);
            this.Load += new System.EventHandler(this.MainForm_Load);
            this.menuStrip.ResumeLayout(false);
            this.menuStrip.PerformLayout();
            this.toolbarStd.ResumeLayout(false);
            this.toolbarStd.PerformLayout();
            this.toolStripContainer.ContentPanel.ResumeLayout(false);
            this.toolStripContainer.TopToolStripPanel.ResumeLayout(false);
            this.toolStripContainer.TopToolStripPanel.PerformLayout();
            this.toolStripContainer.ResumeLayout(false);
            this.toolStripContainer.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.tcTerminals)).EndInit();
            this.pnlTagsFavorites.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.tcTagsFavorites)).EndInit();
            this.tcTagsFavorites.ResumeLayout(false);
            this.tciTags.ResumeLayout(false);
            this.tciTags.PerformLayout();
            this.cmsTagConnections.ResumeLayout(false);
            this.cmsTags.ResumeLayout(false);
            this.tciFavorites.ResumeLayout(false);
            this.tciFavorites.PerformLayout();
            this.cmsFavorites.ResumeLayout(false);
            this.pnlHideTagsFavorites.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.pbHideTagsFavorites)).EndInit();
            this.pnlShowTagsFavorites.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.pbShowTagsFavorites)).EndInit();
            this.SystemTrayContextMenuStrip.ResumeLayout(false);
            this.ResumeLayout(false);
        }

        #endregion

        private System.Windows.Forms.MenuStrip menuStrip;
        private System.Windows.Forms.ToolStripMenuItem fileToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem newTerminalToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripMenuItem1;
        private System.Windows.Forms.ToolStripMenuItem exitToolStripMenuItem;
        private System.Windows.Forms.ToolStrip toolbarStd;
        private System.Windows.Forms.ToolStripButton tsbNewTerminal;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripLabel toolStripLabel1;
        private System.Windows.Forms.ToolStripComboBox tscConnectTo;
        private System.Windows.Forms.ToolStripButton tsbConnect;
        private System.Windows.Forms.ToolStripMenuItem viewToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
        public System.Windows.Forms.ToolStripButton tsbGrabInput;
        private System.Windows.Forms.ToolStripButton tsbDisconnect;
        private System.Windows.Forms.ToolStripMenuItem terminalToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem grabInputToolStripMenuItem;
        public TabControl.TabControl tcTerminals;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator5;
        private System.Windows.Forms.ToolStripMenuItem disconnectToolStripMenuItem;
        private System.Windows.Forms.ToolStripButton tsbManageConnections;
        private System.Windows.Forms.ToolStripMenuItem fullScreenToolStripMenuItem;
        private System.Windows.Forms.ToolStripButton tsbFullScreen;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator3;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator6;
        private System.Windows.Forms.ToolStripMenuItem favoritesToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem manageFavoritesToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator favoritesSeparator;
        private System.Windows.Forms.ToolStripMenuItem groupsToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem organizeGroupsToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem saveTerminalsAsGroupToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem addTerminalToGroupToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator groupsSeparator;
        private System.Windows.Forms.ToolStripContainer toolStripContainer;
        private System.Windows.Forms.Timer timerHover;
        private System.Windows.Forms.ToolStripMenuItem organizeFavoritesToolbarToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem toolsToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem optionsToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem helpToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem aboutToolStripMenuItem;
        private System.Windows.Forms.ToolStripButton tsbTags;
        private System.Windows.Forms.Panel pnlTagsFavorites;
        private System.Windows.Forms.Panel pnlHideTagsFavorites;
        private System.Windows.Forms.PictureBox pbHideTagsFavorites;
        private System.Windows.Forms.Panel pnlShowTagsFavorites;
        private System.Windows.Forms.PictureBox pbShowTagsFavorites;
        private System.Windows.Forms.ListView lvTagConnections;
        private System.Windows.Forms.ColumnHeader chConnection;
        private System.Windows.Forms.ListView lvTags;
        private System.Windows.Forms.ColumnHeader chTag;
        internal System.Windows.Forms.TextBox txtSearchTags;
        private System.Windows.Forms.ContextMenuStrip cmsTagConnections;
        private System.Windows.Forms.ToolStripMenuItem connectToolStripMenuItem;
        private System.Windows.Forms.ImageList ilTagConnections;
        private System.Windows.Forms.ContextMenuStrip cmsTags;
        private System.Windows.Forms.ToolStripMenuItem connectToAllToolStripMenuItem;
        private System.Windows.Forms.ImageList ilTags;
        private TabControl.TabControl tcTagsFavorites;
        private TabControl.TabControlItem tciTags;
        private TabControl.TabControlItem tciFavorites;
        private System.Windows.Forms.ToolStripButton tsbFavorites;
        internal System.Windows.Forms.TextBox txtSearchFavorites;
        private System.Windows.Forms.ListView lvFavorites;
        private System.Windows.Forms.ColumnHeader columnHeader1;
        private System.Windows.Forms.ContextMenuStrip cmsFavorites;
        private System.Windows.Forms.ToolStripMenuItem connectToolStripMenuItem1;
        private System.Windows.Forms.NotifyIcon MainWindowNotifyIcon;
        private VncSharp.RemoteDesktop remoteDesktop1;
        private System.Windows.Forms.ContextMenuStrip SystemTrayContextMenuStrip;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItem2;
        private System.Windows.Forms.ToolStripMenuItem newConnectionToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem showToolStripMenuItem;
        private System.Windows.Forms.ToolStripButton CaptureScreenToolStripButton;
        private System.Windows.Forms.ToolStripMenuItem captureTerminalScreenToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator4;
        private System.Windows.Forms.ToolStripButton VMRCAdminSwitchButton;
        private System.Windows.Forms.ToolStripButton VMRCViewOnlyButton;
        private System.Windows.Forms.Button button1;
    }
}