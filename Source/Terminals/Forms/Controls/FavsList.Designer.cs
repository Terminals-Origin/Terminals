namespace Terminals
{
    partial class FavsList
    {
        /// <summary> 
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if(disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.favoritesContextMenu = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.connectToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.extraConnectToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem2 = new System.Windows.Forms.ToolStripSeparator();
            this.pingToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.traceRouteToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.dNSToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.tSAdminToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem1 = new System.Windows.Forms.ToolStripSeparator();
            this.rebootToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.shutdownToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem4 = new System.Windows.Forms.ToolStripSeparator();
            this.computerManagementMMCToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.systemInformationToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.enableRDPToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem3 = new System.Windows.Forms.ToolStripSeparator();
            this.removeSelectedToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.duplicateToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.propertiesToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.FavoritesTabPage = new System.Windows.Forms.TabPage();
            this.favsTree = new Terminals.Forms.Controls.FavoritesTreeView();
            this.favoritesTreeMenu = new System.Windows.Forms.ToolStrip();
            this.addButton = new System.Windows.Forms.ToolStripButton();
            this.removeButton = new System.Windows.Forms.ToolStripButton();
            this.addGroupButton = new System.Windows.Forms.ToolStripButton();
            this.removeGroupButton = new System.Windows.Forms.ToolStripButton();
            this.connectButton = new System.Windows.Forms.ToolStripButton();
            this.collapseButton = new System.Windows.Forms.ToolStripButton();
            this.HistoryTabPage = new System.Windows.Forms.TabPage();
            this.historyTreeView = new Terminals.Forms.Controls.HistoryTreeView();
            this.historyTreeMenu = new System.Windows.Forms.ToolStrip();
            this.connectHistoryButton = new System.Windows.Forms.ToolStripButton();
            this.collpseHistoryButton = new System.Windows.Forms.ToolStripButton();
            this.clearHistoryButton = new System.Windows.Forms.ToolStripButton();
            this.defaultContextMenu = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.createGroupToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.createFavoriteToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.collapseAllToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.groupsContextMenu = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.createFavoriteToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.connectToAllMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.connectToAllExtraMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.deleteToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.setCredentialByTagToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.setUsernameByTagToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.setDomainByTagToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.setPasswordByTagToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.deleteAllFavoritesByTagToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.searchTabPage = new System.Windows.Forms.TabPage();
            this.searchPanel1 = new Terminals.Forms.Controls.SearchPanel();
            this.favoritesContextMenu.SuspendLayout();
            this.tabControl1.SuspendLayout();
            this.FavoritesTabPage.SuspendLayout();
            this.favoritesTreeMenu.SuspendLayout();
            this.HistoryTabPage.SuspendLayout();
            this.historyTreeMenu.SuspendLayout();
            this.defaultContextMenu.SuspendLayout();
            this.groupsContextMenu.SuspendLayout();
            this.searchTabPage.SuspendLayout();
            this.SuspendLayout();
            // 
            // favoritesContextMenu
            // 
            this.favoritesContextMenu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.connectToolStripMenuItem,
            this.extraConnectToolStripMenuItem,
            this.toolStripMenuItem2,
            this.pingToolStripMenuItem,
            this.traceRouteToolStripMenuItem,
            this.dNSToolStripMenuItem,
            this.tSAdminToolStripMenuItem,
            this.toolStripMenuItem1,
            this.rebootToolStripMenuItem,
            this.shutdownToolStripMenuItem,
            this.toolStripMenuItem4,
            this.computerManagementMMCToolStripMenuItem,
            this.systemInformationToolStripMenuItem,
            this.enableRDPToolStripMenuItem,
            this.toolStripMenuItem3,
            this.removeSelectedToolStripMenuItem,
            this.duplicateToolStripMenuItem,
            this.propertiesToolStripMenuItem});
            this.favoritesContextMenu.Name = "contextMenuStrip1";
            this.favoritesContextMenu.Size = new System.Drawing.Size(244, 336);
            // 
            // connectToolStripMenuItem
            // 
            this.connectToolStripMenuItem.Image = global::Terminals.Properties.Resources.application_lightning;
            this.connectToolStripMenuItem.Name = "connectToolStripMenuItem";
            this.connectToolStripMenuItem.Size = new System.Drawing.Size(243, 22);
            this.connectToolStripMenuItem.Text = "Connect";
            this.connectToolStripMenuItem.Click += new System.EventHandler(this.ConnectButton_Click);
            // 
            // extraConnectToolStripMenuItem
            // 
            this.extraConnectToolStripMenuItem.Name = "extraConnectToolStripMenuItem";
            this.extraConnectToolStripMenuItem.Size = new System.Drawing.Size(243, 22);
            this.extraConnectToolStripMenuItem.Text = "Connect with...";
            this.extraConnectToolStripMenuItem.Click += new System.EventHandler(this.ExtraConnectToolStripMenuItem_Click);
            // 
            // toolStripMenuItem2
            // 
            this.toolStripMenuItem2.Name = "toolStripMenuItem2";
            this.toolStripMenuItem2.Size = new System.Drawing.Size(240, 6);
            // 
            // pingToolStripMenuItem
            // 
            this.pingToolStripMenuItem.Name = "pingToolStripMenuItem";
            this.pingToolStripMenuItem.Size = new System.Drawing.Size(243, 22);
            this.pingToolStripMenuItem.Text = "Ping";
            this.pingToolStripMenuItem.ToolTipText = "Opens networking tool and starts PING the target machine.";
            this.pingToolStripMenuItem.Click += new System.EventHandler(this.PingToolStripMenuItem_Click);
            // 
            // traceRouteToolStripMenuItem
            // 
            this.traceRouteToolStripMenuItem.Name = "traceRouteToolStripMenuItem";
            this.traceRouteToolStripMenuItem.Size = new System.Drawing.Size(243, 22);
            this.traceRouteToolStripMenuItem.Text = "Trace Route";
            this.traceRouteToolStripMenuItem.ToolTipText = "Opens networking tool and starts TRACE the target machine.";
            this.traceRouteToolStripMenuItem.Click += new System.EventHandler(this.TraceRouteToolStripMenuItem_Click);
            // 
            // dNSToolStripMenuItem
            // 
            this.dNSToolStripMenuItem.Name = "dNSToolStripMenuItem";
            this.dNSToolStripMenuItem.Size = new System.Drawing.Size(243, 22);
            this.dNSToolStripMenuItem.Text = "DNS";
            this.dNSToolStripMenuItem.ToolTipText = "Opens networking tool and tries to resolve DNS name of the target machine.";
            this.dNSToolStripMenuItem.Click += new System.EventHandler(this.DNsToolStripMenuItem_Click);
            // 
            // tSAdminToolStripMenuItem
            // 
            this.tSAdminToolStripMenuItem.Name = "tSAdminToolStripMenuItem";
            this.tSAdminToolStripMenuItem.Size = new System.Drawing.Size(243, 22);
            this.tSAdminToolStripMenuItem.Text = "TS Admin";
            this.tSAdminToolStripMenuItem.ToolTipText = "Tries to open the Terminals services configuration of the target machine.\r\nRequir" +
    "es admin privileges on the target machine.";
            this.tSAdminToolStripMenuItem.Click += new System.EventHandler(this.TsAdminToolStripMenuItem_Click);
            // 
            // toolStripMenuItem1
            // 
            this.toolStripMenuItem1.Name = "toolStripMenuItem1";
            this.toolStripMenuItem1.Size = new System.Drawing.Size(240, 6);
            // 
            // rebootToolStripMenuItem
            // 
            this.rebootToolStripMenuItem.Name = "rebootToolStripMenuItem";
            this.rebootToolStripMenuItem.Size = new System.Drawing.Size(243, 22);
            this.rebootToolStripMenuItem.Text = "Reboot";
            this.rebootToolStripMenuItem.ToolTipText = "Forces reboot of the computer. Forces all users to log off.\r\nRequires admin acces" +
    "s (used favorite credentials).\r\nRemote management has to be enabled on the targe" +
    "t machine.\r\n";
            this.rebootToolStripMenuItem.Click += new System.EventHandler(this.RebootToolStripMenuItem_Click);
            // 
            // shutdownToolStripMenuItem
            // 
            this.shutdownToolStripMenuItem.Name = "shutdownToolStripMenuItem";
            this.shutdownToolStripMenuItem.Size = new System.Drawing.Size(243, 22);
            this.shutdownToolStripMenuItem.Text = "Shutdown";
            this.shutdownToolStripMenuItem.ToolTipText = "Forces shutdown of the computer. Forces all users to log off.\r\nRequires admin acc" +
    "ess (used favorite credentials).\r\nRemote management has to be enabled on the tar" +
    "get machine.";
            this.shutdownToolStripMenuItem.Click += new System.EventHandler(this.ShutdownToolStripMenuItem_Click);
            // 
            // toolStripMenuItem4
            // 
            this.toolStripMenuItem4.Name = "toolStripMenuItem4";
            this.toolStripMenuItem4.Size = new System.Drawing.Size(240, 6);
            // 
            // computerManagementMMCToolStripMenuItem
            // 
            this.computerManagementMMCToolStripMenuItem.Name = "computerManagementMMCToolStripMenuItem";
            this.computerManagementMMCToolStripMenuItem.Size = new System.Drawing.Size(243, 22);
            this.computerManagementMMCToolStripMenuItem.Text = "Computer Management (MMC)";
            this.computerManagementMMCToolStripMenuItem.Click += new System.EventHandler(this.ComputerManagementMmcToolStripMenuItem_Click);
            // 
            // systemInformationToolStripMenuItem
            // 
            this.systemInformationToolStripMenuItem.Name = "systemInformationToolStripMenuItem";
            this.systemInformationToolStripMenuItem.Size = new System.Drawing.Size(243, 22);
            this.systemInformationToolStripMenuItem.Text = "System Information";
            this.systemInformationToolStripMenuItem.Click += new System.EventHandler(this.SystemInformationToolStripMenuItem_Click);
            // 
            // enableRDPToolStripMenuItem
            // 
            this.enableRDPToolStripMenuItem.Name = "enableRDPToolStripMenuItem";
            this.enableRDPToolStripMenuItem.Size = new System.Drawing.Size(243, 22);
            this.enableRDPToolStripMenuItem.Text = "Enable RDP";
            this.enableRDPToolStripMenuItem.ToolTipText = "Enable RDP service on target computer.\r\nRequires admin access to the computer (us" +
    "ed favorite credentials).\r\nThe computer also needs to have enabled remote access" +
    " to the registry.";
            this.enableRDPToolStripMenuItem.Click += new System.EventHandler(this.EnableRDPToolStripMenuItem_Click);
            // 
            // toolStripMenuItem3
            // 
            this.toolStripMenuItem3.Name = "toolStripMenuItem3";
            this.toolStripMenuItem3.Size = new System.Drawing.Size(240, 6);
            // 
            // removeSelectedToolStripMenuItem
            // 
            this.removeSelectedToolStripMenuItem.Image = global::Terminals.Properties.Resources.delete;
            this.removeSelectedToolStripMenuItem.Name = "removeSelectedToolStripMenuItem";
            this.removeSelectedToolStripMenuItem.Size = new System.Drawing.Size(243, 22);
            this.removeSelectedToolStripMenuItem.Text = "Remove";
            this.removeSelectedToolStripMenuItem.Click += new System.EventHandler(this.RemoveSelectedToolStripMenuItem_Click);
            // 
            // duplicateToolStripMenuItem
            // 
            this.duplicateToolStripMenuItem.Name = "duplicateToolStripMenuItem";
            this.duplicateToolStripMenuItem.Size = new System.Drawing.Size(243, 22);
            this.duplicateToolStripMenuItem.Text = "Duplicate";
            this.duplicateToolStripMenuItem.ToolTipText = "Creates copy of first selected favorite including its group assignment";
            this.duplicateToolStripMenuItem.Click += new System.EventHandler(this.DuplicateToolStripMenuItem_Click);
            // 
            // propertiesToolStripMenuItem
            // 
            this.propertiesToolStripMenuItem.Image = global::Terminals.Properties.Resources.Properties;
            this.propertiesToolStripMenuItem.Name = "propertiesToolStripMenuItem";
            this.propertiesToolStripMenuItem.Size = new System.Drawing.Size(243, 22);
            this.propertiesToolStripMenuItem.Text = "Properties";
            this.propertiesToolStripMenuItem.ToolTipText = "Edit this favorite properties";
            this.propertiesToolStripMenuItem.Click += new System.EventHandler(this.PropertiesToolStripMenuItem_Click);
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.searchTabPage);
            this.tabControl1.Controls.Add(this.FavoritesTabPage);
            this.tabControl1.Controls.Add(this.HistoryTabPage);
            this.tabControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControl1.Location = new System.Drawing.Point(0, 0);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(173, 179);
            this.tabControl1.TabIndex = 1;
            // 
            // FavoritesTabPage
            // 
            this.FavoritesTabPage.Controls.Add(this.favsTree);
            this.FavoritesTabPage.Controls.Add(this.favoritesTreeMenu);
            this.FavoritesTabPage.Location = new System.Drawing.Point(4, 22);
            this.FavoritesTabPage.Name = "FavoritesTabPage";
            this.FavoritesTabPage.Padding = new System.Windows.Forms.Padding(3);
            this.FavoritesTabPage.Size = new System.Drawing.Size(165, 153);
            this.FavoritesTabPage.TabIndex = 0;
            this.FavoritesTabPage.Text = "Favorites";
            this.FavoritesTabPage.UseVisualStyleBackColor = true;
            // 
            // favsTree
            // 
            this.favsTree.AllowDrop = true;
            this.favsTree.CausesValidation = false;
            this.favsTree.Dock = System.Windows.Forms.DockStyle.Fill;
            this.favsTree.HideSelection = false;
            this.favsTree.HotTracking = true;
            this.favsTree.ImageIndex = 0;
            this.favsTree.Location = new System.Drawing.Point(3, 28);
            this.favsTree.Name = "favsTree";
            this.favsTree.SelectedImageIndex = 0;
            this.favsTree.ShowNodeToolTips = true;
            this.favsTree.Size = new System.Drawing.Size(159, 122);
            this.favsTree.TabIndex = 2;
            this.favsTree.DragDrop += new System.Windows.Forms.DragEventHandler(this.FavsTree_DragDrop);
            this.favsTree.DragEnter += new System.Windows.Forms.DragEventHandler(this.FavsTree_DragEnter);
            this.favsTree.DoubleClick += new System.EventHandler(this.FavsTree_DoubleClick);
            // 
            // favoritesTreeMenu
            // 
            this.favoritesTreeMenu.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
            this.favoritesTreeMenu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.addButton,
            this.removeButton,
            this.addGroupButton,
            this.removeGroupButton,
            this.connectButton,
            this.collapseButton});
            this.favoritesTreeMenu.Location = new System.Drawing.Point(3, 3);
            this.favoritesTreeMenu.Name = "favoritesTreeMenu";
            this.favoritesTreeMenu.Size = new System.Drawing.Size(159, 25);
            this.favoritesTreeMenu.TabIndex = 1;
            this.favoritesTreeMenu.Text = "Favorites tree menu";
            // 
            // addButton
            // 
            this.addButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.addButton.Image = global::Terminals.Properties.Resources.add;
            this.addButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.addButton.Name = "addButton";
            this.addButton.Size = new System.Drawing.Size(23, 22);
            this.addButton.Text = "New Connection";
            this.addButton.Click += new System.EventHandler(this.CreateFavoriteToolStripMenuItem_Click);
            // 
            // removeButton
            // 
            this.removeButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.removeButton.Image = global::Terminals.Properties.Resources.delete;
            this.removeButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.removeButton.Name = "removeButton";
            this.removeButton.Size = new System.Drawing.Size(23, 22);
            this.removeButton.Text = "Remove favorite";
            this.removeButton.Click += new System.EventHandler(this.RemoveSelectedToolStripMenuItem_Click);
            // 
            // addGroupButton
            // 
            this.addGroupButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.addGroupButton.Image = global::Terminals.Properties.Resources.tag_blue_add;
            this.addGroupButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.addGroupButton.Name = "addGroupButton";
            this.addGroupButton.Size = new System.Drawing.Size(23, 22);
            this.addGroupButton.Text = "Create Group";
            this.addGroupButton.Click += new System.EventHandler(this.CreateGroupToolStripMenuItem_Click);
            // 
            // removeGroupButton
            // 
            this.removeGroupButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.removeGroupButton.Image = global::Terminals.Properties.Resources.tag_blue_delete;
            this.removeGroupButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.removeGroupButton.Name = "removeGroupButton";
            this.removeGroupButton.Size = new System.Drawing.Size(23, 22);
            this.removeGroupButton.Text = "Delete Group";
            this.removeGroupButton.Click += new System.EventHandler(this.DeleteGroupToolStripMenuItem_Click);
            // 
            // connectButton
            // 
            this.connectButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.connectButton.Image = global::Terminals.Properties.Resources.application_lightning;
            this.connectButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.connectButton.Name = "connectButton";
            this.connectButton.Size = new System.Drawing.Size(23, 22);
            this.connectButton.Text = "Connect";
            this.connectButton.Click += new System.EventHandler(this.ConnectButton_Click);
            // 
            // collapseButton
            // 
            this.collapseButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.collapseButton.Image = global::Terminals.Properties.Resources.collapse_all;
            this.collapseButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.collapseButton.Name = "collapseButton";
            this.collapseButton.Size = new System.Drawing.Size(23, 22);
            this.collapseButton.Text = "Collapse";
            this.collapseButton.ToolTipText = "Collapse favorites tree";
            this.collapseButton.Click += new System.EventHandler(this.CollapseAllToolStripMenuItem_Click);
            // 
            // HistoryTabPage
            // 
            this.HistoryTabPage.Controls.Add(this.historyTreeView);
            this.HistoryTabPage.Controls.Add(this.historyTreeMenu);
            this.HistoryTabPage.Location = new System.Drawing.Point(4, 22);
            this.HistoryTabPage.Name = "HistoryTabPage";
            this.HistoryTabPage.Padding = new System.Windows.Forms.Padding(3);
            this.HistoryTabPage.Size = new System.Drawing.Size(165, 153);
            this.HistoryTabPage.TabIndex = 1;
            this.HistoryTabPage.Text = "History";
            this.HistoryTabPage.UseVisualStyleBackColor = true;
            // 
            // historyTreeView
            // 
            this.historyTreeView.Dock = System.Windows.Forms.DockStyle.Fill;
            this.historyTreeView.HotTracking = true;
            this.historyTreeView.ImageIndex = 0;
            this.historyTreeView.Location = new System.Drawing.Point(3, 28);
            this.historyTreeView.Name = "historyTreeView";
            this.historyTreeView.SelectedImageIndex = 0;
            this.historyTreeView.ShowNodeToolTips = true;
            this.historyTreeView.Size = new System.Drawing.Size(159, 122);
            this.historyTreeView.TabIndex = 2;
            this.historyTreeView.KeyUp += new System.Windows.Forms.KeyEventHandler(this.HistoryTreeView_KeyUp);
            // 
            // historyTreeMenu
            // 
            this.historyTreeMenu.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
            this.historyTreeMenu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.connectHistoryButton,
            this.collpseHistoryButton,
            this.clearHistoryButton});
            this.historyTreeMenu.Location = new System.Drawing.Point(3, 3);
            this.historyTreeMenu.Name = "historyTreeMenu";
            this.historyTreeMenu.Size = new System.Drawing.Size(159, 25);
            this.historyTreeMenu.TabIndex = 1;
            this.historyTreeMenu.Text = "History tree menu";
            // 
            // connectHistoryButton
            // 
            this.connectHistoryButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.connectHistoryButton.Image = global::Terminals.Properties.Resources.application_lightning;
            this.connectHistoryButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.connectHistoryButton.Name = "connectHistoryButton";
            this.connectHistoryButton.Size = new System.Drawing.Size(23, 22);
            this.connectHistoryButton.Text = "Connect";
            this.connectHistoryButton.Click += new System.EventHandler(this.HistoryTreeView_DoubleClick);
            // 
            // collpseHistoryButton
            // 
            this.collpseHistoryButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.collpseHistoryButton.Image = global::Terminals.Properties.Resources.collapse_all;
            this.collpseHistoryButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.collpseHistoryButton.Name = "collpseHistoryButton";
            this.collpseHistoryButton.Size = new System.Drawing.Size(23, 22);
            this.collpseHistoryButton.Text = "Collapse all";
            this.collpseHistoryButton.Click += new System.EventHandler(this.CollpseHistoryButton_Click);
            // 
            // clearHistoryButton
            // 
            this.clearHistoryButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.clearHistoryButton.Image = global::Terminals.Properties.Resources.history_icon_today;
            this.clearHistoryButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.clearHistoryButton.Name = "clearHistoryButton";
            this.clearHistoryButton.Size = new System.Drawing.Size(23, 22);
            this.clearHistoryButton.Text = "Clear history";
            this.clearHistoryButton.Click += new System.EventHandler(this.ClearHistoryButton_Click);
            // 
            // defaultContextMenu
            // 
            this.defaultContextMenu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.createGroupToolStripMenuItem,
            this.createFavoriteToolStripMenuItem1,
            this.collapseAllToolStripMenuItem});
            this.defaultContextMenu.Name = "defaultContextMenu";
            this.defaultContextMenu.Size = new System.Drawing.Size(164, 70);
            // 
            // createGroupToolStripMenuItem
            // 
            this.createGroupToolStripMenuItem.Image = global::Terminals.Properties.Resources.tag_blue_add;
            this.createGroupToolStripMenuItem.Name = "createGroupToolStripMenuItem";
            this.createGroupToolStripMenuItem.Size = new System.Drawing.Size(163, 22);
            this.createGroupToolStripMenuItem.Text = "Create Group";
            this.createGroupToolStripMenuItem.Click += new System.EventHandler(this.CreateGroupToolStripMenuItem_Click);
            // 
            // createFavoriteToolStripMenuItem1
            // 
            this.createFavoriteToolStripMenuItem1.Image = global::Terminals.Properties.Resources.add;
            this.createFavoriteToolStripMenuItem1.Name = "createFavoriteToolStripMenuItem1";
            this.createFavoriteToolStripMenuItem1.Size = new System.Drawing.Size(163, 22);
            this.createFavoriteToolStripMenuItem1.Text = "New Connection";
            this.createFavoriteToolStripMenuItem1.Click += new System.EventHandler(this.CreateFavoriteToolStripMenuItem_Click);
            // 
            // collapseAllToolStripMenuItem
            // 
            this.collapseAllToolStripMenuItem.Image = global::Terminals.Properties.Resources.collapse_all;
            this.collapseAllToolStripMenuItem.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.collapseAllToolStripMenuItem.Name = "collapseAllToolStripMenuItem";
            this.collapseAllToolStripMenuItem.Size = new System.Drawing.Size(163, 22);
            this.collapseAllToolStripMenuItem.Text = "Collapse all";
            this.collapseAllToolStripMenuItem.Click += new System.EventHandler(this.CollapseAllToolStripMenuItem_Click);
            // 
            // groupsContextMenu
            // 
            this.groupsContextMenu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.createFavoriteToolStripMenuItem,
            this.connectToAllMenuItem,
            this.connectToAllExtraMenuItem,
            this.deleteToolStripMenuItem,
            this.toolStripSeparator1,
            this.setCredentialByTagToolStripMenuItem,
            this.setUsernameByTagToolStripMenuItem,
            this.setDomainByTagToolStripMenuItem,
            this.setPasswordByTagToolStripMenuItem,
            this.deleteAllFavoritesByTagToolStripMenuItem});
            this.groupsContextMenu.Name = "contextMenuStrip2";
            this.groupsContextMenu.Size = new System.Drawing.Size(235, 208);
            // 
            // createFavoriteToolStripMenuItem
            // 
            this.createFavoriteToolStripMenuItem.Image = global::Terminals.Properties.Resources.add;
            this.createFavoriteToolStripMenuItem.Name = "createFavoriteToolStripMenuItem";
            this.createFavoriteToolStripMenuItem.Size = new System.Drawing.Size(234, 22);
            this.createFavoriteToolStripMenuItem.Text = "New Connection";
            this.createFavoriteToolStripMenuItem.Click += new System.EventHandler(this.CreateFavoriteToolStripMenuItem_Click);
            // 
            // connectToAllMenuItem
            // 
            this.connectToAllMenuItem.Image = global::Terminals.Properties.Resources.application_lightning;
            this.connectToAllMenuItem.Name = "connectToAllMenuItem";
            this.connectToAllMenuItem.Size = new System.Drawing.Size(234, 22);
            this.connectToAllMenuItem.Text = "Connect to All";
            this.connectToAllMenuItem.Click += new System.EventHandler(this.ConnectButton_Click);
            // 
            // connectToAllExtraMenuItem
            // 
            this.connectToAllExtraMenuItem.Name = "connectToAllExtraMenuItem";
            this.connectToAllExtraMenuItem.Size = new System.Drawing.Size(234, 22);
            this.connectToAllExtraMenuItem.Text = "Connect to All with...";
            this.connectToAllExtraMenuItem.Click += new System.EventHandler(this.ConnectToAllExtraToolStripMenuItem_Click);
            // 
            // deleteToolStripMenuItem
            // 
            this.deleteToolStripMenuItem.Image = global::Terminals.Properties.Resources.tag_blue_delete;
            this.deleteToolStripMenuItem.Name = "deleteToolStripMenuItem";
            this.deleteToolStripMenuItem.Size = new System.Drawing.Size(234, 22);
            this.deleteToolStripMenuItem.Text = "Delete";
            this.deleteToolStripMenuItem.Click += new System.EventHandler(this.DeleteGroupToolStripMenuItem_Click);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(231, 6);
            // 
            // setCredentialByTagToolStripMenuItem
            // 
            this.setCredentialByTagToolStripMenuItem.Name = "setCredentialByTagToolStripMenuItem";
            this.setCredentialByTagToolStripMenuItem.Size = new System.Drawing.Size(234, 22);
            this.setCredentialByTagToolStripMenuItem.Text = "Set Credential to all in Group...";
            this.setCredentialByTagToolStripMenuItem.Click += new System.EventHandler(this.SetCredentialByTagToolStripMenuItem_Click);
            // 
            // setUsernameByTagToolStripMenuItem
            // 
            this.setUsernameByTagToolStripMenuItem.Name = "setUsernameByTagToolStripMenuItem";
            this.setUsernameByTagToolStripMenuItem.Size = new System.Drawing.Size(234, 22);
            this.setUsernameByTagToolStripMenuItem.Text = "Set Username to all in Group...";
            this.setUsernameByTagToolStripMenuItem.Click += new System.EventHandler(this.SetUsernameByTagToolStripMenuItem_Click);
            // 
            // setDomainByTagToolStripMenuItem
            // 
            this.setDomainByTagToolStripMenuItem.Name = "setDomainByTagToolStripMenuItem";
            this.setDomainByTagToolStripMenuItem.Size = new System.Drawing.Size(234, 22);
            this.setDomainByTagToolStripMenuItem.Text = "Set Domain to all in Group...";
            this.setDomainByTagToolStripMenuItem.Click += new System.EventHandler(this.SetDomainByTagToolStripMenuItem_Click);
            // 
            // setPasswordByTagToolStripMenuItem
            // 
            this.setPasswordByTagToolStripMenuItem.Name = "setPasswordByTagToolStripMenuItem";
            this.setPasswordByTagToolStripMenuItem.Size = new System.Drawing.Size(234, 22);
            this.setPasswordByTagToolStripMenuItem.Text = "Set Password to all in Group...";
            this.setPasswordByTagToolStripMenuItem.Click += new System.EventHandler(this.SetPasswordByTagToolStripMenuItem_Click);
            // 
            // deleteAllFavoritesByTagToolStripMenuItem
            // 
            this.deleteAllFavoritesByTagToolStripMenuItem.Image = global::Terminals.Properties.Resources.delete;
            this.deleteAllFavoritesByTagToolStripMenuItem.Name = "deleteAllFavoritesByTagToolStripMenuItem";
            this.deleteAllFavoritesByTagToolStripMenuItem.Size = new System.Drawing.Size(234, 22);
            this.deleteAllFavoritesByTagToolStripMenuItem.Text = "Delete all Favorites in Group...";
            this.deleteAllFavoritesByTagToolStripMenuItem.Click += new System.EventHandler(this.DeleteAllFavoritesByTagToolStripMenuItem_Click);
            // 
            // searchTabPage
            // 
            this.searchTabPage.Controls.Add(this.searchPanel1);
            this.searchTabPage.Location = new System.Drawing.Point(4, 22);
            this.searchTabPage.Name = "searchTabPage";
            this.searchTabPage.Padding = new System.Windows.Forms.Padding(3);
            this.searchTabPage.Size = new System.Drawing.Size(165, 153);
            this.searchTabPage.TabIndex = 2;
            this.searchTabPage.Text = "Search";
            this.searchTabPage.UseVisualStyleBackColor = true;
            // 
            // searchPanel1
            // 
            this.searchPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.searchPanel1.Location = new System.Drawing.Point(3, 3);
            this.searchPanel1.Name = "searchPanel1";
            this.searchPanel1.Size = new System.Drawing.Size(159, 147);
            this.searchPanel1.TabIndex = 0;
            // 
            // FavsList
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.tabControl1);
            this.Name = "FavsList";
            this.Size = new System.Drawing.Size(173, 179);
            this.Load += new System.EventHandler(this.FavsList_Load);
            this.favoritesContextMenu.ResumeLayout(false);
            this.tabControl1.ResumeLayout(false);
            this.FavoritesTabPage.ResumeLayout(false);
            this.FavoritesTabPage.PerformLayout();
            this.favoritesTreeMenu.ResumeLayout(false);
            this.favoritesTreeMenu.PerformLayout();
            this.HistoryTabPage.ResumeLayout(false);
            this.HistoryTabPage.PerformLayout();
            this.historyTreeMenu.ResumeLayout(false);
            this.historyTreeMenu.PerformLayout();
            this.defaultContextMenu.ResumeLayout(false);
            this.groupsContextMenu.ResumeLayout(false);
            this.searchTabPage.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.ContextMenuStrip favoritesContextMenu;
        private System.Windows.Forms.ToolStripMenuItem pingToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem dNSToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem tSAdminToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem traceRouteToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem propertiesToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripMenuItem2;
        private System.Windows.Forms.ToolStripSeparator toolStripMenuItem1;
        private System.Windows.Forms.ToolStripMenuItem rebootToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripMenuItem3;
        private System.Windows.Forms.ToolStripMenuItem enableRDPToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem computerManagementMMCToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripMenuItem4;
        private System.Windows.Forms.ToolStripMenuItem systemInformationToolStripMenuItem;
        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage FavoritesTabPage;
        private System.Windows.Forms.TabPage HistoryTabPage;
        private System.Windows.Forms.ToolStripMenuItem removeSelectedToolStripMenuItem;
        private System.Windows.Forms.ContextMenuStrip groupsContextMenu;
        private System.Windows.Forms.ToolStripMenuItem connectToAllExtraMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripMenuItem setCredentialByTagToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem setUsernameByTagToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem setDomainByTagToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem setPasswordByTagToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem deleteAllFavoritesByTagToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem shutdownToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem createFavoriteToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem connectToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem extraConnectToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem connectToAllMenuItem;
        private System.Windows.Forms.ContextMenuStrip defaultContextMenu;
        private System.Windows.Forms.ToolStripMenuItem createGroupToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem deleteToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem duplicateToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem createFavoriteToolStripMenuItem1;
        private Forms.Controls.FavoritesTreeView favsTree;
        private System.Windows.Forms.ToolStrip favoritesTreeMenu;
        private System.Windows.Forms.ToolStripButton addButton;
        private System.Windows.Forms.ToolStripButton removeButton;
        private System.Windows.Forms.ToolStripButton addGroupButton;
        private System.Windows.Forms.ToolStripButton removeGroupButton;
        private System.Windows.Forms.ToolStripButton connectButton;
        private System.Windows.Forms.ToolStripMenuItem collapseAllToolStripMenuItem;
        private System.Windows.Forms.ToolStripButton collapseButton;
        private Forms.Controls.HistoryTreeView historyTreeView;
        private System.Windows.Forms.ToolStrip historyTreeMenu;
        private System.Windows.Forms.ToolStripButton connectHistoryButton;
        private System.Windows.Forms.ToolStripButton collpseHistoryButton;
        private System.Windows.Forms.ToolStripButton clearHistoryButton;
        private System.Windows.Forms.TabPage searchTabPage;
        private Forms.Controls.SearchPanel searchPanel1;
    }
}
