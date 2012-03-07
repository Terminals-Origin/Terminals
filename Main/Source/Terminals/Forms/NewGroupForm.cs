using System;
using System.Windows.Forms;

namespace Terminals
{
    public partial class NewGroupForm : Form
    {
        public NewGroupForm()
        {
            InitializeComponent();
        }

        private void txtGroupName_TextChanged(object sender, EventArgs e)
        {
            btnOk.Enabled = !string.IsNullOrEmpty(txtGroupName.Text);
        }
    }
}