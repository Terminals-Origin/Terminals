using System;
using System.Collections.Generic;
using System.Windows.Forms;
using Terminals.Data;

namespace Terminals
{
    internal partial class AddConnectionForm : Form
    {
        private readonly IPersistence persistence;

        internal List<IFavorite> SelectedFavorites { get; private set; }

        public AddConnectionForm(IPersistence persistence)
        {
            this.persistence = persistence;
            InitializeComponent();
        }

        private void SearchPanel_ResultsSelectionChanged(object sender, ListViewItemSelectionChangedEventArgs e)
        {
            btnOk.Enabled = this.searchPanel.SelectedFavorites.Count > 0;
        }

        private void BtnOk_Click(object sender, EventArgs e)
        {
            this.SelectedFavorites = this.searchPanel.SelectedFavorites;
        }

        private void AddConnectionForm_Load(object sender, EventArgs e)
        {
            this.searchPanel.LoadEvents(this.persistence);
        }

        private void AddConnectionForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            this.searchPanel.UnLoadEvents();
        }
    }
}