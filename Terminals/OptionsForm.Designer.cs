namespace Terminals
{
    partial class OptionsForm
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
            this.btnOk = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.linkLabel1 = new System.Windows.Forms.LinkLabel();
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tpGeneral = new System.Windows.Forms.TabPage();
            this.autoSwitchToCaptureCheckbox = new System.Windows.Forms.CheckBox();
            this.warnDisconnectCheckBox = new System.Windows.Forms.CheckBox();
            this.validateServerNamesCheckbox = new System.Windows.Forms.CheckBox();
            this.MinimizeToTrayCheckbox = new System.Windows.Forms.CheckBox();
            this.PortscanTimeoutTextBox = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.chkSaveConnections = new System.Windows.Forms.CheckBox();
            this.chkShowConfirmDialog = new System.Windows.Forms.CheckBox();
            this.chkSingleInstance = new System.Windows.Forms.CheckBox();
            this.lblEvaluatedDesktopShare = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.txtDefaultDesktopShare = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.chkShowFullInfo = new System.Windows.Forms.CheckBox();
            this.chkShowUserNameInTitle = new System.Windows.Forms.CheckBox();
            this.chkShowInformationToolTips = new System.Windows.Forms.CheckBox();
            this.tpExecuteBeforeConnect = new System.Windows.Forms.TabPage();
            this.txtInitialDirectory = new System.Windows.Forms.TextBox();
            this.label13 = new System.Windows.Forms.Label();
            this.chkExecuteBeforeConnect = new System.Windows.Forms.CheckBox();
            this.txtArguments = new System.Windows.Forms.TextBox();
            this.label12 = new System.Windows.Forms.Label();
            this.chkWaitForExit = new System.Windows.Forms.CheckBox();
            this.txtCommand = new System.Windows.Forms.TextBox();
            this.label11 = new System.Windows.Forms.Label();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.PasswordsMatchLabel = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.ConfirmPasswordTextBox = new System.Windows.Forms.TextBox();
            this.PasswordTextbox = new System.Windows.Forms.TextBox();
            this.PasswordProtectTerminalsCheckbox = new System.Windows.Forms.CheckBox();
            this.office2007FeelCheckbox = new System.Windows.Forms.CheckBox();
            this.tabControl1.SuspendLayout();
            this.tpGeneral.SuspendLayout();
            this.tpExecuteBeforeConnect.SuspendLayout();
            this.tabPage1.SuspendLayout();
            this.SuspendLayout();
            // 
            // btnOk
            // 
            this.btnOk.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnOk.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.btnOk.Location = new System.Drawing.Point(233, 369);
            this.btnOk.Name = "btnOk";
            this.btnOk.Size = new System.Drawing.Size(80, 24);
            this.btnOk.TabIndex = 1;
            this.btnOk.Text = "OK";
            this.btnOk.UseVisualStyleBackColor = true;
            this.btnOk.Click += new System.EventHandler(this.btnOk_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.Location = new System.Drawing.Point(321, 369);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(80, 24);
            this.btnCancel.TabIndex = 2;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            // 
            // label1
            // 
            this.label1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(81, 301);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(241, 39);
            this.label1.TabIndex = 3;
            this.label1.Text = "More options coming soon to a version near you.\r\nHave a suggestion? submit a feat" +
                "ure request \r\nthrough the Terminals website:\r\n";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // linkLabel1
            // 
            this.linkLabel1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.linkLabel1.AutoSize = true;
            this.linkLabel1.Location = new System.Drawing.Point(111, 346);
            this.linkLabel1.Name = "linkLabel1";
            this.linkLabel1.Size = new System.Drawing.Size(182, 13);
            this.linkLabel1.TabIndex = 4;
            this.linkLabel1.TabStop = true;
            this.linkLabel1.Text = "http://www.codeplex.com/Terminals";
            this.linkLabel1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // tabControl1
            // 
            this.tabControl1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.tabControl1.Controls.Add(this.tpGeneral);
            this.tabControl1.Controls.Add(this.tpExecuteBeforeConnect);
            this.tabControl1.Controls.Add(this.tabPage1);
            this.tabControl1.Location = new System.Drawing.Point(8, 8);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(400, 290);
            this.tabControl1.TabIndex = 5;
            // 
            // tpGeneral
            // 
            this.tpGeneral.Controls.Add(this.office2007FeelCheckbox);
            this.tpGeneral.Controls.Add(this.autoSwitchToCaptureCheckbox);
            this.tpGeneral.Controls.Add(this.warnDisconnectCheckBox);
            this.tpGeneral.Controls.Add(this.validateServerNamesCheckbox);
            this.tpGeneral.Controls.Add(this.MinimizeToTrayCheckbox);
            this.tpGeneral.Controls.Add(this.PortscanTimeoutTextBox);
            this.tpGeneral.Controls.Add(this.label5);
            this.tpGeneral.Controls.Add(this.label4);
            this.tpGeneral.Controls.Add(this.chkSaveConnections);
            this.tpGeneral.Controls.Add(this.chkShowConfirmDialog);
            this.tpGeneral.Controls.Add(this.chkSingleInstance);
            this.tpGeneral.Controls.Add(this.lblEvaluatedDesktopShare);
            this.tpGeneral.Controls.Add(this.label3);
            this.tpGeneral.Controls.Add(this.txtDefaultDesktopShare);
            this.tpGeneral.Controls.Add(this.label2);
            this.tpGeneral.Controls.Add(this.chkShowFullInfo);
            this.tpGeneral.Controls.Add(this.chkShowUserNameInTitle);
            this.tpGeneral.Controls.Add(this.chkShowInformationToolTips);
            this.tpGeneral.Location = new System.Drawing.Point(4, 22);
            this.tpGeneral.Name = "tpGeneral";
            this.tpGeneral.Padding = new System.Windows.Forms.Padding(3);
            this.tpGeneral.Size = new System.Drawing.Size(392, 264);
            this.tpGeneral.TabIndex = 0;
            this.tpGeneral.Text = "General";
            this.tpGeneral.UseVisualStyleBackColor = true;
            // 
            // autoSwitchToCaptureCheckbox
            // 
            this.autoSwitchToCaptureCheckbox.AutoSize = true;
            this.autoSwitchToCaptureCheckbox.Checked = true;
            this.autoSwitchToCaptureCheckbox.CheckState = System.Windows.Forms.CheckState.Checked;
            this.autoSwitchToCaptureCheckbox.Location = new System.Drawing.Point(253, 80);
            this.autoSwitchToCaptureCheckbox.Name = "autoSwitchToCaptureCheckbox";
            this.autoSwitchToCaptureCheckbox.Size = new System.Drawing.Size(139, 17);
            this.autoSwitchToCaptureCheckbox.TabIndex = 20;
            this.autoSwitchToCaptureCheckbox.Text = "Auto switch on Capture";
            this.autoSwitchToCaptureCheckbox.UseVisualStyleBackColor = true;
            // 
            // warnDisconnectCheckBox
            // 
            this.warnDisconnectCheckBox.AutoSize = true;
            this.warnDisconnectCheckBox.Checked = true;
            this.warnDisconnectCheckBox.CheckState = System.Windows.Forms.CheckState.Checked;
            this.warnDisconnectCheckBox.Location = new System.Drawing.Point(253, 53);
            this.warnDisconnectCheckBox.Name = "warnDisconnectCheckBox";
            this.warnDisconnectCheckBox.Size = new System.Drawing.Size(121, 17);
            this.warnDisconnectCheckBox.TabIndex = 19;
            this.warnDisconnectCheckBox.Text = "Warn on disconnect";
            this.warnDisconnectCheckBox.UseVisualStyleBackColor = true;
            // 
            // validateServerNamesCheckbox
            // 
            this.validateServerNamesCheckbox.AutoSize = true;
            this.validateServerNamesCheckbox.Checked = true;
            this.validateServerNamesCheckbox.CheckState = System.Windows.Forms.CheckState.Checked;
            this.validateServerNamesCheckbox.Location = new System.Drawing.Point(253, 30);
            this.validateServerNamesCheckbox.Name = "validateServerNamesCheckbox";
            this.validateServerNamesCheckbox.Size = new System.Drawing.Size(134, 17);
            this.validateServerNamesCheckbox.TabIndex = 18;
            this.validateServerNamesCheckbox.Text = "Validate Server Names";
            this.validateServerNamesCheckbox.UseVisualStyleBackColor = true;
            // 
            // MinimizeToTrayCheckbox
            // 
            this.MinimizeToTrayCheckbox.AutoSize = true;
            this.MinimizeToTrayCheckbox.Checked = true;
            this.MinimizeToTrayCheckbox.CheckState = System.Windows.Forms.CheckState.Checked;
            this.MinimizeToTrayCheckbox.Location = new System.Drawing.Point(253, 6);
            this.MinimizeToTrayCheckbox.Name = "MinimizeToTrayCheckbox";
            this.MinimizeToTrayCheckbox.Size = new System.Drawing.Size(105, 17);
            this.MinimizeToTrayCheckbox.TabIndex = 17;
            this.MinimizeToTrayCheckbox.Text = "Minimize To Tray";
            this.MinimizeToTrayCheckbox.UseVisualStyleBackColor = true;
            // 
            // PortscanTimeoutTextBox
            // 
            this.PortscanTimeoutTextBox.Location = new System.Drawing.Point(130, 234);
            this.PortscanTimeoutTextBox.Name = "PortscanTimeoutTextBox";
            this.PortscanTimeoutTextBox.Size = new System.Drawing.Size(58, 21);
            this.PortscanTimeoutTextBox.TabIndex = 16;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(197, 237);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(51, 13);
            this.label5.TabIndex = 15;
            this.label5.Text = "Seconds.";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(10, 237);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(114, 13);
            this.label4.TabIndex = 14;
            this.label4.Text = "Port Scanner Timeout:";
            // 
            // chkSaveConnections
            // 
            this.chkSaveConnections.AutoSize = true;
            this.chkSaveConnections.Checked = true;
            this.chkSaveConnections.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkSaveConnections.Location = new System.Drawing.Point(16, 128);
            this.chkSaveConnections.Name = "chkSaveConnections";
            this.chkSaveConnections.Size = new System.Drawing.Size(152, 17);
            this.chkSaveConnections.TabIndex = 5;
            this.chkSaveConnections.Text = "Sa&ve connections on close";
            this.chkSaveConnections.UseVisualStyleBackColor = true;
            // 
            // chkShowConfirmDialog
            // 
            this.chkShowConfirmDialog.AutoSize = true;
            this.chkShowConfirmDialog.Checked = true;
            this.chkShowConfirmDialog.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkShowConfirmDialog.Location = new System.Drawing.Point(16, 104);
            this.chkShowConfirmDialog.Name = "chkShowConfirmDialog";
            this.chkShowConfirmDialog.Size = new System.Drawing.Size(172, 17);
            this.chkShowConfirmDialog.TabIndex = 4;
            this.chkShowConfirmDialog.Text = "Show &close confirmation dialog";
            this.chkShowConfirmDialog.UseVisualStyleBackColor = true;
            // 
            // chkSingleInstance
            // 
            this.chkSingleInstance.AutoSize = true;
            this.chkSingleInstance.Checked = true;
            this.chkSingleInstance.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkSingleInstance.Location = new System.Drawing.Point(16, 80);
            this.chkSingleInstance.Name = "chkSingleInstance";
            this.chkSingleInstance.Size = new System.Drawing.Size(219, 17);
            this.chkSingleInstance.TabIndex = 3;
            this.chkSingleInstance.Text = "Allow a &single instance of the application";
            this.chkSingleInstance.UseVisualStyleBackColor = true;
            // 
            // lblEvaluatedDesktopShare
            // 
            this.lblEvaluatedDesktopShare.AutoSize = true;
            this.lblEvaluatedDesktopShare.ForeColor = System.Drawing.Color.Blue;
            this.lblEvaluatedDesktopShare.Location = new System.Drawing.Point(16, 192);
            this.lblEvaluatedDesktopShare.Name = "lblEvaluatedDesktopShare";
            this.lblEvaluatedDesktopShare.Size = new System.Drawing.Size(0, 13);
            this.lblEvaluatedDesktopShare.TabIndex = 13;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(13, 213);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(264, 13);
            this.label3.TabIndex = 8;
            this.label3.Text = "Evaluated Desktop Share (according to selected tab):";
            // 
            // txtDefaultDesktopShare
            // 
            this.txtDefaultDesktopShare.Location = new System.Drawing.Point(13, 181);
            this.txtDefaultDesktopShare.Name = "txtDefaultDesktopShare";
            this.txtDefaultDesktopShare.Size = new System.Drawing.Size(360, 21);
            this.txtDefaultDesktopShare.TabIndex = 7;
            this.txtDefaultDesktopShare.TextChanged += new System.EventHandler(this.txtDefaultDesktopShare_TextChanged);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(13, 157);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(283, 13);
            this.label2.TabIndex = 6;
            this.label2.Text = "Default Desktop Share (Use %SERVER% and %USER%):";
            // 
            // chkShowFullInfo
            // 
            this.chkShowFullInfo.AutoSize = true;
            this.chkShowFullInfo.Location = new System.Drawing.Point(40, 53);
            this.chkShowFullInfo.Name = "chkShowFullInfo";
            this.chkShowFullInfo.Size = new System.Drawing.Size(126, 17);
            this.chkShowFullInfo.TabIndex = 2;
            this.chkShowFullInfo.Text = "Show &full information";
            this.chkShowFullInfo.UseVisualStyleBackColor = true;
            // 
            // chkShowUserNameInTitle
            // 
            this.chkShowUserNameInTitle.AutoSize = true;
            this.chkShowUserNameInTitle.Checked = true;
            this.chkShowUserNameInTitle.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkShowUserNameInTitle.Location = new System.Drawing.Point(16, 7);
            this.chkShowUserNameInTitle.Name = "chkShowUserNameInTitle";
            this.chkShowUserNameInTitle.Size = new System.Drawing.Size(159, 17);
            this.chkShowUserNameInTitle.TabIndex = 0;
            this.chkShowUserNameInTitle.Text = "Show  &user name in tab title";
            this.chkShowUserNameInTitle.UseVisualStyleBackColor = true;
            // 
            // chkShowInformationToolTips
            // 
            this.chkShowInformationToolTips.AutoSize = true;
            this.chkShowInformationToolTips.Checked = true;
            this.chkShowInformationToolTips.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkShowInformationToolTips.Location = new System.Drawing.Point(16, 30);
            this.chkShowInformationToolTips.Name = "chkShowInformationToolTips";
            this.chkShowInformationToolTips.Size = new System.Drawing.Size(216, 17);
            this.chkShowInformationToolTips.TabIndex = 1;
            this.chkShowInformationToolTips.Text = "Show &connection information in tool tips";
            this.chkShowInformationToolTips.UseVisualStyleBackColor = true;
            this.chkShowInformationToolTips.CheckedChanged += new System.EventHandler(this.chkShowInformationToolTips_CheckedChanged);
            // 
            // tpExecuteBeforeConnect
            // 
            this.tpExecuteBeforeConnect.Controls.Add(this.txtInitialDirectory);
            this.tpExecuteBeforeConnect.Controls.Add(this.label13);
            this.tpExecuteBeforeConnect.Controls.Add(this.chkExecuteBeforeConnect);
            this.tpExecuteBeforeConnect.Controls.Add(this.txtArguments);
            this.tpExecuteBeforeConnect.Controls.Add(this.label12);
            this.tpExecuteBeforeConnect.Controls.Add(this.chkWaitForExit);
            this.tpExecuteBeforeConnect.Controls.Add(this.txtCommand);
            this.tpExecuteBeforeConnect.Controls.Add(this.label11);
            this.tpExecuteBeforeConnect.Location = new System.Drawing.Point(4, 22);
            this.tpExecuteBeforeConnect.Name = "tpExecuteBeforeConnect";
            this.tpExecuteBeforeConnect.Padding = new System.Windows.Forms.Padding(3);
            this.tpExecuteBeforeConnect.Size = new System.Drawing.Size(392, 264);
            this.tpExecuteBeforeConnect.TabIndex = 1;
            this.tpExecuteBeforeConnect.Text = "Execute Before Connect";
            this.tpExecuteBeforeConnect.UseVisualStyleBackColor = true;
            // 
            // txtInitialDirectory
            // 
            this.txtInitialDirectory.Location = new System.Drawing.Point(112, 88);
            this.txtInitialDirectory.Name = "txtInitialDirectory";
            this.txtInitialDirectory.Size = new System.Drawing.Size(265, 21);
            this.txtInitialDirectory.TabIndex = 14;
            // 
            // label13
            // 
            this.label13.AutoSize = true;
            this.label13.Location = new System.Drawing.Point(16, 88);
            this.label13.Name = "label13";
            this.label13.Size = new System.Drawing.Size(84, 13);
            this.label13.TabIndex = 18;
            this.label13.Text = "Initial Directory:";
            // 
            // chkExecuteBeforeConnect
            // 
            this.chkExecuteBeforeConnect.AutoSize = true;
            this.chkExecuteBeforeConnect.Location = new System.Drawing.Point(16, 8);
            this.chkExecuteBeforeConnect.Name = "chkExecuteBeforeConnect";
            this.chkExecuteBeforeConnect.Size = new System.Drawing.Size(141, 17);
            this.chkExecuteBeforeConnect.TabIndex = 11;
            this.chkExecuteBeforeConnect.Text = "&Execute before connect";
            this.chkExecuteBeforeConnect.UseVisualStyleBackColor = true;
            // 
            // txtArguments
            // 
            this.txtArguments.Location = new System.Drawing.Point(112, 64);
            this.txtArguments.Name = "txtArguments";
            this.txtArguments.Size = new System.Drawing.Size(265, 21);
            this.txtArguments.TabIndex = 13;
            // 
            // label12
            // 
            this.label12.AutoSize = true;
            this.label12.Location = new System.Drawing.Point(16, 64);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(59, 13);
            this.label12.TabIndex = 17;
            this.label12.Text = "Arguments";
            // 
            // chkWaitForExit
            // 
            this.chkWaitForExit.AutoSize = true;
            this.chkWaitForExit.Location = new System.Drawing.Point(16, 120);
            this.chkWaitForExit.Name = "chkWaitForExit";
            this.chkWaitForExit.Size = new System.Drawing.Size(86, 17);
            this.chkWaitForExit.TabIndex = 16;
            this.chkWaitForExit.Text = "&Wait for exit";
            this.chkWaitForExit.UseVisualStyleBackColor = true;
            // 
            // txtCommand
            // 
            this.txtCommand.Location = new System.Drawing.Point(112, 40);
            this.txtCommand.Name = "txtCommand";
            this.txtCommand.Size = new System.Drawing.Size(265, 21);
            this.txtCommand.TabIndex = 12;
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Location = new System.Drawing.Point(16, 40);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(58, 13);
            this.label11.TabIndex = 15;
            this.label11.Text = "Command:";
            // 
            // tabPage1
            // 
            this.tabPage1.Controls.Add(this.PasswordsMatchLabel);
            this.tabPage1.Controls.Add(this.label7);
            this.tabPage1.Controls.Add(this.label6);
            this.tabPage1.Controls.Add(this.ConfirmPasswordTextBox);
            this.tabPage1.Controls.Add(this.PasswordTextbox);
            this.tabPage1.Controls.Add(this.PasswordProtectTerminalsCheckbox);
            this.tabPage1.Location = new System.Drawing.Point(4, 22);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Size = new System.Drawing.Size(392, 264);
            this.tabPage1.TabIndex = 2;
            this.tabPage1.Text = "Security";
            this.tabPage1.UseVisualStyleBackColor = true;
            // 
            // PasswordsMatchLabel
            // 
            this.PasswordsMatchLabel.AutoSize = true;
            this.PasswordsMatchLabel.Location = new System.Drawing.Point(239, 73);
            this.PasswordsMatchLabel.Name = "PasswordsMatchLabel";
            this.PasswordsMatchLabel.Size = new System.Drawing.Size(0, 13);
            this.PasswordsMatchLabel.TabIndex = 5;
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(8, 69);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(48, 13);
            this.label7.TabIndex = 4;
            this.label7.Text = "Confirm:";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(8, 41);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(57, 13);
            this.label6.TabIndex = 3;
            this.label6.Text = "Password:";
            // 
            // ConfirmPasswordTextBox
            // 
            this.ConfirmPasswordTextBox.Enabled = false;
            this.ConfirmPasswordTextBox.Location = new System.Drawing.Point(72, 66);
            this.ConfirmPasswordTextBox.Name = "ConfirmPasswordTextBox";
            this.ConfirmPasswordTextBox.PasswordChar = '*';
            this.ConfirmPasswordTextBox.Size = new System.Drawing.Size(147, 21);
            this.ConfirmPasswordTextBox.TabIndex = 2;
            this.ConfirmPasswordTextBox.TextChanged += new System.EventHandler(this.ConfirmPasswordTextBox_TextChanged);
            // 
            // PasswordTextbox
            // 
            this.PasswordTextbox.Enabled = false;
            this.PasswordTextbox.Location = new System.Drawing.Point(72, 38);
            this.PasswordTextbox.Name = "PasswordTextbox";
            this.PasswordTextbox.PasswordChar = '*';
            this.PasswordTextbox.Size = new System.Drawing.Size(147, 21);
            this.PasswordTextbox.TabIndex = 1;
            this.PasswordTextbox.TextChanged += new System.EventHandler(this.PasswordTextbox_TextChanged);
            // 
            // PasswordProtectTerminalsCheckbox
            // 
            this.PasswordProtectTerminalsCheckbox.AutoSize = true;
            this.PasswordProtectTerminalsCheckbox.Location = new System.Drawing.Point(14, 15);
            this.PasswordProtectTerminalsCheckbox.Name = "PasswordProtectTerminalsCheckbox";
            this.PasswordProtectTerminalsCheckbox.Size = new System.Drawing.Size(110, 17);
            this.PasswordProtectTerminalsCheckbox.TabIndex = 0;
            this.PasswordProtectTerminalsCheckbox.Text = "Password Protect";
            this.PasswordProtectTerminalsCheckbox.UseVisualStyleBackColor = true;
            this.PasswordProtectTerminalsCheckbox.CheckedChanged += new System.EventHandler(this.PasswordProtectTerminalsCheckbox_CheckedChanged);
            // 
            // office2007FeelCheckbox
            // 
            this.office2007FeelCheckbox.AutoSize = true;
            this.office2007FeelCheckbox.Location = new System.Drawing.Point(253, 104);
            this.office2007FeelCheckbox.Name = "office2007FeelCheckbox";
            this.office2007FeelCheckbox.Size = new System.Drawing.Size(105, 17);
            this.office2007FeelCheckbox.TabIndex = 21;
            this.office2007FeelCheckbox.Text = "Office 2007 Feel";
            this.office2007FeelCheckbox.UseVisualStyleBackColor = true;
            // 
            // OptionsForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(413, 402);
            this.Controls.Add(this.tabControl1);
            this.Controls.Add(this.linkLabel1);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.btnOk);
            this.Controls.Add(this.btnCancel);
            this.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(177)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "OptionsForm";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Options";
            this.tabControl1.ResumeLayout(false);
            this.tpGeneral.ResumeLayout(false);
            this.tpGeneral.PerformLayout();
            this.tpExecuteBeforeConnect.ResumeLayout(false);
            this.tpExecuteBeforeConnect.PerformLayout();
            this.tabPage1.ResumeLayout(false);
            this.tabPage1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnOk;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.LinkLabel linkLabel1;
        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tpGeneral;
        private System.Windows.Forms.Label lblEvaluatedDesktopShare;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox txtDefaultDesktopShare;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.CheckBox chkShowFullInfo;
        private System.Windows.Forms.CheckBox chkShowUserNameInTitle;
        private System.Windows.Forms.CheckBox chkShowInformationToolTips;
        private System.Windows.Forms.TabPage tpExecuteBeforeConnect;
        private System.Windows.Forms.TextBox txtInitialDirectory;
        private System.Windows.Forms.Label label13;
        private System.Windows.Forms.CheckBox chkExecuteBeforeConnect;
        private System.Windows.Forms.TextBox txtArguments;
        private System.Windows.Forms.Label label12;
        private System.Windows.Forms.CheckBox chkWaitForExit;
        private System.Windows.Forms.TextBox txtCommand;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.CheckBox chkSingleInstance;
        private System.Windows.Forms.CheckBox chkShowConfirmDialog;
        private System.Windows.Forms.CheckBox chkSaveConnections;
        private System.Windows.Forms.TextBox PortscanTimeoutTextBox;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TabPage tabPage1;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.TextBox ConfirmPasswordTextBox;
        private System.Windows.Forms.TextBox PasswordTextbox;
        private System.Windows.Forms.CheckBox PasswordProtectTerminalsCheckbox;
        private System.Windows.Forms.Label PasswordsMatchLabel;
        private System.Windows.Forms.CheckBox MinimizeToTrayCheckbox;
        private System.Windows.Forms.CheckBox validateServerNamesCheckbox;
        private System.Windows.Forms.CheckBox warnDisconnectCheckBox;
        private System.Windows.Forms.CheckBox autoSwitchToCaptureCheckbox;
        private System.Windows.Forms.CheckBox office2007FeelCheckbox;
    }
}