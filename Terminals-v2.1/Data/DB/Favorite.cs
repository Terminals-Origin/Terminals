using System;
using System.Collections.Generic;
using System.Linq;
using Unified;

namespace Terminals.Data.DB
{
    internal partial class Favorite : IFavorite
    {
        private Guid guid = Guid.NewGuid();
        Guid IFavorite.Id
        {
            get { return this.guid; }
            set { this.guid = value; }
        }

        public Guid Guid
        {
            get { return this.guid; }
            set { this.guid = value; }
        }

        IBeforeConnectExecuteOptions IFavorite.ExecuteBeforeConnect
        {
            get { return this.ExecuteBeforeConnect; }
        }

        IDisplayOptions IFavorite.Display
        {
            get { return this.Display; }
        }

        ISecurityOptions IFavorite.Security
        {
            get { return this.Security; }
        }

        List<IGroup> IFavorite.Groups
        {
            get { return GetInvariantGroups(); }
        }

        private List<IGroup> GetInvariantGroups()
        {
            return this.Groups.Cast<IGroup>().ToList();
        }

        private ProtocolOptions protocolProperties;
        /// <summary>
        /// Gets or sets the protocol specific container. This isnt a part of an entity,
        /// because we are using lazy loading of this property and we dont want to cache
        /// its xml persisted content.
        /// </summary>
        public ProtocolOptions ProtocolProperties
        {
            get
            {
                if (this.protocolProperties == null) // expensive property lazy loading
                    DeserializeProperties();
                return this.protocolProperties;
            }
            set
            {
                this.protocolProperties = value;
                SerializeProperties(value); // todo dont commit the protocol proerties immediately
            }
        }

        private void SerializeProperties(ProtocolOptions value)
        {
            string serializedProperties = Serialize.SerializeXMLAsString(value);
            var database = new DataBase();
            //database.
        }

        private void DeserializeProperties()
        {
            this.protocolProperties = Data.Favorite.UpdateProtocolPropertiesByProtocol(this.Protocol, new RdpOptions());
            try
            {
                using (var database = new DataBase())
                {
                    string serializedProperties = database.GetFavoriteProtocolProperties(this.Id).FirstOrDefault();
                    this.protocolProperties =
                        Serialize.DeSerializeXML(serializedProperties, this.protocolProperties.GetType()) as ProtocolOptions;
                }
            }
            catch(Exception exception)
            {
                Logging.Log.Error("Couldnt obtain protocol properties from database", exception);
            }
        }

        public string ToolBarIcon
        {
            get { throw new NotImplementedException(); }
            set { throw new NotImplementedException(); }
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

        public override String ToString()
        {
            return Data.Favorite.ToString(this);
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
    }
}
