using System;
using System.Drawing;
using System.Windows.Forms;
using Terminals.Common.Connections;
using Terminals.Connections;
using Terminals.Data;
using Terminals.Plugins.TeamViewer.Properties;

namespace Terminals.Plugins.WinBox
{
    internal class TeamViewerConnectionPlugin : IConnectionPlugin, IOptionsConverterFactory
    {
        internal static readonly Image TreeIconTeamViewer = Resources.treeIcon_TeamViewer;

        internal const int WinBox_Port = 1337;

        internal const string TeamViewer = "TeamViewer";

        public int Port { get { return WinBox_Port; } }

        public string PortName { get { return TeamViewer; } }

        public Connection CreateConnection()
        {
            return new TeamViewerConnection();
        }

        public Control[] CreateOptionsControls()
        {
            return new Control[0];
        }

        public Type GetOptionsType()
        {
            return typeof(TeamViewerOptions);
        }

        public ProtocolOptions CreateOptions()
        {
            return new TeamViewerOptions();
        }

        public Image GetIcon()
        {
            return TreeIconTeamViewer;
        }

        public IOptionsConverter CreatOptionsConverter()
        {
            return new TeamViewerOptionsConverter();
        }
    }
}
