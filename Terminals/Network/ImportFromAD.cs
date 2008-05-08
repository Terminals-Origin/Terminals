using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace Terminals.Network
{
    public partial class ImportFromAD : Form
    {
        public ImportFromAD()
        {
            InitializeComponent();
        }

        private void ImportFromAD_Load(object sender, EventArgs e)
        {
            listComplete = new MethodInvoker(UpdateComputerList);
            this.progressBar1.Visible = false;
            if(Settings.DefaultDomain != null && Settings.DefaultDomain != "")
            {
                this.domainTextbox.Text = Settings.ToTitleCase(Settings.DefaultDomain);
            }
            else
            {
                this.domainTextbox.Text = Settings.ToTitleCase(System.Environment.UserDomainName);
            }
        }
        MethodInvoker listComplete;

        private void ScanADButton_Click(object sender, EventArgs e)
        {
            this.progressBar1.Visible = true;
            Network.ActiveDirectoryClient adClient = new ActiveDirectoryClient();
            adClient.OnListComputersDoneDelegate += new ActiveDirectoryClient.ListComputersDoneDelegate(adClient_OnListComputersDoneDelegate);
            adClient.ListComputers(Settings.ToTitleCase(this.domainTextbox.Text));
        }
        private void UpdateComputerList()
        {
            this.dataGridView1.DataSource = Computers;
            this.progressBar1.Visible = false;
            if(!this.Success)
            {
                System.Windows.Forms.MessageBox.Show("Could not connect to the Domain: " + this.domainTextbox.Text);
            }
        }
        List<ActiveDirectoryComputer> Computers;
        bool Success;
        void adClient_OnListComputersDoneDelegate(List<ActiveDirectoryComputer> Computers, bool Success)
        {
            this.Computers = Computers;
            this.Success = Success;
            try
            {
                this.Invoke(listComplete);
            }
            catch(Exception exc)
            {
                Terminals.Logging.Log.Info("Could not call invoke on AD Client List, this probably means they closed the form before waiting for a response", exc);
            }
        }

        private void CancelButton_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void OkButton_Click(object sender, EventArgs e)
        {
            //do the import here
            List<ActiveDirectoryComputer> Computers = (List<ActiveDirectoryComputer>)this.dataGridView1.DataSource;
            foreach(ActiveDirectoryComputer computer in Computers)
            {
                FavoriteConfigurationElement elm = new FavoriteConfigurationElement(computer.ComputerName);
                elm.Name = computer.ComputerName;
                elm.ServerName = computer.ComputerName;
                elm.UserName = System.Environment.UserName;
                elm.DomainName = this.domainTextbox.Text;
                elm.Tags = computer.Tags;                
                elm.Port = Connections.ConnectionManager.GetPort(computer.Protocol);
                elm.Protocol = computer.Protocol;
                elm.Notes = computer.Notes;
                Settings.AddFavorite(elm, false);

            }
            this.Close();
        }

        private void button1_Click(object sender, EventArgs e) {
            foreach(ActiveDirectoryComputer computer in Computers) {
                computer.Import = true;                
            }
            this.dataGridView1.DataSource = Computers;
        }

        private void button2_Click(object sender, EventArgs e) {
            foreach(ActiveDirectoryComputer computer in Computers) {
                computer.Import = false;
            }
            this.dataGridView1.DataSource = Computers;
        }
    }
}
