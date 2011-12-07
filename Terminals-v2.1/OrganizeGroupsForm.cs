using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Terminals.Configuration;

namespace Terminals
{
    public partial class OrganizeGroupsForm : Form
    {
        internal OrganizeGroupsForm()
        {
            InitializeComponent();
            LoadGroups();
        }

        private void LoadGroups()
        {
            lvGroups.BeginUpdate();
            try
            {
                lvGroups.Items.Clear();
                GroupConfigurationElementCollection groups = Settings.GetGroups();
                foreach (GroupConfigurationElement group in groups)
                {
                    ListViewItem item = lvGroups.Items.Add(group.Name);
                    item.Name = group.Name;
                    item.Tag = group;
                }
                if (lvGroups.Items.Count > 0)
                {
                    lvGroups.Items[0].Focused = true;
                    lvGroups.Items[0].Selected = true;
                }
            }
            finally
            {
                lvGroups.EndUpdate();
            }
        }

        private void LoadConnections(GroupConfigurationElement group)
        {
            lvConnections.BeginUpdate();
            try
            {
                lvConnections.Items.Clear();
                foreach (FavoriteAliasConfigurationElement favorite in group.FavoriteAliases)
                {
                    ListViewItem item = lvConnections.Items.Add(favorite.Name);
                    item.Name = favorite.Name;
                }
                if (lvConnections.Items.Count > 0)
                {
                    lvConnections.Items[0].Focused = true;
                    lvConnections.Items[0].Selected = true;
                }
            }
            finally
            {
                lvConnections.EndUpdate();
            }
        }

        private GroupConfigurationElement GetSelectedGroup()
        {
            if (lvGroups.SelectedItems.Count > 0)
                return (GroupConfigurationElement)lvGroups.SelectedItems[0].Tag;
            return null;
        }

        private void tsbDeleteGroup_Click(object sender, EventArgs e)
        {
            GroupConfigurationElement group = GetSelectedGroup();
            if (group != null)
            {
                Settings.DeleteGroup(group.Name);
                LoadGroups();
            }
        }

        private void lvGroups_SelectedIndexChanged(object sender, EventArgs e)
        {
            GroupConfigurationElement group = GetSelectedGroup();
            if (group != null)
            {
                LoadConnections(group);
            }
        }

        private void tsbAddConnection_Click(object sender, EventArgs e)
        {
            GroupConfigurationElement group = GetSelectedGroup();
            if (group != null)
            {
                AddConnectionForm frmAddConnection = new AddConnectionForm();
                if (frmAddConnection.ShowDialog() == DialogResult.OK)
                {
                    foreach (string connection in frmAddConnection.Connections)
                    {
                        group.FavoriteAliases.Add(new FavoriteAliasConfigurationElement(connection));
                        lvConnections.Items.Add(connection);
                    }
                    Settings.DeleteGroup(group.Name);
                    Settings.AddGroup(group);
                }
            }
        }

        private void tsbDeleteConnection_Click(object sender, EventArgs e)
        {
            GroupConfigurationElement group = GetSelectedGroup();
            if (group != null)
            {
                if (lvConnections.SelectedItems.Count > 0)
                {
                    foreach (ListViewItem item in lvConnections.SelectedItems)
                    {
                        group.FavoriteAliases.Remove(item.Text);
                        Settings.DeleteGroup(group.Name);
                        Settings.AddGroup(group);
                        item.Remove();
                    }
                }
            }
        }

        private void tsbAddGroup_Click(object sender, EventArgs e)
        {
            using (NewGroupForm frmNewGroup = new NewGroupForm())
            {
                if (frmNewGroup.ShowDialog() == DialogResult.OK)
                {
                    GroupConfigurationElement serversGroup = new GroupConfigurationElement();
                    serversGroup.Name = frmNewGroup.txtGroupName.Text;
                    serversGroup.FavoriteAliases = new FavoriteAliasConfigurationElementCollection();
                    Settings.AddGroup(serversGroup);
                    LoadGroups();
                }
            }
        }
    }
}