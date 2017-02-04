using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Xml.Linq;
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
            XmlAttributeOverrides attributes = this.CreateAttributes();
            Serialize.SerializeXMLToDisk(fileContent, fileLocation, attributes);
        }

        public SerializationContext DeserializeContext(string fileLocation)
        {
            try
            {
                if (!File.Exists(fileLocation))
                    return new SerializationContext();

                return this.TryDeserializeContext(fileLocation);
            }
            catch
            {
                return new SerializationContext();
            }
        }

        private SerializationContext TryDeserializeContext(string fileLocation)
        {
            var availableProtocols = this.connectinManager.GetAvailableProtocols();
            FavoritesXmlFile document = FavoritesXmlFile.LoadXmlDocument(fileLocation, availableProtocols);
            List<XElement> unknownFavorites = document.RemoveUnknownFavorites();
            XmlAttributeOverrides attributes = this.CreateAttributes();
            var serializer = new XmlSerializer(typeof(FavoritesFile), attributes);
            var loaded = serializer.Deserialize(document.CreateReader()) as FavoritesFile;

            if (loaded != null)
                return new SerializationContext(loaded, unknownFavorites);

            return new SerializationContext();
        }

        public FavoritesFile Deserialize(string fileLocation)
        {
            XmlAttributeOverrides attributes = this.CreateAttributes();
            object deserialized = Serialize.DeserializeXMLFromDisk(fileLocation, typeof(FavoritesFile), attributes);
            return deserialized as FavoritesFile;
        }

        private XmlAttributeOverrides CreateAttributes()
        {
            Type[] extraTypes = this.connectinManager.GetAllKnownProtocolOptionTypes();
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
