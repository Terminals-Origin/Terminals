using System;
using System.Collections.Generic;
using System.Windows.Forms;
using System.Xml;
using System.Xml.Serialization;

namespace Terminals
{
    public partial class Export : Form
    {
        public Export()
        {
            InitializeComponent();
            saveFileDialog = new SaveFileDialog();
            saveFileDialog.Filter = "Save as XML *.xml|*.xml";
            saveFileDialog.Title = "Save export list as";

            openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "Open XML *.xml|*.xml";
            openFileDialog.Title = "Open file to import";
            LoadExportList();
        }        

        #region private
        private void btnCancel_Click(object sender, EventArgs e)
        {
            Close();
        }
        private void btnExport_Click(object sender, EventArgs e)
        {
            if (saveFileDialog.ShowDialog() == DialogResult.OK)
            {

                List<FavoriteConfigurationElement> favorites = new List<FavoriteConfigurationElement>();
                if (favsTree.SelectedNode != null)
                {
                    TreeNodeCollection tnc = favsTree.Nodes;
                    foreach (TreeNode tn in tnc)
                    {
                        if (tn.Checked && tn.Tag == null)
                        {
                            TreeNodeCollection nodes = tn.Nodes;
                            foreach (TreeNode node in nodes)
                            {
                                if (node.Checked)
                                    favorites.Add(node.Tag as FavoriteConfigurationElement);
                            }
                        }
                    }
                    ExportImport.ExportImport.ExportXML(saveFileDialog.FileName, favorites, checkBox1.Checked);
                }
                MessageBox.Show("Done exporting, you can find your exported file at " + saveFileDialog.FileName);
                Close();
            }
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
                foreach (TreeNode node in tn.Nodes)
                {
                    node.Checked = true;
                }
                tn.ExpandAll();
            }
            else
            {
                foreach (TreeNode node in tn.Nodes)
                {
                    node.Checked = false;
                }
                tn.Collapse();
            }
        }
        private void CheckNode(TreeNode node)
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
            SortedDictionary<string, FavoriteConfigurationElement> favorites = Settings.GetSortedFavorites(Settings.DefaultSortProperty);
            SortedDictionary<string, TreeNode> sortedTags = new SortedDictionary<string, TreeNode>();
            if (favorites != null)
            {
                foreach (string key in favorites.Keys)
                {
                    FavoriteConfigurationElement fav = favorites[key];
                    if (fav.TagList.Count > 0)
                    {
                        foreach (string tag in fav.TagList)
                        {
                            TreeNode favNode = new TreeNode(fav.Name);
                            favNode.Tag = fav;
                            if (!sortedTags.ContainsKey(tag))
                            {
                                TreeNode tagNode = new TreeNode(tag);
                                if (!tag.Contains("Terminals"))
                                {
                                    favsTree.Nodes.Add(tagNode);
                                    sortedTags.Add(tag, tagNode);
                                }
                            }

                            if (!tag.Contains("Terminals"))
                            {
                                if (!sortedTags[tag].Nodes.Contains(favNode))
                                    sortedTags[tag].Nodes.Add(favNode);
                            }
                        }
                    }
                    else
                    {
                        TreeNode favNode = new TreeNode(fav.Name);
                        favNode.Tag = fav;
                    }
                }
            }
            favsTree.Sort();
        }        
        #endregion
    }
}
