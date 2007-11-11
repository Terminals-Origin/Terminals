using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace Terminals
{
    public enum WizardForms { Intro, MasterPassword, DefaultCredentials, Options, Scanner }
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
        Wizard.DefaultCredentials dc = new Terminals.Wizard.DefaultCredentials();

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
                if(mp.StorePassword)
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
            else if(SelectedForm == WizardForms.DefaultCredentials)
            {
                Settings.DefaultDomain = dc.DefaultDomain;
                Settings.DefaultPassword = dc.DefaultPassword;
                Settings.DefaultUsername = dc.DefaultUsername;

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
                Settings.AutoSwitchOnCapture = co.AutoSwitchOnCapture;

                try {
                    if(co.LoadDefaultShortcuts) {
                        SpecialCommandConfigurationElementCollection cmdList = Settings.SpecialCommands;
                        //add the command prompt
                        SpecialCommandConfigurationElement elm = new SpecialCommandConfigurationElement("Command Shell");
                        elm.Executable = @"%systemroot%\system32\cmd.exe";
                        cmdList.Add(elm);

                        System.IO.DirectoryInfo systemroot = new System.IO.DirectoryInfo(System.Environment.GetFolderPath(System.Environment.SpecialFolder.System));

                        string regEditFile = System.IO.Path.Combine(systemroot.FullName, "regedt32.exe");
                        Icon[] regeditIcons = IconHandler.IconHandler.IconsFromFile(regEditFile, IconHandler.IconSize.Small);
                        SpecialCommandConfigurationElement regEditElm = new SpecialCommandConfigurationElement(regEditFile);
                        if(regeditIcons != null && regeditIcons.Length > 0) {
                            if(!System.IO.Directory.Exists("Thumbs")) System.IO.Directory.CreateDirectory("Thumbs");
                            string thumbName = string.Format(@"Thumbs\regedt32.exe.jpg", regEditFile);
                            if(!System.IO.File.Exists(thumbName)) regeditIcons[0].ToBitmap().Save(thumbName);
                            regEditElm.Thumbnail = thumbName;
                        }

                        //elm1.Thumbnail = "";
                        regEditElm.Executable = regEditFile;
                        cmdList.Add(regEditElm);

                        Icon[] IconsList = IconHandler.IconHandler.IconsFromFile(System.IO.Path.Combine(systemroot.FullName, "mmc.exe"), IconHandler.IconSize.Small);
                        System.Random rnd = new Random();
                        foreach(System.IO.FileInfo file in systemroot.GetFiles("*.msc")) {
                            SpecialCommandConfigurationElement elm1 = new SpecialCommandConfigurationElement(file.Name);

                            if(IconsList != null && IconsList.Length > 0) {
                                
                                if(!System.IO.Directory.Exists("Thumbs")) System.IO.Directory.CreateDirectory("Thumbs");
                                string thumbName = string.Format(@"Thumbs\{0}.jpg", file.Name);
                                if(!System.IO.File.Exists(thumbName)) IconsList[rnd.Next(IconsList.Length - 1)].ToBitmap().Save(thumbName);
                                elm1.Thumbnail = thumbName;
                            }

                            //elm1.Thumbnail = "";
                            elm1.Executable = @"%systemroot%\system32\" + file.Name;
                            cmdList.Add(elm1);
                        }

                        Settings.SpecialCommands = cmdList;



                    }
                } catch(Exception exc) {
                    Terminals.Logging.Log.Error("Loading default shortcuts in the wizard.", exc);
                }

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