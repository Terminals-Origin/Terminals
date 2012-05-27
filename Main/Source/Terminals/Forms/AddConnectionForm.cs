using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using Terminals.Data;

namespace Terminals
{
    internal partial class AddConnectionForm : Form
    {
        public AddConnectionForm()
        {
            InitializeComponent();

            this.gridFavorites.AutoGenerateColumns = false;
            this.gridFavorites.DataSource = Persistence.Instance.Favorites.ToListOrderedByDefaultSorting();
        }

        private List<IFavorite> selectedFavorites;
        internal List<IFavorite> SelectedFavorites
        {
            get { return this.selectedFavorites; }
        }
        
        private void gridFavorites_SelectionChanged(object sender, EventArgs e)
        {
            btnOk.Enabled = this.gridFavorites.SelectedRows.Count > 0;
        }

        private void btnOk_Click(object sender, EventArgs e)
        {
            var selectedRows = this.gridFavorites.SelectedRows.Cast<DataGridViewRow>();
            this.selectedFavorites = selectedRows.Select(row => row.DataBoundItem as IFavorite)
                .ToList();
        }

    }
}