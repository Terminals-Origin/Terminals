using System;
using System.Xml.Linq;
using Terminals.Connections;

namespace Terminals.Integration.Import.RdcMan
{
    internal class ConnectionSettings : Settings<ConnectionSettings>
    {
        internal string StartDir
        {
            get 
            {
                Func<string> getParentValue = () => this.Parent.StartDir;
                Func<string> getElementValue = this.PropertiesElement.GetWorkingDir;
                return this.ResolveValue(getParentValue, getElementValue, string.Empty);
            }
        }

        internal string StartProgram
        {
            get
            {
                Func<string> getParentValue = () => this.Parent.StartProgram;
                Func<string> getElementValue = this.PropertiesElement.GetStartProgram;
                return this.ResolveValue(getParentValue, getElementValue, string.Empty);
            }
        }

        internal bool ConnectToConsole
        {
            get
            {
                Func<bool> getParentValue = () => this.Parent.ConnectToConsole;
                Func<bool> getElementValue = this.PropertiesElement.GetConnectToConsole;
                return this.ResolveValue(getParentValue, getElementValue);
            }
        }

        internal int Port
        {
            get
            {
                Func<int> getParentValue = () => this.Parent.Port;
                Func<int> getElementValue = this.PropertiesElement.GetPort;
                return this.ResolveValue(getParentValue, getElementValue, ConnectionManager.RDPPort);
            }
        }

        public ConnectionSettings(XElement settingsElement, ConnectionSettings parent = null)
            : base(settingsElement, parent)
        {
        }
    }
}