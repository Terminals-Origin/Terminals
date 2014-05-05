using System;
using System.Collections;
using System.Collections.Generic;
using System.Windows.Forms;
using FalafelSoftware.TransPort;
using Terminals.Connections;

namespace Terminals.Forms.EditFavorite
{
    public partial class RasControl : UserControl, IProtocolOptionsControl
    {
        private Dictionary<string, RASENTRY> dialupList = new Dictionary<string, RASENTRY>();

        internal List<string> ConnectionNames { get; private set; }

        public RasControl()
        {
            InitializeComponent();

            this.ConnectionNames = new List<string>();
        }

        internal void OnServerNameChanged(string serverName)
        {
            if (serverName == ConnectionManager.RAS)
                this.FillRasControls(serverName);
        }

        internal void FillRasControls(string serverName)
        {
            this.LoadDialupConnections();
            this.RASDetailsListBox.Items.Clear();
            if (this.dialupList != null && this.dialupList.ContainsKey(serverName))
            {
                RASENTRY selectedRAS = this.dialupList[serverName];
                this.RASDetailsListBox.Items.Add(String.Format("{0}:{1}", "Connection", serverName));
                this.RASDetailsListBox.Items.Add(String.Format("{0}:{1}", "Area Code", selectedRAS.AreaCode));
                this.RASDetailsListBox.Items.Add(String.Format("{0}:{1}", "Country Code", selectedRAS.CountryCode));
                this.RASDetailsListBox.Items.Add(String.Format("{0}:{1}", "Device Name", selectedRAS.DeviceName));
                this.RASDetailsListBox.Items.Add(String.Format("{0}:{1}", "Device Type", selectedRAS.DeviceType));
                this.RASDetailsListBox.Items.Add(String.Format("{0}:{1}", "Local Phone Number", selectedRAS.LocalPhoneNumber));
            }
        }

        public void SetControls()
        {
            this.LoadDialupConnections();
            this.RASDetailsListBox.Items.Clear();
        }

        private void LoadDialupConnections()
        {
            this.dialupList = new Dictionary<String, RASENTRY>();
            this.ConnectionNames.Clear();
            var rasEntries = new ArrayList();
            ras1.ListEntries(ref rasEntries);
            foreach (String item in rasEntries)
            {
                RASENTRY details = new RASENTRY();
                ras1.GetEntry(item, ref details);
                this.dialupList.Add(item, details);
                this.ConnectionNames.Add(item);
            }
        }
    }
}
