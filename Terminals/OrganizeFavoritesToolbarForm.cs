using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace Terminals
{
    public partial class OrganizeFavoritesToolbarForm : Form
    {
        public OrganizeFavoritesToolbarForm()
        {
            InitializeComponent();
            foreach (string favoriteButton in Settings.FavoritesToolbarButtons)
            {
                lvFavoriteButtons.Items.Add(favoriteButton);
            }
        }

        private void lvFavoriteButtons_SelectedIndexChanged(object sender, EventArgs e)
        {
            ListViewItem selectedItem = null;
            if (lvFavoriteButtons.SelectedItems.Count == 1)
            {
                selectedItem = lvFavoriteButtons.SelectedItems[0];
            }
            bool isSelected = (selectedItem != null);
            tsbMoveToFirst.Enabled = (isSelected && selectedItem.Index > 0);
            tsbMoveUp.Enabled = (isSelected && selectedItem.Index > 0);
            tsbMoveDown.Enabled = (isSelected && selectedItem.Index < lvFavoriteButtons.Items.Count - 1);
            tsbMoveToLast.Enabled = (isSelected && selectedItem.Index < lvFavoriteButtons.Items.Count - 1);
        }

        private void tsbMoveToFirst_Click(object sender, EventArgs e)
        {
            string name = lvFavoriteButtons.SelectedItems[0].Text;
            lvFavoriteButtons.SelectedItems[0].Remove();
            ListViewItem listViewItem = lvFavoriteButtons.Items.Insert(0, name);
            listViewItem.Selected = true;
        }

        private void tsbMoveUp_Click(object sender, EventArgs e)
        {
            string name = lvFavoriteButtons.SelectedItems[0].Text;
            int index = lvFavoriteButtons.SelectedItems[0].Index;
            lvFavoriteButtons.SelectedItems[0].Remove();
            ListViewItem listViewItem = lvFavoriteButtons.Items.Insert(index - 1, name);
            listViewItem.Selected = true;
        }

        private void tsbMoveDown_Click(object sender, EventArgs e)
        {
            string name = lvFavoriteButtons.SelectedItems[0].Text;
            int index = lvFavoriteButtons.SelectedItems[0].Index;
            lvFavoriteButtons.SelectedItems[0].Remove();
            ListViewItem listViewItem = lvFavoriteButtons.Items.Insert(index + 1, name);
            listViewItem.Selected = true;
        }

        private void tsbMoveToLast_Click(object sender, EventArgs e)
        {
            string name = lvFavoriteButtons.SelectedItems[0].Text;
            lvFavoriteButtons.SelectedItems[0].Remove();
            ListViewItem listViewItem = lvFavoriteButtons.Items.Add(name);
            listViewItem.Selected = true;
        }

        private void btnOk_Click(object sender, EventArgs e)
        {
            List<string> names = new List<string>();
            foreach (ListViewItem item in lvFavoriteButtons.Items)
            {
                names.Add(item.Text);
            }
            Settings.CreateFavoritesToolbarButtonsList(names.ToArray());
        }
    }
}