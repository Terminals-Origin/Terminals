namespace Terminals.Forms
{
    partial class OptionDialog
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
            System.Windows.Forms.TreeNode treeNode1 = new System.Windows.Forms.TreeNode("Startup & Shutdown");
            System.Windows.Forms.TreeNode treeNode2 = new System.Windows.Forms.TreeNode("Favorites");
            System.Windows.Forms.TreeNode treeNode3 = new System.Windows.Forms.TreeNode("Interface", new System.Windows.Forms.TreeNode[] {
            treeNode2});
            System.Windows.Forms.TreeNode treeNode4 = new System.Windows.Forms.TreeNode("Master Password");
            System.Windows.Forms.TreeNode treeNode5 = new System.Windows.Forms.TreeNode("Default Password");
            System.Windows.Forms.TreeNode treeNode6 = new System.Windows.Forms.TreeNode("Amazon");
            System.Windows.Forms.TreeNode treeNode7 = new System.Windows.Forms.TreeNode("Security", new System.Windows.Forms.TreeNode[] {
            treeNode4,
            treeNode5,
            treeNode6});
            System.Windows.Forms.TreeNode treeNode8 = new System.Windows.Forms.TreeNode("Execute Before Connect");
            System.Windows.Forms.TreeNode treeNode9 = new System.Windows.Forms.TreeNode("Proxy");
            System.Windows.Forms.TreeNode treeNode10 = new System.Windows.Forms.TreeNode("Connections", new System.Windows.Forms.TreeNode[] {
            treeNode8,
            treeNode9});
            System.Windows.Forms.TreeNode treeNode11 = new System.Windows.Forms.TreeNode("Flickr");
            System.Windows.Forms.TreeNode treeNode12 = new System.Windows.Forms.TreeNode("Screen Capture", new System.Windows.Forms.TreeNode[] {
            treeNode11});
            System.Windows.Forms.TreeNode treeNode13 = new System.Windows.Forms.TreeNode("Data store");
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(OptionDialog));
            this.btnOk = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.linkLabel1 = new System.Windows.Forms.LinkLabel();
            this.OptionsTreeView = new System.Windows.Forms.TreeView();
            this.tabCtrlOptionPanels = new System.Windows.Forms.TabControl();
            this.tabPageStartupShutdown = new System.Windows.Forms.TabPage();
            this.panelStartupShutdown = new Terminals.Forms.StartShutdownOptionPanel();
            this.tabPageInterface = new System.Windows.Forms.TabPage();
            this.panelInterface = new Terminals.Forms.InterfaceOptionPanel();
            this.tabPageFavorites = new System.Windows.Forms.TabPage();
            this.panelFavorites = new Terminals.Forms.FavoritesOptionPanel();
            this.tabPageMasterPwd = new System.Windows.Forms.TabPage();
            this.panelMasterPassword = new Terminals.Forms.MasterPasswordOptionPanel();
            this.tabPageDefaultPwd = new System.Windows.Forms.TabPage();
            this.panelDefaultPassword = new Terminals.Forms.DefaultPasswordOptionPanel();
            this.tabPageAmazon = new System.Windows.Forms.TabPage();
            this.panelAmazon = new Terminals.Forms.AmazonOptionPanel();
            this.tabPageConnections = new System.Windows.Forms.TabPage();
            this.panelConnections = new Terminals.Forms.ConnectionsOptionPanel();
            this.tabPageBeforeConnect = new System.Windows.Forms.TabPage();
            this.panelExecuteBeforeConnect = new Terminals.Forms.ConnectCommandOptionPanel();
            this.tabPageProxy = new System.Windows.Forms.TabPage();
            this.panelProxy = new Terminals.Forms.ProxyOptionPanel();
            this.tabPageScreenCapture = new System.Windows.Forms.TabPage();
            this.panelScreenCapture = new Terminals.Forms.CaptureOptionPanel();
            this.tabPageFlickr = new System.Windows.Forms.TabPage();
            this.panelFlickr = new Terminals.Forms.FlickrOptionPanel();
            this.tabPagePersistence = new System.Windows.Forms.TabPage();
            this.panelPersistence = new Terminals.Forms.PersistenceOptionPanel();
            this.OptionTitelLabel = new System.Windows.Forms.Label();
            this.tabCtrlOptionPanels.SuspendLayout();
            this.tabPageStartupShutdown.SuspendLayout();
            this.tabPageInterface.SuspendLayout();
            this.tabPageFavorites.SuspendLayout();
            this.tabPageMasterPwd.SuspendLayout();
            this.tabPageDefaultPwd.SuspendLayout();
            this.tabPageAmazon.SuspendLayout();
            this.tabPageConnections.SuspendLayout();
            this.tabPageBeforeConnect.SuspendLayout();
            this.tabPageProxy.SuspendLayout();
            this.tabPageScreenCapture.SuspendLayout();
            this.tabPageFlickr.SuspendLayout();
            this.tabPagePersistence.SuspendLayout();
            this.SuspendLayout();
            // 
            // btnOk
            // 
            this.btnOk.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnOk.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.btnOk.Location = new System.Drawing.Point(656, 424);
            this.btnOk.Name = "btnOk";
            this.btnOk.Size = new System.Drawing.Size(88, 26);
            this.btnOk.TabIndex = 1;
            this.btnOk.Text = "OK";
            this.btnOk.UseVisualStyleBackColor = true;
            this.btnOk.Click += new System.EventHandler(this.btnOk_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.Location = new System.Drawing.Point(750, 424);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(88, 26);
            this.btnCancel.TabIndex = 2;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            // 
            // label1
            // 
            this.label1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.label1.Location = new System.Drawing.Point(12, 380);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(470, 49);
            this.label1.TabIndex = 3;
            this.label1.Text = "More options coming soon to a version near you.\r\nHave a suggestion? submit a feat" +
    "ure request \r\nthrough the Terminals website:\r\n";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // linkLabel1
            // 
            this.linkLabel1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.linkLabel1.Location = new System.Drawing.Point(12, 429);
            this.linkLabel1.Name = "linkLabel1";
            this.linkLabel1.Size = new System.Drawing.Size(470, 20);
            this.linkLabel1.TabIndex = 4;
            this.linkLabel1.TabStop = true;
            this.linkLabel1.Text = "http://www.codeplex.com/Terminals";
            this.linkLabel1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // OptionsTreeView
            // 
            this.OptionsTreeView.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.OptionsTreeView.HotTracking = true;
            this.OptionsTreeView.Location = new System.Drawing.Point(8, 8);
            this.OptionsTreeView.Name = "OptionsTreeView";
            treeNode1.Name = "Startup & Shutdown";
            treeNode1.Tag = "StartupShutdown";
            treeNode1.Text = "Startup & Shutdown";
            treeNode2.Name = "Favorites";
            treeNode2.Tag = "Favorites";
            treeNode2.Text = "Favorites";
            treeNode3.Name = "Interface";
            treeNode3.Tag = "Interface";
            treeNode3.Text = "Interface";
            treeNode4.Name = "Master Password";
            treeNode4.Tag = "MasterPassword";
            treeNode4.Text = "Master Password";
            treeNode5.Name = "Default Password";
            treeNode5.Tag = "DefaultPassword";
            treeNode5.Text = "Default Password";
            treeNode6.Name = "Amazon";
            treeNode6.Tag = "Amazon";
            treeNode6.Text = "Amazon";
            treeNode7.Name = "Master Password";
            treeNode7.Tag = "MasterPassword";
            treeNode7.Text = "Security";
            treeNode8.Name = "Execute Before Connect";
            treeNode8.Tag = "ExecuteBeforeConnect";
            treeNode8.Text = "Execute Before Connect";
            treeNode9.Name = "Proxy";
            treeNode9.Tag = "Proxy";
            treeNode9.Text = "Proxy";
            treeNode10.Name = "Connections";
            treeNode10.Tag = "Connections";
            treeNode10.Text = "Connections";
            treeNode11.Name = "Flickr";
            treeNode11.Tag = "Flickr";
            treeNode11.Text = "Flickr";
            treeNode12.Name = "Screen Capture";
            treeNode12.Tag = "ScreenCapture";
            treeNode12.Text = "Screen Capture";
            treeNode13.Name = "Data store";
            treeNode13.Tag = "Persistence";
            treeNode13.Text = "Data store";
            this.OptionsTreeView.Nodes.AddRange(new System.Windows.Forms.TreeNode[] {
            treeNode1,
            treeNode3,
            treeNode7,
            treeNode10,
            treeNode12,
            treeNode13});
            this.OptionsTreeView.ShowLines = false;
            this.OptionsTreeView.Size = new System.Drawing.Size(192, 365);
            this.OptionsTreeView.TabIndex = 6;
            this.OptionsTreeView.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(this.OptionsTreeView_AfterSelect);
            // 
            // tabCtrlOptionPanels
            // 
            this.tabCtrlOptionPanels.Alignment = System.Windows.Forms.TabAlignment.Right;
            this.tabCtrlOptionPanels.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tabCtrlOptionPanels.Controls.Add(this.tabPageStartupShutdown);
            this.tabCtrlOptionPanels.Controls.Add(this.tabPageInterface);
            this.tabCtrlOptionPanels.Controls.Add(this.tabPageFavorites);
            this.tabCtrlOptionPanels.Controls.Add(this.tabPageMasterPwd);
            this.tabCtrlOptionPanels.Controls.Add(this.tabPageDefaultPwd);
            this.tabCtrlOptionPanels.Controls.Add(this.tabPageAmazon);
            this.tabCtrlOptionPanels.Controls.Add(this.tabPageConnections);
            this.tabCtrlOptionPanels.Controls.Add(this.tabPageBeforeConnect);
            this.tabCtrlOptionPanels.Controls.Add(this.tabPageProxy);
            this.tabCtrlOptionPanels.Controls.Add(this.tabPageScreenCapture);
            this.tabCtrlOptionPanels.Controls.Add(this.tabPageFlickr);
            this.tabCtrlOptionPanels.Controls.Add(this.tabPagePersistence);
            this.tabCtrlOptionPanels.ItemSize = new System.Drawing.Size(20, 20);
            this.tabCtrlOptionPanels.Location = new System.Drawing.Point(198, 8);
            this.tabCtrlOptionPanels.Multiline = true;
            this.tabCtrlOptionPanels.Name = "tabCtrlOptionPanels";
            this.tabCtrlOptionPanels.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.tabCtrlOptionPanels.SelectedIndex = 0;
            this.tabCtrlOptionPanels.Size = new System.Drawing.Size(650, 365);
            this.tabCtrlOptionPanels.TabIndex = 7;
            // 
            // tabPageStartupShutdown
            // 
            this.tabPageStartupShutdown.AutoScroll = true;
            this.tabPageStartupShutdown.Controls.Add(this.panelStartupShutdown);
            this.tabPageStartupShutdown.Location = new System.Drawing.Point(4, 4);
            this.tabPageStartupShutdown.Name = "tabPageStartupShutdown";
            this.tabPageStartupShutdown.Padding = new System.Windows.Forms.Padding(3);
            this.tabPageStartupShutdown.Size = new System.Drawing.Size(602, 357);
            this.tabPageStartupShutdown.TabIndex = 0;
            this.tabPageStartupShutdown.Text = "Startup";
            this.tabPageStartupShutdown.UseVisualStyleBackColor = true;
            // 
            // panelStartupShutdown
            // 
            this.panelStartupShutdown.Location = new System.Drawing.Point(6, 26);
            this.panelStartupShutdown.Name = "panelStartupShutdown";
            this.panelStartupShutdown.Size = new System.Drawing.Size(512, 328);
            this.panelStartupShutdown.TabIndex = 0;
            // 
            // tabPageInterface
            // 
            this.tabPageInterface.AutoScroll = true;
            this.tabPageInterface.Controls.Add(this.panelInterface);
            this.tabPageInterface.Location = new System.Drawing.Point(4, 4);
            this.tabPageInterface.Name = "tabPageInterface";
            this.tabPageInterface.Padding = new System.Windows.Forms.Padding(3);
            this.tabPageInterface.Size = new System.Drawing.Size(602, 357);
            this.tabPageInterface.TabIndex = 1;
            this.tabPageInterface.Text = "Interface";
            this.tabPageInterface.UseVisualStyleBackColor = true;
            // 
            // panelInterface
            // 
            this.panelInterface.Location = new System.Drawing.Point(8, 27);
            this.panelInterface.Name = "panelInterface";
            this.panelInterface.Size = new System.Drawing.Size(519, 325);
            this.panelInterface.TabIndex = 0;
            // 
            // tabPageFavorites
            // 
            this.tabPageFavorites.AutoScroll = true;
            this.tabPageFavorites.Controls.Add(this.panelFavorites);
            this.tabPageFavorites.Location = new System.Drawing.Point(4, 4);
            this.tabPageFavorites.Name = "tabPageFavorites";
            this.tabPageFavorites.Size = new System.Drawing.Size(602, 357);
            this.tabPageFavorites.TabIndex = 10;
            this.tabPageFavorites.Text = "Favorites";
            this.tabPageFavorites.UseVisualStyleBackColor = true;
            // 
            // panelFavorites
            // 
            this.panelFavorites.Location = new System.Drawing.Point(4, 26);
            this.panelFavorites.Name = "panelFavorites";
            this.panelFavorites.Size = new System.Drawing.Size(519, 328);
            this.panelFavorites.TabIndex = 0;
            // 
            // tabPageMasterPwd
            // 
            this.tabPageMasterPwd.AutoScroll = true;
            this.tabPageMasterPwd.Controls.Add(this.panelMasterPassword);
            this.tabPageMasterPwd.Location = new System.Drawing.Point(4, 4);
            this.tabPageMasterPwd.Name = "tabPageMasterPwd";
            this.tabPageMasterPwd.Size = new System.Drawing.Size(602, 357);
            this.tabPageMasterPwd.TabIndex = 2;
            this.tabPageMasterPwd.Text = "Master Pwd";
            this.tabPageMasterPwd.UseVisualStyleBackColor = true;
            // 
            // panelMasterPassword
            // 
            this.panelMasterPassword.Location = new System.Drawing.Point(8, 27);
            this.panelMasterPassword.Name = "panelMasterPassword";
            this.panelMasterPassword.Size = new System.Drawing.Size(511, 327);
            this.panelMasterPassword.TabIndex = 0;
            // 
            // tabPageDefaultPwd
            // 
            this.tabPageDefaultPwd.AutoScroll = true;
            this.tabPageDefaultPwd.Controls.Add(this.panelDefaultPassword);
            this.tabPageDefaultPwd.Location = new System.Drawing.Point(4, 4);
            this.tabPageDefaultPwd.Name = "tabPageDefaultPwd";
            this.tabPageDefaultPwd.Size = new System.Drawing.Size(602, 357);
            this.tabPageDefaultPwd.TabIndex = 4;
            this.tabPageDefaultPwd.Text = "Default Pwd";
            this.tabPageDefaultPwd.UseVisualStyleBackColor = true;
            // 
            // panelDefaultPassword
            // 
            this.panelDefaultPassword.Location = new System.Drawing.Point(4, 29);
            this.panelDefaultPassword.Name = "panelDefaultPassword";
            this.panelDefaultPassword.Size = new System.Drawing.Size(513, 325);
            this.panelDefaultPassword.TabIndex = 0;
            // 
            // tabPageAmazon
            // 
            this.tabPageAmazon.AutoScroll = true;
            this.tabPageAmazon.Controls.Add(this.panelAmazon);
            this.tabPageAmazon.Location = new System.Drawing.Point(4, 4);
            this.tabPageAmazon.Name = "tabPageAmazon";
            this.tabPageAmazon.Size = new System.Drawing.Size(602, 357);
            this.tabPageAmazon.TabIndex = 5;
            this.tabPageAmazon.Text = "Amazon";
            this.tabPageAmazon.UseVisualStyleBackColor = true;
            // 
            // panelAmazon
            // 
            this.panelAmazon.Location = new System.Drawing.Point(4, 26);
            this.panelAmazon.Name = "panelAmazon";
            this.panelAmazon.Size = new System.Drawing.Size(513, 328);
            this.panelAmazon.TabIndex = 0;
            // 
            // tabPageConnections
            // 
            this.tabPageConnections.AutoScroll = true;
            this.tabPageConnections.Controls.Add(this.panelConnections);
            this.tabPageConnections.Location = new System.Drawing.Point(4, 4);
            this.tabPageConnections.Name = "tabPageConnections";
            this.tabPageConnections.Size = new System.Drawing.Size(602, 357);
            this.tabPageConnections.TabIndex = 3;
            this.tabPageConnections.Text = "Connections";
            this.tabPageConnections.UseVisualStyleBackColor = true;
            // 
            // panelConnections
            // 
            this.panelConnections.Location = new System.Drawing.Point(4, 22);
            this.panelConnections.Name = "panelConnections";
            this.panelConnections.Size = new System.Drawing.Size(514, 332);
            this.panelConnections.TabIndex = 0;
            // 
            // tabPageBeforeConnect
            // 
            this.tabPageBeforeConnect.AutoScroll = true;
            this.tabPageBeforeConnect.Controls.Add(this.panelExecuteBeforeConnect);
            this.tabPageBeforeConnect.Location = new System.Drawing.Point(4, 4);
            this.tabPageBeforeConnect.Name = "tabPageBeforeConnect";
            this.tabPageBeforeConnect.Size = new System.Drawing.Size(602, 357);
            this.tabPageBeforeConnect.TabIndex = 6;
            this.tabPageBeforeConnect.Text = "Before Connect";
            this.tabPageBeforeConnect.UseVisualStyleBackColor = true;
            // 
            // panelExecuteBeforeConnect
            // 
            this.panelExecuteBeforeConnect.Location = new System.Drawing.Point(3, 26);
            this.panelExecuteBeforeConnect.Name = "panelExecuteBeforeConnect";
            this.panelExecuteBeforeConnect.Size = new System.Drawing.Size(513, 327);
            this.panelExecuteBeforeConnect.TabIndex = 0;
            // 
            // tabPageProxy
            // 
            this.tabPageProxy.AutoScroll = true;
            this.tabPageProxy.Controls.Add(this.panelProxy);
            this.tabPageProxy.Location = new System.Drawing.Point(4, 4);
            this.tabPageProxy.Name = "tabPageProxy";
            this.tabPageProxy.Size = new System.Drawing.Size(602, 357);
            this.tabPageProxy.TabIndex = 7;
            this.tabPageProxy.Text = "Proxy";
            this.tabPageProxy.UseVisualStyleBackColor = true;
            // 
            // panelProxy
            // 
            this.panelProxy.Location = new System.Drawing.Point(3, 22);
            this.panelProxy.Name = "panelProxy";
            this.panelProxy.Size = new System.Drawing.Size(514, 332);
            this.panelProxy.TabIndex = 0;
            // 
            // tabPageScreenCapture
            // 
            this.tabPageScreenCapture.AutoScroll = true;
            this.tabPageScreenCapture.Controls.Add(this.panelScreenCapture);
            this.tabPageScreenCapture.Location = new System.Drawing.Point(4, 4);
            this.tabPageScreenCapture.Name = "tabPageScreenCapture";
            this.tabPageScreenCapture.Size = new System.Drawing.Size(602, 357);
            this.tabPageScreenCapture.TabIndex = 8;
            this.tabPageScreenCapture.Text = "Capture";
            this.tabPageScreenCapture.UseVisualStyleBackColor = true;
            // 
            // panelScreenCapture
            // 
            this.panelScreenCapture.Location = new System.Drawing.Point(4, 24);
            this.panelScreenCapture.Name = "panelScreenCapture";
            this.panelScreenCapture.Size = new System.Drawing.Size(513, 330);
            this.panelScreenCapture.TabIndex = 0;
            // 
            // tabPageFlickr
            // 
            this.tabPageFlickr.AutoScroll = true;
            this.tabPageFlickr.Controls.Add(this.panelFlickr);
            this.tabPageFlickr.Location = new System.Drawing.Point(4, 4);
            this.tabPageFlickr.Name = "tabPageFlickr";
            this.tabPageFlickr.Size = new System.Drawing.Size(602, 357);
            this.tabPageFlickr.TabIndex = 9;
            this.tabPageFlickr.Text = "Flickr";
            this.tabPageFlickr.UseVisualStyleBackColor = true;
            // 
            // panelFlickr
            // 
            this.panelFlickr.Location = new System.Drawing.Point(5, 27);
            this.panelFlickr.Name = "panelFlickr";
            this.panelFlickr.Size = new System.Drawing.Size(514, 328);
            this.panelFlickr.TabIndex = 0;
            // 
            // tabPagePersistence
            // 
            this.tabPagePersistence.Controls.Add(this.panelPersistence);
            this.tabPagePersistence.Location = new System.Drawing.Point(4, 4);
            this.tabPagePersistence.Name = "tabPagePersistence";
            this.tabPagePersistence.Padding = new System.Windows.Forms.Padding(3);
            this.tabPagePersistence.Size = new System.Drawing.Size(602, 357);
            this.tabPagePersistence.TabIndex = 11;
            this.tabPagePersistence.Text = "Data store";
            this.tabPagePersistence.UseVisualStyleBackColor = true;
            // 
            // panelPersistence
            // 
            this.panelPersistence.Location = new System.Drawing.Point(8, 26);
            this.panelPersistence.Name = "panelPersistence";
            this.panelPersistence.Size = new System.Drawing.Size(512, 328);
            this.panelPersistence.TabIndex = 0;
            // 
            // OptionTitelLabel
            // 
            this.OptionTitelLabel.BackColor = System.Drawing.SystemColors.ActiveCaption;
            this.OptionTitelLabel.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.OptionTitelLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.OptionTitelLabel.ForeColor = System.Drawing.Color.White;
            this.OptionTitelLabel.Location = new System.Drawing.Point(210, 8);
            this.OptionTitelLabel.Name = "OptionTitelLabel";
            this.OptionTitelLabel.Size = new System.Drawing.Size(509, 27);
            this.OptionTitelLabel.TabIndex = 8;
            this.OptionTitelLabel.Text = "Option Title";
            // 
            // OptionDialog
            // 
            this.AcceptButton = this.btnOk;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.btnCancel;
            this.ClientSize = new System.Drawing.Size(850, 462);
            this.Controls.Add(this.OptionTitelLabel);
            this.Controls.Add(this.tabCtrlOptionPanels);
            this.Controls.Add(this.OptionsTreeView);
            this.Controls.Add(this.linkLabel1);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.btnOk);
            this.Controls.Add(this.btnCancel);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "OptionDialog";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Options";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.OptionDialog_FormClosed);
            this.tabCtrlOptionPanels.ResumeLayout(false);
            this.tabPageStartupShutdown.ResumeLayout(false);
            this.tabPageInterface.ResumeLayout(false);
            this.tabPageFavorites.ResumeLayout(false);
            this.tabPageMasterPwd.ResumeLayout(false);
            this.tabPageDefaultPwd.ResumeLayout(false);
            this.tabPageAmazon.ResumeLayout(false);
            this.tabPageConnections.ResumeLayout(false);
            this.tabPageBeforeConnect.ResumeLayout(false);
            this.tabPageProxy.ResumeLayout(false);
            this.tabPageScreenCapture.ResumeLayout(false);
            this.tabPageFlickr.ResumeLayout(false);
            this.tabPagePersistence.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button btnOk;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.LinkLabel linkLabel1;
        private System.Windows.Forms.TreeView OptionsTreeView;
        private System.Windows.Forms.TabControl tabCtrlOptionPanels;
        private System.Windows.Forms.TabPage tabPageStartupShutdown;
        private System.Windows.Forms.TabPage tabPageInterface;
        private System.Windows.Forms.TabPage tabPageFavorites;
        private System.Windows.Forms.TabPage tabPageMasterPwd;
        private System.Windows.Forms.TabPage tabPageDefaultPwd;
        private System.Windows.Forms.TabPage tabPageAmazon;
        private System.Windows.Forms.TabPage tabPageConnections;
        private System.Windows.Forms.TabPage tabPageBeforeConnect;
        private System.Windows.Forms.TabPage tabPageProxy;
        private System.Windows.Forms.TabPage tabPageScreenCapture;
        private System.Windows.Forms.TabPage tabPageFlickr;
        private System.Windows.Forms.Label OptionTitelLabel;
        private StartShutdownOptionPanel panelStartupShutdown;
        private InterfaceOptionPanel panelInterface;
        private FavoritesOptionPanel panelFavorites;
        private MasterPasswordOptionPanel panelMasterPassword;
        private DefaultPasswordOptionPanel panelDefaultPassword;
        private AmazonOptionPanel panelAmazon;
        private ConnectionsOptionPanel panelConnections;
        private ConnectCommandOptionPanel panelExecuteBeforeConnect;
        private ProxyOptionPanel panelProxy;
        private CaptureOptionPanel panelScreenCapture;
        private FlickrOptionPanel panelFlickr;
        private System.Windows.Forms.TabPage tabPagePersistence;
        private PersistenceOptionPanel panelPersistence;
    }
}
