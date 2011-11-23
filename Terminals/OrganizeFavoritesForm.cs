using System;
using System.Collections.Generic;
using System.Windows.Forms;
using Terminals.Configuration;
using Terminals.Data;
using Terminals.Forms;
using Terminals.Forms.Controls;
using Terminals.Integration;
using Terminals.Integration.Import;
using Terminals.Network;

namespace Terminals
{
    internal partial class OrganizeFavoritesForm : Form
    {
        private string editedFavoriteName = String.Empty;
        private FavoriteConfigurationElement editedFavorite;
        internal MainForm MainForm { get; set; }

        private Favorites PersistedFavorites
        {
            get { return Persistance.Instance.Favorites; }
        }

        internal OrganizeFavoritesForm()
        {
            InitializeComponent();

            InitializeDataGrid();
            ImportOpenFileDialog.Filter = Integrations.Importers.GetProvidersDialogFilter();
            UpdateCountLabels();
        }

        private void InitializeDataGrid()
        {
            this.dataGridFavorites.AutoGenerateColumns = false;
            this.bsFavorites.DataSource = PersistedFavorites.GetFavorites().ToListOrderedByDefaultSorting();
            string sortingProperty = FavoriteConfigurationElement.GetDefaultSortPropertyName();
            DataGridViewColumn sortedColumn = this.dataGridFavorites.FindColumnByPropertyName(sortingProperty);
            sortedColumn.HeaderCell.SortGlyphDirection = SortOrder.Ascending;
            this.dataGridFavorites.DataSource = this.bsFavorites;
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
            if (e.RowIndex >= 0) // dont allow double click on column row
                EditFavorite();
        }

        /// <summary>
        /// Delete key press in grid.
        /// </summary>
        private void dataGridFavorites_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Control && e.KeyCode == Keys.C)
                CopySelectedFavorite();

            if (e.KeyCode == Keys.Delete)
                DeleteSelectedFavorites();
        }

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
            if (editedFavorite.Name.Equals(this.editedFavoriteName, StringComparison.CurrentCultureIgnoreCase))
                return;

            var copy = editedFavorite.Clone() as FavoriteConfigurationElement;
            editedFavorite.Name = this.editedFavoriteName;
            var oldFavorite = PersistedFavorites.GetOneFavorite(copy.Name);
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
            PersistedFavorites.EditFavorite(this.editedFavoriteName, copy);
            this.UpdateFavoritesBindingSource();
        }

        private void OrganizeFavoritesForm_Shown(object sender, EventArgs e)
        {
            if (this.dataGridFavorites.RowCount > 0)
                dataGridFavorites.Rows[0].Selected = true;
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
            var managedImport = new ImportWithDialogs(this);
            bool imported = managedImport.Import(favoritesToImport);
            if (imported)
                this.UpdateFavoritesBindingSource();
        }

        /// <summary>
        /// Replace the favorites in binding source doesnt affect performance
        /// </summary>
        private void UpdateFavoritesBindingSource()
        {
            var data = PersistedFavorites.GetFavorites().ToListOrderedByDefaultSorting();

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

        private void newToolStripMenuItem_Click(object sender, EventArgs e)
        {
            using (NewTerminalForm frmNewTerminal = new NewTerminalForm(String.Empty))
            {
                if (frmNewTerminal.ShowDialog() != TerminalFormDialogResult.Cancel)
                {
                    UpdateFavoritesBindingSource();
                }
            }
        }

        private void editConnectinoToolStripMenuItem_Click(object sender, EventArgs e)
        {
            EditFavorite();
        }

        private void EditFavorite()
        {
            FavoriteConfigurationElement favorite = this.GetSelectedFavorite();
            if (favorite != null)
                this.EditFavorite(favorite);
        }

        private void deleteToolStripMenuItem_Click(object sender, EventArgs e)
        {
            DeleteSelectedFavorites();
        }

        private void DeleteSelectedFavorites()
        {
            // prevent delete whole favorite when editing its name directly in grid cell
            if (this.dataGridFavorites.IsCurrentCellInEditMode)
                return;

            this.Cursor = Cursors.WaitCursor;
            List<FavoriteConfigurationElement> selectedFavorites = GetSelectedFavorites();
            PersistedFavorites.DeleteFavorites(selectedFavorites);
            this.UpdateFavoritesBindingSource();
            this.Cursor = Cursors.Default;
        }

        private List<FavoriteConfigurationElement> GetSelectedFavorites()
        {
            var selectedFavorites = new List<FavoriteConfigurationElement>();
            foreach (DataGridViewRow selectedRow in this.dataGridFavorites.SelectedRows)
            {
                var selectedFavorite = selectedRow.DataBoundItem as FavoriteConfigurationElement;
                selectedFavorites.Add(selectedFavorite);
            }
            return selectedFavorites;
        }

        private void copyToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (!this.dataGridFavorites.IsCurrentCellInEditMode)
                CopySelectedFavorite();
        }

        private void CopySelectedFavorite()
        {
            FavoriteConfigurationElement favorite = this.GetSelectedFavorite();
            if (favorite != null)
            {
                InputBoxResult result = InputBox.Show("New Connection Name");
                if (result.ReturnCode == DialogResult.OK && !string.IsNullOrEmpty(result.Text))
                {
                    this.CopySelectedFavorite(favorite, result.Text);
                }
            }
        }

        private void CopySelectedFavorite(FavoriteConfigurationElement favorite, string newName)
        {
            FavoriteConfigurationElement newFav = favorite.Clone() as FavoriteConfigurationElement;
            if (newFav != null)
            {
                newFav.Name = newName;
                PersistedFavorites.AddFavorite(newFav);
                if (Settings.HasToolbarButton(favorite.Name))
                    Settings.AddFavoriteButton(newFav.Name);
                this.UpdateFavoritesBindingSource();
            }
        }

        private void renameToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (this.dataGridFavorites.SelectedRows.Count > 0)
            {
                dataGridFavorites.CurrentCell = this.dataGridFavorites.SelectedRows[0].Cells["colName"];
                this.dataGridFavorites.BeginEdit(true);
            }
        }

        private void scanActiveDirectoryToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ImportFromAD activeDirectoryForm = new ImportFromAD();
            activeDirectoryForm.ShowDialog();
            this.UpdateFavoritesBindingSource();
        }

        private void scanNetworkToolStripMenuItem_Click(object sender, EventArgs e)
        {
            NetworkScanner networkScanForm = new NetworkScanner();
            networkScanForm.ShowDialog();
            this.UpdateFavoritesBindingSource();
        }

        private void scanRegistryToolStripMenuItem_Click(object sender, EventArgs e)
        {
            List<FavoriteConfigurationElement> favoritesToImport = ImportRdpRegistry.Import();
            ImportFavoritesWithManagerImport(favoritesToImport);
        }

        private void closeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void importFromFileToolStripMenuItem_Click_1(object sender, EventArgs e)
        {
            CallImport();
        }

        private void exportToAFileToolStripMenuItem_Click_1(object sender, EventArgs e)
        {
            ExportFrom exportFrom = new ExportFrom();
            exportFrom.Show();
        }

        private void connectToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (MainForm != null)
            {
                FavoriteConfigurationElement favorite = GetSelectedFavorite();
                MainForm.Connect(favorite.Name, favorite.ConnectToConsole, favorite.NewWindow);
            }
        }
    }
}