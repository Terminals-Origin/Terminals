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
            this.favsTree = new Terminals.Forms.Controls.FavoritesTreeView();
            this.favoritesContextMenu = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.connectToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.consoleToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.newWindowToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this.connectAsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.userConnectToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem2 = new System.Windows.Forms.ToolStripSeparator();
            this.pingToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.traceRouteToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.dNSToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.tSAdminToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem1 = new System.Windows.Forms.ToolStripSeparator();
            this.removeSelectedToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.rebootToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.shutdownToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem4 = new System.Windows.Forms.ToolStripSeparator();
            this.computerManagementMMCToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.systemInformationToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.enableRDPToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem3 = new System.Windows.Forms.ToolStripSeparator();
            this.propertiesToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.FavoritesTabPage = new System.Windows.Forms.TabPage();
            this.HistoryTabPage = new System.Windows.Forms.TabPage();
            this.historyTreeView = new Terminals.Forms.Controls.HistoryTreeView();
            this.groupsContextMenu = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.createFavoriteToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.connectToAllToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.consoleAllToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.newWindowAllToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.setCredentialByTagToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.setUsernameByTagToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.setDomainByTagToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.setPasswordByTagToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.deleteAllFavoritesByTagToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.favoritesContextMenu.SuspendLayout();
            this.tabControl1.SuspendLayout();
            this.FavoritesTabPage.SuspendLayout();
            this.HistoryTabPage.SuspendLayout();
            this.groupsContextMenu.SuspendLayout();
            this.SuspendLayout();
            // 
            // favsTree
            // 
            this.favsTree.AllowDrop = true;
            this.favsTree.CausesValidation = false;
            this.favsTree.ContextMenuStrip = this.favoritesContextMenu;
            this.favsTree.Dock = System.Windows.Forms.DockStyle.Fill;
            this.favsTree.HideSelection = false;
            this.favsTree.HotTracking = true;
            this.favsTree.ImageIndex = 0;
            this.favsTree.Location = new System.Drawing.Point(3, 3);
            this.favsTree.Name = "favsTree";
            this.favsTree.SelectedImageIndex = 0;
            this.favsTree.ShowNodeToolTips = true;
            this.favsTree.Size = new System.Drawing.Size(136, 118);
            this.favsTree.TabIndex = 0;
            this.favsTree.DragDrop += new System.Windows.Forms.DragEventHandler(this.FavsTree_DragDrop);
            this.favsTree.DragEnter += new System.Windows.Forms.DragEventHandler(this.FavsTree_DragEnter);
            this.favsTree.DoubleClick += new System.EventHandler(this.FavsTree_DoubleClick);
            // 
            // favoritesContextMenu
            // 
            this.favoritesContextMenu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.connectToolStripMenuItem,
            this.toolStripMenuItem2,
            this.pingToolStripMenuItem,
            this.traceRouteToolStripMenuItem,
            this.dNSToolStripMenuItem,
            this.tSAdminToolStripMenuItem,
            this.toolStripMenuItem1,
            this.removeSelectedToolStripMenuItem,
            this.rebootToolStripMenuItem,
            this.shutdownToolStripMenuItem,
            this.toolStripMenuItem4,
            this.computerManagementMMCToolStripMenuItem,
            this.systemInformationToolStripMenuItem,
            this.enableRDPToolStripMenuItem,
            this.toolStripMenuItem3,
            this.propertiesToolStripMenuItem});
            this.favoritesContextMenu.Name = "contextMenuStrip1";
            this.favoritesContextMenu.Size = new System.Drawing.Size(244, 292);
            // 
            // connectToolStripMenuItem
            // 
            this.connectToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.consoleToolStripMenuItem,
            this.newWindowToolStripMenuItem,
            this.toolStripSeparator2,
            this.connectAsToolStripMenuItem});
            this.connectToolStripMenuItem.Image = global::Terminals.Properties.Resources.application_lightning;
            this.connectToolStripMenuItem.Name = "connectToolStripMenuItem";
            this.connectToolStripMenuItem.Size = new System.Drawing.Size(243, 22);
            this.connectToolStripMenuItem.Text = "Connect";
            this.connectToolStripMenuItem.Click += new System.EventHandler(this.ConnectToolStripMenuItem_Click);
            // 
            // consoleToolStripMenuItem
            // 
            this.consoleToolStripMenuItem.CheckOnClick = true;
            this.consoleToolStripMenuItem.Name = "consoleToolStripMenuItem";
            this.consoleToolStripMenuItem.Size = new System.Drawing.Size(145, 22);
            this.consoleToolStripMenuItem.Text = "Console";
            this.consoleToolStripMenuItem.Click += new System.EventHandler(this.DisplayWindow_Click);
            // 
            // newWindowToolStripMenuItem
            // 
            this.newWindowToolStripMenuItem.CheckOnClick = true;
            this.newWindowToolStripMenuItem.Name = "newWindowToolStripMenuItem";
            this.newWindowToolStripMenuItem.Size = new System.Drawing.Size(145, 22);
            this.newWindowToolStripMenuItem.Text = "New Window";
            this.newWindowToolStripMenuItem.Click += new System.EventHandler(this.DisplayWindow_Click);
            // 
            // toolStripSeparator2
            // 
            this.toolStripSeparator2.Name = "toolStripSeparator2";
            this.toolStripSeparator2.Size = new System.Drawing.Size(142, 6);
            // 
            // connectAsToolStripMenuItem
            // 
            this.connectAsToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.userConnectToolStripMenuItem});
            this.connectAsToolStripMenuItem.Name = "connectAsToolStripMenuItem";
            this.connectAsToolStripMenuItem.Size = new System.Drawing.Size(145, 22);
            this.connectAsToolStripMenuItem.Text = "Connect As";
            this.connectAsToolStripMenuItem.DropDownOpening += new System.EventHandler(this.ConnectAsToolStripMenuItem_DropDownOpening);
            // 
            // userConnectToolStripMenuItem
            // 
            this.userConnectToolStripMenuItem.Name = "userConnectToolStripMenuItem";
            this.userConnectToolStripMenuItem.Size = new System.Drawing.Size(106, 22);
            this.userConnectToolStripMenuItem.Text = "User...";
            this.userConnectToolStripMenuItem.Click += new System.EventHandler(this.UserConnectToolStripMenuItem_Click);
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
            // removeSelectedToolStripMenuItem
            // 
            this.removeSelectedToolStripMenuItem.Image = global::Terminals.Properties.Resources.delete;
            this.removeSelectedToolStripMenuItem.Name = "removeSelectedToolStripMenuItem";
            this.removeSelectedToolStripMenuItem.Size = new System.Drawing.Size(243, 22);
            this.removeSelectedToolStripMenuItem.Text = "Remove Selected";
            this.removeSelectedToolStripMenuItem.Click += new System.EventHandler(this.RemoveSelectedToolStripMenuItem_Click);
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
            this.tabControl1.Controls.Add(this.FavoritesTabPage);
            this.tabControl1.Controls.Add(this.HistoryTabPage);
            this.tabControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControl1.Location = new System.Drawing.Point(0, 0);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(150, 150);
            this.tabControl1.TabIndex = 1;
            // 
            // FavoritesTabPage
            // 
            this.FavoritesTabPage.Controls.Add(this.favsTree);
            this.FavoritesTabPage.Location = new System.Drawing.Point(4, 22);
            this.FavoritesTabPage.Name = "FavoritesTabPage";
            this.FavoritesTabPage.Padding = new System.Windows.Forms.Padding(3);
            this.FavoritesTabPage.Size = new System.Drawing.Size(142, 124);
            this.FavoritesTabPage.TabIndex = 0;
            this.FavoritesTabPage.Text = "Favorites";
            this.FavoritesTabPage.UseVisualStyleBackColor = true;
            // 
            // HistoryTabPage
            // 
            this.HistoryTabPage.Controls.Add(this.historyTreeView);
            this.HistoryTabPage.Location = new System.Drawing.Point(4, 22);
            this.HistoryTabPage.Name = "HistoryTabPage";
            this.HistoryTabPage.Padding = new System.Windows.Forms.Padding(3);
            this.HistoryTabPage.Size = new System.Drawing.Size(142, 124);
            this.HistoryTabPage.TabIndex = 1;
            this.HistoryTabPage.Text = "History";
            this.HistoryTabPage.UseVisualStyleBackColor = true;
            // 
            // historyTreeView
            // 
            this.historyTreeView.Dock = System.Windows.Forms.DockStyle.Fill;
            this.historyTreeView.HotTracking = true;
            this.historyTreeView.ImageIndex = 0;
            this.historyTreeView.Location = new System.Drawing.Point(3, 3);
            this.historyTreeView.Name = "historyTreeView";
            this.historyTreeView.SelectedImageIndex = 0;
            this.historyTreeView.ShowNodeToolTips = true;
            this.historyTreeView.Size = new System.Drawing.Size(136, 118);
            this.historyTreeView.TabIndex = 0;
            this.historyTreeView.KeyUp += new System.Windows.Forms.KeyEventHandler(this.HistoryTreeView_KeyUp);
            // 
            // groupsContextMenu
            // 
            this.groupsContextMenu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.createFavoriteToolStripMenuItem,
            this.connectToAllToolStripMenuItem,
            this.toolStripSeparator1,
            this.setCredentialByTagToolStripMenuItem,
            this.setUsernameByTagToolStripMenuItem,
            this.setDomainByTagToolStripMenuItem,
            this.setPasswordByTagToolStripMenuItem,
            this.deleteAllFavoritesByTagToolStripMenuItem});
            this.groupsContextMenu.Name = "contextMenuStrip2";
            this.groupsContextMenu.Size = new System.Drawing.Size(235, 186);
            // 
            // createFavoriteToolStripMenuItem
            // 
            this.createFavoriteToolStripMenuItem.Image = global::Terminals.Properties.Resources.add;
            this.createFavoriteToolStripMenuItem.Name = "createFavoriteToolStripMenuItem";
            this.createFavoriteToolStripMenuItem.Size = new System.Drawing.Size(234, 22);
            this.createFavoriteToolStripMenuItem.Text = "New Connection";
            this.createFavoriteToolStripMenuItem.Click += new System.EventHandler(this.CreateFavoriteToolStripMenuItem_Click);
            // 
            // connectToAllToolStripMenuItem
            // 
            this.connectToAllToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.consoleAllToolStripMenuItem,
            this.newWindowAllToolStripMenuItem});
            this.connectToAllToolStripMenuItem.Image = global::Terminals.Properties.Resources.application_lightning;
            this.connectToAllToolStripMenuItem.Name = "connectToAllToolStripMenuItem";
            this.connectToAllToolStripMenuItem.Size = new System.Drawing.Size(234, 22);
            this.connectToAllToolStripMenuItem.Text = "Connect to All";
            this.connectToAllToolStripMenuItem.Click += new System.EventHandler(this.ConnectToAllToolStripMenuItem_Click);
            // 
            // consoleAllToolStripMenuItem
            // 
            this.consoleAllToolStripMenuItem.CheckOnClick = true;
            this.consoleAllToolStripMenuItem.Name = "consoleAllToolStripMenuItem";
            this.consoleAllToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
            this.consoleAllToolStripMenuItem.Text = "Console";
            this.consoleAllToolStripMenuItem.Click += new System.EventHandler(this.DisplayAllWindow_Click);
            // 
            // newWindowAllToolStripMenuItem
            // 
            this.newWindowAllToolStripMenuItem.CheckOnClick = true;
            this.newWindowAllToolStripMenuItem.Name = "newWindowAllToolStripMenuItem";
            this.newWindowAllToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
            this.newWindowAllToolStripMenuItem.Text = "New Window";
            this.newWindowAllToolStripMenuItem.Click += new System.EventHandler(this.DisplayAllWindow_Click);
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
            // FavsList
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.tabControl1);
            this.Name = "FavsList";
            this.Load += new System.EventHandler(this.FavsList_Load);
            this.favoritesContextMenu.ResumeLayout(false);
            this.tabControl1.ResumeLayout(false);
            this.FavoritesTabPage.ResumeLayout(false);
            this.HistoryTabPage.ResumeLayout(false);
            this.groupsContextMenu.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private Terminals.Forms.Controls.FavoritesTreeView favsTree;
        private System.Windows.Forms.ContextMenuStrip favoritesContextMenu;
        private System.Windows.Forms.ToolStripMenuItem connectToolStripMenuItem;
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
        private System.Windows.Forms.ToolStripMenuItem consoleToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem newWindowToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem computerManagementMMCToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripMenuItem4;
        private System.Windows.Forms.ToolStripMenuItem systemInformationToolStripMenuItem;
        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage FavoritesTabPage;
        private System.Windows.Forms.TabPage HistoryTabPage;
        private Terminals.Forms.Controls.HistoryTreeView historyTreeView;
        private System.Windows.Forms.ToolStripMenuItem removeSelectedToolStripMenuItem;
        private System.Windows.Forms.ContextMenuStrip groupsContextMenu;
        private System.Windows.Forms.ToolStripMenuItem connectToAllToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem consoleAllToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem newWindowAllToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripMenuItem setCredentialByTagToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem setUsernameByTagToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem setDomainByTagToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem setPasswordByTagToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem deleteAllFavoritesByTagToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem connectAsToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem userConnectToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
        private System.Windows.Forms.ToolStripMenuItem shutdownToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem createFavoriteToolStripMenuItem;
    }
}
