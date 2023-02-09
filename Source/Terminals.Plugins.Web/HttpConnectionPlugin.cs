using System;
using System.Drawing;
using System.Windows.Forms;
using Terminals.Common.Connections;
using Terminals.Data;
using Terminals.Plugins.Web;
using Terminals.Plugins.Web.Properties;
using Terminals.Forms.EditFavorite;
using Terminals.Integration.Export;

namespace Terminals.Connections.Web
{
    internal class HttpConnectionPlugin : IConnectionPlugin, IOptionsExporterFactory, IOptionsConverterFactory
    {
        internal static readonly Image TreeIconHttp = Resources.treeIcon_http;
        internal const string WEB_HTTP = "HTTP";

        public int Port { get { return KnownConnectionConstants.HTTPPort; } }

        public string PortName { get { return KnownConnectionConstants.HTTP; } }

        public Connection CreateConnection()
        {
            return new HTTPConnection();
        }

        public Control[] CreateOptionsControls()
        {
            return new Control[] { new WebControl() { Name = WEB_HTTP } };
        }

        public Type GetOptionsType()
        {
            return typeof (WebOptions);
        }

        public ProtocolOptions CreateOptions()
        {
            return new WebOptions();
        }

        public Image GetIcon()
        {
            return TreeIconHttp;
        }

        public IOptionsConverter CreatOptionsConverter()
        {
            return new WebOptionsConverter();
        }

        public ITerminalsOptionsExport CreateOptionsExporter()
        {
            return new TerminalsWebExport();
        }
    }
}
