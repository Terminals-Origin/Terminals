using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using Terminals.Configuration;
using Terminals.Connections;
using Terminals.Data;
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
        private readonly IPersistence persistence;
        private readonly Settings settings = Settings.Instance;
        private WizardForms SelectedForm = WizardForms.Intro;
        private MethodInvoker miv;
        private AddExistingRDPConnections rdp = new AddExistingRDPConnections();
        private MasterPassword mp = new MasterPassword();
        private CommonOptions co = new CommonOptions();
        private DefaultCredentials dc = new DefaultCredentials();

        private readonly ConnectionManager connectionManager;

        public FirstRunWizard(IPersistence persistence, ConnectionManager connectionManager)
        {
            InitializeComponent();
            rdp.OnDiscoveryCompleted += new AddExistingRDPConnections.DiscoveryCompleted(rdp_OnDiscoveryCompleted);
            miv = new MethodInvoker(DiscoComplete);
            this.persistence = persistence;
            this.connectionManager = connectionManager;
            this.mp.AssignPersistence(persistence);
        }

        private void FirstRunWizard_Load(object sender, EventArgs e)
        {
            IntroForm frm = new IntroForm();
            frm.Dock = DockStyle.Fill;
            this.panel1.Controls.Add(frm);
            settings.StartDelayedUpdate();
        }

        private void NextButton_Click(object sender, EventArgs e)
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
            try
            {
                ApplySettings();
                StartImportIfRequested();
            }
            catch (Exception exc)
            {
                Logging.Error("Apply settings in the first run wizard failed.", exc);
            }
        }

        private void StartImportIfRequested()
        {
            if (this.co.ImportRDPConnections)
            {
                this.nextButton.Enabled = false;
                this.nextButton.Text = "Finished!";
                this.panel1.Controls.Clear();
                this.rdp.Dock = DockStyle.Fill;
                this.panel1.Controls.Add(this.rdp);
                this.rdp.StartImport(this.connectionManager);
                this.SelectedForm = WizardForms.Scanner;
            }
            else
            {
                this.rdp.CancelDiscovery();
                this.Hide();
            }
        }

        private void ApplySettings()
        {
            settings.MinimizeToTray = this.co.MinimizeToTray;
            settings.SingleInstance = this.co.AllowOnlySingleInstance;
            settings.ShowConfirmDialog = this.co.WarnOnDisconnect;
            settings.EnableCaptureToClipboard = this.co.EnableCaptureToClipboard;
            settings.EnableCaptureToFolder = this.co.EnableCaptureToFolder;
            settings.AutoSwitchOnCapture = this.co.AutoSwitchOnCapture;

            if (this.co.LoadDefaultShortcuts)
                settings.SpecialCommands = SpecialCommandsWizard.LoadSpecialCommands();
        }

        private void SwitchToOptionsFromCredentials()
        {
            settings.DefaultDomain = this.dc.DefaultDomain;
            settings.DefaultPassword = this.dc.DefaultPassword;
            settings.DefaultUsername = this.dc.DefaultUsername;

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
            this.persistence.Security.UpdateMasterPassword(this.mp.Password);
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

        private void CancelButton_Click(object sender, EventArgs e)
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
            this.settings.ShowWizard = false;
            this.settings.SaveAndFinishDelayedUpdate();
            ImportDiscoveredFavorites();
        }

        private void ImportDiscoveredFavorites()
        {
            if (this.rdp.DiscoveredConnections.Count > 0)
            {
                String message = String.Format("Automatic Discovery was able to find {0} connections.\r\n" +
                  "Would you like to add them to your connections list?",
                  this.rdp.DiscoveredConnections.Count);
                if (MessageBox.Show(message, "Terminals Confirmation", MessageBoxButtons.YesNo) == DialogResult.Yes)
                {
                    List<FavoriteConfigurationElement> favoritesToImport = this.rdp.DiscoveredConnections.ToList();
                    var managedImport = new ImportWithDialogs(this, this.persistence, this.connectionManager);
                    managedImport.Import(favoritesToImport);
                }
            }
        }
    }
}