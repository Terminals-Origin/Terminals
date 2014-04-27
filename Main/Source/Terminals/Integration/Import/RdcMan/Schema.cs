using System;
using System.Collections.Generic;
using System.Xml.Linq;

namespace Terminals.Integration.Import.RdcMan
{
    /// <summary>
    /// All constants and element resolution extension methods for RdcMan xml structure
    /// </summary>
    internal static class Schema
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
        private const string INHERITS = "inherit";
        private const string FROMPARENT = "FromParent";
        private const string CONNECT_TO_CONSOLE = "connectToConsole";

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

        private static XElement ResolveChildElement(this XElement currenElement, string elementName)
        {
            var found = currenElement.Element(elementName);
            if (found != null)
                return found;

            return new XElement("Dummmy");
        }

        internal static bool Inherits(this XElement currentElement)
        {
            XAttribute inheritAttribute = currentElement.Attribute(INHERITS);
            return inheritAttribute == null || inheritAttribute.Value == FROMPARENT;
        }

        internal static bool GetConnectToConsole(this XElement currenElement)
        {
            XElement element = currenElement.ResolveChildElement(CONNECT_TO_CONSOLE);
            return Convert.ToBoolean(element.Value);
        }

        internal static string GetWorkingDir(this XElement currenElement)
        {
            XElement element = currenElement.ResolveChildElement("workingDir");
            return element.Value;
        }

        internal static string GetStartProgram(this XElement currenElement)
        {
            XElement element = currenElement.ResolveChildElement("startProgram");
            return element.Value;
        }

        internal static int GetPort(this XElement currenElement)
        {
            XElement element = currenElement.ResolveChildElement("port");
            return Convert.ToInt32(element.Value);
        }

        internal static string GetUserName(this XElement currenElement)
        {
            XElement element = currenElement.ResolveChildElement("userName");
            return element.Value;
        }

        internal static string GetDomain(this XElement currenElement)
        {
            XElement element = currenElement.ResolveChildElement("domain");
            return element.Value;
        }

        internal static string GetPassword(this XElement currenElement)
        {
            XElement element = currenElement.ResolveChildElement("password");
            return element.IsClearText() ? element.Value : string.Empty;
        }
        
        private static bool IsClearText(this XElement currenElement)
        {
            var attribute = currenElement.Attribute("storeAsClearText");
            return Convert.ToBoolean(attribute.Value);
        }

        internal static bool IsEnabled(this XElement currenElement)
        {
            XElement element = currenElement.ResolveChildElement("enabled");
            return Convert.ToBoolean(element.Value);
        }
        
        internal static string GetTsGwHostName(this XElement currenElement)
        {
            XElement element = currenElement.ResolveChildElement("hostName");
            return element.Value;
        }
        
        /// <summary>
        /// values: 0=NTLM, 4= dont defined, 1=SmartCard
        /// </summary>
        internal static int GetLogonMethod(this XElement currenElement)
        {
            XElement element = currenElement.ResolveChildElement("logonMethod");
            return Convert.ToInt32(element.Value);
        }

        // Gateway properties localBypass and credSharing not covered in our application
        
        /// <summary>
        /// Gets all possible sizes as two numbers delimetd by "x" and spaces, eg. "1024 x 576".
        /// If custom is selected, the numbers dont have to fit to our enum values.
        /// </summary>
        internal static string GetSize(this XElement currenElement)
        {
            XElement element = currenElement.ResolveChildElement("size");
            return element.Value;
        }

        internal static bool GetSameSizeAsClientArea(this XElement currenElement)
        {
            XElement element = currenElement.ResolveChildElement("sameSizeAsClientArea");
            return Convert.ToBoolean(element.Value);
        }

        internal static bool GetFullScreen(this XElement currenElement)
        {
            XElement element = currenElement.ResolveChildElement("fullScreen");
            return Convert.ToBoolean(element.Value);
        }

        /// <summary>
        /// Possible values 8,15,16,24,32
        /// </summary>
        internal static int GetColorDepth(this XElement currenElement)
        {
            XElement element = currenElement.ResolveChildElement("colorDepth");
            return Convert.ToInt32(element.Value);
        }

        /// <summary>
        /// Values compatible with our values
        /// </summary>
        internal static int GetAudioRedirection(this XElement currenElement)
        {
            XElement element = currenElement.ResolveChildElement("audioRedirection");
            return Convert.ToInt32(element.Value);
        }

        // Redirect audioRedirectionQuality, audioCaptureRedirection properties not covered in our application

        /// <summary>
        /// values: local=0, Remote=1, InFullScreen=2
        /// </summary>
        internal static int GetKeyboardHook(this XElement currenElement)
        {
            XElement element = currenElement.ResolveChildElement("keyboardHook");
            return Convert.ToInt32(element.Value);
        }

        internal static bool GetRedirectClipboard(this XElement currenElement)
        {
            XElement element = currenElement.ResolveChildElement("redirectClipboard");
            return Convert.ToBoolean(element.Value);
        }

        internal static bool GetRedirectPorts(this XElement currenElement)
        {
            XElement element = currenElement.ResolveChildElement("redirectPorts");
            return Convert.ToBoolean(element.Value);
        }

        internal static bool GetRedirectPrinters(this XElement currenElement)
        {
            XElement element = currenElement.ResolveChildElement("redirectPrinters");
            return Convert.ToBoolean(element.Value);
        }

        internal static bool GetRedirectSmartCards(this XElement currenElement)
        {
            XElement element = currenElement.ResolveChildElement("redirectSmartCards");
            return Convert.ToBoolean(element.Value);
        }
        
    }
}
