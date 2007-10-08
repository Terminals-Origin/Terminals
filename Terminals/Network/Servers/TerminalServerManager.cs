using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;

namespace Terminals.Network.Servers
{
    public partial class TerminalServerManager : UserControl
    {
        public TerminalServerManager()
        {
            InitializeComponent();
        }

        TerminalServices.TerminalServer server;
        private void button1_Click(object sender, EventArgs e)
        {
            server = TerminalServices.TerminalServer.LoadServer(this.ServerNameTextBox.Text);
            dataGridView1.DataSource = server.Sessions;
            dataGridView1.Columns[1].Visible = false;
        }

        private void dataGridView1_RowEnter(object sender, DataGridViewCellEventArgs e)
        {
            if(dataGridView1.DataSource != null)
            {
                SelectedSession= server.Sessions[e.RowIndex];
                this.propertyGrid1.SelectedObject = SelectedSession.Client;
            }
        }
        TerminalServices.Session SelectedSession = null;
        private void TerminalServerManager_Load(object sender, EventArgs e)
        {

        }

        private void sendMessageToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Terminals.InputBoxResult result = Terminals.InputBox.Show("Please enter the message to send..");
            if(result.ReturnCode == DialogResult.OK && result.Text.Trim()!=null)
            {
                TerminalServices.TerminalServicesAPI.SendMessage(SelectedSession, "Message from your Administrator", result.Text.Trim(), 0, 10, false);
            }
        }

        private void logoffSessionToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if(System.Windows.Forms.MessageBox.Show("Are you sure you want to log off the selected session?", "Confirmation Required", MessageBoxButtons.OKCancel, MessageBoxIcon.Exclamation) == DialogResult.OK)
            {
                TerminalServices.TerminalServicesAPI.LogOffSession(SelectedSession, false);
            }
        }

        private void rebootServerToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if(System.Windows.Forms.MessageBox.Show("Are you sure you want to reboot this server?", "Confirmation Required", MessageBoxButtons.OKCancel, MessageBoxIcon.Exclamation) == DialogResult.OK)
            {
                TerminalServices.TerminalServicesAPI.ShutdownSystem(this.server, true);
            }
        }

        private void shutdownServerToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if(System.Windows.Forms.MessageBox.Show("Are you sure you want to shutdown this server?", "Confirmation Required", MessageBoxButtons.OKCancel, MessageBoxIcon.Exclamation) == DialogResult.OK)
            {
                TerminalServices.TerminalServicesAPI.ShutdownSystem(this.server, false);
            }
        }
    }
}
