using System.Xml.Linq;

namespace Terminals.Integration.Import
{
    internal class ConnectionSettings : RdcManSettings<ConnectionSettings>
    {
        internal string StartDir
        {
            get 
            {
                return this.PropertiesElement.GetWorkingDir();
            }
        }

        internal string StartProgram
        {
            get
            {
                return this.PropertiesElement.GetStartProgram();
            }
        }

        internal bool ConnectToConsole
        {
            get
            {
                return this.PropertiesElement.GetConnectToConsole();
            }
        }

        internal int Port
        {
            get
            {
                return this.PropertiesElement.GetPort();
            }
        }

        public ConnectionSettings(XElement settingsElement, ConnectionSettings parent = null)
            : base(settingsElement, parent)
        {
        }
    }
}