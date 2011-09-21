using System;
using System.Collections.Generic;
using System.Windows.Forms;
using Terminals.Configuration;
using Terminals.Forms;
using Terminals.Forms.Controls;
using Terminals.Integration.Import;
using Terminals.Network;

namespace Terminals
{
    public partial class OrganizeFavoritesForm : Form
    {
        public OrganizeFavoritesForm()
        {
            InitializeComponent();

            this.dataGridFavorites.AutoGenerateColumns = false;
            this.bsFavorites.DataSource = Settings.GetFavorites().ToList()
                                                  .SortByProperty("Name",SortOrder.Ascending);
            this.dataGridFavorites.Columns["colName"].HeaderCell.SortGlyphDirection = SortOrder.Ascending;

            ImportOpenFileDialog.Filter = Importers.GetImportersDialogFilter();
            UpdateCountLabels();
        }

        private void UpdateCountLabels()
        {
            Int32 selectedItems = this.dataGridFavorites.SelectedRows.Count;
            this.lblSelectedCount.Text = String.Format("({0} selected)", selectedItems);
            this.lblConnectionCount.Text = this.bsFavorites.Count.ToString();
        }

        private void EditFavorite(FavoriteConfigurationElement favorite)
        {
            NewTerminalForm frmNewTerminal = new NewTerminalForm(favorite);
            if (frmNewTerminal.ShowDialog() != TerminalFormDialogResult.Cancel)
            {
                // because the favorite instance is replaced
                int dataSourceIndex = bsFavorites.IndexOf(favorite);
                bsFavorites.RemoveAt(dataSourceIndex);
                bsFavorites.Insert(dataSourceIndex, frmNewTerminal.Favorite);
            }
        }

        private FavoriteConfigurationElement GetSelectedFavorite()
        {
            if (dataGridFavorites.SelectedRows.Count > 0)
                return dataGridFavorites.SelectedRows[0].DataBoundItem as FavoriteConfigurationElement;
            return null;
        }

        /// <summary>
        /// Start edit in data grid
        /// </summary>
        private void ConnectionManager_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.F2)
                btnRename.PerformClick();
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            this.Cursor = Cursors.WaitCursor;
            var selectedFavorites = new List<FavoriteConfigurationElement>();
            foreach (DataGridViewRow selectedRow in this.dataGridFavorites.SelectedRows)
            {
                var selectedFavorite = selectedRow.DataBoundItem as FavoriteConfigurationElement;
                selectedFavorites.Add(selectedFavorite);
            }

