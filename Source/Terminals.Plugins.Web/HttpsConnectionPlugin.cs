using System;
using System.Drawing;
using System.Windows.Forms;
using Terminals.Common.Connections;
using Terminals.Data;
using Terminals.Forms.EditFavorite;
using Terminals.Integration.Export;
using Terminals.Plugins.Web;

namespace Terminals.Connections.Web
{
    internal class HttpsConnectionPlugin: IConnectionPlugin, IOptionsExporterFactory, IOptionsConverterFactory
    {
        internal const int HTTPSPort = 443;

        public int Port { get { return HTTPSPort; } }

        public string PortName { get { return KnownConnectionConstants.HTTPS; } }

        public Connection CreateConnection()
        {
            return new HTTPConnection();
        }

        public Control[] CreateOptionsControls()
        {
            return new Control[] { new WebControl() { Name = "HTTPS" } };
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
            return HttpConnectionPlugin.TreeIconHttp;
        }

        public IOptionsConverter CreatOptionsConverter()
        {
            return new WebOptionsConverter();
        }

        public ITerminalsOptionsExport CreateOptionsExporter()
        {
            return new TerminalsHTTPSExport();
        }
    }
}
