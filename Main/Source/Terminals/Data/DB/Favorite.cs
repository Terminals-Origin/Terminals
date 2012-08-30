using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using Terminals.Connections;
using Unified;

namespace Terminals.Data.DB
{
    internal partial class Favorite : IFavorite, IIntegerKeyEnityty
    {
        /// <summary>
        /// cant be set in constructor, because the constructor is used by EF when loading the entities
        /// </summary>
        private bool isNewlyCreated;

        private readonly FavoriteDetails details;

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

        private BeforeConnectExecute executeBeforeConnect;

        IBeforeConnectExecuteOptions IFavorite.ExecuteBeforeConnect
        {
            get
            {
                this.details.Load();
                return this.executeBeforeConnect;
            }
        }

        private DisplayOptions display;

        IDisplayOptions IFavorite.Display
        {
            get
            {
                this.details.Load();
                return this.display;
            }
        }

        private SecurityOptions security;

        ISecurityOptions IFavorite.Security
        {
            get
            {
                this.details.Load();
                return this.security;
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
                this.details.LoadProtocolProperties();
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
                if (this.toolBarIcon == null)
                    this.details.LoadImageFromDatabase();

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
            this.details = new FavoriteDetails(this);
        }

        internal void MarkAsNewlyCreated()
        {
            this.isNewlyCreated = true;
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

        bool IStoreIdEquals<IFavorite>.StoreIdEquals(IFavorite oponent)
        {
            var oponentFavorite = oponent as Favorite;
            if (oponentFavorite == null)
                return false;

            return oponentFavorite.Id == this.Id;
        }

        /// <summary>
        /// Reflect the protocol change into the protocol properties
        /// </summary>
        partial void OnProtocolChanged()
        {
            this.protocolProperties = Data.Favorite.UpdateProtocolPropertiesByProtocol(this.Protocol, this.protocolProperties);
        }

        internal List<IGroup> GetInvariantGroups()
        {
            return this.Groups.ToList().Cast<IGroup>().ToList();
        }

        internal void MarkAsModified(Database database)
        {
            database.MarkAsModified(this);
            this.details.MarkAsModified(database);
        }

        internal void SaveDetails(Database database)
        {
            this.details.Save(database);
        }

        internal void DetachDetails(Database database)
        {
            this.details.Detach(database);
        }
        
        public override String ToString()
        {
            return Data.Favorite.ToString(this);
        }
    }
}