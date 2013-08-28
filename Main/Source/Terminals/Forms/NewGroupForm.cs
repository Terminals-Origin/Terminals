using System;
using System.Windows.Forms;

namespace Terminals
{
    internal partial class NewGroupForm : Form
    {
        /// <summary>
        /// Gets the entered name of a required group
        /// </summary>
        internal string GroupName { get { return this.txtGroupName.Text; }}

        public NewGroupForm()
        {
            InitializeComponent();
        }

        private void TxtGroupName_TextChanged(object sender, EventArgs e)
        {
            this.btnOk.Enabled = !string.IsNullOrEmpty(txtGroupName.Text);
        }
    }
}