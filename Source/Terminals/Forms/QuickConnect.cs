using System;
using System.Linq;
using System.Windows.Forms;
using Terminals.Data;

namespace Terminals
{
    internal partial class QuickConnect : Form
    {
        public string ConnectionName
        {
            get
            {
                return this.inputTextbox.Text;
            }
        }

        public QuickConnect()
        {
            InitializeComponent();
            LoadFavorites();
            this.inputTextbox.Focus();
        }

        private void LoadFavorites()
        {
            IFavorites favorites = Persistence.Instance.Favorites;
            string[] favoriteNames = favorites.Select(f => f.Name).ToArray();
            this.inputTextbox.AutoCompleteCustomSource = new AutoCompleteStringCollection();
            this.inputTextbox.AutoCompleteCustomSource.AddRange(favoriteNames);
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

        private void TestKeys(KeyEventArgs e)
        {
            if (e.KeyCode == Keys.F3)
            {
                this.DialogResult = DialogResult.No;
                this.Close();
            }
        }
        private void QuickConnect_KeyUp(object sender, KeyEventArgs e)
        {
            TestKeys(e);
        }

        private void InputTextbox_KeyUp(object sender, KeyEventArgs e)
        {
            TestKeys(e);
        }

        private void ButtonConnect_KeyUp(object sender, KeyEventArgs e)
        {
            TestKeys(e);
        }

        private void ButtonCancel_KeyUp(object sender, KeyEventArgs e)
        {
            TestKeys(e);
        }
    }
}