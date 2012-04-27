using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using Terminals.Connections;
using Unified;

namespace Terminals.Data.DB
{
    internal partial class Favorite : IFavorite, IEntityContext
    {
        public Database Database { get; set; }

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
                this.ExecuteBeforeConnectReference.Load();
                return this.ExecuteBeforeConnect; 
            }
        }

        IDisplayOptions IFavorite.Display
        {
            get
            {
                this.DisplayReference.Load();
                return this.Display;
            }
        }

        ISecurityOptions IFavorite.Security
        {
            get
            {
                this.SecurityReference.Load();
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

        public string ToolBarIcon
        {
            get
            {
                return string.Empty;
                // todo ToolBarIcon throw new NotImplementedException();
            }
            set
            {
                //throw new NotImplementedException();
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
            copy.ToolBarIcon = this.ToolBarIcon;

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
        /// Initializes new instance of a favorite and sets its properties to default values,
        /// which arent defined by database.
        /// </summary>
        public Favorite()
        {
            this._Protocol = ConnectionManager.RDP;
            this._Port = ConnectionManager.RDPPort;
            this.protocolProperties = new RdpOptions();
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
            if (database != null)
            {
                try
                {
                    string serializedProperties = Serialize.SerializeXMLAsString(this.protocolProperties);
                    database.UpdateFavoriteProtocolProperties(this.Id, serializedProperties);
                }
                catch (Exception exception)
                {
                    Logging.Log.Error("Couldnt save protocol properties to database", exception);
                    // we dont have chance to 
                }
            }
        }

        /// <summary>
        /// Realization of expensive property lazy loading
        /// </summary>
        private void LoadProtocolProperties()
        {
            try
            {
                if (this.Database != null)
                {
                    string serializedProperties = this.Database.GetFavoriteProtocolProperties(this.Id).FirstOrDefault();
                    Type propertiesType = this.protocolProperties.GetType();
                    this.protocolProperties =
                        Serialize.DeSerializeXML(serializedProperties, propertiesType) as ProtocolOptions;
                }
            }
            catch (Exception exception)
            {
                Logging.Log.Error("Couldnt obtain protocol properties from database", exception);
                this.protocolProperties = Data.Favorite.UpdateProtocolPropertiesByProtocol(this.Protocol, new RdpOptions());
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