using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace Terminals
{
    public partial class AddConnectionForm : Form
    {
        public AddConnectionForm()
        {
            InitializeComponent();
            FavoriteConfigurationElementCollection favorites = Settings.GetFavorites();
            foreach (FavoriteConfigurationElement favorite in favorites)
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