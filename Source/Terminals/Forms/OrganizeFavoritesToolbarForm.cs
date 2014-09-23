using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using Terminals.Configuration;
using Terminals.Data;

namespace Terminals
{
    internal partial class OrganizeFavoritesToolbarForm : Form
    {
        private readonly Settings settings = Settings.Instance;

        private ListViewItem SelectedItem
        {
            get
            {
                return this.lvFavoriteButtons.SelectedItems[0];
            }
        }

        public OrganizeFavoritesToolbarForm(IPersistence persistence)
        {
            InitializeComponent();

            IFavorites persistedFavorites = persistence.Favorites;
            ListViewItem[] listViewItems = settings.FavoritesToolbarButtons
                .Select(id => persistedFavorites[id])
                .Where(candidate => candidate != null)
                .Select(favorite => new ListViewItem(favorite.Name) { Tag = favorite.Id })
                .ToArray();

            lvFavoriteButtons.Items.AddRange(listViewItems);
        }

        private void LvFavoriteButtons_SelectedIndexChanged(object sender, EventArgs e)
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

        private void TsbMoveUp_Click(object sender, EventArgs e)
        {
            this.MoveToIndex(this.SelectedItem.Index - 1);
        }

        private void TsbMoveDown_Click(object sender, EventArgs e)
        {
            this.MoveToIndex(this.SelectedItem.Index + 1);
        }

        private void TsbMoveToFirst_Click(object sender, EventArgs e)
        {
          this.MoveToIndex(0);
        }

        private void TsbMoveToLast_Click(object sender, EventArgs e)
        {
            Action<ListViewItem, int> insertAction = (item, index) => this.lvFavoriteButtons.Items.Add(item);
            this.MoveToIndex(insertAction); // index is ignored
        }

        private void MoveToIndex(int newIndex)
        {
            Action<ListViewItem, int> insertAction = (item, index) => this.lvFavoriteButtons.Items.Insert(index, item);
            this.MoveToIndex(insertAction, newIndex);
        }

        private void MoveToIndex(Action<ListViewItem, int> insertAction, int newIndex = 0)
        {
            ListViewItem selectedItem = this.SelectedItem;
            this.lvFavoriteButtons.Items.Remove(selectedItem);
            insertAction(selectedItem, newIndex);
            selectedItem.Selected = true;
        }

        private void BtnOk_Click(object sender, EventArgs e)
        {
            List<Guid> favoriteIds = lvFavoriteButtons.Items.Cast<ListViewItem>()
                .Select(item => item.Tag)
                .OfType<Guid>()
                .ToList();
            settings.UpdateFavoritesToolbarButtons(favoriteIds);
        }
    }
}