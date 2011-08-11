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
      System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(OptionDialog));
      this.btnOk = new System.Windows.Forms.Button();
      this.btnCancel = new System.Windows.Forms.Button();
      this.label1 = new System.Windows.Forms.Label();
      this.linkLabel1 = new System.Windows.Forms.LinkLabel();
      this.PasswordProtectTerminalsCheckbox = new System.Windows.Forms.CheckBox();
      this.ClearMasterButton = new System.Windows.Forms.Button();
      this.PasswordTextbox = new System.Windows.Forms.TextBox();
      this.ConfirmPasswordTextBox = new System.Windows.Forms.TextBox();
      this.label7 = new System.Windows.Forms.Label();
      this.label6 = new System.Windows.Forms.Label();
      this.autoCaseTagsCheckbox = new System.Windows.Forms.CheckBox();
      this.MinimizeToTrayCheckbox = new System.Windows.Forms.CheckBox();
      this.chkSaveConnections = new System.Windows.Forms.CheckBox();
      this.chkShowConfirmDialog = new System.Windows.Forms.CheckBox();
      this.chkSingleInstance = new System.Windows.Forms.CheckBox();
      this.chkShowFullInfo = new System.Windows.Forms.CheckBox();
      this.chkShowUserNameInTitle = new System.Windows.Forms.CheckBox();
      this.chkShowInformationToolTips = new System.Windows.Forms.CheckBox();
      this.AutoExapandTagsPanelCheckBox = new System.Windows.Forms.CheckBox();
      this.FavSortGroupBox = new System.Windows.Forms.GroupBox();
      this.NoneRadioButton = new System.Windows.Forms.RadioButton();
      this.ProtocolRadionButton = new System.Windows.Forms.RadioButton();
      this.ConnectionNameRadioButton = new System.Windows.Forms.RadioButton();
      this.ServerNameRadio = new System.Windows.Forms.RadioButton();
      this.groupBox1 = new System.Windows.Forms.GroupBox();
      this.RenderBlackRadio = new System.Windows.Forms.RadioButton();
      this.RenderBlueRadio = new System.Windows.Forms.RadioButton();
      this.RenderNormalRadio = new System.Windows.Forms.RadioButton();
      this.EnableFavoritesPanel = new System.Windows.Forms.CheckBox();
      this.EnableGroupsMenu = new System.Windows.Forms.CheckBox();
      this.OptionsTreeView = new System.Windows.Forms.TreeView();
      this.tabControl3 = new System.Windows.Forms.TabControl();
      this.tabPageStartupShutdown = new System.Windows.Forms.TabPage();
      this.panelStartupShutdown = new System.Windows.Forms.Panel();
      this.groupBoxShutdown = new System.Windows.Forms.GroupBox();
      this.groupBoxStartup = new System.Windows.Forms.GroupBox();
      this.NeverShowTerminalsCheckbox = new System.Windows.Forms.CheckBox();
      this.tabPageInterface = new System.Windows.Forms.TabPage();
      this.panelInterface = new System.Windows.Forms.Panel();
      this.groupBox10 = new System.Windows.Forms.GroupBox();
      this.groupBox7 = new System.Windows.Forms.GroupBox();
      this.groupBoxInformation = new System.Windows.Forms.GroupBox();
      this.tabPageFavorites = new System.Windows.Forms.TabPage();
      this.panelFavorites = new System.Windows.Forms.Panel();
      this.groupBox11 = new System.Windows.Forms.GroupBox();
      this.tabPageMasterPwd = new System.Windows.Forms.TabPage();
      this.panelMasterPassword = new System.Windows.Forms.Panel();
      this.groupBox3 = new System.Windows.Forms.GroupBox();
      this.PasswordsMatchLabel = new System.Windows.Forms.Label();
      this.tabPageDefaultPwd = new System.Windows.Forms.TabPage();
      this.panelDefaultPassword = new System.Windows.Forms.Panel();
      this.groupBox2 = new System.Windows.Forms.GroupBox();
      this.label21 = new System.Windows.Forms.Label();
      this.passwordTxtBox = new System.Windows.Forms.TextBox();
      this.label18 = new System.Windows.Forms.Label();
      this.usernameTextbox = new System.Windows.Forms.TextBox();
      this.label19 = new System.Windows.Forms.Label();
      this.domainTextbox = new System.Windows.Forms.TextBox();
      this.label20 = new System.Windows.Forms.Label();
      this.tabPageAmazon = new System.Windows.Forms.TabPage();
      this.panelAmazon = new System.Windows.Forms.Panel();
      this.groupBox4 = new System.Windows.Forms.GroupBox();
      this.label22 = new System.Windows.Forms.Label();
      this.BucketNameTextBox = new System.Windows.Forms.TextBox();
      this.RestoreButton = new System.Windows.Forms.Button();
      this.BackupButton = new System.Windows.Forms.Button();
      this.ErrorLabel = new System.Windows.Forms.Label();
      this.TestButton = new System.Windows.Forms.Button();
      this.label17 = new System.Windows.Forms.Label();
      this.label16 = new System.Windows.Forms.Label();
      this.SecretKeyTextbox = new System.Windows.Forms.TextBox();
      this.AccessKeyTextbox = new System.Windows.Forms.TextBox();
      this.AmazonBackupCheckbox = new System.Windows.Forms.CheckBox();
      this.pictureBox1 = new System.Windows.Forms.PictureBox();
      this.tabPageConnections = new System.Windows.Forms.TabPage();
      this.panelConnections = new System.Windows.Forms.Panel();
      this.groupBox12 = new System.Windows.Forms.GroupBox();
      this.EvaluatedDesktopShareLabel = new System.Windows.Forms.Label();
      this.PortscanTimeoutTextBox = new System.Windows.Forms.TextBox();
      this.label2 = new System.Windows.Forms.Label();
      this.txtDefaultDesktopShare = new System.Windows.Forms.TextBox();
      this.label3 = new System.Windows.Forms.Label();
      this.label5 = new System.Windows.Forms.Label();
      this.label4 = new System.Windows.Forms.Label();
      this.groupBoxConnections = new System.Windows.Forms.GroupBox();
      this.validateServerNamesCheckbox = new System.Windows.Forms.CheckBox();
      this.warnDisconnectCheckBox = new System.Windows.Forms.CheckBox();
      this.tabPageBeforeConnect = new System.Windows.Forms.TabPage();
      this.panelExecuteBeforeConnect = new System.Windows.Forms.Panel();
      this.groupBox5 = new System.Windows.Forms.GroupBox();
      this.txtInitialDirectory = new System.Windows.Forms.TextBox();
      this.txtArguments = new System.Windows.Forms.TextBox();
      this.txtCommand = new System.Windows.Forms.TextBox();
      this.label13 = new System.Windows.Forms.Label();
      this.chkExecuteBeforeConnect = new System.Windows.Forms.CheckBox();
      this.label12 = new System.Windows.Forms.Label();
      this.chkWaitForExit = new System.Windows.Forms.CheckBox();
      this.label11 = new System.Windows.Forms.Label();
      this.tabPageProxy = new System.Windows.Forms.TabPage();
      this.panelProxy = new System.Windows.Forms.Panel();
      this.groupBox6 = new System.Windows.Forms.GroupBox();
      this.label15 = new System.Windows.Forms.Label();
      this.ProxyPortTextbox = new System.Windows.Forms.TextBox();
      this.ProxyAddressTextbox = new System.Windows.Forms.TextBox();
      this.label14 = new System.Windows.Forms.Label();
      this.AutoProxyRadioButton = new System.Windows.Forms.RadioButton();
      this.ProxyRadionButton = new System.Windows.Forms.RadioButton();
      this.tabPageScreenCapture = new System.Windows.Forms.TabPage();
      this.panelScreenCapture = new System.Windows.Forms.Panel();
      this.groupBox8 = new System.Windows.Forms.GroupBox();
      this.chkAutoSwitchToCaptureCheckbox = new System.Windows.Forms.CheckBox();
      this.chkEnableCaptureToFolder = new System.Windows.Forms.CheckBox();
      this.chkEnableCaptureToClipboard = new System.Windows.Forms.CheckBox();
      this.ButtonBrowseCaptureFolder = new System.Windows.Forms.Button();
      this.label23 = new System.Windows.Forms.Label();
      this.txtScreenCaptureFolder = new System.Windows.Forms.TextBox();
      this.tabPageFlickr = new System.Windows.Forms.TabPage();
      this.panelFlickr = new System.Windows.Forms.Panel();
      this.groupBox9 = new System.Windows.Forms.GroupBox();
      this.label25 = new System.Windows.Forms.Label();
      this.label10 = new System.Windows.Forms.Label();
      this.label9 = new System.Windows.Forms.Label();
      this.CompleteAuthButton = new System.Windows.Forms.Button();
      this.AuthorizeFlickrButton = new System.Windows.Forms.Button();
      this.label8 = new System.Windows.Forms.Label();
      this.OptionTitelLabel = new System.Windows.Forms.Label();
      this.FavSortGroupBox.SuspendLayout();
      this.groupBox1.SuspendLayout();
      this.tabControl3.SuspendLayout();
      this.tabPageStartupShutdown.SuspendLayout();
      this.panelStartupShutdown.SuspendLayout();
      this.groupBoxShutdown.SuspendLayout();
      this.groupBoxStartup.SuspendLayout();
      this.tabPageInterface.SuspendLayout();
      this.panelInterface.SuspendLayout();
      this.groupBox10.SuspendLayout();
      this.groupBox7.SuspendLayout();
      this.groupBoxInformation.SuspendLayout();
      this.tabPageFavorites.SuspendLayout();
      this.panelFavorites.SuspendLayout();
      this.groupBox11.SuspendLayout();
      this.tabPageMasterPwd.SuspendLayout();
      this.panelMasterPassword.SuspendLayout();
      this.groupBox3.SuspendLayout();
      this.tabPageDefaultPwd.SuspendLayout();
      this.panelDefaultPassword.SuspendLayout();
      this.groupBox2.SuspendLayout();
      this.tabPageAmazon.SuspendLayout();
      this.panelAmazon.SuspendLayout();
      this.groupBox4.SuspendLayout();
      ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
      this.tabPageConnections.SuspendLayout();
      this.panelConnections.SuspendLayout();
      this.groupBox12.SuspendLayout();
      this.groupBoxConnections.SuspendLayout();
      this.tabPageBeforeConnect.SuspendLayout();
      this.panelExecuteBeforeConnect.SuspendLayout();
      this.groupBox5.SuspendLayout();
      this.tabPageProxy.SuspendLayout();
      this.panelProxy.SuspendLayout();
      this.groupBox6.SuspendLayout();
      this.tabPageScreenCapture.SuspendLayout();
      this.panelScreenCapture.SuspendLayout();
      this.groupBox8.SuspendLayout();
      this.tabPageFlickr.SuspendLayout();
      this.panelFlickr.SuspendLayout();
      this.groupBox9.SuspendLayout();
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
      // PasswordProtectTerminalsCheckbox
      // 
      this.PasswordProtectTerminalsCheckbox.AutoSize = true;
      this.PasswordProtectTerminalsCheckbox.Location = new System.Drawing.Point(6, 20);
      this.PasswordProtectTerminalsCheckbox.Name = "PasswordProtectTerminalsCheckbox";
      this.PasswordProtectTerminalsCheckbox.Size = new System.Drawing.Size(109, 17);
      this.PasswordProtectTerminalsCheckbox.TabIndex = 0;
      this.PasswordProtectTerminalsCheckbox.Text = "Password Protect";
      this.PasswordProtectTerminalsCheckbox.UseVisualStyleBackColor = true;
      this.PasswordProtectTerminalsCheckbox.CheckedChanged += new System.EventHandler(this.PasswordProtectTerminalsCheckbox_CheckedChanged);
      // 
      // ClearMasterButton
      // 
      this.ClearMasterButton.Location = new System.Drawing.Point(79, 101);
      this.ClearMasterButton.Name = "ClearMasterButton";
      this.ClearMasterButton.Size = new System.Drawing.Size(138, 23);
      this.ClearMasterButton.TabIndex = 6;
      this.ClearMasterButton.Text = "Clear Master Password";
      this.ClearMasterButton.UseVisualStyleBackColor = true;
      this.ClearMasterButton.Click += new System.EventHandler(this.ClearMasterButton_Click);
      // 
      // PasswordTextbox
      // 
      this.PasswordTextbox.Enabled = false;
      this.PasswordTextbox.Location = new System.Drawing.Point(70, 42);
      this.PasswordTextbox.Name = "PasswordTextbox";
      this.PasswordTextbox.PasswordChar = '*';
      this.PasswordTextbox.Size = new System.Drawing.Size(147, 20);
      this.PasswordTextbox.TabIndex = 1;
      this.PasswordTextbox.TextChanged += new System.EventHandler(this.PasswordTextbox_TextChanged);
      // 
      // ConfirmPasswordTextBox
      // 
      this.ConfirmPasswordTextBox.Enabled = false;
      this.ConfirmPasswordTextBox.Location = new System.Drawing.Point(70, 70);
      this.ConfirmPasswordTextBox.Name = "ConfirmPasswordTextBox";
      this.ConfirmPasswordTextBox.PasswordChar = '*';
      this.ConfirmPasswordTextBox.Size = new System.Drawing.Size(147, 20);
      this.ConfirmPasswordTextBox.TabIndex = 2;
      this.ConfirmPasswordTextBox.TextChanged += new System.EventHandler(this.ConfirmPasswordTextBox_TextChanged);
      // 
      // label7
      // 
      this.label7.AutoSize = true;
      this.label7.Location = new System.Drawing.Point(6, 73);
      this.label7.Name = "label7";
      this.label7.Size = new System.Drawing.Size(45, 13);
      this.label7.TabIndex = 4;
      this.label7.Text = "Confirm:";
      // 
      // label6
      // 
      this.label6.AutoSize = true;
      this.label6.Location = new System.Drawing.Point(6, 45);
      this.label6.Name = "label6";
      this.label6.Size = new System.Drawing.Size(56, 13);
      this.label6.TabIndex = 3;
      this.label6.Text = "Password:";
      // 
      // autoCaseTagsCheckbox
      // 
      this.autoCaseTagsCheckbox.AutoSize = true;
      this.autoCaseTagsCheckbox.Location = new System.Drawing.Point(6, 66);
      this.autoCaseTagsCheckbox.Name = "autoCaseTagsCheckbox";
      this.autoCaseTagsCheckbox.Size = new System.Drawing.Size(143, 17);
      this.autoCaseTagsCheckbox.TabIndex = 23;
      this.autoCaseTagsCheckbox.Text = "Auto-Case Favorite Tags";
      this.autoCaseTagsCheckbox.UseVisualStyleBackColor = true;
      // 
      // MinimizeToTrayCheckbox
      // 
      this.MinimizeToTrayCheckbox.AutoSize = true;
      this.MinimizeToTrayCheckbox.Checked = true;
      this.MinimizeToTrayCheckbox.CheckState = System.Windows.Forms.CheckState.Checked;
      this.MinimizeToTrayCheckbox.Location = new System.Drawing.Point(6, 20);
      this.MinimizeToTrayCheckbox.Name = "MinimizeToTrayCheckbox";
      this.MinimizeToTrayCheckbox.Size = new System.Drawing.Size(106, 17);
      this.MinimizeToTrayCheckbox.TabIndex = 17;
      this.MinimizeToTrayCheckbox.Text = "Minimize To Tray";
      this.MinimizeToTrayCheckbox.UseVisualStyleBackColor = true;
      // 
      // chkSaveConnections
      // 
      this.chkSaveConnections.AutoSize = true;
      this.chkSaveConnections.Checked = true;
      this.chkSaveConnections.CheckState = System.Windows.Forms.CheckState.Checked;
      this.chkSaveConnections.Location = new System.Drawing.Point(6, 43);
      this.chkSaveConnections.Name = "chkSaveConnections";
      this.chkSaveConnections.Size = new System.Drawing.Size(155, 17);
      this.chkSaveConnections.TabIndex = 5;
      this.chkSaveConnections.Text = "Save connections on close";
      this.chkSaveConnections.UseVisualStyleBackColor = true;
      // 
      // chkShowConfirmDialog
      // 
      this.chkShowConfirmDialog.AutoSize = true;
      this.chkShowConfirmDialog.Checked = true;
      this.chkShowConfirmDialog.CheckState = System.Windows.Forms.CheckState.Checked;
      this.chkShowConfirmDialog.Location = new System.Drawing.Point(6, 20);
      this.chkShowConfirmDialog.Name = "chkShowConfirmDialog";
      this.chkShowConfirmDialog.Size = new System.Drawing.Size(172, 17);
      this.chkShowConfirmDialog.TabIndex = 4;
      this.chkShowConfirmDialog.Text = "Show close confirmation dialog";
      this.chkShowConfirmDialog.UseVisualStyleBackColor = true;
      // 
      // chkSingleInstance
      // 
      this.chkSingleInstance.AutoSize = true;
      this.chkSingleInstance.Checked = true;
      this.chkSingleInstance.CheckState = System.Windows.Forms.CheckState.Checked;
      this.chkSingleInstance.Location = new System.Drawing.Point(6, 19);
      this.chkSingleInstance.Name = "chkSingleInstance";
      this.chkSingleInstance.Size = new System.Drawing.Size(217, 17);
      this.chkSingleInstance.TabIndex = 3;
      this.chkSingleInstance.Text = "Allow a single instance of the application";
      this.chkSingleInstance.UseVisualStyleBackColor = true;
      // 
      // chkShowFullInfo
      // 
      this.chkShowFullInfo.AutoSize = true;
      this.chkShowFullInfo.Location = new System.Drawing.Point(26, 66);
      this.chkShowFullInfo.Name = "chkShowFullInfo";
      this.chkShowFullInfo.Size = new System.Drawing.Size(123, 17);
      this.chkShowFullInfo.TabIndex = 2;
      this.chkShowFullInfo.Text = "Show full information";
      this.chkShowFullInfo.UseVisualStyleBackColor = true;
      // 
      // chkShowUserNameInTitle
      // 
      this.chkShowUserNameInTitle.AutoSize = true;
      this.chkShowUserNameInTitle.Checked = true;
      this.chkShowUserNameInTitle.CheckState = System.Windows.Forms.CheckState.Checked;
      this.chkShowUserNameInTitle.Location = new System.Drawing.Point(6, 20);
      this.chkShowUserNameInTitle.Name = "chkShowUserNameInTitle";
      this.chkShowUserNameInTitle.Size = new System.Drawing.Size(156, 17);
      this.chkShowUserNameInTitle.TabIndex = 0;
      this.chkShowUserNameInTitle.Text = "Show  user name in tab title";
      this.chkShowUserNameInTitle.UseVisualStyleBackColor = true;
      // 
      // chkShowInformationToolTips
      // 
      this.chkShowInformationToolTips.AutoSize = true;
      this.chkShowInformationToolTips.Checked = true;
      this.chkShowInformationToolTips.CheckState = System.Windows.Forms.CheckState.Checked;
      this.chkShowInformationToolTips.Location = new System.Drawing.Point(6, 43);
      this.chkShowInformationToolTips.Name = "chkShowInformationToolTips";
      this.chkShowInformationToolTips.Size = new System.Drawing.Size(213, 17);
      this.chkShowInformationToolTips.TabIndex = 1;
      this.chkShowInformationToolTips.Text = "Show connection information in tool tips";
      this.chkShowInformationToolTips.UseVisualStyleBackColor = true;
      this.chkShowInformationToolTips.CheckedChanged += new System.EventHandler(this.chkShowInformationToolTips_CheckedChanged);
      // 
      // AutoExapandTagsPanelCheckBox
      // 
      this.AutoExapandTagsPanelCheckBox.AutoSize = true;
      this.AutoExapandTagsPanelCheckBox.Location = new System.Drawing.Point(6, 43);
      this.AutoExapandTagsPanelCheckBox.Name = "AutoExapandTagsPanelCheckBox";
      this.AutoExapandTagsPanelCheckBox.Size = new System.Drawing.Size(133, 17);
      this.AutoExapandTagsPanelCheckBox.TabIndex = 27;
      this.AutoExapandTagsPanelCheckBox.Text = "Auto Expand Favorites";
      this.AutoExapandTagsPanelCheckBox.UseVisualStyleBackColor = true;
      // 
      // FavSortGroupBox
      // 
      this.FavSortGroupBox.Controls.Add(this.NoneRadioButton);
      this.FavSortGroupBox.Controls.Add(this.ProtocolRadionButton);
      this.FavSortGroupBox.Controls.Add(this.ConnectionNameRadioButton);
      this.FavSortGroupBox.Controls.Add(this.ServerNameRadio);
      this.FavSortGroupBox.Location = new System.Drawing.Point(6, 97);
      this.FavSortGroupBox.Name = "FavSortGroupBox";
      this.FavSortGroupBox.Size = new System.Drawing.Size(500, 114);
      this.FavSortGroupBox.TabIndex = 26;
      this.FavSortGroupBox.TabStop = false;
      this.FavSortGroupBox.Text = "Favorites Sort";
      // 
      // NoneRadioButton
      // 
      this.NoneRadioButton.AutoSize = true;
      this.NoneRadioButton.Location = new System.Drawing.Point(7, 89);
      this.NoneRadioButton.Name = "NoneRadioButton";
      this.NoneRadioButton.Size = new System.Drawing.Size(51, 17);
      this.NoneRadioButton.TabIndex = 3;
      this.NoneRadioButton.TabStop = true;
      this.NoneRadioButton.Text = "None";
      this.NoneRadioButton.UseVisualStyleBackColor = true;
      // 
      // ProtocolRadionButton
      // 
      this.ProtocolRadionButton.AutoSize = true;
      this.ProtocolRadionButton.Location = new System.Drawing.Point(6, 66);
      this.ProtocolRadionButton.Name = "ProtocolRadionButton";
      this.ProtocolRadionButton.Size = new System.Drawing.Size(64, 17);
      this.ProtocolRadionButton.TabIndex = 2;
      this.ProtocolRadionButton.TabStop = true;
      this.ProtocolRadionButton.Text = "Protocol";
      this.ProtocolRadionButton.UseVisualStyleBackColor = true;
      // 
      // ConnectionNameRadioButton
      // 
      this.ConnectionNameRadioButton.AutoSize = true;
      this.ConnectionNameRadioButton.Location = new System.Drawing.Point(6, 43);
      this.ConnectionNameRadioButton.Name = "ConnectionNameRadioButton";
      this.ConnectionNameRadioButton.Size = new System.Drawing.Size(110, 17);
      this.ConnectionNameRadioButton.TabIndex = 1;
      this.ConnectionNameRadioButton.TabStop = true;
      this.ConnectionNameRadioButton.Text = "Connection Name";
      this.ConnectionNameRadioButton.UseVisualStyleBackColor = true;
      // 
      // ServerNameRadio
      // 
      this.ServerNameRadio.AutoSize = true;
      this.ServerNameRadio.Location = new System.Drawing.Point(7, 20);
      this.ServerNameRadio.Name = "ServerNameRadio";
      this.ServerNameRadio.Size = new System.Drawing.Size(87, 17);
      this.ServerNameRadio.TabIndex = 0;
      this.ServerNameRadio.TabStop = true;
      this.ServerNameRadio.Text = "Server Name";
      this.ServerNameRadio.UseVisualStyleBackColor = true;
      // 
      // groupBox1
      // 
      this.groupBox1.Controls.Add(this.RenderBlackRadio);
      this.groupBox1.Controls.Add(this.RenderBlueRadio);
      this.groupBox1.Controls.Add(this.RenderNormalRadio);
      this.groupBox1.Location = new System.Drawing.Point(6, 196);
      this.groupBox1.Name = "groupBox1";
      this.groupBox1.Size = new System.Drawing.Size(500, 90);
      this.groupBox1.TabIndex = 25;
      this.groupBox1.TabStop = false;
      this.groupBox1.Text = "Theme";
      // 
      // RenderBlackRadio
      // 
      this.RenderBlackRadio.AutoSize = true;
      this.RenderBlackRadio.Location = new System.Drawing.Point(6, 66);
      this.RenderBlackRadio.Name = "RenderBlackRadio";
      this.RenderBlackRadio.Size = new System.Drawing.Size(83, 17);
      this.RenderBlackRadio.TabIndex = 2;
      this.RenderBlackRadio.TabStop = true;
      this.RenderBlackRadio.Text = "Office Black";
      this.RenderBlackRadio.UseVisualStyleBackColor = true;
      // 
      // RenderBlueRadio
      // 
      this.RenderBlueRadio.AutoSize = true;
      this.RenderBlueRadio.Location = new System.Drawing.Point(6, 43);
      this.RenderBlueRadio.Name = "RenderBlueRadio";
      this.RenderBlueRadio.Size = new System.Drawing.Size(77, 17);
      this.RenderBlueRadio.TabIndex = 1;
      this.RenderBlueRadio.TabStop = true;
      this.RenderBlueRadio.Text = "Office Blue";
      this.RenderBlueRadio.UseVisualStyleBackColor = true;
      // 
      // RenderNormalRadio
      // 
      this.RenderNormalRadio.AutoSize = true;
      this.RenderNormalRadio.Location = new System.Drawing.Point(6, 20);
      this.RenderNormalRadio.Name = "RenderNormalRadio";
      this.RenderNormalRadio.Size = new System.Drawing.Size(58, 17);
      this.RenderNormalRadio.TabIndex = 0;
      this.RenderNormalRadio.TabStop = true;
      this.RenderNormalRadio.Text = "Normal";
      this.RenderNormalRadio.UseVisualStyleBackColor = true;
      // 
      // EnableFavoritesPanel
      // 
      this.EnableFavoritesPanel.AutoSize = true;
      this.EnableFavoritesPanel.Checked = true;
      this.EnableFavoritesPanel.CheckState = System.Windows.Forms.CheckState.Checked;
      this.EnableFavoritesPanel.Location = new System.Drawing.Point(6, 20);
      this.EnableFavoritesPanel.Name = "EnableFavoritesPanel";
      this.EnableFavoritesPanel.Size = new System.Drawing.Size(135, 17);
      this.EnableFavoritesPanel.TabIndex = 24;
      this.EnableFavoritesPanel.Text = "Enable Favorites Panel";
      this.EnableFavoritesPanel.UseVisualStyleBackColor = true;
      // 
      // EnableGroupsMenu
      // 
      this.EnableGroupsMenu.AutoSize = true;
      this.EnableGroupsMenu.Checked = true;
      this.EnableGroupsMenu.CheckState = System.Windows.Forms.CheckState.Checked;
      this.EnableGroupsMenu.Location = new System.Drawing.Point(6, 20);
      this.EnableGroupsMenu.Name = "EnableGroupsMenu";
      this.EnableGroupsMenu.Size = new System.Drawing.Size(126, 17);
      this.EnableGroupsMenu.TabIndex = 23;
      this.EnableGroupsMenu.Text = "Enable Groups Menu";
      this.EnableGroupsMenu.UseVisualStyleBackColor = true;
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
      this.OptionsTreeView.Nodes.AddRange(new System.Windows.Forms.TreeNode[] {
            treeNode1,
            treeNode3,
            treeNode7,
            treeNode10,
            treeNode12});
      this.OptionsTreeView.ShowLines = false;
      this.OptionsTreeView.Size = new System.Drawing.Size(192, 365);
      this.OptionsTreeView.TabIndex = 6;
      this.OptionsTreeView.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(this.OptionsTreeView_AfterSelect);
      // 
      // tabControl3
      // 
      this.tabControl3.Alignment = System.Windows.Forms.TabAlignment.Right;
      this.tabControl3.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
      this.tabControl3.Controls.Add(this.tabPageStartupShutdown);
      this.tabControl3.Controls.Add(this.tabPageInterface);
      this.tabControl3.Controls.Add(this.tabPageFavorites);
      this.tabControl3.Controls.Add(this.tabPageMasterPwd);
      this.tabControl3.Controls.Add(this.tabPageDefaultPwd);
      this.tabControl3.Controls.Add(this.tabPageAmazon);
      this.tabControl3.Controls.Add(this.tabPageConnections);
      this.tabControl3.Controls.Add(this.tabPageBeforeConnect);
      this.tabControl3.Controls.Add(this.tabPageProxy);
      this.tabControl3.Controls.Add(this.tabPageScreenCapture);
      this.tabControl3.Controls.Add(this.tabPageFlickr);
      this.tabControl3.ItemSize = new System.Drawing.Size(20, 20);
      this.tabControl3.Location = new System.Drawing.Point(198, 8);
      this.tabControl3.Multiline = true;
      this.tabControl3.Name = "tabControl3";
      this.tabControl3.RightToLeft = System.Windows.Forms.RightToLeft.No;
      this.tabControl3.SelectedIndex = 0;
      this.tabControl3.Size = new System.Drawing.Size(650, 365);
      this.tabControl3.TabIndex = 7;
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
      this.panelStartupShutdown.Controls.Add(this.groupBoxShutdown);
      this.panelStartupShutdown.Controls.Add(this.groupBoxStartup);
      this.panelStartupShutdown.Location = new System.Drawing.Point(4, 26);
      this.panelStartupShutdown.Name = "panelStartupShutdown";
      this.panelStartupShutdown.Size = new System.Drawing.Size(509, 325);
      this.panelStartupShutdown.TabIndex = 0;
      // 
      // groupBoxShutdown
      // 
      this.groupBoxShutdown.Controls.Add(this.chkSaveConnections);
      this.groupBoxShutdown.Controls.Add(this.chkShowConfirmDialog);
      this.groupBoxShutdown.Location = new System.Drawing.Point(6, 74);
      this.groupBoxShutdown.Name = "groupBoxShutdown";
      this.groupBoxShutdown.Size = new System.Drawing.Size(500, 65);
      this.groupBoxShutdown.TabIndex = 1;
      this.groupBoxShutdown.TabStop = false;
      this.groupBoxShutdown.Text = "Shutdown";
      // 
      // groupBoxStartup
      // 
      this.groupBoxStartup.Controls.Add(this.NeverShowTerminalsCheckbox);
      this.groupBoxStartup.Controls.Add(this.chkSingleInstance);
      this.groupBoxStartup.Location = new System.Drawing.Point(6, 3);
      this.groupBoxStartup.Name = "groupBoxStartup";
      this.groupBoxStartup.Size = new System.Drawing.Size(500, 65);
      this.groupBoxStartup.TabIndex = 0;
      this.groupBoxStartup.TabStop = false;
      this.groupBoxStartup.Text = "Startup";
      // 
      // NeverShowTerminalsCheckbox
      // 
      this.NeverShowTerminalsCheckbox.AutoSize = true;
      this.NeverShowTerminalsCheckbox.Location = new System.Drawing.Point(6, 42);
      this.NeverShowTerminalsCheckbox.Name = "NeverShowTerminalsCheckbox";
      this.NeverShowTerminalsCheckbox.Size = new System.Drawing.Size(251, 17);
      this.NeverShowTerminalsCheckbox.TabIndex = 4;
      this.NeverShowTerminalsCheckbox.Text = "Do not keep me up-to-date on Terminals project";
      this.NeverShowTerminalsCheckbox.UseVisualStyleBackColor = true;
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
      this.panelInterface.Controls.Add(this.groupBox10);
      this.panelInterface.Controls.Add(this.groupBox1);
      this.panelInterface.Controls.Add(this.groupBox7);
      this.panelInterface.Controls.Add(this.groupBoxInformation);
      this.panelInterface.Location = new System.Drawing.Point(4, 26);
      this.panelInterface.Name = "panelInterface";
      this.panelInterface.Size = new System.Drawing.Size(509, 325);
      this.panelInterface.TabIndex = 1;
      // 
      // groupBox10
      // 
      this.groupBox10.Controls.Add(this.EnableGroupsMenu);
      this.groupBox10.Location = new System.Drawing.Point(6, 146);
      this.groupBox10.Name = "groupBox10";
      this.groupBox10.Size = new System.Drawing.Size(500, 44);
      this.groupBox10.TabIndex = 26;
      this.groupBox10.TabStop = false;
      this.groupBox10.Text = "Groups";
      // 
      // groupBox7
      // 
      this.groupBox7.Controls.Add(this.MinimizeToTrayCheckbox);
      this.groupBox7.Location = new System.Drawing.Point(6, 96);
      this.groupBox7.Name = "groupBox7";
      this.groupBox7.Size = new System.Drawing.Size(500, 44);
      this.groupBox7.TabIndex = 3;
      this.groupBox7.TabStop = false;
      this.groupBox7.Text = "System Tray";
      // 
      // groupBoxInformation
      // 
      this.groupBoxInformation.Controls.Add(this.chkShowUserNameInTitle);
      this.groupBoxInformation.Controls.Add(this.chkShowInformationToolTips);
      this.groupBoxInformation.Controls.Add(this.chkShowFullInfo);
      this.groupBoxInformation.Location = new System.Drawing.Point(6, 3);
      this.groupBoxInformation.Name = "groupBoxInformation";
      this.groupBoxInformation.Size = new System.Drawing.Size(500, 87);
      this.groupBoxInformation.TabIndex = 0;
      this.groupBoxInformation.TabStop = false;
      this.groupBoxInformation.Text = "Information";
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
      this.panelFavorites.Controls.Add(this.FavSortGroupBox);
      this.panelFavorites.Controls.Add(this.groupBox11);
      this.panelFavorites.Location = new System.Drawing.Point(4, 26);
      this.panelFavorites.Name = "panelFavorites";
      this.panelFavorites.Size = new System.Drawing.Size(509, 328);
      this.panelFavorites.TabIndex = 1;
      // 
      // groupBox11
      // 
      this.groupBox11.Controls.Add(this.autoCaseTagsCheckbox);
      this.groupBox11.Controls.Add(this.AutoExapandTagsPanelCheckBox);
      this.groupBox11.Controls.Add(this.EnableFavoritesPanel);
      this.groupBox11.Location = new System.Drawing.Point(6, 3);
      this.groupBox11.Name = "groupBox11";
      this.groupBox11.Size = new System.Drawing.Size(500, 88);
      this.groupBox11.TabIndex = 0;
      this.groupBox11.TabStop = false;
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
      this.panelMasterPassword.Controls.Add(this.groupBox3);
      this.panelMasterPassword.Location = new System.Drawing.Point(4, 26);
      this.panelMasterPassword.Name = "panelMasterPassword";
      this.panelMasterPassword.Size = new System.Drawing.Size(509, 328);
      this.panelMasterPassword.TabIndex = 1;
      // 
      // groupBox3
      // 
      this.groupBox3.Controls.Add(this.PasswordsMatchLabel);
      this.groupBox3.Controls.Add(this.PasswordProtectTerminalsCheckbox);
      this.groupBox3.Controls.Add(this.ClearMasterButton);
      this.groupBox3.Controls.Add(this.label6);
      this.groupBox3.Controls.Add(this.PasswordTextbox);
      this.groupBox3.Controls.Add(this.label7);
      this.groupBox3.Controls.Add(this.ConfirmPasswordTextBox);
      this.groupBox3.Location = new System.Drawing.Point(6, 3);
      this.groupBox3.Name = "groupBox3";
      this.groupBox3.Size = new System.Drawing.Size(500, 133);
      this.groupBox3.TabIndex = 0;
      this.groupBox3.TabStop = false;
      // 
      // PasswordsMatchLabel
      // 
      this.PasswordsMatchLabel.AutoSize = true;
      this.PasswordsMatchLabel.ForeColor = System.Drawing.SystemColors.ControlText;
      this.PasswordsMatchLabel.Location = new System.Drawing.Point(223, 73);
      this.PasswordsMatchLabel.Name = "PasswordsMatchLabel";
      this.PasswordsMatchLabel.Size = new System.Drawing.Size(115, 13);
      this.PasswordsMatchLabel.TabIndex = 6;
      this.PasswordsMatchLabel.Text = "Password Match Label";
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
      this.panelDefaultPassword.Controls.Add(this.groupBox2);
      this.panelDefaultPassword.Location = new System.Drawing.Point(4, 26);
      this.panelDefaultPassword.Name = "panelDefaultPassword";
      this.panelDefaultPassword.Size = new System.Drawing.Size(509, 328);
      this.panelDefaultPassword.TabIndex = 2;
      // 
      // groupBox2
      // 
      this.groupBox2.Controls.Add(this.label21);
      this.groupBox2.Controls.Add(this.passwordTxtBox);
      this.groupBox2.Controls.Add(this.label18);
      this.groupBox2.Controls.Add(this.usernameTextbox);
      this.groupBox2.Controls.Add(this.label19);
      this.groupBox2.Controls.Add(this.domainTextbox);
      this.groupBox2.Controls.Add(this.label20);
      this.groupBox2.Location = new System.Drawing.Point(6, 3);
      this.groupBox2.Name = "groupBox2";
      this.groupBox2.Size = new System.Drawing.Size(500, 188);
      this.groupBox2.TabIndex = 0;
      this.groupBox2.TabStop = false;
      // 
      // label21
      // 
      this.label21.Location = new System.Drawing.Point(6, 17);
      this.label21.Name = "label21";
      this.label21.Size = new System.Drawing.Size(360, 84);
      this.label21.TabIndex = 26;
      this.label21.Text = resources.GetString("label21.Text");
      // 
      // passwordTxtBox
      // 
      this.passwordTxtBox.Location = new System.Drawing.Point(71, 158);
      this.passwordTxtBox.Name = "passwordTxtBox";
      this.passwordTxtBox.PasswordChar = '*';
      this.passwordTxtBox.Size = new System.Drawing.Size(149, 20);
      this.passwordTxtBox.TabIndex = 25;
      // 
      // label18
      // 
      this.label18.AutoSize = true;
      this.label18.Location = new System.Drawing.Point(6, 161);
      this.label18.Name = "label18";
      this.label18.Size = new System.Drawing.Size(56, 13);
      this.label18.TabIndex = 24;
      this.label18.Text = "Password:";
      // 
      // usernameTextbox
      // 
      this.usernameTextbox.Location = new System.Drawing.Point(71, 131);
      this.usernameTextbox.Name = "usernameTextbox";
      this.usernameTextbox.Size = new System.Drawing.Size(149, 20);
      this.usernameTextbox.TabIndex = 23;
      // 
      // label19
      // 
      this.label19.AutoSize = true;
      this.label19.Location = new System.Drawing.Point(6, 134);
      this.label19.Name = "label19";
      this.label19.Size = new System.Drawing.Size(58, 13);
      this.label19.TabIndex = 22;
      this.label19.Text = "Username:";
      // 
      // domainTextbox
      // 
      this.domainTextbox.Location = new System.Drawing.Point(71, 104);
      this.domainTextbox.Name = "domainTextbox";
      this.domainTextbox.Size = new System.Drawing.Size(149, 20);
      this.domainTextbox.TabIndex = 21;
      // 
      // label20
      // 
      this.label20.AutoSize = true;
      this.label20.Location = new System.Drawing.Point(6, 107);
      this.label20.Name = "label20";
      this.label20.Size = new System.Drawing.Size(46, 13);
      this.label20.TabIndex = 20;
      this.label20.Text = "Domain:";
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
      this.panelAmazon.Controls.Add(this.groupBox4);
      this.panelAmazon.Location = new System.Drawing.Point(4, 26);
      this.panelAmazon.Name = "panelAmazon";
      this.panelAmazon.Size = new System.Drawing.Size(509, 328);
      this.panelAmazon.TabIndex = 24;
      // 
      // groupBox4
      // 
      this.groupBox4.Controls.Add(this.label22);
      this.groupBox4.Controls.Add(this.BucketNameTextBox);
      this.groupBox4.Controls.Add(this.RestoreButton);
      this.groupBox4.Controls.Add(this.BackupButton);
      this.groupBox4.Controls.Add(this.ErrorLabel);
      this.groupBox4.Controls.Add(this.TestButton);
      this.groupBox4.Controls.Add(this.label17);
      this.groupBox4.Controls.Add(this.label16);
      this.groupBox4.Controls.Add(this.SecretKeyTextbox);
      this.groupBox4.Controls.Add(this.AccessKeyTextbox);
      this.groupBox4.Controls.Add(this.AmazonBackupCheckbox);
      this.groupBox4.Controls.Add(this.pictureBox1);
      this.groupBox4.Location = new System.Drawing.Point(6, 3);
      this.groupBox4.Name = "groupBox4";
      this.groupBox4.Size = new System.Drawing.Size(500, 216);
      this.groupBox4.TabIndex = 3;
      this.groupBox4.TabStop = false;
      // 
      // label22
      // 
      this.label22.AutoSize = true;
      this.label22.Location = new System.Drawing.Point(3, 100);
      this.label22.Name = "label22";
      this.label22.Size = new System.Drawing.Size(72, 13);
      this.label22.TabIndex = 23;
      this.label22.Text = "Bucket Name";
      // 
      // BucketNameTextBox
      // 
      this.BucketNameTextBox.Location = new System.Drawing.Point(143, 97);
      this.BucketNameTextBox.Name = "BucketNameTextBox";
      this.BucketNameTextBox.Size = new System.Drawing.Size(212, 20);
      this.BucketNameTextBox.TabIndex = 22;
      // 
      // RestoreButton
      // 
      this.RestoreButton.Location = new System.Drawing.Point(87, 184);
      this.RestoreButton.Name = "RestoreButton";
      this.RestoreButton.Size = new System.Drawing.Size(75, 23);
      this.RestoreButton.TabIndex = 21;
      this.RestoreButton.Text = "Restore";
      this.RestoreButton.UseVisualStyleBackColor = true;
      this.RestoreButton.Click += new System.EventHandler(this.RestoreButton_Click);
      // 
      // BackupButton
      // 
      this.BackupButton.Location = new System.Drawing.Point(6, 185);
      this.BackupButton.Name = "BackupButton";
      this.BackupButton.Size = new System.Drawing.Size(75, 23);
      this.BackupButton.TabIndex = 20;
      this.BackupButton.Text = "Backup";
      this.BackupButton.UseVisualStyleBackColor = true;
      this.BackupButton.Click += new System.EventHandler(this.BackupButton_Click);
      // 
      // ErrorLabel
      // 
      this.ErrorLabel.ForeColor = System.Drawing.Color.Red;
      this.ErrorLabel.Location = new System.Drawing.Point(3, 137);
      this.ErrorLabel.Name = "ErrorLabel";
      this.ErrorLabel.Size = new System.Drawing.Size(211, 70);
      this.ErrorLabel.TabIndex = 19;
      // 
      // TestButton
      // 
      this.TestButton.Location = new System.Drawing.Point(280, 124);
      this.TestButton.Name = "TestButton";
      this.TestButton.Size = new System.Drawing.Size(75, 23);
      this.TestButton.TabIndex = 18;
      this.TestButton.Text = "Test";
      this.TestButton.UseVisualStyleBackColor = true;
      this.TestButton.Click += new System.EventHandler(this.TestButton_Click);
      // 
      // label17
      // 
      this.label17.AutoSize = true;
      this.label17.Location = new System.Drawing.Point(3, 73);
      this.label17.Name = "label17";
      this.label17.Size = new System.Drawing.Size(125, 13);
      this.label17.TabIndex = 17;
      this.label17.Text = "Your Secret Access Key:";
      // 
      // label16
      // 
      this.label16.AutoSize = true;
      this.label16.Location = new System.Drawing.Point(3, 46);
      this.label16.Name = "label16";
      this.label16.Size = new System.Drawing.Size(105, 13);
      this.label16.TabIndex = 16;
      this.label16.Text = "Your Access Key ID:";
      // 
      // SecretKeyTextbox
      // 
      this.SecretKeyTextbox.Location = new System.Drawing.Point(143, 70);
      this.SecretKeyTextbox.Name = "SecretKeyTextbox";
      this.SecretKeyTextbox.Size = new System.Drawing.Size(212, 20);
      this.SecretKeyTextbox.TabIndex = 15;
      // 
      // AccessKeyTextbox
      // 
      this.AccessKeyTextbox.Location = new System.Drawing.Point(143, 43);
      this.AccessKeyTextbox.Name = "AccessKeyTextbox";
      this.AccessKeyTextbox.Size = new System.Drawing.Size(212, 20);
      this.AccessKeyTextbox.TabIndex = 14;
      // 
      // AmazonBackupCheckbox
      // 
      this.AmazonBackupCheckbox.AutoSize = true;
      this.AmazonBackupCheckbox.Location = new System.Drawing.Point(6, 20);
      this.AmazonBackupCheckbox.Name = "AmazonBackupCheckbox";
      this.AmazonBackupCheckbox.Size = new System.Drawing.Size(222, 17);
      this.AmazonBackupCheckbox.TabIndex = 13;
      this.AmazonBackupCheckbox.Text = "Backup Favorites to Amazons S3 Service";
      this.AmazonBackupCheckbox.UseVisualStyleBackColor = true;
      this.AmazonBackupCheckbox.CheckedChanged += new System.EventHandler(this.AmazonBackupCheckbox_CheckedChanged);
      // 
      // pictureBox1
      // 
      this.pictureBox1.Image = global::Terminals.Properties.Resources.amazon;
      this.pictureBox1.Location = new System.Drawing.Point(220, 153);
      this.pictureBox1.Name = "pictureBox1";
      this.pictureBox1.Size = new System.Drawing.Size(135, 55);
      this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
      this.pictureBox1.TabIndex = 12;
      this.pictureBox1.TabStop = false;
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
      this.panelConnections.Controls.Add(this.groupBox12);
      this.panelConnections.Controls.Add(this.groupBoxConnections);
      this.panelConnections.Location = new System.Drawing.Point(4, 26);
      this.panelConnections.Name = "panelConnections";
      this.panelConnections.Size = new System.Drawing.Size(509, 328);
      this.panelConnections.TabIndex = 24;
      // 
      // groupBox12
      // 
      this.groupBox12.Controls.Add(this.EvaluatedDesktopShareLabel);
      this.groupBox12.Controls.Add(this.PortscanTimeoutTextBox);
      this.groupBox12.Controls.Add(this.label2);
      this.groupBox12.Controls.Add(this.txtDefaultDesktopShare);
      this.groupBox12.Controls.Add(this.label3);
      this.groupBox12.Controls.Add(this.label5);
      this.groupBox12.Controls.Add(this.label4);
      this.groupBox12.Location = new System.Drawing.Point(6, 74);
      this.groupBox12.Name = "groupBox12";
      this.groupBox12.Size = new System.Drawing.Size(500, 151);
      this.groupBox12.TabIndex = 20;
      this.groupBox12.TabStop = false;
      this.groupBox12.Text = "RDP Desktop Share";
      // 
      // EvaluatedDesktopShareLabel
      // 
      this.EvaluatedDesktopShareLabel.AutoSize = true;
      this.EvaluatedDesktopShareLabel.ForeColor = System.Drawing.Color.Blue;
      this.EvaluatedDesktopShareLabel.Location = new System.Drawing.Point(350, 17);
      this.EvaluatedDesktopShareLabel.Name = "EvaluatedDesktopShareLabel";
      this.EvaluatedDesktopShareLabel.Size = new System.Drawing.Size(146, 13);
      this.EvaluatedDesktopShareLabel.TabIndex = 23;
      this.EvaluatedDesktopShareLabel.Text = "Hidden Label Evaluate Share";
      // 
      // PortscanTimeoutTextBox
      // 
      this.PortscanTimeoutTextBox.Location = new System.Drawing.Point(123, 84);
      this.PortscanTimeoutTextBox.Name = "PortscanTimeoutTextBox";
      this.PortscanTimeoutTextBox.Size = new System.Drawing.Size(58, 20);
      this.PortscanTimeoutTextBox.TabIndex = 22;
      // 
      // label2
      // 
      this.label2.AutoSize = true;
      this.label2.Location = new System.Drawing.Point(3, 17);
      this.label2.Name = "label2";
      this.label2.Size = new System.Drawing.Size(279, 13);
      this.label2.TabIndex = 17;
      this.label2.Text = "Default Desktop Share (Use %SERVER% and %USER%):";
      // 
      // txtDefaultDesktopShare
      // 
      this.txtDefaultDesktopShare.Location = new System.Drawing.Point(6, 33);
      this.txtDefaultDesktopShare.Name = "txtDefaultDesktopShare";
      this.txtDefaultDesktopShare.Size = new System.Drawing.Size(360, 20);
      this.txtDefaultDesktopShare.TabIndex = 18;
      this.txtDefaultDesktopShare.TextChanged += new System.EventHandler(this.txtDefaultDesktopShare_TextChanged);
      // 
      // label3
      // 
      this.label3.AutoSize = true;
      this.label3.Location = new System.Drawing.Point(3, 68);
      this.label3.Name = "label3";
      this.label3.Size = new System.Drawing.Size(317, 13);
      this.label3.TabIndex = 19;
      this.label3.Text = "Evaluated Desktop Share (according to selected connection tab):";
      // 
      // label5
      // 
      this.label5.AutoSize = true;
      this.label5.Location = new System.Drawing.Point(187, 87);
      this.label5.Name = "label5";
      this.label5.Size = new System.Drawing.Size(49, 13);
      this.label5.TabIndex = 21;
      this.label5.Text = "Seconds";
      // 
      // label4
      // 
      this.label4.AutoSize = true;
      this.label4.Location = new System.Drawing.Point(3, 87);
      this.label4.Name = "label4";
      this.label4.Size = new System.Drawing.Size(113, 13);
      this.label4.TabIndex = 20;
      this.label4.Text = "Port Scanner Timeout:";
      // 
      // groupBoxConnections
      // 
      this.groupBoxConnections.Controls.Add(this.validateServerNamesCheckbox);
      this.groupBoxConnections.Controls.Add(this.warnDisconnectCheckBox);
      this.groupBoxConnections.Location = new System.Drawing.Point(6, 3);
      this.groupBoxConnections.Name = "groupBoxConnections";
      this.groupBoxConnections.Size = new System.Drawing.Size(500, 65);
      this.groupBoxConnections.TabIndex = 4;
      this.groupBoxConnections.TabStop = false;
      // 
      // validateServerNamesCheckbox
      // 
      this.validateServerNamesCheckbox.AutoSize = true;
      this.validateServerNamesCheckbox.Checked = true;
      this.validateServerNamesCheckbox.CheckState = System.Windows.Forms.CheckState.Checked;
      this.validateServerNamesCheckbox.Location = new System.Drawing.Point(6, 20);
      this.validateServerNamesCheckbox.Name = "validateServerNamesCheckbox";
      this.validateServerNamesCheckbox.Size = new System.Drawing.Size(134, 17);
      this.validateServerNamesCheckbox.TabIndex = 18;
      this.validateServerNamesCheckbox.Text = "Validate Server Names";
      this.validateServerNamesCheckbox.UseVisualStyleBackColor = true;
      // 
      // warnDisconnectCheckBox
      // 
      this.warnDisconnectCheckBox.AutoSize = true;
      this.warnDisconnectCheckBox.Checked = true;
      this.warnDisconnectCheckBox.CheckState = System.Windows.Forms.CheckState.Checked;
      this.warnDisconnectCheckBox.Location = new System.Drawing.Point(6, 43);
      this.warnDisconnectCheckBox.Name = "warnDisconnectCheckBox";
      this.warnDisconnectCheckBox.Size = new System.Drawing.Size(122, 17);
      this.warnDisconnectCheckBox.TabIndex = 19;
      this.warnDisconnectCheckBox.Text = "Warn on disconnect";
      this.warnDisconnectCheckBox.UseVisualStyleBackColor = true;
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
      this.panelExecuteBeforeConnect.Controls.Add(this.groupBox5);
      this.panelExecuteBeforeConnect.Location = new System.Drawing.Point(4, 26);
      this.panelExecuteBeforeConnect.Name = "panelExecuteBeforeConnect";
      this.panelExecuteBeforeConnect.Size = new System.Drawing.Size(509, 328);
      this.panelExecuteBeforeConnect.TabIndex = 25;
      // 
      // groupBox5
      // 
      this.groupBox5.Controls.Add(this.txtInitialDirectory);
      this.groupBox5.Controls.Add(this.txtArguments);
      this.groupBox5.Controls.Add(this.txtCommand);
      this.groupBox5.Controls.Add(this.label13);
      this.groupBox5.Controls.Add(this.chkExecuteBeforeConnect);
      this.groupBox5.Controls.Add(this.label12);
      this.groupBox5.Controls.Add(this.chkWaitForExit);
      this.groupBox5.Controls.Add(this.label11);
      this.groupBox5.Location = new System.Drawing.Point(6, 3);
      this.groupBox5.Name = "groupBox5";
      this.groupBox5.Size = new System.Drawing.Size(500, 150);
      this.groupBox5.TabIndex = 4;
      this.groupBox5.TabStop = false;
      // 
      // txtInitialDirectory
      // 
      this.txtInitialDirectory.Location = new System.Drawing.Point(102, 97);
      this.txtInitialDirectory.Name = "txtInitialDirectory";
      this.txtInitialDirectory.Size = new System.Drawing.Size(265, 20);
      this.txtInitialDirectory.TabIndex = 22;
      // 
      // txtArguments
      // 
      this.txtArguments.Location = new System.Drawing.Point(102, 70);
      this.txtArguments.Name = "txtArguments";
      this.txtArguments.Size = new System.Drawing.Size(265, 20);
      this.txtArguments.TabIndex = 21;
      // 
      // txtCommand
      // 
      this.txtCommand.Location = new System.Drawing.Point(102, 43);
      this.txtCommand.Name = "txtCommand";
      this.txtCommand.Size = new System.Drawing.Size(265, 20);
      this.txtCommand.TabIndex = 20;
      // 
      // label13
      // 
      this.label13.AutoSize = true;
      this.label13.Location = new System.Drawing.Point(3, 100);
      this.label13.Name = "label13";
      this.label13.Size = new System.Drawing.Size(79, 13);
      this.label13.TabIndex = 26;
      this.label13.Text = "Initial Directory:";
      // 
      // chkExecuteBeforeConnect
      // 
      this.chkExecuteBeforeConnect.AutoSize = true;
      this.chkExecuteBeforeConnect.Location = new System.Drawing.Point(6, 20);
      this.chkExecuteBeforeConnect.Name = "chkExecuteBeforeConnect";
      this.chkExecuteBeforeConnect.Size = new System.Drawing.Size(175, 17);
      this.chkExecuteBeforeConnect.TabIndex = 19;
      this.chkExecuteBeforeConnect.Text = "Enable execute before connect";
      this.chkExecuteBeforeConnect.UseVisualStyleBackColor = true;
      this.chkExecuteBeforeConnect.CheckedChanged += new System.EventHandler(this.chkExecuteBeforeConnect_CheckedChanged);
      // 
      // label12
      // 
      this.label12.AutoSize = true;
      this.label12.Location = new System.Drawing.Point(3, 73);
      this.label12.Name = "label12";
      this.label12.Size = new System.Drawing.Size(57, 13);
      this.label12.TabIndex = 25;
      this.label12.Text = "Arguments";
      // 
      // chkWaitForExit
      // 
      this.chkWaitForExit.AutoSize = true;
      this.chkWaitForExit.Location = new System.Drawing.Point(6, 124);
      this.chkWaitForExit.Name = "chkWaitForExit";
      this.chkWaitForExit.Size = new System.Drawing.Size(82, 17);
      this.chkWaitForExit.TabIndex = 24;
      this.chkWaitForExit.Text = "Wait for exit";
      this.chkWaitForExit.UseVisualStyleBackColor = true;
      // 
      // label11
      // 
      this.label11.AutoSize = true;
      this.label11.Location = new System.Drawing.Point(3, 46);
      this.label11.Name = "label11";
      this.label11.Size = new System.Drawing.Size(57, 13);
      this.label11.TabIndex = 23;
      this.label11.Text = "Command:";
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
      this.panelProxy.Controls.Add(this.groupBox6);
      this.panelProxy.Location = new System.Drawing.Point(4, 26);
      this.panelProxy.Name = "panelProxy";
      this.panelProxy.Size = new System.Drawing.Size(509, 328);
      this.panelProxy.TabIndex = 26;
      // 
      // groupBox6
      // 
      this.groupBox6.Controls.Add(this.label15);
      this.groupBox6.Controls.Add(this.ProxyPortTextbox);
      this.groupBox6.Controls.Add(this.ProxyAddressTextbox);
      this.groupBox6.Controls.Add(this.label14);
      this.groupBox6.Controls.Add(this.AutoProxyRadioButton);
      this.groupBox6.Controls.Add(this.ProxyRadionButton);
      this.groupBox6.Location = new System.Drawing.Point(6, 3);
      this.groupBox6.Name = "groupBox6";
      this.groupBox6.Size = new System.Drawing.Size(500, 98);
      this.groupBox6.TabIndex = 5;
      this.groupBox6.TabStop = false;
      // 
      // label15
      // 
      this.label15.AutoSize = true;
      this.label15.Location = new System.Drawing.Point(258, 70);
      this.label15.Name = "label15";
      this.label15.Size = new System.Drawing.Size(29, 13);
      this.label15.TabIndex = 9;
      this.label15.Text = "Port:";
      // 
      // ProxyPortTextbox
      // 
      this.ProxyPortTextbox.Location = new System.Drawing.Point(295, 67);
      this.ProxyPortTextbox.Name = "ProxyPortTextbox";
      this.ProxyPortTextbox.Size = new System.Drawing.Size(34, 20);
      this.ProxyPortTextbox.TabIndex = 8;
      this.ProxyPortTextbox.Text = "80";
      // 
      // ProxyAddressTextbox
      // 
      this.ProxyAddressTextbox.Location = new System.Drawing.Point(60, 66);
      this.ProxyAddressTextbox.Name = "ProxyAddressTextbox";
      this.ProxyAddressTextbox.Size = new System.Drawing.Size(192, 20);
      this.ProxyAddressTextbox.TabIndex = 7;
      // 
      // label14
      // 
      this.label14.AutoSize = true;
      this.label14.Location = new System.Drawing.Point(3, 70);
      this.label14.Name = "label14";
      this.label14.Size = new System.Drawing.Size(48, 13);
      this.label14.TabIndex = 6;
      this.label14.Text = "Address:";
      // 
      // AutoProxyRadioButton
      // 
      this.AutoProxyRadioButton.AutoSize = true;
      this.AutoProxyRadioButton.Location = new System.Drawing.Point(6, 20);
      this.AutoProxyRadioButton.Name = "AutoProxyRadioButton";
      this.AutoProxyRadioButton.Size = new System.Drawing.Size(151, 17);
      this.AutoProxyRadioButton.TabIndex = 0;
      this.AutoProxyRadioButton.TabStop = true;
      this.AutoProxyRadioButton.Text = "Automatically Detect Proxy";
      this.AutoProxyRadioButton.UseVisualStyleBackColor = true;
      this.AutoProxyRadioButton.CheckedChanged += new System.EventHandler(this.ProxyRadioButton_CheckedChanged);
      // 
      // ProxyRadionButton
      // 
      this.ProxyRadionButton.AutoSize = true;
      this.ProxyRadionButton.Location = new System.Drawing.Point(6, 43);
      this.ProxyRadionButton.Name = "ProxyRadionButton";
      this.ProxyRadionButton.Size = new System.Drawing.Size(172, 17);
      this.ProxyRadionButton.TabIndex = 1;
      this.ProxyRadionButton.TabStop = true;
      this.ProxyRadionButton.Text = "Use the following Proxy Server:";
      this.ProxyRadionButton.UseVisualStyleBackColor = true;
      this.ProxyRadionButton.CheckedChanged += new System.EventHandler(this.ProxyRadioButton_CheckedChanged);
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
      this.panelScreenCapture.Controls.Add(this.groupBox8);
      this.panelScreenCapture.Location = new System.Drawing.Point(4, 26);
      this.panelScreenCapture.Name = "panelScreenCapture";
      this.panelScreenCapture.Size = new System.Drawing.Size(509, 328);
      this.panelScreenCapture.TabIndex = 25;
      // 
      // groupBox8
      // 
      this.groupBox8.Controls.Add(this.chkAutoSwitchToCaptureCheckbox);
      this.groupBox8.Controls.Add(this.chkEnableCaptureToFolder);
      this.groupBox8.Controls.Add(this.chkEnableCaptureToClipboard);
      this.groupBox8.Controls.Add(this.ButtonBrowseCaptureFolder);
      this.groupBox8.Controls.Add(this.label23);
      this.groupBox8.Controls.Add(this.txtScreenCaptureFolder);
      this.groupBox8.Location = new System.Drawing.Point(6, 3);
      this.groupBox8.Name = "groupBox8";
      this.groupBox8.Size = new System.Drawing.Size(500, 138);
      this.groupBox8.TabIndex = 4;
      this.groupBox8.TabStop = false;
      // 
      // chkAutoSwitchToCaptureCheckbox
      // 
      this.chkAutoSwitchToCaptureCheckbox.AutoSize = true;
      this.chkAutoSwitchToCaptureCheckbox.Checked = true;
      this.chkAutoSwitchToCaptureCheckbox.CheckState = System.Windows.Forms.CheckState.Checked;
      this.chkAutoSwitchToCaptureCheckbox.Location = new System.Drawing.Point(26, 66);
      this.chkAutoSwitchToCaptureCheckbox.Name = "chkAutoSwitchToCaptureCheckbox";
      this.chkAutoSwitchToCaptureCheckbox.Size = new System.Drawing.Size(191, 17);
      this.chkAutoSwitchToCaptureCheckbox.TabIndex = 27;
      this.chkAutoSwitchToCaptureCheckbox.Text = "Auto switch to manager on capture";
      this.chkAutoSwitchToCaptureCheckbox.UseVisualStyleBackColor = true;
      // 
      // chkEnableCaptureToFolder
      // 
      this.chkEnableCaptureToFolder.AutoSize = true;
      this.chkEnableCaptureToFolder.Location = new System.Drawing.Point(6, 43);
      this.chkEnableCaptureToFolder.Name = "chkEnableCaptureToFolder";
      this.chkEnableCaptureToFolder.Size = new System.Drawing.Size(174, 17);
      this.chkEnableCaptureToFolder.TabIndex = 26;
      this.chkEnableCaptureToFolder.Text = "Enable screen capture to folder";
      this.chkEnableCaptureToFolder.UseVisualStyleBackColor = true;
      this.chkEnableCaptureToFolder.CheckedChanged += new System.EventHandler(this.chkDisableCaptureToFolder_CheckedChanged);
      // 
      // chkEnableCaptureToClipboard
      // 
      this.chkEnableCaptureToClipboard.AutoSize = true;
      this.chkEnableCaptureToClipboard.Checked = true;
      this.chkEnableCaptureToClipboard.CheckState = System.Windows.Forms.CheckState.Checked;
      this.chkEnableCaptureToClipboard.Location = new System.Drawing.Point(6, 20);
      this.chkEnableCaptureToClipboard.Name = "chkEnableCaptureToClipboard";
      this.chkEnableCaptureToClipboard.Size = new System.Drawing.Size(191, 17);
      this.chkEnableCaptureToClipboard.TabIndex = 25;
      this.chkEnableCaptureToClipboard.Text = "Enable screen capture to clipboard";
      this.chkEnableCaptureToClipboard.UseVisualStyleBackColor = true;
      // 
      // ButtonBrowseCaptureFolder
      // 
      this.ButtonBrowseCaptureFolder.Location = new System.Drawing.Point(383, 105);
      this.ButtonBrowseCaptureFolder.Name = "ButtonBrowseCaptureFolder";
      this.ButtonBrowseCaptureFolder.Size = new System.Drawing.Size(65, 23);
      this.ButtonBrowseCaptureFolder.TabIndex = 24;
      this.ButtonBrowseCaptureFolder.Text = "Browse...";
      this.ButtonBrowseCaptureFolder.UseVisualStyleBackColor = true;
      this.ButtonBrowseCaptureFolder.Click += new System.EventHandler(this.ButtonBrowseCaptureFolder_Click);
      // 
      // label23
      // 
      this.label23.AutoSize = true;
      this.label23.Location = new System.Drawing.Point(6, 91);
      this.label23.Name = "label23";
      this.label23.Size = new System.Drawing.Size(133, 13);
      this.label23.TabIndex = 23;
      this.label23.Text = "Screen capture root folder:";
      // 
      // txtScreenCaptureFolder
      // 
      this.txtScreenCaptureFolder.Location = new System.Drawing.Point(6, 107);
      this.txtScreenCaptureFolder.Name = "txtScreenCaptureFolder";
      this.txtScreenCaptureFolder.Size = new System.Drawing.Size(371, 20);
      this.txtScreenCaptureFolder.TabIndex = 22;
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
      this.panelFlickr.Controls.Add(this.groupBox9);
      this.panelFlickr.Location = new System.Drawing.Point(4, 26);
      this.panelFlickr.Name = "panelFlickr";
      this.panelFlickr.Size = new System.Drawing.Size(509, 328);
      this.panelFlickr.TabIndex = 25;
      // 
      // groupBox9
      // 
      this.groupBox9.Controls.Add(this.label25);
      this.groupBox9.Controls.Add(this.label10);
      this.groupBox9.Controls.Add(this.label9);
      this.groupBox9.Controls.Add(this.CompleteAuthButton);
      this.groupBox9.Controls.Add(this.AuthorizeFlickrButton);
      this.groupBox9.Controls.Add(this.label8);
      this.groupBox9.Location = new System.Drawing.Point(6, 3);
      this.groupBox9.Name = "groupBox9";
      this.groupBox9.Size = new System.Drawing.Size(500, 213);
      this.groupBox9.TabIndex = 4;
      this.groupBox9.TabStop = false;
      // 
      // label25
      // 
      this.label25.Location = new System.Drawing.Point(6, 17);
      this.label25.Name = "label25";
      this.label25.Size = new System.Drawing.Size(382, 40);
      this.label25.TabIndex = 21;
      this.label25.Text = "Use Flickr as storage place for screen captures.";
      // 
      // label10
      // 
      this.label10.Location = new System.Drawing.Point(87, 133);
      this.label10.Name = "label10";
      this.label10.Size = new System.Drawing.Size(301, 66);
      this.label10.TabIndex = 20;
      this.label10.Text = "Second you must click the Complete button to finish the process. Only do this AFT" +
    "ER you have accepted Terminals access to your account on the Flickr Web Site.";
      this.label10.DoubleClick += new System.EventHandler(this.label10_DoubleClick);
      // 
      // label9
      // 
      this.label9.Location = new System.Drawing.Point(87, 57);
      this.label9.Name = "label9";
      this.label9.Size = new System.Drawing.Size(301, 61);
      this.label9.TabIndex = 19;
      this.label9.Text = "First you must first Authorize Terminals with your Flickr account. Press the Auth" +
    "orize button now, login to your Flickr Account and allow Terminals limited acces" +
    "s to your account.";
      // 
      // CompleteAuthButton
      // 
      this.CompleteAuthButton.Enabled = false;
      this.CompleteAuthButton.Location = new System.Drawing.Point(6, 133);
      this.CompleteAuthButton.Name = "CompleteAuthButton";
      this.CompleteAuthButton.Size = new System.Drawing.Size(75, 23);
      this.CompleteAuthButton.TabIndex = 18;
      this.CompleteAuthButton.Text = "Complete...";
      this.CompleteAuthButton.UseVisualStyleBackColor = true;
      this.CompleteAuthButton.Click += new System.EventHandler(this.CompleteAuthButton_Click);
      // 
      // AuthorizeFlickrButton
      // 
      this.AuthorizeFlickrButton.Location = new System.Drawing.Point(9, 57);
      this.AuthorizeFlickrButton.Name = "AuthorizeFlickrButton";
      this.AuthorizeFlickrButton.Size = new System.Drawing.Size(75, 23);
      this.AuthorizeFlickrButton.TabIndex = 17;
      this.AuthorizeFlickrButton.Text = "Authorize...";
      this.AuthorizeFlickrButton.UseVisualStyleBackColor = true;
      this.AuthorizeFlickrButton.Click += new System.EventHandler(this.AuthorizeFlickrButton_Click);
      // 
      // label8
      // 
      this.label8.AutoSize = true;
      this.label8.Location = new System.Drawing.Point(218, 76);
      this.label8.Name = "label8";
      this.label8.Size = new System.Drawing.Size(0, 13);
      this.label8.TabIndex = 16;
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
      this.Controls.Add(this.tabControl3);
      this.Controls.Add(this.OptionsTreeView);
      this.Controls.Add(this.linkLabel1);
      this.Controls.Add(this.label1);
      this.Controls.Add(this.btnOk);
      this.Controls.Add(this.btnCancel);
      this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
      this.MaximizeBox = false;
      this.MinimizeBox = false;
      this.Name = "OptionDialog";
      this.ShowIcon = false;
      this.ShowInTaskbar = false;
      this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
      this.Text = "Options";
      this.FavSortGroupBox.ResumeLayout(false);
      this.FavSortGroupBox.PerformLayout();
      this.groupBox1.ResumeLayout(false);
      this.groupBox1.PerformLayout();
      this.tabControl3.ResumeLayout(false);
      this.tabPageStartupShutdown.ResumeLayout(false);
      this.panelStartupShutdown.ResumeLayout(false);
      this.groupBoxShutdown.ResumeLayout(false);
      this.groupBoxShutdown.PerformLayout();
      this.groupBoxStartup.ResumeLayout(false);
      this.groupBoxStartup.PerformLayout();
      this.tabPageInterface.ResumeLayout(false);
      this.panelInterface.ResumeLayout(false);
      this.groupBox10.ResumeLayout(false);
      this.groupBox10.PerformLayout();
      this.groupBox7.ResumeLayout(false);
      this.groupBox7.PerformLayout();
      this.groupBoxInformation.ResumeLayout(false);
      this.groupBoxInformation.PerformLayout();
      this.tabPageFavorites.ResumeLayout(false);
      this.panelFavorites.ResumeLayout(false);
      this.groupBox11.ResumeLayout(false);
      this.groupBox11.PerformLayout();
      this.tabPageMasterPwd.ResumeLayout(false);
      this.panelMasterPassword.ResumeLayout(false);
      this.groupBox3.ResumeLayout(false);
      this.groupBox3.PerformLayout();
      this.tabPageDefaultPwd.ResumeLayout(false);
      this.panelDefaultPassword.ResumeLayout(false);
      this.groupBox2.ResumeLayout(false);
      this.groupBox2.PerformLayout();
      this.tabPageAmazon.ResumeLayout(false);
      this.panelAmazon.ResumeLayout(false);
      this.groupBox4.ResumeLayout(false);
      this.groupBox4.PerformLayout();
      ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
      this.tabPageConnections.ResumeLayout(false);
      this.panelConnections.ResumeLayout(false);
      this.groupBox12.ResumeLayout(false);
      this.groupBox12.PerformLayout();
      this.groupBoxConnections.ResumeLayout(false);
      this.groupBoxConnections.PerformLayout();
      this.tabPageBeforeConnect.ResumeLayout(false);
      this.panelExecuteBeforeConnect.ResumeLayout(false);
      this.groupBox5.ResumeLayout(false);
      this.groupBox5.PerformLayout();
      this.tabPageProxy.ResumeLayout(false);
      this.panelProxy.ResumeLayout(false);
      this.groupBox6.ResumeLayout(false);
      this.groupBox6.PerformLayout();
      this.tabPageScreenCapture.ResumeLayout(false);
      this.panelScreenCapture.ResumeLayout(false);
      this.groupBox8.ResumeLayout(false);
      this.groupBox8.PerformLayout();
      this.tabPageFlickr.ResumeLayout(false);
      this.panelFlickr.ResumeLayout(false);
      this.groupBox9.ResumeLayout(false);
      this.groupBox9.PerformLayout();
      this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button btnOk;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.LinkLabel linkLabel1;
        private System.Windows.Forms.Button ClearMasterButton;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.TextBox ConfirmPasswordTextBox;
        private System.Windows.Forms.TextBox PasswordTextbox;
        private System.Windows.Forms.CheckBox PasswordProtectTerminalsCheckbox;
        private System.Windows.Forms.CheckBox MinimizeToTrayCheckbox;
        private System.Windows.Forms.CheckBox chkSaveConnections;
        private System.Windows.Forms.CheckBox chkShowConfirmDialog;
        private System.Windows.Forms.CheckBox chkSingleInstance;
        private System.Windows.Forms.CheckBox chkShowFullInfo;
        private System.Windows.Forms.CheckBox chkShowUserNameInTitle;
        private System.Windows.Forms.CheckBox chkShowInformationToolTips;
        private System.Windows.Forms.CheckBox EnableFavoritesPanel;
        private System.Windows.Forms.CheckBox EnableGroupsMenu;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.RadioButton RenderBlackRadio;
        private System.Windows.Forms.RadioButton RenderBlueRadio;
        private System.Windows.Forms.RadioButton RenderNormalRadio;
        private System.Windows.Forms.GroupBox FavSortGroupBox;
        private System.Windows.Forms.RadioButton NoneRadioButton;
        private System.Windows.Forms.RadioButton ProtocolRadionButton;
        private System.Windows.Forms.RadioButton ConnectionNameRadioButton;
        private System.Windows.Forms.RadioButton ServerNameRadio;
        private System.Windows.Forms.CheckBox autoCaseTagsCheckbox;
        private System.Windows.Forms.CheckBox AutoExapandTagsPanelCheckBox;
        private System.Windows.Forms.TreeView OptionsTreeView;
        private System.Windows.Forms.TabControl tabControl3;
        private System.Windows.Forms.TabPage tabPageStartupShutdown;
        private System.Windows.Forms.Panel panelStartupShutdown;
        private System.Windows.Forms.GroupBox groupBoxShutdown;
        private System.Windows.Forms.GroupBox groupBoxStartup;
        private System.Windows.Forms.TabPage tabPageInterface;
        private System.Windows.Forms.Panel panelInterface;
        private System.Windows.Forms.GroupBox groupBox10;
        private System.Windows.Forms.GroupBox groupBox7;
        private System.Windows.Forms.GroupBox groupBoxInformation;
        private System.Windows.Forms.TabPage tabPageFavorites;
        private System.Windows.Forms.Panel panelFavorites;
        private System.Windows.Forms.GroupBox groupBox11;
        private System.Windows.Forms.TabPage tabPageMasterPwd;
        private System.Windows.Forms.Panel panelMasterPassword;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.TabPage tabPageDefaultPwd;
        private System.Windows.Forms.Panel panelDefaultPassword;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.Label label21;
        private System.Windows.Forms.TextBox passwordTxtBox;
        private System.Windows.Forms.Label label18;
        private System.Windows.Forms.TextBox usernameTextbox;
        private System.Windows.Forms.Label label19;
        private System.Windows.Forms.TextBox domainTextbox;
        private System.Windows.Forms.Label label20;
        private System.Windows.Forms.TabPage tabPageAmazon;
        private System.Windows.Forms.Panel panelAmazon;
        private System.Windows.Forms.GroupBox groupBox4;
        private System.Windows.Forms.Label label22;
        private System.Windows.Forms.TextBox BucketNameTextBox;
        private System.Windows.Forms.Button RestoreButton;
        private System.Windows.Forms.Button BackupButton;
        private System.Windows.Forms.Label ErrorLabel;
        private System.Windows.Forms.Button TestButton;
        private System.Windows.Forms.Label label17;
        private System.Windows.Forms.Label label16;
        private System.Windows.Forms.TextBox SecretKeyTextbox;
        private System.Windows.Forms.TextBox AccessKeyTextbox;
        private System.Windows.Forms.CheckBox AmazonBackupCheckbox;
        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.TabPage tabPageConnections;
        private System.Windows.Forms.Panel panelConnections;
        private System.Windows.Forms.GroupBox groupBoxConnections;
        private System.Windows.Forms.CheckBox validateServerNamesCheckbox;
        private System.Windows.Forms.CheckBox warnDisconnectCheckBox;
        private System.Windows.Forms.TabPage tabPageBeforeConnect;
        private System.Windows.Forms.Panel panelExecuteBeforeConnect;
        private System.Windows.Forms.GroupBox groupBox5;
        private System.Windows.Forms.TextBox txtInitialDirectory;
        private System.Windows.Forms.TextBox txtArguments;
        private System.Windows.Forms.TextBox txtCommand;
        private System.Windows.Forms.Label label13;
        private System.Windows.Forms.CheckBox chkExecuteBeforeConnect;
        private System.Windows.Forms.Label label12;
        private System.Windows.Forms.CheckBox chkWaitForExit;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.TabPage tabPageProxy;
        private System.Windows.Forms.Panel panelProxy;
        private System.Windows.Forms.GroupBox groupBox6;
        private System.Windows.Forms.Label label15;
        private System.Windows.Forms.TextBox ProxyPortTextbox;
        private System.Windows.Forms.TextBox ProxyAddressTextbox;
        private System.Windows.Forms.Label label14;
        private System.Windows.Forms.RadioButton AutoProxyRadioButton;
        private System.Windows.Forms.RadioButton ProxyRadionButton;
        private System.Windows.Forms.TabPage tabPageScreenCapture;
        private System.Windows.Forms.Panel panelScreenCapture;
        private System.Windows.Forms.GroupBox groupBox8;
        private System.Windows.Forms.CheckBox chkAutoSwitchToCaptureCheckbox;
        private System.Windows.Forms.CheckBox chkEnableCaptureToFolder;
        private System.Windows.Forms.CheckBox chkEnableCaptureToClipboard;
        private System.Windows.Forms.Button ButtonBrowseCaptureFolder;
        private System.Windows.Forms.Label label23;
        private System.Windows.Forms.TextBox txtScreenCaptureFolder;
        private System.Windows.Forms.TabPage tabPageFlickr;
        private System.Windows.Forms.Panel panelFlickr;
        private System.Windows.Forms.GroupBox groupBox9;
        private System.Windows.Forms.Label label25;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.Button CompleteAuthButton;
        private System.Windows.Forms.Button AuthorizeFlickrButton;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Label OptionTitelLabel;
        private System.Windows.Forms.TextBox PortscanTimeoutTextBox;
        private System.Windows.Forms.TextBox txtDefaultDesktopShare;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label PasswordsMatchLabel;
        private System.Windows.Forms.Label EvaluatedDesktopShareLabel;
        private System.Windows.Forms.GroupBox groupBox12;
        private System.Windows.Forms.CheckBox NeverShowTerminalsCheckbox;
    }
}
