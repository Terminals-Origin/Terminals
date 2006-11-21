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
            chkAddtoToolbar.Checked = favorite.ShowOnToolbar;
            chkDrives.Checked = favorite.RedirectDrives;
            chkSerialPorts.Checked = favorite.RedirectPorts;
            chkPrinters.Checked = favorite.RedirectPrinters;
            cmbSounds.SelectedIndex = (int)favorite.Sounds;
            txtPort.Text = favorite.Port.ToString();
        }

        private void FillFavorite()
        {
            favorite.ServerName = cmbServers.Text;
            favorite.DomainName = cmbDomains.Text;
            favorite.UserName = cmbUsers.Text;
            favorite.Password = (chkSavePassword.Checked ? txtPassword.Text : "");
            favorite.DesktopSize = (DesktopSize)cmbResolution.SelectedIndex;
            favorite.Colors = (Colors)cmbColors.SelectedIndex;
            favorite.ConnectToConsole = chkConnectToConsole.Checked;
            favorite.ShowOnToolbar = chkAddtoToolbar.Checked;
            favorite.RedirectDrives = chkDrives.Checked;
            favorite.RedirectPorts = chkSerialPorts.Checked;
            favorite.RedirectPrinters = chkPrinters.Checked;
            favorite.Sounds = (RemoteSounds)cmbSounds.SelectedIndex;
            if (txtPort.Text.Trim() != "")
                favorite.Port = int.Parse(txtPort.Text);
            else
                favorite.Port = 3389;
        }

        private FavoriteConfigurationElement favorite;

        internal FavoriteConfigurationElement Favorite
        {
            get { return favorite; }
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
            FillFavorite();
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

        private void txtPort_Validating(object sender, CancelEventArgs e)
        {
            try
            {
                int.Parse(txtPort.Text);
            }
            catch
            {
                MessageBox.Show("Port must be an integer");
                txtPort.Text = "";
            }
        }
    }
}