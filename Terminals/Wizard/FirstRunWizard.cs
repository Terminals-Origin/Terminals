using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace Terminals
{
    public enum WizardForms { Intro, MasterPassword, Options, Scanner }
    public partial class FirstRunWizard : Form
    {
        WizardForms SelectedForm = WizardForms.Intro;
        public FirstRunWizard()
        {
            InitializeComponent();

            rdp.OnDiscoveryCompleted += new Terminals.Wizard.AddExistingRDPConnections.DiscoveryCompleted(rdp_OnDiscoveryCompleted);

            miv = new MethodInvoker(DiscoComplete);
            
        }

        MethodInvoker miv;
        private void FirstRunWizard_Load(object sender, EventArgs e)
        {
            Wizard.IntroForm frm = new Terminals.Wizard.IntroForm();
            frm.Dock = DockStyle.Fill;
            this.panel1.Controls.Add(frm);
        }
        Wizard.AddExistingRDPConnections rdp = new Terminals.Wizard.AddExistingRDPConnections();
        Wizard.MasterPassword mp = new Terminals.Wizard.MasterPassword();
        Wizard.CommonOptions co = new Terminals.Wizard.CommonOptions();

        private void nextButton_Click(object sender, EventArgs e)
        {

            if(SelectedForm == WizardForms.Intro)
            {
                nextButton.Enabled = true;
                this.panel1.Controls.Clear();
                this.panel1.Controls.Add(mp);
                this.SelectedForm = WizardForms.MasterPassword;

            }
            else if(SelectedForm == WizardForms.MasterPassword)
            {
                if(mp.Password != "")
                {
                    Settings.TerminalsPassword = mp.Password;
                }
                nextButton.Enabled = true;
                this.panel1.Controls.Clear();
                this.panel1.Controls.Add(co);
                this.SelectedForm = WizardForms.Options;
                
            }
            else if(SelectedForm == WizardForms.Options)
            {
                Settings.MinimizeToTray = co.MinimizeToTray;
                Settings.SingleInstance = co.AllowOnlySingleInstance;
                Settings.ShowConfirmDialog = co.WarnOnDisconnect;

                nextButton.Enabled = false;
                nextButton.Text = "Finished!";
                this.panel1.Controls.Clear();
                rdp.Dock = DockStyle.Fill;
                this.panel1.Controls.Add(rdp);
                rdp.StartImport();
                this.SelectedForm = WizardForms.Scanner;

            }
            else if(SelectedForm == WizardForms.Scanner)
            {
                this.Hide();
            }
        }


        private void cancelButton_Click(object sender, EventArgs e)
        {
            rdp.CancelDiscovery();
            this.Hide();

        }
        void DiscoComplete()
        {
            nextButton.Enabled = true;
            cancelButton.Enabled = false;
            this.Hide();
            
        }
        void rdp_OnDiscoveryCompleted()
        {
            this.Invoke(miv);
        }

        private void FirstRunWizard_FormClosing(object sender, FormClosingEventArgs e)
        {
            if(rdp.DiscoFavs != null && rdp.DiscoFavs.Count > 0)
            {
                if(System.Windows.Forms.MessageBox.Show("Automatic Discovery was able to find " + rdp.DiscoFavs.Count + " connections.  Would you like to add them to your connections list?", "Terminals Confirmation", MessageBoxButtons.OKCancel) == DialogResult.OK)
                {
                    foreach(FavoriteConfigurationElement  elm in rdp.DiscoFavs)
                    {
                        Settings.AddFavorite(elm, false);
                    }
                }
            }
        }

    }
}