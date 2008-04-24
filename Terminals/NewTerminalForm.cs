using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Microsoft.Win32;
using Terminals.Properties;
using System.Collections.Specialized;
using System.Configuration;
using System.IO;
using FalafelSoftware.TransPort;

namespace Terminals
{
    internal partial class NewTerminalForm : Form
    {
        Terminals.Network.Servers.TerminalServerManager terminalServerManager1 = new Terminals.Network.Servers.TerminalServerManager();

        public NewTerminalForm(string server, bool connect)
        {
            InitializeComponent();

            LoadMRUs();

            cmbResolution.SelectedIndex = 4;
            cmbColors.SelectedIndex = 1;
            cmbSounds.SelectedIndex = 2;
            SetOkButtonState();
            SetOkTitle(connect);
            this.ProtocolComboBox.SelectedIndex = 0;


            string Server = server;
            int port = 3389;
            GetServerAndPort(server, out Server, out port);
            cmbServers.Text = Server;
            txtPort.Text = port.ToString();

        }
        private void NewTerminalForm_Load(object sender, EventArgs e)
        {
            //LoadDialupConnections();
            this.SuspendLayout();
            // 
            // terminalServerManager1
            // 
            this.terminalServerManager1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.terminalServerManager1.Location = new System.Drawing.Point(0, 0);
            this.terminalServerManager1.Name = "terminalServerManager1";
            this.terminalServerManager1.Size = new System.Drawing.Size(748, 309);
            this.terminalServerManager1.TabIndex = 0;
            this.tabPage10.Controls.Add(terminalServerManager1);
            this.ResumeLayout(true);

        }
        private Dictionary<string, RASENTRY> dialupList = new Dictionary<string, RASENTRY>();
        private void LoadDialupConnections()
        {
            dialupList = new Dictionary<string, RASENTRY>();
            System.Collections.ArrayList rasEntries = new System.Collections.ArrayList();
            RasError error = ras1.ListEntries(ref rasEntries);
            foreach(string item in rasEntries)
            {
                RASENTRY details = new RASENTRY();
                ras1.GetEntry(item, ref details);
                dialupList.Add(item, details);
                if(!cmbServers.Items.Contains(item)) this.cmbServers.Items.Add(item);

            }
        }

        private void SetOkTitle(bool connect)
        {
            if(connect)
                btnOk.Text = "Co&nnect";
            else
                btnOk.Text = "OK";
        }

        public NewTerminalForm(FavoriteConfigurationElement favorite)
        {
            InitializeComponent();
            LoadMRUs();
            SetOkTitle(false);
            this.Text = "Edit Connection";
            FillControls(favorite);
            SetOkButtonState();
        }

        private void LoadMRUs()
        {
            cmbServers.Items.AddRange(Settings.MRUServerNames);
            cmbDomains.Items.AddRange(Settings.MRUDomainNames);
            cmbUsers.Items.AddRange(Settings.MRUUserNames);
            txtTag.AutoCompleteCustomSource.AddRange(Settings.Tags);
        }

        private void SaveMRUs()
        {
            Settings.AddServerMRUItem(cmbServers.Text);
            Settings.AddDomainMRUItem(cmbDomains.Text);
            Settings.AddUserMRUItem(cmbUsers.Text);
        }

