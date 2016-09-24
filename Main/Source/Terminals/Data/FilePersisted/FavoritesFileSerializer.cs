using System;
using System.Xml.Serialization;
using Terminals.Connections;
using Unified;

namespace Terminals.Data.FilePersisted
{
    internal class FavoritesFileSerializer
    {
        private readonly ConnectionManager connectinManager;

        public FavoritesFileSerializer(ConnectionManager connectinManager)
        {
            this.connectinManager = connectinManager;
        }

        public void SerializeToXml(FavoritesFile fileContent, string fileLocation)
        {
            XmlAttributeOverrides attributes = CreateAttributes();
            Serialize.SerializeXMLToDisk(fileContent, fileLocation, attributes);
        }

        public FavoritesFile Deserialize(string fileLocation)
        {
            XmlAttributeOverrides attributes = CreateAttributes();
            object deserialized = Serialize.DeserializeXMLFromDisk(fileLocation, typeof(FavoritesFile), attributes);
            return deserialized as FavoritesFile;
        }

        private XmlAttributeOverrides CreateAttributes()
        {
            Type[] extraTypes = connectinManager.GetAllKnownProtocolOptionTypes();
            var attributeOverrides = new XmlAttributeOverrides();
            var listAttribs = new XmlAttributes();

            foreach (Type extraType in extraTypes)
            {
                listAttribs.XmlElements.Add(new XmlElementAttribute(extraType.Name, extraType));
            }

            attributeOverrides.Add(typeof(Favorite), "ProtocolProperties", listAttribs);
            return attributeOverrides;
        }
    }
}
