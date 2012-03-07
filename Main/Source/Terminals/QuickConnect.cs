using System;
using System.Linq;
using System.Windows.Forms;
using Terminals.Data;

namespace Terminals
{
    public partial class QuickConnect : Form
    {
        public QuickConnect()
        {
            InitializeComponent();
            LoadFavorites();
        }

        private void LoadFavorites()
        {
            cmbServerList.Items.Clear();
            var favorites = Persistance.Instance.Favorites;
            var favoriteNames = favorites.Select(favorite => favorite.Name).ToArray();
            cmbServerList.Items.AddRange(favoriteNames);
        }

        public string ConnectionName
        {
            get
            {
                return cmbServerList.Text;
            }
        }

        private void ConnectButton_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.OK;
            this.Close();
        }

        private void CancelButton_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }
    }
}