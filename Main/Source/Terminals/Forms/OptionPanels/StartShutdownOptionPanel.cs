using System.Windows.Forms;
using Terminals.Configuration;

namespace Terminals.Forms
{
    internal partial class StartShutdownOptionPanel : UserControl, IOptionPanel
    {
        public StartShutdownOptionPanel()
        {
            InitializeComponent();
        }

        public void LoadSettings()
        {
            this.chkSingleInstance.Checked = Settings.SingleInstance;
            this.chkNeverShowTerminalsCheckbox.Checked = Settings.NeverShowTerminalsWindow;
            this.chkShowConfirmDialog.Checked = Settings.ShowConfirmDialog;
            this.chkSaveConnections.Checked = Settings.SaveConnectionsOnClose;
        }

        public void SaveSettings()
        {
            Settings.SingleInstance = this.chkSingleInstance.Checked;
            Settings.NeverShowTerminalsWindow = this.chkNeverShowTerminalsCheckbox.Checked;
            Settings.ShowConfirmDialog = this.chkShowConfirmDialog.Checked;
            Settings.SaveConnectionsOnClose = this.chkSaveConnections.Checked;
        }
    }
}
