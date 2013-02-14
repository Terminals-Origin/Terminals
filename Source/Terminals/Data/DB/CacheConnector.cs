using System.Collections.Generic;
using System.Data;

namespace Terminals.Data.DB
{
    /// <summary>
    /// Connects or disconnects the cached entities to the database context
    /// </summary>
    internal class CacheConnector
    {
        private readonly Database database;

        internal CacheConnector(Database database)
        {
            this.database = database;
        }

        /// <summary>
        /// Attaches all groups in toAttach to this context.
        /// Does not check, if the entities are already in the context.
        /// </summary>
        /// <param name="toAttach">Not null collection of items from this model</param>
        internal void AttachAll(IEnumerable<DbGroup> toAttach)
        {
            foreach (DbGroup group in toAttach)
            {
                this.database.Groups.Attach(@group);
            }
        }

        internal void AttachAll(IEnumerable<DbFavorite> favorites)
        {
            foreach (DbFavorite favorite in favorites)
            {
                this.AttachFavorite(favorite);
            }
        }

        internal void AttachFavorite(DbFavorite favorite)
        {
            this.Attach(favorite.Details);
            this.database.Favorites.Attach(favorite);
        }

        private void Attach(DbFavorite.FavoriteDetails favorite)
        {
            if (!favorite.Loaded)
                return;

            this.Attach(favorite.Security);
            this.database.DisplayOptions.Attach(favorite.Display);
            this.database.BeforeConnectExecute.Attach(favorite.ExecuteBeforeConnect);
        }

        private void Attach(DbSecurityOptions security)
        {
            this.database.Security.Attach(security);
            this.AttachCredentialBase(security);
            this.AttachCredentialSet(security);
        }

        private void AttachCredentialSet(DbSecurityOptions security)
        {
            DbCredentialSet credentialSet = security.ResolveCredentailFromStore();
            if (credentialSet == null)
                return;

            this.database.CredentialBase.Attach(credentialSet);
        }

        private void AttachCredentialBase(DbSecurityOptions security)
        {
            if (security.CachedCredentials == null)
                return;

            security.CredentialBase = security.CachedCredentials;
            this.database.CredentialBase.Attach(security.CachedCredentials);
        }

        internal void MarkAsModified(List<DbFavorite> toUpdate)
        {
            foreach (DbFavorite favorite in toUpdate)
            {
                this.MarkFavoriteAsModified(favorite);
            }
        }

        internal void MarkFavoriteAsModified(DbFavorite favorite)
        {
            this.MarkAsModified(favorite);
            this.MarkAsModified(favorite.Details);
        }

        private void MarkAsModified(DbFavorite.FavoriteDetails favorite)
        {
            if (!favorite.Loaded)
                return;

            this.MarkSecurityAsModified(favorite.Security);
            this.MarkAsModified(favorite.Display);
            this.MarkAsModified(favorite.ExecuteBeforeConnect);
        }

        private void MarkSecurityAsModified(DbSecurityOptions security)
        {
            this.MarkAsModified(security);
            if (security.CachedCredentials != null)
                this.MarkAsModified(security.CachedCredentials);

            security.UpdateCredentialSetReference();
        }

        /// <summary>
        /// Switch toUpdate entity state to Modified. 
        /// </summary>
        internal void MarkAsModified<TEntity>(TEntity toUpdate)
            where TEntity : class
        {
            this.database.Entry(toUpdate).State = EntityState.Modified;
        }

        internal void DetachAll(IEnumerable<DbFavorite> favorites)
        {
            foreach (DbFavorite favorite in favorites)
            {
                this.DetachFavorite(favorite);
            }
        }

        internal void DetachAll(IEnumerable<DbGroup> entitiesToDetach)
        {
            foreach (DbGroup entity in entitiesToDetach)
            {
                this.Detach(entity);
            }
        }

        internal void Detach<TEntity>(TEntity entity)
             where TEntity : class
        {
            this.database.Entry(entity).State = EntityState.Detached;
        }

        internal void DetachFavorite(DbFavorite favorite)
        {
            this.Detach(favorite.Details);
            this.Detach(favorite);
        }

        private void Detach(DbFavorite.FavoriteDetails favorite)
        {
            if (!favorite.Loaded)
                return;

            this.DetachSecurity(favorite.Security);
            this.Detach(favorite.Display);
            this.Detach(favorite.ExecuteBeforeConnect);
        }

        private void DetachSecurity(DbSecurityOptions security)
        {
            if (security.CredentialSet != null)
                this.Detach(security.CredentialSet);

            // check the reference, not local cached field
            if (security.CredentialBase != null)
                this.Detach(security.CachedCredentials);

            this.Detach(security);
        }
    }
}
