using System;
using System.Collections.Generic;
using System.Windows.Forms;
using Terminals.Configuration;

namespace Terminals
{
    internal partial class ExportFrom : Form
    {
        public ExportFrom()
        {
            InitializeComponent();
            LoadExportList();
        }        

        private void btnCancel_Click(object sender, EventArgs e)
        {
            Close();
        }
        
        private void btnExport_Click(object sender, EventArgs e)
        {
            if (saveFileDialog.ShowDialog() == DialogResult.OK)
            {
                if (favsTree.SelectedNode != null)
                {
                    List<FavoriteConfigurationElement> favorites = FindSelectedFavorites();
                    ExportImport.ExportImport.ExportXML(saveFileDialog.FileName, favorites, checkBox1.Checked);
                }
                MessageBox.Show("Done exporting, you can find your exported file at " + saveFileDialog.FileName,
                                "Terminals export");
                Close();
            }
        }

        private List<FavoriteConfigurationElement> FindSelectedFavorites()
        {
            List<FavoriteConfigurationElement> favorites = new List<FavoriteConfigurationElement>();
            foreach (TreeNode tn in this.favsTree.Nodes)
            {
                foreach (TreeNode node in tn.Nodes)
                {
                    if (node.Checked)
                        favorites.Add(node.Tag as FavoriteConfigurationElement);
                }
            }

            return favorites;
        }

        private void btnSelect_Click(object sender, EventArgs e)
        {
            TreeNodeCollection tn = favsTree.Nodes;
            foreach (TreeNode node in tn)
            {
                node.Checked = true;
                node.ExpandAll();
                CheckNode(node);
            }
        }
       
        private void favsTree_NodeMouseClick(object sender, TreeNodeMouseClickEventArgs e)
        {
            TreeNode tn = e.Node;
            if (tn.Checked)
            {
                CheckSubNodes(tn, true);
            }
            else
            {
                CheckSubNodes(tn, false);
            }
        }

        private static void CheckSubNodes(TreeNode tn, Boolean check)
        {
            foreach (TreeNode node in tn.Nodes)
            {
                node.Checked = check;
            }
        }

        private static void CheckNode(TreeNode node)
        {
            TreeNodeCollection tn = node.Nodes;
            foreach (TreeNode n in tn)
            {
                n.Checked = true;
                n.ExpandAll();
                if (n.GetNodeCount(true) != 0)
                    CheckNode(n);
            }
        }        
        
        private void LoadExportList()
        {
            favsTree.Nodes.Clear();
            SortedDictionary<string, FavoriteConfigurationElement> favorites =
                Settings.GetSortedFavorites(Settings.DefaultSortProperty);
            
            if (favorites != null)
            {
                SortedDictionary<string, TreeNode> sortedTags = new SortedDictionary<string, TreeNode>();
                foreach (KeyValuePair<string, FavoriteConfigurationElement> favoriteKeyPair in favorites)
                {
                    AddFavoriteTreeNode(favoriteKeyPair, sortedTags);
                }
            }
            favsTree.Sort();
        }

        private void AddFavoriteTreeNode(KeyValuePair<string, FavoriteConfigurationElement> favoriteKeyPair,
                                         SortedDictionary<string, TreeNode> sortedTags)
        {
            FavoriteConfigurationElement favorite = favoriteKeyPair.Value;
            if (favorite.TagList.Count > 0)
            {
                foreach (string tag in favorite.TagList)
                {
                    this.CreateNodeInSortedTagsByTag(sortedTags, tag, favorite);
                }
            }
            else
            {
                this.CreateNodeInSortedTagsByTag(sortedTags, FavsList.UNTAGGED_NODENAME, favorite);
            }
        }

        private void CreateNodeInSortedTagsByTag(SortedDictionary<string, TreeNode> sortedTags,
                                                 string tag, FavoriteConfigurationElement favorite)
        {
            this.EnsureTagNode(sortedTags, tag);
            TreeNode favNode = new TreeNode(favorite.Name);
            favNode.Tag = favorite;

            if (!sortedTags[tag].Nodes.Contains(favNode))
                sortedTags[tag].Nodes.Add(favNode);
        }

        /// <summary>
        /// Create tree node for Tag, if isn't already in sortedTags
        /// </summary>
        private void EnsureTagNode(SortedDictionary<string, TreeNode> sortedTags, string tag)
        {
            if (!sortedTags.ContainsKey(tag))
            {
                TreeNode tagNode = new TreeNode(tag);
                this.favsTree.Nodes.Add(tagNode);
                sortedTags.Add(tag, tagNode);
            }
        }
    }
}
