using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using Terminals.Security;

namespace Terminals.Data.DB
{
    internal class DatabasePasswordUpdate
    {
        private readonly SqlPersistenceSecurity persistenceSecurity;

        private readonly string newKeyMaterial;

        private Database database;

        private DatabasePasswordUpdate(string oldPassword, string newPassword)
        {
            this.persistenceSecurity = new SqlPersistenceSecurity();
            this.persistenceSecurity.UpdateDatabaseKey(oldPassword);
            this.newKeyMaterial = PasswordFunctions.CalculateMasterPasswordKey(newPassword); 
        }

        internal static void UpdateMastrerPassord(string connectionString, string oldPassword, string newPassword)
        {
            var update = new DatabasePasswordUpdate(oldPassword, newPassword);
            update.Run(connectionString, oldPassword, newPassword);
        }

        private void Run(string connectionString, string oldPassword, string newPassword)
        {
            Tuple<bool, string> oldPasswordCheck = Database.TestConnection(connectionString, oldPassword);
            if (oldPasswordCheck.Item1)
                CommitNewMastrerPassord(connectionString, newPassword);
        }

        private void CommitNewMastrerPassord(string connecitonString, string newPassword)
        {
            // todo surround all database usings by try/catch
            // todo do it in transaction to prevent inconsistent data
            using (this.database = Database.CreateInstance(connecitonString))
            {
                UpdateStoredPasswords();
                UpdateMasterPasswodInOptions(newPassword);
                database.SaveChanges();
            }

            this.database = null;
        }

        private void UpdateMasterPasswodInOptions(string newPassword)
        {
            string newHash = string.Empty;
            if (!string.IsNullOrEmpty(newPassword))
                newHash = PasswordFunctions.ComputeMasterPasswordHash(newPassword);
            this.database.UpdateMasterPassword(newHash);
        }

        private void UpdateStoredPasswords()
        {
            this.UpdateCredentialBasePasswords();
            List<int> rdpFavoriteIds = this.database.GetRdpFavoriteIds();
            this.UpdateFavoriteProtocolPasswords(rdpFavoriteIds);
        }

        private void UpdateFavoriteProtocolPasswords(List<int> rdpFavorites)
        {
            foreach (int favoriteId in rdpFavorites)
            {
                // there is no other choice, we have to download the properties content
                // end replace the passwords xml element content
                string rdpOptions = this.database.GetProtocolPropertiesByFavorite(favoriteId);
                rdpOptions = this.UpdateThePropertiesPassword(rdpOptions);
                this.database.UpdateFavoriteProtocolProperties(favoriteId, rdpOptions);
            }
        }

        private string UpdateThePropertiesPassword(string rdpOptions)
        {
            var document = XDocument.Parse(rdpOptions);
            if (document.Root == null)
                return rdpOptions;

            XElement tsgwPasswordHash = document.Root.Descendants("EncryptedPassword").First();
            string oldPassword = this.persistenceSecurity.DecryptPersistencePassword(tsgwPasswordHash.Value);
            tsgwPasswordHash.Value = PasswordFunctions.EncryptPassword(oldPassword, this.newKeyMaterial);
            return document.ToString();
        }

        /// <summary>
        /// both Credential and security passwords are in the same table, updated by this method
        /// </summary>
        private void UpdateCredentialBasePasswords()
        {
            foreach (CredentialBase credentials in this.database.CredentialBase)
            {
                credentials.AssignSecurity(this.persistenceSecurity);
                credentials.UpdatePasswordByNewKeyMaterial(this.newKeyMaterial);
            }
        }
    }
}
