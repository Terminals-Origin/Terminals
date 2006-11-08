namespace Terminals
{
    partial class MainForm
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
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
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
            this.tcTerminals = new TabControl.TabControl();
            this.toolStripContainer = new System.Windows.Forms.ToolStripContainer();
            this.timerHover = new System.Windows.Forms.Timer(this.components);
            this.menuStrip.SuspendLayout();
            this.toolbarStd.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.tcTerminals)).BeginInit();
            this.toolStripContainer.ContentPanel.SuspendLayout();
            this.toolStripContainer.TopToolStripPanel.SuspendLayout();
            this.toolStripContainer.SuspendLayout();
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
            this.groupsToolStripMenuItem});
            this.menuStrip.Location = new System.Drawing.Point(3, 0);
            this.menuStrip.Name = "menuStrip";
            this.menuStrip.Size = new System.Drawing.Size(260, 24);
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
            this.newTerminalToolStripMenuItem.Size = new System.Drawing.Size(203, 22);
            this.newTerminalToolStripMenuItem.Text = "&New Connection...";
            this.newTerminalToolStripMenuItem.Click += new System.EventHandler(this.newTerminalToolStripMenuItem_Click_1);
            // 
            // toolStripMenuItem1
            // 
            this.toolStripMenuItem1.Name = "toolStripMenuItem1";
            this.toolStripMenuItem1.Size = new System.Drawing.Size(200, 6);
            // 
            // exitToolStripMenuItem
            // 
            this.exitToolStripMenuItem.Name = "exitToolStripMenuItem";
            this.exitToolStripMenuItem.Size = new System.Drawing.Size(203, 22);
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
            this.fullScreenToolStripMenuItem.Size = new System.Drawing.Size(151, 22);
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
            this.manageFavoritesToolStripMenuItem.Size = new System.Drawing.Size(216, 22);
            this.manageFavoritesToolStripMenuItem.Text = "&Organize Favorites...";
            this.manageFavoritesToolStripMenuItem.Click += new System.EventHandler(this.manageConnectionsToolStripMenuItem_Click);
            // 
            // organizeFavoritesToolbarToolStripMenuItem
            // 
            this.organizeFavoritesToolbarToolStripMenuItem.Name = "organizeFavoritesToolbarToolStripMenuItem";
            this.organizeFavoritesToolbarToolStripMenuItem.Size = new System.Drawing.Size(216, 22);
            this.organizeFavoritesToolbarToolStripMenuItem.Text = "Organize Favorites Toolbar...";
            this.organizeFavoritesToolbarToolStripMenuItem.Click += new System.EventHandler(this.organizeFavoritesToolbarToolStripMenuItem_Click);
            // 
            // favoritesSeparator
            // 
            this.favoritesSeparator.Name = "favoritesSeparator";
            this.favoritesSeparator.Size = new System.Drawing.Size(213, 6);
            // 
            // terminalToolStripMenuItem
            // 
            this.terminalToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.grabInputToolStripMenuItem,
            this.toolStripSeparator5,
            this.disconnectToolStripMenuItem});
            this.terminalToolStripMenuItem.Name = "terminalToolStripMenuItem";
            this.terminalToolStripMenuItem.Size = new System.Drawing.Size(59, 20);
            this.terminalToolStripMenuItem.Text = "&Terminal";
            // 
            // grabInputToolStripMenuItem
            // 
            this.grabInputToolStripMenuItem.Enabled = false;
            this.grabInputToolStripMenuItem.Image = global::Terminals.Properties.Resources.arrow_out;
            this.grabInputToolStripMenuItem.Name = "grabInputToolStripMenuItem";
            this.grabInputToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.G)));
            this.grabInputToolStripMenuItem.Size = new System.Drawing.Size(165, 22);
            this.grabInputToolStripMenuItem.Text = "&Grab Input";
            this.grabInputToolStripMenuItem.Click += new System.EventHandler(this.tsbGrabInput_Click);
            // 
            // toolStripSeparator5
            // 
            this.toolStripSeparator5.Name = "toolStripSeparator5";
            this.toolStripSeparator5.Size = new System.Drawing.Size(162, 6);
            // 
            // disconnectToolStripMenuItem
            // 
            this.disconnectToolStripMenuItem.Enabled = false;
            this.disconnectToolStripMenuItem.Name = "disconnectToolStripMenuItem";
            this.disconnectToolStripMenuItem.Size = new System.Drawing.Size(165, 22);
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
            this.organizeGroupsToolStripMenuItem.Size = new System.Drawing.Size(261, 22);
            this.organizeGroupsToolStripMenuItem.Text = "&Organize Groups...";
            this.organizeGroupsToolStripMenuItem.Click += new System.EventHandler(this.organizeGroupsToolStripMenuItem_Click);
            // 
            // saveTerminalsAsGroupToolStripMenuItem
            // 
            this.saveTerminalsAsGroupToolStripMenuItem.Name = "saveTerminalsAsGroupToolStripMenuItem";
            this.saveTerminalsAsGroupToolStripMenuItem.Size = new System.Drawing.Size(261, 22);
            this.saveTerminalsAsGroupToolStripMenuItem.Text = "&Create Group From Active Connections";
            this.saveTerminalsAsGroupToolStripMenuItem.Click += new System.EventHandler(this.saveTerminalsAsGroupToolStripMenuItem_Click);
            // 
            // addTerminalToGroupToolStripMenuItem
            // 
            this.addTerminalToGroupToolStripMenuItem.Name = "addTerminalToGroupToolStripMenuItem";
            this.addTerminalToGroupToolStripMenuItem.Size = new System.Drawing.Size(261, 22);
            this.addTerminalToGroupToolStripMenuItem.Text = "&Add Current Connection To ";
            // 
            // groupsSeparator
            // 
            this.groupsSeparator.Name = "groupsSeparator";
            this.groupsSeparator.Size = new System.Drawing.Size(258, 6);
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
            this.toolStripSeparator6});
            this.toolbarStd.Location = new System.Drawing.Point(3, 24);
            this.toolbarStd.Name = "toolbarStd";
            this.toolbarStd.Size = new System.Drawing.Size(361, 25);
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
            this.tscConnectTo.Size = new System.Drawing.Size(121, 25);
            this.tscConnectTo.Sorted = true;
            this.tscConnectTo.KeyDown += new System.Windows.Forms.KeyEventHandler(this.tscConnectTo_KeyDown);
            this.tscConnectTo.TextChanged += new System.EventHandler(this.tscConnectTo_TextChanged);
            // 
            // tsbConnect
            // 
            this.tsbConnect.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.tsbConnect.Enabled = false;
            this.tsbConnect.Image = ((System.Drawing.Image)(resources.GetObject("tsbConnect.Image")));
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
            // tcTerminals
            // 
            this.tcTerminals.AlwaysShowClose = false;
            this.tcTerminals.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tcTerminals.Font = new System.Drawing.Font("Tahoma", 8.25F);
            this.tcTerminals.Location = new System.Drawing.Point(0, 0);
            this.tcTerminals.Name = "tcTerminals";
            this.tcTerminals.Size = new System.Drawing.Size(873, 673);
            this.tcTerminals.TabIndex = 3;
            this.tcTerminals.TabControlItemSelectionChanged += new TabControl.TabControlItemChangedHandler(this.tcTerminals_TabControlItemSelectionChanged);
            this.tcTerminals.DoubleClick += new System.EventHandler(this.tcTerminals_DoubleClick);
            this.tcTerminals.MouseLeave += new System.EventHandler(this.tcTerminals_MouseLeave);
            this.tcTerminals.TabControlItemClosed += new System.EventHandler(this.tcTerminals_TabControlItemClosed);
            this.tcTerminals.MenuItemsLoaded += new System.EventHandler(this.tcTerminals_MenuItemsLoaded);
            this.tcTerminals.MouseHover += new System.EventHandler(this.tcTerminals_MouseHover);
            this.tcTerminals.TabControlItemClosing += new TabControl.TabControlItemClosingHandler(this.tcTerminals_TabControlItemClosing);
            // 
            // toolStripContainer
            // 
            // 
            // toolStripContainer.ContentPanel
            // 
            this.toolStripContainer.ContentPanel.AutoScroll = true;
            this.toolStripContainer.ContentPanel.Controls.Add(this.tcTerminals);
            this.toolStripContainer.ContentPanel.Size = new System.Drawing.Size(873, 673);
            this.toolStripContainer.Dock = System.Windows.Forms.DockStyle.Fill;
            this.toolStripContainer.Location = new System.Drawing.Point(0, 0);
            this.toolStripContainer.Name = "toolStripContainer";
            this.toolStripContainer.Size = new System.Drawing.Size(873, 722);
            this.toolStripContainer.TabIndex = 0;
            this.toolStripContainer.Text = "toolStripContainer1";
            // 
            // toolStripContainer.TopToolStripPanel
            // 
            this.toolStripContainer.TopToolStripPanel.Controls.Add(this.menuStrip);
            this.toolStripContainer.TopToolStripPanel.Controls.Add(this.toolbarStd);
            // 
            // timerHover
            // 
            this.timerHover.Interval = 200;
            this.timerHover.Tick += new System.EventHandler(this.timerHover_Tick);
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(873, 722);
            this.Controls.Add(this.toolStripContainer);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.KeyPreview = true;
            this.MainMenuStrip = this.menuStrip;
            this.Name = "MainForm";
            this.Text = "Terminals";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.Shown += new System.EventHandler(this.MainForm_Shown);
            this.Activated += new System.EventHandler(this.MainForm_Activated);
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.MainForm_FormClosing);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.MainForm_KeyDown);
            this.Load += new System.EventHandler(this.MainForm_Load);
            this.menuStrip.ResumeLayout(false);
            this.menuStrip.PerformLayout();
            this.toolbarStd.ResumeLayout(false);
            this.toolbarStd.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.tcTerminals)).EndInit();
            this.toolStripContainer.ContentPanel.ResumeLayout(false);
            this.toolStripContainer.TopToolStripPanel.ResumeLayout(false);
            this.toolStripContainer.TopToolStripPanel.PerformLayout();
            this.toolStripContainer.ResumeLayout(false);
            this.toolStripContainer.PerformLayout();
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
      private System.Windows.Forms.ToolStripButton tsbGrabInput;
        private System.Windows.Forms.ToolStripButton tsbDisconnect;
        private System.Windows.Forms.ToolStripMenuItem terminalToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem grabInputToolStripMenuItem;
        private TabControl.TabControl tcTerminals;
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


      }
}

