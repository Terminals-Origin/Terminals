using System.Data;
using System.Data.Entity.Infrastructure;

namespace Terminals.Data.DB
{
    internal class DbFavoriteImagesStore
    {
        private readonly DataDispatcher dispatcher;

        private readonly FavoriteIcons favoriteIcons;

        internal DbFavoriteImagesStore(DataDispatcher dispatcher, FavoriteIcons favoriteIcons)
        {
            this.dispatcher = dispatcher;
            this.favoriteIcons = favoriteIcons;
        }

        internal void LoadImageFromDatabase(DbFavorite favorite)
        {
            try
            {
                this.TryLoadImageFromDatabase(favorite);
            }
            catch (DbUpdateException)
            {
                // assign default image and let fix by next refresh
                this.AssignImageByLoadedData(favorite, new byte[0]);
            }
            catch (EntityException exception)
            {
                this.dispatcher.ReportActionError(() => this.LoadImageFromDatabase(favorite), favorite, exception,
                    "Unable to load favorite icon from database.\r\nDatabase connection lost.");
            }
        }

        private void TryLoadImageFromDatabase(DbFavorite favorite)
        {
            using (Database database = DatabaseConnections.CreateInstance())
            {
                byte[] imageData = database.GetFavoriteIcon(favorite.Id);
                this.AssignImageByLoadedData(favorite, imageData);
            }
        }

        private void AssignImageByLoadedData(DbFavorite favorite, byte[] imageData)
        {
            favorite.ToolBarIconImage = this.favoriteIcons.LoadImage(imageData, favorite);
        }

        internal void UpdateImageInDatabase(DbFavorite favorite, Database database)
        {
            try
            {
                this.TryUpdateImageInDatabase(favorite, database);
            }
            catch (DbUpdateException)
            {
                // recovery will be done by next update
                Logging.Error("Unable to update favorite image in database because of concurrency exception");
            }
            catch (EntityException exception)
            {
                this.dispatcher.ReportActionError(db => this.UpdateImageInDatabase(favorite, db), database, favorite, exception,
                    "Unable to Save favorite icon to database.\r\nDatabase connection lost.");
            }
        }

        private void TryUpdateImageInDatabase(DbFavorite favorite, Database database)
        {
            if (this.ShouldSaveIcon(favorite))
            {
                byte[] imageData = this.favoriteIcons.ImageToBinary(favorite.ToolBarIconImage);
                if (imageData.Length > 0)
                {
                    database.SetFavoriteIcon(favorite.Id, imageData);
                }
            }
        }

        private bool ShouldSaveIcon(DbFavorite favorite)
        {
            return favorite.ToolBarIconImage != null &&
                   !this.favoriteIcons.IsDefaultProtocolImage(favorite.ToolBarIconImage);
        }

        public void AssingNewIcon(DbFavorite favorite, string imageFilePath)
        {
            favorite.ToolBarIconImage = this.favoriteIcons.LoadImage(imageFilePath, favorite);
        }
    }
}