        private void FillControls(FavoriteConfigurationElement favorite)
        {
            BackColorTextBox.Text = favorite.TelnetBackColor;
            TelnetFontTextbox.Text = favorite.TelnetFont;
            TelnetCursorColorTextBox.Text = favorite.TelnetCursorColor;
            TelnetTextColorTextBox.Text = favorite.TelnetTextColor;

            ProtocolComboBox.SelectedItem = favorite.Protocol;
            VMRCAdminModeCheckbox.Checked = favorite.VMRCAdministratorMode;
            TelnetRadioButton.Checked = favorite.Telnet;
            SSHRadioButton.Checked = !favorite.Telnet;
            ColumnsTextBox.Text = favorite.TelnetCols.ToString();
            RowsTextBox.Text = favorite.TelnetRows.ToString();
            VMRCReducedColorsCheckbox.Checked = favorite.VMRCReducedColorsMode;
            txtName.Text = favorite.Name;
            cmbServers.Text = favorite.ServerName;
            cmbDomains.Text = favorite.DomainName;
            cmbUsers.Text = favorite.UserName;
            txtPassword.Text = favorite.Password;
            chkSavePassword.Checked = favorite.Password != "";
            cmbResolution.SelectedIndex = (int)favorite.DesktopSize;
            cmbColors.SelectedIndex = (int)favorite.Colors;
            chkConnectToConsole.Checked = favorite.ConnectToConsole;

            chkAddtoToolbar.Checked = Settings.HasToolbarButton(favorite.Name);
            chkDrives.Checked = favorite.RedirectDrives;
            chkSerialPorts.Checked = favorite.RedirectPorts;
            chkPrinters.Checked = favorite.RedirectPrinters;
            chkRedirectClipboard.Checked = favorite.RedirectClipboard;
            chkRedirectDevices.Checked = favorite.RedirectDevices;
            chkRedirectSmartcards.Checked = favorite.RedirectSmartCards;
            cmbSounds.SelectedIndex = (int)favorite.Sounds;
            txtPort.Text = favorite.Port.ToString();
            txtDesktopShare.Text = favorite.DesktopShare;
            chkExecuteBeforeConnect.Checked = favorite.ExecuteBeforeConnect;
            txtCommand.Text = favorite.ExecuteBeforeConnectCommand;
            txtArguments.Text = favorite.ExecuteBeforeConnectArgs;
            txtInitialDirectory.Text = favorite.ExecuteBeforeConnectInitialDirectory;
            chkWaitForExit.Checked = favorite.ExecuteBeforeConnectWaitForExit;

            string[] tagsArray = favorite.Tags.Split(',');
            if(!((tagsArray.Length == 1) && (String.IsNullOrEmpty(tagsArray[0]))))
            {
                foreach(string tag in tagsArray)
                {
                    lvConnectionTags.Items.Add(tag, tag, -1);
                }
            }

            if(favorite.ToolBarIcon != null && System.IO.File.Exists(favorite.ToolBarIcon))
            {
                this.pictureBox2.Load(favorite.ToolBarIcon);
                this.currentToolBarFileName = favorite.ToolBarIcon;
            }

            //extended settings
            this.ShutdownTimeoutTextBox.Text = favorite.ShutdownTimeout.ToString();
            this.OverallTimeoutTextbox.Text = favorite.OverallTimeout.ToString();
            this.SingleTimeOutTextbox.Text = favorite.ConnectionTimeout.ToString();
            this.IdleTimeoutMinutesTextBox.Text = favorite.IdleTimeout.ToString();
            this.SecurityWorkingFolderTextBox.Text = favorite.SecurityWorkingFolder;
            this.SecuriytStartProgramTextbox.Text = favorite.SecurityStartProgram;
            this.SecurityStartFullScreenCheckbox.Checked = favorite.SecurityFullScreen;
            this.SecuritySettingsEnabledCheckbox.Checked = favorite.EnableSecuritySettings;
            this.GrabFocusOnConnectCheckbox.Checked = favorite.GrabFocusOnConnect;
            this.EnableEncryptionCheckbox.Checked = favorite.EnableEncryption;
            this.DisableWindowsKeyCheckbox.Checked = favorite.DisableWindowsKey;
            this.DetectDoubleClicksCheckbox.Checked = favorite.DoubleClickDetect;
            this.DisplayConnectionBarCheckbox.Checked = favorite.DisplayConnectionBar;
            this.DisableControlAltDeleteCheckbox.Checked= favorite.DisableControlAltDelete;
            this.AcceleratorPassthroughCheckBox.Checked = favorite.AcceleratorPassthrough;
            this.EnableCompressionCheckbox.Checked = favorite.EnableCompression;
            this.EnableBitmapPersistanceCheckbox.Checked = favorite.BitmapPeristence;
            this.AllowBackgroundInputCheckBox.Checked = favorite.AllowBackgroundInput;

            chkDisableCursorShadow.Checked = false;
            chkDisableCursorBlinking.Checked = false;
            chkDisableFullWindowDrag.Checked = false;
            chkDisableMenuAnimations.Checked = false;
            chkDisableThemes.Checked = false;
            chkDisableWallpaper.Checked = false;


            if(favorite.PerformanceFlags > 0)
            {
                chkDisableCursorShadow.Checked = favorite.DisableCursorShadow;
                chkDisableCursorBlinking.Checked = favorite.DisableCursorBlinking;
                chkDisableFullWindowDrag.Checked = favorite.DisableFullWindowDrag;
                chkDisableMenuAnimations.Checked = favorite.DisableMenuAnimations;
                chkDisableThemes.Checked = favorite.DisableTheming;
                chkDisableWallpaper.Checked = favorite.DisableWallPaper;
            }
            this.widthUpDown.Value = (decimal)favorite.DesktopSizeWidth;
            this.heightUpDown.Value = (decimal)favorite.DesktopSizeHeight;


            foreach(string tag in Settings.Tags) {
                ListViewItem lvi = new ListViewItem(tag);
                this.AllTagsListView.Items.Add(lvi);
            }

            httpUrlTextBox.Text = favorite.Url;

            ICAClientINI.Text = favorite.IcaClientINI;
            ICAServerINI.Text = favorite.IcaServerINI;
            ICAEncryptionLevelCombobox.Text = favorite.IcaEncryptionLevel;
            ICAEnableEncryptionCheckbox.Checked = favorite.IcaEnableEncryption;
            ICAEncryptionLevelCombobox.Enabled = ICAEnableEncryptionCheckbox.Checked;

            NotesTextbox.Text = favorite.Notes;

        }

