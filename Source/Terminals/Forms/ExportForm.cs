using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using Terminals.Data;
using Terminals.Forms.Controls;
using Terminals.Integration;
using Terminals.Integration.Export;

namespace Terminals.Forms
{
    internal partial class ExportForm : Form
    {
        private readonly IPersistence persistence = Persistence.Instance;
        private readonly FavoriteTreeListLoader treeLoader; 

        public ExportForm()
        {
            this.InitializeComponent();

            this.treeLoader = new FavoriteTreeListLoader(this.favsTree, this.persistence);
            this.treeLoader.LoadGroups();
            this.saveFileDialog.Filter = Integrations.Exporters.GetProvidersDialogFilter();
        }

        private void BtnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }
        
        private void BtnExport_Click(object sender, EventArgs e)
        {
            if (this.saveFileDialog.ShowDialog() == DialogResult.OK)
            {
                if (this.favsTree.SelectedNode != null)
                    this.RunExport();

                string message = "Done exporting, you can find your exported file at " + this.saveFileDialog.FileName;
                MessageBox.Show(message, "Terminals export");
                this.Close();
            }
        }

        private void RunExport() 
        {
            List<FavoriteConfigurationElement> favorites = this.GetFavoritesToExport();
            // filter index is 1 based
            int filterSplitIndex = (this.saveFileDialog.FilterIndex - 1) * 2;
            string providerFilter = this.saveFileDialog.Filter.Split('|')[filterSplitIndex];
            var options = new ExportOptions
                {
                    ProviderFilter = providerFilter,
                    Favorites = favorites,
                    FileName = this.saveFileDialog.FileName,
                    IncludePasswords = this.checkBox1.Checked
                };
            Integrations.Exporters.Export(options);
        }

        private List<FavoriteConfigurationElement> GetFavoritesToExport()
        {
            var favorites = new List<IFavorite>();
            TreeNodeCollection nodes = this.favsTree.Nodes;
            this.FindAllFavorites(favorites, nodes);
            return this.ConvertFavoritesToExport(favorites);
        }

        private void FindAllFavorites(List<IFavorite> favorites, TreeNodeCollection nodes)
        {
            var candidates = this.FindCheckedFavorites(nodes);
            favorites.AddRange(candidates);

            foreach (GroupTreeNode groupNode in nodes.OfType<GroupTreeNode>())
            {
                // dont expect only Favorite nodes, because of group nodes on the same level
                ExpandCheckedGroupNode(groupNode);
                this.FindAllFavorites(favorites, groupNode.Nodes);
            }
        }

        private List<FavoriteConfigurationElement> ConvertFavoritesToExport(List<IFavorite> favorites)
        {
            return favorites.Distinct()
                .Select(favorite => ModelConverterV2ToV1.ConvertToFavorite(favorite, this.persistence))
                .ToList();
        }

        private IEnumerable<IFavorite> FindCheckedFavorites(TreeNodeCollection treeNodes)
        {
            return treeNodes.OfType<FavoriteTreeNode>()
                            .Where(node => node.Checked)
                            .Select(node => node.Favorite);
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

        private void BtnSelect_Click(object sender, EventArgs e)
        {
            TreeNodeCollection rootNodes = this.favsTree.Nodes;
            foreach (TreeNode node in rootNodes)
            {
                node.Checked = true;
                node.ExpandAll();
                CheckNode(node);
            }
        }
       
        private void FavsTree_NodeMouseClick(object sender, TreeNodeMouseClickEventArgs e)
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

        private void ExportForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            this.treeLoader.UnregisterEvents();
        }        
    }
}
