using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;

namespace Terminals.Network
{
    public partial class InterfacesList : UserControl
    {
        public InterfacesList()
        {
            InitializeComponent();
        }
        
        private void InterfacesList_Load(object sender, EventArgs e)
        {
            this.dataGridView1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.None;
            timer1_Tick(null, null);
            this.timer1.Enabled = true;
            this.timer1.Start();
            
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            Metro.NetworkInterfaceList nil = new Metro.NetworkInterfaceList();
            this.dataGridView1.DataSource = null;
            this.dataGridView1.DataSource = nil.Interfaces;
        }
    }
}
