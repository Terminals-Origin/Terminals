using System;
using System.Collections.Generic;
using System.Windows.Forms;
using Terminals.Integration;

namespace Terminals
{
    internal partial class ExportFrom : Form
    {
        public ExportFrom()
        {
            InitializeComponent();

            this.favsTree.Load();
            this.saveFileDialog.Filter = Integrations.Exporters.GetProvidersDialogFilter();
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
                    Integrations.Exporters.Export(saveFileDialog.FileName, favorites, checkBox1.Checked);
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

        private void ExportFrom_FormClosing(object sender, FormClosingEventArgs e)
        {
            this.favsTree.UnregisterEvents();
        }        
    }
}
