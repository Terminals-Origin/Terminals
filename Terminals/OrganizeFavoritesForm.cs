using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace Terminals
{
    public partial class OrganizeFavoritesForm : Form
    {
        public OrganizeFavoritesForm()
        {
            InitializeComponent();
            LoadConnections();
        }

        private void LoadConnections()
        {            
            lvConnections.BeginUpdate();
            try
            {
                lvConnections.Items.Clear();
                //FavoriteConfigurationElementCollection favorites = Settings.GetFavorites();
                SortedDictionary<string, FavoriteConfigurationElement> favorites = Settings.GetSortedFavorites(Settings.DefaultSortProperty);
                foreach (string key in favorites.Keys)
                {
                    FavoriteConfigurationElement favorite = favorites[key];
                    lvConnections.ShowItemToolTips = true;
                    ListViewItem item = lvConnections.Items.Add(favorite.Name);
                    item.ToolTipText = favorite.Notes;
                    
                    item.Name = favorite.Name;
                    item.SubItems.Add(favorite.Protocol);
                    item.SubItems.Add(favorite.ServerName);
                    item.SubItems.Add(favorite.Credential);
                    item.SubItems.Add(favorite.DomainName);
                    item.SubItems.Add(favorite.UserName);
                    item.SubItems.Add(favorite.Tags);
                    item.SubItems.Add(favorite.Notes);
                    item.Tag = favorite;
                }
            }
            finally
            {
                lvConnections.EndUpdate();
            }
            lvConnections.AutoResizeColumns(ColumnHeaderAutoResizeStyle.ColumnContent);
            if (lvConnections.Columns[0].Width < 50) lvConnections.Columns[0].Width = 50;
            if (lvConnections.Columns[1].Width < 50) lvConnections.Columns[1].Width = 50;
            if (lvConnections.Columns[2].Width < 50) lvConnections.Columns[2].Width = 50;
            if (lvConnections.Columns[3].Width < 50) lvConnections.Columns[3].Width = 50;
            if (lvConnections.Columns[4].Width < 50) lvConnections.Columns[4].Width = 50;
            if (lvConnections.Columns[5].Width < 50) lvConnections.Columns[5].Width = 50;
            if (lvConnections.Columns[6].Width < 50) lvConnections.Columns[6].Width = 50;
            if (lvConnections.Columns[7].Width < 50) lvConnections.Columns[7].Width = 50;
        }

        private void btnRename_Click(object sender, EventArgs e)
        {
            if (lvConnections.SelectedItems.Count > 0)
            {
                lvConnections.SelectedItems[0].BeginEdit();
            }
        }

        private void btnEdit_Click(object sender, EventArgs e)
        {
            FavoriteConfigurationElement favorite = GetSelectedFavorite();
            if (favorite != null)
            {
                EditFavorite(favorite);
            }
        }

        private void EditFavorite(FavoriteConfigurationElement favorite)
        {
            NewTerminalForm frmNewTerminal = new NewTerminalForm(favorite);
            string oldName = favorite.Name;
            if (frmNewTerminal.ShowDialog() == DialogResult.OK)
            {
                if (oldName != frmNewTerminal.favorite.Name) Settings.DeleteFavorite(oldName);
                LoadConnections();
            }
        }

        private void lvConnections_AfterLabelEdit(object sender, LabelEditEventArgs e)
        {
            ListViewItem item = lvConnections.Items[e.Item];
            if (!String.IsNullOrEmpty(e.Label) && e.Label != item.Text)
            {
                if (lvConnections.Items.ContainsKey(e.Label))
                {
                    e.CancelEdit = true;
                    MessageBox.Show(this, "A connection named " + e.Label + " already exists", "Terminals", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                FavoriteConfigurationElement favorite = (FavoriteConfigurationElement)item.Tag;
                string oldName = favorite.Name;
                favorite.Name = e.Label;
                item.Name = e.Label;
                //Settings.DeleteFavorite(favorite.Name);
                //Settings.AddFavorite(favorite);
                Settings.EditFavorite(oldName, favorite);
            }
        }

        private void ConnectionManager_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.F2)
            {
                btnRename.PerformClick();
            }
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {

            foreach(ListViewItem item in lvConnections.SelectedItems) {
                FavoriteConfigurationElement favorite = (item.Tag as FavoriteConfigurationElement);
                if(favorite != null) {
                    Settings.DeleteFavorite(favorite.Name);
                    Settings.DeleteFavoriteButton(favorite.Name);
                }
            }
            LoadConnections();
        }

        private FavoriteConfigurationElement GetSelectedFavorite()
        {
            if (lvConnections.SelectedItems.Count > 0)
                return (FavoriteConfigurationElement)lvConnections.SelectedItems[0].Tag;
            return null;
        }

        private void btnCopy_Click(object sender, EventArgs e)
        {
            FavoriteConfigurationElement favorite = GetSelectedFavorite();
            if (favorite != null)
            {
                InputBoxResult result = InputBox.Show("New Connection Name");
                if (result.ReturnCode == System.Windows.Forms.DialogResult.OK && !string.IsNullOrEmpty(result.Text))
                {
                    FavoriteConfigurationElement newFav = (favorite.Clone() as FavoriteConfigurationElement);
                    if (newFav != null)
                    {
                        newFav.Name = result.Text;
                        Settings.AddFavorite(newFav, Settings.HasToolbarButton(newFav.Name));
                        LoadConnections();
                    }
                }
            }
        }

        private string GetUniqeName(string favoriteName)
        {
            string result = "Copy of " + favoriteName;
            int i = 1;
            while (lvConnections.Items.ContainsKey(result))
            {
                i++;
                result = "Copy (" + i.ToString() + ") of " + favoriteName;
            }
            return result;
        }

        private void btnNew_Click(object sender, EventArgs e)
        {
            using (NewTerminalForm frmNewTerminal = new NewTerminalForm(String.Empty, false))
            {
                if (frmNewTerminal.ShowDialog() == DialogResult.OK)
                {
                    Settings.AddFavorite(frmNewTerminal.Favorite, frmNewTerminal.ShowOnToolbar);
                    LoadConnections();
                }
            }
        }

        private void OrganizeFavoritesForm_Shown(object sender, EventArgs e)
        {
            if (lvConnections.Items.Count > 0)
            {
                lvConnections.Items[0].Selected = true;
            }
        }

        private void OrganizeFavoritesForm_Activated(object sender, EventArgs e)
        {
            LoadConnections();
        }
        private void ImportFromFile() {
            bool needsReload = false;
            if(ImportOpenFileDialog.ShowDialog() == DialogResult.OK) {
                string filename = ImportOpenFileDialog.FileName;
                Integration.Integration i = new Terminals.Integration.Integration();

                FavoriteConfigurationElementCollection coll = i.ImportFavorites(filename);
                if(coll != null) {
                    needsReload = true;
                    foreach(FavoriteConfigurationElement fav in coll) {
                        Settings.AddFavorite(fav, false);
                    }
                }
            }
            if(needsReload) LoadConnections();
        }
 
        private void lvConnections_SelectedIndexChanged(object sender, EventArgs e)
        {

        }
        NetworkScanner ns = new NetworkScanner();


        private void activeDirectoryToolStripMenuItem_Click(object sender, EventArgs e) {
            Network.ImportFromAD ad = new Terminals.Network.ImportFromAD();
            ad.ShowDialog();
        }

        private void networkDetectionToolStripMenuItem_Click(object sender, EventArgs e) {

            DialogResult result = ns.ShowDialog();
            if(result == DialogResult.OK) {
                //LoadMRUs();
                //if(ns.SelectedScanItem != null)
                //{
                //    Terminals.Scanner.NetworkScanItem item = ns.SelectedScanItem;
                //    this.txtPort.Text = item.Port.ToString();
                //    cmbServers.Text = item.IPAddress;
                //    this.ProtocolComboBox.Text = Terminals.Connections.ConnectionManager.GetPortName(item.Port, item.IsVMRC);
                //    if(item.Port == Terminals.Connections.ConnectionManager.SSHPort) this.SSHRadioButton.Checked = true;
                //    if(item.Port == Terminals.Connections.ConnectionManager.TelnetPort) this.TelnetRadioButton.Checked = true;
                //    this.txtName.Text = string.Format("{0}_{1}", item.HostName, this.ProtocolComboBox.Text);
                //    if(this.ProtocolComboBox.Text == "RDP")
                //    {
                //        this.chkConnectToConsole.Checked = true;
                //        this.cmbResolution.SelectedIndex = this.cmbResolution.Items.Count - 1;
                //    }
                //}
            }
        }

        private void muRDToolStripMenuItem_Click(object sender, EventArgs e) {
            ImportFromFile();
        }

        private void rDPToolStripMenuItem_Click(object sender, EventArgs e) {
            ImportFromFile();
        }

        private void vRDBackupFileToolStripMenuItem_Click(object sender, EventArgs e) {
            ImportFromFile();
        }

        private void ImportButton_Click(object sender, EventArgs e) {
            MouseEventArgs mouse = (e as MouseEventArgs);
            if(mouse != null) {
                contextMenuStrip1.Show(ImportButton, new Point(mouse.X, mouse.Y));
            }
        }

    }
}