        private bool FillFavorite()
        {
            try {
                favorite.VMRCAdministratorMode = VMRCAdminModeCheckbox.Checked;
                favorite.VMRCReducedColorsMode = VMRCReducedColorsCheckbox.Checked;
                favorite.Telnet = TelnetRadioButton.Checked;
                favorite.TelnetCols = Convert.ToInt32(ColumnsTextBox.Text);
                favorite.TelnetRows = Convert.ToInt32(RowsTextBox.Text);
                favorite.TelnetFont = TelnetFontTextbox.Text;
                favorite.TelnetCursorColor = TelnetCursorColorTextBox.Text;
                favorite.TelnetTextColor = TelnetTextColorTextBox.Text;

                favorite.TelnetBackColor = BackColorTextBox.Text;
                favorite.Protocol = ProtocolComboBox.SelectedItem.ToString();
                favorite.ServerName = ValidateServer(cmbServers.Text);
                favorite.DomainName = cmbDomains.Text;
                favorite.UserName = cmbUsers.Text;
                favorite.Password = (chkSavePassword.Checked ? txtPassword.Text : "");
                favorite.DesktopSize = (DesktopSize)cmbResolution.SelectedIndex;
                favorite.Colors = (Colors)cmbColors.SelectedIndex;
                favorite.ConnectToConsole = chkConnectToConsole.Checked;
                favorite.DisableWallPaper = chkDisableWallpaper.Checked;
                favorite.DisableCursorBlinking = chkDisableCursorBlinking.Checked;
                favorite.DisableCursorShadow = chkDisableCursorShadow.Checked;
                favorite.DisableFullWindowDrag = chkDisableFullWindowDrag.Checked;
                favorite.DisableMenuAnimations = chkDisableMenuAnimations.Checked;
                favorite.DisableTheming = chkDisableThemes.Checked;


                favorite.RedirectDrives = chkDrives.Checked;
                favorite.RedirectPorts = chkSerialPorts.Checked;
                favorite.RedirectPrinters = chkPrinters.Checked;
                favorite.RedirectClipboard = chkRedirectClipboard.Checked;
                favorite.RedirectDevices = chkRedirectDevices.Checked;
                favorite.RedirectSmartCards = chkRedirectSmartcards.Checked;
                favorite.Sounds = (RemoteSounds)cmbSounds.SelectedIndex;
                showOnToolbar = chkAddtoToolbar.Checked;
                favorite.Port = ValidatePort(txtPort.Text);
                favorite.DesktopShare = txtDesktopShare.Text;
                favorite.ExecuteBeforeConnect = chkExecuteBeforeConnect.Checked;
                favorite.ExecuteBeforeConnectCommand = txtCommand.Text;
                favorite.ExecuteBeforeConnectArgs = txtArguments.Text;
                favorite.ExecuteBeforeConnectInitialDirectory = txtInitialDirectory.Text;
                favorite.ExecuteBeforeConnectWaitForExit = chkWaitForExit.Checked;
                favorite.ToolBarIcon = currentToolBarFileName;

                favorite.ICAApplicationName = ICAApplicationNameTextBox.Text;
                favorite.ICAApplicationPath = ICAApplicationPath.Text;
                favorite.ICAApplicationWorkingFolder = ICAWorkingFolder.Text;

                List<string> tagList = new List<string>();
                foreach(ListViewItem listViewItem in lvConnectionTags.Items) {
                    tagList.Add(listViewItem.Text);
                }
                favorite.Tags = String.Join(",", tagList.ToArray());


                //extended settings
                if(ShutdownTimeoutTextBox.Text.Trim() != "") {
                    favorite.ShutdownTimeout = Convert.ToInt32(ShutdownTimeoutTextBox.Text.Trim());
                }
                if(OverallTimeoutTextbox.Text.Trim() != "") {
                    favorite.OverallTimeout = Convert.ToInt32(OverallTimeoutTextbox.Text.Trim());
                }
                if(SingleTimeOutTextbox.Text.Trim() != "") {
                    favorite.ConnectionTimeout = Convert.ToInt32(SingleTimeOutTextbox.Text.Trim());
                }
                if(IdleTimeoutMinutesTextBox.Text.Trim() != "") {
                    favorite.IdleTimeout = Convert.ToInt32(IdleTimeoutMinutesTextBox.Text.Trim());
                }

                favorite.EnableSecuritySettings = SecuritySettingsEnabledCheckbox.Checked;
                if(SecuritySettingsEnabledCheckbox.Checked) {
                    favorite.SecurityWorkingFolder = SecurityWorkingFolderTextBox.Text;
                    favorite.SecurityStartProgram = SecuriytStartProgramTextbox.Text;
                    favorite.SecurityFullScreen = SecurityStartFullScreenCheckbox.Checked;
                }
                favorite.GrabFocusOnConnect = GrabFocusOnConnectCheckbox.Checked;
                favorite.EnableEncryption = EnableEncryptionCheckbox.Checked;
                favorite.DisableWindowsKey = DisableWindowsKeyCheckbox.Checked;
                favorite.DoubleClickDetect = DetectDoubleClicksCheckbox.Checked;
                favorite.DisplayConnectionBar = DisplayConnectionBarCheckbox.Checked;
                favorite.DisableControlAltDelete = DisableControlAltDeleteCheckbox.Checked;
                favorite.AcceleratorPassthrough = AcceleratorPassthroughCheckBox.Checked;
                favorite.EnableCompression = EnableCompressionCheckbox.Checked;
                favorite.BitmapPeristence = EnableBitmapPersistanceCheckbox.Checked;
                favorite.AllowBackgroundInput = AllowBackgroundInputCheckBox.Checked;

                favorite.DesktopSizeWidth = (int)this.widthUpDown.Value;
                favorite.DesktopSizeHeight = (int)this.heightUpDown.Value;

                favorite.Url = httpUrlTextBox.Text;


                favorite.IcaClientINI = ICAClientINI.Text;
                favorite.IcaServerINI = ICAServerINI.Text;
                favorite.IcaEncryptionLevel = ICAEncryptionLevelCombobox.Text;
                ICAEnableEncryptionCheckbox.Checked = ICAEncryptionLevelCombobox.Enabled;

                favorite.Notes = NotesTextbox.Text;

                Settings.AddFavorite(favorite, showOnToolbar);

                return true;
            } catch(Exception e) {
                Terminals.Logging.Log.Info("", e);
                MessageBox.Show(this, e.Message, "Terminals", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
        }

        private string ValidateServer(string serverName)
        {
            serverName = serverName.Trim();
            if(Settings.ForceComputerNamesAsURI)
            {
                if(Uri.CheckHostName(serverName) == UriHostNameType.Unknown)
                    throw new ArgumentException("Server name is not valid");
            }
            else
            {
                if(serverName == null) throw new ArgumentException("Server name was not specified.");
                if(serverName.Length < 0) throw new ArgumentException("Server name was not specified.");
            }
            return serverName;
        }

        private int ValidatePort(string port)
        {
            if(txtPort.Text.Trim() != "")
            {
                int result;
                if(int.TryParse(txtPort.Text, out result) && result < 65536 && result > 0)
                    return result;
                else
                    throw new ArgumentException("Port must be a number between 0 and 65535");
            }
            else
                return 3389;
        }

        private FavoriteConfigurationElement favorite;

        internal FavoriteConfigurationElement Favorite
        {
            get
            {
                return favorite;
            }
        }

        private bool showOnToolbar;

        internal bool ShowOnToolbar
        {
            get
            {
                return showOnToolbar;
            }
        }


        private void SetOkButtonState()
        {
            btnOk.Enabled = cmbServers.Text != String.Empty;
        }

        private void btnOk_Click(object sender, EventArgs e)
        {
            SaveMRUs();
            string name = txtName.Text;
            if(name == String.Empty)
            {
                name = cmbServers.Text;
            }
            favorite = new FavoriteConfigurationElement();
            favorite.Name = name;
            if(FillFavorite())
            {
                DialogResult = DialogResult.OK;
            }
        }

        private void control_TextChanged(object sender, EventArgs e)
        {
            SetOkButtonState();
        }

        private void chkSavePassword_CheckedChanged(object sender, EventArgs e)
        {
            txtPassword.ReadOnly = !chkSavePassword.Checked;
        }

        private void txtPassword_TextChanged(object sender, EventArgs e)
        {
            SetOkButtonState();
            //chkSavePassword.Checked = (txtPassword.Text != "");
        }

        private void NewTerminalForm_Shown(object sender, EventArgs e)
        {
            cmbServers.Focus();
        }

        private void cmbServers_TextChanged(object sender, EventArgs e)
        {
            SetOkButtonState();
        }

        private void GetServerAndPort(string Connection, out string Server, out int Port)
        {
            Server = Connection;
            Port = 3389;
            if(Connection != null && Connection.Trim() != "" && Connection.Contains(":"))
            {
                string server = Connection.Substring(0, Connection.IndexOf(":"));
                string rawPort = Connection.Substring(Connection.IndexOf(":") + 1);
                int port = 3389;
                if(rawPort != null && rawPort.Trim() != "")
                {
                    rawPort = rawPort.Trim();
                    int.TryParse(rawPort, out port);
                }
                Server = server;
                Port = port;
            }
        }

        private void cmbServers_Leave(object sender, EventArgs e)
        {
            if(txtName.Text == String.Empty)
            {
                if(cmbServers.Text.Contains(":"))
                {
                    string server = "";
                    int port = 3389;
                    GetServerAndPort(cmbServers.Text, out server, out port);
                    cmbServers.Text = server;
                    txtPort.Text = port.ToString();
                    cmbServers.Text = server;

                    
                }
                txtName.Text = cmbServers.Text;
            }
        }

        private void btnBrowseShare_Click(object sender, EventArgs e)
        {
            folderBrowserDialog.SelectedPath = @"\\" + cmbServers.Text;
            if(folderBrowserDialog.ShowDialog() == DialogResult.OK)
            {
                txtDesktopShare.Text = folderBrowserDialog.SelectedPath;
            }
        }

        private void AddTag()
        {
            if(!String.IsNullOrEmpty(txtTag.Text)) {
                ListViewItem[] items = lvConnectionTags.Items.Find(txtTag.Text, false);
                if(items.Length == 0) {
                    Settings.AddTag(txtTag.Text);
                    lvConnectionTags.Items.Add(txtTag.Text);
                }
            }
        }

        private void btnAddNewTag_Click(object sender, EventArgs e)
        {
            AddTag();
        }

        private void DeleteTag()
        {
            foreach(ListViewItem item in lvConnectionTags.SelectedItems)
            {
                lvConnectionTags.Items.Remove(item);
            }
        }

        private void btnRemoveTag_Click(object sender, EventArgs e)
        {
            DeleteTag();
        }

        private void ProtocolComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            string defaultPort = Connections.ConnectionManager.VNCVMRCPort.ToString();

            groupBox1.Enabled = false;
            chkConnectToConsole.Enabled = false;
            LocalResourceGroupBox.Enabled = false;
            VMRCReducedColorsCheckbox.Enabled = false;
            VMRCAdminModeCheckbox.Enabled = false;
            RASGroupBox.Enabled = false;

            ICAApplicationNameTextBox.Enabled = false;
            ICAApplicationPath.Enabled = false;
            httpUrlTextBox.Enabled = false;
            txtPort.Enabled = true;

            TelnetGroupBox.Enabled = false;
            if(ProtocolComboBox.Text == "RDP")
            {
                defaultPort = Connections.ConnectionManager.RDPPort.ToString();
                groupBox1.Enabled = true;
                chkConnectToConsole.Enabled = true;
                LocalResourceGroupBox.Enabled = true;
            }
            else if(ProtocolComboBox.Text == "VMRC")
            {
                VMRCReducedColorsCheckbox.Enabled = true;
                VMRCAdminModeCheckbox.Enabled = true;
            }
            else if(ProtocolComboBox.Text == "RAS")
            {
                this.cmbServers.Items.Clear();
                LoadDialupConnections();
                RASGroupBox.Enabled = true;
                txtPort.Enabled = false;
                RASDetailsListBox.Items.Clear();
            }
            else if(ProtocolComboBox.Text == "VNC")
            {
                //vnc settings
            }
            else if(ProtocolComboBox.Text == "Telnet")
            {
                TelnetGroupBox.Enabled = true;
                defaultPort = Connections.ConnectionManager.SSHPort.ToString();
            }
            else if(ProtocolComboBox.Text == "ICA Citrix")
            {
                ICAApplicationNameTextBox.Enabled = true;
                ICAApplicationPath.Enabled = true;
                defaultPort = Connections.ConnectionManager.ICAPort.ToString();
            }
            else if(ProtocolComboBox.Text == "HTTP")
            {
                httpUrlTextBox.Enabled = true;
                defaultPort = Connections.ConnectionManager.HTTPPort.ToString();
            }
            else if(ProtocolComboBox.Text == "HTTPS")
            {
                httpUrlTextBox.Enabled = true;
                defaultPort = Connections.ConnectionManager.HTTPSPort.ToString();
            }

            txtPort.Text = defaultPort;
        }

        private void TelnetRadioButton_CheckedChanged(object sender, EventArgs e)
        {
            txtPort.Text = Connections.ConnectionManager.TelnetPort.ToString();
        }

        private void SSHRadioButton_CheckedChanged(object sender, EventArgs e)
        {
            txtPort.Text = Connections.ConnectionManager.SSHPort.ToString();
        }

        private void TelnetFontButton_Click(object sender, EventArgs e)
        {            
            this.fontDialog1.Font = FontParser.ParseFontName(TelnetFontTextbox.Text);
            DialogResult result = this.fontDialog1.ShowDialog();
            if(result == DialogResult.OK)
            {
                this.TelnetFontTextbox.Text = this.fontDialog1.Font.ToString();
            }
        }

        private void BackcolorButton_Click(object sender, EventArgs e)
        {
            this.colorDialog1.Color = Color.FromName(this.BackColorTextBox.Text);
            DialogResult result = this.colorDialog1.ShowDialog();
            if(result == DialogResult.OK)
            {
                this.BackColorTextBox.Text = this.colorDialog1.Color.Name;
            }
        }

        private void TelnetTextColorButton_Click(object sender, EventArgs e)
        {
            this.colorDialog1.Color = Color.FromName(this.TelnetTextColorTextBox.Text);
            DialogResult result = this.colorDialog1.ShowDialog();
            if(result == DialogResult.OK)
            {
                this.TelnetTextColorTextBox.Text = this.colorDialog1.Color.Name;
            }
        }

        private void TelnetCursorColorButton_Click(object sender, EventArgs e)
        {
            this.colorDialog1.Color = Color.FromName(this.TelnetCursorColorTextBox.Text);
            DialogResult result = this.colorDialog1.ShowDialog();
            if(result == DialogResult.OK)
            {
                this.TelnetCursorColorTextBox.Text = this.colorDialog1.Color.Name;
            }
        }
        NetworkScanner ns = new NetworkScanner();
        private void DetectButton_Click(object sender, EventArgs e)
        {

            DialogResult result = ns.ShowDialog();
            if(result == DialogResult.OK)
            {
                LoadMRUs();
                if(ns.SelectedScanItem != null)
                {
                    Terminals.Scanner.NetworkScanItem item = ns.SelectedScanItem;
                    this.txtPort.Text = item.Port.ToString();
                    cmbServers.Text = item.IPAddress;
                    this.ProtocolComboBox.Text = Terminals.Connections.ConnectionManager.GetPortName(item.Port, item.IsVMRC);
                    if(item.Port == Terminals.Connections.ConnectionManager.SSHPort) this.SSHRadioButton.Checked = true;
                    if(item.Port == Terminals.Connections.ConnectionManager.TelnetPort) this.TelnetRadioButton.Checked = true;
                    this.txtName.Text = string.Format("{0}_{1}", item.HostName, this.ProtocolComboBox.Text);
                    if(this.ProtocolComboBox.Text == "RDP")
                    {
                        this.chkConnectToConsole.Checked = true;
                        this.cmbResolution.SelectedIndex = this.cmbResolution.Items.Count - 1;
                    }
                }
            }
        }

        private void ras1_ConnectionChanged(object sender, ConnectionChangedEventArgs e)
        {

        }

        private void cmbServers_SelectedIndexChanged(object sender, EventArgs e)
        {
            if(ProtocolComboBox.Text == "RAS")
            {
                LoadDialupConnections();
                RASDetailsListBox.Items.Clear();
                if(dialupList != null && dialupList.ContainsKey(cmbServers.Text))
                {
                    RASENTRY selectedRAS = dialupList[cmbServers.Text];
                    RASDetailsListBox.Items.Add(string.Format("{0}:{1}", "Connection", cmbServers.Text));
                    RASDetailsListBox.Items.Add(string.Format("{0}:{1}", "Area Code", selectedRAS.AreaCode));
                    RASDetailsListBox.Items.Add(string.Format("{0}:{1}", "Country Code", selectedRAS.CountryCode));
                    RASDetailsListBox.Items.Add(string.Format("{0}:{1}", "Device Name", selectedRAS.DeviceName));
                    RASDetailsListBox.Items.Add(string.Format("{0}:{1}", "Device Type", selectedRAS.DeviceType));
                    RASDetailsListBox.Items.Add(string.Format("{0}:{1}", "Local Phone Number", selectedRAS.LocalPhoneNumber));
                }
            }
        }

        private string currentToolBarFileName = "";
        System.Windows.Forms.OpenFileDialog fd = new OpenFileDialog();
        string appFolder = System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetEntryAssembly().Location);
        private void pictureBox2_Click(object sender, EventArgs e)
        {
            fd.CheckFileExists = true;
            fd.InitialDirectory = appFolder;
            fd.Filter = "PNG Files (*.png)|*.png|JPG Files (*.jpg)|*.jpg|All files (*.*)|*.*";
            fd.Multiselect = false;
            fd.Title = "Select Custom Terminal Image .";
            string thumbsFolder = System.IO.Path.Combine(appFolder, "Thumbs");
            if(!System.IO.Directory.Exists(thumbsFolder)) System.IO.Directory.CreateDirectory(thumbsFolder);

            currentToolBarFileName = "";
            if(fd.ShowDialog() == DialogResult.OK)
            {
                //make it relative to the current application executable
                string filename = fd.FileName;
                try
                {
                    Image i = Image.FromFile(filename);
                    if(i != null)
                    {                        
                        this.pictureBox2.Image = i;
                        string newFile = System.IO.Path.Combine(thumbsFolder, System.IO.Path.GetFileName(filename));
                        if(newFile!=filename && !System.IO.File.Exists(newFile)) System.IO.File.Copy(filename, newFile);
                        currentToolBarFileName = newFile;
                    }
                }
                catch(Exception ex)
                {
                    Terminals.Logging.Log.Info("", ex);
                    currentToolBarFileName = "";
                    System.Windows.Forms.MessageBox.Show("You have chosen an invalid image. Try again.");
                }

            }
        }

