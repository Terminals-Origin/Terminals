
using System;
using System.Linq;

using Unified;

namespace Terminals.Data.DB
{
    internal partial class Favorite : IFavorite
    {
        /// <summary>
        /// Managese saving of icon and favorite detailed properties
        /// </summary>
        private class FavoriteDetails
        {
            private readonly Favorite favorite;

            private bool DetailsLoaded
            {
                get
                {
                    return this.favorite.security != null;
                }
            }

            internal FavoriteDetails(Favorite favorite)
            {
                this.favorite = favorite;
            }

            internal void MarkAsModified(Database database)
            {
                if (this.DetailsLoaded)
                {
                    this.Attach(database);
                    database.MarkAsModified(this.favorite.executeBeforeConnect);
                    database.MarkAsModified(this.favorite.display);
                    database.MarkAsModified(this.favorite.security);
                }
            }

            private void Attach(Database database)
            {
                database.Attach(this.favorite.display);
                database.Attach(this.favorite.security);
                database.Attach(this.favorite.executeBeforeConnect);
            }

            internal void Detach(Database database)
            {
                if (this.DetailsLoaded)
                {
                    database.Detach(this.favorite.display);
                    database.Detach(this.favorite.security);
                    database.Detach(this.favorite.executeBeforeConnect);
                }
            }

            internal void Load()
            {
                if (!this.DetailsLoaded)
                {
                    using (var database = Database.CreateDatabaseInstance())
                    {
                        database.Attach(favorite);
                        this.LoadSecurity();
                        this.LoadDisplay();
                        this.LoadExecuteBeforeConnect();
                        database.DetachFavorite(favorite);
                    }
                }
            }

            private void LoadExecuteBeforeConnect()
            {
                this.favorite.ExecuteBeforeConnectReference.Load();
                this.favorite.executeBeforeConnect = this.favorite.ExecuteBeforeConnect;
            }

            private void LoadDisplay()
            {
                this.favorite.DisplayReference.Load();
                this.favorite.display = this.favorite.Display;
            }

            private void LoadSecurity()
            {
                this.favorite.SecurityReference.Load();
                this.favorite.security = this.favorite.Security;
            }

            internal void Save(Database database)
            {
                this.SaveProtocolProperties(database);
                this.UpdateImageInDatabase(database);
            }

            /// <summary>
            /// Using given database context commits changes of protocol properties into the database
            /// </summary>
            private void SaveProtocolProperties(Database database)
            {
                try
                {
                    string serializedProperties = Serialize.SerializeXMLAsString(this.favorite.protocolProperties);
                    database.UpdateFavoriteProtocolProperties(this.favorite.Id, serializedProperties);
                    this.favorite.isNewlyCreated = false;
                }
                catch (Exception exception)
                {
                    Logging.Log.Error("Couldnt save protocol properties to database", exception);
                }
            }

            private void UpdateImageInDatabase(Database database)
            {
                try
                {
                    this.TryUpdateImageInDatabase(database);
                }
                catch (Exception exception)
                {
                    Logging.Log.Error("Couldnt load image from database", exception);
                }
            }

            private void TryUpdateImageInDatabase(Database database)
            {
                if (this.favorite.toolBarIcon != null)
                {
                    byte[] imageData = FavoriteIcons.ImageToBinary(this.favorite.toolBarIcon);
                    if (imageData.Length > 0)
                    {
                        database.SetFavoriteIcon(this.favorite.Id, imageData);
                    }
                }
            }

            /// <summary>
            /// Realization of expensive property lazy loading
            /// </summary>
            internal void LoadProtocolProperties()
            {
                try
                {
                    if (!this.favorite.isNewlyCreated)
                    {
                        this.LoadPropertiesFromDatabase();
                    }
                }
                catch (Exception exception)
                {
                    Logging.Log.Error("Couldnt obtain protocol properties from database", exception);
                    this.favorite.protocolProperties = Data.Favorite.UpdateProtocolPropertiesByProtocol(
                      this.favorite.Protocol, new RdpOptions());
                }
            }

            private void LoadPropertiesFromDatabase()
            {
                using (var database = Database.CreateDatabaseInstance())
                {
                    string serializedProperties = database.GetFavoriteProtocolProperties(this.favorite.Id).FirstOrDefault();
                    Type propertiesType = this.favorite.protocolProperties.GetType();
                    this.favorite.protocolProperties =
                        Serialize.DeSerializeXML(serializedProperties, propertiesType) as ProtocolOptions;
                }
            }

            internal void LoadImageFromDatabase()
            {
                try
                {
                    this.TryLoadImageFromDatabase();
                }
                catch (Exception exception)
                {
                    Logging.Log.Error("Couldnt load image from database", exception);
                }
            }

            private void TryLoadImageFromDatabase()
            {
                using (var database = Database.CreateDatabaseInstance())
                {
                    byte[] imageData = database.GetFavoriteIcon(this.favorite.Id);
                    this.favorite.toolBarIcon = FavoriteIcons.LoadImage(imageData, this.favorite);
                }
            }
        }
    }
}
