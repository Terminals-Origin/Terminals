using System;
using System.Windows.Forms;

namespace Terminals
{
    internal partial class SaveActiveConnectionsForm : Form
    {
        public SaveActiveConnectionsForm()
        {
            InitializeComponent();
            Height = 160;
        }

        private void MoreButton_Click(object sender, EventArgs e)
        {
            if (Height == 160)
            {
                MoreButton.Text = "Less...";
                Height = 230;
            }
            else
            {
                MoreButton.Text = "More...";
                Height = 160;
            }
        }
    }
}