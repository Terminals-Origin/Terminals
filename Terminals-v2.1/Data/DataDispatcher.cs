using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using Terminals.Configuration;
using Terminals.History;

namespace Terminals.Data
{
    /// <summary>
    /// Infroms about changes in Tags collection
    /// </summary>
    /// <param name="args">Not null container reporting removed and added Tags</param>
    internal delegate void GroupsChangedEventHandler(GroupsChangedArgs args);

    /// <summary>
    /// Informs about changes in favorites collection.
    /// </summary>
    /// <param name="args">Not null container, reporting Added, removed, and updated favorites</param>
    internal delegate void FavoritesChangedEventHandler(FavoritesChangedEventArgs args);

    /// <summary>
    /// Central point, which distributes informations about changes in Tags and Favorites collections
    /// </summary>
    internal sealed class DataDispatcher
    {
        internal event GroupsChangedEventHandler GroupsChanged;
        internal event FavoritesChangedEventHandler FavoritesChanged;
        internal DataDispatcher() { }

        internal static List<IFavorite> GetMissingFavorites(List<IFavorite> newFavorites, List<IFavorite> oldFavorites)
        {
            return newFavorites.Where(
                newFavorite => oldFavorites.FirstOrDefault(oldFavorite => oldFavorite.Name == newFavorite.Name) == null)
                .ToList();
        }

        internal void ReportFavoriteAdded(IFavorite addedFavorite)
        {
            var args = new FavoritesChangedEventArgs();
            args.Added.Add(addedFavorite);
            FireFavoriteChanges(args);
        }

        internal void ReportFavoritesAdded(List<IFavorite> addedFavorites)
        {
            var args = new FavoritesChangedEventArgs();
            args.Added.AddRange(addedFavorites);
            FireFavoriteChanges(args);
        }

        internal void ReportFavoriteUpdated(IFavorite changedFavorite)
        {
            var args = new FavoritesChangedEventArgs();
            args.Updated.Add(changedFavorite);
            FireFavoriteChanges(args);
        }

        internal void ReportFavoritesUpdated(List<IFavorite> changedFavorites)
        {
            var args = new FavoritesChangedEventArgs();
            args.Updated.AddRange(changedFavorites);
            FireFavoriteChanges(args);
        }

        internal void ReportFavoriteDeleted(IFavorite deletedFavorite)
        {
            var args = new FavoritesChangedEventArgs();
            args.Removed.Add(deletedFavorite);
            FireFavoriteChanges(args);
        }

        internal void ReportFavoritesDeleted(List<IFavorite> deletedFavorites)
        {
            var args = new FavoritesChangedEventArgs();
            args.Removed.AddRange(deletedFavorites);
            FireFavoriteChanges(args);
        }

        private void FireFavoriteChanges(FavoritesChangedEventArgs args)
        {
            Debug.WriteLine(args.ToString());
            if (this.FavoritesChanged != null && !args.IsEmpty)
            {
                this.FavoritesChanged(args);
            }
        }

        internal void ReportGroupsAdded(List<IGroup> addedGroups)
        {
            var args = new GroupsChangedArgs();
            args.Added.AddRange(addedGroups);
            this.FireGroupsChanged(args);
        }

        internal void ReportGroupsDeleted(List<IGroup> deletedGroups)
        {
            var args = new GroupsChangedArgs();
            args.Removed.AddRange(deletedGroups);
            this.FireGroupsChanged(args);
        }

        internal void ReportGroupsRecreated(List<IGroup> addedGroups, List<IGroup> deletedGroups)
        {
            var args = new GroupsChangedArgs(addedGroups, deletedGroups);
            this.FireGroupsChanged(args);
        }

        private void FireGroupsChanged(GroupsChangedArgs args)
        {
            Debug.WriteLine(args.ToString());
            if (this.GroupsChanged != null && !args.IsEmpty)
            {
                this.GroupsChanged(args);
            }
        }
    }
}
