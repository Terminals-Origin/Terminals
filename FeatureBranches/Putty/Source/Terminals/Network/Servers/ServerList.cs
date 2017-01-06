using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace Terminals.Network.Servers
{
    internal partial class ServerList : UserControl
    {
        public ServerList()
        {
            InitializeComponent();
        }

        private void ServerList_Load(object sender, EventArgs e)
        {
            this.dataGridView1.DataSource = null;
            Application.DoEvents();
            List<KnownServers> list = new List<KnownServers>();
            NetworkManagement.Servers servers = new NetworkManagement.Servers(NetworkManagement.ServerType.All);
            foreach (string name in servers)
            {
                NetworkManagement.ServerType type = NetworkManagement.Servers.GetServerType(name);
                KnownServers s = new KnownServers();
                s.Name = name;
                s.Type = type;
                list.Add(s);
            }
            this.dataGridView1.DataSource = list;
        }
    }

    internal class KnownServers
    {

        private string name;

        public string Name
        {
            get { return name; }
            set { name = value; }
        }

        private NetworkManagement.ServerType type;

        public NetworkManagement.ServerType Type
        {
            get { return type; }
            set { type = value; }
        }
	
    }
}
