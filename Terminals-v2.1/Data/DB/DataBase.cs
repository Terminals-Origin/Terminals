using System;
using System.Data.EntityClient;
using System.Linq;

namespace Terminals.Data.DB
{
    public partial class DataBase
    {
        private const string developmentConnectionString = @"Data Source=.\SQLEXPRESS;AttachDbFilename=.\Data\Terminals.mdf;Integrated Security=True;User Instance=False";
        private const string metadata = @"res://*/Data.DB.SQLPersistance.csdl|res://*/Data.DB.SQLPersistance.ssdl|res://*/Data.DB.SQLPersistance.msl";

        private bool delaySave = false;
        // TODO Add eventing, if something is changed in database (Jiri Pokorny, 13.02.2012)
        // TODO Add new table for database extra options, which couldnt be stored localy like masterpassword (Jiri Pokorny, 26.02.2012)

        private static string BuildConnectionString()
        {
            var connectionBuilder = new EntityConnectionStringBuilder
            {
                Provider = "System.Data.SqlClient",
                ProviderConnectionString = developmentConnectionString,
                Metadata = metadata
            };

            return connectionBuilder.ToString();
        }

        internal static DataBase CreateDatabaseInstance()
        {
            // TODO Assign custom connection string from settings (Jiri Pokorny, 13.02.2012)
            string connectionString = BuildConnectionString();
            return new DataBase(connectionString);
        }

        public void StartDelayedUpdate()
        {
            delaySave = true;
        }

        public void SaveAndFinishDelayedUpdate()
        {
            delaySave = false;
            SaveImmediatelyIfRequested();
        }

        internal void SaveImmediatelyIfRequested()
        {
            if (!delaySave)
            {
                // todo Add protection against concurent updates
                this.SaveChanges();
            }
        }

        internal Favorite GetFavoriteByGuid(Guid favoriteId)
        {
            return this.Favorites
                .FirstOrDefault(favorite => favorite.Guid == favoriteId);
        }
    }
}
