namespace Terminals
{
    partial class NewTerminalForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(NewTerminalForm));
            this.btnCancel = new System.Windows.Forms.Button();
            this.chkAddtoToolbar = new System.Windows.Forms.CheckBox();
            this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
            this.label10 = new System.Windows.Forms.Label();
            this.txtDesktopShare = new System.Windows.Forms.TextBox();
            this.cmbResolution = new System.Windows.Forms.ComboBox();
            this.heightUpDown = new System.Windows.Forms.NumericUpDown();
            this.widthUpDown = new System.Windows.Forms.NumericUpDown();
            this.EnableNLAAuthenticationCheckbox = new System.Windows.Forms.CheckBox();
            this.EnableTLSAuthenticationCheckbox = new System.Windows.Forms.CheckBox();
            this.ShutdownTimeoutTextBox = new System.Windows.Forms.TextBox();
            this.OverallTimeoutTextbox = new System.Windows.Forms.TextBox();
            this.SingleTimeOutTextbox = new System.Windows.Forms.TextBox();
            this.IdleTimeoutMinutesTextBox = new System.Windows.Forms.TextBox();
            this.GrabFocusOnConnectCheckbox = new System.Windows.Forms.CheckBox();
            this.EnableEncryptionCheckbox = new System.Windows.Forms.CheckBox();
            this.DisableWindowsKeyCheckbox = new System.Windows.Forms.CheckBox();
            this.DetectDoubleClicksCheckbox = new System.Windows.Forms.CheckBox();
            this.DisplayConnectionBarCheckbox = new System.Windows.Forms.CheckBox();
            this.DisableControlAltDeleteCheckbox = new System.Windows.Forms.CheckBox();
            this.AcceleratorPassthroughCheckBox = new System.Windows.Forms.CheckBox();
            this.EnableCompressionCheckbox = new System.Windows.Forms.CheckBox();
            this.EnableBitmapPersistenceCheckbox = new System.Windows.Forms.CheckBox();
            this.AllowBackgroundInputCheckBox = new System.Windows.Forms.CheckBox();
            this.label37 = new System.Windows.Forms.Label();
            this.vncAutoScaleCheckbox = new System.Windows.Forms.CheckBox();
            this.txtPort = new System.Windows.Forms.TextBox();
            this.txtName = new System.Windows.Forms.TextBox();
            this.CredentialManagerPicturebox = new System.Windows.Forms.PictureBox();
            this.pictureBox2 = new System.Windows.Forms.PictureBox();
            this.ProtocolComboBox = new System.Windows.Forms.ComboBox();
            this.cmbServers = new System.Windows.Forms.ComboBox();
            this.btnAddNewTag = new System.Windows.Forms.Button();
            this.AllTagsAddButton = new System.Windows.Forms.Button();
            this.lvConnectionTags = new System.Windows.Forms.ListView();
            this.btnRemoveTag = new System.Windows.Forms.Button();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.ras1 = new FalafelSoftware.TransPort.Ras();
            this.NewWindowCheckbox = new System.Windows.Forms.CheckBox();
            this.textBox3 = new System.Windows.Forms.TextBox();
            this.label43 = new System.Windows.Forms.Label();
            this.button1 = new System.Windows.Forms.Button();
            this.contextMenuStripDefaults = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.saveCurrentSettingsAsDefaultToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.removeSavedDefaultsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.contextMenuStripSave = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.saveConnectToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.saveNewToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.saveCopyToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.groupBox7 = new System.Windows.Forms.GroupBox();
            this.btnSaveDefault = new Terminals.Forms.Controls.SplitButton();
            this.btnSave = new Terminals.Forms.Controls.SplitButton();
            this.ExecuteTabPage = new System.Windows.Forms.TabPage();
            this.ExecuteGroupBox = new System.Windows.Forms.GroupBox();
            this.txtInitialDirectory = new System.Windows.Forms.TextBox();
            this.txtArguments = new System.Windows.Forms.TextBox();
            this.txtCommand = new System.Windows.Forms.TextBox();
            this.label13 = new System.Windows.Forms.Label();
            this.chkExecuteBeforeConnect = new System.Windows.Forms.CheckBox();
            this.label12 = new System.Windows.Forms.Label();
            this.chkWaitForExit = new System.Windows.Forms.CheckBox();
            this.label11 = new System.Windows.Forms.Label();
            this.TagsTabPage = new System.Windows.Forms.TabPage();
            this.panel4 = new System.Windows.Forms.Panel();
            this.groupBox4 = new System.Windows.Forms.GroupBox();
            this.AllTagsListView = new System.Windows.Forms.ListView();
            this.panel1 = new System.Windows.Forms.Panel();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.panel3 = new System.Windows.Forms.Panel();
            this.txtTag = new System.Windows.Forms.TextBox();
            this.label14 = new System.Windows.Forms.Label();
            this.RAStabPage = new System.Windows.Forms.TabPage();
            this.RASGroupBox = new System.Windows.Forms.GroupBox();
            this.RASDetailsListBox = new System.Windows.Forms.ListBox();
            this.ICAtabPage = new System.Windows.Forms.TabPage();
            this.IcaGroupBox = new System.Windows.Forms.GroupBox();
            this.ClientINIBrowseButton = new System.Windows.Forms.Button();
            this.ServerINIBrowseButton = new System.Windows.Forms.Button();
            this.AppWorkingFolderBrowseButton = new System.Windows.Forms.Button();
            this.appPathBrowseButton = new System.Windows.Forms.Button();
            this.ICAEncryptionLevelCombobox = new System.Windows.Forms.ComboBox();
            this.ICAEnableEncryptionCheckbox = new System.Windows.Forms.CheckBox();
            this.ICAClientINI = new System.Windows.Forms.TextBox();
            this.ICAServerINI = new System.Windows.Forms.TextBox();
            this.ICAWorkingFolder = new System.Windows.Forms.TextBox();
            this.ICAApplicationPath = new System.Windows.Forms.TextBox();
            this.ICAApplicationNameTextBox = new System.Windows.Forms.TextBox();
            this.label35 = new System.Windows.Forms.Label();
            this.label34 = new System.Windows.Forms.Label();
            this.label23 = new System.Windows.Forms.Label();
            this.label22 = new System.Windows.Forms.Label();
            this.label21 = new System.Windows.Forms.Label();
            this.VMRCtabPage = new System.Windows.Forms.TabPage();
            this.VmrcGroupBox = new System.Windows.Forms.GroupBox();
            this.VMRCReducedColorsCheckbox = new System.Windows.Forms.CheckBox();
            this.VMRCAdminModeCheckbox = new System.Windows.Forms.CheckBox();
            this.SSHTabPage = new System.Windows.Forms.TabPage();
            this.SshGroupBox = new System.Windows.Forms.GroupBox();
            this.SSHPreferences = new SSHClient.Preferences();
            this.ConsoleTabPage = new System.Windows.Forms.TabPage();
            this.ConsoleGroupBox = new System.Windows.Forms.GroupBox();
            this.consolePreferences = new Terminals.ConsolePreferences();
            this.VNCTabPage = new System.Windows.Forms.TabPage();
            this.VncGroupBox = new System.Windows.Forms.GroupBox();
            this.VncViewOnlyCheckbox = new System.Windows.Forms.CheckBox();
            this.vncDisplayNumberInput = new System.Windows.Forms.NumericUpDown();
            this.RDPTabPage = new System.Windows.Forms.TabPage();
            this.RDPSubTabPage = new System.Windows.Forms.TabControl();
            this.RDPDisplayTabPage = new System.Windows.Forms.TabPage();
            this.chkConnectToConsole = new System.Windows.Forms.CheckBox();
            this.DisplaySettingsGroupBox = new System.Windows.Forms.GroupBox();
            this.AllowDesktopCompositionCheckbox = new System.Windows.Forms.CheckBox();
            this.AllowFontSmoothingCheckbox = new System.Windows.Forms.CheckBox();
            this.customSizePanel = new System.Windows.Forms.Panel();
            this.label31 = new System.Windows.Forms.Label();
            this.label32 = new System.Windows.Forms.Label();
            this.chkDisableWallpaper = new System.Windows.Forms.CheckBox();
            this.chkDisableThemes = new System.Windows.Forms.CheckBox();
            this.chkDisableMenuAnimations = new System.Windows.Forms.CheckBox();
            this.chkDisableFullWindowDrag = new System.Windows.Forms.CheckBox();
            this.chkDisableCursorBlinking = new System.Windows.Forms.CheckBox();
            this.chkDisableCursorShadow = new System.Windows.Forms.CheckBox();
            this.cmbColors = new System.Windows.Forms.ComboBox();
            this.label7 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.RDPLocalResourcesTabPage = new System.Windows.Forms.TabPage();
            this.LocalResourceGroupBox = new System.Windows.Forms.GroupBox();
            this.btnDrives = new System.Windows.Forms.Button();
            this.cmbSounds = new System.Windows.Forms.ComboBox();
            this.chkRedirectSmartcards = new System.Windows.Forms.CheckBox();
            this.label8 = new System.Windows.Forms.Label();
            this.chkRedirectClipboard = new System.Windows.Forms.CheckBox();
            this.label9 = new System.Windows.Forms.Label();
            this.btnBrowseShare = new System.Windows.Forms.Button();
            this.chkPrinters = new System.Windows.Forms.CheckBox();
            this.chkSerialPorts = new System.Windows.Forms.CheckBox();
            this.tabPage8 = new System.Windows.Forms.TabPage();
            this.ExtendedSettingsGgroupBox = new System.Windows.Forms.GroupBox();
            this.label29 = new System.Windows.Forms.Label();
            this.label28 = new System.Windows.Forms.Label();
            this.label27 = new System.Windows.Forms.Label();
            this.label26 = new System.Windows.Forms.Label();
            this.label30 = new System.Windows.Forms.Label();
            this.tabPage9 = new System.Windows.Forms.TabPage();
            this.SecuritySettingsGroupBox = new System.Windows.Forms.GroupBox();
            this.panel2 = new System.Windows.Forms.Panel();
            this.SecurityStartFullScreenCheckbox = new System.Windows.Forms.CheckBox();
            this.label25 = new System.Windows.Forms.Label();
            this.SecurityWorkingFolderTextBox = new System.Windows.Forms.TextBox();
            this.label24 = new System.Windows.Forms.Label();
            this.SecuriytStartProgramTextbox = new System.Windows.Forms.TextBox();
            this.SecuritySettingsEnabledCheckbox = new System.Windows.Forms.CheckBox();
            this.RdpSessionTabPage = new System.Windows.Forms.TabPage();
            this.RdpTsgwTabPage = new System.Windows.Forms.TabPage();
            this.TerminalGwLoginSettingsGroupBox = new System.Windows.Forms.GroupBox();
            this.pnlTSGWlogon = new System.Windows.Forms.Panel();
            this.txtTSGWDomain = new System.Windows.Forms.TextBox();
            this.txtTSGWPassword = new System.Windows.Forms.TextBox();
            this.label18 = new System.Windows.Forms.Label();
            this.label19 = new System.Windows.Forms.Label();
            this.label16 = new System.Windows.Forms.Label();
            this.txtTSGWUserName = new System.Windows.Forms.TextBox();
            this.chkTSGWlogin = new System.Windows.Forms.CheckBox();
            this.TerminalGwSettingsGroupBox = new System.Windows.Forms.GroupBox();
            this.radTSGWenable = new System.Windows.Forms.RadioButton();
            this.radTSGWdisable = new System.Windows.Forms.RadioButton();
            this.pnlTSGWsettings = new System.Windows.Forms.Panel();
            this.label20 = new System.Windows.Forms.Label();
            this.cmbTSGWLogonMethod = new System.Windows.Forms.ComboBox();
            this.chkTSGWlocalBypass = new System.Windows.Forms.CheckBox();
            this.label17 = new System.Windows.Forms.Label();
            this.txtTSGWServer = new System.Windows.Forms.TextBox();
            this.GeneralTabPage = new System.Windows.Forms.TabPage();
            this.GeneralGroupBox = new System.Windows.Forms.GroupBox();
            this.httpUrlTextBox = new System.Windows.Forms.TextBox();
            this.NotesTextbox = new System.Windows.Forms.TextBox();
            this.label15 = new System.Windows.Forms.Label();
            this.CredentialDropdown = new System.Windows.Forms.ComboBox();
            this.CredentialsPanel = new System.Windows.Forms.Panel();
            this.cmbUsers = new System.Windows.Forms.ComboBox();
            this.label1 = new System.Windows.Forms.Label();
            this.cmbDomains = new System.Windows.Forms.ComboBox();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.txtPassword = new System.Windows.Forms.TextBox();
            this.chkSavePassword = new System.Windows.Forms.CheckBox();
            this.label36 = new System.Windows.Forms.Label();
            this.lblPort = new System.Windows.Forms.Label();
            this.ProtocolLabel = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.lblServerName = new System.Windows.Forms.Label();
            this.tabControl1 = new System.Windows.Forms.TabControl();
            ((System.ComponentModel.ISupportInitialize)(this.heightUpDown)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.widthUpDown)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.CredentialManagerPicturebox)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.contextMenuStripDefaults.SuspendLayout();
            this.contextMenuStripSave.SuspendLayout();
            this.groupBox7.SuspendLayout();
            this.ExecuteTabPage.SuspendLayout();
            this.ExecuteGroupBox.SuspendLayout();
            this.TagsTabPage.SuspendLayout();
            this.panel4.SuspendLayout();
            this.groupBox4.SuspendLayout();
            this.panel1.SuspendLayout();
            this.groupBox3.SuspendLayout();
            this.panel3.SuspendLayout();
            this.RAStabPage.SuspendLayout();
            this.RASGroupBox.SuspendLayout();
            this.ICAtabPage.SuspendLayout();
            this.IcaGroupBox.SuspendLayout();
            this.VMRCtabPage.SuspendLayout();
            this.VmrcGroupBox.SuspendLayout();
            this.SSHTabPage.SuspendLayout();
            this.SshGroupBox.SuspendLayout();
            this.ConsoleTabPage.SuspendLayout();
            this.ConsoleGroupBox.SuspendLayout();
            this.VNCTabPage.SuspendLayout();
            this.VncGroupBox.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.vncDisplayNumberInput)).BeginInit();
            this.RDPTabPage.SuspendLayout();
            this.RDPSubTabPage.SuspendLayout();
            this.RDPDisplayTabPage.SuspendLayout();
            this.DisplaySettingsGroupBox.SuspendLayout();
            this.customSizePanel.SuspendLayout();
            this.RDPLocalResourcesTabPage.SuspendLayout();
            this.LocalResourceGroupBox.SuspendLayout();
            this.tabPage8.SuspendLayout();
            this.ExtendedSettingsGgroupBox.SuspendLayout();
            this.tabPage9.SuspendLayout();
            this.SecuritySettingsGroupBox.SuspendLayout();
            this.panel2.SuspendLayout();
            this.RdpTsgwTabPage.SuspendLayout();
            this.TerminalGwLoginSettingsGroupBox.SuspendLayout();
            this.pnlTSGWlogon.SuspendLayout();
            this.TerminalGwSettingsGroupBox.SuspendLayout();
            this.pnlTSGWsettings.SuspendLayout();
            this.GeneralTabPage.SuspendLayout();
            this.GeneralGroupBox.SuspendLayout();
            this.CredentialsPanel.SuspendLayout();
            this.tabControl1.SuspendLayout();
            this.SuspendLayout();
            // 
            // btnCancel
            // 
            this.btnCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.Location = new System.Drawing.Point(497, 42);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(72, 24);
            this.btnCancel.TabIndex = 2;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            // 
            // chkAddtoToolbar
            // 
            this.chkAddtoToolbar.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.chkAddtoToolbar.AutoSize = true;
            this.chkAddtoToolbar.Location = new System.Drawing.Point(21, 49);
            this.chkAddtoToolbar.Name = "chkAddtoToolbar";
            this.chkAddtoToolbar.Size = new System.Drawing.Size(97, 17);
            this.chkAddtoToolbar.TabIndex = 3;
            this.chkAddtoToolbar.Text = "Add to &Toolbar";
            this.chkAddtoToolbar.UseVisualStyleBackColor = true;
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Location = new System.Drawing.Point(15, 161);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(81, 13);
            this.label10.TabIndex = 9;
            this.label10.Text = "Desktop S&hare:";
            this.toolTip1.SetToolTip(this.label10, "Enter a share on the server where files will be copied\r\nto when draging files fro" +
        "m your computer to the\r\nterminal window.");
            // 
            // txtDesktopShare
            // 
            this.txtDesktopShare.Location = new System.Drawing.Point(108, 157);
            this.txtDesktopShare.Name = "txtDesktopShare";
            this.txtDesktopShare.Size = new System.Drawing.Size(306, 21);
            this.txtDesktopShare.TabIndex = 10;
            this.toolTip1.SetToolTip(this.txtDesktopShare, "Enter a share on the server where files will be copied\r\nto when draging files fro" +
        "m your computer to the\r\nterminal window.");
            // 
            // cmbResolution
            // 
            this.cmbResolution.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbResolution.FormattingEnabled = true;
            this.cmbResolution.Items.AddRange(new object[] {
            "640x480",
            "800x600",
            "1024x768",
            "1152x864",
            "1280x1024",
            "Fit to Window",
            "Full Screen",
            "Auto Scale",
            "Custom"});
            this.cmbResolution.Location = new System.Drawing.Point(124, 18);
            this.cmbResolution.Name = "cmbResolution";
            this.cmbResolution.Size = new System.Drawing.Size(133, 21);
            this.cmbResolution.TabIndex = 1;
            this.toolTip1.SetToolTip(this.cmbResolution, resources.GetString("cmbResolution.ToolTip"));
            this.cmbResolution.SelectedIndexChanged += new System.EventHandler(this.cmbResolution_SelectedIndexChanged);
            // 
            // heightUpDown
            // 
            this.heightUpDown.Location = new System.Drawing.Point(56, 39);
            this.heightUpDown.Maximum = new decimal(new int[] {
            10000,
            0,
            0,
            0});
            this.heightUpDown.Name = "heightUpDown";
            this.heightUpDown.Size = new System.Drawing.Size(72, 21);
            this.heightUpDown.TabIndex = 13;
            this.toolTip1.SetToolTip(this.heightUpDown, "Maximum terminal window height");
            this.heightUpDown.Value = new decimal(new int[] {
            600,
            0,
            0,
            0});
            // 
            // widthUpDown
            // 
            this.widthUpDown.Location = new System.Drawing.Point(56, 12);
            this.widthUpDown.Maximum = new decimal(new int[] {
            10000,
            0,
            0,
            0});
            this.widthUpDown.Name = "widthUpDown";
            this.widthUpDown.Size = new System.Drawing.Size(72, 21);
            this.widthUpDown.TabIndex = 11;
            this.toolTip1.SetToolTip(this.widthUpDown, "Maximum terminal window width");
            this.widthUpDown.Value = new decimal(new int[] {
            800,
            0,
            0,
            0});
            // 
            // EnableNLAAuthenticationCheckbox
            // 
            this.EnableNLAAuthenticationCheckbox.AutoSize = true;
            this.EnableNLAAuthenticationCheckbox.Checked = true;
            this.EnableNLAAuthenticationCheckbox.CheckState = System.Windows.Forms.CheckState.Checked;
            this.EnableNLAAuthenticationCheckbox.Location = new System.Drawing.Point(241, 112);
            this.EnableNLAAuthenticationCheckbox.Name = "EnableNLAAuthenticationCheckbox";
            this.EnableNLAAuthenticationCheckbox.Size = new System.Drawing.Size(153, 17);
            this.EnableNLAAuthenticationCheckbox.TabIndex = 32;
            this.EnableNLAAuthenticationCheckbox.Text = "Enable NLA Authentication";
            this.toolTip1.SetToolTip(this.EnableNLAAuthenticationCheckbox, "This setting enables Network Level Authentication");
            this.EnableNLAAuthenticationCheckbox.UseVisualStyleBackColor = true;
            // 
            // EnableTLSAuthenticationCheckbox
            // 
            this.EnableTLSAuthenticationCheckbox.AutoSize = true;
            this.EnableTLSAuthenticationCheckbox.Checked = true;
            this.EnableTLSAuthenticationCheckbox.CheckState = System.Windows.Forms.CheckState.Checked;
            this.EnableTLSAuthenticationCheckbox.Location = new System.Drawing.Point(241, 89);
            this.EnableTLSAuthenticationCheckbox.Name = "EnableTLSAuthenticationCheckbox";
            this.EnableTLSAuthenticationCheckbox.Size = new System.Drawing.Size(151, 17);
            this.EnableTLSAuthenticationCheckbox.TabIndex = 31;
            this.EnableTLSAuthenticationCheckbox.Text = "Enable TLS Authentication";
            this.toolTip1.SetToolTip(this.EnableTLSAuthenticationCheckbox, "By default RDP encryption is used. This setting enables TLS Authentication.");
            this.EnableTLSAuthenticationCheckbox.UseVisualStyleBackColor = true;
            // 
            // ShutdownTimeoutTextBox
            // 
            this.ShutdownTimeoutTextBox.Location = new System.Drawing.Point(344, 213);
            this.ShutdownTimeoutTextBox.Name = "ShutdownTimeoutTextBox";
            this.ShutdownTimeoutTextBox.Size = new System.Drawing.Size(36, 21);
            this.ShutdownTimeoutTextBox.TabIndex = 40;
            this.ShutdownTimeoutTextBox.Text = "10";
            this.toolTip1.SetToolTip(this.ShutdownTimeoutTextBox, "Specifies the length of time, in seconds, to wait for the server to respond to a " +
        "disconnection request. If the server does not reply within the specified time, t" +
        "he client control disconnects.");
            // 
            // OverallTimeoutTextbox
            // 
            this.OverallTimeoutTextbox.Location = new System.Drawing.Point(344, 159);
            this.OverallTimeoutTextbox.Name = "OverallTimeoutTextbox";
            this.OverallTimeoutTextbox.Size = new System.Drawing.Size(36, 21);
            this.OverallTimeoutTextbox.TabIndex = 36;
            this.OverallTimeoutTextbox.Text = "600";
            this.toolTip1.SetToolTip(this.OverallTimeoutTextbox, "Specifies the total length of time, in seconds, that the client control waits for" +
        " a connection to complete.");
            // 
            // SingleTimeOutTextbox
            // 
            this.SingleTimeOutTextbox.Location = new System.Drawing.Point(344, 186);
            this.SingleTimeOutTextbox.Name = "SingleTimeOutTextbox";
            this.SingleTimeOutTextbox.Size = new System.Drawing.Size(36, 21);
            this.SingleTimeOutTextbox.TabIndex = 38;
            this.SingleTimeOutTextbox.Text = "600";
            this.toolTip1.SetToolTip(this.SingleTimeOutTextbox, "Specifies the maximum length of time, in seconds, that the client control waits f" +
        "or a connection to an IP address. During connection, the control may attempt to " +
        "connect to multiple IP addresses.");
            // 
            // IdleTimeoutMinutesTextBox
            // 
            this.IdleTimeoutMinutesTextBox.Location = new System.Drawing.Point(344, 136);
            this.IdleTimeoutMinutesTextBox.Name = "IdleTimeoutMinutesTextBox";
            this.IdleTimeoutMinutesTextBox.Size = new System.Drawing.Size(36, 21);
            this.IdleTimeoutMinutesTextBox.TabIndex = 34;
            this.IdleTimeoutMinutesTextBox.Text = "240";
            this.toolTip1.SetToolTip(this.IdleTimeoutMinutesTextBox, "Specifies the maximum length of time, in minutes, that the client should remain c" +
        "onnected without user input.");
            // 
            // GrabFocusOnConnectCheckbox
            // 
            this.GrabFocusOnConnectCheckbox.AutoSize = true;
            this.GrabFocusOnConnectCheckbox.Location = new System.Drawing.Point(6, 43);
            this.GrabFocusOnConnectCheckbox.Name = "GrabFocusOnConnectCheckbox";
            this.GrabFocusOnConnectCheckbox.Size = new System.Drawing.Size(138, 17);
            this.GrabFocusOnConnectCheckbox.TabIndex = 22;
            this.GrabFocusOnConnectCheckbox.Text = "Grab Focus on Connect";
            this.toolTip1.SetToolTip(this.GrabFocusOnConnectCheckbox, "Specifies whether the client control should have the focus while connecting.");
            this.GrabFocusOnConnectCheckbox.UseVisualStyleBackColor = true;
            // 
            // EnableEncryptionCheckbox
            // 
            this.EnableEncryptionCheckbox.AutoSize = true;
            this.EnableEncryptionCheckbox.Location = new System.Drawing.Point(242, 43);
            this.EnableEncryptionCheckbox.Name = "EnableEncryptionCheckbox";
            this.EnableEncryptionCheckbox.Size = new System.Drawing.Size(112, 17);
            this.EnableEncryptionCheckbox.TabIndex = 29;
            this.EnableEncryptionCheckbox.Text = "Enable Encryption";
            this.toolTip1.SetToolTip(this.EnableEncryptionCheckbox, "Reserved. You cannot disable encryption. (Funny isnt it?)");
            this.EnableEncryptionCheckbox.UseVisualStyleBackColor = true;
            // 
            // DisableWindowsKeyCheckbox
            // 
            this.DisableWindowsKeyCheckbox.AutoSize = true;
            this.DisableWindowsKeyCheckbox.Location = new System.Drawing.Point(6, 158);
            this.DisableWindowsKeyCheckbox.Name = "DisableWindowsKeyCheckbox";
            this.DisableWindowsKeyCheckbox.Size = new System.Drawing.Size(127, 17);
            this.DisableWindowsKeyCheckbox.TabIndex = 27;
            this.DisableWindowsKeyCheckbox.Text = "Disable Windows Key";
            this.toolTip1.SetToolTip(this.DisableWindowsKeyCheckbox, "Specifies whether the Windows key can be used in the remote session.");
            this.DisableWindowsKeyCheckbox.UseVisualStyleBackColor = true;
            // 
            // DetectDoubleClicksCheckbox
            // 
            this.DetectDoubleClicksCheckbox.AutoSize = true;
            this.DetectDoubleClicksCheckbox.Location = new System.Drawing.Point(6, 135);
            this.DetectDoubleClicksCheckbox.Name = "DetectDoubleClicksCheckbox";
            this.DetectDoubleClicksCheckbox.Size = new System.Drawing.Size(123, 17);
            this.DetectDoubleClicksCheckbox.TabIndex = 26;
            this.DetectDoubleClicksCheckbox.Text = "Detect Double Clicks";
            this.toolTip1.SetToolTip(this.DetectDoubleClicksCheckbox, "Specifies whether the client identifies double-clicks for the server.");
            this.DetectDoubleClicksCheckbox.UseVisualStyleBackColor = true;
            // 
            // DisplayConnectionBarCheckbox
            // 
            this.DisplayConnectionBarCheckbox.AutoSize = true;
            this.DisplayConnectionBarCheckbox.Location = new System.Drawing.Point(6, 112);
            this.DisplayConnectionBarCheckbox.Name = "DisplayConnectionBarCheckbox";
            this.DisplayConnectionBarCheckbox.Size = new System.Drawing.Size(136, 17);
            this.DisplayConnectionBarCheckbox.TabIndex = 25;
            this.DisplayConnectionBarCheckbox.Text = "Display Connection Bar";
            this.toolTip1.SetToolTip(this.DisplayConnectionBarCheckbox, "Specifies whether to use the connection bar.");
            this.DisplayConnectionBarCheckbox.UseVisualStyleBackColor = true;
            // 
            // DisableControlAltDeleteCheckbox
            // 
            this.DisableControlAltDeleteCheckbox.AutoSize = true;
            this.DisableControlAltDeleteCheckbox.Location = new System.Drawing.Point(6, 89);
            this.DisableControlAltDeleteCheckbox.Name = "DisableControlAltDeleteCheckbox";
            this.DisableControlAltDeleteCheckbox.Size = new System.Drawing.Size(148, 17);
            this.DisableControlAltDeleteCheckbox.TabIndex = 24;
            this.DisableControlAltDeleteCheckbox.Text = "Disable Control Alt Delete";
            this.toolTip1.SetToolTip(this.DisableControlAltDeleteCheckbox, "This method sets a value that indicates whether the initial prompt screen in Winl" +
        "ogon, which prompts you to unlock your computer, should appear.");
            this.DisableControlAltDeleteCheckbox.UseVisualStyleBackColor = true;
            // 
            // AcceleratorPassthroughCheckBox
            // 
            this.AcceleratorPassthroughCheckBox.AutoSize = true;
            this.AcceleratorPassthroughCheckBox.Location = new System.Drawing.Point(6, 66);
            this.AcceleratorPassthroughCheckBox.Name = "AcceleratorPassthroughCheckBox";
            this.AcceleratorPassthroughCheckBox.Size = new System.Drawing.Size(231, 17);
            this.AcceleratorPassthroughCheckBox.TabIndex = 23;
            this.AcceleratorPassthroughCheckBox.Text = "Enable Keyboard Accelerator Passthrough ";
            this.toolTip1.SetToolTip(this.AcceleratorPassthroughCheckBox, resources.GetString("AcceleratorPassthroughCheckBox.ToolTip"));
            this.AcceleratorPassthroughCheckBox.UseVisualStyleBackColor = true;
            // 
            // EnableCompressionCheckbox
            // 
            this.EnableCompressionCheckbox.AutoSize = true;
            this.EnableCompressionCheckbox.Location = new System.Drawing.Point(242, 20);
            this.EnableCompressionCheckbox.Name = "EnableCompressionCheckbox";
            this.EnableCompressionCheckbox.Size = new System.Drawing.Size(122, 17);
            this.EnableCompressionCheckbox.TabIndex = 28;
            this.EnableCompressionCheckbox.Text = "Enable Compression";
            this.toolTip1.SetToolTip(this.EnableCompressionCheckbox, "This method sets the value of the Compression property, which enables or disables" +
        " compression.");
            this.EnableCompressionCheckbox.UseVisualStyleBackColor = true;
            // 
            // EnableBitmapPersistenceCheckbox
            // 
            this.EnableBitmapPersistenceCheckbox.AutoSize = true;
            this.EnableBitmapPersistenceCheckbox.Location = new System.Drawing.Point(241, 66);
            this.EnableBitmapPersistenceCheckbox.Name = "EnableBitmapPersistenceCheckbox";
            this.EnableBitmapPersistenceCheckbox.Size = new System.Drawing.Size(151, 17);
            this.EnableBitmapPersistenceCheckbox.TabIndex = 30;
            this.EnableBitmapPersistenceCheckbox.Text = "Enable Bitmap Persistence";
            this.toolTip1.SetToolTip(this.EnableBitmapPersistenceCheckbox, resources.GetString("EnableBitmapPersistenceCheckbox.ToolTip"));
            this.EnableBitmapPersistenceCheckbox.UseVisualStyleBackColor = true;
            // 
            // AllowBackgroundInputCheckBox
            // 
            this.AllowBackgroundInputCheckBox.AutoSize = true;
            this.AllowBackgroundInputCheckBox.Location = new System.Drawing.Point(6, 20);
            this.AllowBackgroundInputCheckBox.Name = "AllowBackgroundInputCheckBox";
            this.AllowBackgroundInputCheckBox.Size = new System.Drawing.Size(139, 17);
            this.AllowBackgroundInputCheckBox.TabIndex = 21;
            this.AllowBackgroundInputCheckBox.Text = "Allow Background Input";
            this.toolTip1.SetToolTip(this.AllowBackgroundInputCheckBox, "Specifies whether background input mode is enabled. When background input is enab" +
        "led the client can accept input when the client does not have focus.");
            this.AllowBackgroundInputCheckBox.UseVisualStyleBackColor = true;
            // 
            // label37
            // 
            this.label37.AutoSize = true;
            this.label37.Location = new System.Drawing.Point(3, 257);
            this.label37.Name = "label37";
            this.label37.Size = new System.Drawing.Size(85, 13);
            this.label37.TabIndex = 7;
            this.label37.Text = "Display Number:";
            this.toolTip1.SetToolTip(this.label37, "The Display number (used on Unix hosts).");
            // 
            // vncAutoScaleCheckbox
            // 
            this.vncAutoScaleCheckbox.AutoSize = true;
            this.vncAutoScaleCheckbox.Location = new System.Drawing.Point(6, 20);
            this.vncAutoScaleCheckbox.Name = "vncAutoScaleCheckbox";
            this.vncAutoScaleCheckbox.Size = new System.Drawing.Size(119, 17);
            this.vncAutoScaleCheckbox.TabIndex = 6;
            this.vncAutoScaleCheckbox.Text = "Auto Scale Desktop";
            this.toolTip1.SetToolTip(this.vncAutoScaleCheckbox, "Switch between clipped and scaled desktop.");
            this.vncAutoScaleCheckbox.UseVisualStyleBackColor = true;
            // 
            // txtPort
            // 
            this.txtPort.Location = new System.Drawing.Point(415, 43);
            this.txtPort.Name = "txtPort";
            this.txtPort.Size = new System.Drawing.Size(46, 21);
            this.txtPort.TabIndex = 26;
            this.toolTip1.SetToolTip(this.txtPort, "Set the service port number.\r\nIf not defined, than selected service port number i" +
        "s used.");
            // 
            // txtName
            // 
            this.txtName.Location = new System.Drawing.Point(105, 70);
            this.txtName.Name = "txtName";
            this.txtName.Size = new System.Drawing.Size(334, 21);
            this.txtName.TabIndex = 28;
            this.toolTip1.SetToolTip(this.txtName, "Name your connection visible in shortcuts tree.\r\nIf you define already saved valu" +
        "e, the existing configuration will be overwritten.");
            // 
            // CredentialManagerPicturebox
            // 
            this.CredentialManagerPicturebox.Image = global::Terminals.Properties.Resources.computer_security;
            this.CredentialManagerPicturebox.Location = new System.Drawing.Point(445, 96);
            this.CredentialManagerPicturebox.Name = "CredentialManagerPicturebox";
            this.CredentialManagerPicturebox.Size = new System.Drawing.Size(16, 16);
            this.CredentialManagerPicturebox.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.CredentialManagerPicturebox.TabIndex = 37;
            this.CredentialManagerPicturebox.TabStop = false;
            this.toolTip1.SetToolTip(this.CredentialManagerPicturebox, "Open the Credential Manager window to manage your stored passwords.");
            this.CredentialManagerPicturebox.Click += new System.EventHandler(this.CredentialManagerPicturebox_Click);
            // 
            // pictureBox2
            // 
            this.pictureBox2.Image = global::Terminals.Properties.Resources.terminalsicon;
            this.pictureBox2.InitialImage = global::Terminals.Properties.Resources.smallterm;
            this.pictureBox2.Location = new System.Drawing.Point(445, 70);
            this.pictureBox2.Name = "pictureBox2";
            this.pictureBox2.Size = new System.Drawing.Size(16, 16);
            this.pictureBox2.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pictureBox2.TabIndex = 36;
            this.pictureBox2.TabStop = false;
            this.toolTip1.SetToolTip(this.pictureBox2, "Select your custom icon for this shortcut using custom image, \r\nwhich will be sho" +
        "wn in the shortcuts menu.");
            this.pictureBox2.Click += new System.EventHandler(this.pictureBox2_Click);
            // 
            // ProtocolComboBox
            // 
            this.ProtocolComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.ProtocolComboBox.FormattingEnabled = true;
            this.ProtocolComboBox.Items.AddRange(new object[] {
            "RDP",
            "VNC",
            "VMRC",
            "SSH",
            "Telnet",
            "RAS",
            "ICA Citrix",
            "HTTP",
            "HTTPS"});
            this.ProtocolComboBox.Location = new System.Drawing.Point(105, 14);
            this.ProtocolComboBox.MaxDropDownItems = 10;
            this.ProtocolComboBox.Name = "ProtocolComboBox";
            this.ProtocolComboBox.Size = new System.Drawing.Size(356, 21);
            this.ProtocolComboBox.TabIndex = 35;
            this.toolTip1.SetToolTip(this.ProtocolComboBox, "Selecting the service provider also enables/disables the configuration options av" +
        "ailable on other pages.");
            this.ProtocolComboBox.SelectedIndexChanged += new System.EventHandler(this.ProtocolComboBox_SelectedIndexChanged);
            // 
            // cmbServers
            // 
            this.cmbServers.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.Suggest;
            this.cmbServers.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
            this.cmbServers.Location = new System.Drawing.Point(105, 43);
            this.cmbServers.Name = "cmbServers";
            this.cmbServers.Size = new System.Drawing.Size(265, 21);
            this.cmbServers.TabIndex = 24;
            this.toolTip1.SetToolTip(this.cmbServers, "Here you can define the IP address of the server or its host name.");
            this.cmbServers.SelectedIndexChanged += new System.EventHandler(this.cmbServers_SelectedIndexChanged);
            this.cmbServers.TextChanged += new System.EventHandler(this.cmbServers_TextChanged);
            this.cmbServers.Leave += new System.EventHandler(this.cmbServers_Leave);
            // 
            // btnAddNewTag
            // 
            this.btnAddNewTag.Image = global::Terminals.Properties.Resources.tag_blue_add;
            this.btnAddNewTag.Location = new System.Drawing.Point(421, 18);
            this.btnAddNewTag.Name = "btnAddNewTag";
            this.btnAddNewTag.Size = new System.Drawing.Size(21, 21);
            this.btnAddNewTag.TabIndex = 2;
            this.toolTip1.SetToolTip(this.btnAddNewTag, "Add new Tag to Connection Tags list.\r\n");
            this.btnAddNewTag.UseVisualStyleBackColor = true;
            this.btnAddNewTag.Click += new System.EventHandler(this.btnAddNewTag_Click);
            // 
            // AllTagsAddButton
            // 
            this.AllTagsAddButton.Image = global::Terminals.Properties.Resources.tag_blue_add;
            this.AllTagsAddButton.Location = new System.Drawing.Point(421, 20);
            this.AllTagsAddButton.Name = "AllTagsAddButton";
            this.AllTagsAddButton.Size = new System.Drawing.Size(21, 21);
            this.AllTagsAddButton.TabIndex = 1;
            this.toolTip1.SetToolTip(this.AllTagsAddButton, "Add Tag to Connection");
            this.AllTagsAddButton.UseVisualStyleBackColor = true;
            this.AllTagsAddButton.Click += new System.EventHandler(this.AllTagsAddButton_Click);
            // 
            // lvConnectionTags
            // 
            this.lvConnectionTags.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lvConnectionTags.HideSelection = false;
            this.lvConnectionTags.Location = new System.Drawing.Point(8, 24);
            this.lvConnectionTags.Name = "lvConnectionTags";
            this.lvConnectionTags.Size = new System.Drawing.Size(394, 66);
            this.lvConnectionTags.TabIndex = 0;
            this.toolTip1.SetToolTip(this.lvConnectionTags, "Tags listed here define groups in which this connection will appear in connection" +
        " shortcuts tree.");
            this.lvConnectionTags.UseCompatibleStateImageBehavior = false;
            this.lvConnectionTags.View = System.Windows.Forms.View.List;
            this.lvConnectionTags.DoubleClick += new System.EventHandler(this.lvConnectionTags_DoubleClick);
            // 
            // btnRemoveTag
            // 
            this.btnRemoveTag.Image = global::Terminals.Properties.Resources.tag_blue_delete;
            this.btnRemoveTag.Location = new System.Drawing.Point(421, 24);
            this.btnRemoveTag.Name = "btnRemoveTag";
            this.btnRemoveTag.Size = new System.Drawing.Size(21, 21);
            this.btnRemoveTag.TabIndex = 1;
            this.toolTip1.SetToolTip(this.btnRemoveTag, "Remove Connection Tag");
            this.btnRemoveTag.UseVisualStyleBackColor = true;
            this.btnRemoveTag.Click += new System.EventHandler(this.btnRemoveTag_Click);
            // 
            // pictureBox1
            // 
            this.pictureBox1.BackColor = System.Drawing.Color.White;
            this.pictureBox1.Dock = System.Windows.Forms.DockStyle.Left;
            this.pictureBox1.Image = global::Terminals.Properties.Resources.terminalsbanner_left;
            this.pictureBox1.Location = new System.Drawing.Point(0, 0);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(84, 396);
            this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pictureBox1.TabIndex = 9;
            this.pictureBox1.TabStop = false;
            // 
            // ras1
            // 
            this.ras1.CallBackNumber = null;
            this.ras1.Domain = null;
            this.ras1.EntryName = null;
            this.ras1.Password = null;
            this.ras1.PhoneNumber = null;
            this.ras1.UserName = null;
            // 
            // NewWindowCheckbox
            // 
            this.NewWindowCheckbox.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.NewWindowCheckbox.AutoSize = true;
            this.NewWindowCheckbox.Location = new System.Drawing.Point(21, 22);
            this.NewWindowCheckbox.Name = "NewWindowCheckbox";
            this.NewWindowCheckbox.Size = new System.Drawing.Size(128, 17);
            this.NewWindowCheckbox.TabIndex = 4;
            this.NewWindowCheckbox.Text = "&Open in New Window";
            this.NewWindowCheckbox.UseVisualStyleBackColor = true;
            // 
            // textBox3
            // 
            this.textBox3.Location = new System.Drawing.Point(154, 132);
            this.textBox3.Name = "textBox3";
            this.textBox3.ReadOnly = true;
            this.textBox3.Size = new System.Drawing.Size(160, 20);
            this.textBox3.TabIndex = 10;
            this.textBox3.Text = "Black";
            // 
            // label43
            // 
            this.label43.AutoSize = true;
            this.label43.Location = new System.Drawing.Point(101, 108);
            this.label43.Name = "label43";
            this.label43.Size = new System.Drawing.Size(31, 13);
            this.label43.TabIndex = 8;
            this.label43.Text = "Font:";
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(320, 130);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(31, 23);
            this.button1.TabIndex = 9;
            this.button1.Text = "...";
            this.button1.UseVisualStyleBackColor = true;
            // 
            // contextMenuStripDefaults
            // 
            this.contextMenuStripDefaults.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.contextMenuStripDefaults.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.saveCurrentSettingsAsDefaultToolStripMenuItem,
            this.removeSavedDefaultsToolStripMenuItem});
            this.contextMenuStripDefaults.Name = "contextMenuStripDefaults";
            this.contextMenuStripDefaults.RenderMode = System.Windows.Forms.ToolStripRenderMode.System;
            this.contextMenuStripDefaults.ShowImageMargin = false;
            this.contextMenuStripDefaults.Size = new System.Drawing.Size(208, 48);
            // 
            // saveCurrentSettingsAsDefaultToolStripMenuItem
            // 
            this.saveCurrentSettingsAsDefaultToolStripMenuItem.Name = "saveCurrentSettingsAsDefaultToolStripMenuItem";
            this.saveCurrentSettingsAsDefaultToolStripMenuItem.Size = new System.Drawing.Size(207, 22);
            this.saveCurrentSettingsAsDefaultToolStripMenuItem.Text = "Save Current Settings as Default";
            this.saveCurrentSettingsAsDefaultToolStripMenuItem.Click += new System.EventHandler(this.saveCurrentSettingsAsDefaultToolStripMenuItem_Click);
            // 
            // removeSavedDefaultsToolStripMenuItem
            // 
            this.removeSavedDefaultsToolStripMenuItem.Name = "removeSavedDefaultsToolStripMenuItem";
            this.removeSavedDefaultsToolStripMenuItem.Size = new System.Drawing.Size(207, 22);
            this.removeSavedDefaultsToolStripMenuItem.Text = "Remove Saved Defaults";
            this.removeSavedDefaultsToolStripMenuItem.Click += new System.EventHandler(this.removeSavedDefaultsToolStripMenuItem_Click);
            // 
            // contextMenuStripSave
            // 
            this.contextMenuStripSave.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.contextMenuStripSave.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.saveConnectToolStripMenuItem,
            this.saveNewToolStripMenuItem,
            this.saveCopyToolStripMenuItem});
            this.contextMenuStripSave.Name = "contextMenuStripSave";
            this.contextMenuStripSave.RenderMode = System.Windows.Forms.ToolStripRenderMode.System;
            this.contextMenuStripSave.Size = new System.Drawing.Size(152, 70);
            // 
            // saveConnectToolStripMenuItem
            // 
            this.saveConnectToolStripMenuItem.Name = "saveConnectToolStripMenuItem";
            this.saveConnectToolStripMenuItem.Size = new System.Drawing.Size(151, 22);
            this.saveConnectToolStripMenuItem.Text = "Save && Connect";
            this.saveConnectToolStripMenuItem.Click += new System.EventHandler(this.saveConnectToolStripMenuItem_Click);
            // 
            // saveNewToolStripMenuItem
            // 
            this.saveNewToolStripMenuItem.Name = "saveNewToolStripMenuItem";
            this.saveNewToolStripMenuItem.Size = new System.Drawing.Size(151, 22);
            this.saveNewToolStripMenuItem.Text = "Save && New";
            this.saveNewToolStripMenuItem.Click += new System.EventHandler(this.saveNewToolStripMenuItem_Click);
            // 
            // saveCopyToolStripMenuItem
            // 
            this.saveCopyToolStripMenuItem.Name = "saveCopyToolStripMenuItem";
            this.saveCopyToolStripMenuItem.Size = new System.Drawing.Size(151, 22);
            this.saveCopyToolStripMenuItem.Text = "Save && Copy";
            this.saveCopyToolStripMenuItem.Click += new System.EventHandler(this.saveCopyToolStripMenuItem_Click);
            // 
            // groupBox7
            // 
            this.groupBox7.BackColor = System.Drawing.Color.White;
            this.groupBox7.Controls.Add(this.btnSaveDefault);
            this.groupBox7.Controls.Add(this.btnSave);
            this.groupBox7.Controls.Add(this.btnCancel);
            this.groupBox7.Controls.Add(this.chkAddtoToolbar);
            this.groupBox7.Controls.Add(this.NewWindowCheckbox);
            this.groupBox7.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.groupBox7.Location = new System.Drawing.Point(0, 396);
            this.groupBox7.Name = "groupBox7";
            this.groupBox7.Size = new System.Drawing.Size(579, 78);
            this.groupBox7.TabIndex = 10;
            this.groupBox7.TabStop = false;
            // 
            // btnSaveDefault
            // 
            this.btnSaveDefault.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnSaveDefault.AutoSize = true;
            this.btnSaveDefault.ContextMenuStrip = this.contextMenuStripDefaults;
            this.btnSaveDefault.Location = new System.Drawing.Point(277, 41);
            this.btnSaveDefault.Name = "btnSaveDefault";
            this.btnSaveDefault.Size = new System.Drawing.Size(88, 27);
            this.btnSaveDefault.SplitMenuStrip = this.contextMenuStripDefaults;
            this.btnSaveDefault.TabIndex = 5;
            this.btnSaveDefault.Text = "Defaults";
            this.btnSaveDefault.UseVisualStyleBackColor = true;
            this.btnSaveDefault.Click += new System.EventHandler(this.btnSaveDefault_Click);
            // 
            // btnSave
            // 
            this.btnSave.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnSave.AutoSize = true;
            this.btnSave.ContextMenuStrip = this.contextMenuStripSave;
            this.btnSave.Location = new System.Drawing.Point(371, 41);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(120, 27);
            this.btnSave.SplitMenuStrip = this.contextMenuStripSave;
            this.btnSave.TabIndex = 1;
            this.btnSave.Text = "Save && Close";
            this.btnSave.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnSave.UseVisualStyleBackColor = true;
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // ExecuteTabPage
            // 
            this.ExecuteTabPage.Controls.Add(this.ExecuteGroupBox);
            this.ExecuteTabPage.Location = new System.Drawing.Point(4, 22);
            this.ExecuteTabPage.Name = "ExecuteTabPage";
            this.ExecuteTabPage.Padding = new System.Windows.Forms.Padding(3);
            this.ExecuteTabPage.Size = new System.Drawing.Size(487, 370);
            this.ExecuteTabPage.TabIndex = 3;
            this.ExecuteTabPage.Text = "Execute";
            this.ExecuteTabPage.UseVisualStyleBackColor = true;
            // 
            // ExecuteGroupBox
            // 
            this.ExecuteGroupBox.Controls.Add(this.txtInitialDirectory);
            this.ExecuteGroupBox.Controls.Add(this.txtArguments);
            this.ExecuteGroupBox.Controls.Add(this.txtCommand);
            this.ExecuteGroupBox.Controls.Add(this.label13);
            this.ExecuteGroupBox.Controls.Add(this.chkExecuteBeforeConnect);
            this.ExecuteGroupBox.Controls.Add(this.label12);
            this.ExecuteGroupBox.Controls.Add(this.chkWaitForExit);
            this.ExecuteGroupBox.Controls.Add(this.label11);
            this.ExecuteGroupBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ExecuteGroupBox.Location = new System.Drawing.Point(3, 3);
            this.ExecuteGroupBox.Name = "ExecuteGroupBox";
            this.ExecuteGroupBox.Size = new System.Drawing.Size(481, 364);
            this.ExecuteGroupBox.TabIndex = 0;
            this.ExecuteGroupBox.TabStop = false;
            // 
            // txtInitialDirectory
            // 
            this.txtInitialDirectory.Location = new System.Drawing.Point(102, 100);
            this.txtInitialDirectory.Name = "txtInitialDirectory";
            this.txtInitialDirectory.Size = new System.Drawing.Size(265, 21);
            this.txtInitialDirectory.TabIndex = 14;
            // 
            // txtArguments
            // 
            this.txtArguments.Location = new System.Drawing.Point(102, 76);
            this.txtArguments.Name = "txtArguments";
            this.txtArguments.Size = new System.Drawing.Size(265, 21);
            this.txtArguments.TabIndex = 13;
            // 
            // txtCommand
            // 
            this.txtCommand.Location = new System.Drawing.Point(102, 52);
            this.txtCommand.Name = "txtCommand";
            this.txtCommand.Size = new System.Drawing.Size(265, 21);
            this.txtCommand.TabIndex = 12;
            // 
            // label13
            // 
            this.label13.AutoSize = true;
            this.label13.Location = new System.Drawing.Point(6, 100);
            this.label13.Name = "label13";
            this.label13.Size = new System.Drawing.Size(84, 13);
            this.label13.TabIndex = 18;
            this.label13.Text = "Initial Directory:";
            // 
            // chkExecuteBeforeConnect
            // 
            this.chkExecuteBeforeConnect.AutoSize = true;
            this.chkExecuteBeforeConnect.Location = new System.Drawing.Point(6, 20);
            this.chkExecuteBeforeConnect.Name = "chkExecuteBeforeConnect";
            this.chkExecuteBeforeConnect.Size = new System.Drawing.Size(141, 17);
            this.chkExecuteBeforeConnect.TabIndex = 11;
            this.chkExecuteBeforeConnect.Text = "&Execute before connect";
            this.chkExecuteBeforeConnect.UseVisualStyleBackColor = true;
            // 
            // label12
            // 
            this.label12.AutoSize = true;
            this.label12.Location = new System.Drawing.Point(6, 76);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(59, 13);
            this.label12.TabIndex = 17;
            this.label12.Text = "Arguments";
            // 
            // chkWaitForExit
            // 
            this.chkWaitForExit.AutoSize = true;
            this.chkWaitForExit.Location = new System.Drawing.Point(6, 132);
            this.chkWaitForExit.Name = "chkWaitForExit";
            this.chkWaitForExit.Size = new System.Drawing.Size(86, 17);
            this.chkWaitForExit.TabIndex = 15;
            this.chkWaitForExit.Text = "&Wait for exit";
            this.chkWaitForExit.UseVisualStyleBackColor = true;
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Location = new System.Drawing.Point(6, 52);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(58, 13);
            this.label11.TabIndex = 16;
            this.label11.Text = "Command:";
            // 
            // TagsTabPage
            // 
            this.TagsTabPage.Controls.Add(this.panel4);
            this.TagsTabPage.Controls.Add(this.panel1);
            this.TagsTabPage.Controls.Add(this.panel3);
            this.TagsTabPage.Location = new System.Drawing.Point(4, 22);
            this.TagsTabPage.Name = "TagsTabPage";
            this.TagsTabPage.Padding = new System.Windows.Forms.Padding(3);
            this.TagsTabPage.Size = new System.Drawing.Size(487, 370);
            this.TagsTabPage.TabIndex = 4;
            this.TagsTabPage.Text = "Tags";
            this.TagsTabPage.UseVisualStyleBackColor = true;
            // 
            // panel4
            // 
            this.panel4.Controls.Add(this.groupBox4);
            this.panel4.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel4.Location = new System.Drawing.Point(3, 149);
            this.panel4.Name = "panel4";
            this.panel4.Size = new System.Drawing.Size(481, 218);
            this.panel4.TabIndex = 14;
            // 
            // groupBox4
            // 
            this.groupBox4.Controls.Add(this.AllTagsAddButton);
            this.groupBox4.Controls.Add(this.AllTagsListView);
            this.groupBox4.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBox4.Location = new System.Drawing.Point(0, 0);
            this.groupBox4.Name = "groupBox4";
            this.groupBox4.Size = new System.Drawing.Size(481, 218);
            this.groupBox4.TabIndex = 0;
            this.groupBox4.TabStop = false;
            this.groupBox4.Text = "All Available Tags";
            // 
            // AllTagsListView
            // 
            this.AllTagsListView.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.AllTagsListView.HideSelection = false;
            this.AllTagsListView.Location = new System.Drawing.Point(8, 20);
            this.AllTagsListView.Name = "AllTagsListView";
            this.AllTagsListView.Size = new System.Drawing.Size(394, 175);
            this.AllTagsListView.TabIndex = 0;
            this.AllTagsListView.UseCompatibleStateImageBehavior = false;
            this.AllTagsListView.View = System.Windows.Forms.View.List;
            this.AllTagsListView.DoubleClick += new System.EventHandler(this.AllTagsListView_DoubleClick);
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.groupBox3);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel1.Location = new System.Drawing.Point(3, 53);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(481, 96);
            this.panel1.TabIndex = 0;
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.btnRemoveTag);
            this.groupBox3.Controls.Add(this.lvConnectionTags);
            this.groupBox3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBox3.Location = new System.Drawing.Point(0, 0);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(481, 96);
            this.groupBox3.TabIndex = 0;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "Connection Tags";
            // 
            // panel3
            // 
            this.panel3.Controls.Add(this.txtTag);
            this.panel3.Controls.Add(this.btnAddNewTag);
            this.panel3.Controls.Add(this.label14);
            this.panel3.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel3.Location = new System.Drawing.Point(3, 3);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(481, 50);
            this.panel3.TabIndex = 0;
            // 
            // txtTag
            // 
            this.txtTag.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend;
            this.txtTag.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.CustomSource;
            this.txtTag.Location = new System.Drawing.Point(70, 19);
            this.txtTag.Name = "txtTag";
            this.txtTag.Size = new System.Drawing.Size(332, 21);
            this.txtTag.TabIndex = 1;
            // 
            // label14
            // 
            this.label14.AutoSize = true;
            this.label14.Location = new System.Drawing.Point(11, 19);
            this.label14.Name = "label14";
            this.label14.Size = new System.Drawing.Size(53, 13);
            this.label14.TabIndex = 0;
            this.label14.Text = "New Tag:";
            // 
            // RAStabPage
            // 
            this.RAStabPage.Controls.Add(this.RASGroupBox);
            this.RAStabPage.Location = new System.Drawing.Point(4, 22);
            this.RAStabPage.Name = "RAStabPage";
            this.RAStabPage.Padding = new System.Windows.Forms.Padding(3);
            this.RAStabPage.Size = new System.Drawing.Size(487, 370);
            this.RAStabPage.TabIndex = 9;
            this.RAStabPage.Text = "RAS";
            this.RAStabPage.UseVisualStyleBackColor = true;
            // 
            // RASGroupBox
            // 
            this.RASGroupBox.Controls.Add(this.RASDetailsListBox);
            this.RASGroupBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.RASGroupBox.Location = new System.Drawing.Point(3, 3);
            this.RASGroupBox.Name = "RASGroupBox";
            this.RASGroupBox.Size = new System.Drawing.Size(481, 364);
            this.RASGroupBox.TabIndex = 0;
            this.RASGroupBox.TabStop = false;
            // 
            // RASDetailsListBox
            // 
            this.RASDetailsListBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.RASDetailsListBox.FormattingEnabled = true;
            this.RASDetailsListBox.Location = new System.Drawing.Point(3, 17);
            this.RASDetailsListBox.Name = "RASDetailsListBox";
            this.RASDetailsListBox.Size = new System.Drawing.Size(475, 344);
            this.RASDetailsListBox.TabIndex = 0;
            // 
            // ICAtabPage
            // 
            this.ICAtabPage.Controls.Add(this.IcaGroupBox);
            this.ICAtabPage.Location = new System.Drawing.Point(4, 22);
            this.ICAtabPage.Name = "ICAtabPage";
            this.ICAtabPage.Padding = new System.Windows.Forms.Padding(3);
            this.ICAtabPage.Size = new System.Drawing.Size(487, 370);
            this.ICAtabPage.TabIndex = 10;
            this.ICAtabPage.Text = "Citrix";
            this.ICAtabPage.UseVisualStyleBackColor = true;
            // 
            // IcaGroupBox
            // 
            this.IcaGroupBox.Controls.Add(this.ClientINIBrowseButton);
            this.IcaGroupBox.Controls.Add(this.ServerINIBrowseButton);
            this.IcaGroupBox.Controls.Add(this.AppWorkingFolderBrowseButton);
            this.IcaGroupBox.Controls.Add(this.appPathBrowseButton);
            this.IcaGroupBox.Controls.Add(this.ICAEncryptionLevelCombobox);
            this.IcaGroupBox.Controls.Add(this.ICAEnableEncryptionCheckbox);
            this.IcaGroupBox.Controls.Add(this.ICAClientINI);
            this.IcaGroupBox.Controls.Add(this.ICAServerINI);
            this.IcaGroupBox.Controls.Add(this.ICAWorkingFolder);
            this.IcaGroupBox.Controls.Add(this.ICAApplicationPath);
            this.IcaGroupBox.Controls.Add(this.ICAApplicationNameTextBox);
            this.IcaGroupBox.Controls.Add(this.label35);
            this.IcaGroupBox.Controls.Add(this.label34);
            this.IcaGroupBox.Controls.Add(this.label23);
            this.IcaGroupBox.Controls.Add(this.label22);
            this.IcaGroupBox.Controls.Add(this.label21);
            this.IcaGroupBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.IcaGroupBox.Location = new System.Drawing.Point(3, 3);
            this.IcaGroupBox.Name = "IcaGroupBox";
            this.IcaGroupBox.Size = new System.Drawing.Size(481, 364);
            this.IcaGroupBox.TabIndex = 0;
            this.IcaGroupBox.TabStop = false;
            // 
            // ClientINIBrowseButton
            // 
            this.ClientINIBrowseButton.Location = new System.Drawing.Point(380, 120);
            this.ClientINIBrowseButton.Name = "ClientINIBrowseButton";
            this.ClientINIBrowseButton.Size = new System.Drawing.Size(28, 23);
            this.ClientINIBrowseButton.TabIndex = 31;
            this.ClientINIBrowseButton.Text = "...";
            this.ClientINIBrowseButton.UseVisualStyleBackColor = true;
            this.ClientINIBrowseButton.Click += new System.EventHandler(this.ClientINIBrowseButton_Click);
            // 
            // ServerINIBrowseButton
            // 
            this.ServerINIBrowseButton.Location = new System.Drawing.Point(380, 93);
            this.ServerINIBrowseButton.Name = "ServerINIBrowseButton";
            this.ServerINIBrowseButton.Size = new System.Drawing.Size(28, 23);
            this.ServerINIBrowseButton.TabIndex = 30;
            this.ServerINIBrowseButton.Text = "...";
            this.ServerINIBrowseButton.UseVisualStyleBackColor = true;
            this.ServerINIBrowseButton.Click += new System.EventHandler(this.ServerINIBrowseButton_Click);
            // 
            // AppWorkingFolderBrowseButton
            // 
            this.AppWorkingFolderBrowseButton.Location = new System.Drawing.Point(380, 66);
            this.AppWorkingFolderBrowseButton.Name = "AppWorkingFolderBrowseButton";
            this.AppWorkingFolderBrowseButton.Size = new System.Drawing.Size(28, 23);
            this.AppWorkingFolderBrowseButton.TabIndex = 29;
            this.AppWorkingFolderBrowseButton.Text = "...";
            this.AppWorkingFolderBrowseButton.UseVisualStyleBackColor = true;
            this.AppWorkingFolderBrowseButton.Click += new System.EventHandler(this.AppWorkingFolderBrowseButton_Click);
            // 
            // appPathBrowseButton
            // 
            this.appPathBrowseButton.Location = new System.Drawing.Point(380, 39);
            this.appPathBrowseButton.Name = "appPathBrowseButton";
            this.appPathBrowseButton.Size = new System.Drawing.Size(28, 23);
            this.appPathBrowseButton.TabIndex = 28;
            this.appPathBrowseButton.Text = "...";
            this.appPathBrowseButton.UseVisualStyleBackColor = true;
            this.appPathBrowseButton.Click += new System.EventHandler(this.appPathBrowseButton_Click);
            // 
            // ICAEncryptionLevelCombobox
            // 
            this.ICAEncryptionLevelCombobox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.ICAEncryptionLevelCombobox.Enabled = false;
            this.ICAEncryptionLevelCombobox.FormattingEnabled = true;
            this.ICAEncryptionLevelCombobox.Items.AddRange(new object[] {
            "Encrypt / Basic (default)",
            "EncRC5-0 / RC5 128 bit-logon only",
            "EncRC5-40 / RC5 40 bit",
            "EncRC5-56 / RC5 56 bit",
            "EncRC5-128 / RC5 128 bit"});
            this.ICAEncryptionLevelCombobox.Location = new System.Drawing.Point(148, 158);
            this.ICAEncryptionLevelCombobox.Name = "ICAEncryptionLevelCombobox";
            this.ICAEncryptionLevelCombobox.Size = new System.Drawing.Size(226, 21);
            this.ICAEncryptionLevelCombobox.TabIndex = 27;
            // 
            // ICAEnableEncryptionCheckbox
            // 
            this.ICAEnableEncryptionCheckbox.AutoSize = true;
            this.ICAEnableEncryptionCheckbox.Location = new System.Drawing.Point(9, 162);
            this.ICAEnableEncryptionCheckbox.Name = "ICAEnableEncryptionCheckbox";
            this.ICAEnableEncryptionCheckbox.Size = new System.Drawing.Size(112, 17);
            this.ICAEnableEncryptionCheckbox.TabIndex = 26;
            this.ICAEnableEncryptionCheckbox.Text = "Enable Encryption";
            this.ICAEnableEncryptionCheckbox.UseVisualStyleBackColor = true;
            this.ICAEnableEncryptionCheckbox.CheckedChanged += new System.EventHandler(this.ICAEnableEncryptionCheckbox_CheckedChanged);
            // 
            // ICAClientINI
            // 
            this.ICAClientINI.Location = new System.Drawing.Point(148, 122);
            this.ICAClientINI.Name = "ICAClientINI";
            this.ICAClientINI.Size = new System.Drawing.Size(226, 21);
            this.ICAClientINI.TabIndex = 25;
            // 
            // ICAServerINI
            // 
            this.ICAServerINI.Location = new System.Drawing.Point(148, 95);
            this.ICAServerINI.Name = "ICAServerINI";
            this.ICAServerINI.Size = new System.Drawing.Size(226, 21);
            this.ICAServerINI.TabIndex = 23;
            // 
            // ICAWorkingFolder
            // 
            this.ICAWorkingFolder.Location = new System.Drawing.Point(148, 68);
            this.ICAWorkingFolder.Name = "ICAWorkingFolder";
            this.ICAWorkingFolder.Size = new System.Drawing.Size(226, 21);
            this.ICAWorkingFolder.TabIndex = 21;
            // 
            // ICAApplicationPath
            // 
            this.ICAApplicationPath.Location = new System.Drawing.Point(148, 41);
            this.ICAApplicationPath.Name = "ICAApplicationPath";
            this.ICAApplicationPath.Size = new System.Drawing.Size(226, 21);
            this.ICAApplicationPath.TabIndex = 19;
            this.ICAApplicationPath.Text = ".";
            // 
            // ICAApplicationNameTextBox
            // 
            this.ICAApplicationNameTextBox.Location = new System.Drawing.Point(148, 14);
            this.ICAApplicationNameTextBox.Name = "ICAApplicationNameTextBox";
            this.ICAApplicationNameTextBox.Size = new System.Drawing.Size(226, 21);
            this.ICAApplicationNameTextBox.TabIndex = 17;
            // 
            // label35
            // 
            this.label35.AutoSize = true;
            this.label35.Location = new System.Drawing.Point(6, 125);
            this.label35.Name = "label35";
            this.label35.Size = new System.Drawing.Size(56, 13);
            this.label35.TabIndex = 24;
            this.label35.Text = "Client INI:";
            // 
            // label34
            // 
            this.label34.AutoSize = true;
            this.label34.Location = new System.Drawing.Point(6, 98);
            this.label34.Name = "label34";
            this.label34.Size = new System.Drawing.Size(61, 13);
            this.label34.TabIndex = 22;
            this.label34.Text = "Server INI:";
            // 
            // label23
            // 
            this.label23.AutoSize = true;
            this.label23.Location = new System.Drawing.Point(6, 71);
            this.label23.Name = "label23";
            this.label23.Size = new System.Drawing.Size(138, 13);
            this.label23.TabIndex = 20;
            this.label23.Text = "Application Working Folder:";
            // 
            // label22
            // 
            this.label22.AutoSize = true;
            this.label22.Location = new System.Drawing.Point(6, 44);
            this.label22.Name = "label22";
            this.label22.Size = new System.Drawing.Size(88, 13);
            this.label22.TabIndex = 18;
            this.label22.Text = "Application Path:";
            // 
            // label21
            // 
            this.label21.AutoSize = true;
            this.label21.Location = new System.Drawing.Point(6, 17);
            this.label21.Name = "label21";
            this.label21.Size = new System.Drawing.Size(38, 13);
            this.label21.TabIndex = 16;
            this.label21.Text = "Name:";
            // 
            // VMRCtabPage
            // 
            this.VMRCtabPage.Controls.Add(this.VmrcGroupBox);
            this.VMRCtabPage.Location = new System.Drawing.Point(4, 22);
            this.VMRCtabPage.Name = "VMRCtabPage";
            this.VMRCtabPage.Padding = new System.Windows.Forms.Padding(3);
            this.VMRCtabPage.Size = new System.Drawing.Size(487, 370);
            this.VMRCtabPage.TabIndex = 7;
            this.VMRCtabPage.Text = "VMRC";
            this.VMRCtabPage.UseVisualStyleBackColor = true;
            // 
            // VmrcGroupBox
            // 
            this.VmrcGroupBox.Controls.Add(this.VMRCReducedColorsCheckbox);
            this.VmrcGroupBox.Controls.Add(this.VMRCAdminModeCheckbox);
            this.VmrcGroupBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.VmrcGroupBox.Location = new System.Drawing.Point(3, 3);
            this.VmrcGroupBox.Name = "VmrcGroupBox";
            this.VmrcGroupBox.Size = new System.Drawing.Size(481, 364);
            this.VmrcGroupBox.TabIndex = 0;
            this.VmrcGroupBox.TabStop = false;
            // 
            // VMRCReducedColorsCheckbox
            // 
            this.VMRCReducedColorsCheckbox.AutoSize = true;
            this.VMRCReducedColorsCheckbox.Location = new System.Drawing.Point(6, 44);
            this.VMRCReducedColorsCheckbox.Name = "VMRCReducedColorsCheckbox";
            this.VMRCReducedColorsCheckbox.Size = new System.Drawing.Size(101, 17);
            this.VMRCReducedColorsCheckbox.TabIndex = 3;
            this.VMRCReducedColorsCheckbox.Text = "Reduced Colors";
            this.VMRCReducedColorsCheckbox.UseVisualStyleBackColor = true;
            // 
            // VMRCAdminModeCheckbox
            // 
            this.VMRCAdminModeCheckbox.AutoSize = true;
            this.VMRCAdminModeCheckbox.Location = new System.Drawing.Point(6, 20);
            this.VMRCAdminModeCheckbox.Name = "VMRCAdminModeCheckbox";
            this.VMRCAdminModeCheckbox.Size = new System.Drawing.Size(119, 17);
            this.VMRCAdminModeCheckbox.TabIndex = 2;
            this.VMRCAdminModeCheckbox.Text = "Administrator Mode";
            this.VMRCAdminModeCheckbox.UseVisualStyleBackColor = true;
            // 
            // SSHTabPage
            // 
            this.SSHTabPage.Controls.Add(this.SshGroupBox);
            this.SSHTabPage.Location = new System.Drawing.Point(4, 22);
            this.SSHTabPage.Name = "SSHTabPage";
            this.SSHTabPage.Padding = new System.Windows.Forms.Padding(3);
            this.SSHTabPage.Size = new System.Drawing.Size(487, 370);
            this.SSHTabPage.TabIndex = 13;
            this.SSHTabPage.Text = "SSH";
            this.SSHTabPage.UseVisualStyleBackColor = true;
            // 
            // SshGroupBox
            // 
            this.SshGroupBox.Controls.Add(this.SSHPreferences);
            this.SshGroupBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.SshGroupBox.Location = new System.Drawing.Point(3, 3);
            this.SshGroupBox.Name = "SshGroupBox";
            this.SshGroupBox.Size = new System.Drawing.Size(481, 364);
            this.SshGroupBox.TabIndex = 0;
            this.SshGroupBox.TabStop = false;
            // 
            // SSHPreferences
            // 
            this.SSHPreferences.AuthMethod = SSHClient.AuthMethod.PublicKey;
            this.SSHPreferences.KeyTag = "";
            this.SSHPreferences.Location = new System.Drawing.Point(-2, 4);
            this.SSHPreferences.Margin = new System.Windows.Forms.Padding(4);
            this.SSHPreferences.Name = "SSHPreferences";
            this.SSHPreferences.Size = new System.Drawing.Size(466, 267);
            this.SSHPreferences.SSH1 = false;
            this.SSHPreferences.TabIndex = 1;
            // 
            // ConsoleTabPage
            // 
            this.ConsoleTabPage.Controls.Add(this.ConsoleGroupBox);
            this.ConsoleTabPage.Location = new System.Drawing.Point(4, 22);
            this.ConsoleTabPage.Name = "ConsoleTabPage";
            this.ConsoleTabPage.Padding = new System.Windows.Forms.Padding(3);
            this.ConsoleTabPage.Size = new System.Drawing.Size(487, 370);
            this.ConsoleTabPage.TabIndex = 14;
            this.ConsoleTabPage.Text = "Console";
            this.ConsoleTabPage.UseVisualStyleBackColor = true;
            // 
            // ConsoleGroupBox
            // 
            this.ConsoleGroupBox.Controls.Add(this.consolePreferences);
            this.ConsoleGroupBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ConsoleGroupBox.Location = new System.Drawing.Point(3, 3);
            this.ConsoleGroupBox.Name = "ConsoleGroupBox";
            this.ConsoleGroupBox.Size = new System.Drawing.Size(481, 364);
            this.ConsoleGroupBox.TabIndex = 1;
            this.ConsoleGroupBox.TabStop = false;
            // 
            // consolePreferences
            // 
            this.consolePreferences.Location = new System.Drawing.Point(3, 8);
            this.consolePreferences.Margin = new System.Windows.Forms.Padding(4);
            this.consolePreferences.Name = "consolePreferences";
            this.consolePreferences.Size = new System.Drawing.Size(285, 210);
            this.consolePreferences.TabIndex = 0;
            // 
            // VNCTabPage
            // 
            this.VNCTabPage.Controls.Add(this.VncGroupBox);
            this.VNCTabPage.Location = new System.Drawing.Point(4, 22);
            this.VNCTabPage.Name = "VNCTabPage";
            this.VNCTabPage.Padding = new System.Windows.Forms.Padding(3);
            this.VNCTabPage.Size = new System.Drawing.Size(487, 370);
            this.VNCTabPage.TabIndex = 12;
            this.VNCTabPage.Text = "VNC";
            this.VNCTabPage.UseVisualStyleBackColor = true;
            // 
            // VncGroupBox
            // 
            this.VncGroupBox.Controls.Add(this.VncViewOnlyCheckbox);
            this.VncGroupBox.Controls.Add(this.vncDisplayNumberInput);
            this.VncGroupBox.Controls.Add(this.label37);
            this.VncGroupBox.Controls.Add(this.vncAutoScaleCheckbox);
            this.VncGroupBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.VncGroupBox.Location = new System.Drawing.Point(3, 3);
            this.VncGroupBox.Name = "VncGroupBox";
            this.VncGroupBox.Size = new System.Drawing.Size(481, 364);
            this.VncGroupBox.TabIndex = 0;
            this.VncGroupBox.TabStop = false;
            // 
            // VncViewOnlyCheckbox
            // 
            this.VncViewOnlyCheckbox.AutoSize = true;
            this.VncViewOnlyCheckbox.Location = new System.Drawing.Point(6, 43);
            this.VncViewOnlyCheckbox.Name = "VncViewOnlyCheckbox";
            this.VncViewOnlyCheckbox.Size = new System.Drawing.Size(73, 17);
            this.VncViewOnlyCheckbox.TabIndex = 9;
            this.VncViewOnlyCheckbox.Text = "View Only";
            this.VncViewOnlyCheckbox.UseVisualStyleBackColor = true;
            // 
            // vncDisplayNumberInput
            // 
            this.vncDisplayNumberInput.Location = new System.Drawing.Point(94, 255);
            this.vncDisplayNumberInput.Name = "vncDisplayNumberInput";
            this.vncDisplayNumberInput.Size = new System.Drawing.Size(47, 21);
            this.vncDisplayNumberInput.TabIndex = 8;
            // 
            // RDPTabPage
            // 
            this.RDPTabPage.Controls.Add(this.RDPSubTabPage);
            this.RDPTabPage.Location = new System.Drawing.Point(4, 22);
            this.RDPTabPage.Name = "RDPTabPage";
            this.RDPTabPage.Padding = new System.Windows.Forms.Padding(3);
            this.RDPTabPage.Size = new System.Drawing.Size(487, 370);
            this.RDPTabPage.TabIndex = 1;
            this.RDPTabPage.Text = "RDP";
            this.RDPTabPage.UseVisualStyleBackColor = true;
            // 
            // RDPSubTabPage
            // 
            this.RDPSubTabPage.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.RDPSubTabPage.Controls.Add(this.RDPDisplayTabPage);
            this.RDPSubTabPage.Controls.Add(this.RDPLocalResourcesTabPage);
            this.RDPSubTabPage.Controls.Add(this.tabPage8);
            this.RDPSubTabPage.Controls.Add(this.tabPage9);
            this.RDPSubTabPage.Controls.Add(this.RdpSessionTabPage);
            this.RDPSubTabPage.Controls.Add(this.RdpTsgwTabPage);
            this.RDPSubTabPage.Location = new System.Drawing.Point(3, 3);
            this.RDPSubTabPage.Name = "RDPSubTabPage";
            this.RDPSubTabPage.SelectedIndex = 0;
            this.RDPSubTabPage.Size = new System.Drawing.Size(484, 364);
            this.RDPSubTabPage.TabIndex = 0;
            this.RDPSubTabPage.SelectedIndexChanged += new System.EventHandler(this.RDPSubTabPage_SelectedIndexChanged);
            // 
            // RDPDisplayTabPage
            // 
            this.RDPDisplayTabPage.Controls.Add(this.chkConnectToConsole);
            this.RDPDisplayTabPage.Controls.Add(this.DisplaySettingsGroupBox);
            this.RDPDisplayTabPage.Location = new System.Drawing.Point(4, 22);
            this.RDPDisplayTabPage.Name = "RDPDisplayTabPage";
            this.RDPDisplayTabPage.Padding = new System.Windows.Forms.Padding(3);
            this.RDPDisplayTabPage.Size = new System.Drawing.Size(476, 338);
            this.RDPDisplayTabPage.TabIndex = 0;
            this.RDPDisplayTabPage.Text = "Display Settings";
            this.RDPDisplayTabPage.UseVisualStyleBackColor = true;
            // 
            // chkConnectToConsole
            // 
            this.chkConnectToConsole.AutoSize = true;
            this.chkConnectToConsole.Location = new System.Drawing.Point(6, 214);
            this.chkConnectToConsole.Name = "chkConnectToConsole";
            this.chkConnectToConsole.Size = new System.Drawing.Size(120, 17);
            this.chkConnectToConsole.TabIndex = 18;
            this.chkConnectToConsole.Text = "Co&nnect to Console";
            this.chkConnectToConsole.UseVisualStyleBackColor = true;
            // 
            // DisplaySettingsGroupBox
            // 
            this.DisplaySettingsGroupBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.DisplaySettingsGroupBox.Controls.Add(this.AllowDesktopCompositionCheckbox);
            this.DisplaySettingsGroupBox.Controls.Add(this.AllowFontSmoothingCheckbox);
            this.DisplaySettingsGroupBox.Controls.Add(this.customSizePanel);
            this.DisplaySettingsGroupBox.Controls.Add(this.chkDisableWallpaper);
            this.DisplaySettingsGroupBox.Controls.Add(this.chkDisableThemes);
            this.DisplaySettingsGroupBox.Controls.Add(this.chkDisableMenuAnimations);
            this.DisplaySettingsGroupBox.Controls.Add(this.chkDisableFullWindowDrag);
            this.DisplaySettingsGroupBox.Controls.Add(this.chkDisableCursorBlinking);
            this.DisplaySettingsGroupBox.Controls.Add(this.chkDisableCursorShadow);
            this.DisplaySettingsGroupBox.Controls.Add(this.cmbColors);
            this.DisplaySettingsGroupBox.Controls.Add(this.label7);
            this.DisplaySettingsGroupBox.Controls.Add(this.cmbResolution);
            this.DisplaySettingsGroupBox.Controls.Add(this.label6);
            this.DisplaySettingsGroupBox.Location = new System.Drawing.Point(6, 6);
            this.DisplaySettingsGroupBox.Name = "DisplaySettingsGroupBox";
            this.DisplaySettingsGroupBox.Size = new System.Drawing.Size(461, 200);
            this.DisplaySettingsGroupBox.TabIndex = 0;
            this.DisplaySettingsGroupBox.TabStop = false;
            // 
            // AllowDesktopCompositionCheckbox
            // 
            this.AllowDesktopCompositionCheckbox.AutoSize = true;
            this.AllowDesktopCompositionCheckbox.Location = new System.Drawing.Point(297, 177);
            this.AllowDesktopCompositionCheckbox.Name = "AllowDesktopCompositionCheckbox";
            this.AllowDesktopCompositionCheckbox.Size = new System.Drawing.Size(154, 17);
            this.AllowDesktopCompositionCheckbox.TabIndex = 16;
            this.AllowDesktopCompositionCheckbox.Text = "Allow Desktop Composition";
            this.AllowDesktopCompositionCheckbox.UseVisualStyleBackColor = true;
            // 
            // AllowFontSmoothingCheckbox
            // 
            this.AllowFontSmoothingCheckbox.AutoSize = true;
            this.AllowFontSmoothingCheckbox.Location = new System.Drawing.Point(297, 157);
            this.AllowFontSmoothingCheckbox.Name = "AllowFontSmoothingCheckbox";
            this.AllowFontSmoothingCheckbox.Size = new System.Drawing.Size(129, 17);
            this.AllowFontSmoothingCheckbox.TabIndex = 15;
            this.AllowFontSmoothingCheckbox.Text = "Allow Font Smoothing";
            this.AllowFontSmoothingCheckbox.UseVisualStyleBackColor = true;
            // 
            // customSizePanel
            // 
            this.customSizePanel.Controls.Add(this.widthUpDown);
            this.customSizePanel.Controls.Add(this.heightUpDown);
            this.customSizePanel.Controls.Add(this.label31);
            this.customSizePanel.Controls.Add(this.label32);
            this.customSizePanel.Location = new System.Drawing.Point(297, 14);
            this.customSizePanel.Name = "customSizePanel";
            this.customSizePanel.Size = new System.Drawing.Size(142, 80);
            this.customSizePanel.TabIndex = 14;
            this.customSizePanel.Visible = false;
            // 
            // label31
            // 
            this.label31.AutoSize = true;
            this.label31.Location = new System.Drawing.Point(12, 14);
            this.label31.Name = "label31";
            this.label31.Size = new System.Drawing.Size(39, 13);
            this.label31.TabIndex = 10;
            this.label31.Text = "Width:";
            // 
            // label32
            // 
            this.label32.AutoSize = true;
            this.label32.Location = new System.Drawing.Point(9, 41);
            this.label32.Name = "label32";
            this.label32.Size = new System.Drawing.Size(42, 13);
            this.label32.TabIndex = 12;
            this.label32.Text = "Height:";
            // 
            // chkDisableWallpaper
            // 
            this.chkDisableWallpaper.AutoSize = true;
            this.chkDisableWallpaper.Location = new System.Drawing.Point(124, 177);
            this.chkDisableWallpaper.Name = "chkDisableWallpaper";
            this.chkDisableWallpaper.Size = new System.Drawing.Size(153, 17);
            this.chkDisableWallpaper.TabIndex = 9;
            this.chkDisableWallpaper.Text = "Disable Desktop Wallpaper";
            this.chkDisableWallpaper.UseVisualStyleBackColor = true;
            // 
            // chkDisableThemes
            // 
            this.chkDisableThemes.AutoSize = true;
            this.chkDisableThemes.Location = new System.Drawing.Point(124, 157);
            this.chkDisableThemes.Name = "chkDisableThemes";
            this.chkDisableThemes.Size = new System.Drawing.Size(103, 17);
            this.chkDisableThemes.TabIndex = 8;
            this.chkDisableThemes.Text = "Disable Theming";
            this.chkDisableThemes.UseVisualStyleBackColor = true;
            // 
            // chkDisableMenuAnimations
            // 
            this.chkDisableMenuAnimations.AutoSize = true;
            this.chkDisableMenuAnimations.Location = new System.Drawing.Point(124, 137);
            this.chkDisableMenuAnimations.Name = "chkDisableMenuAnimations";
            this.chkDisableMenuAnimations.Size = new System.Drawing.Size(144, 17);
            this.chkDisableMenuAnimations.TabIndex = 7;
            this.chkDisableMenuAnimations.Text = "Disable Menu Animations";
            this.chkDisableMenuAnimations.UseVisualStyleBackColor = true;
            // 
            // chkDisableFullWindowDrag
            // 
            this.chkDisableFullWindowDrag.AutoSize = true;
            this.chkDisableFullWindowDrag.Location = new System.Drawing.Point(124, 117);
            this.chkDisableFullWindowDrag.Name = "chkDisableFullWindowDrag";
            this.chkDisableFullWindowDrag.Size = new System.Drawing.Size(146, 17);
            this.chkDisableFullWindowDrag.TabIndex = 6;
            this.chkDisableFullWindowDrag.Text = "Disable Full-Window drag";
            this.chkDisableFullWindowDrag.UseVisualStyleBackColor = true;
            // 
            // chkDisableCursorBlinking
            // 
            this.chkDisableCursorBlinking.AutoSize = true;
            this.chkDisableCursorBlinking.Location = new System.Drawing.Point(124, 97);
            this.chkDisableCursorBlinking.Name = "chkDisableCursorBlinking";
            this.chkDisableCursorBlinking.Size = new System.Drawing.Size(133, 17);
            this.chkDisableCursorBlinking.TabIndex = 5;
            this.chkDisableCursorBlinking.Text = "Disable Cursor Blinking";
            this.chkDisableCursorBlinking.UseVisualStyleBackColor = true;
            // 
            // chkDisableCursorShadow
            // 
            this.chkDisableCursorShadow.AutoSize = true;
            this.chkDisableCursorShadow.Location = new System.Drawing.Point(124, 77);
            this.chkDisableCursorShadow.Name = "chkDisableCursorShadow";
            this.chkDisableCursorShadow.Size = new System.Drawing.Size(136, 17);
            this.chkDisableCursorShadow.TabIndex = 4;
            this.chkDisableCursorShadow.Text = "Disable Cursor Shadow";
            this.chkDisableCursorShadow.UseVisualStyleBackColor = true;
            // 
            // cmbColors
            // 
            this.cmbColors.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbColors.FormattingEnabled = true;
            this.cmbColors.Items.AddRange(new object[] {
            "256 Colors",
            "High Color (16 Bit)",
            "True Color (24 Bit)",
            "Highest Quality (32 Bit)"});
            this.cmbColors.Location = new System.Drawing.Point(124, 50);
            this.cmbColors.Name = "cmbColors";
            this.cmbColors.Size = new System.Drawing.Size(133, 21);
            this.cmbColors.TabIndex = 3;
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(18, 53);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(41, 13);
            this.label7.TabIndex = 2;
            this.label7.Text = "Co&lors:";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(18, 26);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(71, 13);
            this.label6.TabIndex = 0;
            this.label6.Text = "&Desktop size:";
            // 
            // RDPLocalResourcesTabPage
            // 
            this.RDPLocalResourcesTabPage.Controls.Add(this.LocalResourceGroupBox);
            this.RDPLocalResourcesTabPage.Location = new System.Drawing.Point(4, 22);
            this.RDPLocalResourcesTabPage.Name = "RDPLocalResourcesTabPage";
            this.RDPLocalResourcesTabPage.Padding = new System.Windows.Forms.Padding(3);
            this.RDPLocalResourcesTabPage.Size = new System.Drawing.Size(476, 338);
            this.RDPLocalResourcesTabPage.TabIndex = 1;
            this.RDPLocalResourcesTabPage.Text = "Local Resources";
            this.RDPLocalResourcesTabPage.UseVisualStyleBackColor = true;
            // 
            // LocalResourceGroupBox
            // 
            this.LocalResourceGroupBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.LocalResourceGroupBox.Controls.Add(this.btnDrives);
            this.LocalResourceGroupBox.Controls.Add(this.cmbSounds);
            this.LocalResourceGroupBox.Controls.Add(this.chkRedirectSmartcards);
            this.LocalResourceGroupBox.Controls.Add(this.label8);
            this.LocalResourceGroupBox.Controls.Add(this.chkRedirectClipboard);
            this.LocalResourceGroupBox.Controls.Add(this.label9);
            this.LocalResourceGroupBox.Controls.Add(this.btnBrowseShare);
            this.LocalResourceGroupBox.Controls.Add(this.chkPrinters);
            this.LocalResourceGroupBox.Controls.Add(this.txtDesktopShare);
            this.LocalResourceGroupBox.Controls.Add(this.chkSerialPorts);
            this.LocalResourceGroupBox.Controls.Add(this.label10);
            this.LocalResourceGroupBox.Location = new System.Drawing.Point(6, 6);
            this.LocalResourceGroupBox.Name = "LocalResourceGroupBox";
            this.LocalResourceGroupBox.Size = new System.Drawing.Size(464, 200);
            this.LocalResourceGroupBox.TabIndex = 12;
            this.LocalResourceGroupBox.TabStop = false;
            // 
            // btnDrives
            // 
            this.btnDrives.Location = new System.Drawing.Point(106, 71);
            this.btnDrives.Name = "btnDrives";
            this.btnDrives.Size = new System.Drawing.Size(236, 23);
            this.btnDrives.TabIndex = 12;
            this.btnDrives.Text = "&Disk Drives && Plug and Play Devices...";
            this.btnDrives.UseVisualStyleBackColor = true;
            this.btnDrives.Click += new System.EventHandler(this.btnDrives_Click);
            // 
            // cmbSounds
            // 
            this.cmbSounds.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbSounds.FormattingEnabled = true;
            this.cmbSounds.Items.AddRange(new object[] {
            "Redirect sounds to the client",
            "Play sounds at the remote computer",
            "Disable sound redirection; do not play sounds at the server"});
            this.cmbSounds.Location = new System.Drawing.Point(106, 20);
            this.cmbSounds.Name = "cmbSounds";
            this.cmbSounds.Size = new System.Drawing.Size(335, 21);
            this.cmbSounds.TabIndex = 1;
            // 
            // chkRedirectSmartcards
            // 
            this.chkRedirectSmartcards.AutoSize = true;
            this.chkRedirectSmartcards.Location = new System.Drawing.Point(216, 123);
            this.chkRedirectSmartcards.Name = "chkRedirectSmartcards";
            this.chkRedirectSmartcards.Size = new System.Drawing.Size(126, 17);
            this.chkRedirectSmartcards.TabIndex = 8;
            this.chkRedirectSmartcards.Text = "Redirect Smart ca&rds";
            this.chkRedirectSmartcards.UseVisualStyleBackColor = true;
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(15, 23);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(85, 13);
            this.label8.TabIndex = 0;
            this.label8.Text = "Remote &sounds:";
            // 
            // chkRedirectClipboard
            // 
            this.chkRedirectClipboard.AutoSize = true;
            this.chkRedirectClipboard.Checked = true;
            this.chkRedirectClipboard.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkRedirectClipboard.Location = new System.Drawing.Point(216, 100);
            this.chkRedirectClipboard.Name = "chkRedirectClipboard";
            this.chkRedirectClipboard.Size = new System.Drawing.Size(114, 17);
            this.chkRedirectClipboard.TabIndex = 7;
            this.chkRedirectClipboard.Text = "Redirect &Clipboard";
            this.chkRedirectClipboard.UseVisualStyleBackColor = true;
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(105, 55);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(225, 13);
            this.label9.TabIndex = 2;
            this.label9.Text = "Automatically connect to these local devices :";
            // 
            // btnBrowseShare
            // 
            this.btnBrowseShare.Image = global::Terminals.Properties.Resources.folder;
            this.btnBrowseShare.Location = new System.Drawing.Point(420, 157);
            this.btnBrowseShare.Name = "btnBrowseShare";
            this.btnBrowseShare.Size = new System.Drawing.Size(21, 21);
            this.btnBrowseShare.TabIndex = 11;
            this.btnBrowseShare.UseVisualStyleBackColor = true;
            this.btnBrowseShare.Click += new System.EventHandler(this.btnBrowseShare_Click);
            // 
            // chkPrinters
            // 
            this.chkPrinters.AutoSize = true;
            this.chkPrinters.Location = new System.Drawing.Point(108, 100);
            this.chkPrinters.Name = "chkPrinters";
            this.chkPrinters.Size = new System.Drawing.Size(63, 17);
            this.chkPrinters.TabIndex = 4;
            this.chkPrinters.Text = "&Printers";
            this.chkPrinters.UseVisualStyleBackColor = true;
            // 
            // chkSerialPorts
            // 
            this.chkSerialPorts.AutoSize = true;
            this.chkSerialPorts.Location = new System.Drawing.Point(108, 123);
            this.chkSerialPorts.Name = "chkSerialPorts";
            this.chkSerialPorts.Size = new System.Drawing.Size(80, 17);
            this.chkSerialPorts.TabIndex = 5;
            this.chkSerialPorts.Text = "Seria&l ports";
            this.chkSerialPorts.UseVisualStyleBackColor = true;
            // 
            // tabPage8
            // 
            this.tabPage8.Controls.Add(this.ExtendedSettingsGgroupBox);
            this.tabPage8.Controls.Add(this.label30);
            this.tabPage8.Location = new System.Drawing.Point(4, 22);
            this.tabPage8.Name = "tabPage8";
            this.tabPage8.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage8.Size = new System.Drawing.Size(476, 338);
            this.tabPage8.TabIndex = 2;
            this.tabPage8.Text = "Extended Settings";
            this.tabPage8.UseVisualStyleBackColor = true;
            // 
            // ExtendedSettingsGgroupBox
            // 
            this.ExtendedSettingsGgroupBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.ExtendedSettingsGgroupBox.Controls.Add(this.EnableNLAAuthenticationCheckbox);
            this.ExtendedSettingsGgroupBox.Controls.Add(this.EnableTLSAuthenticationCheckbox);
            this.ExtendedSettingsGgroupBox.Controls.Add(this.label29);
            this.ExtendedSettingsGgroupBox.Controls.Add(this.ShutdownTimeoutTextBox);
            this.ExtendedSettingsGgroupBox.Controls.Add(this.label28);
            this.ExtendedSettingsGgroupBox.Controls.Add(this.OverallTimeoutTextbox);
            this.ExtendedSettingsGgroupBox.Controls.Add(this.label27);
            this.ExtendedSettingsGgroupBox.Controls.Add(this.SingleTimeOutTextbox);
            this.ExtendedSettingsGgroupBox.Controls.Add(this.label26);
            this.ExtendedSettingsGgroupBox.Controls.Add(this.IdleTimeoutMinutesTextBox);
            this.ExtendedSettingsGgroupBox.Controls.Add(this.GrabFocusOnConnectCheckbox);
            this.ExtendedSettingsGgroupBox.Controls.Add(this.EnableEncryptionCheckbox);
            this.ExtendedSettingsGgroupBox.Controls.Add(this.DisableWindowsKeyCheckbox);
            this.ExtendedSettingsGgroupBox.Controls.Add(this.DetectDoubleClicksCheckbox);
            this.ExtendedSettingsGgroupBox.Controls.Add(this.DisplayConnectionBarCheckbox);
            this.ExtendedSettingsGgroupBox.Controls.Add(this.DisableControlAltDeleteCheckbox);
            this.ExtendedSettingsGgroupBox.Controls.Add(this.AcceleratorPassthroughCheckBox);
            this.ExtendedSettingsGgroupBox.Controls.Add(this.EnableCompressionCheckbox);
            this.ExtendedSettingsGgroupBox.Controls.Add(this.EnableBitmapPersistenceCheckbox);
            this.ExtendedSettingsGgroupBox.Controls.Add(this.AllowBackgroundInputCheckBox);
            this.ExtendedSettingsGgroupBox.Location = new System.Drawing.Point(6, 6);
            this.ExtendedSettingsGgroupBox.Name = "ExtendedSettingsGgroupBox";
            this.ExtendedSettingsGgroupBox.Size = new System.Drawing.Size(461, 245);
            this.ExtendedSettingsGgroupBox.TabIndex = 21;
            this.ExtendedSettingsGgroupBox.TabStop = false;
            // 
            // label29
            // 
            this.label29.AutoSize = true;
            this.label29.Location = new System.Drawing.Point(239, 216);
            this.label29.Name = "label29";
            this.label29.Size = new System.Drawing.Size(100, 13);
            this.label29.TabIndex = 39;
            this.label29.Text = "Shutdown Timeout:";
            // 
            // label28
            // 
            this.label28.AutoSize = true;
            this.label28.Location = new System.Drawing.Point(239, 162);
            this.label28.Name = "label28";
            this.label28.Size = new System.Drawing.Size(86, 13);
            this.label28.TabIndex = 35;
            this.label28.Text = "Overall Timeout:";
            // 
            // label27
            // 
            this.label27.AutoSize = true;
            this.label27.Location = new System.Drawing.Point(239, 189);
            this.label27.Name = "label27";
            this.label27.Size = new System.Drawing.Size(80, 13);
            this.label27.TabIndex = 37;
            this.label27.Text = "Single Timeout:";
            // 
            // label26
            // 
            this.label26.AutoSize = true;
            this.label26.Location = new System.Drawing.Point(239, 139);
            this.label26.Name = "label26";
            this.label26.Size = new System.Drawing.Size(70, 13);
            this.label26.TabIndex = 33;
            this.label26.Text = "Idle Timeout:";
            // 
            // label30
            // 
            this.label30.AutoSize = true;
            this.label30.Location = new System.Drawing.Point(3, 280);
            this.label30.Name = "label30";
            this.label30.Size = new System.Drawing.Size(195, 13);
            this.label30.TabIndex = 7;
            this.label30.Text = "Note: These are experimental settings.";
            // 
            // tabPage9
            // 
            this.tabPage9.Controls.Add(this.SecuritySettingsGroupBox);
            this.tabPage9.Location = new System.Drawing.Point(4, 22);
            this.tabPage9.Name = "tabPage9";
            this.tabPage9.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage9.Size = new System.Drawing.Size(476, 338);
            this.tabPage9.TabIndex = 3;
            this.tabPage9.Text = "Security Settings";
            this.tabPage9.UseVisualStyleBackColor = true;
            // 
            // SecuritySettingsGroupBox
            // 
            this.SecuritySettingsGroupBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.SecuritySettingsGroupBox.Controls.Add(this.panel2);
            this.SecuritySettingsGroupBox.Controls.Add(this.SecuritySettingsEnabledCheckbox);
            this.SecuritySettingsGroupBox.Location = new System.Drawing.Point(6, 6);
            this.SecuritySettingsGroupBox.Name = "SecuritySettingsGroupBox";
            this.SecuritySettingsGroupBox.Size = new System.Drawing.Size(461, 245);
            this.SecuritySettingsGroupBox.TabIndex = 2;
            this.SecuritySettingsGroupBox.TabStop = false;
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.SecurityStartFullScreenCheckbox);
            this.panel2.Controls.Add(this.label25);
            this.panel2.Controls.Add(this.SecurityWorkingFolderTextBox);
            this.panel2.Controls.Add(this.label24);
            this.panel2.Controls.Add(this.SecuriytStartProgramTextbox);
            this.panel2.Enabled = false;
            this.panel2.Location = new System.Drawing.Point(6, 43);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(385, 171);
            this.panel2.TabIndex = 3;
            // 
            // SecurityStartFullScreenCheckbox
            // 
            this.SecurityStartFullScreenCheckbox.AutoSize = true;
            this.SecurityStartFullScreenCheckbox.Location = new System.Drawing.Point(6, 66);
            this.SecurityStartFullScreenCheckbox.Name = "SecurityStartFullScreenCheckbox";
            this.SecurityStartFullScreenCheckbox.Size = new System.Drawing.Size(105, 17);
            this.SecurityStartFullScreenCheckbox.TabIndex = 4;
            this.SecurityStartFullScreenCheckbox.Text = "Start Full Screen";
            this.SecurityStartFullScreenCheckbox.UseVisualStyleBackColor = true;
            // 
            // label25
            // 
            this.label25.AutoSize = true;
            this.label25.Location = new System.Drawing.Point(3, 37);
            this.label25.Name = "label25";
            this.label25.Size = new System.Drawing.Size(83, 13);
            this.label25.TabIndex = 3;
            this.label25.Text = "Working Folder:";
            // 
            // SecurityWorkingFolderTextBox
            // 
            this.SecurityWorkingFolderTextBox.Location = new System.Drawing.Point(87, 34);
            this.SecurityWorkingFolderTextBox.Name = "SecurityWorkingFolderTextBox";
            this.SecurityWorkingFolderTextBox.Size = new System.Drawing.Size(286, 21);
            this.SecurityWorkingFolderTextBox.TabIndex = 2;
            // 
            // label24
            // 
            this.label24.AutoSize = true;
            this.label24.Location = new System.Drawing.Point(3, 10);
            this.label24.Name = "label24";
            this.label24.Size = new System.Drawing.Size(78, 13);
            this.label24.TabIndex = 1;
            this.label24.Text = "Start Program:";
            // 
            // SecuriytStartProgramTextbox
            // 
            this.SecuriytStartProgramTextbox.Location = new System.Drawing.Point(87, 7);
            this.SecuriytStartProgramTextbox.Name = "SecuriytStartProgramTextbox";
            this.SecuriytStartProgramTextbox.Size = new System.Drawing.Size(286, 21);
            this.SecuriytStartProgramTextbox.TabIndex = 0;
            // 
            // SecuritySettingsEnabledCheckbox
            // 
            this.SecuritySettingsEnabledCheckbox.AutoSize = true;
            this.SecuritySettingsEnabledCheckbox.Location = new System.Drawing.Point(6, 20);
            this.SecuritySettingsEnabledCheckbox.Name = "SecuritySettingsEnabledCheckbox";
            this.SecuritySettingsEnabledCheckbox.Size = new System.Drawing.Size(64, 17);
            this.SecuritySettingsEnabledCheckbox.TabIndex = 2;
            this.SecuritySettingsEnabledCheckbox.Text = "Enabled";
            this.SecuritySettingsEnabledCheckbox.UseVisualStyleBackColor = true;
            this.SecuritySettingsEnabledCheckbox.CheckedChanged += new System.EventHandler(this.SecuritySettingsEnabledCheckbox_CheckedChanged);
            // 
            // RdpSessionTabPage
            // 
            this.RdpSessionTabPage.Location = new System.Drawing.Point(4, 22);
            this.RdpSessionTabPage.Name = "RdpSessionTabPage";
            this.RdpSessionTabPage.Padding = new System.Windows.Forms.Padding(3);
            this.RdpSessionTabPage.Size = new System.Drawing.Size(476, 338);
            this.RdpSessionTabPage.TabIndex = 4;
            this.RdpSessionTabPage.Text = "TS Sessions";
            this.RdpSessionTabPage.UseVisualStyleBackColor = true;
            // 
            // RdpTsgwTabPage
            // 
            this.RdpTsgwTabPage.Controls.Add(this.TerminalGwLoginSettingsGroupBox);
            this.RdpTsgwTabPage.Controls.Add(this.TerminalGwSettingsGroupBox);
            this.RdpTsgwTabPage.Location = new System.Drawing.Point(4, 22);
            this.RdpTsgwTabPage.Name = "RdpTsgwTabPage";
            this.RdpTsgwTabPage.Padding = new System.Windows.Forms.Padding(3);
            this.RdpTsgwTabPage.Size = new System.Drawing.Size(476, 338);
            this.RdpTsgwTabPage.TabIndex = 5;
            this.RdpTsgwTabPage.Text = "TSGW";
            this.RdpTsgwTabPage.UseVisualStyleBackColor = true;
            // 
            // TerminalGwLoginSettingsGroupBox
            // 
            this.TerminalGwLoginSettingsGroupBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.TerminalGwLoginSettingsGroupBox.Controls.Add(this.pnlTSGWlogon);
            this.TerminalGwLoginSettingsGroupBox.Controls.Add(this.chkTSGWlogin);
            this.TerminalGwLoginSettingsGroupBox.Location = new System.Drawing.Point(7, 159);
            this.TerminalGwLoginSettingsGroupBox.Name = "TerminalGwLoginSettingsGroupBox";
            this.TerminalGwLoginSettingsGroupBox.Size = new System.Drawing.Size(460, 121);
            this.TerminalGwLoginSettingsGroupBox.TabIndex = 1;
            this.TerminalGwLoginSettingsGroupBox.TabStop = false;
            this.TerminalGwLoginSettingsGroupBox.Text = "Login Settings";
            // 
            // pnlTSGWlogon
            // 
            this.pnlTSGWlogon.Controls.Add(this.txtTSGWDomain);
            this.pnlTSGWlogon.Controls.Add(this.txtTSGWPassword);
            this.pnlTSGWlogon.Controls.Add(this.label18);
            this.pnlTSGWlogon.Controls.Add(this.label19);
            this.pnlTSGWlogon.Controls.Add(this.label16);
            this.pnlTSGWlogon.Controls.Add(this.txtTSGWUserName);
            this.pnlTSGWlogon.Enabled = false;
            this.pnlTSGWlogon.Location = new System.Drawing.Point(7, 39);
            this.pnlTSGWlogon.Name = "pnlTSGWlogon";
            this.pnlTSGWlogon.Size = new System.Drawing.Size(460, 80);
            this.pnlTSGWlogon.TabIndex = 1;
            // 
            // txtTSGWDomain
            // 
            this.txtTSGWDomain.Location = new System.Drawing.Point(106, 55);
            this.txtTSGWDomain.Name = "txtTSGWDomain";
            this.txtTSGWDomain.Size = new System.Drawing.Size(224, 21);
            this.txtTSGWDomain.TabIndex = 5;
            // 
            // txtTSGWPassword
            // 
            this.txtTSGWPassword.Location = new System.Drawing.Point(106, 29);
            this.txtTSGWPassword.Name = "txtTSGWPassword";
            this.txtTSGWPassword.PasswordChar = '*';
            this.txtTSGWPassword.Size = new System.Drawing.Size(224, 21);
            this.txtTSGWPassword.TabIndex = 4;
            // 
            // label18
            // 
            this.label18.AutoSize = true;
            this.label18.Location = new System.Drawing.Point(21, 58);
            this.label18.Name = "label18";
            this.label18.Size = new System.Drawing.Size(46, 13);
            this.label18.TabIndex = 3;
            this.label18.Text = "Domain:";
            // 
            // label19
            // 
            this.label19.AutoSize = true;
            this.label19.Location = new System.Drawing.Point(21, 32);
            this.label19.Name = "label19";
            this.label19.Size = new System.Drawing.Size(57, 13);
            this.label19.TabIndex = 2;
            this.label19.Text = "Password:";
            // 
            // label16
            // 
            this.label16.AutoSize = true;
            this.label16.Location = new System.Drawing.Point(21, 6);
            this.label16.Name = "label16";
            this.label16.Size = new System.Drawing.Size(59, 13);
            this.label16.TabIndex = 1;
            this.label16.Text = "Username:";
            // 
            // txtTSGWUserName
            // 
            this.txtTSGWUserName.Location = new System.Drawing.Point(106, 3);
            this.txtTSGWUserName.Name = "txtTSGWUserName";
            this.txtTSGWUserName.Size = new System.Drawing.Size(224, 21);
            this.txtTSGWUserName.TabIndex = 0;
            // 
            // chkTSGWlogin
            // 
            this.chkTSGWlogin.AutoSize = true;
            this.chkTSGWlogin.Location = new System.Drawing.Point(7, 21);
            this.chkTSGWlogin.Name = "chkTSGWlogin";
            this.chkTSGWlogin.Size = new System.Drawing.Size(176, 17);
            this.chkTSGWlogin.TabIndex = 0;
            this.chkTSGWlogin.Text = "Use Separate Login Credentials";
            this.chkTSGWlogin.UseVisualStyleBackColor = true;
            this.chkTSGWlogin.CheckedChanged += new System.EventHandler(this.checkBox1_CheckedChanged);
            // 
            // TerminalGwSettingsGroupBox
            // 
            this.TerminalGwSettingsGroupBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.TerminalGwSettingsGroupBox.Controls.Add(this.radTSGWenable);
            this.TerminalGwSettingsGroupBox.Controls.Add(this.radTSGWdisable);
            this.TerminalGwSettingsGroupBox.Controls.Add(this.pnlTSGWsettings);
            this.TerminalGwSettingsGroupBox.Location = new System.Drawing.Point(6, 6);
            this.TerminalGwSettingsGroupBox.Name = "TerminalGwSettingsGroupBox";
            this.TerminalGwSettingsGroupBox.Size = new System.Drawing.Size(461, 147);
            this.TerminalGwSettingsGroupBox.TabIndex = 0;
            this.TerminalGwSettingsGroupBox.TabStop = false;
            this.TerminalGwSettingsGroupBox.Text = "Terminal Server Gateway Settings";
            // 
            // radTSGWenable
            // 
            this.radTSGWenable.AutoSize = true;
            this.radTSGWenable.Location = new System.Drawing.Point(6, 43);
            this.radTSGWenable.Name = "radTSGWenable";
            this.radTSGWenable.Size = new System.Drawing.Size(213, 17);
            this.radTSGWenable.TabIndex = 2;
            this.radTSGWenable.Text = "Use the following TS Gateway settings:";
            this.radTSGWenable.UseVisualStyleBackColor = true;
            this.radTSGWenable.CheckedChanged += new System.EventHandler(this.radTSGWenable_CheckedChanged);
            // 
            // radTSGWdisable
            // 
            this.radTSGWdisable.AutoSize = true;
            this.radTSGWdisable.Checked = true;
            this.radTSGWdisable.Location = new System.Drawing.Point(6, 20);
            this.radTSGWdisable.Name = "radTSGWdisable";
            this.radTSGWdisable.Size = new System.Drawing.Size(138, 17);
            this.radTSGWdisable.TabIndex = 1;
            this.radTSGWdisable.TabStop = true;
            this.radTSGWdisable.Text = "Do not use TS Gateway";
            this.radTSGWdisable.UseVisualStyleBackColor = true;
            // 
            // pnlTSGWsettings
            // 
            this.pnlTSGWsettings.Controls.Add(this.label20);
            this.pnlTSGWsettings.Controls.Add(this.cmbTSGWLogonMethod);
            this.pnlTSGWsettings.Controls.Add(this.chkTSGWlocalBypass);
            this.pnlTSGWsettings.Controls.Add(this.label17);
            this.pnlTSGWsettings.Controls.Add(this.txtTSGWServer);
            this.pnlTSGWsettings.Enabled = false;
            this.pnlTSGWsettings.Location = new System.Drawing.Point(6, 61);
            this.pnlTSGWsettings.Name = "pnlTSGWsettings";
            this.pnlTSGWsettings.Size = new System.Drawing.Size(388, 80);
            this.pnlTSGWsettings.TabIndex = 8;
            // 
            // label20
            // 
            this.label20.AutoSize = true;
            this.label20.Location = new System.Drawing.Point(28, 33);
            this.label20.Name = "label20";
            this.label20.Size = new System.Drawing.Size(79, 13);
            this.label20.TabIndex = 10;
            this.label20.Text = "Logon Method:";
            // 
            // cmbTSGWLogonMethod
            // 
            this.cmbTSGWLogonMethod.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbTSGWLogonMethod.FormattingEnabled = true;
            this.cmbTSGWLogonMethod.Items.AddRange(new object[] {
            "Ask for Password (NTLM)",
            "Smart Card"});
            this.cmbTSGWLogonMethod.Location = new System.Drawing.Point(113, 30);
            this.cmbTSGWLogonMethod.Name = "cmbTSGWLogonMethod";
            this.cmbTSGWLogonMethod.Size = new System.Drawing.Size(224, 21);
            this.cmbTSGWLogonMethod.TabIndex = 9;
            // 
            // chkTSGWlocalBypass
            // 
            this.chkTSGWlocalBypass.AutoSize = true;
            this.chkTSGWlocalBypass.Location = new System.Drawing.Point(31, 58);
            this.chkTSGWlocalBypass.Name = "chkTSGWlocalBypass";
            this.chkTSGWlocalBypass.Size = new System.Drawing.Size(214, 17);
            this.chkTSGWlocalBypass.TabIndex = 8;
            this.chkTSGWlocalBypass.Text = "Bypass TS Gateway for local addresses";
            this.chkTSGWlocalBypass.UseVisualStyleBackColor = true;
            // 
            // label17
            // 
            this.label17.AutoSize = true;
            this.label17.Location = new System.Drawing.Point(28, 6);
            this.label17.Name = "label17";
            this.label17.Size = new System.Drawing.Size(73, 13);
            this.label17.TabIndex = 5;
            this.label17.Text = "Server Name:";
            // 
            // txtTSGWServer
            // 
            this.txtTSGWServer.Location = new System.Drawing.Point(113, 3);
            this.txtTSGWServer.Name = "txtTSGWServer";
            this.txtTSGWServer.Size = new System.Drawing.Size(224, 21);
            this.txtTSGWServer.TabIndex = 3;
            // 
            // GeneralTabPage
            // 
            this.GeneralTabPage.Controls.Add(this.GeneralGroupBox);
            this.GeneralTabPage.Location = new System.Drawing.Point(4, 22);
            this.GeneralTabPage.Name = "GeneralTabPage";
            this.GeneralTabPage.Padding = new System.Windows.Forms.Padding(3);
            this.GeneralTabPage.Size = new System.Drawing.Size(487, 370);
            this.GeneralTabPage.TabIndex = 0;
            this.GeneralTabPage.Text = "General";
            this.GeneralTabPage.UseVisualStyleBackColor = true;
            // 
            // GeneralGroupBox
            // 
            this.GeneralGroupBox.Controls.Add(this.httpUrlTextBox);
            this.GeneralGroupBox.Controls.Add(this.NotesTextbox);
            this.GeneralGroupBox.Controls.Add(this.txtPort);
            this.GeneralGroupBox.Controls.Add(this.txtName);
            this.GeneralGroupBox.Controls.Add(this.label15);
            this.GeneralGroupBox.Controls.Add(this.CredentialDropdown);
            this.GeneralGroupBox.Controls.Add(this.CredentialManagerPicturebox);
            this.GeneralGroupBox.Controls.Add(this.CredentialsPanel);
            this.GeneralGroupBox.Controls.Add(this.label36);
            this.GeneralGroupBox.Controls.Add(this.pictureBox2);
            this.GeneralGroupBox.Controls.Add(this.lblPort);
            this.GeneralGroupBox.Controls.Add(this.ProtocolComboBox);
            this.GeneralGroupBox.Controls.Add(this.ProtocolLabel);
            this.GeneralGroupBox.Controls.Add(this.label5);
            this.GeneralGroupBox.Controls.Add(this.cmbServers);
            this.GeneralGroupBox.Controls.Add(this.lblServerName);
            this.GeneralGroupBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.GeneralGroupBox.Location = new System.Drawing.Point(3, 3);
            this.GeneralGroupBox.Name = "GeneralGroupBox";
            this.GeneralGroupBox.Size = new System.Drawing.Size(481, 364);
            this.GeneralGroupBox.TabIndex = 0;
            this.GeneralGroupBox.TabStop = false;
            // 
            // httpUrlTextBox
            // 
            this.httpUrlTextBox.Location = new System.Drawing.Point(105, 43);
            this.httpUrlTextBox.Name = "httpUrlTextBox";
            this.httpUrlTextBox.Size = new System.Drawing.Size(356, 21);
            this.httpUrlTextBox.TabIndex = 38;
            this.httpUrlTextBox.Text = "http://terminals.codeplex.com";
            this.httpUrlTextBox.Visible = false;
            this.httpUrlTextBox.TextChanged += new System.EventHandler(this.httpUrlTextBox_TextChanged);
            // 
            // NotesTextbox
            // 
            this.NotesTextbox.Location = new System.Drawing.Point(105, 233);
            this.NotesTextbox.Multiline = true;
            this.NotesTextbox.Name = "NotesTextbox";
            this.NotesTextbox.Size = new System.Drawing.Size(356, 131);
            this.NotesTextbox.TabIndex = 33;
            // 
            // label15
            // 
            this.label15.AutoSize = true;
            this.label15.Location = new System.Drawing.Point(6, 97);
            this.label15.Name = "label15";
            this.label15.Size = new System.Drawing.Size(60, 13);
            this.label15.TabIndex = 29;
            this.label15.Text = "Credential:";
            // 
            // CredentialDropdown
            // 
            this.CredentialDropdown.DisplayMember = "Name";
            this.CredentialDropdown.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.CredentialDropdown.FormattingEnabled = true;
            this.CredentialDropdown.Location = new System.Drawing.Point(105, 94);
            this.CredentialDropdown.Name = "CredentialDropdown";
            this.CredentialDropdown.Size = new System.Drawing.Size(334, 21);
            this.CredentialDropdown.TabIndex = 30;
            this.CredentialDropdown.SelectedIndexChanged += new System.EventHandler(this.CredentialDropdown_SelectedIndexChanged);
            // 
            // CredentialsPanel
            // 
            this.CredentialsPanel.Controls.Add(this.cmbUsers);
            this.CredentialsPanel.Controls.Add(this.label1);
            this.CredentialsPanel.Controls.Add(this.cmbDomains);
            this.CredentialsPanel.Controls.Add(this.label3);
            this.CredentialsPanel.Controls.Add(this.label4);
            this.CredentialsPanel.Controls.Add(this.txtPassword);
            this.CredentialsPanel.Controls.Add(this.chkSavePassword);
            this.CredentialsPanel.Location = new System.Drawing.Point(5, 121);
            this.CredentialsPanel.Name = "CredentialsPanel";
            this.CredentialsPanel.Size = new System.Drawing.Size(456, 106);
            this.CredentialsPanel.TabIndex = 31;
            // 
            // cmbUsers
            // 
            this.cmbUsers.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
            this.cmbUsers.Location = new System.Drawing.Point(100, 30);
            this.cmbUsers.Name = "cmbUsers";
            this.cmbUsers.Size = new System.Drawing.Size(334, 21);
            this.cmbUsers.TabIndex = 3;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(1, 6);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(46, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "&Domain:";
            // 
            // cmbDomains
            // 
            this.cmbDomains.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.Suggest;
            this.cmbDomains.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
            this.cmbDomains.Location = new System.Drawing.Point(100, 3);
            this.cmbDomains.Name = "cmbDomains";
            this.cmbDomains.Size = new System.Drawing.Size(334, 21);
            this.cmbDomains.TabIndex = 1;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(1, 33);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(62, 13);
            this.label3.TabIndex = 2;
            this.label3.Text = "&User name:";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(1, 63);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(57, 13);
            this.label4.TabIndex = 4;
            this.label4.Text = "&Password:";
            // 
            // txtPassword
            // 
            this.txtPassword.Location = new System.Drawing.Point(100, 60);
            this.txtPassword.Name = "txtPassword";
            this.txtPassword.PasswordChar = '*';
            this.txtPassword.Size = new System.Drawing.Size(334, 21);
            this.txtPassword.TabIndex = 5;
            this.txtPassword.TextChanged += new System.EventHandler(this.txtPassword_TextChanged);
            // 
            // chkSavePassword
            // 
            this.chkSavePassword.AutoSize = true;
            this.chkSavePassword.Location = new System.Drawing.Point(100, 87);
            this.chkSavePassword.Name = "chkSavePassword";
            this.chkSavePassword.Size = new System.Drawing.Size(99, 17);
            this.chkSavePassword.TabIndex = 6;
            this.chkSavePassword.Text = "S&ave password";
            this.chkSavePassword.UseVisualStyleBackColor = true;
            // 
            // label36
            // 
            this.label36.AutoSize = true;
            this.label36.Location = new System.Drawing.Point(6, 236);
            this.label36.Name = "label36";
            this.label36.Size = new System.Drawing.Size(39, 13);
            this.label36.TabIndex = 32;
            this.label36.Text = "Notes:";
            // 
            // lblPort
            // 
            this.lblPort.AutoSize = true;
            this.lblPort.Location = new System.Drawing.Point(378, 46);
            this.lblPort.Name = "lblPort";
            this.lblPort.Size = new System.Drawing.Size(31, 13);
            this.lblPort.TabIndex = 25;
            this.lblPort.Text = "Port:";
            // 
            // ProtocolLabel
            // 
            this.ProtocolLabel.AutoSize = true;
            this.ProtocolLabel.Location = new System.Drawing.Point(6, 17);
            this.ProtocolLabel.Name = "ProtocolLabel";
            this.ProtocolLabel.Size = new System.Drawing.Size(50, 13);
            this.ProtocolLabel.TabIndex = 34;
            this.ProtocolLabel.Text = "&Protocol:";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(6, 73);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(94, 13);
            this.label5.TabIndex = 27;
            this.label5.Text = "Connection na&me:";
            // 
            // lblServerName
            // 
            this.lblServerName.AutoSize = true;
            this.lblServerName.Location = new System.Drawing.Point(6, 46);
            this.lblServerName.Name = "lblServerName";
            this.lblServerName.Size = new System.Drawing.Size(58, 13);
            this.lblServerName.TabIndex = 23;
            this.lblServerName.Text = "&Computer:";
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.GeneralTabPage);
            this.tabControl1.Controls.Add(this.RDPTabPage);
            this.tabControl1.Controls.Add(this.VNCTabPage);
            this.tabControl1.Controls.Add(this.ConsoleTabPage);
            this.tabControl1.Controls.Add(this.SSHTabPage);
            this.tabControl1.Controls.Add(this.VMRCtabPage);
            this.tabControl1.Controls.Add(this.ICAtabPage);
            this.tabControl1.Controls.Add(this.RAStabPage);
            this.tabControl1.Controls.Add(this.TagsTabPage);
            this.tabControl1.Controls.Add(this.ExecuteTabPage);
            this.tabControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControl1.Location = new System.Drawing.Point(84, 0);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(495, 396);
            this.tabControl1.TabIndex = 0;
            this.tabControl1.SelectedIndexChanged += new System.EventHandler(this.tabControl1_SelectedIndexChanged);
            // 
            // NewTerminalForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.btnCancel;
            this.ClientSize = new System.Drawing.Size(579, 474);
            this.Controls.Add(this.tabControl1);
            this.Controls.Add(this.pictureBox1);
            this.Controls.Add(this.groupBox7);
            this.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(177)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "NewTerminalForm";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "New Connection";
            this.Load += new System.EventHandler(this.NewTerminalForm_Load);
            this.Shown += new System.EventHandler(this.NewTerminalForm_Shown);
            ((System.ComponentModel.ISupportInitialize)(this.heightUpDown)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.widthUpDown)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.CredentialManagerPicturebox)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.contextMenuStripDefaults.ResumeLayout(false);
            this.contextMenuStripSave.ResumeLayout(false);
            this.groupBox7.ResumeLayout(false);
            this.groupBox7.PerformLayout();
            this.ExecuteTabPage.ResumeLayout(false);
            this.ExecuteGroupBox.ResumeLayout(false);
            this.ExecuteGroupBox.PerformLayout();
            this.TagsTabPage.ResumeLayout(false);
            this.panel4.ResumeLayout(false);
            this.groupBox4.ResumeLayout(false);
            this.panel1.ResumeLayout(false);
            this.groupBox3.ResumeLayout(false);
            this.panel3.ResumeLayout(false);
            this.panel3.PerformLayout();
            this.RAStabPage.ResumeLayout(false);
            this.RASGroupBox.ResumeLayout(false);
            this.ICAtabPage.ResumeLayout(false);
            this.IcaGroupBox.ResumeLayout(false);
            this.IcaGroupBox.PerformLayout();
            this.VMRCtabPage.ResumeLayout(false);
            this.VmrcGroupBox.ResumeLayout(false);
            this.VmrcGroupBox.PerformLayout();
            this.SSHTabPage.ResumeLayout(false);
            this.SshGroupBox.ResumeLayout(false);
            this.ConsoleTabPage.ResumeLayout(false);
            this.ConsoleGroupBox.ResumeLayout(false);
            this.VNCTabPage.ResumeLayout(false);
            this.VncGroupBox.ResumeLayout(false);
            this.VncGroupBox.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.vncDisplayNumberInput)).EndInit();
            this.RDPTabPage.ResumeLayout(false);
            this.RDPSubTabPage.ResumeLayout(false);
            this.RDPDisplayTabPage.ResumeLayout(false);
            this.RDPDisplayTabPage.PerformLayout();
            this.DisplaySettingsGroupBox.ResumeLayout(false);
            this.DisplaySettingsGroupBox.PerformLayout();
            this.customSizePanel.ResumeLayout(false);
            this.customSizePanel.PerformLayout();
            this.RDPLocalResourcesTabPage.ResumeLayout(false);
            this.LocalResourceGroupBox.ResumeLayout(false);
            this.LocalResourceGroupBox.PerformLayout();
            this.tabPage8.ResumeLayout(false);
            this.tabPage8.PerformLayout();
            this.ExtendedSettingsGgroupBox.ResumeLayout(false);
            this.ExtendedSettingsGgroupBox.PerformLayout();
            this.tabPage9.ResumeLayout(false);
            this.SecuritySettingsGroupBox.ResumeLayout(false);
            this.SecuritySettingsGroupBox.PerformLayout();
            this.panel2.ResumeLayout(false);
            this.panel2.PerformLayout();
            this.RdpTsgwTabPage.ResumeLayout(false);
            this.TerminalGwLoginSettingsGroupBox.ResumeLayout(false);
            this.TerminalGwLoginSettingsGroupBox.PerformLayout();
            this.pnlTSGWlogon.ResumeLayout(false);
            this.pnlTSGWlogon.PerformLayout();
            this.TerminalGwSettingsGroupBox.ResumeLayout(false);
            this.TerminalGwSettingsGroupBox.PerformLayout();
            this.pnlTSGWsettings.ResumeLayout(false);
            this.pnlTSGWsettings.PerformLayout();
            this.GeneralTabPage.ResumeLayout(false);
            this.GeneralGroupBox.ResumeLayout(false);
            this.GeneralGroupBox.PerformLayout();
            this.CredentialsPanel.ResumeLayout(false);
            this.CredentialsPanel.PerformLayout();
            this.tabControl1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.CheckBox chkAddtoToolbar;
        private System.Windows.Forms.ToolTip toolTip1;

        //private System.Windows.Forms.GroupBox groupBox2;

        private FalafelSoftware.TransPort.Ras ras1;
        private System.Windows.Forms.CheckBox NewWindowCheckbox;
        private System.Windows.Forms.TextBox textBox3;
        private System.Windows.Forms.Label label43;
        private System.Windows.Forms.Button button1;
        private Terminals.Forms.Controls.SplitButton btnSaveDefault;
        private System.Windows.Forms.ContextMenuStrip contextMenuStripDefaults;
        private System.Windows.Forms.ToolStripMenuItem saveCurrentSettingsAsDefaultToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem removeSavedDefaultsToolStripMenuItem;
        private Forms.Controls.SplitButton btnSave;
        private System.Windows.Forms.ContextMenuStrip contextMenuStripSave;
        private System.Windows.Forms.ToolStripMenuItem saveConnectToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem saveNewToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem saveCopyToolStripMenuItem;
        private System.Windows.Forms.GroupBox groupBox7;
        private System.Windows.Forms.TabPage ExecuteTabPage;
        private System.Windows.Forms.TabPage TagsTabPage;
        private System.Windows.Forms.Panel panel4;
        private System.Windows.Forms.TabPage RAStabPage;
        private System.Windows.Forms.GroupBox RASGroupBox;
        private System.Windows.Forms.ListBox RASDetailsListBox;
        private System.Windows.Forms.TabPage ICAtabPage;
        private System.Windows.Forms.TabPage VMRCtabPage;
        private System.Windows.Forms.TabPage SSHTabPage;
        private System.Windows.Forms.TabPage ConsoleTabPage;
        private System.Windows.Forms.GroupBox ConsoleGroupBox;
        private ConsolePreferences consolePreferences;
        private System.Windows.Forms.TabPage VNCTabPage;
        private System.Windows.Forms.TabPage RDPTabPage;
        private System.Windows.Forms.TabControl RDPSubTabPage;
        private System.Windows.Forms.TabPage RDPDisplayTabPage;
        private System.Windows.Forms.GroupBox DisplaySettingsGroupBox;
        private System.Windows.Forms.CheckBox AllowDesktopCompositionCheckbox;
        private System.Windows.Forms.CheckBox AllowFontSmoothingCheckbox;
        private System.Windows.Forms.Panel customSizePanel;
        private System.Windows.Forms.NumericUpDown widthUpDown;
        private System.Windows.Forms.NumericUpDown heightUpDown;
        private System.Windows.Forms.Label label31;
        private System.Windows.Forms.Label label32;
        private System.Windows.Forms.CheckBox chkDisableWallpaper;
        private System.Windows.Forms.CheckBox chkDisableThemes;
        private System.Windows.Forms.CheckBox chkDisableMenuAnimations;
        private System.Windows.Forms.CheckBox chkDisableFullWindowDrag;
        private System.Windows.Forms.CheckBox chkDisableCursorBlinking;
        private System.Windows.Forms.CheckBox chkDisableCursorShadow;
        private System.Windows.Forms.ComboBox cmbColors;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.ComboBox cmbResolution;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.TabPage RDPLocalResourcesTabPage;
        private System.Windows.Forms.GroupBox LocalResourceGroupBox;
        private System.Windows.Forms.Button btnDrives;
        private System.Windows.Forms.ComboBox cmbSounds;
        private System.Windows.Forms.CheckBox chkRedirectSmartcards;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.CheckBox chkRedirectClipboard;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.Button btnBrowseShare;
        private System.Windows.Forms.CheckBox chkPrinters;
        private System.Windows.Forms.TextBox txtDesktopShare;
        private System.Windows.Forms.CheckBox chkSerialPorts;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.TabPage tabPage8;
        private System.Windows.Forms.Label label30;
        private System.Windows.Forms.TabPage tabPage9;
        private System.Windows.Forms.TabPage RdpSessionTabPage;
        private System.Windows.Forms.TabPage RdpTsgwTabPage;
        private System.Windows.Forms.GroupBox TerminalGwLoginSettingsGroupBox;
        private System.Windows.Forms.Panel pnlTSGWlogon;
        private System.Windows.Forms.TextBox txtTSGWDomain;
        private System.Windows.Forms.TextBox txtTSGWPassword;
        private System.Windows.Forms.Label label18;
        private System.Windows.Forms.Label label19;
        private System.Windows.Forms.Label label16;
        private System.Windows.Forms.TextBox txtTSGWUserName;
        private System.Windows.Forms.CheckBox chkTSGWlogin;
        private System.Windows.Forms.GroupBox TerminalGwSettingsGroupBox;
        private System.Windows.Forms.RadioButton radTSGWenable;
        private System.Windows.Forms.RadioButton radTSGWdisable;
        private System.Windows.Forms.Panel pnlTSGWsettings;
        private System.Windows.Forms.Label label17;
        private System.Windows.Forms.TextBox txtTSGWServer;
        private System.Windows.Forms.TabPage GeneralTabPage;
        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.GroupBox ExtendedSettingsGgroupBox;
        private System.Windows.Forms.CheckBox EnableNLAAuthenticationCheckbox;
        private System.Windows.Forms.CheckBox EnableTLSAuthenticationCheckbox;
        private System.Windows.Forms.Label label29;
        private System.Windows.Forms.TextBox ShutdownTimeoutTextBox;
        private System.Windows.Forms.Label label28;
        private System.Windows.Forms.TextBox OverallTimeoutTextbox;
        private System.Windows.Forms.Label label27;
        private System.Windows.Forms.TextBox SingleTimeOutTextbox;
        private System.Windows.Forms.Label label26;
        private System.Windows.Forms.TextBox IdleTimeoutMinutesTextBox;
        private System.Windows.Forms.CheckBox GrabFocusOnConnectCheckbox;
        private System.Windows.Forms.CheckBox EnableEncryptionCheckbox;
        private System.Windows.Forms.CheckBox DisableWindowsKeyCheckbox;
        private System.Windows.Forms.CheckBox DetectDoubleClicksCheckbox;
        private System.Windows.Forms.CheckBox DisplayConnectionBarCheckbox;
        private System.Windows.Forms.CheckBox DisableControlAltDeleteCheckbox;
        private System.Windows.Forms.CheckBox AcceleratorPassthroughCheckBox;
        private System.Windows.Forms.CheckBox EnableCompressionCheckbox;
        private System.Windows.Forms.CheckBox EnableBitmapPersistenceCheckbox;
        private System.Windows.Forms.CheckBox AllowBackgroundInputCheckBox;
        private System.Windows.Forms.GroupBox SecuritySettingsGroupBox;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.CheckBox SecurityStartFullScreenCheckbox;
        private System.Windows.Forms.Label label25;
        private System.Windows.Forms.TextBox SecurityWorkingFolderTextBox;
        private System.Windows.Forms.Label label24;
        private System.Windows.Forms.TextBox SecuriytStartProgramTextbox;
        private System.Windows.Forms.CheckBox SecuritySettingsEnabledCheckbox;
        private System.Windows.Forms.CheckBox chkConnectToConsole;
        private System.Windows.Forms.CheckBox chkTSGWlocalBypass;
        private System.Windows.Forms.Label label20;
        private System.Windows.Forms.ComboBox cmbTSGWLogonMethod;
        private System.Windows.Forms.GroupBox ExecuteGroupBox;
        private System.Windows.Forms.TextBox txtInitialDirectory;
        private System.Windows.Forms.TextBox txtArguments;
        private System.Windows.Forms.TextBox txtCommand;
        private System.Windows.Forms.Label label13;
        private System.Windows.Forms.CheckBox chkExecuteBeforeConnect;
        private System.Windows.Forms.Label label12;
        private System.Windows.Forms.CheckBox chkWaitForExit;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.GroupBox IcaGroupBox;
        private System.Windows.Forms.Button ClientINIBrowseButton;
        private System.Windows.Forms.Button ServerINIBrowseButton;
        private System.Windows.Forms.Button AppWorkingFolderBrowseButton;
        private System.Windows.Forms.Button appPathBrowseButton;
        private System.Windows.Forms.ComboBox ICAEncryptionLevelCombobox;
        private System.Windows.Forms.CheckBox ICAEnableEncryptionCheckbox;
        private System.Windows.Forms.TextBox ICAClientINI;
        private System.Windows.Forms.TextBox ICAServerINI;
        private System.Windows.Forms.TextBox ICAWorkingFolder;
        private System.Windows.Forms.TextBox ICAApplicationPath;
        private System.Windows.Forms.TextBox ICAApplicationNameTextBox;
        private System.Windows.Forms.Label label35;
        private System.Windows.Forms.Label label34;
        private System.Windows.Forms.Label label23;
        private System.Windows.Forms.Label label22;
        private System.Windows.Forms.Label label21;
        private System.Windows.Forms.GroupBox VmrcGroupBox;
        private System.Windows.Forms.CheckBox VMRCReducedColorsCheckbox;
        private System.Windows.Forms.CheckBox VMRCAdminModeCheckbox;
        private System.Windows.Forms.GroupBox VncGroupBox;
        private System.Windows.Forms.CheckBox VncViewOnlyCheckbox;
        private System.Windows.Forms.NumericUpDown vncDisplayNumberInput;
        private System.Windows.Forms.Label label37;
        private System.Windows.Forms.CheckBox vncAutoScaleCheckbox;
        private System.Windows.Forms.GroupBox SshGroupBox;
        private SSHClient.Preferences SSHPreferences;
        private System.Windows.Forms.GroupBox GeneralGroupBox;
        private System.Windows.Forms.TextBox httpUrlTextBox;
        private System.Windows.Forms.TextBox NotesTextbox;
        private System.Windows.Forms.TextBox txtPort;
        private System.Windows.Forms.TextBox txtName;
        private System.Windows.Forms.Label label15;
        private System.Windows.Forms.ComboBox CredentialDropdown;
        private System.Windows.Forms.PictureBox CredentialManagerPicturebox;
        private System.Windows.Forms.Panel CredentialsPanel;
        private System.Windows.Forms.ComboBox cmbUsers;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ComboBox cmbDomains;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox txtPassword;
        private System.Windows.Forms.CheckBox chkSavePassword;
        private System.Windows.Forms.Label label36;
        private System.Windows.Forms.PictureBox pictureBox2;
        private System.Windows.Forms.Label lblPort;
        private System.Windows.Forms.ComboBox ProtocolComboBox;
        private System.Windows.Forms.Label ProtocolLabel;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.ComboBox cmbServers;
        private System.Windows.Forms.Label lblServerName;
        private System.Windows.Forms.GroupBox groupBox4;
        private System.Windows.Forms.Button AllTagsAddButton;
        private System.Windows.Forms.ListView AllTagsListView;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.Button btnRemoveTag;
        private System.Windows.Forms.ListView lvConnectionTags;
        private System.Windows.Forms.Panel panel3;
        private System.Windows.Forms.TextBox txtTag;
        private System.Windows.Forms.Button btnAddNewTag;
        private System.Windows.Forms.Label label14;
    }
}
