using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;

namespace Terminals.Network
{
    public partial class LocalConnections : UserControl
    {
        public LocalConnections()
        {
            InitializeComponent();

        }
        private void timer1_Tick(object sender, EventArgs e)
        {
            Metro.TransportLayer.Tcp.TcpConnection[] connections = Metro.TransportLayer.Tcp.TcpConnectionManager.GetCurrentTcpConnections();
            //this.dataGridView1.DataSource = null;
            this.dataGridView1.DataSource = connections;
        }

        private void LocalConnections_Load(object sender, EventArgs e)
        {
            this.dataGridView1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.None;
            timer1_Tick(null, null);
            timer1.Enabled = true;
            timer1.Start();
        }
    }
}
