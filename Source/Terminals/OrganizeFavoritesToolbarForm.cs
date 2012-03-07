using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using Terminals.Configuration;
using Terminals.Data;

namespace Terminals
{
    public partial class OrganizeFavoritesToolbarForm : Form
    {
        public OrganizeFavoritesToolbarForm()
        {
            InitializeComponent();

            var favoritesWithButton = Persistance.Instance.Favorites
                .Where(candidate => Settings.FavoritesToolbarButtons.Contains(candidate.Id));

            foreach (IFavorite favoriteWithButton in favoritesWithButton)
            {
                ListViewItem favoriteItem = new ListViewItem(favoriteWithButton.Name);
                favoriteItem.Tag = favoriteWithButton.Id;
                lvFavoriteButtons.Items.Add(favoriteItem);
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
            var favoriteIds = new List<Guid>();
            foreach (ListViewItem item in lvFavoriteButtons.Items)
            {
                favoriteIds.Add((Guid)item.Tag);
            }
            Settings.UpdateFavoritesToolbarButtons(favoriteIds);
        }
    }
}