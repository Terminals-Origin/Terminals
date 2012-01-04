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
        /// </summary>
        void Update(IFavorite favorite);

        /// <summary>
        /// Removes the favorite from persistance, if it is present.
        /// Saves changes, if delayed save isnt required.
        /// </summary>
        void Delete(IFavorite favorite);

        /// <summary>
        /// Removes all required favorites from persistance, if they are present.
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
        void ApplyCredentialsToAllFavorites(List<IFavorite> selectedFavorites, string credentialName);

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
