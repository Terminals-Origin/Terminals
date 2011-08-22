using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using Terminals.Configuration;
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
                nextButton.Enabled = true;
                this.panel1.Controls.Clear();
                this.panel1.Controls.Add(mp);
                this.SelectedForm = WizardForms.MasterPassword;
            }
            else if (SelectedForm == WizardForms.MasterPassword)
            {
                if (mp.StorePassword)
                {
                    Settings.TerminalsPassword = mp.Password;
                    nextButton.Enabled = true;
                    this.panel1.Controls.Clear();
                    this.panel1.Controls.Add(dc);
                    this.SelectedForm = WizardForms.DefaultCredentials;
                }
                else
                {
                    nextButton.Enabled = true;
                    this.panel1.Controls.Clear();
                    this.panel1.Controls.Add(co);
                    this.SelectedForm = WizardForms.Options;
                }
            }
            else if (SelectedForm == WizardForms.DefaultCredentials)
            {
                Settings.DefaultDomain = dc.DefaultDomain;
                Settings.DefaultPassword = dc.DefaultPassword;
                Settings.DefaultUsername = dc.DefaultUsername;

                nextButton.Enabled = true;
                this.panel1.Controls.Clear();
                this.panel1.Controls.Add(co);
                this.SelectedForm = WizardForms.Options;                
            }
            else if (SelectedForm == WizardForms.Options)
            {
                Settings.MinimizeToTray = co.MinimizeToTray;
                Settings.SingleInstance = co.AllowOnlySingleInstance;
                Settings.ShowConfirmDialog = co.WarnOnDisconnect;
                Settings.EnableCaptureToClipboard = co.EnableCaptureToClipboard;
                Settings.EnableCaptureToFolder = co.EnableCaptureToFolder;
                Settings.AutoSwitchOnCapture = co.AutoSwitchOnCapture;

                try
                {
                    if (co.LoadDefaultShortcuts)
                        Settings.SpecialCommands = SpecialCommandsWizard.LoadSpecialCommands();                   
                }
                catch(Exception exc)
                {
                    Logging.Log.Error("Loading default shortcuts in the wizard.", exc);
                }

                if (co.ImportRDPConnections)
                {
                    nextButton.Enabled = false;
                    nextButton.Text = "Finished!";
                    this.panel1.Controls.Clear();
                    rdp.Dock = DockStyle.Fill;
                    this.panel1.Controls.Add(rdp);
                    rdp.StartImport();
                    this.SelectedForm = WizardForms.Scanner;
                }
                else
                {
                    rdp.CancelDiscovery();
                    this.Hide();
                }
            }
            else if (SelectedForm == WizardForms.Scanner)
            {
                this.Hide();
            }
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
            if (rdp.DiscoFavs != null && rdp.DiscoFavs.Count > 0)
            {
                String message = String.Format("Automatic Discovery was able to find {0} connections.\r\nWould you like to add them to your connections list?",
                                                rdp.DiscoFavs.Count);
                if (MessageBox.Show(message, "Terminals Confirmation", MessageBoxButtons.OKCancel) == DialogResult.OK)
                {
                    List<FavoriteConfigurationElement> favoritesToImport = rdp.DiscoFavs.ToList();
                    Settings.AddFavorites(favoritesToImport, false);
                }
            }
        }
    }
}