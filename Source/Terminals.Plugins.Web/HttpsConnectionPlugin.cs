using System;
using System.Drawing;
using System.Windows.Forms;
using Terminals.Common.Connections;
using Terminals.Data;
using Terminals.Forms.EditFavorite;
using Terminals.Integration.Export;
using Terminals.Plugins.Web;
using Terminals.Plugins.Web.Properties;

namespace Terminals.Connections.Web
{
    internal class HttpsConnectionPlugin: IConnectionPlugin, IOptionsExporterFactory, IOptionsConverterFactory
    {
        internal static readonly Image TreeIconHttps = Resources.treeIcon_http;
        internal const string WEB_HTTPS = "HTTPS";

        public int Port { get { return KnownConnectionConstants.HTTPSPort; } }

        public string PortName { get { return KnownConnectionConstants.HTTPS; } }

        public Connection CreateConnection()
        {
            return new HTTPConnection();
        }

        public Control[] CreateOptionsControls()
        {
            return new Control[] { new WebControl() { Name = WEB_HTTPS } };
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
            return TreeIconHttps;
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
