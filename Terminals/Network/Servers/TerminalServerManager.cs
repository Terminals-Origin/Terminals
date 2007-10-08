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

        private void button1_Click(object sender, EventArgs e)
        {
            Terminals.TerminalServices.TerminalServer s = new Terminals.TerminalServices.TerminalServer(this.textBox1.Text);
            this.label1.Text = s.Connect().ToString();
            string f = "";
            s.DisConnect();
        }
    }
}
