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
        }

        private void SetOkTitle(bool connect)
        {
            if (connect)
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
            if (!((tagsArray.Length == 1) && (String.IsNullOrEmpty(tagsArray[0]))))
            {
                foreach (string tag in tagsArray)
                {
                    lvConnectionTags.Items.Add(tag, tag, -1);
                }
            }
        }

        private bool FillFavorite()
        {
            try
            {
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
                List<string> tagList = new List<string>();
                foreach (ListViewItem listViewItem in lvConnectionTags.Items)
                {
                    tagList.Add(listViewItem.Text);
                }
                favorite.Tags = String.Join(",", tagList.ToArray());
                return true;
            }
            catch (Exception e)
            {
                MessageBox.Show(this, e.Message, "Terminals", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
        }

        private string ValidateServer(string serverName)
        {
            if (Uri.CheckHostName(serverName) == UriHostNameType.Unknown)
                throw new ArgumentException("Server name is not valid");
            return serverName;
        }

        private int ValidatePort(string port)
        {
            if (txtPort.Text.Trim() != "")
            {
                int result;
                if (int.TryParse(txtPort.Text, out result) && result < 65536 && result > 0)
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
            get { return favorite; }
        }

        private bool showOnToolbar;

        internal bool ShowOnToolbar
        {
            get { return showOnToolbar; }
        }


        private void SetOkButtonState()
        {
            btnOk.Enabled = cmbServers.Text != String.Empty;
        }

        private void btnOk_Click(object sender, EventArgs e)
        {
            SaveMRUs();
            string name = txtName.Text;
            if (name == String.Empty)
            {
                name = cmbServers.Text;
            }
            favorite = new FavoriteConfigurationElement();
            favorite.Name = name;
            if (FillFavorite())
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
            if (txtName.Text == String.Empty)
            {
                txtName.Text = cmbServers.Text;
            }
        }

        private void btnBrowseShare_Click(object sender, EventArgs e)
        {
            folderBrowserDialog.SelectedPath = @"\\" + cmbServers.Text;
            if (folderBrowserDialog.ShowDialog() == DialogResult.OK)
            {
                txtDesktopShare.Text = folderBrowserDialog.SelectedPath;
            }
        }

        private void AddTag()
        {
            ListViewItem[] items = lvConnectionTags.Items.Find(txtTag.Text, false);
            if (items.Length == 0)
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
            foreach (ListViewItem item in lvConnectionTags.SelectedItems)
            {
                lvConnectionTags.Items.Remove(item);
            }
        }

        private void btnRemoveTag_Click(object sender, EventArgs e)
        {
            DeleteTag();
        }
    }
}
