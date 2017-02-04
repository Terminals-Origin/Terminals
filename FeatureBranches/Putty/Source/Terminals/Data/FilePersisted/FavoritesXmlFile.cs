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
        private readonly XDocument document;

        private FavoritesXmlFile(XDocument document)
        {
            this.document = document;
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

        internal static FavoritesXmlFile LoadXmlDocument(string fileLocation)
        {
            var document = XDocument.Load(fileLocation);
            return CreateDocument(document);
        }

        internal static FavoritesXmlFile CreateDocument(XDocument source)
        {
            return new FavoritesXmlFile(source);
        }

        internal List<XElement> RemoveUnknownFavorites(string[] availableProtocols)
        {
            IEnumerable<XElement> favorites = this.SelectElements("//t:Favorite");
            List<XElement> toFilter = favorites.Where(f => this.IsUnknownProtocol(f, availableProtocols)).ToList();
            toFilter.ForEach(f => f.Remove());
            return toFilter;
        }

        private bool IsUnknownProtocol(XElement favoriteElement, string[] availableProtocols)
        {
            var protocol = favoriteElement.XPathSelectElements("t:Protocol", this.namespaceManager).First();
            return !availableProtocols.Contains(protocol.Value);
        }

        internal XmlReader CreateReader()
        {
            return this.document.CreateReader();
        }

        internal void AppendUnknownFavorites(List<XElement> unknownFavorites)
        {
            XElement favorites = this.SelectElements("//t:Favorites").First();
            favorites.Add(unknownFavorites);
        }

        private IEnumerable<XElement> SelectElements(string filter)
        {
            return this.document.XPathSelectElements(filter, this.namespaceManager);
        }
    }
}