        private void SecuritySettingsEnabledCheckbox_CheckedChanged(object sender, EventArgs e)
        {
            panel2.Enabled = SecuritySettingsEnabledCheckbox.Checked;
        }


        private void tabControl1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if(tabControl1.SelectedTab == tabPage2)
            {
                RDPSubTabPage_SelectedIndexChanged(null, null);
            }
        }

        private void RDPSubTabPage_SelectedIndexChanged(object sender, EventArgs e)
        {
            if(RDPSubTabPage.SelectedTab == tabPage10)
            {
                terminalServerManager1.Connect(this.cmbServers.Text, true);
                terminalServerManager1.Invalidate();
            }
        }

        private void cmbResolution_SelectedIndexChanged(object sender, EventArgs e)
        {
            customSizePanel.Visible = false;
            if(cmbResolution.Text == "Custom") customSizePanel.Visible = true;            
        }

        private void AllTagsAddButton_Click(object sender, EventArgs e) {
            foreach(ListViewItem lv in AllTagsListView.SelectedItems) {
                ListViewItem[] items = lvConnectionTags.Items.Find(lv.Text, false);
                if(items.Length == 0) {
                    Settings.AddTag(lv.Text);
                    lvConnectionTags.Items.Add(lv.Text);
                }

            }
        }

        private void ICAEnableEncryptionCheckbox_CheckedChanged(object sender, EventArgs e) {
            ICAEncryptionLevelCombobox.Enabled = ICAEnableEncryptionCheckbox.Checked;
        }

    }
}