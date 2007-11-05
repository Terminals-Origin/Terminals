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
                FavoriteConfigurationElementCollection favorites = Settings.GetFavorites();
                foreach (FavoriteConfigurationElement favorite in favorites)
                {
                    ListViewItem item = lvConnections.Items.Add(favorite.Name);
                    item.Name = favorite.Name;
                    item.SubItems.Add(favorite.Protocol);
                    item.SubItems.Add(favorite.ServerName);
                    item.SubItems.Add(favorite.DomainName);
                    item.SubItems.Add(favorite.UserName);
                    item.Tag = favorite;
                }
            }
            finally
            {
                lvConnections.EndUpdate();
            }
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
                Settings.DeleteFavorite(oldName);
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
            FavoriteConfigurationElement favorite = GetSelectedFavorite();
            if (favorite != null)
            {
                Settings.DeleteFavorite(favorite.Name);
                Settings.DeleteFavoriteButton(favorite.Name);
                LoadConnections();
            }
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
                string oldName = favorite.Name;
                favorite.Name = GetUniqeName(favorite.Name);
                Settings.DeleteFavorite(favorite.Name);
                Settings.AddFavorite(favorite, Settings.HasToolbarButton(oldName));
                LoadConnections();
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

        private void ImportButton_Click(object sender, EventArgs e)
        {
            bool needsReload = false;
            if(ImportOpenFileDialog.ShowDialog() == DialogResult.OK)
            {
                string filename = ImportOpenFileDialog.FileName;
                Integration.Integration i = new Terminals.Integration.Integration();
                
                FavoriteConfigurationElementCollection coll = i.ImportFavorites(filename);
                if(coll != null)
                {
                    needsReload = true;
                    foreach(FavoriteConfigurationElement fav in coll)
                    {
                        Settings.AddFavorite(fav, false);
                    }
                }
            }
            if(needsReload) LoadConnections();
        }
    }
}