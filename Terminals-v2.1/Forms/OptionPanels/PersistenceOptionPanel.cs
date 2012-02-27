using System;
using System.Windows.Forms;
using Terminals.Configuration;

namespace Terminals.Forms
{
    internal partial class PersistenceOptionPanel : UserControl, IOptionPanel
    {
        public PersistenceOptionPanel()
        {
            InitializeComponent();
        }

        public void LoadSettings()
        {
            this.rbtnFilePersistence.Checked = Settings.PersistenceType == 0;
            this.txtConnectionString.Text = Settings.ConnectionString;
        }

        public void SaveSettings()
        {
            Settings.PersistenceType = Convert.ToByte(this.rbtnFilePersistence.Checked); 
            Settings.ConnectionString = this.txtConnectionString.Text;
        }

        private void OnRbtnFilePersistenceCheckedChanged(object sender, EventArgs e)
        {
            bool enableSqlControls = !this.rbtnFilePersistence.Checked;
            this.txtConnectionString.Enabled = enableSqlControls;
        }
    }
}
