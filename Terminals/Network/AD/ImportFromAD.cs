using System;
using System.Windows.Forms;
using Terminals.Configuration;

namespace Terminals.Network
{
    internal partial class ImportFromAD : Form
    {
        private ActiveDirectoryClient adClient;

        public ImportFromAD()
        {
            InitializeComponent();
            this.gridComputers.AutoGenerateColumns = false;

            adClient = new ActiveDirectoryClient();
            adClient.ListComputersDone += new ListComputersDoneDelegate(this.AdClient_OnListComputersDone);
            adClient.ComputerFound += new ComputerFoundDelegate(this.OnClientComputerFound);
            this.gridComputers.DataSource = this.bsComputers;
        }

        private void ImportFromAD_Load(object sender, EventArgs e)
        {
            this.progressBar1.Visible = false;
            this.lblProgressStatus.Text = String.Empty;

            if (!String.IsNullOrEmpty(Settings.DefaultDomain))
            {
                this.domainTextbox.Text = Settings.DefaultDomain;
            }
            else
            {
                this.domainTextbox.Text = Environment.UserDomainName;
            }
        }

        private void ScanADButton_Click(object sender, EventArgs e)
        {
            if (!this.adClient.IsRunning)
            {
                this.bsComputers.Clear();
                adClient.FindComputers(this.domainTextbox.Text);
                this.lblProgressStatus.Text = "Contacting domain...";
                this.SwitchToRunningMode();
            }
            else
            {
                adClient.Stop();
                this.lblProgressStatus.Text = "Canceling scan...";
            }
        }

        private void SwitchToRunningMode()
        {
            this.progressBar1.Visible = true;
            this.ButtonScanAD.Text = "Stop";
            this.btnSelectAll.Enabled = false;
            this.btnSelectNone.Enabled = false;
            this.ButtonImport.Enabled = false;
        }

        private void SwitchToStoppedMode()
        {
            this.progressBar1.Visible = false;
            this.ButtonScanAD.Text = "Scan";
            this.btnSelectAll.Enabled = true;
            this.btnSelectNone.Enabled = true;
            this.ButtonImport.Enabled = true;
        }

        private void OnClientComputerFound(ActiveDirectoryComputer computer)
        {
            if (this.InvokeRequired)
            {
                this.Invoke(new ComputerFoundDelegate(this.OnClientComputerFound), new object[] { computer });
            }
            else
            {
                this.bsComputers.Add(computer);
                this.lblProgressStatus.Text = String.Format("Scaning... {0} computers found.", this.bsComputers.Count);
                this.gridComputers.Refresh();
            }
        }

        private void AdClient_OnListComputersDone(bool success)
        {
            if (this.InvokeRequired)
            {
                this.Invoke(new ListComputersDoneDelegate(AdClient_OnListComputersDone), new object[] { success });
            }
            else
            {
                if (success)
                {
                    this.lblProgressStatus.Text = String.Format("Scan complete, {0} computers found.", this.bsComputers.Count);
                }
                else
                {
                    this.lblProgressStatus.Text = "Scan canceled.";
                }

                SwitchToStoppedMode();
            }
        }

        private void CancelButton_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void OnButtonImportClick(object sender, EventArgs e)
        {
            this.Cursor = Cursors.WaitCursor;
            foreach (ActiveDirectoryComputer computer in this.bsComputers)
            {
                if (computer.Import)
                {
                    FavoriteConfigurationElement elm = computer.ToFavorite(this.domainTextbox.Text);
                    Settings.AddFavorite(elm, false);
                }
            }
            this.Cursor = Cursors.Default;
        }

        private void OnBtnSelectAllClick(object sender, EventArgs e)
        {
            SwitchImportFlagForAllComputers(true);
        }

        private void OnBtnSelectNoneClick(object sender, EventArgs e)
        {
            SwitchImportFlagForAllComputers(false);
        }

        private void SwitchImportFlagForAllComputers(Boolean import)
        {
            foreach (ActiveDirectoryComputer computer in this.bsComputers)
            {
                computer.Import = import;
            }
            this.gridComputers.Refresh();
        }

        private void ImportFromAD_FormClosing(object sender, FormClosingEventArgs e)
        {
            adClient.ListComputersDone -= new ListComputersDoneDelegate(this.AdClient_OnListComputersDone);
            adClient.ComputerFound -= new ComputerFoundDelegate(this.OnClientComputerFound);
            adClient.Stop();
        }
    }
}
