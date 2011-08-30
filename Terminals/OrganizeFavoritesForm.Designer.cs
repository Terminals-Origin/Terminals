namespace Terminals
{
    partial class OrganizeFavoritesForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(OrganizeFavoritesForm));
            this.btnClose = new System.Windows.Forms.Button();
            this.btnEdit = new System.Windows.Forms.Button();
            this.btnDelete = new System.Windows.Forms.Button();
            this.btnCopy = new System.Windows.Forms.Button();
            this.btnRename = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.btnNew = new System.Windows.Forms.Button();
            this.ImportButton = new System.Windows.Forms.Button();
            this.ImportOpenFileDialog = new System.Windows.Forms.OpenFileDialog();
            this.ActiveDirectoryButton = new System.Windows.Forms.Button();
            this.ScanNetworkButton = new System.Windows.Forms.Button();
            this.btnExport = new System.Windows.Forms.Button();
            this.dataGridFavorites = new Terminals.SortableUnboundGrid();
            this.colName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colComputer = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colProtocol = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colUserName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colCredentials = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colTags = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colNotes = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.bsFavorites = new System.Windows.Forms.BindingSource(this.components);
            this.performanceFlagsDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.telnetDataGridViewCheckBoxColumn = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.telnetRowsDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.telnetColsDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.shutdownTimeoutDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.overallTimeoutDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.connectionTimeoutDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.idleTimeoutDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.securityWorkingFolderDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.securityStartProgramDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.credentialDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.securityFullScreenDataGridViewCheckBoxColumn = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.enableSecuritySettingsDataGridViewCheckBoxColumn = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.grabFocusOnConnectDataGridViewCheckBoxColumn = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.enableEncryptionDataGridViewCheckBoxColumn = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.disableWindowsKeyDataGridViewCheckBoxColumn = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.doubleClickDetectDataGridViewCheckBoxColumn = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.displayConnectionBarDataGridViewCheckBoxColumn = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.disableControlAltDeleteDataGridViewCheckBoxColumn = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.acceleratorPassthroughDataGridViewCheckBoxColumn = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.enableCompressionDataGridViewCheckBoxColumn = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.bitmapPeristenceDataGridViewCheckBoxColumn = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.enableTLSAuthenticationDataGridViewCheckBoxColumn = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.enableNLAAuthenticationDataGridViewCheckBoxColumn = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.allowBackgroundInputDataGridViewCheckBoxColumn = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.iCAApplicationNameDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.iCAApplicationWorkingFolderDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.iCAApplicationPathDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.vMRCReducedColorsModeDataGridViewCheckBoxColumn = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.vMRCAdministratorModeDataGridViewCheckBoxColumn = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.sSH1DataGridViewCheckBoxColumn = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.consoleRowsDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.consoleColsDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.consoleFontDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.consoleBackColorDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.consoleTextColorDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.consoleCursorColorDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.protocolDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.toolBarIconDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.telnetFontDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.telnetBackColorDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.telnetTextColorDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.telnetCursorColorDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.nameDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.serverNameDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.domainNameDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.authMethodDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.keyTagDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.userNameDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.encryptedPasswordDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.passwordDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.vncAutoScaleDataGridViewCheckBoxColumn = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.vncViewOnlyDataGridViewCheckBoxColumn = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.vncDisplayNumberDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.connectToConsoleDataGridViewCheckBoxColumn = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.desktopSizeHeightDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.desktopSizeWidthDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.desktopSizeDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colorsDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.soundsDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.redirectedDrivesDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.redirectPortsDataGridViewCheckBoxColumn = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.newWindowDataGridViewCheckBoxColumn = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.redirectPrintersDataGridViewCheckBoxColumn = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.redirectSmartCardsDataGridViewCheckBoxColumn = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.redirectClipboardDataGridViewCheckBoxColumn = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.redirectDevicesDataGridViewCheckBoxColumn = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.tsgwUsageMethodDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.tsgwHostnameDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.tsgwCredsSourceDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.tsgwSeparateLoginDataGridViewCheckBoxColumn = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.tsgwUsernameDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.tsgwDomainDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.tsgwEncryptedPasswordDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.tsgwPasswordDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.urlDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.notesDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.icaServerINIDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.icaClientINIDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.icaEnableEncryptionDataGridViewCheckBoxColumn = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.icaEncryptionLevelDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.portDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.desktopShareDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.executeBeforeConnectDataGridViewCheckBoxColumn = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.executeBeforeConnectCommandDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.executeBeforeConnectArgsDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.executeBeforeConnectInitialDirectoryDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.executeBeforeConnectWaitForExitDataGridViewCheckBoxColumn = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.disableThemingDataGridViewCheckBoxColumn = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.disableMenuAnimationsDataGridViewCheckBoxColumn = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.disableFullWindowDragDataGridViewCheckBoxColumn = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.disableCursorBlinkingDataGridViewCheckBoxColumn = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.enableDesktopCompositionDataGridViewCheckBoxColumn = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.enableFontSmoothingDataGridViewCheckBoxColumn = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.disableCursorShadowDataGridViewCheckBoxColumn = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.disableWallPaperDataGridViewCheckBoxColumn = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.tagsDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.lockAttributesDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.lockAllAttributesExceptDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.lockElementsDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.lockAllElementsExceptDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.lockItemDataGridViewCheckBoxColumn = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.elementInformationDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.currentConfigurationDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.performanceFlagsDataGridViewTextBoxColumn1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.telnetDataGridViewCheckBoxColumn1 = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.telnetRowsDataGridViewTextBoxColumn1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.telnetColsDataGridViewTextBoxColumn1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.shutdownTimeoutDataGridViewTextBoxColumn1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.overallTimeoutDataGridViewTextBoxColumn1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.connectionTimeoutDataGridViewTextBoxColumn1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.idleTimeoutDataGridViewTextBoxColumn1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.securityWorkingFolderDataGridViewTextBoxColumn1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.securityStartProgramDataGridViewTextBoxColumn1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.credentialDataGridViewTextBoxColumn1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.securityFullScreenDataGridViewCheckBoxColumn1 = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.enableSecuritySettingsDataGridViewCheckBoxColumn1 = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.grabFocusOnConnectDataGridViewCheckBoxColumn1 = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.enableEncryptionDataGridViewCheckBoxColumn1 = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.disableWindowsKeyDataGridViewCheckBoxColumn1 = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.doubleClickDetectDataGridViewCheckBoxColumn1 = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.displayConnectionBarDataGridViewCheckBoxColumn1 = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.disableControlAltDeleteDataGridViewCheckBoxColumn1 = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.acceleratorPassthroughDataGridViewCheckBoxColumn1 = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.enableCompressionDataGridViewCheckBoxColumn1 = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.bitmapPeristenceDataGridViewCheckBoxColumn1 = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.enableTLSAuthenticationDataGridViewCheckBoxColumn1 = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.enableNLAAuthenticationDataGridViewCheckBoxColumn1 = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.allowBackgroundInputDataGridViewCheckBoxColumn1 = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.iCAApplicationNameDataGridViewTextBoxColumn1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.iCAApplicationWorkingFolderDataGridViewTextBoxColumn1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.iCAApplicationPathDataGridViewTextBoxColumn1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.vMRCReducedColorsModeDataGridViewCheckBoxColumn1 = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.vMRCAdministratorModeDataGridViewCheckBoxColumn1 = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.sSH1DataGridViewCheckBoxColumn1 = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.consoleRowsDataGridViewTextBoxColumn1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.consoleColsDataGridViewTextBoxColumn1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.consoleFontDataGridViewTextBoxColumn1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.consoleBackColorDataGridViewTextBoxColumn1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.consoleTextColorDataGridViewTextBoxColumn1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.consoleCursorColorDataGridViewTextBoxColumn1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.protocolDataGridViewTextBoxColumn1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.toolBarIconDataGridViewTextBoxColumn1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.telnetFontDataGridViewTextBoxColumn1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.telnetBackColorDataGridViewTextBoxColumn1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.telnetTextColorDataGridViewTextBoxColumn1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.telnetCursorColorDataGridViewTextBoxColumn1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.nameDataGridViewTextBoxColumn1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.serverNameDataGridViewTextBoxColumn1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.domainNameDataGridViewTextBoxColumn1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.authMethodDataGridViewTextBoxColumn1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.keyTagDataGridViewTextBoxColumn1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.userNameDataGridViewTextBoxColumn1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.encryptedPasswordDataGridViewTextBoxColumn1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.passwordDataGridViewTextBoxColumn1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.vncAutoScaleDataGridViewCheckBoxColumn1 = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.vncViewOnlyDataGridViewCheckBoxColumn1 = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.vncDisplayNumberDataGridViewTextBoxColumn1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.connectToConsoleDataGridViewCheckBoxColumn1 = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.desktopSizeHeightDataGridViewTextBoxColumn1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.desktopSizeWidthDataGridViewTextBoxColumn1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.desktopSizeDataGridViewTextBoxColumn1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colorsDataGridViewTextBoxColumn1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.soundsDataGridViewTextBoxColumn1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.redirectedDrivesDataGridViewTextBoxColumn1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.redirectPortsDataGridViewCheckBoxColumn1 = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.newWindowDataGridViewCheckBoxColumn1 = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.redirectPrintersDataGridViewCheckBoxColumn1 = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.redirectSmartCardsDataGridViewCheckBoxColumn1 = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.redirectClipboardDataGridViewCheckBoxColumn1 = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.redirectDevicesDataGridViewCheckBoxColumn1 = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.tsgwUsageMethodDataGridViewTextBoxColumn1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.tsgwHostnameDataGridViewTextBoxColumn1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.tsgwCredsSourceDataGridViewTextBoxColumn1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.tsgwSeparateLoginDataGridViewCheckBoxColumn1 = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.tsgwUsernameDataGridViewTextBoxColumn1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.tsgwDomainDataGridViewTextBoxColumn1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.tsgwEncryptedPasswordDataGridViewTextBoxColumn1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.tsgwPasswordDataGridViewTextBoxColumn1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.urlDataGridViewTextBoxColumn1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.notesDataGridViewTextBoxColumn1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.icaServerINIDataGridViewTextBoxColumn1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.icaClientINIDataGridViewTextBoxColumn1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.icaEnableEncryptionDataGridViewCheckBoxColumn1 = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.icaEncryptionLevelDataGridViewTextBoxColumn1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.portDataGridViewTextBoxColumn1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.desktopShareDataGridViewTextBoxColumn1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.executeBeforeConnectDataGridViewCheckBoxColumn1 = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.executeBeforeConnectCommandDataGridViewTextBoxColumn1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.executeBeforeConnectArgsDataGridViewTextBoxColumn1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.executeBeforeConnectInitialDirectoryDataGridViewTextBoxColumn1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.executeBeforeConnectWaitForExitDataGridViewCheckBoxColumn1 = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.disableThemingDataGridViewCheckBoxColumn1 = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.disableMenuAnimationsDataGridViewCheckBoxColumn1 = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.disableFullWindowDragDataGridViewCheckBoxColumn1 = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.disableCursorBlinkingDataGridViewCheckBoxColumn1 = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.enableDesktopCompositionDataGridViewCheckBoxColumn1 = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.enableFontSmoothingDataGridViewCheckBoxColumn1 = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.disableCursorShadowDataGridViewCheckBoxColumn1 = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.disableWallPaperDataGridViewCheckBoxColumn1 = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.tagsDataGridViewTextBoxColumn1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.lockAttributesDataGridViewTextBoxColumn1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.lockAllAttributesExceptDataGridViewTextBoxColumn1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.lockElementsDataGridViewTextBoxColumn1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.lockAllElementsExceptDataGridViewTextBoxColumn1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.lockItemDataGridViewCheckBoxColumn1 = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.elementInformationDataGridViewTextBoxColumn1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.currentConfigurationDataGridViewTextBoxColumn1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridFavorites)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.bsFavorites)).BeginInit();
            this.SuspendLayout();
            // 
            // btnClose
            // 
            this.btnClose.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnClose.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnClose.Location = new System.Drawing.Point(613, 324);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(106, 25);
            this.btnClose.TabIndex = 11;
            this.btnClose.Text = "Clo&se";
            this.btnClose.UseVisualStyleBackColor = true;
            // 
            // btnEdit
            // 
            this.btnEdit.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnEdit.Location = new System.Drawing.Point(613, 55);
            this.btnEdit.Name = "btnEdit";
            this.btnEdit.Size = new System.Drawing.Size(105, 24);
            this.btnEdit.TabIndex = 3;
            this.btnEdit.Text = "&Edit...";
            this.btnEdit.UseVisualStyleBackColor = true;
            this.btnEdit.Click += new System.EventHandler(this.btnEdit_Click);
            // 
            // btnDelete
            // 
            this.btnDelete.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnDelete.Location = new System.Drawing.Point(613, 85);
            this.btnDelete.Name = "btnDelete";
            this.btnDelete.Size = new System.Drawing.Size(105, 24);
            this.btnDelete.TabIndex = 4;
            this.btnDelete.Text = "&Delete";
            this.btnDelete.UseVisualStyleBackColor = true;
            this.btnDelete.Click += new System.EventHandler(this.btnDelete_Click);
            // 
            // btnCopy
            // 
            this.btnCopy.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnCopy.Location = new System.Drawing.Point(613, 115);
            this.btnCopy.Name = "btnCopy";
            this.btnCopy.Size = new System.Drawing.Size(105, 24);
            this.btnCopy.TabIndex = 5;
            this.btnCopy.Text = "&Copy";
            this.btnCopy.UseVisualStyleBackColor = true;
            this.btnCopy.Click += new System.EventHandler(this.btnCopy_Click);
            // 
            // btnRename
            // 
            this.btnRename.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnRename.Location = new System.Drawing.Point(613, 145);
            this.btnRename.Name = "btnRename";
            this.btnRename.Size = new System.Drawing.Size(105, 24);
            this.btnRename.TabIndex = 6;
            this.btnRename.Text = "&Rename";
            this.btnRename.UseVisualStyleBackColor = true;
            this.btnRename.Click += new System.EventHandler(this.btnRename_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 9);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(84, 17);
            this.label1.TabIndex = 0;
            this.label1.Text = "Co&nnections";
            // 
            // btnNew
            // 
            this.btnNew.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnNew.Location = new System.Drawing.Point(613, 25);
            this.btnNew.Name = "btnNew";
            this.btnNew.Size = new System.Drawing.Size(105, 24);
            this.btnNew.TabIndex = 2;
            this.btnNew.Text = "&New...";
            this.btnNew.UseVisualStyleBackColor = true;
            this.btnNew.Click += new System.EventHandler(this.btnNew_Click);
            // 
            // ImportButton
            // 
            this.ImportButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.ImportButton.Location = new System.Drawing.Point(613, 234);
            this.ImportButton.Name = "ImportButton";
            this.ImportButton.Size = new System.Drawing.Size(105, 23);
            this.ImportButton.TabIndex = 9;
            this.ImportButton.Text = "&Import...";
            this.ImportButton.UseVisualStyleBackColor = true;
            this.ImportButton.Click += new System.EventHandler(this.ImportButton_Click);
            // 
            // ImportOpenFileDialog
            // 
            this.ImportOpenFileDialog.Multiselect = true;
            this.ImportOpenFileDialog.Title = "Import favorites from...";
            // 
            // ActiveDirectoryButton
            // 
            this.ActiveDirectoryButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.ActiveDirectoryButton.Location = new System.Drawing.Point(613, 175);
            this.ActiveDirectoryButton.Name = "ActiveDirectoryButton";
            this.ActiveDirectoryButton.Size = new System.Drawing.Size(105, 23);
            this.ActiveDirectoryButton.TabIndex = 7;
            this.ActiveDirectoryButton.Text = "Scan &AD";
            this.ActiveDirectoryButton.UseVisualStyleBackColor = true;
            this.ActiveDirectoryButton.Click += new System.EventHandler(this.activeDirectoryToolStripMenuItem_Click);
            // 
            // ScanNetworkButton
            // 
            this.ScanNetworkButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.ScanNetworkButton.Location = new System.Drawing.Point(613, 205);
            this.ScanNetworkButton.Name = "ScanNetworkButton";
            this.ScanNetworkButton.Size = new System.Drawing.Size(105, 23);
            this.ScanNetworkButton.TabIndex = 8;
            this.ScanNetworkButton.Text = "Scan &Network";
            this.ScanNetworkButton.UseVisualStyleBackColor = true;
            this.ScanNetworkButton.Click += new System.EventHandler(this.networkDetectionToolStripMenuItem_Click);
            // 
            // btnExport
            // 
            this.btnExport.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnExport.Location = new System.Drawing.Point(613, 264);
            this.btnExport.Name = "btnExport";
            this.btnExport.Size = new System.Drawing.Size(105, 23);
            this.btnExport.TabIndex = 10;
            this.btnExport.Text = "&Export...";
            this.btnExport.UseVisualStyleBackColor = true;
            this.btnExport.Click += new System.EventHandler(this.btnExport_Click);
            // 
            // dataGridFavorites
            // 
            this.dataGridFavorites.AllowUserToAddRows = false;
            this.dataGridFavorites.AllowUserToOrderColumns = true;
            this.dataGridFavorites.AllowUserToResizeRows = false;
            this.dataGridFavorites.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.dataGridFavorites.AutoGenerateColumns = false;
            this.dataGridFavorites.BackgroundColor = System.Drawing.SystemColors.Window;
            this.dataGridFavorites.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.dataGridFavorites.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridFavorites.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.colName,
            this.colComputer,
            this.colProtocol,
            this.colUserName,
            this.colCredentials,
            this.colTags,
            this.colNotes,
            this.performanceFlagsDataGridViewTextBoxColumn1,
            this.telnetDataGridViewCheckBoxColumn1,
            this.telnetRowsDataGridViewTextBoxColumn1,
            this.telnetColsDataGridViewTextBoxColumn1,
            this.shutdownTimeoutDataGridViewTextBoxColumn1,
            this.overallTimeoutDataGridViewTextBoxColumn1,
            this.connectionTimeoutDataGridViewTextBoxColumn1,
            this.idleTimeoutDataGridViewTextBoxColumn1,
            this.securityWorkingFolderDataGridViewTextBoxColumn1,
            this.securityStartProgramDataGridViewTextBoxColumn1,
            this.credentialDataGridViewTextBoxColumn1,
            this.securityFullScreenDataGridViewCheckBoxColumn1,
            this.enableSecuritySettingsDataGridViewCheckBoxColumn1,
            this.grabFocusOnConnectDataGridViewCheckBoxColumn1,
            this.enableEncryptionDataGridViewCheckBoxColumn1,
            this.disableWindowsKeyDataGridViewCheckBoxColumn1,
            this.doubleClickDetectDataGridViewCheckBoxColumn1,
            this.displayConnectionBarDataGridViewCheckBoxColumn1,
            this.disableControlAltDeleteDataGridViewCheckBoxColumn1,
            this.acceleratorPassthroughDataGridViewCheckBoxColumn1,
            this.enableCompressionDataGridViewCheckBoxColumn1,
            this.bitmapPeristenceDataGridViewCheckBoxColumn1,
            this.enableTLSAuthenticationDataGridViewCheckBoxColumn1,
            this.enableNLAAuthenticationDataGridViewCheckBoxColumn1,
            this.allowBackgroundInputDataGridViewCheckBoxColumn1,
            this.iCAApplicationNameDataGridViewTextBoxColumn1,
            this.iCAApplicationWorkingFolderDataGridViewTextBoxColumn1,
            this.iCAApplicationPathDataGridViewTextBoxColumn1,
            this.vMRCReducedColorsModeDataGridViewCheckBoxColumn1,
            this.vMRCAdministratorModeDataGridViewCheckBoxColumn1,
            this.sSH1DataGridViewCheckBoxColumn1,
            this.consoleRowsDataGridViewTextBoxColumn1,
            this.consoleColsDataGridViewTextBoxColumn1,
            this.consoleFontDataGridViewTextBoxColumn1,
            this.consoleBackColorDataGridViewTextBoxColumn1,
            this.consoleTextColorDataGridViewTextBoxColumn1,
            this.consoleCursorColorDataGridViewTextBoxColumn1,
            this.protocolDataGridViewTextBoxColumn1,
            this.toolBarIconDataGridViewTextBoxColumn1,
            this.telnetFontDataGridViewTextBoxColumn1,
            this.telnetBackColorDataGridViewTextBoxColumn1,
            this.telnetTextColorDataGridViewTextBoxColumn1,
            this.telnetCursorColorDataGridViewTextBoxColumn1,
            this.nameDataGridViewTextBoxColumn1,
            this.serverNameDataGridViewTextBoxColumn1,
            this.domainNameDataGridViewTextBoxColumn1,
            this.authMethodDataGridViewTextBoxColumn1,
            this.keyTagDataGridViewTextBoxColumn1,
            this.userNameDataGridViewTextBoxColumn1,
            this.encryptedPasswordDataGridViewTextBoxColumn1,
            this.passwordDataGridViewTextBoxColumn1,
            this.vncAutoScaleDataGridViewCheckBoxColumn1,
            this.vncViewOnlyDataGridViewCheckBoxColumn1,
            this.vncDisplayNumberDataGridViewTextBoxColumn1,
            this.connectToConsoleDataGridViewCheckBoxColumn1,
            this.desktopSizeHeightDataGridViewTextBoxColumn1,
            this.desktopSizeWidthDataGridViewTextBoxColumn1,
            this.desktopSizeDataGridViewTextBoxColumn1,
            this.colorsDataGridViewTextBoxColumn1,
            this.soundsDataGridViewTextBoxColumn1,
            this.redirectedDrivesDataGridViewTextBoxColumn1,
            this.redirectPortsDataGridViewCheckBoxColumn1,
            this.newWindowDataGridViewCheckBoxColumn1,
            this.redirectPrintersDataGridViewCheckBoxColumn1,
            this.redirectSmartCardsDataGridViewCheckBoxColumn1,
            this.redirectClipboardDataGridViewCheckBoxColumn1,
            this.redirectDevicesDataGridViewCheckBoxColumn1,
            this.tsgwUsageMethodDataGridViewTextBoxColumn1,
            this.tsgwHostnameDataGridViewTextBoxColumn1,
            this.tsgwCredsSourceDataGridViewTextBoxColumn1,
            this.tsgwSeparateLoginDataGridViewCheckBoxColumn1,
            this.tsgwUsernameDataGridViewTextBoxColumn1,
            this.tsgwDomainDataGridViewTextBoxColumn1,
            this.tsgwEncryptedPasswordDataGridViewTextBoxColumn1,
            this.tsgwPasswordDataGridViewTextBoxColumn1,
            this.urlDataGridViewTextBoxColumn1,
            this.notesDataGridViewTextBoxColumn1,
            this.icaServerINIDataGridViewTextBoxColumn1,
            this.icaClientINIDataGridViewTextBoxColumn1,
            this.icaEnableEncryptionDataGridViewCheckBoxColumn1,
            this.icaEncryptionLevelDataGridViewTextBoxColumn1,
            this.portDataGridViewTextBoxColumn1,
            this.desktopShareDataGridViewTextBoxColumn1,
            this.executeBeforeConnectDataGridViewCheckBoxColumn1,
            this.executeBeforeConnectCommandDataGridViewTextBoxColumn1,
            this.executeBeforeConnectArgsDataGridViewTextBoxColumn1,
            this.executeBeforeConnectInitialDirectoryDataGridViewTextBoxColumn1,
            this.executeBeforeConnectWaitForExitDataGridViewCheckBoxColumn1,
            this.disableThemingDataGridViewCheckBoxColumn1,
            this.disableMenuAnimationsDataGridViewCheckBoxColumn1,
            this.disableFullWindowDragDataGridViewCheckBoxColumn1,
            this.disableCursorBlinkingDataGridViewCheckBoxColumn1,
            this.enableDesktopCompositionDataGridViewCheckBoxColumn1,
            this.enableFontSmoothingDataGridViewCheckBoxColumn1,
            this.disableCursorShadowDataGridViewCheckBoxColumn1,
            this.disableWallPaperDataGridViewCheckBoxColumn1,
            this.tagsDataGridViewTextBoxColumn1,
            this.lockAttributesDataGridViewTextBoxColumn1,
            this.lockAllAttributesExceptDataGridViewTextBoxColumn1,
            this.lockElementsDataGridViewTextBoxColumn1,
            this.lockAllElementsExceptDataGridViewTextBoxColumn1,
            this.lockItemDataGridViewCheckBoxColumn1,
            this.elementInformationDataGridViewTextBoxColumn1,
            this.currentConfigurationDataGridViewTextBoxColumn1});
            this.dataGridFavorites.DataSource = this.bsFavorites;
            this.dataGridFavorites.Location = new System.Drawing.Point(12, 25);
            this.dataGridFavorites.Name = "dataGridFavorites";
            this.dataGridFavorites.RowHeadersVisible = false;
            this.dataGridFavorites.RowTemplate.Height = 20;
            this.dataGridFavorites.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dataGridFavorites.Size = new System.Drawing.Size(595, 324);
            this.dataGridFavorites.TabIndex = 12;
            this.dataGridFavorites.CellBeginEdit += new System.Windows.Forms.DataGridViewCellCancelEventHandler(this.dataGridFavorites_CellBeginEdit);
            this.dataGridFavorites.CellEndEdit += new System.Windows.Forms.DataGridViewCellEventHandler(this.dataGridFavorites_CellEndEdit);
            this.dataGridFavorites.ColumnHeaderMouseClick += new System.Windows.Forms.DataGridViewCellMouseEventHandler(this.dataGridFavorites_ColumnHeaderMouseClick);
            this.dataGridFavorites.DoubleClick += new System.EventHandler(this.btnEdit_Click);
            // 
            // colName
            // 
            this.colName.DataPropertyName = "Name";
            this.colName.HeaderText = "Name";
            this.colName.Name = "colName";
            // 
            // colComputer
            // 
            this.colComputer.DataPropertyName = "ServerName";
            this.colComputer.HeaderText = "Computer";
            this.colComputer.Name = "colComputer";
            this.colComputer.ReadOnly = true;
            // 
            // colProtocol
            // 
            this.colProtocol.DataPropertyName = "Protocol";
            this.colProtocol.HeaderText = "Protocol";
            this.colProtocol.Name = "colProtocol";
            this.colProtocol.ReadOnly = true;
            this.colProtocol.Width = 60;
            // 
            // colUserName
            // 
            this.colUserName.DataPropertyName = "UserName";
            this.colUserName.HeaderText = "UserName";
            this.colUserName.Name = "colUserName";
            this.colUserName.ReadOnly = true;
            this.colUserName.Width = 60;
            // 
            // colCredentials
            // 
            this.colCredentials.DataPropertyName = "Credential";
            this.colCredentials.HeaderText = "Credential";
            this.colCredentials.Name = "colCredentials";
            this.colCredentials.ReadOnly = true;
            this.colCredentials.Width = 60;
            // 
            // colTags
            // 
            this.colTags.DataPropertyName = "Tags";
            this.colTags.HeaderText = "Tags";
            this.colTags.Name = "colTags";
            this.colTags.ReadOnly = true;
            // 
            // colNotes
            // 
            this.colNotes.DataPropertyName = "Notes";
            this.colNotes.HeaderText = "Notes";
            this.colNotes.Name = "colNotes";
            this.colNotes.ReadOnly = true;
            // 
            // bsFavorites
            // 
            this.bsFavorites.DataSource = typeof(Terminals.FavoriteConfigurationElement);
            this.bsFavorites.Sort = "";
            // 
            // performanceFlagsDataGridViewTextBoxColumn
            // 
            this.performanceFlagsDataGridViewTextBoxColumn.DataPropertyName = "PerformanceFlags";
            this.performanceFlagsDataGridViewTextBoxColumn.HeaderText = "PerformanceFlags";
            this.performanceFlagsDataGridViewTextBoxColumn.Name = "performanceFlagsDataGridViewTextBoxColumn";
            this.performanceFlagsDataGridViewTextBoxColumn.ReadOnly = true;
            // 
            // telnetDataGridViewCheckBoxColumn
            // 
            this.telnetDataGridViewCheckBoxColumn.DataPropertyName = "Telnet";
            this.telnetDataGridViewCheckBoxColumn.HeaderText = "Telnet";
            this.telnetDataGridViewCheckBoxColumn.Name = "telnetDataGridViewCheckBoxColumn";
            // 
            // telnetRowsDataGridViewTextBoxColumn
            // 
            this.telnetRowsDataGridViewTextBoxColumn.DataPropertyName = "TelnetRows";
            this.telnetRowsDataGridViewTextBoxColumn.HeaderText = "TelnetRows";
            this.telnetRowsDataGridViewTextBoxColumn.Name = "telnetRowsDataGridViewTextBoxColumn";
            // 
            // telnetColsDataGridViewTextBoxColumn
            // 
            this.telnetColsDataGridViewTextBoxColumn.DataPropertyName = "TelnetCols";
            this.telnetColsDataGridViewTextBoxColumn.HeaderText = "TelnetCols";
            this.telnetColsDataGridViewTextBoxColumn.Name = "telnetColsDataGridViewTextBoxColumn";
            // 
            // shutdownTimeoutDataGridViewTextBoxColumn
            // 
            this.shutdownTimeoutDataGridViewTextBoxColumn.DataPropertyName = "ShutdownTimeout";
            this.shutdownTimeoutDataGridViewTextBoxColumn.HeaderText = "ShutdownTimeout";
            this.shutdownTimeoutDataGridViewTextBoxColumn.Name = "shutdownTimeoutDataGridViewTextBoxColumn";
            // 
            // overallTimeoutDataGridViewTextBoxColumn
            // 
            this.overallTimeoutDataGridViewTextBoxColumn.DataPropertyName = "OverallTimeout";
            this.overallTimeoutDataGridViewTextBoxColumn.HeaderText = "OverallTimeout";
            this.overallTimeoutDataGridViewTextBoxColumn.Name = "overallTimeoutDataGridViewTextBoxColumn";
            // 
            // connectionTimeoutDataGridViewTextBoxColumn
            // 
            this.connectionTimeoutDataGridViewTextBoxColumn.DataPropertyName = "ConnectionTimeout";
            this.connectionTimeoutDataGridViewTextBoxColumn.HeaderText = "ConnectionTimeout";
            this.connectionTimeoutDataGridViewTextBoxColumn.Name = "connectionTimeoutDataGridViewTextBoxColumn";
            // 
            // idleTimeoutDataGridViewTextBoxColumn
            // 
            this.idleTimeoutDataGridViewTextBoxColumn.DataPropertyName = "IdleTimeout";
            this.idleTimeoutDataGridViewTextBoxColumn.HeaderText = "IdleTimeout";
            this.idleTimeoutDataGridViewTextBoxColumn.Name = "idleTimeoutDataGridViewTextBoxColumn";
            // 
            // securityWorkingFolderDataGridViewTextBoxColumn
            // 
            this.securityWorkingFolderDataGridViewTextBoxColumn.DataPropertyName = "SecurityWorkingFolder";
            this.securityWorkingFolderDataGridViewTextBoxColumn.HeaderText = "SecurityWorkingFolder";
            this.securityWorkingFolderDataGridViewTextBoxColumn.Name = "securityWorkingFolderDataGridViewTextBoxColumn";
            // 
            // securityStartProgramDataGridViewTextBoxColumn
            // 
            this.securityStartProgramDataGridViewTextBoxColumn.DataPropertyName = "SecurityStartProgram";
            this.securityStartProgramDataGridViewTextBoxColumn.HeaderText = "SecurityStartProgram";
            this.securityStartProgramDataGridViewTextBoxColumn.Name = "securityStartProgramDataGridViewTextBoxColumn";
            // 
            // credentialDataGridViewTextBoxColumn
            // 
            this.credentialDataGridViewTextBoxColumn.DataPropertyName = "Credential";
            this.credentialDataGridViewTextBoxColumn.HeaderText = "Credential";
            this.credentialDataGridViewTextBoxColumn.Name = "credentialDataGridViewTextBoxColumn";
            // 
            // securityFullScreenDataGridViewCheckBoxColumn
            // 
            this.securityFullScreenDataGridViewCheckBoxColumn.DataPropertyName = "SecurityFullScreen";
            this.securityFullScreenDataGridViewCheckBoxColumn.HeaderText = "SecurityFullScreen";
            this.securityFullScreenDataGridViewCheckBoxColumn.Name = "securityFullScreenDataGridViewCheckBoxColumn";
            // 
            // enableSecuritySettingsDataGridViewCheckBoxColumn
            // 
            this.enableSecuritySettingsDataGridViewCheckBoxColumn.DataPropertyName = "EnableSecuritySettings";
            this.enableSecuritySettingsDataGridViewCheckBoxColumn.HeaderText = "EnableSecuritySettings";
            this.enableSecuritySettingsDataGridViewCheckBoxColumn.Name = "enableSecuritySettingsDataGridViewCheckBoxColumn";
            // 
            // grabFocusOnConnectDataGridViewCheckBoxColumn
            // 
            this.grabFocusOnConnectDataGridViewCheckBoxColumn.DataPropertyName = "GrabFocusOnConnect";
            this.grabFocusOnConnectDataGridViewCheckBoxColumn.HeaderText = "GrabFocusOnConnect";
            this.grabFocusOnConnectDataGridViewCheckBoxColumn.Name = "grabFocusOnConnectDataGridViewCheckBoxColumn";
            // 
            // enableEncryptionDataGridViewCheckBoxColumn
            // 
            this.enableEncryptionDataGridViewCheckBoxColumn.DataPropertyName = "EnableEncryption";
            this.enableEncryptionDataGridViewCheckBoxColumn.HeaderText = "EnableEncryption";
            this.enableEncryptionDataGridViewCheckBoxColumn.Name = "enableEncryptionDataGridViewCheckBoxColumn";
            // 
            // disableWindowsKeyDataGridViewCheckBoxColumn
            // 
            this.disableWindowsKeyDataGridViewCheckBoxColumn.DataPropertyName = "DisableWindowsKey";
            this.disableWindowsKeyDataGridViewCheckBoxColumn.HeaderText = "DisableWindowsKey";
            this.disableWindowsKeyDataGridViewCheckBoxColumn.Name = "disableWindowsKeyDataGridViewCheckBoxColumn";
            // 
            // doubleClickDetectDataGridViewCheckBoxColumn
            // 
            this.doubleClickDetectDataGridViewCheckBoxColumn.DataPropertyName = "DoubleClickDetect";
            this.doubleClickDetectDataGridViewCheckBoxColumn.HeaderText = "DoubleClickDetect";
            this.doubleClickDetectDataGridViewCheckBoxColumn.Name = "doubleClickDetectDataGridViewCheckBoxColumn";
            // 
            // displayConnectionBarDataGridViewCheckBoxColumn
            // 
            this.displayConnectionBarDataGridViewCheckBoxColumn.DataPropertyName = "DisplayConnectionBar";
            this.displayConnectionBarDataGridViewCheckBoxColumn.HeaderText = "DisplayConnectionBar";
            this.displayConnectionBarDataGridViewCheckBoxColumn.Name = "displayConnectionBarDataGridViewCheckBoxColumn";
            // 
            // disableControlAltDeleteDataGridViewCheckBoxColumn
            // 
            this.disableControlAltDeleteDataGridViewCheckBoxColumn.DataPropertyName = "DisableControlAltDelete";
            this.disableControlAltDeleteDataGridViewCheckBoxColumn.HeaderText = "DisableControlAltDelete";
            this.disableControlAltDeleteDataGridViewCheckBoxColumn.Name = "disableControlAltDeleteDataGridViewCheckBoxColumn";
            // 
            // acceleratorPassthroughDataGridViewCheckBoxColumn
            // 
            this.acceleratorPassthroughDataGridViewCheckBoxColumn.DataPropertyName = "AcceleratorPassthrough";
            this.acceleratorPassthroughDataGridViewCheckBoxColumn.HeaderText = "AcceleratorPassthrough";
            this.acceleratorPassthroughDataGridViewCheckBoxColumn.Name = "acceleratorPassthroughDataGridViewCheckBoxColumn";
            // 
            // enableCompressionDataGridViewCheckBoxColumn
            // 
            this.enableCompressionDataGridViewCheckBoxColumn.DataPropertyName = "EnableCompression";
            this.enableCompressionDataGridViewCheckBoxColumn.HeaderText = "EnableCompression";
            this.enableCompressionDataGridViewCheckBoxColumn.Name = "enableCompressionDataGridViewCheckBoxColumn";
            // 
            // bitmapPeristenceDataGridViewCheckBoxColumn
            // 
            this.bitmapPeristenceDataGridViewCheckBoxColumn.DataPropertyName = "BitmapPeristence";
            this.bitmapPeristenceDataGridViewCheckBoxColumn.HeaderText = "BitmapPeristence";
            this.bitmapPeristenceDataGridViewCheckBoxColumn.Name = "bitmapPeristenceDataGridViewCheckBoxColumn";
            // 
            // enableTLSAuthenticationDataGridViewCheckBoxColumn
            // 
            this.enableTLSAuthenticationDataGridViewCheckBoxColumn.DataPropertyName = "EnableTLSAuthentication";
            this.enableTLSAuthenticationDataGridViewCheckBoxColumn.HeaderText = "EnableTLSAuthentication";
            this.enableTLSAuthenticationDataGridViewCheckBoxColumn.Name = "enableTLSAuthenticationDataGridViewCheckBoxColumn";
            // 
            // enableNLAAuthenticationDataGridViewCheckBoxColumn
            // 
            this.enableNLAAuthenticationDataGridViewCheckBoxColumn.DataPropertyName = "EnableNLAAuthentication";
            this.enableNLAAuthenticationDataGridViewCheckBoxColumn.HeaderText = "EnableNLAAuthentication";
            this.enableNLAAuthenticationDataGridViewCheckBoxColumn.Name = "enableNLAAuthenticationDataGridViewCheckBoxColumn";
            // 
            // allowBackgroundInputDataGridViewCheckBoxColumn
            // 
            this.allowBackgroundInputDataGridViewCheckBoxColumn.DataPropertyName = "AllowBackgroundInput";
            this.allowBackgroundInputDataGridViewCheckBoxColumn.HeaderText = "AllowBackgroundInput";
            this.allowBackgroundInputDataGridViewCheckBoxColumn.Name = "allowBackgroundInputDataGridViewCheckBoxColumn";
            // 
            // iCAApplicationNameDataGridViewTextBoxColumn
            // 
            this.iCAApplicationNameDataGridViewTextBoxColumn.DataPropertyName = "ICAApplicationName";
            this.iCAApplicationNameDataGridViewTextBoxColumn.HeaderText = "ICAApplicationName";
            this.iCAApplicationNameDataGridViewTextBoxColumn.Name = "iCAApplicationNameDataGridViewTextBoxColumn";
            // 
            // iCAApplicationWorkingFolderDataGridViewTextBoxColumn
            // 
            this.iCAApplicationWorkingFolderDataGridViewTextBoxColumn.DataPropertyName = "ICAApplicationWorkingFolder";
            this.iCAApplicationWorkingFolderDataGridViewTextBoxColumn.HeaderText = "ICAApplicationWorkingFolder";
            this.iCAApplicationWorkingFolderDataGridViewTextBoxColumn.Name = "iCAApplicationWorkingFolderDataGridViewTextBoxColumn";
            // 
            // iCAApplicationPathDataGridViewTextBoxColumn
            // 
            this.iCAApplicationPathDataGridViewTextBoxColumn.DataPropertyName = "ICAApplicationPath";
            this.iCAApplicationPathDataGridViewTextBoxColumn.HeaderText = "ICAApplicationPath";
            this.iCAApplicationPathDataGridViewTextBoxColumn.Name = "iCAApplicationPathDataGridViewTextBoxColumn";
            // 
            // vMRCReducedColorsModeDataGridViewCheckBoxColumn
            // 
            this.vMRCReducedColorsModeDataGridViewCheckBoxColumn.DataPropertyName = "VMRCReducedColorsMode";
            this.vMRCReducedColorsModeDataGridViewCheckBoxColumn.HeaderText = "VMRCReducedColorsMode";
            this.vMRCReducedColorsModeDataGridViewCheckBoxColumn.Name = "vMRCReducedColorsModeDataGridViewCheckBoxColumn";
            // 
            // vMRCAdministratorModeDataGridViewCheckBoxColumn
            // 
            this.vMRCAdministratorModeDataGridViewCheckBoxColumn.DataPropertyName = "VMRCAdministratorMode";
            this.vMRCAdministratorModeDataGridViewCheckBoxColumn.HeaderText = "VMRCAdministratorMode";
            this.vMRCAdministratorModeDataGridViewCheckBoxColumn.Name = "vMRCAdministratorModeDataGridViewCheckBoxColumn";
            // 
            // sSH1DataGridViewCheckBoxColumn
            // 
            this.sSH1DataGridViewCheckBoxColumn.DataPropertyName = "SSH1";
            this.sSH1DataGridViewCheckBoxColumn.HeaderText = "SSH1";
            this.sSH1DataGridViewCheckBoxColumn.Name = "sSH1DataGridViewCheckBoxColumn";
            // 
            // consoleRowsDataGridViewTextBoxColumn
            // 
            this.consoleRowsDataGridViewTextBoxColumn.DataPropertyName = "ConsoleRows";
            this.consoleRowsDataGridViewTextBoxColumn.HeaderText = "ConsoleRows";
            this.consoleRowsDataGridViewTextBoxColumn.Name = "consoleRowsDataGridViewTextBoxColumn";
            // 
            // consoleColsDataGridViewTextBoxColumn
            // 
            this.consoleColsDataGridViewTextBoxColumn.DataPropertyName = "ConsoleCols";
            this.consoleColsDataGridViewTextBoxColumn.HeaderText = "ConsoleCols";
            this.consoleColsDataGridViewTextBoxColumn.Name = "consoleColsDataGridViewTextBoxColumn";
            // 
            // consoleFontDataGridViewTextBoxColumn
            // 
            this.consoleFontDataGridViewTextBoxColumn.DataPropertyName = "ConsoleFont";
            this.consoleFontDataGridViewTextBoxColumn.HeaderText = "ConsoleFont";
            this.consoleFontDataGridViewTextBoxColumn.Name = "consoleFontDataGridViewTextBoxColumn";
            // 
            // consoleBackColorDataGridViewTextBoxColumn
            // 
            this.consoleBackColorDataGridViewTextBoxColumn.DataPropertyName = "ConsoleBackColor";
            this.consoleBackColorDataGridViewTextBoxColumn.HeaderText = "ConsoleBackColor";
            this.consoleBackColorDataGridViewTextBoxColumn.Name = "consoleBackColorDataGridViewTextBoxColumn";
            // 
            // consoleTextColorDataGridViewTextBoxColumn
            // 
            this.consoleTextColorDataGridViewTextBoxColumn.DataPropertyName = "ConsoleTextColor";
            this.consoleTextColorDataGridViewTextBoxColumn.HeaderText = "ConsoleTextColor";
            this.consoleTextColorDataGridViewTextBoxColumn.Name = "consoleTextColorDataGridViewTextBoxColumn";
            // 
            // consoleCursorColorDataGridViewTextBoxColumn
            // 
            this.consoleCursorColorDataGridViewTextBoxColumn.DataPropertyName = "ConsoleCursorColor";
            this.consoleCursorColorDataGridViewTextBoxColumn.HeaderText = "ConsoleCursorColor";
            this.consoleCursorColorDataGridViewTextBoxColumn.Name = "consoleCursorColorDataGridViewTextBoxColumn";
            // 
            // protocolDataGridViewTextBoxColumn
            // 
            this.protocolDataGridViewTextBoxColumn.DataPropertyName = "Protocol";
            this.protocolDataGridViewTextBoxColumn.HeaderText = "Protocol";
            this.protocolDataGridViewTextBoxColumn.Name = "protocolDataGridViewTextBoxColumn";
            // 
            // toolBarIconDataGridViewTextBoxColumn
            // 
            this.toolBarIconDataGridViewTextBoxColumn.DataPropertyName = "ToolBarIcon";
            this.toolBarIconDataGridViewTextBoxColumn.HeaderText = "ToolBarIcon";
            this.toolBarIconDataGridViewTextBoxColumn.Name = "toolBarIconDataGridViewTextBoxColumn";
            // 
            // telnetFontDataGridViewTextBoxColumn
            // 
            this.telnetFontDataGridViewTextBoxColumn.DataPropertyName = "TelnetFont";
            this.telnetFontDataGridViewTextBoxColumn.HeaderText = "TelnetFont";
            this.telnetFontDataGridViewTextBoxColumn.Name = "telnetFontDataGridViewTextBoxColumn";
            // 
            // telnetBackColorDataGridViewTextBoxColumn
            // 
            this.telnetBackColorDataGridViewTextBoxColumn.DataPropertyName = "TelnetBackColor";
            this.telnetBackColorDataGridViewTextBoxColumn.HeaderText = "TelnetBackColor";
            this.telnetBackColorDataGridViewTextBoxColumn.Name = "telnetBackColorDataGridViewTextBoxColumn";
            // 
            // telnetTextColorDataGridViewTextBoxColumn
            // 
            this.telnetTextColorDataGridViewTextBoxColumn.DataPropertyName = "TelnetTextColor";
            this.telnetTextColorDataGridViewTextBoxColumn.HeaderText = "TelnetTextColor";
            this.telnetTextColorDataGridViewTextBoxColumn.Name = "telnetTextColorDataGridViewTextBoxColumn";
            // 
            // telnetCursorColorDataGridViewTextBoxColumn
            // 
            this.telnetCursorColorDataGridViewTextBoxColumn.DataPropertyName = "TelnetCursorColor";
            this.telnetCursorColorDataGridViewTextBoxColumn.HeaderText = "TelnetCursorColor";
            this.telnetCursorColorDataGridViewTextBoxColumn.Name = "telnetCursorColorDataGridViewTextBoxColumn";
            // 
            // nameDataGridViewTextBoxColumn
            // 
            this.nameDataGridViewTextBoxColumn.DataPropertyName = "Name";
            this.nameDataGridViewTextBoxColumn.HeaderText = "Name";
            this.nameDataGridViewTextBoxColumn.Name = "nameDataGridViewTextBoxColumn";
            // 
            // serverNameDataGridViewTextBoxColumn
            // 
            this.serverNameDataGridViewTextBoxColumn.DataPropertyName = "ServerName";
            this.serverNameDataGridViewTextBoxColumn.HeaderText = "ServerName";
            this.serverNameDataGridViewTextBoxColumn.Name = "serverNameDataGridViewTextBoxColumn";
            // 
            // domainNameDataGridViewTextBoxColumn
            // 
            this.domainNameDataGridViewTextBoxColumn.DataPropertyName = "DomainName";
            this.domainNameDataGridViewTextBoxColumn.HeaderText = "DomainName";
            this.domainNameDataGridViewTextBoxColumn.Name = "domainNameDataGridViewTextBoxColumn";
            // 
            // authMethodDataGridViewTextBoxColumn
            // 
            this.authMethodDataGridViewTextBoxColumn.DataPropertyName = "AuthMethod";
            this.authMethodDataGridViewTextBoxColumn.HeaderText = "AuthMethod";
            this.authMethodDataGridViewTextBoxColumn.Name = "authMethodDataGridViewTextBoxColumn";
            // 
            // keyTagDataGridViewTextBoxColumn
            // 
            this.keyTagDataGridViewTextBoxColumn.DataPropertyName = "KeyTag";
            this.keyTagDataGridViewTextBoxColumn.HeaderText = "KeyTag";
            this.keyTagDataGridViewTextBoxColumn.Name = "keyTagDataGridViewTextBoxColumn";
            // 
            // userNameDataGridViewTextBoxColumn
            // 
            this.userNameDataGridViewTextBoxColumn.DataPropertyName = "UserName";
            this.userNameDataGridViewTextBoxColumn.HeaderText = "UserName";
            this.userNameDataGridViewTextBoxColumn.Name = "userNameDataGridViewTextBoxColumn";
            // 
            // encryptedPasswordDataGridViewTextBoxColumn
            // 
            this.encryptedPasswordDataGridViewTextBoxColumn.DataPropertyName = "EncryptedPassword";
            this.encryptedPasswordDataGridViewTextBoxColumn.HeaderText = "EncryptedPassword";
            this.encryptedPasswordDataGridViewTextBoxColumn.Name = "encryptedPasswordDataGridViewTextBoxColumn";
            // 
            // passwordDataGridViewTextBoxColumn
            // 
            this.passwordDataGridViewTextBoxColumn.DataPropertyName = "Password";
            this.passwordDataGridViewTextBoxColumn.HeaderText = "Password";
            this.passwordDataGridViewTextBoxColumn.Name = "passwordDataGridViewTextBoxColumn";
            // 
            // vncAutoScaleDataGridViewCheckBoxColumn
            // 
            this.vncAutoScaleDataGridViewCheckBoxColumn.DataPropertyName = "VncAutoScale";
            this.vncAutoScaleDataGridViewCheckBoxColumn.HeaderText = "VncAutoScale";
            this.vncAutoScaleDataGridViewCheckBoxColumn.Name = "vncAutoScaleDataGridViewCheckBoxColumn";
            // 
            // vncViewOnlyDataGridViewCheckBoxColumn
            // 
            this.vncViewOnlyDataGridViewCheckBoxColumn.DataPropertyName = "VncViewOnly";
            this.vncViewOnlyDataGridViewCheckBoxColumn.HeaderText = "VncViewOnly";
            this.vncViewOnlyDataGridViewCheckBoxColumn.Name = "vncViewOnlyDataGridViewCheckBoxColumn";
            // 
            // vncDisplayNumberDataGridViewTextBoxColumn
            // 
            this.vncDisplayNumberDataGridViewTextBoxColumn.DataPropertyName = "VncDisplayNumber";
            this.vncDisplayNumberDataGridViewTextBoxColumn.HeaderText = "VncDisplayNumber";
            this.vncDisplayNumberDataGridViewTextBoxColumn.Name = "vncDisplayNumberDataGridViewTextBoxColumn";
            // 
            // connectToConsoleDataGridViewCheckBoxColumn
            // 
            this.connectToConsoleDataGridViewCheckBoxColumn.DataPropertyName = "ConnectToConsole";
            this.connectToConsoleDataGridViewCheckBoxColumn.HeaderText = "ConnectToConsole";
            this.connectToConsoleDataGridViewCheckBoxColumn.Name = "connectToConsoleDataGridViewCheckBoxColumn";
            // 
            // desktopSizeHeightDataGridViewTextBoxColumn
            // 
            this.desktopSizeHeightDataGridViewTextBoxColumn.DataPropertyName = "DesktopSizeHeight";
            this.desktopSizeHeightDataGridViewTextBoxColumn.HeaderText = "DesktopSizeHeight";
            this.desktopSizeHeightDataGridViewTextBoxColumn.Name = "desktopSizeHeightDataGridViewTextBoxColumn";
            // 
            // desktopSizeWidthDataGridViewTextBoxColumn
            // 
            this.desktopSizeWidthDataGridViewTextBoxColumn.DataPropertyName = "DesktopSizeWidth";
            this.desktopSizeWidthDataGridViewTextBoxColumn.HeaderText = "DesktopSizeWidth";
            this.desktopSizeWidthDataGridViewTextBoxColumn.Name = "desktopSizeWidthDataGridViewTextBoxColumn";
            // 
            // desktopSizeDataGridViewTextBoxColumn
            // 
            this.desktopSizeDataGridViewTextBoxColumn.DataPropertyName = "DesktopSize";
            this.desktopSizeDataGridViewTextBoxColumn.HeaderText = "DesktopSize";
            this.desktopSizeDataGridViewTextBoxColumn.Name = "desktopSizeDataGridViewTextBoxColumn";
            // 
            // colorsDataGridViewTextBoxColumn
            // 
            this.colorsDataGridViewTextBoxColumn.DataPropertyName = "Colors";
            this.colorsDataGridViewTextBoxColumn.HeaderText = "Colors";
            this.colorsDataGridViewTextBoxColumn.Name = "colorsDataGridViewTextBoxColumn";
            // 
            // soundsDataGridViewTextBoxColumn
            // 
            this.soundsDataGridViewTextBoxColumn.DataPropertyName = "Sounds";
            this.soundsDataGridViewTextBoxColumn.HeaderText = "Sounds";
            this.soundsDataGridViewTextBoxColumn.Name = "soundsDataGridViewTextBoxColumn";
            // 
            // redirectedDrivesDataGridViewTextBoxColumn
            // 
            this.redirectedDrivesDataGridViewTextBoxColumn.DataPropertyName = "redirectedDrives";
            this.redirectedDrivesDataGridViewTextBoxColumn.HeaderText = "redirectedDrives";
            this.redirectedDrivesDataGridViewTextBoxColumn.Name = "redirectedDrivesDataGridViewTextBoxColumn";
            // 
            // redirectPortsDataGridViewCheckBoxColumn
            // 
            this.redirectPortsDataGridViewCheckBoxColumn.DataPropertyName = "RedirectPorts";
            this.redirectPortsDataGridViewCheckBoxColumn.HeaderText = "RedirectPorts";
            this.redirectPortsDataGridViewCheckBoxColumn.Name = "redirectPortsDataGridViewCheckBoxColumn";
            // 
            // newWindowDataGridViewCheckBoxColumn
            // 
            this.newWindowDataGridViewCheckBoxColumn.DataPropertyName = "NewWindow";
            this.newWindowDataGridViewCheckBoxColumn.HeaderText = "NewWindow";
            this.newWindowDataGridViewCheckBoxColumn.Name = "newWindowDataGridViewCheckBoxColumn";
            // 
            // redirectPrintersDataGridViewCheckBoxColumn
            // 
            this.redirectPrintersDataGridViewCheckBoxColumn.DataPropertyName = "RedirectPrinters";
            this.redirectPrintersDataGridViewCheckBoxColumn.HeaderText = "RedirectPrinters";
            this.redirectPrintersDataGridViewCheckBoxColumn.Name = "redirectPrintersDataGridViewCheckBoxColumn";
            // 
            // redirectSmartCardsDataGridViewCheckBoxColumn
            // 
            this.redirectSmartCardsDataGridViewCheckBoxColumn.DataPropertyName = "RedirectSmartCards";
            this.redirectSmartCardsDataGridViewCheckBoxColumn.HeaderText = "RedirectSmartCards";
            this.redirectSmartCardsDataGridViewCheckBoxColumn.Name = "redirectSmartCardsDataGridViewCheckBoxColumn";
            // 
            // redirectClipboardDataGridViewCheckBoxColumn
            // 
            this.redirectClipboardDataGridViewCheckBoxColumn.DataPropertyName = "RedirectClipboard";
            this.redirectClipboardDataGridViewCheckBoxColumn.HeaderText = "RedirectClipboard";
            this.redirectClipboardDataGridViewCheckBoxColumn.Name = "redirectClipboardDataGridViewCheckBoxColumn";
            // 
            // redirectDevicesDataGridViewCheckBoxColumn
            // 
            this.redirectDevicesDataGridViewCheckBoxColumn.DataPropertyName = "RedirectDevices";
            this.redirectDevicesDataGridViewCheckBoxColumn.HeaderText = "RedirectDevices";
            this.redirectDevicesDataGridViewCheckBoxColumn.Name = "redirectDevicesDataGridViewCheckBoxColumn";
            // 
            // tsgwUsageMethodDataGridViewTextBoxColumn
            // 
            this.tsgwUsageMethodDataGridViewTextBoxColumn.DataPropertyName = "TsgwUsageMethod";
            this.tsgwUsageMethodDataGridViewTextBoxColumn.HeaderText = "TsgwUsageMethod";
            this.tsgwUsageMethodDataGridViewTextBoxColumn.Name = "tsgwUsageMethodDataGridViewTextBoxColumn";
            // 
            // tsgwHostnameDataGridViewTextBoxColumn
            // 
            this.tsgwHostnameDataGridViewTextBoxColumn.DataPropertyName = "TsgwHostname";
            this.tsgwHostnameDataGridViewTextBoxColumn.HeaderText = "TsgwHostname";
            this.tsgwHostnameDataGridViewTextBoxColumn.Name = "tsgwHostnameDataGridViewTextBoxColumn";
            // 
            // tsgwCredsSourceDataGridViewTextBoxColumn
            // 
            this.tsgwCredsSourceDataGridViewTextBoxColumn.DataPropertyName = "TsgwCredsSource";
            this.tsgwCredsSourceDataGridViewTextBoxColumn.HeaderText = "TsgwCredsSource";
            this.tsgwCredsSourceDataGridViewTextBoxColumn.Name = "tsgwCredsSourceDataGridViewTextBoxColumn";
            // 
            // tsgwSeparateLoginDataGridViewCheckBoxColumn
            // 
            this.tsgwSeparateLoginDataGridViewCheckBoxColumn.DataPropertyName = "TsgwSeparateLogin";
            this.tsgwSeparateLoginDataGridViewCheckBoxColumn.HeaderText = "TsgwSeparateLogin";
            this.tsgwSeparateLoginDataGridViewCheckBoxColumn.Name = "tsgwSeparateLoginDataGridViewCheckBoxColumn";
            // 
            // tsgwUsernameDataGridViewTextBoxColumn
            // 
            this.tsgwUsernameDataGridViewTextBoxColumn.DataPropertyName = "TsgwUsername";
            this.tsgwUsernameDataGridViewTextBoxColumn.HeaderText = "TsgwUsername";
            this.tsgwUsernameDataGridViewTextBoxColumn.Name = "tsgwUsernameDataGridViewTextBoxColumn";
            // 
            // tsgwDomainDataGridViewTextBoxColumn
            // 
            this.tsgwDomainDataGridViewTextBoxColumn.DataPropertyName = "TsgwDomain";
            this.tsgwDomainDataGridViewTextBoxColumn.HeaderText = "TsgwDomain";
            this.tsgwDomainDataGridViewTextBoxColumn.Name = "tsgwDomainDataGridViewTextBoxColumn";
            // 
            // tsgwEncryptedPasswordDataGridViewTextBoxColumn
            // 
            this.tsgwEncryptedPasswordDataGridViewTextBoxColumn.DataPropertyName = "TsgwEncryptedPassword";
            this.tsgwEncryptedPasswordDataGridViewTextBoxColumn.HeaderText = "TsgwEncryptedPassword";
            this.tsgwEncryptedPasswordDataGridViewTextBoxColumn.Name = "tsgwEncryptedPasswordDataGridViewTextBoxColumn";
            // 
            // tsgwPasswordDataGridViewTextBoxColumn
            // 
            this.tsgwPasswordDataGridViewTextBoxColumn.DataPropertyName = "TsgwPassword";
            this.tsgwPasswordDataGridViewTextBoxColumn.HeaderText = "TsgwPassword";
            this.tsgwPasswordDataGridViewTextBoxColumn.Name = "tsgwPasswordDataGridViewTextBoxColumn";
            // 
            // urlDataGridViewTextBoxColumn
            // 
            this.urlDataGridViewTextBoxColumn.DataPropertyName = "Url";
            this.urlDataGridViewTextBoxColumn.HeaderText = "Url";
            this.urlDataGridViewTextBoxColumn.Name = "urlDataGridViewTextBoxColumn";
            // 
            // notesDataGridViewTextBoxColumn
            // 
            this.notesDataGridViewTextBoxColumn.DataPropertyName = "Notes";
            this.notesDataGridViewTextBoxColumn.HeaderText = "Notes";
            this.notesDataGridViewTextBoxColumn.Name = "notesDataGridViewTextBoxColumn";
            // 
            // icaServerINIDataGridViewTextBoxColumn
            // 
            this.icaServerINIDataGridViewTextBoxColumn.DataPropertyName = "IcaServerINI";
            this.icaServerINIDataGridViewTextBoxColumn.HeaderText = "IcaServerINI";
            this.icaServerINIDataGridViewTextBoxColumn.Name = "icaServerINIDataGridViewTextBoxColumn";
            // 
            // icaClientINIDataGridViewTextBoxColumn
            // 
            this.icaClientINIDataGridViewTextBoxColumn.DataPropertyName = "IcaClientINI";
            this.icaClientINIDataGridViewTextBoxColumn.HeaderText = "IcaClientINI";
            this.icaClientINIDataGridViewTextBoxColumn.Name = "icaClientINIDataGridViewTextBoxColumn";
            // 
            // icaEnableEncryptionDataGridViewCheckBoxColumn
            // 
            this.icaEnableEncryptionDataGridViewCheckBoxColumn.DataPropertyName = "IcaEnableEncryption";
            this.icaEnableEncryptionDataGridViewCheckBoxColumn.HeaderText = "IcaEnableEncryption";
            this.icaEnableEncryptionDataGridViewCheckBoxColumn.Name = "icaEnableEncryptionDataGridViewCheckBoxColumn";
            // 
            // icaEncryptionLevelDataGridViewTextBoxColumn
            // 
            this.icaEncryptionLevelDataGridViewTextBoxColumn.DataPropertyName = "IcaEncryptionLevel";
            this.icaEncryptionLevelDataGridViewTextBoxColumn.HeaderText = "IcaEncryptionLevel";
            this.icaEncryptionLevelDataGridViewTextBoxColumn.Name = "icaEncryptionLevelDataGridViewTextBoxColumn";
            // 
            // portDataGridViewTextBoxColumn
            // 
            this.portDataGridViewTextBoxColumn.DataPropertyName = "Port";
            this.portDataGridViewTextBoxColumn.HeaderText = "Port";
            this.portDataGridViewTextBoxColumn.Name = "portDataGridViewTextBoxColumn";
            // 
            // desktopShareDataGridViewTextBoxColumn
            // 
            this.desktopShareDataGridViewTextBoxColumn.DataPropertyName = "DesktopShare";
            this.desktopShareDataGridViewTextBoxColumn.HeaderText = "DesktopShare";
            this.desktopShareDataGridViewTextBoxColumn.Name = "desktopShareDataGridViewTextBoxColumn";
            // 
            // executeBeforeConnectDataGridViewCheckBoxColumn
            // 
            this.executeBeforeConnectDataGridViewCheckBoxColumn.DataPropertyName = "ExecuteBeforeConnect";
            this.executeBeforeConnectDataGridViewCheckBoxColumn.HeaderText = "ExecuteBeforeConnect";
            this.executeBeforeConnectDataGridViewCheckBoxColumn.Name = "executeBeforeConnectDataGridViewCheckBoxColumn";
            // 
            // executeBeforeConnectCommandDataGridViewTextBoxColumn
            // 
            this.executeBeforeConnectCommandDataGridViewTextBoxColumn.DataPropertyName = "ExecuteBeforeConnectCommand";
            this.executeBeforeConnectCommandDataGridViewTextBoxColumn.HeaderText = "ExecuteBeforeConnectCommand";
            this.executeBeforeConnectCommandDataGridViewTextBoxColumn.Name = "executeBeforeConnectCommandDataGridViewTextBoxColumn";
            // 
            // executeBeforeConnectArgsDataGridViewTextBoxColumn
            // 
            this.executeBeforeConnectArgsDataGridViewTextBoxColumn.DataPropertyName = "ExecuteBeforeConnectArgs";
            this.executeBeforeConnectArgsDataGridViewTextBoxColumn.HeaderText = "ExecuteBeforeConnectArgs";
            this.executeBeforeConnectArgsDataGridViewTextBoxColumn.Name = "executeBeforeConnectArgsDataGridViewTextBoxColumn";
            // 
            // executeBeforeConnectInitialDirectoryDataGridViewTextBoxColumn
            // 
            this.executeBeforeConnectInitialDirectoryDataGridViewTextBoxColumn.DataPropertyName = "ExecuteBeforeConnectInitialDirectory";
            this.executeBeforeConnectInitialDirectoryDataGridViewTextBoxColumn.HeaderText = "ExecuteBeforeConnectInitialDirectory";
            this.executeBeforeConnectInitialDirectoryDataGridViewTextBoxColumn.Name = "executeBeforeConnectInitialDirectoryDataGridViewTextBoxColumn";
            // 
            // executeBeforeConnectWaitForExitDataGridViewCheckBoxColumn
            // 
            this.executeBeforeConnectWaitForExitDataGridViewCheckBoxColumn.DataPropertyName = "ExecuteBeforeConnectWaitForExit";
            this.executeBeforeConnectWaitForExitDataGridViewCheckBoxColumn.HeaderText = "ExecuteBeforeConnectWaitForExit";
            this.executeBeforeConnectWaitForExitDataGridViewCheckBoxColumn.Name = "executeBeforeConnectWaitForExitDataGridViewCheckBoxColumn";
            // 
            // disableThemingDataGridViewCheckBoxColumn
            // 
            this.disableThemingDataGridViewCheckBoxColumn.DataPropertyName = "DisableTheming";
            this.disableThemingDataGridViewCheckBoxColumn.HeaderText = "DisableTheming";
            this.disableThemingDataGridViewCheckBoxColumn.Name = "disableThemingDataGridViewCheckBoxColumn";
            // 
            // disableMenuAnimationsDataGridViewCheckBoxColumn
            // 
            this.disableMenuAnimationsDataGridViewCheckBoxColumn.DataPropertyName = "DisableMenuAnimations";
            this.disableMenuAnimationsDataGridViewCheckBoxColumn.HeaderText = "DisableMenuAnimations";
            this.disableMenuAnimationsDataGridViewCheckBoxColumn.Name = "disableMenuAnimationsDataGridViewCheckBoxColumn";
            // 
            // disableFullWindowDragDataGridViewCheckBoxColumn
            // 
            this.disableFullWindowDragDataGridViewCheckBoxColumn.DataPropertyName = "DisableFullWindowDrag";
            this.disableFullWindowDragDataGridViewCheckBoxColumn.HeaderText = "DisableFullWindowDrag";
            this.disableFullWindowDragDataGridViewCheckBoxColumn.Name = "disableFullWindowDragDataGridViewCheckBoxColumn";
            // 
            // disableCursorBlinkingDataGridViewCheckBoxColumn
            // 
            this.disableCursorBlinkingDataGridViewCheckBoxColumn.DataPropertyName = "DisableCursorBlinking";
            this.disableCursorBlinkingDataGridViewCheckBoxColumn.HeaderText = "DisableCursorBlinking";
            this.disableCursorBlinkingDataGridViewCheckBoxColumn.Name = "disableCursorBlinkingDataGridViewCheckBoxColumn";
            // 
            // enableDesktopCompositionDataGridViewCheckBoxColumn
            // 
            this.enableDesktopCompositionDataGridViewCheckBoxColumn.DataPropertyName = "EnableDesktopComposition";
            this.enableDesktopCompositionDataGridViewCheckBoxColumn.HeaderText = "EnableDesktopComposition";
            this.enableDesktopCompositionDataGridViewCheckBoxColumn.Name = "enableDesktopCompositionDataGridViewCheckBoxColumn";
            // 
            // enableFontSmoothingDataGridViewCheckBoxColumn
            // 
            this.enableFontSmoothingDataGridViewCheckBoxColumn.DataPropertyName = "EnableFontSmoothing";
            this.enableFontSmoothingDataGridViewCheckBoxColumn.HeaderText = "EnableFontSmoothing";
            this.enableFontSmoothingDataGridViewCheckBoxColumn.Name = "enableFontSmoothingDataGridViewCheckBoxColumn";
            // 
            // disableCursorShadowDataGridViewCheckBoxColumn
            // 
            this.disableCursorShadowDataGridViewCheckBoxColumn.DataPropertyName = "DisableCursorShadow";
            this.disableCursorShadowDataGridViewCheckBoxColumn.HeaderText = "DisableCursorShadow";
            this.disableCursorShadowDataGridViewCheckBoxColumn.Name = "disableCursorShadowDataGridViewCheckBoxColumn";
            // 
            // disableWallPaperDataGridViewCheckBoxColumn
            // 
            this.disableWallPaperDataGridViewCheckBoxColumn.DataPropertyName = "DisableWallPaper";
            this.disableWallPaperDataGridViewCheckBoxColumn.HeaderText = "DisableWallPaper";
            this.disableWallPaperDataGridViewCheckBoxColumn.Name = "disableWallPaperDataGridViewCheckBoxColumn";
            // 
            // tagsDataGridViewTextBoxColumn
            // 
            this.tagsDataGridViewTextBoxColumn.DataPropertyName = "Tags";
            this.tagsDataGridViewTextBoxColumn.HeaderText = "Tags";
            this.tagsDataGridViewTextBoxColumn.Name = "tagsDataGridViewTextBoxColumn";
            // 
            // lockAttributesDataGridViewTextBoxColumn
            // 
            this.lockAttributesDataGridViewTextBoxColumn.DataPropertyName = "LockAttributes";
            this.lockAttributesDataGridViewTextBoxColumn.HeaderText = "LockAttributes";
            this.lockAttributesDataGridViewTextBoxColumn.Name = "lockAttributesDataGridViewTextBoxColumn";
            this.lockAttributesDataGridViewTextBoxColumn.ReadOnly = true;
            // 
            // lockAllAttributesExceptDataGridViewTextBoxColumn
            // 
            this.lockAllAttributesExceptDataGridViewTextBoxColumn.DataPropertyName = "LockAllAttributesExcept";
            this.lockAllAttributesExceptDataGridViewTextBoxColumn.HeaderText = "LockAllAttributesExcept";
            this.lockAllAttributesExceptDataGridViewTextBoxColumn.Name = "lockAllAttributesExceptDataGridViewTextBoxColumn";
            this.lockAllAttributesExceptDataGridViewTextBoxColumn.ReadOnly = true;
            // 
            // lockElementsDataGridViewTextBoxColumn
            // 
            this.lockElementsDataGridViewTextBoxColumn.DataPropertyName = "LockElements";
            this.lockElementsDataGridViewTextBoxColumn.HeaderText = "LockElements";
            this.lockElementsDataGridViewTextBoxColumn.Name = "lockElementsDataGridViewTextBoxColumn";
            this.lockElementsDataGridViewTextBoxColumn.ReadOnly = true;
            // 
            // lockAllElementsExceptDataGridViewTextBoxColumn
            // 
            this.lockAllElementsExceptDataGridViewTextBoxColumn.DataPropertyName = "LockAllElementsExcept";
            this.lockAllElementsExceptDataGridViewTextBoxColumn.HeaderText = "LockAllElementsExcept";
            this.lockAllElementsExceptDataGridViewTextBoxColumn.Name = "lockAllElementsExceptDataGridViewTextBoxColumn";
            this.lockAllElementsExceptDataGridViewTextBoxColumn.ReadOnly = true;
            // 
            // lockItemDataGridViewCheckBoxColumn
            // 
            this.lockItemDataGridViewCheckBoxColumn.DataPropertyName = "LockItem";
            this.lockItemDataGridViewCheckBoxColumn.HeaderText = "LockItem";
            this.lockItemDataGridViewCheckBoxColumn.Name = "lockItemDataGridViewCheckBoxColumn";
            // 
            // elementInformationDataGridViewTextBoxColumn
            // 
            this.elementInformationDataGridViewTextBoxColumn.DataPropertyName = "ElementInformation";
            this.elementInformationDataGridViewTextBoxColumn.HeaderText = "ElementInformation";
            this.elementInformationDataGridViewTextBoxColumn.Name = "elementInformationDataGridViewTextBoxColumn";
            this.elementInformationDataGridViewTextBoxColumn.ReadOnly = true;
            // 
            // currentConfigurationDataGridViewTextBoxColumn
            // 
            this.currentConfigurationDataGridViewTextBoxColumn.DataPropertyName = "CurrentConfiguration";
            this.currentConfigurationDataGridViewTextBoxColumn.HeaderText = "CurrentConfiguration";
            this.currentConfigurationDataGridViewTextBoxColumn.Name = "currentConfigurationDataGridViewTextBoxColumn";
            this.currentConfigurationDataGridViewTextBoxColumn.ReadOnly = true;
            // 
            // performanceFlagsDataGridViewTextBoxColumn1
            // 
            this.performanceFlagsDataGridViewTextBoxColumn1.DataPropertyName = "PerformanceFlags";
            this.performanceFlagsDataGridViewTextBoxColumn1.HeaderText = "PerformanceFlags";
            this.performanceFlagsDataGridViewTextBoxColumn1.Name = "performanceFlagsDataGridViewTextBoxColumn1";
            this.performanceFlagsDataGridViewTextBoxColumn1.ReadOnly = true;
            // 
            // telnetDataGridViewCheckBoxColumn1
            // 
            this.telnetDataGridViewCheckBoxColumn1.DataPropertyName = "Telnet";
            this.telnetDataGridViewCheckBoxColumn1.HeaderText = "Telnet";
            this.telnetDataGridViewCheckBoxColumn1.Name = "telnetDataGridViewCheckBoxColumn1";
            // 
            // telnetRowsDataGridViewTextBoxColumn1
            // 
            this.telnetRowsDataGridViewTextBoxColumn1.DataPropertyName = "TelnetRows";
            this.telnetRowsDataGridViewTextBoxColumn1.HeaderText = "TelnetRows";
            this.telnetRowsDataGridViewTextBoxColumn1.Name = "telnetRowsDataGridViewTextBoxColumn1";
            // 
            // telnetColsDataGridViewTextBoxColumn1
            // 
            this.telnetColsDataGridViewTextBoxColumn1.DataPropertyName = "TelnetCols";
            this.telnetColsDataGridViewTextBoxColumn1.HeaderText = "TelnetCols";
            this.telnetColsDataGridViewTextBoxColumn1.Name = "telnetColsDataGridViewTextBoxColumn1";
            // 
            // shutdownTimeoutDataGridViewTextBoxColumn1
            // 
            this.shutdownTimeoutDataGridViewTextBoxColumn1.DataPropertyName = "ShutdownTimeout";
            this.shutdownTimeoutDataGridViewTextBoxColumn1.HeaderText = "ShutdownTimeout";
            this.shutdownTimeoutDataGridViewTextBoxColumn1.Name = "shutdownTimeoutDataGridViewTextBoxColumn1";
            // 
            // overallTimeoutDataGridViewTextBoxColumn1
            // 
            this.overallTimeoutDataGridViewTextBoxColumn1.DataPropertyName = "OverallTimeout";
            this.overallTimeoutDataGridViewTextBoxColumn1.HeaderText = "OverallTimeout";
            this.overallTimeoutDataGridViewTextBoxColumn1.Name = "overallTimeoutDataGridViewTextBoxColumn1";
            // 
            // connectionTimeoutDataGridViewTextBoxColumn1
            // 
            this.connectionTimeoutDataGridViewTextBoxColumn1.DataPropertyName = "ConnectionTimeout";
            this.connectionTimeoutDataGridViewTextBoxColumn1.HeaderText = "ConnectionTimeout";
            this.connectionTimeoutDataGridViewTextBoxColumn1.Name = "connectionTimeoutDataGridViewTextBoxColumn1";
            // 
            // idleTimeoutDataGridViewTextBoxColumn1
            // 
            this.idleTimeoutDataGridViewTextBoxColumn1.DataPropertyName = "IdleTimeout";
            this.idleTimeoutDataGridViewTextBoxColumn1.HeaderText = "IdleTimeout";
            this.idleTimeoutDataGridViewTextBoxColumn1.Name = "idleTimeoutDataGridViewTextBoxColumn1";
            // 
            // securityWorkingFolderDataGridViewTextBoxColumn1
            // 
            this.securityWorkingFolderDataGridViewTextBoxColumn1.DataPropertyName = "SecurityWorkingFolder";
            this.securityWorkingFolderDataGridViewTextBoxColumn1.HeaderText = "SecurityWorkingFolder";
            this.securityWorkingFolderDataGridViewTextBoxColumn1.Name = "securityWorkingFolderDataGridViewTextBoxColumn1";
            // 
            // securityStartProgramDataGridViewTextBoxColumn1
            // 
            this.securityStartProgramDataGridViewTextBoxColumn1.DataPropertyName = "SecurityStartProgram";
            this.securityStartProgramDataGridViewTextBoxColumn1.HeaderText = "SecurityStartProgram";
            this.securityStartProgramDataGridViewTextBoxColumn1.Name = "securityStartProgramDataGridViewTextBoxColumn1";
            // 
            // credentialDataGridViewTextBoxColumn1
            // 
            this.credentialDataGridViewTextBoxColumn1.DataPropertyName = "Credential";
            this.credentialDataGridViewTextBoxColumn1.HeaderText = "Credential";
            this.credentialDataGridViewTextBoxColumn1.Name = "credentialDataGridViewTextBoxColumn1";
            // 
            // securityFullScreenDataGridViewCheckBoxColumn1
            // 
            this.securityFullScreenDataGridViewCheckBoxColumn1.DataPropertyName = "SecurityFullScreen";
            this.securityFullScreenDataGridViewCheckBoxColumn1.HeaderText = "SecurityFullScreen";
            this.securityFullScreenDataGridViewCheckBoxColumn1.Name = "securityFullScreenDataGridViewCheckBoxColumn1";
            // 
            // enableSecuritySettingsDataGridViewCheckBoxColumn1
            // 
            this.enableSecuritySettingsDataGridViewCheckBoxColumn1.DataPropertyName = "EnableSecuritySettings";
            this.enableSecuritySettingsDataGridViewCheckBoxColumn1.HeaderText = "EnableSecuritySettings";
            this.enableSecuritySettingsDataGridViewCheckBoxColumn1.Name = "enableSecuritySettingsDataGridViewCheckBoxColumn1";
            // 
            // grabFocusOnConnectDataGridViewCheckBoxColumn1
            // 
            this.grabFocusOnConnectDataGridViewCheckBoxColumn1.DataPropertyName = "GrabFocusOnConnect";
            this.grabFocusOnConnectDataGridViewCheckBoxColumn1.HeaderText = "GrabFocusOnConnect";
            this.grabFocusOnConnectDataGridViewCheckBoxColumn1.Name = "grabFocusOnConnectDataGridViewCheckBoxColumn1";
            // 
            // enableEncryptionDataGridViewCheckBoxColumn1
            // 
            this.enableEncryptionDataGridViewCheckBoxColumn1.DataPropertyName = "EnableEncryption";
            this.enableEncryptionDataGridViewCheckBoxColumn1.HeaderText = "EnableEncryption";
            this.enableEncryptionDataGridViewCheckBoxColumn1.Name = "enableEncryptionDataGridViewCheckBoxColumn1";
            // 
            // disableWindowsKeyDataGridViewCheckBoxColumn1
            // 
            this.disableWindowsKeyDataGridViewCheckBoxColumn1.DataPropertyName = "DisableWindowsKey";
            this.disableWindowsKeyDataGridViewCheckBoxColumn1.HeaderText = "DisableWindowsKey";
            this.disableWindowsKeyDataGridViewCheckBoxColumn1.Name = "disableWindowsKeyDataGridViewCheckBoxColumn1";
            // 
            // doubleClickDetectDataGridViewCheckBoxColumn1
            // 
            this.doubleClickDetectDataGridViewCheckBoxColumn1.DataPropertyName = "DoubleClickDetect";
            this.doubleClickDetectDataGridViewCheckBoxColumn1.HeaderText = "DoubleClickDetect";
            this.doubleClickDetectDataGridViewCheckBoxColumn1.Name = "doubleClickDetectDataGridViewCheckBoxColumn1";
            // 
            // displayConnectionBarDataGridViewCheckBoxColumn1
            // 
            this.displayConnectionBarDataGridViewCheckBoxColumn1.DataPropertyName = "DisplayConnectionBar";
            this.displayConnectionBarDataGridViewCheckBoxColumn1.HeaderText = "DisplayConnectionBar";
            this.displayConnectionBarDataGridViewCheckBoxColumn1.Name = "displayConnectionBarDataGridViewCheckBoxColumn1";
            // 
            // disableControlAltDeleteDataGridViewCheckBoxColumn1
            // 
            this.disableControlAltDeleteDataGridViewCheckBoxColumn1.DataPropertyName = "DisableControlAltDelete";
            this.disableControlAltDeleteDataGridViewCheckBoxColumn1.HeaderText = "DisableControlAltDelete";
            this.disableControlAltDeleteDataGridViewCheckBoxColumn1.Name = "disableControlAltDeleteDataGridViewCheckBoxColumn1";
            // 
            // acceleratorPassthroughDataGridViewCheckBoxColumn1
            // 
            this.acceleratorPassthroughDataGridViewCheckBoxColumn1.DataPropertyName = "AcceleratorPassthrough";
            this.acceleratorPassthroughDataGridViewCheckBoxColumn1.HeaderText = "AcceleratorPassthrough";
            this.acceleratorPassthroughDataGridViewCheckBoxColumn1.Name = "acceleratorPassthroughDataGridViewCheckBoxColumn1";
            // 
            // enableCompressionDataGridViewCheckBoxColumn1
            // 
            this.enableCompressionDataGridViewCheckBoxColumn1.DataPropertyName = "EnableCompression";
            this.enableCompressionDataGridViewCheckBoxColumn1.HeaderText = "EnableCompression";
            this.enableCompressionDataGridViewCheckBoxColumn1.Name = "enableCompressionDataGridViewCheckBoxColumn1";
            // 
            // bitmapPeristenceDataGridViewCheckBoxColumn1
            // 
            this.bitmapPeristenceDataGridViewCheckBoxColumn1.DataPropertyName = "BitmapPeristence";
            this.bitmapPeristenceDataGridViewCheckBoxColumn1.HeaderText = "BitmapPeristence";
            this.bitmapPeristenceDataGridViewCheckBoxColumn1.Name = "bitmapPeristenceDataGridViewCheckBoxColumn1";
            // 
            // enableTLSAuthenticationDataGridViewCheckBoxColumn1
            // 
            this.enableTLSAuthenticationDataGridViewCheckBoxColumn1.DataPropertyName = "EnableTLSAuthentication";
            this.enableTLSAuthenticationDataGridViewCheckBoxColumn1.HeaderText = "EnableTLSAuthentication";
            this.enableTLSAuthenticationDataGridViewCheckBoxColumn1.Name = "enableTLSAuthenticationDataGridViewCheckBoxColumn1";
            // 
            // enableNLAAuthenticationDataGridViewCheckBoxColumn1
            // 
            this.enableNLAAuthenticationDataGridViewCheckBoxColumn1.DataPropertyName = "EnableNLAAuthentication";
            this.enableNLAAuthenticationDataGridViewCheckBoxColumn1.HeaderText = "EnableNLAAuthentication";
            this.enableNLAAuthenticationDataGridViewCheckBoxColumn1.Name = "enableNLAAuthenticationDataGridViewCheckBoxColumn1";
            // 
            // allowBackgroundInputDataGridViewCheckBoxColumn1
            // 
            this.allowBackgroundInputDataGridViewCheckBoxColumn1.DataPropertyName = "AllowBackgroundInput";
            this.allowBackgroundInputDataGridViewCheckBoxColumn1.HeaderText = "AllowBackgroundInput";
            this.allowBackgroundInputDataGridViewCheckBoxColumn1.Name = "allowBackgroundInputDataGridViewCheckBoxColumn1";
            // 
            // iCAApplicationNameDataGridViewTextBoxColumn1
            // 
            this.iCAApplicationNameDataGridViewTextBoxColumn1.DataPropertyName = "ICAApplicationName";
            this.iCAApplicationNameDataGridViewTextBoxColumn1.HeaderText = "ICAApplicationName";
            this.iCAApplicationNameDataGridViewTextBoxColumn1.Name = "iCAApplicationNameDataGridViewTextBoxColumn1";
            // 
            // iCAApplicationWorkingFolderDataGridViewTextBoxColumn1
            // 
            this.iCAApplicationWorkingFolderDataGridViewTextBoxColumn1.DataPropertyName = "ICAApplicationWorkingFolder";
            this.iCAApplicationWorkingFolderDataGridViewTextBoxColumn1.HeaderText = "ICAApplicationWorkingFolder";
            this.iCAApplicationWorkingFolderDataGridViewTextBoxColumn1.Name = "iCAApplicationWorkingFolderDataGridViewTextBoxColumn1";
            // 
            // iCAApplicationPathDataGridViewTextBoxColumn1
            // 
            this.iCAApplicationPathDataGridViewTextBoxColumn1.DataPropertyName = "ICAApplicationPath";
            this.iCAApplicationPathDataGridViewTextBoxColumn1.HeaderText = "ICAApplicationPath";
            this.iCAApplicationPathDataGridViewTextBoxColumn1.Name = "iCAApplicationPathDataGridViewTextBoxColumn1";
            // 
            // vMRCReducedColorsModeDataGridViewCheckBoxColumn1
            // 
            this.vMRCReducedColorsModeDataGridViewCheckBoxColumn1.DataPropertyName = "VMRCReducedColorsMode";
            this.vMRCReducedColorsModeDataGridViewCheckBoxColumn1.HeaderText = "VMRCReducedColorsMode";
            this.vMRCReducedColorsModeDataGridViewCheckBoxColumn1.Name = "vMRCReducedColorsModeDataGridViewCheckBoxColumn1";
            // 
            // vMRCAdministratorModeDataGridViewCheckBoxColumn1
            // 
            this.vMRCAdministratorModeDataGridViewCheckBoxColumn1.DataPropertyName = "VMRCAdministratorMode";
            this.vMRCAdministratorModeDataGridViewCheckBoxColumn1.HeaderText = "VMRCAdministratorMode";
            this.vMRCAdministratorModeDataGridViewCheckBoxColumn1.Name = "vMRCAdministratorModeDataGridViewCheckBoxColumn1";
            // 
            // sSH1DataGridViewCheckBoxColumn1
            // 
            this.sSH1DataGridViewCheckBoxColumn1.DataPropertyName = "SSH1";
            this.sSH1DataGridViewCheckBoxColumn1.HeaderText = "SSH1";
            this.sSH1DataGridViewCheckBoxColumn1.Name = "sSH1DataGridViewCheckBoxColumn1";
            // 
            // consoleRowsDataGridViewTextBoxColumn1
            // 
            this.consoleRowsDataGridViewTextBoxColumn1.DataPropertyName = "ConsoleRows";
            this.consoleRowsDataGridViewTextBoxColumn1.HeaderText = "ConsoleRows";
            this.consoleRowsDataGridViewTextBoxColumn1.Name = "consoleRowsDataGridViewTextBoxColumn1";
            // 
            // consoleColsDataGridViewTextBoxColumn1
            // 
            this.consoleColsDataGridViewTextBoxColumn1.DataPropertyName = "ConsoleCols";
            this.consoleColsDataGridViewTextBoxColumn1.HeaderText = "ConsoleCols";
            this.consoleColsDataGridViewTextBoxColumn1.Name = "consoleColsDataGridViewTextBoxColumn1";
            // 
            // consoleFontDataGridViewTextBoxColumn1
            // 
            this.consoleFontDataGridViewTextBoxColumn1.DataPropertyName = "ConsoleFont";
            this.consoleFontDataGridViewTextBoxColumn1.HeaderText = "ConsoleFont";
            this.consoleFontDataGridViewTextBoxColumn1.Name = "consoleFontDataGridViewTextBoxColumn1";
            // 
            // consoleBackColorDataGridViewTextBoxColumn1
            // 
            this.consoleBackColorDataGridViewTextBoxColumn1.DataPropertyName = "ConsoleBackColor";
            this.consoleBackColorDataGridViewTextBoxColumn1.HeaderText = "ConsoleBackColor";
            this.consoleBackColorDataGridViewTextBoxColumn1.Name = "consoleBackColorDataGridViewTextBoxColumn1";
            // 
            // consoleTextColorDataGridViewTextBoxColumn1
            // 
            this.consoleTextColorDataGridViewTextBoxColumn1.DataPropertyName = "ConsoleTextColor";
            this.consoleTextColorDataGridViewTextBoxColumn1.HeaderText = "ConsoleTextColor";
            this.consoleTextColorDataGridViewTextBoxColumn1.Name = "consoleTextColorDataGridViewTextBoxColumn1";
            // 
            // consoleCursorColorDataGridViewTextBoxColumn1
            // 
            this.consoleCursorColorDataGridViewTextBoxColumn1.DataPropertyName = "ConsoleCursorColor";
            this.consoleCursorColorDataGridViewTextBoxColumn1.HeaderText = "ConsoleCursorColor";
            this.consoleCursorColorDataGridViewTextBoxColumn1.Name = "consoleCursorColorDataGridViewTextBoxColumn1";
            // 
            // protocolDataGridViewTextBoxColumn1
            // 
            this.protocolDataGridViewTextBoxColumn1.DataPropertyName = "Protocol";
            this.protocolDataGridViewTextBoxColumn1.HeaderText = "Protocol";
            this.protocolDataGridViewTextBoxColumn1.Name = "protocolDataGridViewTextBoxColumn1";
            // 
            // toolBarIconDataGridViewTextBoxColumn1
            // 
            this.toolBarIconDataGridViewTextBoxColumn1.DataPropertyName = "ToolBarIcon";
            this.toolBarIconDataGridViewTextBoxColumn1.HeaderText = "ToolBarIcon";
            this.toolBarIconDataGridViewTextBoxColumn1.Name = "toolBarIconDataGridViewTextBoxColumn1";
            // 
            // telnetFontDataGridViewTextBoxColumn1
            // 
            this.telnetFontDataGridViewTextBoxColumn1.DataPropertyName = "TelnetFont";
            this.telnetFontDataGridViewTextBoxColumn1.HeaderText = "TelnetFont";
            this.telnetFontDataGridViewTextBoxColumn1.Name = "telnetFontDataGridViewTextBoxColumn1";
            // 
            // telnetBackColorDataGridViewTextBoxColumn1
            // 
            this.telnetBackColorDataGridViewTextBoxColumn1.DataPropertyName = "TelnetBackColor";
            this.telnetBackColorDataGridViewTextBoxColumn1.HeaderText = "TelnetBackColor";
            this.telnetBackColorDataGridViewTextBoxColumn1.Name = "telnetBackColorDataGridViewTextBoxColumn1";
            // 
            // telnetTextColorDataGridViewTextBoxColumn1
            // 
            this.telnetTextColorDataGridViewTextBoxColumn1.DataPropertyName = "TelnetTextColor";
            this.telnetTextColorDataGridViewTextBoxColumn1.HeaderText = "TelnetTextColor";
            this.telnetTextColorDataGridViewTextBoxColumn1.Name = "telnetTextColorDataGridViewTextBoxColumn1";
            // 
            // telnetCursorColorDataGridViewTextBoxColumn1
            // 
            this.telnetCursorColorDataGridViewTextBoxColumn1.DataPropertyName = "TelnetCursorColor";
            this.telnetCursorColorDataGridViewTextBoxColumn1.HeaderText = "TelnetCursorColor";
            this.telnetCursorColorDataGridViewTextBoxColumn1.Name = "telnetCursorColorDataGridViewTextBoxColumn1";
            // 
            // nameDataGridViewTextBoxColumn1
            // 
            this.nameDataGridViewTextBoxColumn1.DataPropertyName = "Name";
            this.nameDataGridViewTextBoxColumn1.HeaderText = "Name";
            this.nameDataGridViewTextBoxColumn1.Name = "nameDataGridViewTextBoxColumn1";
            // 
            // serverNameDataGridViewTextBoxColumn1
            // 
            this.serverNameDataGridViewTextBoxColumn1.DataPropertyName = "ServerName";
            this.serverNameDataGridViewTextBoxColumn1.HeaderText = "ServerName";
            this.serverNameDataGridViewTextBoxColumn1.Name = "serverNameDataGridViewTextBoxColumn1";
            // 
            // domainNameDataGridViewTextBoxColumn1
            // 
            this.domainNameDataGridViewTextBoxColumn1.DataPropertyName = "DomainName";
            this.domainNameDataGridViewTextBoxColumn1.HeaderText = "DomainName";
            this.domainNameDataGridViewTextBoxColumn1.Name = "domainNameDataGridViewTextBoxColumn1";
            // 
            // authMethodDataGridViewTextBoxColumn1
            // 
            this.authMethodDataGridViewTextBoxColumn1.DataPropertyName = "AuthMethod";
            this.authMethodDataGridViewTextBoxColumn1.HeaderText = "AuthMethod";
            this.authMethodDataGridViewTextBoxColumn1.Name = "authMethodDataGridViewTextBoxColumn1";
            // 
            // keyTagDataGridViewTextBoxColumn1
            // 
            this.keyTagDataGridViewTextBoxColumn1.DataPropertyName = "KeyTag";
            this.keyTagDataGridViewTextBoxColumn1.HeaderText = "KeyTag";
            this.keyTagDataGridViewTextBoxColumn1.Name = "keyTagDataGridViewTextBoxColumn1";
            // 
            // userNameDataGridViewTextBoxColumn1
            // 
            this.userNameDataGridViewTextBoxColumn1.DataPropertyName = "UserName";
            this.userNameDataGridViewTextBoxColumn1.HeaderText = "UserName";
            this.userNameDataGridViewTextBoxColumn1.Name = "userNameDataGridViewTextBoxColumn1";
            // 
            // encryptedPasswordDataGridViewTextBoxColumn1
            // 
            this.encryptedPasswordDataGridViewTextBoxColumn1.DataPropertyName = "EncryptedPassword";
            this.encryptedPasswordDataGridViewTextBoxColumn1.HeaderText = "EncryptedPassword";
            this.encryptedPasswordDataGridViewTextBoxColumn1.Name = "encryptedPasswordDataGridViewTextBoxColumn1";
            // 
            // passwordDataGridViewTextBoxColumn1
            // 
            this.passwordDataGridViewTextBoxColumn1.DataPropertyName = "Password";
            this.passwordDataGridViewTextBoxColumn1.HeaderText = "Password";
            this.passwordDataGridViewTextBoxColumn1.Name = "passwordDataGridViewTextBoxColumn1";
            // 
            // vncAutoScaleDataGridViewCheckBoxColumn1
            // 
            this.vncAutoScaleDataGridViewCheckBoxColumn1.DataPropertyName = "VncAutoScale";
            this.vncAutoScaleDataGridViewCheckBoxColumn1.HeaderText = "VncAutoScale";
            this.vncAutoScaleDataGridViewCheckBoxColumn1.Name = "vncAutoScaleDataGridViewCheckBoxColumn1";
            // 
            // vncViewOnlyDataGridViewCheckBoxColumn1
            // 
            this.vncViewOnlyDataGridViewCheckBoxColumn1.DataPropertyName = "VncViewOnly";
            this.vncViewOnlyDataGridViewCheckBoxColumn1.HeaderText = "VncViewOnly";
            this.vncViewOnlyDataGridViewCheckBoxColumn1.Name = "vncViewOnlyDataGridViewCheckBoxColumn1";
            // 
            // vncDisplayNumberDataGridViewTextBoxColumn1
            // 
            this.vncDisplayNumberDataGridViewTextBoxColumn1.DataPropertyName = "VncDisplayNumber";
            this.vncDisplayNumberDataGridViewTextBoxColumn1.HeaderText = "VncDisplayNumber";
            this.vncDisplayNumberDataGridViewTextBoxColumn1.Name = "vncDisplayNumberDataGridViewTextBoxColumn1";
            // 
            // connectToConsoleDataGridViewCheckBoxColumn1
            // 
            this.connectToConsoleDataGridViewCheckBoxColumn1.DataPropertyName = "ConnectToConsole";
            this.connectToConsoleDataGridViewCheckBoxColumn1.HeaderText = "ConnectToConsole";
            this.connectToConsoleDataGridViewCheckBoxColumn1.Name = "connectToConsoleDataGridViewCheckBoxColumn1";
            // 
            // desktopSizeHeightDataGridViewTextBoxColumn1
            // 
            this.desktopSizeHeightDataGridViewTextBoxColumn1.DataPropertyName = "DesktopSizeHeight";
            this.desktopSizeHeightDataGridViewTextBoxColumn1.HeaderText = "DesktopSizeHeight";
            this.desktopSizeHeightDataGridViewTextBoxColumn1.Name = "desktopSizeHeightDataGridViewTextBoxColumn1";
            // 
            // desktopSizeWidthDataGridViewTextBoxColumn1
            // 
            this.desktopSizeWidthDataGridViewTextBoxColumn1.DataPropertyName = "DesktopSizeWidth";
            this.desktopSizeWidthDataGridViewTextBoxColumn1.HeaderText = "DesktopSizeWidth";
            this.desktopSizeWidthDataGridViewTextBoxColumn1.Name = "desktopSizeWidthDataGridViewTextBoxColumn1";
            // 
            // desktopSizeDataGridViewTextBoxColumn1
            // 
            this.desktopSizeDataGridViewTextBoxColumn1.DataPropertyName = "DesktopSize";
            this.desktopSizeDataGridViewTextBoxColumn1.HeaderText = "DesktopSize";
            this.desktopSizeDataGridViewTextBoxColumn1.Name = "desktopSizeDataGridViewTextBoxColumn1";
            // 
            // colorsDataGridViewTextBoxColumn1
            // 
            this.colorsDataGridViewTextBoxColumn1.DataPropertyName = "Colors";
            this.colorsDataGridViewTextBoxColumn1.HeaderText = "Colors";
            this.colorsDataGridViewTextBoxColumn1.Name = "colorsDataGridViewTextBoxColumn1";
            // 
            // soundsDataGridViewTextBoxColumn1
            // 
            this.soundsDataGridViewTextBoxColumn1.DataPropertyName = "Sounds";
            this.soundsDataGridViewTextBoxColumn1.HeaderText = "Sounds";
            this.soundsDataGridViewTextBoxColumn1.Name = "soundsDataGridViewTextBoxColumn1";
            // 
            // redirectedDrivesDataGridViewTextBoxColumn1
            // 
            this.redirectedDrivesDataGridViewTextBoxColumn1.DataPropertyName = "redirectedDrives";
            this.redirectedDrivesDataGridViewTextBoxColumn1.HeaderText = "redirectedDrives";
            this.redirectedDrivesDataGridViewTextBoxColumn1.Name = "redirectedDrivesDataGridViewTextBoxColumn1";
            // 
            // redirectPortsDataGridViewCheckBoxColumn1
            // 
            this.redirectPortsDataGridViewCheckBoxColumn1.DataPropertyName = "RedirectPorts";
            this.redirectPortsDataGridViewCheckBoxColumn1.HeaderText = "RedirectPorts";
            this.redirectPortsDataGridViewCheckBoxColumn1.Name = "redirectPortsDataGridViewCheckBoxColumn1";
            // 
            // newWindowDataGridViewCheckBoxColumn1
            // 
            this.newWindowDataGridViewCheckBoxColumn1.DataPropertyName = "NewWindow";
            this.newWindowDataGridViewCheckBoxColumn1.HeaderText = "NewWindow";
            this.newWindowDataGridViewCheckBoxColumn1.Name = "newWindowDataGridViewCheckBoxColumn1";
            // 
            // redirectPrintersDataGridViewCheckBoxColumn1
            // 
            this.redirectPrintersDataGridViewCheckBoxColumn1.DataPropertyName = "RedirectPrinters";
            this.redirectPrintersDataGridViewCheckBoxColumn1.HeaderText = "RedirectPrinters";
            this.redirectPrintersDataGridViewCheckBoxColumn1.Name = "redirectPrintersDataGridViewCheckBoxColumn1";
            // 
            // redirectSmartCardsDataGridViewCheckBoxColumn1
            // 
            this.redirectSmartCardsDataGridViewCheckBoxColumn1.DataPropertyName = "RedirectSmartCards";
            this.redirectSmartCardsDataGridViewCheckBoxColumn1.HeaderText = "RedirectSmartCards";
            this.redirectSmartCardsDataGridViewCheckBoxColumn1.Name = "redirectSmartCardsDataGridViewCheckBoxColumn1";
            // 
            // redirectClipboardDataGridViewCheckBoxColumn1
            // 
            this.redirectClipboardDataGridViewCheckBoxColumn1.DataPropertyName = "RedirectClipboard";
            this.redirectClipboardDataGridViewCheckBoxColumn1.HeaderText = "RedirectClipboard";
            this.redirectClipboardDataGridViewCheckBoxColumn1.Name = "redirectClipboardDataGridViewCheckBoxColumn1";
            // 
            // redirectDevicesDataGridViewCheckBoxColumn1
            // 
            this.redirectDevicesDataGridViewCheckBoxColumn1.DataPropertyName = "RedirectDevices";
            this.redirectDevicesDataGridViewCheckBoxColumn1.HeaderText = "RedirectDevices";
            this.redirectDevicesDataGridViewCheckBoxColumn1.Name = "redirectDevicesDataGridViewCheckBoxColumn1";
            // 
            // tsgwUsageMethodDataGridViewTextBoxColumn1
            // 
            this.tsgwUsageMethodDataGridViewTextBoxColumn1.DataPropertyName = "TsgwUsageMethod";
            this.tsgwUsageMethodDataGridViewTextBoxColumn1.HeaderText = "TsgwUsageMethod";
            this.tsgwUsageMethodDataGridViewTextBoxColumn1.Name = "tsgwUsageMethodDataGridViewTextBoxColumn1";
            // 
            // tsgwHostnameDataGridViewTextBoxColumn1
            // 
            this.tsgwHostnameDataGridViewTextBoxColumn1.DataPropertyName = "TsgwHostname";
            this.tsgwHostnameDataGridViewTextBoxColumn1.HeaderText = "TsgwHostname";
            this.tsgwHostnameDataGridViewTextBoxColumn1.Name = "tsgwHostnameDataGridViewTextBoxColumn1";
            // 
            // tsgwCredsSourceDataGridViewTextBoxColumn1
            // 
            this.tsgwCredsSourceDataGridViewTextBoxColumn1.DataPropertyName = "TsgwCredsSource";
            this.tsgwCredsSourceDataGridViewTextBoxColumn1.HeaderText = "TsgwCredsSource";
            this.tsgwCredsSourceDataGridViewTextBoxColumn1.Name = "tsgwCredsSourceDataGridViewTextBoxColumn1";
            // 
            // tsgwSeparateLoginDataGridViewCheckBoxColumn1
            // 
            this.tsgwSeparateLoginDataGridViewCheckBoxColumn1.DataPropertyName = "TsgwSeparateLogin";
            this.tsgwSeparateLoginDataGridViewCheckBoxColumn1.HeaderText = "TsgwSeparateLogin";
            this.tsgwSeparateLoginDataGridViewCheckBoxColumn1.Name = "tsgwSeparateLoginDataGridViewCheckBoxColumn1";
            // 
            // tsgwUsernameDataGridViewTextBoxColumn1
            // 
            this.tsgwUsernameDataGridViewTextBoxColumn1.DataPropertyName = "TsgwUsername";
            this.tsgwUsernameDataGridViewTextBoxColumn1.HeaderText = "TsgwUsername";
            this.tsgwUsernameDataGridViewTextBoxColumn1.Name = "tsgwUsernameDataGridViewTextBoxColumn1";
            // 
            // tsgwDomainDataGridViewTextBoxColumn1
            // 
            this.tsgwDomainDataGridViewTextBoxColumn1.DataPropertyName = "TsgwDomain";
            this.tsgwDomainDataGridViewTextBoxColumn1.HeaderText = "TsgwDomain";
            this.tsgwDomainDataGridViewTextBoxColumn1.Name = "tsgwDomainDataGridViewTextBoxColumn1";
            // 
            // tsgwEncryptedPasswordDataGridViewTextBoxColumn1
            // 
            this.tsgwEncryptedPasswordDataGridViewTextBoxColumn1.DataPropertyName = "TsgwEncryptedPassword";
            this.tsgwEncryptedPasswordDataGridViewTextBoxColumn1.HeaderText = "TsgwEncryptedPassword";
            this.tsgwEncryptedPasswordDataGridViewTextBoxColumn1.Name = "tsgwEncryptedPasswordDataGridViewTextBoxColumn1";
            // 
            // tsgwPasswordDataGridViewTextBoxColumn1
            // 
            this.tsgwPasswordDataGridViewTextBoxColumn1.DataPropertyName = "TsgwPassword";
            this.tsgwPasswordDataGridViewTextBoxColumn1.HeaderText = "TsgwPassword";
            this.tsgwPasswordDataGridViewTextBoxColumn1.Name = "tsgwPasswordDataGridViewTextBoxColumn1";
            // 
            // urlDataGridViewTextBoxColumn1
            // 
            this.urlDataGridViewTextBoxColumn1.DataPropertyName = "Url";
            this.urlDataGridViewTextBoxColumn1.HeaderText = "Url";
            this.urlDataGridViewTextBoxColumn1.Name = "urlDataGridViewTextBoxColumn1";
            // 
            // notesDataGridViewTextBoxColumn1
            // 
            this.notesDataGridViewTextBoxColumn1.DataPropertyName = "Notes";
            this.notesDataGridViewTextBoxColumn1.HeaderText = "Notes";
            this.notesDataGridViewTextBoxColumn1.Name = "notesDataGridViewTextBoxColumn1";
            // 
            // icaServerINIDataGridViewTextBoxColumn1
            // 
            this.icaServerINIDataGridViewTextBoxColumn1.DataPropertyName = "IcaServerINI";
            this.icaServerINIDataGridViewTextBoxColumn1.HeaderText = "IcaServerINI";
            this.icaServerINIDataGridViewTextBoxColumn1.Name = "icaServerINIDataGridViewTextBoxColumn1";
            // 
            // icaClientINIDataGridViewTextBoxColumn1
            // 
            this.icaClientINIDataGridViewTextBoxColumn1.DataPropertyName = "IcaClientINI";
            this.icaClientINIDataGridViewTextBoxColumn1.HeaderText = "IcaClientINI";
            this.icaClientINIDataGridViewTextBoxColumn1.Name = "icaClientINIDataGridViewTextBoxColumn1";
            // 
            // icaEnableEncryptionDataGridViewCheckBoxColumn1
            // 
            this.icaEnableEncryptionDataGridViewCheckBoxColumn1.DataPropertyName = "IcaEnableEncryption";
            this.icaEnableEncryptionDataGridViewCheckBoxColumn1.HeaderText = "IcaEnableEncryption";
            this.icaEnableEncryptionDataGridViewCheckBoxColumn1.Name = "icaEnableEncryptionDataGridViewCheckBoxColumn1";
            // 
            // icaEncryptionLevelDataGridViewTextBoxColumn1
            // 
            this.icaEncryptionLevelDataGridViewTextBoxColumn1.DataPropertyName = "IcaEncryptionLevel";
            this.icaEncryptionLevelDataGridViewTextBoxColumn1.HeaderText = "IcaEncryptionLevel";
            this.icaEncryptionLevelDataGridViewTextBoxColumn1.Name = "icaEncryptionLevelDataGridViewTextBoxColumn1";
            // 
            // portDataGridViewTextBoxColumn1
            // 
            this.portDataGridViewTextBoxColumn1.DataPropertyName = "Port";
            this.portDataGridViewTextBoxColumn1.HeaderText = "Port";
            this.portDataGridViewTextBoxColumn1.Name = "portDataGridViewTextBoxColumn1";
            // 
            // desktopShareDataGridViewTextBoxColumn1
            // 
            this.desktopShareDataGridViewTextBoxColumn1.DataPropertyName = "DesktopShare";
            this.desktopShareDataGridViewTextBoxColumn1.HeaderText = "DesktopShare";
            this.desktopShareDataGridViewTextBoxColumn1.Name = "desktopShareDataGridViewTextBoxColumn1";
            // 
            // executeBeforeConnectDataGridViewCheckBoxColumn1
            // 
            this.executeBeforeConnectDataGridViewCheckBoxColumn1.DataPropertyName = "ExecuteBeforeConnect";
            this.executeBeforeConnectDataGridViewCheckBoxColumn1.HeaderText = "ExecuteBeforeConnect";
            this.executeBeforeConnectDataGridViewCheckBoxColumn1.Name = "executeBeforeConnectDataGridViewCheckBoxColumn1";
            // 
            // executeBeforeConnectCommandDataGridViewTextBoxColumn1
            // 
            this.executeBeforeConnectCommandDataGridViewTextBoxColumn1.DataPropertyName = "ExecuteBeforeConnectCommand";
            this.executeBeforeConnectCommandDataGridViewTextBoxColumn1.HeaderText = "ExecuteBeforeConnectCommand";
            this.executeBeforeConnectCommandDataGridViewTextBoxColumn1.Name = "executeBeforeConnectCommandDataGridViewTextBoxColumn1";
            // 
            // executeBeforeConnectArgsDataGridViewTextBoxColumn1
            // 
            this.executeBeforeConnectArgsDataGridViewTextBoxColumn1.DataPropertyName = "ExecuteBeforeConnectArgs";
            this.executeBeforeConnectArgsDataGridViewTextBoxColumn1.HeaderText = "ExecuteBeforeConnectArgs";
            this.executeBeforeConnectArgsDataGridViewTextBoxColumn1.Name = "executeBeforeConnectArgsDataGridViewTextBoxColumn1";
            // 
            // executeBeforeConnectInitialDirectoryDataGridViewTextBoxColumn1
            // 
            this.executeBeforeConnectInitialDirectoryDataGridViewTextBoxColumn1.DataPropertyName = "ExecuteBeforeConnectInitialDirectory";
            this.executeBeforeConnectInitialDirectoryDataGridViewTextBoxColumn1.HeaderText = "ExecuteBeforeConnectInitialDirectory";
            this.executeBeforeConnectInitialDirectoryDataGridViewTextBoxColumn1.Name = "executeBeforeConnectInitialDirectoryDataGridViewTextBoxColumn1";
            // 
            // executeBeforeConnectWaitForExitDataGridViewCheckBoxColumn1
            // 
            this.executeBeforeConnectWaitForExitDataGridViewCheckBoxColumn1.DataPropertyName = "ExecuteBeforeConnectWaitForExit";
            this.executeBeforeConnectWaitForExitDataGridViewCheckBoxColumn1.HeaderText = "ExecuteBeforeConnectWaitForExit";
            this.executeBeforeConnectWaitForExitDataGridViewCheckBoxColumn1.Name = "executeBeforeConnectWaitForExitDataGridViewCheckBoxColumn1";
            // 
            // disableThemingDataGridViewCheckBoxColumn1
            // 
            this.disableThemingDataGridViewCheckBoxColumn1.DataPropertyName = "DisableTheming";
            this.disableThemingDataGridViewCheckBoxColumn1.HeaderText = "DisableTheming";
            this.disableThemingDataGridViewCheckBoxColumn1.Name = "disableThemingDataGridViewCheckBoxColumn1";
            // 
            // disableMenuAnimationsDataGridViewCheckBoxColumn1
            // 
            this.disableMenuAnimationsDataGridViewCheckBoxColumn1.DataPropertyName = "DisableMenuAnimations";
            this.disableMenuAnimationsDataGridViewCheckBoxColumn1.HeaderText = "DisableMenuAnimations";
            this.disableMenuAnimationsDataGridViewCheckBoxColumn1.Name = "disableMenuAnimationsDataGridViewCheckBoxColumn1";
            // 
            // disableFullWindowDragDataGridViewCheckBoxColumn1
            // 
            this.disableFullWindowDragDataGridViewCheckBoxColumn1.DataPropertyName = "DisableFullWindowDrag";
            this.disableFullWindowDragDataGridViewCheckBoxColumn1.HeaderText = "DisableFullWindowDrag";
            this.disableFullWindowDragDataGridViewCheckBoxColumn1.Name = "disableFullWindowDragDataGridViewCheckBoxColumn1";
            // 
            // disableCursorBlinkingDataGridViewCheckBoxColumn1
            // 
            this.disableCursorBlinkingDataGridViewCheckBoxColumn1.DataPropertyName = "DisableCursorBlinking";
            this.disableCursorBlinkingDataGridViewCheckBoxColumn1.HeaderText = "DisableCursorBlinking";
            this.disableCursorBlinkingDataGridViewCheckBoxColumn1.Name = "disableCursorBlinkingDataGridViewCheckBoxColumn1";
            // 
            // enableDesktopCompositionDataGridViewCheckBoxColumn1
            // 
            this.enableDesktopCompositionDataGridViewCheckBoxColumn1.DataPropertyName = "EnableDesktopComposition";
            this.enableDesktopCompositionDataGridViewCheckBoxColumn1.HeaderText = "EnableDesktopComposition";
            this.enableDesktopCompositionDataGridViewCheckBoxColumn1.Name = "enableDesktopCompositionDataGridViewCheckBoxColumn1";
            // 
            // enableFontSmoothingDataGridViewCheckBoxColumn1
            // 
            this.enableFontSmoothingDataGridViewCheckBoxColumn1.DataPropertyName = "EnableFontSmoothing";
            this.enableFontSmoothingDataGridViewCheckBoxColumn1.HeaderText = "EnableFontSmoothing";
            this.enableFontSmoothingDataGridViewCheckBoxColumn1.Name = "enableFontSmoothingDataGridViewCheckBoxColumn1";
            // 
            // disableCursorShadowDataGridViewCheckBoxColumn1
            // 
            this.disableCursorShadowDataGridViewCheckBoxColumn1.DataPropertyName = "DisableCursorShadow";
            this.disableCursorShadowDataGridViewCheckBoxColumn1.HeaderText = "DisableCursorShadow";
            this.disableCursorShadowDataGridViewCheckBoxColumn1.Name = "disableCursorShadowDataGridViewCheckBoxColumn1";
            // 
            // disableWallPaperDataGridViewCheckBoxColumn1
            // 
            this.disableWallPaperDataGridViewCheckBoxColumn1.DataPropertyName = "DisableWallPaper";
            this.disableWallPaperDataGridViewCheckBoxColumn1.HeaderText = "DisableWallPaper";
            this.disableWallPaperDataGridViewCheckBoxColumn1.Name = "disableWallPaperDataGridViewCheckBoxColumn1";
            // 
            // tagsDataGridViewTextBoxColumn1
            // 
            this.tagsDataGridViewTextBoxColumn1.DataPropertyName = "Tags";
            this.tagsDataGridViewTextBoxColumn1.HeaderText = "Tags";
            this.tagsDataGridViewTextBoxColumn1.Name = "tagsDataGridViewTextBoxColumn1";
            // 
            // lockAttributesDataGridViewTextBoxColumn1
            // 
            this.lockAttributesDataGridViewTextBoxColumn1.DataPropertyName = "LockAttributes";
            this.lockAttributesDataGridViewTextBoxColumn1.HeaderText = "LockAttributes";
            this.lockAttributesDataGridViewTextBoxColumn1.Name = "lockAttributesDataGridViewTextBoxColumn1";
            this.lockAttributesDataGridViewTextBoxColumn1.ReadOnly = true;
            // 
            // lockAllAttributesExceptDataGridViewTextBoxColumn1
            // 
            this.lockAllAttributesExceptDataGridViewTextBoxColumn1.DataPropertyName = "LockAllAttributesExcept";
            this.lockAllAttributesExceptDataGridViewTextBoxColumn1.HeaderText = "LockAllAttributesExcept";
            this.lockAllAttributesExceptDataGridViewTextBoxColumn1.Name = "lockAllAttributesExceptDataGridViewTextBoxColumn1";
            this.lockAllAttributesExceptDataGridViewTextBoxColumn1.ReadOnly = true;
            // 
            // lockElementsDataGridViewTextBoxColumn1
            // 
            this.lockElementsDataGridViewTextBoxColumn1.DataPropertyName = "LockElements";
            this.lockElementsDataGridViewTextBoxColumn1.HeaderText = "LockElements";
            this.lockElementsDataGridViewTextBoxColumn1.Name = "lockElementsDataGridViewTextBoxColumn1";
            this.lockElementsDataGridViewTextBoxColumn1.ReadOnly = true;
            // 
            // lockAllElementsExceptDataGridViewTextBoxColumn1
            // 
            this.lockAllElementsExceptDataGridViewTextBoxColumn1.DataPropertyName = "LockAllElementsExcept";
            this.lockAllElementsExceptDataGridViewTextBoxColumn1.HeaderText = "LockAllElementsExcept";
            this.lockAllElementsExceptDataGridViewTextBoxColumn1.Name = "lockAllElementsExceptDataGridViewTextBoxColumn1";
            this.lockAllElementsExceptDataGridViewTextBoxColumn1.ReadOnly = true;
            // 
            // lockItemDataGridViewCheckBoxColumn1
            // 
            this.lockItemDataGridViewCheckBoxColumn1.DataPropertyName = "LockItem";
            this.lockItemDataGridViewCheckBoxColumn1.HeaderText = "LockItem";
            this.lockItemDataGridViewCheckBoxColumn1.Name = "lockItemDataGridViewCheckBoxColumn1";
            // 
            // elementInformationDataGridViewTextBoxColumn1
            // 
            this.elementInformationDataGridViewTextBoxColumn1.DataPropertyName = "ElementInformation";
            this.elementInformationDataGridViewTextBoxColumn1.HeaderText = "ElementInformation";
            this.elementInformationDataGridViewTextBoxColumn1.Name = "elementInformationDataGridViewTextBoxColumn1";
            this.elementInformationDataGridViewTextBoxColumn1.ReadOnly = true;
            // 
            // currentConfigurationDataGridViewTextBoxColumn1
            // 
            this.currentConfigurationDataGridViewTextBoxColumn1.DataPropertyName = "CurrentConfiguration";
            this.currentConfigurationDataGridViewTextBoxColumn1.HeaderText = "CurrentConfiguration";
            this.currentConfigurationDataGridViewTextBoxColumn1.Name = "currentConfigurationDataGridViewTextBoxColumn1";
            this.currentConfigurationDataGridViewTextBoxColumn1.ReadOnly = true;
            // 
            // OrganizeFavoritesForm
            // 
            this.AcceptButton = this.btnClose;
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 17F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.btnClose;
            this.ClientSize = new System.Drawing.Size(726, 361);
            this.Controls.Add(this.dataGridFavorites);
            this.Controls.Add(this.btnExport);
            this.Controls.Add(this.ScanNetworkButton);
            this.Controls.Add(this.ActiveDirectoryButton);
            this.Controls.Add(this.ImportButton);
            this.Controls.Add(this.btnNew);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.btnRename);
            this.Controls.Add(this.btnCopy);
            this.Controls.Add(this.btnDelete);
            this.Controls.Add(this.btnEdit);
            this.Controls.Add(this.btnClose);
            this.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(177)));
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.KeyPreview = true;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.MinimumSize = new System.Drawing.Size(734, 388);
            this.Name = "OrganizeFavoritesForm";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Terminals - Organize Favorites";
            this.Shown += new System.EventHandler(this.OrganizeFavoritesForm_Shown);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.ConnectionManager_KeyDown);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridFavorites)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.bsFavorites)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnClose;
        private System.Windows.Forms.Button btnEdit;
        private System.Windows.Forms.Button btnDelete;
        private System.Windows.Forms.Button btnCopy;
        private System.Windows.Forms.Button btnRename;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button btnNew;
        private System.Windows.Forms.Button ImportButton;
        private System.Windows.Forms.OpenFileDialog ImportOpenFileDialog;
        private System.Windows.Forms.Button ActiveDirectoryButton;
        private System.Windows.Forms.Button ScanNetworkButton;
        private System.Windows.Forms.Button btnExport;
        private System.Windows.Forms.BindingSource bsFavorites;
        private Terminals.SortableUnboundGrid dataGridFavorites;
        private System.Windows.Forms.DataGridViewTextBoxColumn performanceFlagsDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewCheckBoxColumn telnetDataGridViewCheckBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn telnetRowsDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn telnetColsDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn shutdownTimeoutDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn overallTimeoutDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn connectionTimeoutDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn idleTimeoutDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn securityWorkingFolderDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn securityStartProgramDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn credentialDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewCheckBoxColumn securityFullScreenDataGridViewCheckBoxColumn;
        private System.Windows.Forms.DataGridViewCheckBoxColumn enableSecuritySettingsDataGridViewCheckBoxColumn;
        private System.Windows.Forms.DataGridViewCheckBoxColumn grabFocusOnConnectDataGridViewCheckBoxColumn;
        private System.Windows.Forms.DataGridViewCheckBoxColumn enableEncryptionDataGridViewCheckBoxColumn;
        private System.Windows.Forms.DataGridViewCheckBoxColumn disableWindowsKeyDataGridViewCheckBoxColumn;
        private System.Windows.Forms.DataGridViewCheckBoxColumn doubleClickDetectDataGridViewCheckBoxColumn;
        private System.Windows.Forms.DataGridViewCheckBoxColumn displayConnectionBarDataGridViewCheckBoxColumn;
        private System.Windows.Forms.DataGridViewCheckBoxColumn disableControlAltDeleteDataGridViewCheckBoxColumn;
        private System.Windows.Forms.DataGridViewCheckBoxColumn acceleratorPassthroughDataGridViewCheckBoxColumn;
        private System.Windows.Forms.DataGridViewCheckBoxColumn enableCompressionDataGridViewCheckBoxColumn;
        private System.Windows.Forms.DataGridViewCheckBoxColumn bitmapPeristenceDataGridViewCheckBoxColumn;
        private System.Windows.Forms.DataGridViewCheckBoxColumn enableTLSAuthenticationDataGridViewCheckBoxColumn;
        private System.Windows.Forms.DataGridViewCheckBoxColumn enableNLAAuthenticationDataGridViewCheckBoxColumn;
        private System.Windows.Forms.DataGridViewCheckBoxColumn allowBackgroundInputDataGridViewCheckBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn iCAApplicationNameDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn iCAApplicationWorkingFolderDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn iCAApplicationPathDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewCheckBoxColumn vMRCReducedColorsModeDataGridViewCheckBoxColumn;
        private System.Windows.Forms.DataGridViewCheckBoxColumn vMRCAdministratorModeDataGridViewCheckBoxColumn;
        private System.Windows.Forms.DataGridViewCheckBoxColumn sSH1DataGridViewCheckBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn consoleRowsDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn consoleColsDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn consoleFontDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn consoleBackColorDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn consoleTextColorDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn consoleCursorColorDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn protocolDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn toolBarIconDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn telnetFontDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn telnetBackColorDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn telnetTextColorDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn telnetCursorColorDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn nameDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn serverNameDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn domainNameDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn authMethodDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn keyTagDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn userNameDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn encryptedPasswordDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn passwordDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewCheckBoxColumn vncAutoScaleDataGridViewCheckBoxColumn;
        private System.Windows.Forms.DataGridViewCheckBoxColumn vncViewOnlyDataGridViewCheckBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn vncDisplayNumberDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewCheckBoxColumn connectToConsoleDataGridViewCheckBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn desktopSizeHeightDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn desktopSizeWidthDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn desktopSizeDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn colorsDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn soundsDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn redirectedDrivesDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewCheckBoxColumn redirectPortsDataGridViewCheckBoxColumn;
        private System.Windows.Forms.DataGridViewCheckBoxColumn newWindowDataGridViewCheckBoxColumn;
        private System.Windows.Forms.DataGridViewCheckBoxColumn redirectPrintersDataGridViewCheckBoxColumn;
        private System.Windows.Forms.DataGridViewCheckBoxColumn redirectSmartCardsDataGridViewCheckBoxColumn;
        private System.Windows.Forms.DataGridViewCheckBoxColumn redirectClipboardDataGridViewCheckBoxColumn;
        private System.Windows.Forms.DataGridViewCheckBoxColumn redirectDevicesDataGridViewCheckBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn tsgwUsageMethodDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn tsgwHostnameDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn tsgwCredsSourceDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewCheckBoxColumn tsgwSeparateLoginDataGridViewCheckBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn tsgwUsernameDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn tsgwDomainDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn tsgwEncryptedPasswordDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn tsgwPasswordDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn urlDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn notesDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn icaServerINIDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn icaClientINIDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewCheckBoxColumn icaEnableEncryptionDataGridViewCheckBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn icaEncryptionLevelDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn portDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn desktopShareDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewCheckBoxColumn executeBeforeConnectDataGridViewCheckBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn executeBeforeConnectCommandDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn executeBeforeConnectArgsDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn executeBeforeConnectInitialDirectoryDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewCheckBoxColumn executeBeforeConnectWaitForExitDataGridViewCheckBoxColumn;
        private System.Windows.Forms.DataGridViewCheckBoxColumn disableThemingDataGridViewCheckBoxColumn;
        private System.Windows.Forms.DataGridViewCheckBoxColumn disableMenuAnimationsDataGridViewCheckBoxColumn;
        private System.Windows.Forms.DataGridViewCheckBoxColumn disableFullWindowDragDataGridViewCheckBoxColumn;
        private System.Windows.Forms.DataGridViewCheckBoxColumn disableCursorBlinkingDataGridViewCheckBoxColumn;
        private System.Windows.Forms.DataGridViewCheckBoxColumn enableDesktopCompositionDataGridViewCheckBoxColumn;
        private System.Windows.Forms.DataGridViewCheckBoxColumn enableFontSmoothingDataGridViewCheckBoxColumn;
        private System.Windows.Forms.DataGridViewCheckBoxColumn disableCursorShadowDataGridViewCheckBoxColumn;
        private System.Windows.Forms.DataGridViewCheckBoxColumn disableWallPaperDataGridViewCheckBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn tagsDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn lockAttributesDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn lockAllAttributesExceptDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn lockElementsDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn lockAllElementsExceptDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewCheckBoxColumn lockItemDataGridViewCheckBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn elementInformationDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn currentConfigurationDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn colName;
        private System.Windows.Forms.DataGridViewTextBoxColumn colComputer;
        private System.Windows.Forms.DataGridViewTextBoxColumn colProtocol;
        private System.Windows.Forms.DataGridViewTextBoxColumn colUserName;
        private System.Windows.Forms.DataGridViewTextBoxColumn colCredentials;
        private System.Windows.Forms.DataGridViewTextBoxColumn colTags;
        private System.Windows.Forms.DataGridViewTextBoxColumn colNotes;
        private System.Windows.Forms.DataGridViewTextBoxColumn performanceFlagsDataGridViewTextBoxColumn1;
        private System.Windows.Forms.DataGridViewCheckBoxColumn telnetDataGridViewCheckBoxColumn1;
        private System.Windows.Forms.DataGridViewTextBoxColumn telnetRowsDataGridViewTextBoxColumn1;
        private System.Windows.Forms.DataGridViewTextBoxColumn telnetColsDataGridViewTextBoxColumn1;
        private System.Windows.Forms.DataGridViewTextBoxColumn shutdownTimeoutDataGridViewTextBoxColumn1;
        private System.Windows.Forms.DataGridViewTextBoxColumn overallTimeoutDataGridViewTextBoxColumn1;
        private System.Windows.Forms.DataGridViewTextBoxColumn connectionTimeoutDataGridViewTextBoxColumn1;
        private System.Windows.Forms.DataGridViewTextBoxColumn idleTimeoutDataGridViewTextBoxColumn1;
        private System.Windows.Forms.DataGridViewTextBoxColumn securityWorkingFolderDataGridViewTextBoxColumn1;
        private System.Windows.Forms.DataGridViewTextBoxColumn securityStartProgramDataGridViewTextBoxColumn1;
        private System.Windows.Forms.DataGridViewTextBoxColumn credentialDataGridViewTextBoxColumn1;
        private System.Windows.Forms.DataGridViewCheckBoxColumn securityFullScreenDataGridViewCheckBoxColumn1;
        private System.Windows.Forms.DataGridViewCheckBoxColumn enableSecuritySettingsDataGridViewCheckBoxColumn1;
        private System.Windows.Forms.DataGridViewCheckBoxColumn grabFocusOnConnectDataGridViewCheckBoxColumn1;
        private System.Windows.Forms.DataGridViewCheckBoxColumn enableEncryptionDataGridViewCheckBoxColumn1;
        private System.Windows.Forms.DataGridViewCheckBoxColumn disableWindowsKeyDataGridViewCheckBoxColumn1;
        private System.Windows.Forms.DataGridViewCheckBoxColumn doubleClickDetectDataGridViewCheckBoxColumn1;
        private System.Windows.Forms.DataGridViewCheckBoxColumn displayConnectionBarDataGridViewCheckBoxColumn1;
        private System.Windows.Forms.DataGridViewCheckBoxColumn disableControlAltDeleteDataGridViewCheckBoxColumn1;
        private System.Windows.Forms.DataGridViewCheckBoxColumn acceleratorPassthroughDataGridViewCheckBoxColumn1;
        private System.Windows.Forms.DataGridViewCheckBoxColumn enableCompressionDataGridViewCheckBoxColumn1;
        private System.Windows.Forms.DataGridViewCheckBoxColumn bitmapPeristenceDataGridViewCheckBoxColumn1;
        private System.Windows.Forms.DataGridViewCheckBoxColumn enableTLSAuthenticationDataGridViewCheckBoxColumn1;
        private System.Windows.Forms.DataGridViewCheckBoxColumn enableNLAAuthenticationDataGridViewCheckBoxColumn1;
        private System.Windows.Forms.DataGridViewCheckBoxColumn allowBackgroundInputDataGridViewCheckBoxColumn1;
        private System.Windows.Forms.DataGridViewTextBoxColumn iCAApplicationNameDataGridViewTextBoxColumn1;
        private System.Windows.Forms.DataGridViewTextBoxColumn iCAApplicationWorkingFolderDataGridViewTextBoxColumn1;
        private System.Windows.Forms.DataGridViewTextBoxColumn iCAApplicationPathDataGridViewTextBoxColumn1;
        private System.Windows.Forms.DataGridViewCheckBoxColumn vMRCReducedColorsModeDataGridViewCheckBoxColumn1;
        private System.Windows.Forms.DataGridViewCheckBoxColumn vMRCAdministratorModeDataGridViewCheckBoxColumn1;
        private System.Windows.Forms.DataGridViewCheckBoxColumn sSH1DataGridViewCheckBoxColumn1;
        private System.Windows.Forms.DataGridViewTextBoxColumn consoleRowsDataGridViewTextBoxColumn1;
        private System.Windows.Forms.DataGridViewTextBoxColumn consoleColsDataGridViewTextBoxColumn1;
        private System.Windows.Forms.DataGridViewTextBoxColumn consoleFontDataGridViewTextBoxColumn1;
        private System.Windows.Forms.DataGridViewTextBoxColumn consoleBackColorDataGridViewTextBoxColumn1;
        private System.Windows.Forms.DataGridViewTextBoxColumn consoleTextColorDataGridViewTextBoxColumn1;
        private System.Windows.Forms.DataGridViewTextBoxColumn consoleCursorColorDataGridViewTextBoxColumn1;
        private System.Windows.Forms.DataGridViewTextBoxColumn protocolDataGridViewTextBoxColumn1;
        private System.Windows.Forms.DataGridViewTextBoxColumn toolBarIconDataGridViewTextBoxColumn1;
        private System.Windows.Forms.DataGridViewTextBoxColumn telnetFontDataGridViewTextBoxColumn1;
        private System.Windows.Forms.DataGridViewTextBoxColumn telnetBackColorDataGridViewTextBoxColumn1;
        private System.Windows.Forms.DataGridViewTextBoxColumn telnetTextColorDataGridViewTextBoxColumn1;
        private System.Windows.Forms.DataGridViewTextBoxColumn telnetCursorColorDataGridViewTextBoxColumn1;
        private System.Windows.Forms.DataGridViewTextBoxColumn nameDataGridViewTextBoxColumn1;
        private System.Windows.Forms.DataGridViewTextBoxColumn serverNameDataGridViewTextBoxColumn1;
        private System.Windows.Forms.DataGridViewTextBoxColumn domainNameDataGridViewTextBoxColumn1;
        private System.Windows.Forms.DataGridViewTextBoxColumn authMethodDataGridViewTextBoxColumn1;
        private System.Windows.Forms.DataGridViewTextBoxColumn keyTagDataGridViewTextBoxColumn1;
        private System.Windows.Forms.DataGridViewTextBoxColumn userNameDataGridViewTextBoxColumn1;
        private System.Windows.Forms.DataGridViewTextBoxColumn encryptedPasswordDataGridViewTextBoxColumn1;
        private System.Windows.Forms.DataGridViewTextBoxColumn passwordDataGridViewTextBoxColumn1;
        private System.Windows.Forms.DataGridViewCheckBoxColumn vncAutoScaleDataGridViewCheckBoxColumn1;
        private System.Windows.Forms.DataGridViewCheckBoxColumn vncViewOnlyDataGridViewCheckBoxColumn1;
        private System.Windows.Forms.DataGridViewTextBoxColumn vncDisplayNumberDataGridViewTextBoxColumn1;
        private System.Windows.Forms.DataGridViewCheckBoxColumn connectToConsoleDataGridViewCheckBoxColumn1;
        private System.Windows.Forms.DataGridViewTextBoxColumn desktopSizeHeightDataGridViewTextBoxColumn1;
        private System.Windows.Forms.DataGridViewTextBoxColumn desktopSizeWidthDataGridViewTextBoxColumn1;
        private System.Windows.Forms.DataGridViewTextBoxColumn desktopSizeDataGridViewTextBoxColumn1;
        private System.Windows.Forms.DataGridViewTextBoxColumn colorsDataGridViewTextBoxColumn1;
        private System.Windows.Forms.DataGridViewTextBoxColumn soundsDataGridViewTextBoxColumn1;
        private System.Windows.Forms.DataGridViewTextBoxColumn redirectedDrivesDataGridViewTextBoxColumn1;
        private System.Windows.Forms.DataGridViewCheckBoxColumn redirectPortsDataGridViewCheckBoxColumn1;
        private System.Windows.Forms.DataGridViewCheckBoxColumn newWindowDataGridViewCheckBoxColumn1;
        private System.Windows.Forms.DataGridViewCheckBoxColumn redirectPrintersDataGridViewCheckBoxColumn1;
        private System.Windows.Forms.DataGridViewCheckBoxColumn redirectSmartCardsDataGridViewCheckBoxColumn1;
        private System.Windows.Forms.DataGridViewCheckBoxColumn redirectClipboardDataGridViewCheckBoxColumn1;
        private System.Windows.Forms.DataGridViewCheckBoxColumn redirectDevicesDataGridViewCheckBoxColumn1;
        private System.Windows.Forms.DataGridViewTextBoxColumn tsgwUsageMethodDataGridViewTextBoxColumn1;
        private System.Windows.Forms.DataGridViewTextBoxColumn tsgwHostnameDataGridViewTextBoxColumn1;
        private System.Windows.Forms.DataGridViewTextBoxColumn tsgwCredsSourceDataGridViewTextBoxColumn1;
        private System.Windows.Forms.DataGridViewCheckBoxColumn tsgwSeparateLoginDataGridViewCheckBoxColumn1;
        private System.Windows.Forms.DataGridViewTextBoxColumn tsgwUsernameDataGridViewTextBoxColumn1;
        private System.Windows.Forms.DataGridViewTextBoxColumn tsgwDomainDataGridViewTextBoxColumn1;
        private System.Windows.Forms.DataGridViewTextBoxColumn tsgwEncryptedPasswordDataGridViewTextBoxColumn1;
        private System.Windows.Forms.DataGridViewTextBoxColumn tsgwPasswordDataGridViewTextBoxColumn1;
        private System.Windows.Forms.DataGridViewTextBoxColumn urlDataGridViewTextBoxColumn1;
        private System.Windows.Forms.DataGridViewTextBoxColumn notesDataGridViewTextBoxColumn1;
        private System.Windows.Forms.DataGridViewTextBoxColumn icaServerINIDataGridViewTextBoxColumn1;
        private System.Windows.Forms.DataGridViewTextBoxColumn icaClientINIDataGridViewTextBoxColumn1;
        private System.Windows.Forms.DataGridViewCheckBoxColumn icaEnableEncryptionDataGridViewCheckBoxColumn1;
        private System.Windows.Forms.DataGridViewTextBoxColumn icaEncryptionLevelDataGridViewTextBoxColumn1;
        private System.Windows.Forms.DataGridViewTextBoxColumn portDataGridViewTextBoxColumn1;
        private System.Windows.Forms.DataGridViewTextBoxColumn desktopShareDataGridViewTextBoxColumn1;
        private System.Windows.Forms.DataGridViewCheckBoxColumn executeBeforeConnectDataGridViewCheckBoxColumn1;
        private System.Windows.Forms.DataGridViewTextBoxColumn executeBeforeConnectCommandDataGridViewTextBoxColumn1;
        private System.Windows.Forms.DataGridViewTextBoxColumn executeBeforeConnectArgsDataGridViewTextBoxColumn1;
        private System.Windows.Forms.DataGridViewTextBoxColumn executeBeforeConnectInitialDirectoryDataGridViewTextBoxColumn1;
        private System.Windows.Forms.DataGridViewCheckBoxColumn executeBeforeConnectWaitForExitDataGridViewCheckBoxColumn1;
        private System.Windows.Forms.DataGridViewCheckBoxColumn disableThemingDataGridViewCheckBoxColumn1;
        private System.Windows.Forms.DataGridViewCheckBoxColumn disableMenuAnimationsDataGridViewCheckBoxColumn1;
        private System.Windows.Forms.DataGridViewCheckBoxColumn disableFullWindowDragDataGridViewCheckBoxColumn1;
        private System.Windows.Forms.DataGridViewCheckBoxColumn disableCursorBlinkingDataGridViewCheckBoxColumn1;
        private System.Windows.Forms.DataGridViewCheckBoxColumn enableDesktopCompositionDataGridViewCheckBoxColumn1;
        private System.Windows.Forms.DataGridViewCheckBoxColumn enableFontSmoothingDataGridViewCheckBoxColumn1;
        private System.Windows.Forms.DataGridViewCheckBoxColumn disableCursorShadowDataGridViewCheckBoxColumn1;
        private System.Windows.Forms.DataGridViewCheckBoxColumn disableWallPaperDataGridViewCheckBoxColumn1;
        private System.Windows.Forms.DataGridViewTextBoxColumn tagsDataGridViewTextBoxColumn1;
        private System.Windows.Forms.DataGridViewTextBoxColumn lockAttributesDataGridViewTextBoxColumn1;
        private System.Windows.Forms.DataGridViewTextBoxColumn lockAllAttributesExceptDataGridViewTextBoxColumn1;
        private System.Windows.Forms.DataGridViewTextBoxColumn lockElementsDataGridViewTextBoxColumn1;
        private System.Windows.Forms.DataGridViewTextBoxColumn lockAllElementsExceptDataGridViewTextBoxColumn1;
        private System.Windows.Forms.DataGridViewCheckBoxColumn lockItemDataGridViewCheckBoxColumn1;
        private System.Windows.Forms.DataGridViewTextBoxColumn elementInformationDataGridViewTextBoxColumn1;
        private System.Windows.Forms.DataGridViewTextBoxColumn currentConfigurationDataGridViewTextBoxColumn1;
    }
}