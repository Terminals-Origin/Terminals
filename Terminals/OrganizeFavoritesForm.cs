using System;
using System.Collections.Generic;
using System.Windows.Forms;
using Terminals.Configuration;
using Terminals.Forms;
using Terminals.Integration.Import;

namespace Terminals
{
    public partial class OrganizeFavoritesForm : Form
    {
        private ListViewColumnSorter _lvwColumnSorter;
        private NetworkScanner _networkScanner = new NetworkScanner();
        
        public OrganizeFavoritesForm()
        {
            InitializeComponent();
            LoadConnections();
            _lvwColumnSorter = new ListViewColumnSorter();
            lvConnections.ListViewItemSorter = _lvwColumnSorter;
            ImportOpenFileDialog.Filter = Importers.GetImportersDialogFilter();
        }

        private void LoadConnections()
        {            
            lvConnections.BeginUpdate();
            try
            {
                lvConnections.Items.Clear();
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

        private void EditFavorite(FavoriteConfigurationElement favorite)
        {
            NewTerminalForm frmNewTerminal = new NewTerminalForm(favorite);
            string oldName = favorite.Name;
            if (frmNewTerminal.ShowDialog() == DialogResult.OK)
            {
                if (oldName != frmNewTerminal.Favorite.Name) 
                    Settings.DeleteFavorite(oldName);
                LoadConnections();
            }
        }

        private FavoriteConfigurationElement GetSelectedFavorite()
        {
            if (lvConnections.SelectedItems.Count > 0)
                return (FavoriteConfigurationElement)lvConnections.SelectedItems[0].Tag;
            return null;
        }

        private void lvConnections_AfterLabelEdit(object sender, LabelEditEventArgs e)
        {
            ListViewItem item = lvConnections.Items[e.Item];
            if (!String.IsNullOrEmpty(e.Label) && e.Label != item.Text)
            {
                if (lvConnections.Items.ContainsKey(e.Label) && e.Label.ToLower() != item.Text.ToLower())
                {
                    e.CancelEdit = true;
                    MessageBox.Show(this, "A connection named " + e.Label + " already exists", "Terminals", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                FavoriteConfigurationElement favorite = (FavoriteConfigurationElement)item.Tag;
                string oldName = favorite.Name;
                favorite.Name = e.Label;
                item.Name = e.Label;
                Settings.EditFavorite(oldName, favorite);
            }
        }

        private void ConnectionManager_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.F2)
                btnRename.PerformClick();
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            foreach (ListViewItem item in lvConnections.SelectedItems)
            {
                FavoriteConfigurationElement favorite = (item.Tag as FavoriteConfigurationElement);
                if (favorite != null)
                {
                    Settings.DeleteFavorite(favorite.Name);
                    Settings.DeleteFavoriteButton(favorite.Name);
                }
            }
            LoadConnections();
        }        

        private void btnRename_Click(object sender, EventArgs e)
        {
            if (lvConnections.SelectedItems.Count > 0)
                lvConnections.SelectedItems[0].BeginEdit();
        }

        private void btnEdit_Click(object sender, EventArgs e)
        {
            FavoriteConfigurationElement favorite = GetSelectedFavorite();
            if (favorite != null)
                EditFavorite(favorite);
        }

        private void btnCopy_Click(object sender, EventArgs e)
        {
            FavoriteConfigurationElement favorite = GetSelectedFavorite();
            if (favorite != null)
            {
                InputBoxResult result = InputBox.Show("New Connection Name");
                if (result.ReturnCode == DialogResult.OK && !string.IsNullOrEmpty(result.Text))
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
                lvConnections.Items[0].Selected = true;
        }

        private void OrganizeFavoritesForm_Activated(object sender, EventArgs e)
        {
            LoadConnections();
        }
       
        private void activeDirectoryToolStripMenuItem_Click(object sender, EventArgs e) 
        {
            Network.ImportFromAD ad = new Network.ImportFromAD();
            ad.ShowDialog();
        }

        private void networkDetectionToolStripMenuItem_Click(object sender, EventArgs e) 
        {
            _networkScanner.ShowDialog();
        }

        private void ImportButton_Click(object sender, EventArgs e)
        {
            CallImport();
        }

        private void lvConnections_ColumnClick(object sender, ColumnClickEventArgs e)
        {
            // Determine if clicked column is already the column that is being sorted.
            if (e.Column == _lvwColumnSorter.SortColumn)
            {
                // Reverse the current sort direction for this column.
                if (_lvwColumnSorter.Order == SortOrder.Ascending)
                    _lvwColumnSorter.Order = SortOrder.Descending;
                else
                    _lvwColumnSorter.Order = SortOrder.Ascending;
            }
            else
            {
                // Set the column number that is to be sorted; default to ascending.
                _lvwColumnSorter.SortColumn = e.Column;
                _lvwColumnSorter.Order = SortOrder.Ascending;
            }

            // Perform the sort with these new sort options.
            lvConnections.Sort();
        }

        private void btnExport_Click(object sender, EventArgs e)
        {
            ExportFrom ei = new ExportFrom();
            ei.Show();
        }

        internal void CallImport()
        {
            bool needsReload = false;
            if (ImportOpenFileDialog.ShowDialog() == DialogResult.OK)
            {
                string filename = ImportOpenFileDialog.FileName;

                FavoriteConfigurationElementCollection coll = Importers.ImportFavorites(filename);
                if (coll != null)
                {
                    needsReload = true;
                    foreach (FavoriteConfigurationElement fav in coll)
                    {
                        Settings.AddFavorite(fav, false);
                    }
                }
            }

            if (needsReload)
                LoadConnections();
        }
    }
}