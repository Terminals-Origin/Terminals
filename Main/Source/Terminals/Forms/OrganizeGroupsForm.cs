using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using Terminals.Data;

namespace Terminals
{
    internal partial class OrganizeGroupsForm : Form
    {
        private readonly IPersistence persistence;

        private readonly FavoriteIcons favoriteIcons;

        internal OrganizeGroupsForm(IPersistence persistence, FavoriteIcons favoriteIcons)
        {
            this.persistence = persistence;
            this.favoriteIcons = favoriteIcons;

            InitializeComponent();

            this.gridGroups.AutoGenerateColumns = false;
            this.gridGroupFavorites.AutoGenerateColumns = false;
            LoadGroups();
        }

        private void LoadGroups()
        {
            // because the Name property is on the INamedItem level, we have to bound directly the item with the name property
            this.gridGroups.DataSource = this.persistence.Groups.ToList();
        }

        private IGroup GetSelectedGroup()
        {
            if (gridGroups.SelectedRows.Count > 0)
                return gridGroups.SelectedRows[0].DataBoundItem as IGroup;
            return null;
        }

        private void LoadSelectedGroupFavorites()
        {
            IGroup selectedGroup = GetSelectedGroup();
            if (selectedGroup != null)
            {
                this.gridGroupFavorites.DataSource = selectedGroup.Favorites.ToList();
            }
        }

        private void GridGroups_SelectedRowChanged(object sender, EventArgs e)
        {
            LoadSelectedGroupFavorites();
        }

        private void TsbAddGroup_Click(object sender, EventArgs e)
        {
            string newGroupName = NewGroupForm.AskFroGroupName(this.persistence);
            if (string.IsNullOrEmpty(newGroupName))
                return;
            
            FavoritesFactory.GetOrAddNewGroup(this.persistence, newGroupName);   
            this.LoadGroups();
        }

        private void TsbDeleteGroup_Click(object sender, EventArgs e)
        {
            IGroup group = GetSelectedGroup();
            if (group != null && OrganizeFavoritesForm.AskIfRealyDelete("group"))
            {
                this.persistence.Groups.Delete(group);
                LoadGroups();
            }
        }

        private void TsbAddConnection_Click(object sender, EventArgs e)
        {
            IGroup group = GetSelectedGroup();
            if (group != null)
                this.AddFavoritesToGroup(group);
        }

        private void AddFavoritesToGroup(IGroup group)
        {
            using (var frmAddConnection = new AddConnectionForm(this.persistence, this.favoriteIcons))
            {
                if (frmAddConnection.ShowDialog() == DialogResult.OK)
                {
                    group.AddFavorites(frmAddConnection.SelectedFavorites);
                    this.LoadSelectedGroupFavorites();
                    this.persistence.SaveAndFinishDelayedUpdate();
                }
            }
        }

        private void TsbDeleteConnection_Click(object sender, EventArgs e)
        {
            IGroup group = GetSelectedGroup();
            if (group != null)
            {
                if (this.gridGroupFavorites.SelectedRows.Count > 0)
                {
                    List<IFavorite> selectedFavorites = GetSelectedFavorites();
                    group.RemoveFavorites(selectedFavorites);
                    LoadSelectedGroupFavorites();
                    this.persistence.SaveAndFinishDelayedUpdate();
                }
            }
        }

        private List<IFavorite> GetSelectedFavorites()
        {
            var selectedRows = this.gridGroupFavorites.SelectedRows.Cast<DataGridViewRow>();
            return selectedRows.Select(row => row.DataBoundItem as IFavorite)
                .ToList();
        }
    }
}