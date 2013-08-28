using System;
using System.Windows.Forms;

namespace Terminals
{
    internal partial class NewGroupForm : Form
    {
        private NewGroupForm()
        {
            InitializeComponent();
        }

        private void TxtGroupName_TextChanged(object sender, EventArgs e)
        {
            this.btnOk.Enabled = !string.IsNullOrEmpty(txtGroupName.Text);
        }

        /// <summary>
        /// Shows this dialog to the user asking for group name.
        /// If user confirms, the entered value is returned; otherwiser returns empty string.
        /// </summary>
        internal static string AskFroGroupName()
        {
            using (var frmNewGroup = new NewGroupForm())
            {
                if (frmNewGroup.ShowDialog() == DialogResult.OK)
                    return frmNewGroup.txtGroupName.Text;

                return string.Empty;
            }
        }
    }
}