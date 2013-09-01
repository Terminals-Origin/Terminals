using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using Terminals.Data;

namespace Terminals
{
    internal partial class AddConnectionForm : Form
    {
        internal List<IFavorite> SelectedFavorites { get; private set; }

        public AddConnectionForm()
        {
            InitializeComponent();

            this.gridFavorites.AutoGenerateColumns = false;
            this.gridFavorites.DataSource = Persistence.Instance.Favorites.ToListOrderedByDefaultSorting();
        }

        private void GridFavorites_SelectionChanged(object sender, EventArgs e)
        {
            btnOk.Enabled = this.gridFavorites.SelectedRows.Count > 0;
        }

        private void BtnOk_Click(object sender, EventArgs e)
        {
            var selectedRows = this.gridFavorites.SelectedRows.Cast<DataGridViewRow>();
            this.SelectedFavorites = selectedRows.Select(row => row.DataBoundItem as IFavorite)
                .ToList();
        }
    }
}