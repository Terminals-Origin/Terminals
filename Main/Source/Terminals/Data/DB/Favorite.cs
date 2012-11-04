using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using Terminals.Connections;
using Terminals.Converters;

namespace Terminals.Data.DB
{
    internal partial class Favorite : IFavorite, IIntegerKeyEnityty
    {
        private Groups groups;

        private StoredCredentials credentials;

        private PersistenceSecurity persistenceSecurity;

        /// <summary>
        /// cant be set in constructor, because the constructor is used by EF when loading the entities
        /// </summary>
        private bool isNewlyCreated;

        private readonly FavoriteDetails details;

        /// <summary>
        /// Should be never null to prevent access violations
        /// </summary>
        private ProtocolOptions protocolProperties;

        // for backwrad compatibility with the file persistence only
        private Guid guid;

        internal Guid Guid
        {
            get
            {
                if (this.guid == Guid.Empty)
                    this.guid = GuidConverter.ToGuid(this.Id);

                return this.guid;
            }
        }

        Guid IFavorite.Id
        {
            get { return this.Guid; }
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
                return this.GetSecurity();
            }
        }

        private SecurityOptions GetSecurity()
        {
            this.details.Load();
            return this.security;
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
                List<IGroup> loadedGroups = GetInvariantGroups();
                return Data.Favorite.GroupsListToString(loadedGroups);
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
            this.details.LoadFieldsFromReferences();
        }

        IFavorite IFavorite.Copy()
        {
            var copy = new Favorite();
            copy.UpdateFrom(this);
            return copy;
        }

        void IFavorite.UpdateFrom(IFavorite source)
        {
            var sourceFavorite = source as Favorite;
            if (sourceFavorite == null)
                return;
            this.UpdateFrom(sourceFavorite);
        }

        private void UpdateFrom(Favorite source)
        {
            // force load first to fill the content, otherwise we dont have to able to copy
            this.details.Load();
            source.details.Load();

            this.DesktopShare = source.DesktopShare;
            // we cand copy the fields, because they are also dependent on the favorite Id
            this.display.UpdateFrom(source.display);
            this.executeBeforeConnect.UpdateFrom(source.executeBeforeConnect);
            this.Name = source.Name;
            this.NewWindow = source.NewWindow;
            this.Notes = source.Notes;
            this.Port = source.Port;
            this.Protocol = source.Protocol;
            this.security.UpdateFrom(source.security);
            this.ServerName = source.ServerName;
            this.toolBarIcon = source.ToolBarIconImage;
            // protocolProperties dont have a favorite Id reference, so we can overwrite complete content
            this.protocolProperties = source.protocolProperties.Copy();
            this.AssignStores(source.groups, source.credentials, source.persistenceSecurity);
        }

        public string GetToolTipText()
        {
            return Data.Favorite.GetToolTipText(this);
        }

        public int CompareByDefaultSorting(IFavorite target)
        {
            return Data.Favorite.CompareByDefaultSorting(this, target);
        }

        bool IStoreIdEquals<IFavorite>.StoreIdEquals(IFavorite oponent)
        {
            var oponentFavorite = oponent as Favorite;
            if (oponentFavorite == null)
                return false;

            return oponentFavorite.Id == this.Id;
        }

        partial void OnIdChanging(int value)
        {
            this.guid = GuidConverter.ToGuid(value);
        }

        /// <summary>
        /// Reflect the protocol change into the protocol properties
        /// </summary>
        partial void OnProtocolChanged()
        {
            this.protocolProperties = Data.Favorite.UpdateProtocolPropertiesByProtocol(this.Protocol, this.protocolProperties);
        }

        private List<IGroup> GetInvariantGroups()
        {
            // see also the Group.Favorites
            // prefere to select cached items, instead of selecting from database directly
            return this.groups.GetGroupsContainingFavorite(this.Id)
                .Cast<IGroup>()
                .ToList();
        }

        internal void AssignStoreToRdpOptions(PersistenceSecurity persistenceSecurity)
        {
            // only, if the favorite is newly created
            Data.Favorite.AssignStoreToRdpOptions(this.protocolProperties, persistenceSecurity);
        }

        internal void AssignStores(Groups groups, StoredCredentials credentials, PersistenceSecurity persistenceSecurity)
        {
            this.groups = groups;
            this.credentials = credentials;
            this.persistenceSecurity = persistenceSecurity;
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

        public void AttachDetails(Database database)
        {
            this.details.Attach(database);
        }

        internal void DetachDetails(Database database)
        {
            this.details.Detach(database);
        }

        internal void ReleaseLoadedDetails()
        {
            this.details.ReleaseLoadedDetails();
        }

        public override String ToString()
        {
            return Data.Favorite.ToString(this);
        }
    }
}