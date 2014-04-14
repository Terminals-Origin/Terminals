using System.Xml.Linq;

namespace Terminals.Integration.Import
{
    internal class ConnectionSettings : RdcManSettings<ConnectionSettings>
    {
        internal string StartDir
        {
            get 
            {
                return this.Inherited ? this.Parent.StartDir : this.PropertiesElement.GetWorkingDir();
            }
        }

        internal string StartProgram
        {
            get
            {
                return this.Inherited ? this.Parent.StartProgram : this.PropertiesElement.GetStartProgram();
            }
        }

        internal bool ConnectToConsole
        {
            get
            {
                return this.Inherited ? this.Parent.ConnectToConsole : this.PropertiesElement.GetConnectToConsole();
            }
        }

        internal int Port
        {
            get
            {
                return this.Inherited ? this.Parent.Port : this.PropertiesElement.GetPort();
            }
        }

        public ConnectionSettings(XElement settingsElement, ConnectionSettings parent = null)
            : base(settingsElement, parent)
        {
        }
    }
}