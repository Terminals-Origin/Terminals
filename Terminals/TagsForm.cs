using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace Terminals
{
    public partial class TagsForm : Form
    {
        public TagsForm(MainForm mainForm)
        {
            InitializeComponent();
            this.mainForm = mainForm;
            LoadTags("");
        }

        private MainForm mainForm;

        private void LoadTags(string filter)
        {
            lvTags.Items.Clear();
            ListViewItem unTaggedListViewItem = new ListViewItem();
            unTaggedListViewItem.ImageIndex = 0;
            unTaggedListViewItem.StateImageIndex = 0;
            List<FavoriteConfigurationElement> unTaggedFavorites = new List<FavoriteConfigurationElement>();
            foreach (string tag in Settings.Tags)
            {
                if ((String.IsNullOrEmpty(filter) || (tag.StartsWith(filter))))
                {
                    ListViewItem item = new ListViewItem();
                    item.ImageIndex = 0;
                    item.StateImageIndex = 0;
                    FavoriteConfigurationElementCollection favorites = Settings.GetFavorites();
                    List<FavoriteConfigurationElement> tagFavorites = new List<FavoriteConfigurationElement>();
                    foreach (FavoriteConfigurationElement favorite in favorites)
                    {
                        if (favorite.TagList.IndexOf(tag) >= 0)
                        {
                            tagFavorites.Add(favorite);
                        }
                        else if (favorite.TagList.Count == 0)
                        {
                            if (unTaggedFavorites.IndexOf(favorite) < 0)
                            {
                                unTaggedFavorites.Add(favorite);
                            }
                        }
                    }
                    item.Tag = tagFavorites;
                    item.Text = tag + " (" + tagFavorites.Count.ToString() + ")";
                    lvTags.Items.Add(item);
                }
            }
            unTaggedListViewItem.Tag = unTaggedFavorites;
            unTaggedListViewItem.Text = "UnTagged (" + unTaggedFavorites.Count.ToString() + ")";
            lvTags.Items.Add(unTaggedListViewItem);
        }

        private void lvTags_SelectedIndexChanged(object sender, EventArgs e)
        {
            connectToolStripMenuItem.Enabled = lvTags.SelectedItems.Count > 0;
            lvTagConnections.Items.Clear();
            if (lvTags.SelectedItems.Count > 0)
            {
                List<FavoriteConfigurationElement> tagFavorites = (List<FavoriteConfigurationElement>)lvTags.SelectedItems[0].Tag;
                foreach (FavoriteConfigurationElement favorite in tagFavorites)
                {
                    ListViewItem item = lvTagConnections.Items.Add(favorite.Name);
                    item.ImageIndex = 0;
                    item.StateImageIndex = 0;
                    item.Tag = favorite;
                }
            }
        }

        private void lvTagConnections_SelectedIndexChanged(object sender, EventArgs e)
        {
            connectToolStripMenuItem.Enabled = lvTagConnections.SelectedItems.Count > 0;
        }

        private void connectToolStripMenuItem_Click(object sender, EventArgs e)
        {
            foreach (ListViewItem item in lvTagConnections.SelectedItems)
            {
                FavoriteConfigurationElement favorite = (FavoriteConfigurationElement)item.Tag;
                mainForm.Connect(favorite.Name);
            }
            Close();
        }

        private void connectToAllToolStripMenuItem_Click(object sender, EventArgs e)
        {
            foreach (ListViewItem item in lvTagConnections.Items)
            {
                FavoriteConfigurationElement favorite = (FavoriteConfigurationElement)item.Tag;
                mainForm.Connect(favorite.Name);
            }
            Close();
        }

        private void TagsForm_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape)
            {
                Close();
            }
        }

        private void TagsForm_Activated(object sender, EventArgs e)
        {
            this.Opacity = 1;
        }

        private void TagsForm_Deactivate(object sender, EventArgs e)
        {
            this.Opacity = 0.5;
        }

        private void tscbSearchTag_TextChanged(object sender, EventArgs e)
        {
            LoadTags(tscbSearchTag.Text);
        }

    }
}