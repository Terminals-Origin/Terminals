using System;
using System.Collections.Generic;
using System.Linq;
using System.Transactions;
using System.Xml.Linq;
using Terminals.Security;

namespace Terminals.Data.DB
{
    internal class DatabasePasswordUpdate
    {
        private SqlPersistenceSecurity persistenceSecurity;

        private string newStoredKey = string.Empty;

        private string newKeyMaterial;

        private Database database;

        /// <summary>
        /// Only for security reasons
        /// </summary>
        private DatabasePasswordUpdate()
        {
        }

        internal static void UpdateMastrerPassord(string connectionString, string oldPassword, string newPassword)
        {
            var update = new DatabasePasswordUpdate();
            update.Run(connectionString, oldPassword, newPassword);
        }

        private void Run(string connectionString, string oldPassword, string newPassword)
        {
            TestConnectionResult oldPasswordCheck = Database.TestConnection(connectionString, oldPassword);
            if (!oldPasswordCheck.Successful)
                return;

            this.Configure(connectionString, oldPassword, newPassword);
            this.CommitMasterPasswordInTransaction(connectionString);

        }

        private void CommitMasterPasswordInTransaction(string connectionString)
        {
            try
            {
                using (var transaction = new TransactionScope())
                {
                    // dangerous operation, which may break all stored passwords or database access
                    this.CommitNewMastrerPassord(connectionString);
                    transaction.Complete();
                }
            }
            catch (Exception ex)
            {
                Logging.Log.Error("Unable to update the database master password", ex);
            }
        }

        private void Configure(string connectionString, string oldPassword, string newPassword)
        {
            // persistence doesn't have to be fully configured, we need only the persistence passwords part
            this.persistenceSecurity = new SqlPersistenceSecurity();
            this.persistenceSecurity.UpdateDatabaseKey(connectionString, oldPassword);

            if (!string.IsNullOrEmpty(newPassword))
                this.newStoredKey = PasswordFunctions2.CalculateStoredMasterPasswordKey(newPassword);
            this.newKeyMaterial = PasswordFunctions2.CalculateMasterPasswordKey(newPassword, this.newStoredKey);
        }

        private void CommitNewMastrerPassord(string connectionString)
        {
            // todo surround all database usages by try/catch
            using (this.database = Database.CreateInstance(connectionString))
            {
                UpdateStoredPasswords();
                this.database.UpdateMasterPassword(this.newStoredKey);
                this.database.SaveChanges();
            }
            this.database = null;
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
            tsgwPasswordHash.Value = PasswordFunctions2.EncryptPassword(oldPassword, this.newKeyMaterial);
            return document.ToString();
        }

        /// <summary>
        /// both Credential and security passwords are in the same table, updated by this method
        /// </summary>
        private void UpdateCredentialBasePasswords()
        {
            foreach (DbCredentialBase credentials in this.database.CredentialBase)
            {
                credentials.AssignSecurity(this.persistenceSecurity);
                credentials.UpdatePasswordByNewKeyMaterial(this.newKeyMaterial);
            }
        }
    }
}
