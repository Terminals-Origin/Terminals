using System;
using System.Collections.Generic;

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
        /// If there is no favorite with such name, returns null. Search isnt case sensitive.
        /// Use this only to identify, if favorite with required name isnt already present,
        /// to prevent name duplicities.
        /// </summary>
        IFavorite this[string favoriteName] { get; }
        
        /// <summary>
        /// Adds favorite in the persistance, if no favorite with its identifier exists.
        /// Saves changes, if delayed save isnt required.
        /// </summary>
        void Add(IFavorite favorite);

        /// <summary>
        /// Adds all favorites in the persistance, which identifier doesnt exist in current collection yet.
        /// Saves changes, if delayed save isnt required.
        /// </summary>
        void Add(List<IFavorite> favorites);

        /// <summary>
        /// Replace favorite with the present identifier in persistance with the one send as parameter.
        /// Saves changes, if delayed save isnt required.
        /// This expects, that user didnt change favorite groups.
        /// </summary>
        void Update(IFavorite favorite);

        /// <summary>
        /// Updates favorite and also updates it in all groups defined by groups collection.
        /// If favorite is present in group not listed in groupNames it will be removed,
        /// if it isnt present, but is listed in groupNames, then it will be add to the group.
        /// Saves changes, if delayed save isnt required.
        /// </summary>
        /// <param name="favorite">Not null item to update, which holds old groups yet.</param>
        /// <param name="groups">New collection of groups in which the favorite should be listed.</param>
        void UpdateFavorite(IFavorite favorite, List<IGroup> groups);

        /// <summary>
        /// Removes the favorite from persistance, if it is present.
        /// and remove it also from all its groups.
        /// Saves changes, if delayed save isnt required.
        /// </summary>
        void Delete(IFavorite favorite);

        /// <summary>
        /// Removes all required favorites from persistance, if they are present.
        /// And remove them from all their groups.
        /// Saves changes, if delayed save isnt required.
        /// </summary>
        void Delete(List<IFavorite> favorites);

        /// <summary>
        /// Gets favorites as sortable collection. Items arent sorted.
        /// </summary>
        SortableList<IFavorite> ToList();

        SortableList<IFavorite> ToListOrderedByDefaultSorting();

        /// <summary>
        /// updates all required favorites in persistance, if they are present,
        /// updating their credential property by credentailName value.
        /// Saves changes, if delayed save isnt required.
        /// </summary>
        void ApplyCredentialsToAllFavorites(List<IFavorite> selectedFavorites, ICredentialSet credential);

        /// <summary>
        /// updates all required favorites in persistance, if they are present,
        /// updating their password property by newPassword value.
        /// Saves changes, if delayed save isnt required.
        /// </summary>
        void SetPasswordToAllFavorites(List<IFavorite> selectedFavorites, string newPassword);

        /// <summary>
        /// updates all required favorites in persistance, if they are present,
        /// updating their Domain property by newDomainName value.
        /// Saves changes, if delayed save isnt required.
        /// </summary>
        void ApplyDomainNameToAllFavorites(List<IFavorite> selectedFavorites, string newDomainName);

        /// <summary>
        /// updates all required favorites in persistance, if they are present,
        /// updating their UserName property by newUserName value.
        /// Saves changes, if delayed save isnt required.
        /// </summary>
        void ApplyUserNameToAllFavorites(List<IFavorite> selectedFavorites, string newUserName);
    }
}
