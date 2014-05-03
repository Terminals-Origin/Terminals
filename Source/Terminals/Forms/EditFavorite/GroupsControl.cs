using System;
using System.Linq;
using System.Windows.Forms;
using Terminals.Data;
using Terminals.Forms.Controls;

namespace Terminals.Forms.EditFavorite
{
    public partial class GroupsControl : UserControl
    {
        public GroupsControl()
        {
            InitializeComponent();
        }

        private void BtnAddNewTag_Click(object sender, EventArgs e)
        {
            this.AddGroup();
        }

        private void BtnRemoveTag_Click(object sender, EventArgs e)
        {
            this.DeleteGroup();
        }

        private void LvConnectionTags_DoubleClick(object sender, EventArgs e)
        {
            this.DeleteGroup();
        }

        private void AllTagsAddButton_Click(object sender, EventArgs e)
        {
            this.AddGroupsToFavorite();
        }

        private void AllTagsListView_DoubleClick(object sender, EventArgs e)
        {
            this.AddGroupsToFavorite();
        }

        private void AddGroup()
        {
            //string newGroupName = this.txtGroupName.Text;
            //if (!this.validator.ValidateGroupName(this.txtGroupName))
            //    return;
            //// not unique name already handled by validation
            //IGroup candidate = this.persistence.Factory.CreateGroup(newGroupName);
            //this.AddGroupIfNotAlreadyThere(candidate);
        }

        private void AddGroupsToFavorite()
        {
            foreach (GroupListViewItem groupItem in this.AllTagsListView.SelectedItems)
            {
                this.AddGroupIfNotAlreadyThere(groupItem.FavoritesGroup);
            }
        }

        private void AddGroupIfNotAlreadyThere(IGroup selectedGroup)
        {
            // this also prevents duplicities in newly created groups not stored in persistence yet
            bool containsName = SelectedGroupsContainGroupName(selectedGroup);
            if (!containsName)
            {
                var selectedGroupItem = new GroupListViewItem(selectedGroup);
                this.lvConnectionTags.Items.Add(selectedGroupItem);
            }
        }

        private bool SelectedGroupsContainGroupName(IGroup selectedGroup)
        {
            return this.lvConnectionTags.Items.Cast<ListViewItem>()
                .Any(candidate => candidate.Text == selectedGroup.Name);
        }

        private void DeleteGroup()
        {
            foreach (ListViewItem groupItem in this.lvConnectionTags.SelectedItems)
            {
                this.lvConnectionTags.Items.Remove(groupItem);
            }
        }
    }
}
