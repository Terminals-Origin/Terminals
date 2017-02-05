using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using Terminals.Data.FilePersisted;

namespace Terminals.Data
{
    internal class SerializationContextBuilder
    {
        private readonly Groups groups;

        private readonly Favorites favorites;

        private readonly DataDispatcher dispatcher;

        public SerializationContextBuilder(Groups groups, Favorites favorites, DataDispatcher dispatcher)
        {
            this.groups = groups;
            this.favorites = favorites;
            this.dispatcher = dispatcher;
        }

        internal SerializationContext CreateDataFromCache(List<XElement> cachedUnknownFavorites)
        {
            var file = new FavoritesFile
            {
                Favorites = this.favorites.Cast<Favorite>().ToArray(),
                Groups = this.groups.Cast<Group>().ToArray(),
                FavoritesInGroups = this.GetFavoriteInGroups()
            };
            return new SerializationContext(file, cachedUnknownFavorites);
        }

        private FavoritesInGroup[] GetFavoriteInGroups()
        {
            List<FavoritesInGroup> references = new List<FavoritesInGroup>();
            foreach (Group group in this.groups)
            {
                FavoritesInGroup groupReferences = group.GetGroupReferences();
                references.Add(groupReferences);
            }

            return references.ToArray();
        }

        internal void AssignServices(FavoritesFile file)
        {
            this.AssignServices(file.Favorites);
            this.AssignServices(file.Groups);
        }

        private void AssignServices(Group[] fileGroups)
        {
            foreach (Group group in fileGroups)
            {
                group.AssignStores(this.groups, this.dispatcher);
            }
        }
        private void AssignServices(Favorite[] fileFavorites)
        {
            foreach (Favorite favorite in fileFavorites)
            {
                favorite.AssignStores(this.groups);
            }
        }
    }
}