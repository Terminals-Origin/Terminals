using System;
using System.Collections.Generic;
using System.Windows.Forms;
using Terminals.Data;
using Terminals.Forms.Controls;
using Terminals.Integration;
using Terminals.Integration.Export;

namespace Terminals.Forms
{
    internal partial class ExportFrom : Form
    {
        public ExportFrom()
        {
            this.InitializeComponent();

            this.favsTree.Load();
            this.saveFileDialog.Filter = Integrations.Exporters.GetProvidersDialogFilter();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }
        
        private void btnExport_Click(object sender, EventArgs e)
        {
            if (this.saveFileDialog.ShowDialog() == DialogResult.OK)
            {
                if (this.favsTree.SelectedNode != null)
                {
                    this.RunExport();
                }

                MessageBox.Show("Done exporting, you can find your exported file at " + this.saveFileDialog.FileName, "Terminals export");
                this.Close();
            }
        }

        private void RunExport() 
        {
            List<FavoriteConfigurationElement> favorites = this.FindSelectedFavorites();
            // filter index is 1 based
            int filterSplitIndex = (this.saveFileDialog.FilterIndex - 1) * 2;
            string providerFilter = this.saveFileDialog.Filter.Split('|')[filterSplitIndex];
            ExportOptions options = new ExportOptions
                {
                    ProviderFilter = providerFilter,
                    Favorites = favorites,
                    FileName = this.saveFileDialog.FileName,
                    IncludePasswords = this.checkBox1.Checked
                };
            Integrations.Exporters.Export(options);
        }

        private List<FavoriteConfigurationElement> FindSelectedFavorites()
        {
            var favorites = new List<FavoriteConfigurationElement>();
            foreach (GroupTreeNode groupNode in this.favsTree.Nodes)
            {
                ExpandCheckedGroupNode(groupNode);
                FindSelectedGroupFavorites(favorites, groupNode);
            }

            return favorites;
        }

        private static void FindSelectedGroupFavorites(List<FavoriteConfigurationElement> favorites, GroupTreeNode groupNode)
        {
            var persistence = Persistence.Instance;
            // dont expect only Favorite nodes, because dummy nodes arent
            foreach (TreeNode childNode in groupNode.Nodes) 
            {
                if (childNode.Checked)
                {
                    var favoriteNode = childNode as FavoriteTreeNode;
                    if (favoriteNode != null)
                    {
                        FavoriteConfigurationElement favoriteConfig = ModelConverterV2ToV1.ConvertToFavorite(favoriteNode.Favorite, persistence);
                        // Check for duplicate item before adding
                        if (favorites.IndexOf(favoriteConfig) < 0)
                            favorites.Add(favoriteConfig);
                    }  
                }
            }
        }

        /// <summary>
        /// because of lazy loading, expand the node, it doesnt have be already loaded
        /// </summary>
        private static void ExpandCheckedGroupNode(GroupTreeNode groupNode)
        {
            if (groupNode.Checked && groupNode.NotLoadedYet)
            {
                groupNode.ExpandAll();
                CheckSubNodes(groupNode, true);
            }
        }

        private void btnSelect_Click(object sender, EventArgs e)
        {
            TreeNodeCollection rootNodes = this.favsTree.Nodes;
            foreach (TreeNode node in rootNodes)
            {
                node.Checked = true;
                node.ExpandAll();
                CheckNode(node);
            }
        }
       
        private void favsTree_NodeMouseClick(object sender, TreeNodeMouseClickEventArgs e)
        {
            TreeNode node = e.Node;
            CheckSubNodes(node, node.Checked);
        }

        private static void CheckSubNodes(TreeNode nodeToCheck, Boolean check)
        {
            foreach (TreeNode node in nodeToCheck.Nodes)
            {
                node.Checked = check;
            }
        }

        private static void CheckNode(TreeNode nodeToCheck)
        {
            TreeNodeCollection childNodes = nodeToCheck.Nodes;
            foreach (TreeNode childNode in childNodes)
            {
                childNode.Checked = true;
                childNode.ExpandAll();
                if (childNode.GetNodeCount(true) != 0)
                    CheckNode(childNode);
            }
        }

        private void ExportFrom_FormClosing(object sender, FormClosingEventArgs e)
        {
            this.favsTree.UnregisterEvents();
        }        
    }
}
