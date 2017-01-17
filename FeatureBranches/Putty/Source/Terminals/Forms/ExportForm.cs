using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using Terminals.Connections;
using Terminals.Data;
using Terminals.Forms.Controls;
using Terminals.Integration;
using Terminals.Integration.Export;

namespace Terminals.Forms
{
    internal partial class ExportForm : Form
    {
        private readonly IPersistence persistence;
        private readonly FavoriteTreeListLoader treeLoader;
        private readonly Exporters exporters;

        private readonly ConnectionManager connectionManager;

        private readonly FavoriteIcons favoriteIcons;

        public ExportForm(IPersistence persistence, ConnectionManager connectionManager, FavoriteIcons favoriteIcons)
        {
            this.persistence = persistence;
            this.InitializeComponent();

            this.treeLoader = new FavoriteTreeListLoader(this.favsTree, this.persistence, this.favoriteIcons);
            this.treeLoader.LoadRootNodes();
            this.connectionManager = connectionManager;
            this.favoriteIcons = favoriteIcons;
            this.exporters = new Exporters(this.persistence, this.connectionManager);
            this.saveFileDialog.Filter = this.exporters.GetProvidersDialogFilter();
        }

        private void ExportForm_Load(object sender, EventArgs e)
        {
            this.favsTree.AssignServices(this.persistence, this.favoriteIcons, this.connectionManager);
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
            this.exporters.Export(options);
        }

        private List<FavoriteConfigurationElement> GetFavoritesToExport()
        {
            List<IFavorite> favorites = TreeListNodes.FindAllCheckedFavorites(this.favsTree.Nodes);
            return this.ConvertFavoritesToExport(favorites);
        }

        private List<FavoriteConfigurationElement> ConvertFavoritesToExport(List<IFavorite> favorites)
        {
            return favorites.Distinct()
                .Select(favorite => ModelConverterV2ToV1.ConvertToFavorite(favorite, this.persistence, this.connectionManager))
                .ToList();
        }

        private void FavsTree_AfterCheck(object sender, TreeViewEventArgs e)
        {
            var groupNode = e.Node as GroupTreeNode;
            if (groupNode != null)
                groupNode.CheckChildsByParent();
        }

        private void BtnSelect_Click(object sender, EventArgs e)
        {
            // dont expand only load complete subtree
            this.treeLoader.LoadGroupNodesRecursive();
            TreeListNodes.CheckChildNodesRecursive(this.favsTree.Nodes, true);
        }

        private void ExportForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            this.treeLoader.UnregisterEvents();
        }
    }
}
