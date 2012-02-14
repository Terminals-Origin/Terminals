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
            get { return this.Groups.Cast<IGroup>().ToList(); }
        }

        private ProtocolOptions protocolProperties;
        ProtocolOptions IFavorite.ProtocolProperties
        {
            get
            {
                DeserializeProperties();
                return this.protocolProperties;
            }
            set
            {
                this.protocolProperties = value;
                SerializeProperties(value);
            }
        }

        private void SerializeProperties(ProtocolOptions value)
        {
            this.ProtocolProperties = Serialize.SerializeXMLAsString(value);
        }

        private void DeserializeProperties()
        {
            try
            {
                Type propertiesType = GetPropertiesTypeByProtocol();
                if (this.protocolProperties == null)
                    this.protocolProperties = Serialize.DeSerializeXML(this.ProtocolProperties, propertiesType) as ProtocolOptions;

            }
            catch
            {
                this.protocolProperties = new RdpOptions();
            }
        }

        private Type GetPropertiesTypeByProtocol()
        {
            throw new NotImplementedException();
        }

        public string ToolBarIcon
        {
            get { throw new NotImplementedException(); }
            set { throw new NotImplementedException(); }
        }

        public string GroupNames
        {
            get { throw new NotImplementedException(); }
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

            copy.ProtocolProperties = this.ProtocolProperties;

            return copy; 
        }

        public string GetToolTipText()
        {
           return Terminals.Data.Favorite.GetToolTipText(this);
        }

        public int CompareByDefaultSorting(IFavorite target)
        {
            throw new NotImplementedException();
        }

        public void UpdatePasswordsByNewKeyMaterial(string newKeyMaterial)
        {
            throw new NotImplementedException();
        }
    }
}
