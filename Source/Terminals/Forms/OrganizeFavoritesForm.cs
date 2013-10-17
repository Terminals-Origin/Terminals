using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Windows.Forms;
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
        private ConnectionsUiFactory connectionsUiFactory;
        private SortableList<IFavorite> foundFavorites = new SortableList<IFavorite>();

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

        internal void AssignConnectionsUiFactory(ConnectionsUiFactory connectionsUiFactory)
        {
            this.connectionsUiFactory = connectionsUiFactory;
        }

        private void OrganizeFavoritesForm_Shown(object sender, EventArgs e)
        {
            if (this.dataGridFavorites.RowCount > 0)
                dataGridFavorites.Rows[0].Selected = true;
        }

        private void InitializeDataGrid()
        {
            this.dataGridFavorites.AutoGenerateColumns = false; // because of designer
            this.bsFavorites.DataSource = ConvertFavoritesToViewModel(PersistedFavorites.ToListOrderedByDefaultSorting());
            string sortingProperty = FavoriteConfigurationElement.GetDefaultSortPropertyName();
            DataGridViewColumn sortedColumn = this.dataGridFavorites.FindColumnByPropertyName(sortingProperty);
            sortedColumn.HeaderCell.SortGlyphDirection = SortOrder.Ascending;
            this.dataGridFavorites.DataSource = this.bsFavorites;
        }

        private static SortableList<FavoriteViewModel> ConvertFavoritesToViewModel(SortableList<IFavorite> source)
        {
            ICredentials storedCredentials = Persistence.Instance.Credentials;
            IEnumerable<FavoriteViewModel> sortedFavorites = source.Select(favorite => new FavoriteViewModel(favorite, storedCredentials));
            return new SortableList<FavoriteViewModel>(sortedFavorites);
        }

        private void UpdateCountLabels()
        {
            Int32 selectedItems = this.dataGridFavorites.SelectedRows.Count;
            this.lblSelectedCount.Text = String.Format("({0} selected)", selectedItems);
            this.lblConnectionCount.Text = this.bsFavorites.Count.ToString(CultureInfo.InvariantCulture);
        }

        private void EditFavorite(IFavorite favorite)
        {
            using (var frmNewTerminal = new NewTerminalForm(Persistence.Instance, favorite))
            {
                if (frmNewTerminal.ShowDialog() != TerminalFormDialogResult.Cancel)
                {
                    UpdateFavoritesBindingSource();
                }
            }
        }

        private IFavorite GetSelectedFavorite()
        {
            if (dataGridFavorites.SelectedRows.Count > 0)
            {
                var viewModelItem = (FavoriteViewModel)dataGridFavorites.SelectedRows[0].DataBoundItem;
                return viewModelItem.Favorite;
            }
            return null;
        }

        private void DataGridFavorites_DoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0) // dont allow double click on column row
                EditFavorite();
        }

        private void OrganizeFavoritesForm_KeyDown(object sender, KeyEventArgs e)
        {
            switch (e.KeyCode)
            {
                case Keys.Escape:
                    this.Close();
                    break;
                case Keys.C:
                    if (e.Control)
                        CopySelectedFavorite();
                    break;
                case Keys.Delete:
                    DeleteSelectedFavorites();
                    break;
            }
        }

        private void DataGridFavorites_CellBeginEdit(object sender, DataGridViewCellCancelEventArgs e)
        {
            this.editedFavorite = this.GetSelectedFavorite();
            this.editedFavoriteName = this.editedFavorite.Name;
        }

        /// <summary>
        /// Rename favorite directly in a cell has to be confirmed into the Settings
        /// </summary>
        private void DataGridFavorites_CellEndEdit(object sender, DataGridViewCellEventArgs e)
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
            // todo duplicit Rename logic which should be merged with the renameUI command
            editedFavorite.Name = oldName; // to prevent find it self as oldFavorite
            var oldFavorite = PersistedFavorites[newName];
            // prevent conflict with another favorite than edited
            if (oldFavorite != null && !editedFavorite.StoreIdEquals(oldFavorite))
            {
                OverwriteByConflictingName(newName, oldFavorite, editedFavorite);
            }
            else
            {
                editedFavorite.Name = newName;
                // dont have to update buttons here, because they arent changed
                PersistedFavorites.Update(editedFavorite);
            }
        }

        private static void OverwriteByConflictingName(string newName, IFavorite oldFavorite, IFavorite editedFavorite)
        {
            if (!AskUserIfWantsToOverwrite(newName))
                return;

            Persistence.Instance.StartDelayedUpdate();
            // remember the edited favorite groups, because delete may also delete its groups,
            // if it is the last favorite in the group
            List<IGroup> groups = editedFavorite.Groups;
            editedFavorite.Name = newName;
            oldFavorite.UpdateFrom(editedFavorite);
            PersistedFavorites.Update(oldFavorite);
            PersistedFavorites.UpdateFavorite(oldFavorite, groups);
            PersistedFavorites.Delete(editedFavorite);
            Persistence.Instance.SaveAndFinishDelayedUpdate();
        }

        private static bool AskUserIfWantsToOverwrite(string newName)
        {
            string message = String.Format("A connection named \"{0}\" already exists\r\nDo you want to overwrite it?", newName);
            return MessageBox.Show(message, Program.Info.Title, MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes;
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
            var managedImport = new ImportWithDialogs(this, Persistence.Instance);
            bool imported = managedImport.Import(favoritesToImport);
            if (imported)
                this.UpdateFavoritesBindingSource();
        }

        private void UpdateFavoritesBindingSource()
        {
            if (this.foundFavorites.Count > 0)
                this.UpdateFavoritesBindingSource(this.foundFavorites);
            else
                UpdateFavoritesBindingSource(PersistedFavorites.ToListOrderedByDefaultSorting());
        }

        /// <summary>
        /// Replace the favorites in binding source doesnt affect performance
        /// </summary>
        private void UpdateFavoritesBindingSource(SortableList<IFavorite> newData)
        {
            SortableList<FavoriteViewModel> data = ConvertFavoritesToViewModel(newData);

            DataGridViewColumn lastSortedColumn = this.dataGridFavorites.FindLastSortedColumn();
            if (lastSortedColumn != null) // keep last ordered column
            {
                SortOrder backupGliph = lastSortedColumn.HeaderCell.SortGlyphDirection;
                this.bsFavorites.DataSource = data.SortByProperty(lastSortedColumn.DataPropertyName, backupGliph);
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
        private void DataGridFavorites_ColumnHeaderMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            DataGridViewColumn lastSortedColumn = this.dataGridFavorites.FindLastSortedColumn();
            DataGridViewColumn column = this.dataGridFavorites.Columns[e.ColumnIndex];

            SortOrder newSortDirection = SortableUnboundGrid.GetNewSortDirection(lastSortedColumn, column);
            var data = (SortableList<FavoriteViewModel>)this.bsFavorites.DataSource;
            this.bsFavorites.DataSource = data.SortByProperty(column.DataPropertyName, newSortDirection);
            column.HeaderCell.SortGlyphDirection = newSortDirection;
        }

        private void DataGridFavorites_SelectionChanged(object sender, EventArgs e)
        {
            UpdateCountLabels();
        }

        private void NewToolStripMenuItem_Click(object sender, EventArgs e)
        {
            using (var frmNewTerminal = new NewTerminalForm(Persistence.Instance, String.Empty))
            {
                if (frmNewTerminal.ShowDialog() != TerminalFormDialogResult.Cancel)
                {
                    UpdateFavoritesBindingSource();
                }
            }
        }

        private void EditConnectinoToolStripMenuItem_Click(object sender, EventArgs e)
        {
            EditFavorite();
        }

        private void EditFavorite()
        {
            IFavorite favorite = this.GetSelectedFavorite();
            if (favorite != null)
                this.EditFavorite(favorite);
        }

        private void DeleteToolStripMenuItem_Click(object sender, EventArgs e)
        {
            DeleteSelectedFavorites();
        }

        private void DeleteSelectedFavorites()
        {
            // prevent delete whole favorite when editing its name directly in grid cell
            if (this.dataGridFavorites.IsCurrentCellInEditMode)
                return;

            if (AskIfRealyDelete("favorites"))
                this.PerformDelete();
        }

        internal static bool AskIfRealyDelete(string target)
        {
            string messsage = string.Format("Do your realy want to delete selected {0}?", target);
            return MessageBox.Show(messsage, "Terminals - Delete",
                MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes;
        }

        private void PerformDelete()
        {
            this.Cursor = Cursors.WaitCursor;
            List<IFavorite> selectedFavorites = this.GetSelectedFavorites();
            PersistedFavorites.Delete(selectedFavorites);
            this.UpdateFavoritesBindingSource();
            this.Cursor = Cursors.Default;
        }

        private List<IFavorite> GetSelectedFavorites()
        {
            return this.dataGridFavorites.SelectedRows
                .Cast<DataGridViewRow>()
                .Select(row => ((FavoriteViewModel)row.DataBoundItem).Favorite)
                .ToList();
        }

        private void CopyToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (!this.dataGridFavorites.IsCurrentCellInEditMode)
                CopySelectedFavorite();
        }

        private void CopySelectedFavorite()
        {
            IFavorite favorite = this.GetSelectedFavorite();
            var copyCommand = new CopyFavoriteCommand(Persistence.Instance);
            var copy = copyCommand.Copy(favorite);
            if (copy != null)
                this.UpdateFavoritesBindingSource();
        }

        private void RenameToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (this.dataGridFavorites.SelectedRows.Count > 0)
            {
                dataGridFavorites.CurrentCell = this.dataGridFavorites.SelectedRows[0].Cells["colName"];
                this.dataGridFavorites.BeginEdit(true);
            }
        }

        private void ScanActiveDirectoryToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ImportFromAD activeDirectoryForm = new ImportFromAD();
            activeDirectoryForm.ShowDialog();
            this.UpdateFavoritesBindingSource();
        }

        private void ScanNetworkToolStripMenuItem_Click(object sender, EventArgs e)
        {
            NetworkScanner networkScanForm = new NetworkScanner();
            networkScanForm.ShowDialog();
            this.UpdateFavoritesBindingSource();
        }

        private void ScanRegistryToolStripMenuItem_Click(object sender, EventArgs e)
        {
            List<FavoriteConfigurationElement> favoritesToImport = ImportRdpRegistry.Import();
            ImportFavoritesWithManagerImport(favoritesToImport);
        }

        private void CloseToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void ImportFromFileToolStripMenuItem_Click(object sender, EventArgs e)
        {
            CallImport();
        }

        private void ExportToFileToolStripMenuItem_Click(object sender, EventArgs e)
        {
            using (var frm = new ExportForm())
                frm.ShowDialog();
        }

        private void ConnectToolStripMenuItem_Click(object sender, EventArgs e)
        {
            IFavorite favorite = this.GetSelectedFavorite();
            var definition = new ConnectionDefinition(new List<IFavorite>() { favorite });
            this.connectionsUiFactory.Connect(definition);
        }

        private void FavoritesSearchBoxFound(object sender, FavoritesFoundEventArgs e)
        {
            this.foundFavorites = new SortableList<IFavorite>(e.Favorites);
            this.UpdateFavoritesBindingSource();
        }

        private void FavoritesSearchBox_Canceled(object sender, EventArgs e)
        {
            this.foundFavorites = new SortableList<IFavorite>();
            this.UpdateFavoritesBindingSource();
        }

        private void OrganizeFavoritesForm_Load(object sender, EventArgs e)
        {
            this.favoritesSearchBox.LoadEvents();
        }

        private void OrganizeFavoritesForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            this.favoritesSearchBox.UnloadEvents();
        }
    }
}