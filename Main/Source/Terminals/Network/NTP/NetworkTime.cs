using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;

namespace Terminals.Network.NTP
{
    public partial class NetworkTime : UserControl
    {
        public NetworkTime()
        {
            InitializeComponent();
        }

        private void LookupButton_Click(object sender, EventArgs e)
        {
            this.propertyGrid1.SelectedObject = null;
            Application.DoEvents();
            Unified.Network.SNTP.NTPClient client = null;
            string server = TimeServerTextBox.Text;
            if(server!="" && server!=Unified.Network.SNTP.NTPClient.DefaultTimeServer)                
                client = Unified.Network.SNTP.NTPClient.GetTime(server);
            else
                client = Unified.Network.SNTP.NTPClient.GetTime();

            this.propertyGrid1.SelectedObject = client;
        }

        private void UpdateButton_Click(object sender, EventArgs e)
        {
            this.propertyGrid1.SelectedObject = null;
            Application.DoEvents();
            Unified.Network.SNTP.NTPClient client = null;
            string server = TimeServerTextBox.Text;
            if (server != "" && server != Unified.Network.SNTP.NTPClient.DefaultTimeServer)
                client = Unified.Network.SNTP.NTPClient.GetTime(server);
            else
                client = Unified.Network.SNTP.NTPClient.GetTime();


            this.propertyGrid1.SelectedObject = client;

        }

        private void NetworkTime_Load(object sender, EventArgs e)
        {
            this.TimeServerTextBox.Text = Unified.Network.SNTP.NTPClient.DefaultTimeServer;
            
        }
    }
}
