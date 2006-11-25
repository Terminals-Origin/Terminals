using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace Terminals
{
    public partial class OptionsForm : Form
    {
        public OptionsForm()
        {
            InitializeComponent();
            cbShowInformationToolTips.Checked = Settings.ShowInformationToolTips;
            cbShowUserNameInTitle.Checked = Settings.ShowUserNameInTitle;
        }

        private void btnOk_Click(object sender, EventArgs e)
        {
            Settings.ShowInformationToolTips = cbShowInformationToolTips.Checked;
            Settings.ShowUserNameInTitle = cbShowUserNameInTitle.Checked;
        }
    }
}