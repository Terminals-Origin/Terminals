using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace Terminals
{
    public partial class SaveActiveConnectionsForm : Form
    {
        public SaveActiveConnectionsForm()
        {
            InitializeComponent();
            Height = 160;
        }

        private void chkShowOptions_CheckedChanged(object sender, EventArgs e)
        {
            if (chkShowOptions.Checked)
            {
                Height = 160;
            }
            else
            {
                Height = 230;
            }
        }
    }
}