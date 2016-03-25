using System;
using System.Collections.Generic;
using Terminals.Data.Credentials;

namespace Terminals.Data.FilePersisted
{
    internal class FavoriteBatchUpdates
    {
        private readonly PersistenceSecurity persistenceSecurity;

        public FavoriteBatchUpdates(PersistenceSecurity persistenceSecurity)
        {
            this.persistenceSecurity = persistenceSecurity;
        }

        internal virtual void SetPasswordToFavorites(List<IFavorite> selectedFavorites, string newPassword)
        {
            foreach (IFavorite favorite in selectedFavorites)
            {
                favorite.Security.Password = newPassword;
            }
        }

        internal virtual void ApplyUserNameToFavorites(List<IFavorite> selectedFavorites, string newUserName)
        {
            Action<IGuardedSecurity, string> applyValue = (g, v) => g.UserName = v;
            this.ApplyValueToAllFavorites(selectedFavorites, newUserName, applyValue);
        }

        internal virtual void ApplyDomainNameToFavorites(List<IFavorite> selectedFavorites, string newDomainName)
        {
            Action<IGuardedSecurity, string> applyValue = (g, v) => g.Domain = v;
            this.ApplyValueToAllFavorites(selectedFavorites, newDomainName, applyValue);
        }

        private void ApplyValueToAllFavorites(List<IFavorite> selectedFavorites,
            string newUserName, Action<IGuardedSecurity, string> applyValue)
        {
            foreach (IFavorite favorite in selectedFavorites)
            {
                var guarded = new GuardedSecurity(this.persistenceSecurity, favorite.Security);
                applyValue(guarded, newUserName);
            }
        }
    }
}