using System;
using System.Collections.Generic;
using System.Drawing;

namespace Terminals.Data
{
    /// <summary>
    /// Part of application data store, which handles favorites collection
    /// </summary>
    internal interface IFavorites : IEnumerable<IFavorite>
    {
        /// <summary>
        /// Gets the stored favorite identified by its unique identifier.
        /// If no favorites exists with provided identifier, than returns null.
        /// </summary>
        IFavorite this[Guid favoriteId] { get; }
        
        /// <summary>
        /// Gets favorite by its name. If there are more than one with this name returns the first found.
        /// If there is no favorite with such name, returns null. Search isn't case sensitive.
        /// Use this only to identify, if favorite with required name isn't already present,
        /// to prevent name duplicities.
        /// </summary>
        IFavorite this[string favoriteName] { get; }
        
        /// <summary>
        /// Adds favorite in the persistence, if no favorite with its identifier exists.
        /// Saves changes, if delayed save isn't required.
        /// </summary>
        void Add(IFavorite favorite);

        /// <summary>
        /// Adds all favorites in the persistence, which identifier doesn't exist in current collection yet.
        /// Saves changes, if delayed save isn't required.
        /// </summary>
        void Add(List<IFavorite> favorites);

        /// <summary>
        /// Replace favorite with the present identifier in persistence with the one send as parameter.
        /// Saves changes, if delayed save isn't required.
        /// This expects, that user didn't change favorite groups.
        /// </summary>
        void Update(IFavorite favorite);

        /// <summary>
        /// Updates favorite and also updates it in all groups defined by groups collection.
        /// If favorite is present in group not listed in groupNames it will be removed,
        /// if it isn't present, but is listed in groupNames, then it will be add to the group.
        /// Saves changes, if delayed save isn't required.
        /// </summary>
        /// <param name="favorite">Not null item to update, which holds old groups yet.</param>
        /// <param name="groups">New collection of groups in which the favorite should be listed.</param>
        void UpdateFavorite(IFavorite favorite, List<IGroup> groups);

        /// <summary>
        /// Removes the favorite from persistence, if it is present.
        /// and remove it also from all its groups.
        /// Saves changes, if delayed save isn't required.
        /// </summary>
        void Delete(IFavorite favorite);

        /// <summary>
        /// Removes all required favorites from persistence, if they are present.
        /// And remove them from all their groups.
        /// Saves changes, if delayed save isn't required.
        /// </summary>
        void Delete(List<IFavorite> favorites);

        SortableList<IFavorite> ToListOrderedByDefaultSorting();

        /// <summary>
        /// updates all required favorites in persistence, if they are present,
        /// updating their credential property by credentailName value.
        /// Saves changes, if delayed save isn't required.
        /// </summary>
        void ApplyCredentialsToAllFavorites(List<IFavorite> selectedFavorites, ICredentialSet credential);

        /// <summary>
        /// updates all required favorites in persistence, if they are present,
        /// updating their password property by newPassword value.
        /// Saves changes, if delayed save isn't required.
        /// </summary>
        void SetPasswordToAllFavorites(List<IFavorite> selectedFavorites, string newPassword);

        /// <summary>
        /// updates all required favorites in persistence, if they are present,
        /// updating their Domain property by newDomainName value.
        /// Saves changes, if delayed save isn't required.
        /// </summary>
        void ApplyDomainNameToAllFavorites(List<IFavorite> selectedFavorites, string newDomainName);

        /// <summary>
        /// updates all required favorites in persistence, if they are present,
        /// updating their UserName property by newUserName value.
        /// Saves changes, if delayed save isn't required.
        /// </summary>
        void ApplyUserNameToAllFavorites(List<IFavorite> selectedFavorites, string newUserName);

        /// <summary>
        /// Stores new icon for selected favorite.
        /// </summary>
        /// <param name="favorite">Not null favorite to be updated.</param>
        /// <param name="imageFilePath">Full path to the file to be applied as new favorite icon.</param>
        void UpdateFavoriteIcon(IFavorite favorite, string imageFilePath);

        /// <summary>
        /// Loads icon image for selected favorite. Returns not null custom icon or default based on favorite protocol.
        /// </summary>
        /// <param name="favorite">Not null favorite for which to obtain the icon.</param>
        Image LoadFavoriteIcon(IFavorite favorite);
    }
}
