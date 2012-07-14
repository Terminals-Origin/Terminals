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
        private IFavorite editedFavorite;
        internal MainForm MainForm { get; set; }

        private static IFavorites PersistedFavorites
        {
            get { return Persistence.Instance.Favorites; }
        }

        internal OrganizeFavoritesForm()
        {
            InitializeComponent();

            InitializeDataGrid();
            ImportOpenFileDialog.Filter = Integrations.Importers.GetProvidersDialogFilter();
            UpdateCountLabels();
        }

        private void OrganizeFavoritesForm_Shown(object sender, EventArgs e)
        {
            if (this.dataGridFavorites.RowCount > 0)
                dataGridFavorites.Rows[0].Selected = true;
        }


        private void OrganizeFavoritesForm_KeyDown(object sender, System.Windows.Forms.KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape)
                this.Close();
        }

        private void InitializeDataGrid()
        {
            this.dataGridFavorites.AutoGenerateColumns = false; // because of designer
            this.bsFavorites.DataSource = PersistedFavorites.ToListOrderedByDefaultSorting();
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

        private void EditFavorite(IFavorite favorite)
        {
            NewTerminalForm frmNewTerminal = new NewTerminalForm(favorite);
            if (frmNewTerminal.ShowDialog() != TerminalFormDialogResult.Cancel)
            {
                UpdateFavoritesBindingSource();
            }
        }

        private IFavorite GetSelectedFavorite()
        {
            if (dataGridFavorites.SelectedRows.Count > 0)
                return dataGridFavorites.SelectedRows[0].DataBoundItem as IFavorite;
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
            this.editedFavorite = this.dataGridFavorites.SelectedRows[0].DataBoundItem as IFavorite;
            this.editedFavoriteName = this.editedFavorite.Name;
        }

        /// <summary>
        /// Rename favorite directly in a cell has to be confirmed into the Settings
        /// </summary>
        private void dataGridFavorites_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            if (String.IsNullOrEmpty(this.editedFavorite.Name)) // cancel or nothing changed
                editedFavorite.Name = this.editedFavoriteName;
            if (editedFavorite.Name.Equals(this.editedFavoriteName, StringComparison.CurrentCultureIgnoreCase))
            {
                editedFavorite.Name = this.editedFavoriteName;
                return;
            }

            UpdateFavoritePreservingDuplicitNames(this.editedFavoriteName, editedFavorite.Name, this.editedFavorite);
            this.UpdateFavoritesBindingSource();
        }

        /// <summary>
        /// Asks user, if wants to overwrite already present favorite the newName by conflicting (editedFavorite).
        /// </summary>
        /// <param name="oldName">The olready present favorite name to check.</param>
        /// <param name="newName">The newly assigned name of edited favorite.</param>
        /// <param name="editedFavorite">The currently edited favorite to update.</param>
        internal static void UpdateFavoritePreservingDuplicitNames(string oldName, string newName, IFavorite editedFavorite)
        {
            IFavorites persistedFavorites = Persistence.Instance.Favorites;
            editedFavorite.Name = oldName; // to prevent find it self as oldFavorite
            var oldFavorite = persistedFavorites[newName];
            if (oldFavorite != null && !editedFavorite.Equals(oldFavorite)) // prevent conflict with another favorite than edited
            {
                OverwriteByConflictingName(newName, oldFavorite, editedFavorite);
            }
            else
            {
                editedFavorite.Name = newName;
                // dont have to update buttons here, because they arent changed
                persistedFavorites.Update(editedFavorite);
            }
        }

        private static void OverwriteByConflictingName(string newName, IFavorite oldFavorite, IFavorite editedFavorite)
        {
            if (AskUserIfWantsToOverwrite(newName))
            {
                IFavorites persistedFavorites = Persistence.Instance.Favorites;
                Persistence.Instance.StartDelayedUpdate();
                var groups = editedFavorite.Groups;
                persistedFavorites.Delete(editedFavorite);
                editedFavorite.Id = oldFavorite.Id;
                editedFavorite.Name = newName;
                persistedFavorites.Update(editedFavorite);
                persistedFavorites.UpdateFavorite(editedFavorite, groups);
                Persistence.Instance.SaveAndFinishDelayedUpdate();
            }
        }

        private static bool AskUserIfWantsToOverwrite(string newName)
        {
            string message = String.Format("A connection named \"{0}\" already exists\r\nDo you want to overwrite it?", newName);
            return MessageBox.Show(message, Terminals.Program.Info.Title, MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes;
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
            var data = PersistedFavorites.ToListOrderedByDefaultSorting();

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
            var data = this.bsFavorites.DataSource as SortableList<IFavorite>;
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
            IFavorite favorite = this.GetSelectedFavorite();
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
            List<IFavorite> selectedFavorites = GetSelectedFavorites();
            PersistedFavorites.Delete(selectedFavorites);

            this.UpdateFavoritesBindingSource();
            this.Cursor = Cursors.Default;
        }

        private List<IFavorite> GetSelectedFavorites()
        {
            var selectedFavorites = new List<IFavorite>();
            foreach (DataGridViewRow selectedRow in this.dataGridFavorites.SelectedRows)
            {
                var selectedFavorite = selectedRow.DataBoundItem as IFavorite;
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
            IFavorite favorite = this.GetSelectedFavorite();
            if (favorite != null)
            {
                InputBoxResult result = InputBox.Show("Enter new name:", "Copy selected favorite as ...");
                if (result.ReturnCode == DialogResult.OK && !string.IsNullOrEmpty(result.Text))
                {
                    this.CopySelectedFavorite(favorite, result.Text);
                }
            }
        }

        private void CopySelectedFavorite(IFavorite favorite, string newName)
        {
            IFavorite copy = favorite.Copy();
            copy.Name = newName;
            PersistedFavorites.Add(copy);
            PersistedFavorites.UpdateFavorite(copy, favorite.Groups);
            
            if (Settings.HasToolbarButton(favorite.Id))
                Settings.AddFavoriteButton(copy.Id);
            this.UpdateFavoritesBindingSource();
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
                IFavorite favorite = GetSelectedFavorite();
                bool console = false;
                RdpOptions rdpOptions = favorite.ProtocolProperties as RdpOptions;
                if (rdpOptions != null)
                    console = rdpOptions.ConnectToConsole;
                MainForm.Connect(favorite.Name, console, favorite.NewWindow);
            }
        }
    }
}