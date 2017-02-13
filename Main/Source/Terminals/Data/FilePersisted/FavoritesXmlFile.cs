using System.Collections.Generic;
using System.Linq;
using System.Xml;
using System.Xml.Linq;
using System.Xml.XPath;

namespace Terminals.Data.FilePersisted
{
    internal class FavoritesXmlFile
    {
        private const string FAVORITESINGROUP = "//t:FavoritesInGroup";

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

        internal UnknonwPluginElements RemoveUnknownFavorites(string[] availableProtocols)
        {
            IEnumerable<XElement> favorites = this.SelectElements("//t:Favorite");
            List<XElement> unknownFavorites = favorites.Where(f => this.IsUnknownProtocol(f, availableProtocols)).ToList();
            unknownFavorites.ForEach(f => f.Remove());
            IEnumerable<XElement> groupMembership = this.SelectElements(FAVORITESINGROUP);
            Dictionary<string, List<XElement>> unknownMemberships = this.FilterGroupMembeship(groupMembership, unknownFavorites);
            return new UnknonwPluginElements(unknownFavorites, unknownMemberships);
        }

        private Dictionary<string, List<XElement>> FilterGroupMembeship(IEnumerable<XElement> favoritesInGroups, List<XElement> unknownFavorites)
        {
            string[] unknownFavoriteIds = unknownFavorites.Select(f => f.Attribute("id").Value).ToArray();
            return favoritesInGroups.ToDictionary(FindGroupId, fg => this.FilterUnknownFavoritesForGroup(fg, unknownFavoriteIds));
        }

        private List<XElement> FilterUnknownFavoritesForGroup(XElement favoritesInGroup, string[] unknownFavoriteIds)
        {
            return unknownFavoriteIds.Select(f => this.ExtractFavoriteGuid(favoritesInGroup, f)).ToList();
        }

        private XElement ExtractFavoriteGuid(XElement favoritesInGroup, string favoriteId)
        {
            var unknownFavorite = favoritesInGroup.XPathSelectElements("//t:guid", this.namespaceManager)
                .First(e => e.Value == favoriteId);
            unknownFavorite.Remove();
            return unknownFavorite;
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

        internal void AppenUnknownContent(UnknonwPluginElements unknownElements)
        {
            this.AppendUnknownFavorites(unknownElements.Favorites);
            this.AppenUnknownGroupMembership(unknownElements.GroupMembership);
        }

        private void AppenUnknownGroupMembership(Dictionary<string, List<XElement>> unknownFavoritesInGroup)
        {
            foreach (XElement favoritesInGroup in this.SelectElements(FAVORITESINGROUP))
            {
                this.AddUnknownFavoritesToGroup(unknownFavoritesInGroup, favoritesInGroup);
            }
        }

        private void AddUnknownFavoritesToGroup(Dictionary<string, List<XElement>> unknownFavoritesInGroup, XElement favoritesInGroup)
        {
            string groupId = FindGroupId(favoritesInGroup);
            List<XElement> toAdd = null;

            if (unknownFavoritesInGroup.TryGetValue(groupId, out toAdd))
            {
                // missing backslash is not a mistake: search inside the element.
                XElement favorites = this.SelectElements(favoritesInGroup, "t:Favorites").First();
                favorites.Add(toAdd);
            }
        }

        private static string FindGroupId(XElement favoritesInGroup)
        {
            return favoritesInGroup.Attribute("groupId").Value;
        }

        private void AppendUnknownFavorites(List<XElement> unknownFavorites)
        {
            XElement favorites = this.SelectElements("//t:Favorites").First();
            favorites.Add(unknownFavorites);
        }

        private IEnumerable<XElement> SelectElements(string filter)
        {
            return this.SelectElements(this.document, filter);
        }

        private IEnumerable<XElement> SelectElements(XNode target, string filter)
        {
            return target.XPathSelectElements(filter, this.namespaceManager);
        }
    }
}