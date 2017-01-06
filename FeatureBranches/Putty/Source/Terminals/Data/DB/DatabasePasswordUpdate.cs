using System;
using System.Collections.Generic;
using System.Linq;
using System.Transactions;
using System.Xml.Linq;
using Terminals.Data.Credentials;
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

        internal static TestConnectionResult UpdateMastrerPassord(string connectionString, string oldPassword, string newPassword)
        {
            TestConnectionResult oldPasswordCheck = DatabaseConnections.TestConnection(connectionString, oldPassword);
            if (!oldPasswordCheck.Successful)
                return oldPasswordCheck;

            var update = new DatabasePasswordUpdate();
            return update.Run(connectionString, oldPassword, newPassword);
        }

        private TestConnectionResult Run(string connectionString, string oldPassword, string newPassword)
        {
            try
            {
                this.Configure(connectionString, oldPassword, newPassword);
                this.CommitMasterPasswordInTransaction(connectionString);
                return new TestConnectionResult();
            }
            catch (Exception ex)
            {
                Logging.Error("Unable to update the database master password", ex);
                return new TestConnectionResult(ex.Message);
            }
        }

        private void CommitMasterPasswordInTransaction(string connectionString)
        {
            using (var transaction = new TransactionScope())
            {
                // dangerous operation, which may break all stored passwords or database access
                this.CommitNewMastrerPassord(connectionString);
                transaction.Complete();
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
            using (this.database = DatabaseConnections.CreateInstance(connectionString))
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
                var guarded = new GuardedCredential(credentials, this.persistenceSecurity);
                guarded.UpdatePasswordByNewKeyMaterial(this.newKeyMaterial);
            }
        }
    }
}
