using System.Windows.Forms;
using Terminals.Configuration;

namespace Terminals.Forms
{
    internal partial class StartShutdownOptionPanel : UserControl, IOptionPanel
    {
        private readonly Settings settings = Settings.Instance;

        public StartShutdownOptionPanel()
        {
            InitializeComponent();
        }

        public void LoadSettings()
        {
            this.chkSingleInstance.Checked = settings.SingleInstance;
            this.chkNeverShowTerminalsCheckbox.Checked = settings.NeverShowTerminalsWindow;
            this.chkShowConfirmDialog.Checked = settings.ShowConfirmDialog;
            this.chkSaveConnections.Checked = settings.SaveConnectionsOnClose;
        }

        public void SaveSettings()
        {
            settings.SingleInstance = this.chkSingleInstance.Checked;
            settings.NeverShowTerminalsWindow = this.chkNeverShowTerminalsCheckbox.Checked;
            settings.ShowConfirmDialog = this.chkShowConfirmDialog.Checked;
            settings.SaveConnectionsOnClose = this.chkSaveConnections.Checked;
        }
    }
}
