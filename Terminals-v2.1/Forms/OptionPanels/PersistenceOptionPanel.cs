using System;
using System.Windows.Forms;
using Terminals.Configuration;
using Terminals.Data.DB;

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
            if (String.IsNullOrEmpty(Settings.ConnectionString))
                this.txtConnectionString.Text = DataBase.DEVELOPMENT_CONNECTION_STRING;
            else
                this.txtConnectionString.Text = Settings.ConnectionString;
        }

        public void SaveSettings()
        {
            Settings.PersistenceType = Convert.ToByte(this.rbtnSqlPersistence.Checked);
            if (this.txtConnectionString.Enabled)
                Settings.ConnectionString = this.txtConnectionString.Text;
        }

        private void OnRbtnFilePersistenceCheckedChanged(object sender, EventArgs e)
        {
            bool enableSqlControls = !this.rbtnFilePersistence.Checked;
            this.txtConnectionString.Enabled = enableSqlControls;
        }
    }
}
