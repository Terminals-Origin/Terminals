using System;
using System.Windows.Forms;
using Terminals.Configuration;

namespace Terminals.Wizard
{
    internal partial class CommonOptions : UserControl
    {
        private readonly Settings settings = Settings.Instance;

        public CommonOptions()
        {
            InitializeComponent();
            this.MinimizeCheckbox.Checked = settings.MinimizeToTray;
            this.SingleCheckbox.Checked = settings.SingleInstance;
            this.WarnCheckbox.Checked = settings.WarnOnConnectionClose;
            this.CaptureToClipboard.Checked = settings.EnableCaptureToClipboard;
            this.CaptureToFolder.Checked = settings.EnableCaptureToFolder;
            this.autoSwitchOnCapture.Checked = settings.AutoSwitchOnCapture;
            this.autoSwitchOnCapture.Enabled = settings.EnableCaptureToFolder;
        }

        public bool MinimizeToTray
        {
            get { return this.MinimizeCheckbox.Checked; }
        }

        public bool AllowOnlySingleInstance
        {
            get { return this.SingleCheckbox.Checked; }
        }

        public bool WarnOnDisconnect
        {
            get { return this.WarnCheckbox.Checked; }
        }

        public bool LoadDefaultShortcuts
        {
            get { return this.LoadDefaultShortcutsCheckbox.Checked; }
        }

        public bool ImportRDPConnections
        {
            get { return this.ImportRDP.Checked; }
        }

        public bool EnableCaptureToClipboard
        {
            get { return this.CaptureToClipboard.Checked; }
        }

        public bool EnableCaptureToFolder
        {
            get { return this.CaptureToFolder.Checked; }
        }

        public bool AutoSwitchOnCapture
        {
            get { return this.autoSwitchOnCapture.Checked; }
        }

        private void CaptureToFolder_CheckedChanged(object sender, EventArgs e)
        {
            this.autoSwitchOnCapture.Enabled = this.CaptureToFolder.Checked;
        }
    }
}