            Settings.DeleteFavorites(selectedFavorites);
            AddFavoritesToBindingSource();
            this.Cursor = Cursors.Default;
        }

        private void btnRename_Click(object sender, EventArgs e)
        {
            if (this.dataGridFavorites.SelectedRows.Count > 0)
            {
                dataGridFavorites.CurrentCell = this.dataGridFavorites.SelectedRows[0].Cells["colName"];
                this.dataGridFavorites.BeginEdit(true);
            }
        }

        private string editedFavoriteName = String.Empty;

        private void dataGridFavorites_CellBeginEdit(object sender, DataGridViewCellCancelEventArgs e)
        {
            // the only editable cell should be name
            this.editedFavoriteName = dataGridFavorites.CurrentCell.Value.ToString();
        }

        /// <summary>
        /// Rename favorite directly in a cell has to be confirmed into the Settings
        /// </summary>
        private void dataGridFavorites_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            var editedFavorite = this.dataGridFavorites.SelectedRows[0].DataBoundItem as FavoriteConfigurationElement;
            if(editedFavorite.Name.Equals(this.editedFavoriteName, StringComparison.CurrentCultureIgnoreCase))
                return;  // cancel or nothing changed

            var copy = editedFavorite.Clone() as FavoriteConfigurationElement;
            editedFavorite.Name = this.editedFavoriteName;
            var oldFavorite = Settings.GetOneFavorite(copy.Name);
            if (oldFavorite != null)
            {
                string message = String.Format("A connection named \"{0}\" already exists\r\nDo you want to overwrite it?", copy.Name);
                if (MessageBox.Show(this, message, "Terminals", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    this.bsFavorites.Remove(oldFavorite);
                    ReplaceFavoriteInBindingSource(copy, editedFavorite);
                }
            }
            else
            {
                ReplaceFavoriteInBindingSource(copy, editedFavorite);
            }
        }

        private void ReplaceFavoriteInBindingSource(FavoriteConfigurationElement copy, FavoriteConfigurationElement oldFavorite)
        {
            Settings.EditFavorite(this.editedFavoriteName, copy, true);
            this.bsFavorites.Remove(oldFavorite);
            this.bsFavorites.Add(copy);
        }

        private void btnEdit_Click(object sender, EventArgs e)
        {
            FavoriteConfigurationElement favorite = GetSelectedFavorite();
            if (favorite != null)
                EditFavorite(favorite);
        }

        private void btnCopy_Click(object sender, EventArgs e)
        {
            FavoriteConfigurationElement favorite = GetSelectedFavorite();
            if (favorite != null)
            {
                InputBoxResult result = InputBox.Show("New Connection Name");
                if (result.ReturnCode == DialogResult.OK && !string.IsNullOrEmpty(result.Text))
                {
                    FavoriteConfigurationElement newFav = favorite.Clone() as FavoriteConfigurationElement;
                    if (newFav != null)
                    {
                        newFav.Name = result.Text;
                        Settings.AddFavorite(newFav, Settings.HasToolbarButton(newFav.Name));
                        this.bsFavorites.Add(newFav);
                        UpdateCountLabels();
                    }
                }
            }
        }

        private void btnNew_Click(object sender, EventArgs e)
        {
            using (NewTerminalForm frmNewTerminal = new NewTerminalForm(String.Empty))
            {
                if (frmNewTerminal.ShowDialog() != TerminalFormDialogResult.Cancel)
                {
                    Settings.AddFavorite(frmNewTerminal.Favorite, frmNewTerminal.ShowOnToolbar);
                    this.bsFavorites.Add(frmNewTerminal.Favorite);
                    UpdateCountLabels();
                }
            }
        }

        private void OrganizeFavoritesForm_Shown(object sender, EventArgs e)
        {
            if (this.dataGridFavorites.RowCount > 0)
                dataGridFavorites.Rows[0].Selected = true;
        }

        private void activeDirectoryToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ImportFromAD activeDirectoryForm = new ImportFromAD();
            activeDirectoryForm.ShowDialog();
            AddFavoritesToBindingSource();
        }

        private void networkDetectionToolStripMenuItem_Click(object sender, EventArgs e)
        {
            NetworkScanner networkScanForm = new NetworkScanner();
            networkScanForm.ShowDialog();
            AddFavoritesToBindingSource();
        }

        private void ImportButton_Click(object sender, EventArgs e)
        {
            CallImport();
        }

        private void btnExport_Click(object sender, EventArgs e)
        {
            ExportFrom exportFrom = new ExportFrom();
            exportFrom.Show();
        }

        internal void CallImport()
        {
            if (ImportOpenFileDialog.ShowDialog() == DialogResult.OK)
            {
                String[] filenames = this.ImportOpenFileDialog.FileNames;
                this.Focus();
                this.Refresh();
                this.Cursor = Cursors.WaitCursor;
                List<FavoriteConfigurationElement> favoritesToImport = Importers.ImportFavorites(filenames);

                var managedImport = new ImportWithDialogs(this, false);
                Boolean imported = managedImport.Import(favoritesToImport);

                if (imported)
                    this.AddFavoritesToBindingSource();
            }
        }

        private void AddFavoritesToBindingSource()
        {
            this.bsFavorites.DataSource = Settings.GetFavorites().ToList();
            this.bsFavorites.ResetBindings(false);
            UpdateCountLabels();
        }

        private void dataGridFavorites_ColumnHeaderMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            DataGridViewColumn lastSortedColumn = this.dataGridFavorites.FindLastSortedColumn();
            DataGridViewColumn column = this.dataGridFavorites.Columns[e.ColumnIndex];

            SortOrder newSortDirection = SortableUnboundGrid.GetNewSortDirection(lastSortedColumn, column);
            var data = this.bsFavorites.DataSource as SortableList<FavoriteConfigurationElement>;
            this.bsFavorites.DataSource = data.SortByProperty(column.DataPropertyName, newSortDirection);
            column.HeaderCell.SortGlyphDirection = newSortDirection;
        }

        private void dataGridFavorites_SelectionChanged(object sender, EventArgs e)
        {
            UpdateCountLabels();
        }
    }
}