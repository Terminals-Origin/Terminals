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
        public NewTerminalForm(string server, bool connect)
        {
            InitializeComponent();
            LoadMRUs();
            cmbServers.Text = server;
            cmbResolution.SelectedIndex = 3;
            cmbColors.SelectedIndex = 1;
            cmbSounds.SelectedIndex = 2;
            SetOkButtonState();
            SetOkTitle(connect);
            this.ProtocolComboBox.SelectedIndex = 0;
        }
        private void NewTerminalForm_Load(object sender, EventArgs e)
        {
            //LoadDialupConnections();

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
            chkAllowDesktopBG.Checked = favorite.ShowDesktopBackground;
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
        }

        private bool FillFavorite()
        {
            try
            {
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
                favorite.ShowDesktopBackground = chkAllowDesktopBG.Checked;
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

                List<string> tagList = new List<string>();
                foreach(ListViewItem listViewItem in lvConnectionTags.Items)
                {
                    tagList.Add(listViewItem.Text);
                }
                favorite.Tags = String.Join(",", tagList.ToArray());
                return true;
            }
            catch(Exception e)
            {
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
            chkSavePassword.Checked = (txtPassword.Text != "");
        }

        private void NewTerminalForm_Shown(object sender, EventArgs e)
        {
            cmbServers.Focus();
        }

        private void cmbServers_TextChanged(object sender, EventArgs e)
        {
            SetOkButtonState();
        }

        private void cmbServers_Leave(object sender, EventArgs e)
        {
            if(txtName.Text == String.Empty)
            {
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
            ListViewItem[] items = lvConnectionTags.Items.Find(txtTag.Text, false);
            if(items.Length == 0)
            {
                Settings.AddTag(txtTag.Text);
                lvConnectionTags.Items.Add(txtTag.Text);
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
            DialogResult result = this.fontDialog1.ShowDialog();
            if(result == DialogResult.OK)
            {
                this.TelnetFontTextbox.Text = this.fontDialog1.Font.ToString();
            }
        }

        private void BackcolorButton_Click(object sender, EventArgs e)
        {
            DialogResult result = this.colorDialog1.ShowDialog();
            if(result == DialogResult.OK)
            {
                this.BackColorTextBox.Text = this.colorDialog1.Color.Name;
            }
        }

        private void TelnetTextColorButton_Click(object sender, EventArgs e)
        {
            DialogResult result = this.colorDialog1.ShowDialog();
            if(result == DialogResult.OK)
            {
                this.TelnetTextColorTextBox.Text = this.colorDialog1.Color.Name;
            }
        }

        private void TelnetCursorColorButton_Click(object sender, EventArgs e)
        {
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
                        System.IO.File.Copy(filename, newFile);
                        currentToolBarFileName = newFile;
                    }
                }
                catch(Exception)
                {
                    currentToolBarFileName = "";
                    System.Windows.Forms.MessageBox.Show("You have chosen an invalid image. Try again.");
                }

            }
        }

    }
}