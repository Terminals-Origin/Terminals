using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
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
            btnOk.Enabled = txtGroupName.Text != String.Empty;
        }
    }
}