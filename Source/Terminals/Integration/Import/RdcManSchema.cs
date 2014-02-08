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

        internal static IEnumerable<XElement> GetServerElements(this XElement currenElement)
        {
            return currenElement.Elements(SERVER);
        }
        
        internal static XElement GetVersionElement(this XElement currenElement)
        {
            return ResolveChildElement(currenElement, VERSION);
        }

        internal static XElement GetFileElement(this XElement currenElement)
        {
            return ResolveChildElement(currenElement, FILE);
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
