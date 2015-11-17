using System;
using System.Windows.Forms;
using Terminals.Connections;
using Terminals.Data;
using Terminals.Forms;
using Terminals.TerminalServices;

namespace Terminals.Network.Servers
{
    internal partial class TerminalServerManager : UserControl
    {
        private Session selectedSession = null;
        private TerminalServer server;
        private String hostName;

        public TerminalServerManager()
        {
            InitializeComponent();
        }

        public String HostName
        {
            get { return this.hostName; }
            set
            {
                this.hostName = value;
                this.ServerNameComboBox.Text = this.hostName;
            }
        }

        private IPersistence persistence;

        internal void AssignPersistence(IPersistence persistence)
        {
            this.persistence = persistence;
        }

        public void ForceTSAdmin(string host)
        {
            this.ServerNameComboBox.Text = host;
            this.ConnectButton_Click(null, null);
        }

        public void Connect(string server, bool headless)
        {
            try
            {
                splitContainer1.Panel1Collapsed = headless;
                if (server != String.Empty)
                {
                    this.ServerNameComboBox.Text = server;
                    this.ConnectButton_Click(null, null);
                }
            }
            catch (Exception exc)
            {
                Logging.Error("Connection Failure.", exc);
            }
        }

        private void ConnectButton_Click(object sender, EventArgs e)
        {
            if (this.ParentForm != null)
                this.ParentForm.Cursor = Cursors.WaitCursor;

            this.selectedSession = null;
            dataGridView1.DataSource = null;
            dataGridView2.DataSource = null;
            this.propertyGrid1.SelectedObject = null;
            Application.DoEvents();
            server = TerminalServer.LoadServer(this.ServerNameComboBox.Text);

            try
            {
                 if (server.IsATerminalServer)
                {
                    dataGridView1.DataSource = server.Sessions;
                    dataGridView1.Columns[1].Visible = false;
                }
                else
                {
                    MessageBox.Show("This machine does not appear to be a Terminal Server");
                }
            }
            catch (Exception)
            {
                // Do nothing when error
            }


            if (this.ParentForm != null)
                this.ParentForm.Cursor = Cursors.Default;
        }

        private void DataGridView1_RowEnter(object sender, DataGridViewCellEventArgs e)
        {
            if(server.IsATerminalServer)
            {
                if(dataGridView1.DataSource != null)
                {
                    this.selectedSession = server.Sessions[e.RowIndex];
                    this.propertyGrid1.SelectedObject = this.selectedSession.Client;
                    this.dataGridView2.DataSource = this.selectedSession.Processes;
                }
            }
        }
        
        private void TerminalServerManager_Load(object sender, EventArgs e)
        {
            ServerNameComboBox.Items.Clear();
            foreach (IFavorite favorite in this.persistence.Favorites)
            {
                if (favorite.Protocol == KnownConnectionConstants.RDP)
                {
                    this.ServerNameComboBox.Items.Add(favorite.ServerName);
                }
            }
        }

        private void SendMessageToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SendMessageToSession(this.selectedSession);
        }

        internal static void SendMessageToSession(Session session)
        {
            if (session != null)
            {
                string prompt = Program.Resources.GetString("Pleaseenterthemessagetosend");
                InputBoxResult result = InputBox.Show(prompt, "Send network message");
                if (result.ReturnCode == DialogResult.OK && !string.IsNullOrEmpty(result.Text))
                {
                    string meessageText = result.Text.Trim();
                    string messageHeader = Program.Resources.GetString("MessagefromyourAdministrator");
                    TerminalServicesAPI.SendMessage(session, messageHeader, meessageText, 0, 10, false);
                }
            }
        }

        private void LogoffSessionToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if(this.selectedSession != null)
            {
                if(MessageBox.Show("Are you sure you want to log off the selected session?", "Confirmation Required", MessageBoxButtons.OKCancel, MessageBoxIcon.Exclamation) == DialogResult.OK)
                {
                    TerminalServicesAPI.LogOffSession(this.selectedSession, false);
                }
            }
        }

        private void RebootServerToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if(server.IsATerminalServer)
            {
                if(MessageBox.Show("Are you sure you want to reboot this server?", "Confirmation Required", MessageBoxButtons.OKCancel, MessageBoxIcon.Exclamation) == DialogResult.OK)
                {
                    TerminalServicesAPI.ShutdownSystem(this.server, true);
                }
            }
        }

        private void ShutdownServerToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if(server.IsATerminalServer)
            {
                if(MessageBox.Show("Are you sure you want to shutdown this server?", "Confirmation Required", MessageBoxButtons.OKCancel, MessageBoxIcon.Exclamation) == DialogResult.OK)
                {
                    TerminalServicesAPI.ShutdownSystem(this.server, false);
                }
            }
        }

        private void ServerNameComboBox_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                this.ConnectButton_Click(null, null);
            }
        }
    }
}