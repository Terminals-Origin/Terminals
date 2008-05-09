using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;

namespace Terminals
{
    public partial class FavsList : UserControl
    {
        public FavsList()
        {
            InitializeComponent();
        }

        private void FavsList_Load(object sender, EventArgs e)
        {
            FavsTree.NodeMouseClick += new TreeNodeMouseClickEventHandler(FavsTree_NodeMouseClick);
            LoadFavs();
        }
        public string UntaggedKey = "Untagged";
        public void LoadFavs()
        {

            FavsTree.Nodes.Clear();

            SortedDictionary<string, FavoriteConfigurationElement> favorites = Settings.GetSortedFavorites(Settings.DefaultSortProperty);
            SortedDictionary<string, TreeNode> SortedTags = new SortedDictionary<string, TreeNode>();
            SortedTags.Add(UntaggedKey, new TreeNode(UntaggedKey));
            FavsTree.Nodes.Add(SortedTags[UntaggedKey]);
            if(favorites != null) {
                foreach(string key in favorites.Keys) {

                    FavoriteConfigurationElement fav = favorites[key];
                    if(fav.TagList.Count > 0) {
                        foreach(string tag in fav.TagList) {
                            TreeNode FavNode = new TreeNode(fav.Name);
                            FavNode.Tag = fav;
                            if(!SortedTags.ContainsKey(tag)) {
                                TreeNode tagNode = new TreeNode(tag);
                                FavsTree.Nodes.Add(tagNode);
                                SortedTags.Add(tag, tagNode);
                            }
                            if(!SortedTags[tag].Nodes.Contains(FavNode)) SortedTags[tag].Nodes.Add(FavNode);
                        }
                    } else {
                        TreeNode FavNode = new TreeNode(fav.Name);
                        FavNode.Tag = fav;

                        if(!SortedTags[UntaggedKey].Nodes.Contains(FavNode)) SortedTags[UntaggedKey].Nodes.Add(FavNode);
                    }
                }
            }
        }

        void FavsTree_NodeMouseClick(object sender, TreeNodeMouseClickEventArgs e) {
            FavoriteConfigurationElement fav = (FavsTree.SelectedNode.Tag as FavoriteConfigurationElement);
            if(e.Button == MouseButtons.Right) {
                FavsTree.SelectedNode = e.Node;
            }
            pingToolStripMenuItem.Visible = true;
            dNSToolStripMenuItem.Visible = true;
            traceRouteToolStripMenuItem.Visible = true;
            tSAdminToolStripMenuItem.Visible = true;
            if(fav == null) {
                pingToolStripMenuItem.Visible = false;
                dNSToolStripMenuItem.Visible = false;
                traceRouteToolStripMenuItem.Visible = false;
                tSAdminToolStripMenuItem.Visible = false;
            }
        }


        private void contextMenuStrip1_Opening(object sender, CancelEventArgs e)
        {
            if(FavsTree.SelectedNode.Tag == null)
            {
                contextMenuStrip1.Items[0].Text = "Connect to All";
            }
            else
            {
                contextMenuStrip1.Items[0].Text = "Connect to " + FavsTree.SelectedNode.Text;
            }
        }
        private MainForm MainForm
        {
            get
            {
                return (this.ParentForm as MainForm);
            }
        }
        private void connectToolStripMenuItem_MouseUp(object sender, MouseEventArgs e) {
            FavoriteConfigurationElement fav = (FavsTree.SelectedNode.Tag as FavoriteConfigurationElement);

            ToolStripMenuItem menuItem = (sender as ToolStripMenuItem);
            ConnectToMenuItem(menuItem, fav);
        }
        private bool ConnectToMenuItem(ToolStripMenuItem menuItem, FavoriteConfigurationElement fav) {
            if(menuItem != null) {
                if(menuItem.Text.StartsWith("Connect to All")) {
                    if(System.Windows.Forms.MessageBox.Show("Are you sure you want to connect to all of these Terminals?", "Confirm", MessageBoxButtons.YesNo) == DialogResult.Yes) {
                        foreach(TreeNode child in FavsTree.SelectedNode.Nodes) {
                            fav = (child.Tag as FavoriteConfigurationElement);
                            if(fav != null) {
                                MainForm.Connect(fav.Name, false);
                            }
                        }
                    }
                    return true;
                } else if(menuItem.Text.StartsWith("Connect to ")) {
                    if(fav != null) {
                        MainForm.Connect(fav.Name, false);
                        return true;
                    }
                }
            }
            return false;
        }

        private void pingToolStripMenuItem_Click(object sender, EventArgs e) {
            FavoriteConfigurationElement fav = (FavsTree.SelectedNode.Tag as FavoriteConfigurationElement);
            if(fav!=null) MainForm.OpenNetworkingTools("Ping", fav.ServerName);
        }

        private void dNSToolStripMenuItem_Click(object sender, EventArgs e) {
            FavoriteConfigurationElement fav = (FavsTree.SelectedNode.Tag as FavoriteConfigurationElement);
            if(fav != null) MainForm.OpenNetworkingTools("DNS", fav.ServerName);
        }

        private void traceRouteToolStripMenuItem_Click(object sender, EventArgs e) {
            FavoriteConfigurationElement fav = (FavsTree.SelectedNode.Tag as FavoriteConfigurationElement);
            if(fav != null) MainForm.OpenNetworkingTools("Trace", fav.ServerName);
        }

        private void tSAdminToolStripMenuItem_Click(object sender, EventArgs e) {
            FavoriteConfigurationElement fav = (FavsTree.SelectedNode.Tag as FavoriteConfigurationElement);
            if(fav != null) MainForm.OpenNetworkingTools("TSAdmin", fav.ServerName);
        }

        private void propertiesToolStripMenuItem_Click(object sender, EventArgs e) {
            FavoriteConfigurationElement fav = (FavsTree.SelectedNode.Tag as FavoriteConfigurationElement);
            if(fav != null) MainForm.ShowManageTerminalForm(fav);
        }
    }
}