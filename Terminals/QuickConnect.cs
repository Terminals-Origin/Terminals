using System;
using System.Windows.Forms;
using Terminals.Configuration;

namespace Terminals {
    public partial class QuickConnect : Form {
        public QuickConnect() {
            InitializeComponent();

            LoadFavorites();
        }

        private void LoadFavorites() {
            FavoriteConfigurationElementCollection favorites = Settings.GetFavorites();
            cmbServerList.Items.Clear();
            foreach(FavoriteConfigurationElement favorite in favorites) {
                cmbServerList.Items.Add(favorite.Name);
            }
        }
        public string ConnectionName {
            get {
                return cmbServerList.Text;
            }
        }

        private void ConnectButton_Click(object sender, EventArgs e) {
            this.DialogResult = DialogResult.OK;
            this.Close();
        }

        private void CancelButton_Click(object sender, EventArgs e) {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }
    }
}