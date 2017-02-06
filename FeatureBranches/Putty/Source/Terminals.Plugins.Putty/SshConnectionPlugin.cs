using System;
using System.Drawing;
using System.Windows.Forms;
using Terminals.Common.Connections;
using Terminals.Connections;
using Terminals.Data;
using Terminals.Integration.Export;
using Terminals.Plugins.Putty.Properties;

namespace Terminals.Plugins.Putty
{
    internal class SshConnectionPlugin : IConnectionPlugin, IOptionsConverterFactory, IOptionsExporterFactory
    {
        internal const int SSHPort = 22;
        internal const string SSH = "SSH";

        public int Port { get { return SSHPort; } }

        public string PortName { get {return SSH; } }

        internal static readonly Image TreeIconSsh = Resources.treeIcon_ssh;


        public Connection CreateConnection()
        {
            return new PuttyConnection();
        }

        public ProtocolOptions CreateOptions()
        {
            return new SshOptions();
        }

        public Control[] CreateOptionsControls()
        {
            return new Control[] { new SshOptionsControl() { Name = "SSH" } };
        }

        public IOptionsConverter CreatOptionsConverter()
        {
            return new SshOptionsConverter();
        }

        public Image GetIcon()
        {
            return TreeIconSsh; 
        }

        public Type GetOptionsType()
        {
            return typeof(SshOptions);
        }

        public ITerminalsOptionsExport CreateOptionsExporter()
        {
            return new TerminalsSshExport();
        }
    }
}
