using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using Terminals;
using Terminals.Network;

namespace Terminals.Network.Servers
{
    public partial class TerminalServerManager : UserControl
    {
        public TerminalServerManager()
        {
            InitializeComponent();
        }

        public void ForceTSAdmin(string Host)
        {
            this.ServerNameComboBox.Text = Host;
            this.button1_Click(null, null);
        }
        TerminalServices.TerminalServer server;
        private void button1_Click(object sender, EventArgs e)
        {
            SelectedSession = null;
            dataGridView1.DataSource = null;
            dataGridView2.DataSource = null;
            this.propertyGrid1.SelectedObject = null;
            Application.DoEvents();
            server = TerminalServices.TerminalServer.LoadServer(this.ServerNameComboBox.Text);
            if(server.IsATerminalServer)
            {
                dataGridView1.DataSource = server.Sessions;
                dataGridView1.Columns[1].Visible = false;
            }
            else
            {
                System.Windows.Forms.MessageBox.Show("This machine does not appear to be a Terminal Server");
            }

        }

        private void dataGridView1_RowEnter(object sender, DataGridViewCellEventArgs e)
        {
            if(server.IsATerminalServer)
            {
                if(dataGridView1.DataSource != null)
                {
                    SelectedSession = server.Sessions[e.RowIndex];
                    this.propertyGrid1.SelectedObject = SelectedSession.Client;
                    this.dataGridView2.DataSource = SelectedSession.Processes;
                }
            }
            
        }
        
        public void Connect(string server, bool headless)
        {
            try
            {
                splitContainer1.Panel1Collapsed = headless;
                if(server != "")
                {
                    this.ServerNameComboBox.Text = server;
                    button1_Click(null, null);
                }
            }
            catch(Exception exc)
            {
                Terminals.Logging.Log.Info("Connection Failure.", exc);
            }
        }
        TerminalServices.Session SelectedSession = null;
        private void TerminalServerManager_Load(object sender, EventArgs e)
        {
            ServerNameComboBox.Items.Clear();
            foreach(FavoriteConfigurationElement elm in Settings.GetFavorites())
            {
                if(elm.Protocol == "RDP")
                {
                    this.ServerNameComboBox.Items.Add(elm.ServerName);
                }
            }
        }


        private void sendMessageToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if(SelectedSession != null)
            {
                Terminals.InputBoxResult result = Terminals.InputBox.Show("Please enter the message to send..");
                if(result.ReturnCode == DialogResult.OK && result.Text.Trim() != null)
                {
                    TerminalServices.TerminalServicesAPI.SendMessage(SelectedSession, "Message from your Administrator", result.Text.Trim(), 0, 10, false);
                }
            }
        }

        private void logoffSessionToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if(SelectedSession != null)
            {
                if(System.Windows.Forms.MessageBox.Show("Are you sure you want to log off the selected session?", "Confirmation Required", MessageBoxButtons.OKCancel, MessageBoxIcon.Exclamation) == DialogResult.OK)
                {
                    TerminalServices.TerminalServicesAPI.LogOffSession(SelectedSession, false);
                }
            }
        }
        private void rebootServerToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if(server.IsATerminalServer)
            {
                if(System.Windows.Forms.MessageBox.Show("Are you sure you want to reboot this server?", "Confirmation Required", MessageBoxButtons.OKCancel, MessageBoxIcon.Exclamation) == DialogResult.OK)
                {
                    TerminalServices.TerminalServicesAPI.ShutdownSystem(this.server, true);
                }
            }
        }
        private void shutdownServerToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if(server.IsATerminalServer)
            {
                if(System.Windows.Forms.MessageBox.Show("Are you sure you want to shutdown this server?", "Confirmation Required", MessageBoxButtons.OKCancel, MessageBoxIcon.Exclamation) == DialogResult.OK)
                {
                    TerminalServices.TerminalServicesAPI.ShutdownSystem(this.server, false);
                }
            }
        }
    }
}