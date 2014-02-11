using System.Collections.Generic;
using System.Xml.Linq;

namespace Terminals.Integration.Import
{
    /// <summary>
    /// All constants and element resolution extension methods for RdcMan xml structure
    /// </summary>
    internal static class RdcManSchema
    {
        private const string VERSION = "version";
        private const string FILE = "file";
        private const string SERVER = "server";
        private const string GROUP = "group";
        private const string PROPERTIES = "properties";
        private const string DISPLAYNAME = "displayName";
        private const string NAME = "name";
        private const string COMMENT = "comment";
        private const string LOGON_CREDENTIALS = "logonCredentials";
        private const string CONNECTION_SETTINGS = "connectionSettings";
        private const string GATEWAY_SETTINGS = "gatewaySettings";
        private const string REMOTE_DESKTOP = "remoteDesktop";
        private const string LOCAL_RESOURCES = "localResources";
        private const string SECURITY_SETTINGS = "securitySettings";
        private const string DISPLAY_SETTINGS = "displaySettings";

        internal static IEnumerable<XElement> GetGroupElements(this XElement currenElement)
        {
            return currenElement.Elements(GROUP);
        }

        internal static IEnumerable<XElement> GetServerElements(this XElement currenElement)
        {
            return currenElement.Elements(SERVER);
        }
        
        internal static XElement GetVersionElement(this XElement currenElement)
        {
            return currenElement.ResolveChildElement(VERSION);
        }

        internal static XElement GetFileElement(this XElement currenElement)
        {
            return currenElement.ResolveChildElement(FILE);
        }

        internal static XElement GetPropertiesElement(this XElement currenElement)
        {
            return currenElement.ResolveChildElement(PROPERTIES);
        }
        
        internal static XElement GetDisplayNameElement(this XElement currenElement)
        {
            return currenElement.ResolveChildElement(DISPLAYNAME);
        }

        internal static XElement GetNameElement(this XElement currenElement)
        {
            return currenElement.ResolveChildElement(NAME);
        }

        internal static XElement GetCommentElement(this XElement currenElement)
        {
            return currenElement.ResolveChildElement(COMMENT);
        }

        internal static XElement GetLogonCredentialsElement(this XElement currenElement)
        {
            return currenElement.ResolveChildElement(LOGON_CREDENTIALS);
        }

        internal static XElement GetConnectionSettingsElement(this XElement currenElement)
        {
            return currenElement.ResolveChildElement(CONNECTION_SETTINGS);
        }

        internal static XElement GetGatewaySettingsElement(this XElement currenElement)
        {
            return currenElement.ResolveChildElement(GATEWAY_SETTINGS);
        }

        internal static XElement GetRemoteDesktopElement(this XElement currenElement)
        {
            return currenElement.ResolveChildElement(REMOTE_DESKTOP);
        }

        internal static XElement GetLocalResourcesElement(this XElement currenElement)
        {
            return currenElement.ResolveChildElement(LOCAL_RESOURCES);
        }

        internal static XElement GetSecuritySettingsElement(this XElement currenElement)
        {
            return currenElement.ResolveChildElement(SECURITY_SETTINGS);
        }

        internal static XElement GetDisplaySettingsElement(this XElement currenElement)
        {
            return currenElement.ResolveChildElement(DISPLAY_SETTINGS);
        }

        private static XElement ResolveChildElement(this XElement currenElement, string elementName)
        {
            var found = currenElement.Element(elementName);
            if (found != null)
                return found;

            return new XElement("Dummmy");
        }
    }
}
