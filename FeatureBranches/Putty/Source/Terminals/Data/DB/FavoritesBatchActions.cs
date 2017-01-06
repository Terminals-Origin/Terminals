using System.Collections.Generic;
using System.Data;
using System.Data.Entity.Infrastructure;
using System.Linq;
using Terminals.Data.FilePersisted;

namespace Terminals.Data.DB
{
    /// <summary>
    /// Does required action on group of favorites
    /// </summary>
    internal class FavoritesBatchActions : FavoriteBatchUpdates
    {
        private readonly Favorites favorites;
        private readonly EntitiesCache<DbFavorite> cache;
        private readonly DataDispatcher dispatcher;

        internal FavoritesBatchActions(Favorites favorites, EntitiesCache<DbFavorite> cache, IPersistence persistence)
            : base(persistence)
        {
            this.favorites = favorites;
            this.cache = cache;
            this.dispatcher = persistence.Dispatcher;
        }

        internal override void ApplyUserNameToFavorites(List<IFavorite> selectedFavorites, string newUserName)
        {
            var values = new ApplyValueParams(base.ApplyUserNameToFavorites,
                                          selectedFavorites, newUserName, "UserName");
            this.ApplyValue(values);
        }

        internal override void ApplyDomainNameToFavorites(List<IFavorite> selectedFavorites, string newDomainName)
        {
            var values = new ApplyValueParams(base.ApplyDomainNameToFavorites,
                                          selectedFavorites, newDomainName, "DomainName");
            this.ApplyValue(values);
        }

        internal override void SetPasswordToFavorites(List<IFavorite> selectedFavorites, string newPassword)
        {
            var values = new ApplyValueParams(base.SetPasswordToFavorites,
                                          selectedFavorites, newPassword, "Password");
            this.ApplyValue(values);
        }

        private void ApplyValue(ApplyValueParams applyParams)
        {
            try
            {
                this.TryApplyValue(applyParams);
            }
            catch (EntityException exception)
            {
                string message = string.Format("Unable to apply {0} to favorites", applyParams.PropertyName);
                this.dispatcher.ReportActionError(ApplyValue, applyParams, this, exception, message);
            }
        }

        private void TryApplyValue(ApplyValueParams applyParams)
        {
            using (Database database = DatabaseConnections.CreateInstance())
            {
                var dbFavorites = applyParams.Favorites.Cast<DbFavorite>().ToList();
                database.Cache.AttachAll(dbFavorites);
                applyParams.Action(applyParams.Favorites, applyParams.ValueToApply);
                this.SaveAndReportFavoritesUpdated(database, dbFavorites, applyParams.Favorites);
            }
        }

        internal void SaveAndReportFavoritesUpdated(Database database, List<DbFavorite> dbFavorites,
            List<IFavorite> selectedFavorites)
        {
            try
            {
                database.SaveImmediatelyIfRequested();
                this.cache.Update(dbFavorites);
                this.dispatcher.ReportFavoritesUpdated(selectedFavorites);
            }
            catch (DbUpdateException)
            {
                this.favorites.RefreshCache();
            }
        }
    }
}
