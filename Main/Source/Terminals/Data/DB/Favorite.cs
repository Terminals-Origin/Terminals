using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using Terminals.Connections;
using Unified;

namespace Terminals.Data.DB
{
    internal partial class Favorite : IFavorite, IEntityContext
    {
        public Database Database { get; set; }

        /// <summary>
        /// cant be set in constructor, because the constructor is used by EF when loading the entities
        /// </summary>
        private bool isNewlyCreated = false;

        /// <summary>
        /// Should be never null to prevent access violations
        /// </summary>
        private ProtocolOptions protocolProperties;

        private Guid guid = Guid.NewGuid();

        internal Guid Guid
        {
            get { return this.guid; }
        }

        Guid IFavorite.Id
        {
            get { return this.guid; } 
            set { this.guid = value; }
        }

        IBeforeConnectExecuteOptions IFavorite.ExecuteBeforeConnect
        {
            get
            {
                return this.ExecuteBeforeConnect; 
            }
        }

        IDisplayOptions IFavorite.Display
        {
            get
            {
                return this.Display;
            }
        }

        ISecurityOptions IFavorite.Security
        {
            get
            {
                return this.Security; 
            }
        }

        List<IGroup> IFavorite.Groups
        {
            get { return GetInvariantGroups(); }
        }

        /// <summary>
        /// Gets or sets the protocol specific container. This isnt a part of an entity,
        /// because we are using lazy loading of this property and we dont want to cache
        /// its xml persisted content.
        /// </summary>
        public ProtocolOptions ProtocolProperties
        {
            get
            {
                this.LoadProtocolProperties();
                return this.protocolProperties;
            }
            set
            {
                this.protocolProperties = value;
            }
        }

        /// <summary>
        /// Gets empty string. Set loads the image from file and updates the icon reference in database.
        /// The string get/set image file path to import/export favorite icon isnt supported in database persistence.
        /// </summary>
        public string ToolBarIconFile
        {
            get
            {
                return string.Empty;
            }
            set
            {
                this.toolBarIcon = FavoriteIcons.LoadImage(value, this);
                //if(FavoriteIcons.IsntDefaultProtocolImage(this.toolBarIcon)) ;
            }
        }

        // because of the disposable image, favorite should implement IDisposable
        private Image toolBarIcon;
        public Image ToolBarIconImage
        {
            get
            {
                if (this.Database != null && this.toolBarIcon == null)
                    this.TryLoadImageFromDatabase();

                return this.toolBarIcon;
            }
        }

        public string GroupNames
        {
            get
            {
                List<IGroup> groups = GetInvariantGroups();
                return Data.Favorite.GroupsListToString(groups);
            }
        }

        /// <summary>
        /// Initializes new instance of a favorite and sets its properties to default values,
        /// which arent defined by database.
        /// </summary>
        public Favorite()
        {
            this._Protocol = ConnectionManager.RDP;
            this._Port = ConnectionManager.RDPPort;
            this.protocolProperties = new RdpOptions();
        }

        internal void MarkAsNewlyCreated()
        {
            this.isNewlyCreated = true;
        }

        internal void UpdateImageInDatabase(Database database)
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
            if (this.toolBarIcon != null)
            {
                byte[] imageData = FavoriteIcons.ImageToBinary(this.toolBarIcon);
                if (imageData.Length > 0)
                {
                    database.SetFavoriteIcon(this.Id, imageData);
                }
            }
        }

        private void TryLoadImageFromDatabase()
        {
            try
            {
                byte[] imageData = this.Database.GetFavoriteIcon(this.Id);
                this.toolBarIcon = FavoriteIcons.LoadImage(imageData, this);
            }
            catch (Exception exception)
            {
                Logging.Log.Error("Couldnt load image from database", exception);
            }
        }

        public IFavorite Copy()
        {
            var copy = new Favorite();
            copy.DesktopShare = this.DesktopShare;
            copy.Display = this.Display.Copy();
            copy.ExecuteBeforeConnect = this.ExecuteBeforeConnect.Copy();
            copy.Name = this.Name;
            copy.NewWindow = this.NewWindow;
            copy.Notes = this.Notes;
            copy.Port = this.Port;
            copy.Protocol = this.Protocol;
            copy.Security = this.Security.Copy();
            copy.ServerName = this.ServerName;
            copy.ToolBarIconFile = this.ToolBarIconFile;

            copy.ProtocolProperties = this.ProtocolProperties.Copy();

            return copy;
        }

        public string GetToolTipText()
        {
            return Data.Favorite.GetToolTipText(this);
        }

        public int CompareByDefaultSorting(IFavorite target)
        {
            return Data.Favorite.CompareByDefaultSorting(this, target);
        }

        public void UpdatePasswordsByNewKeyMaterial(string newKeyMaterial)
        {
            this.Security.UpdatePasswordByNewKeyMaterial(newKeyMaterial);
            Data.Favorite.UpdatePasswordsInProtocolProperties(this.protocolProperties, newKeyMaterial);
        }

        /// <summary>
        /// Reflect the protocol change into the protocol properties
        /// </summary>
        partial void OnProtocolChanged()
        {
            this.protocolProperties = Data.Favorite.UpdateProtocolPropertiesByProtocol(this.Protocol, this.protocolProperties);
        }

        /// <summary>
        /// Using given database context commits changes of protocol properties into the database
        /// </summary>
        internal void SaveProperties(Database database)
        {
            try
            {
                string serializedProperties = Serialize.SerializeXMLAsString(this.protocolProperties);
                database.UpdateFavoriteProtocolProperties(this.Id, serializedProperties);
                this.isNewlyCreated = false;
            }
            catch (Exception exception)
            {
                Logging.Log.Error("Couldnt save protocol properties to database", exception);
            }
        }

        /// <summary>
        /// Realization of expensive property lazy loading
        /// </summary>
        private void LoadProtocolProperties()
        {
            try
            {
                if (!this.isNewlyCreated)
                {
                    this.LoadPropertiesFromDatabase();
                }
            }
            catch (Exception exception)
            {
                Logging.Log.Error("Couldnt obtain protocol properties from database", exception);
                this.protocolProperties = Data.Favorite.UpdateProtocolPropertiesByProtocol(
                  this.Protocol, new RdpOptions());
            }
        }

        private void LoadPropertiesFromDatabase()
        {
            if (this.Database != null)
            {
                string serializedProperties = this.Database.GetFavoriteProtocolProperties(this.Id).FirstOrDefault();
                Type propertiesType = this.protocolProperties.GetType();
                this.protocolProperties =
                    Serialize.DeSerializeXML(serializedProperties, propertiesType) as ProtocolOptions;
            }

        }

        internal List<IGroup> GetInvariantGroups()
        {
            return this.Groups.ToList().Cast<IGroup>().ToList();
        }

        public override bool Equals(object favorite)
        {
            var oponent = favorite as Favorite;
            if (oponent == null)
                return false;

            return this.Id.Equals(oponent.Id);
        }

        public override int GetHashCode()
        {
            return this.Id.GetHashCode();
        }

        public override String ToString()
        {
            return Data.Favorite.ToString(this);
        }
    }
}