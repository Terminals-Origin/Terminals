using System;
using System.Collections.Generic;
using System.Windows.Forms;
using Terminals.Configuration;
using Terminals.Forms;
using Terminals.Forms.Controls;
using Terminals.Integration;
using Terminals.Integration.Import;
using Terminals.Network;

namespace Terminals
{
    public partial class OrganizeFavoritesForm : Form
    {
        public OrganizeFavoritesForm()
        {
            InitializeComponent();

            InitializeDataGrid();
            ImportOpenFileDialog.Filter = Integrations.Importers.GetProvidersDialogFilter();
            UpdateCountLabels();
        }

        private void InitializeDataGrid()
        {
            this.dataGridFavorites.AutoGenerateColumns = false;
            this.bsFavorites.DataSource = Settings.GetFavorites().ToListOrderedByDefaultSorting();
            string sortingProperty = FavoriteConfigurationElement.GetDefaultSortPropertyName();
            DataGridViewColumn sortedColumn = this.dataGridFavorites.FindColumnByPropertyName(sortingProperty);
            sortedColumn.HeaderCell.SortGlyphDirection = SortOrder.Ascending;
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
                UpdateFavoritesBindingSource();
            }
        }

        private FavoriteConfigurationElement GetSelectedFavorite()
        {
            if (dataGridFavorites.SelectedRows.Count > 0)
                return dataGridFavorites.SelectedRows[0].DataBoundItem as FavoriteConfigurationElement;
            return null;
        }

        private void dataGridFavorites_DoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex > 0)
            {
                btnEdit_Click(sender, e);
            }
        }

        /// <summary>
        /// Handles F2 and Delete key press in grid. Escape is handled as dialog cancel
        /// </summary>
        private void ConnectionManager_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.F2)
                btnRename.PerformClick();

            // prevent delete whole favorite when editing its name directly in grid cell
            if (e.KeyCode == Keys.Delete && !this.dataGridFavorites.IsCurrentCellInEditMode)
                btnDelete_Click(sender, e);
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
            this.UpdateFavoritesBindingSource();
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
        private FavoriteConfigurationElement editedFavorite;

        private void dataGridFavorites_CellBeginEdit(object sender, DataGridViewCellCancelEventArgs e)
        {
            // the only editable cell should be name
            this.editedFavoriteName = dataGridFavorites.CurrentCell.Value.ToString();
            this.editedFavorite = this.dataGridFavorites.SelectedRows[0].DataBoundItem as FavoriteConfigurationElement;
        }

        /// <summary>
        /// Rename favorite directly in a cell has to be confirmed into the Settings
        /// </summary>
        private void dataGridFavorites_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            if (String.IsNullOrEmpty(this.editedFavorite.Name)) // cancel or nothing changed
                editedFavorite.Name = this.editedFavoriteName;
            if(editedFavorite.Name.Equals(this.editedFavoriteName, StringComparison.CurrentCultureIgnoreCase))
                return;  

            var copy = editedFavorite.Clone() as FavoriteConfigurationElement;
            editedFavorite.Name = this.editedFavoriteName;
            var oldFavorite = Settings.GetOneFavorite(copy.Name);
            if (oldFavorite != null)
            {
                string message = String.Format("A connection named \"{0}\" already exists\r\nDo you want to overwrite it?", copy.Name);
                if (MessageBox.Show(this, message, "Terminals", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    ReplaceFavoriteInBindingSource(copy);
                }
            }
            else
            {
                ReplaceFavoriteInBindingSource(copy);
            }
        }

        private void ReplaceFavoriteInBindingSource(FavoriteConfigurationElement copy)
        {
            Settings.EditFavorite(this.editedFavoriteName, copy, true);
            this.UpdateFavoritesBindingSource();
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
                        UpdateFavoritesBindingSource();
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
                    UpdateFavoritesBindingSource();
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
            this.UpdateFavoritesBindingSource();
        }

        private void networkDetectionToolStripMenuItem_Click(object sender, EventArgs e)
        {
            NetworkScanner networkScanForm = new NetworkScanner();
            networkScanForm.ShowDialog();
            this.UpdateFavoritesBindingSource();
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

        /// <summary>
        /// Opens file dialog to import favorites and imports them from selected files.
        /// </summary>
        internal void CallImport()
        {
            if (ImportOpenFileDialog.ShowDialog() == DialogResult.OK)
            {
                String[] filenames = this.ImportOpenFileDialog.FileNames;
                this.Focus();
                this.Refresh();
                this.Cursor = Cursors.WaitCursor;

                List<FavoriteConfigurationElement> favoritesToImport = Integrations.Importers.ImportFavorites(filenames);
                ImportFavoritesWithManagerImport(favoritesToImport);
            }
        }

        private void ImportFavoritesWithManagerImport(List<FavoriteConfigurationElement> favoritesToImport)
        {
            var managedImport = new ImportWithDialogs(this, false);
            bool imported = managedImport.Import(favoritesToImport);
            if (imported)
                    this.UpdateFavoritesBindingSource();
        }

        /// <summary>
        /// Replace the favorites in binding source doesnt affect performance
        /// </summary>
        private void UpdateFavoritesBindingSource()
        {
            var data = Settings.GetFavorites().ToList();

            DataGridViewColumn lastSortedColumn = this.dataGridFavorites.FindLastSortedColumn();
            if (lastSortedColumn != null) // keep last ordered column
            {
                var backupGliph = lastSortedColumn.HeaderCell.SortGlyphDirection;
                this.bsFavorites.DataSource = data.SortByProperty(lastSortedColumn.DataPropertyName,
                    lastSortedColumn.HeaderCell.SortGlyphDirection);

                lastSortedColumn.HeaderCell.SortGlyphDirection = backupGliph;
            }
            else
            {
                this.bsFavorites.DataSource = data;
            }

            UpdateCountLabels();
        }

        /// <summary>
        /// Sort columns
        /// </summary>
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

        private void btnRegistryImport_Click(object sender, EventArgs e)
        {
            List<FavoriteConfigurationElement> favoritesToImport = ImportRdpRegistry.Import();
            ImportFavoritesWithManagerImport(favoritesToImport);
        }
    }
}