using System;
using System.Collections.Generic;
using System.Windows.Forms;
using Terminals.Data;

namespace Terminals
{
    public partial class AddConnectionForm : Form
    {
        public AddConnectionForm()
        {
            InitializeComponent();

            foreach (Favorite favorite in Persistance.Instance.Favorites)
            {
                lvFavorites.Items.Add(favorite.Name);
            }
        }

        private List<string> connections;

        public List<string> Connections
        {
            get { return connections; }
        }

        private void lvFavorites_ItemChecked(object sender, ItemCheckedEventArgs e)
        {
            SetButtonOkState();
        }

        private void SetButtonOkState()
        {
            btnOk.Enabled = lvFavorites.CheckedItems.Count > 0;
        }

        private void txtServerName_TextChanged(object sender, EventArgs e)
        {
            SetButtonOkState();
        }

        private void btnOk_Click(object sender, EventArgs e)
        {
            connections = new List<string>();
            foreach (ListViewItem item in lvFavorites.CheckedItems)
            {
                connections.Add(item.Text);
            }
        }
    }
}