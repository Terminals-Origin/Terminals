using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using Terminals.Configuration;
using Terminals.Forms.Controls;
using Terminals.Wizard;

namespace Terminals
{
    internal enum WizardForms 
    { 
        Intro, 
        MasterPassword, 
        DefaultCredentials, 
        Options, 
        Scanner
    }

    internal partial class FirstRunWizard : Form
    {
        private WizardForms SelectedForm = WizardForms.Intro;
        private MethodInvoker miv;
        private AddExistingRDPConnections rdp = new AddExistingRDPConnections();
        private MasterPassword mp = new MasterPassword();
        private CommonOptions co = new CommonOptions();
        private DefaultCredentials dc = new DefaultCredentials();

        public FirstRunWizard()
        {
            InitializeComponent();
            rdp.OnDiscoveryCompleted += new AddExistingRDPConnections.DiscoveryCompleted(rdp_OnDiscoveryCompleted);
            miv = new MethodInvoker(DiscoComplete);
        }

        private void FirstRunWizard_Load(object sender, EventArgs e)
        {
            IntroForm frm = new IntroForm();
            frm.Dock = DockStyle.Fill;
            this.panel1.Controls.Add(frm);
        }

        private void nextButton_Click(object sender, EventArgs e)
        {
            if (SelectedForm == WizardForms.Intro)
            {
                SwitchToMasterPassword();
            }
            else if (SelectedForm == WizardForms.MasterPassword)
            {
                if (mp.StorePassword)
                {
                    SwitchToDefaultCredentials();
                }
                else
                {
                    SwitchToOptions();
                }
            }
            else if (SelectedForm == WizardForms.DefaultCredentials)
            {
                SwitchToOptionsFromCredentials();
            }
            else if (SelectedForm == WizardForms.Options)
            {
                FinishOptions();
            }
            else if (SelectedForm == WizardForms.Scanner)
            {
                this.Hide();
            }
        }

        private void FinishOptions()
        {
            Settings.MinimizeToTray = this.co.MinimizeToTray;
            Settings.SingleInstance = this.co.AllowOnlySingleInstance;
            Settings.ShowConfirmDialog = this.co.WarnOnDisconnect;
            Settings.EnableCaptureToClipboard = this.co.EnableCaptureToClipboard;
            Settings.EnableCaptureToFolder = this.co.EnableCaptureToFolder;
            Settings.AutoSwitchOnCapture = this.co.AutoSwitchOnCapture;

            try
            {
                if (this.co.LoadDefaultShortcuts)
                    Settings.SpecialCommands = SpecialCommandsWizard.LoadSpecialCommands();                   
            }
            catch(Exception exc)
            {
                Logging.Log.Error("Loading default shortcuts in the wizard.", exc);
            }

            if (this.co.ImportRDPConnections)
            {
                this.nextButton.Enabled = false;
                this.nextButton.Text = "Finished!";
                this.panel1.Controls.Clear();
                this.rdp.Dock = DockStyle.Fill;
                this.panel1.Controls.Add(this.rdp);
                this.rdp.StartImport();
                this.SelectedForm = WizardForms.Scanner;
            }
            else
            {
                this.rdp.CancelDiscovery();
                this.Hide();
            }
        }

        private void SwitchToOptionsFromCredentials()
        {
            Settings.DefaultDomain = this.dc.DefaultDomain;
            Settings.DefaultPassword = this.dc.DefaultPassword;
            Settings.DefaultUsername = this.dc.DefaultUsername;

            this.nextButton.Enabled = true;
            this.panel1.Controls.Clear();
            this.panel1.Controls.Add(this.co);
            this.SelectedForm = WizardForms.Options;
        }

        private void SwitchToOptions()
        {
            this.nextButton.Enabled = true;
            this.panel1.Controls.Clear();
            this.panel1.Controls.Add(this.co);
            this.SelectedForm = WizardForms.Options;
        }

        private void SwitchToDefaultCredentials()
        {
            Settings.UpdateMasterPassword(this.mp.Password);
            this.nextButton.Enabled = true;
            this.panel1.Controls.Clear();
            this.panel1.Controls.Add(this.dc);
            this.SelectedForm = WizardForms.DefaultCredentials;
        }

        private void SwitchToMasterPassword()
        {
            this.nextButton.Enabled = true;
            this.panel1.Controls.Clear();
            this.panel1.Controls.Add(this.mp);
            this.SelectedForm = WizardForms.MasterPassword;
        }

        private void cancelButton_Click(object sender, EventArgs e)
        {
            rdp.CancelDiscovery();
            this.Hide();
        }

        private void DiscoComplete()
        {
            nextButton.Enabled = true;
            cancelButton.Enabled = false;
            this.Hide();
        }

        private void rdp_OnDiscoveryCompleted()
        {
            this.Invoke(miv);
        }

        private void FirstRunWizard_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (rdp.DiscoveredConnections.Count > 0)
            {
                String message = String.Format("Automatic Discovery was able to find {0} connections.\r\n" +
                                               "Would you like to add them to your connections list?",
                                                rdp.DiscoveredConnections.Count);
                if (MessageBox.Show(message, "Terminals Confirmation", MessageBoxButtons.YesNo) == DialogResult.Yes)
                {
                    List<FavoriteConfigurationElement> favoritesToImport = rdp.DiscoveredConnections.ToList();
                    var managedImport = new ImportWithDialogs(this, false);
                    managedImport.Import(favoritesToImport);
                }
            }
        }
    }
}