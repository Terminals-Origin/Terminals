using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using FalafelSoftware.TransPort;

namespace Terminals.Forms.EditFavorite
{
    public partial class RasControl : UserControl
    {
        private Dictionary<string, RASENTRY> dialupList = new Dictionary<string, RASENTRY>();

        public RasControl()
        {
            InitializeComponent();
        }

        private void LoadDialupConnections()
        {
            //this.dialupList = new Dictionary<String, RASENTRY>();
            //var rasEntries = new ArrayList();
            //ras1.ListEntries(ref rasEntries);
            //foreach (String item in rasEntries)
            //{
            //    RASENTRY details = new RASENTRY();
            //    ras1.GetEntry(item, ref details);
            //    this.dialupList.Add(item, details);

            //    if (!cmbServers.Items.Contains(item))
            //        this.cmbServers.Items.Add(item);
            //}
        }
    }
}
