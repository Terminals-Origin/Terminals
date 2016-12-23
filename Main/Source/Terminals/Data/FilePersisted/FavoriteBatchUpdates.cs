using System;
using System.Collections.Generic;
using Terminals.Data.Credentials;

namespace Terminals.Data.FilePersisted
{
    internal class FavoriteBatchUpdates
    {
        private readonly IPersistence persistence;

        public FavoriteBatchUpdates(IPersistence persistence)
        {
            this.persistence = persistence;
        }

        internal virtual void SetPasswordToFavorites(List<IFavorite> selectedFavorites, string newPassword)
        {
            Action<IGuardedSecurity, string> applyValue = (g, v) => g.Password = v;
            this.ApplyValueToAllFavorites(selectedFavorites, newPassword, applyValue);
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
                var guarded = new GuardedSecurity(this.persistence, favorite.Security);
                applyValue(guarded, newUserName);
            }
        }
    }
}