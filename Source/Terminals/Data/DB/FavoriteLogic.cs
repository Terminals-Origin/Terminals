using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using Terminals.Connections;
using Terminals.Converters;

namespace Terminals.Data.DB
{
    internal partial class DbFavorite : IFavorite, IIntegerKeyEnityty
    {
        private Groups groups;

        private StoredCredentials credentials;

        private PersistenceSecurity persistenceSecurity;

        /// <summary>
        /// cant be set in constructor, because the constructor is used by EF when loading the entities
        /// </summary>
        private bool isNewlyCreated;

        internal FavoriteDetails Details { get; private set; }

        /// <summary>
        /// Should be never null to prevent access violations
        /// </summary>
        private ProtocolOptions protocolProperties;

        // for backward compatibility with the file persistence only
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

        

        IBeforeConnectExecuteOptions IFavorite.ExecuteBeforeConnect
        {
            get
            {
                this.Details.Load();
                return this.Details.ExecuteBeforeConnect;
            }
        }

        IDisplayOptions IFavorite.Display
        {
            get
            {
                this.Details.Load();
                return this.Details.Display;
            }
        }

        ISecurityOptions IFavorite.Security
        {
            get
            {
                return this.GetSecurity();
            }
        }

        private DbSecurityOptions GetSecurity()
        {
            this.Details.Load();
            // returns null, if the favorite details loading failed.
            // the same for all other detail properties
            return this.Details.Security;
        }

        List<IGroup> IFavorite.Groups
        {
            get { return GetInvariantGroups(); }
        }

        /// <summary>
        /// Gets or sets the protocol specific container. This isn't a part of an entity,
        /// because we are using lazy loading of this property and we don't want to cache
        /// its xml persisted content.
        /// </summary>
        public ProtocolOptions ProtocolProperties
        {
            get
            {
                this.Details.LoadProtocolProperties();
                return this.protocolProperties;
            }
        }

        /// <summary>
        /// Gets empty string. Set loads the image from file and updates the icon reference in database.
        /// The string get/set image file path to import/export favorite icon isn't supported in database persistence.
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
                    this.Details.LoadImageFromDatabase();

                return this.toolBarIcon;
            }
        }

        public string GroupNames
        {
            get
            {
                List<IGroup> loadedGroups = GetInvariantGroups();
                return Favorite.GroupsListToString(loadedGroups);
            }
        }

        private int id;

        public int Id
        {
            get
            {
                return this.id;
            }
            set
            {
                this.id = value;
                this.guid = GuidConverter.ToGuid(value); 
            }
        }

        private string protocol;

        public string Protocol
        {
            get { return this.protocol; }
            set
            {
                this.protocol = value;
                // Reflect the protocol change into the protocol properties
                this.UpdateProtocolProperties(this.protocolProperties);
            }
        }

        /// <summary>
        /// Initializes new instance of a favorite and sets its properties to default values,
        /// which aren't defined by database.
        /// </summary>
        public DbFavorite()
        {
            this.Groups = new HashSet<DbGroup>();
            this.Protocol = ConnectionManager.RDP;
            this.Port = ConnectionManager.RDPPort;
            this.protocolProperties = new RdpOptions();
            this.Details = new FavoriteDetails(this);
        }

        internal void MarkAsNewlyCreated()
        {
            this.isNewlyCreated = true;
            this.Details.LoadFieldsFromReferences();
        }

        IFavorite IFavorite.Copy()
        {
            DbFavorite copy = Factory.CreateFavorite(this.persistenceSecurity, this.groups, 
                this.credentials, this.Details.Dispatcher);
            copy.UpdateFrom(this);
            return copy;
        }

        void IFavorite.UpdateFrom(IFavorite source)
        {
            var sourceFavorite = source as DbFavorite;
            if (sourceFavorite == null)
                return;
            this.UpdateFrom(sourceFavorite);
        }

        private void UpdateFrom(DbFavorite source)
        {
            // force load first to fill the content, otherwise we don't have to able to copy
            this.Details.Load();
            source.Details.Load();

            this.DesktopShare = source.DesktopShare;
            // we cant copy the fields, because they are also dependent on the favorite Id
            this.Details.UpdateFrom(source.Details);
            this.Name = source.Name;
            this.NewWindow = source.NewWindow;
            this.Notes = source.Notes;
            this.Port = source.Port;
            this.Protocol = source.Protocol;
            this.ServerName = source.ServerName;
            this.toolBarIcon = source.ToolBarIconImage;
            // protocolProperties don't have a favorite Id reference, so we can overwrite complete content
            this.protocolProperties = source.protocolProperties.Copy();
            this.AssignStores(source.groups, source.credentials, source.persistenceSecurity, source.Details.Dispatcher);
        }

        public string GetToolTipText()
        {
            return Favorite.GetToolTipText(this);
        }

        public int CompareByDefaultSorting(IFavorite target)
        {
            return Favorite.CompareByDefaultSorting(this, target);
        }

        bool IStoreIdEquals<IFavorite>.StoreIdEquals(IFavorite oponent)
        {
            var oponentFavorite = oponent as DbFavorite;
            if (oponentFavorite == null)
                return false;

            return oponentFavorite.Id == this.Id;
        }

        private List<IGroup> GetInvariantGroups()
        {
            // see also the Group.Favorites
            // prefer to select cached items, instead of selecting from database directly
            return this.groups.GetGroupsContainingFavorite(this.Id)
                .Cast<IGroup>()
                .ToList();
        }

        internal void AssignStores(Groups groups, StoredCredentials credentials,
            PersistenceSecurity persistenceSecurity, DataDispatcher dispatcher)
        {
            this.groups = groups;
            this.credentials = credentials;
            this.persistenceSecurity = persistenceSecurity;
            this.Details.Dispatcher = dispatcher;
            this.AssignStoreToRdpOptions();
        }

        private void UpdateProtocolProperties(ProtocolOptions protocolOptions)
        {
            this.protocolProperties = Favorite.UpdateProtocolPropertiesByProtocol(this.protocol, protocolOptions);
            this.AssignStoreToRdpOptions();
        }

        private void AssignStoreToRdpOptions()
        {
            Favorite.AssignStoreToRdpOptions(this.protocolProperties, this.persistenceSecurity);
        }

        internal void SaveDetails(Database database)
        {
            this.Details.Save(database);
        }

        internal void ReleaseLoadedDetails()
        {
            this.Details.ReleaseLoadedDetails();
        }

        public override String ToString()
        {
            return Favorite.ToString(this);
        }
    }
}