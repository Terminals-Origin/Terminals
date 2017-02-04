using System.Collections.Generic;
using System.Linq;
using System.Xml;
using System.Xml.Linq;
using System.Xml.XPath;

namespace Terminals.Data.FilePersisted
{
    internal class FavoritesXmlFile
    {
        private readonly XmlNamespaceManager namespaceManager;
        private readonly string[] availableProtocols;
        private readonly XDocument document;

        private FavoritesXmlFile(XDocument document, string[] availableProtocols)
        {
            this.document = document;
            this.availableProtocols = availableProtocols;
            this.namespaceManager = CreateNameSpaceManager();
        }

        private static XmlNamespaceManager CreateNameSpaceManager()
        {
            var nameSpaceManager = new XmlNamespaceManager(new NameTable());
            nameSpaceManager.AddNamespace("t", "http://Terminals.codeplex.com");
            nameSpaceManager.AddNamespace("xsi", "http://www.w3.org/2001/XMLSchema-instance");
            nameSpaceManager.AddNamespace("xsd", "http://www.w3.org/2001/XMLSchema");
            return nameSpaceManager;
        }

        internal static FavoritesXmlFile LoadXmlDocument(string fileLocation, string[] availableProtocols)
        {
            var document = XDocument.Load(fileLocation);
            return new FavoritesXmlFile(document, availableProtocols);
        }

        internal List<XElement> RemoveUnknownFavorites()
        {
            IEnumerable<XElement> favorites = this.document.XPathSelectElements("//t:Favorite", this.namespaceManager);
            List<XElement> toFilter = favorites.Where(this.IsUnknownProtocol).ToList();
            toFilter.ForEach(f => f.Remove());
            return toFilter;
        }

        private bool IsUnknownProtocol(XElement favoriteElement)
        {
            var protocol = favoriteElement.XPathSelectElements("t:Protocol", this.namespaceManager).First();
            return !this.availableProtocols.Contains(protocol.Value);
        }

        public XmlReader CreateReader()
        {
            return this.document.CreateReader();
        }
    }
}