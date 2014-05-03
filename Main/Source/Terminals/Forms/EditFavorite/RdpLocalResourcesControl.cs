using System;
using System.Windows.Forms;

namespace Terminals.Forms.EditFavorite
{
    public partial class RdpLocalResourcesControl : UserControl
    {
        public RdpLocalResourcesControl()
        {
            InitializeComponent();
        }

        private void BtnBrowseShare_Click(object sender, EventArgs e)
        {
            //using (FolderBrowserDialog dialog = new FolderBrowserDialog())
            //{
            //    dialog.Description = "Select Desktop Share:";
            //    dialog.ShowNewFolderButton = false;
            //    dialog.SelectedPath = @"\\" + this.cmbServers.Text;
            //    if (dialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            //        this.txtDesktopShare.Text = dialog.SelectedPath;
            //}
        }

        private void BtnDrives_Click(object sender, EventArgs e)
        {
            //DiskDrivesForm drivesForm = new DiskDrivesForm(this);
            //drivesForm.ShowDialog(this);
        }
    }
}
