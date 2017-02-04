using System;
using System.Collections.Generic;
using System.IO;
using System.Xml;
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

        public void SerializeContext(SerializationContext context, string fileName)
        {
            var document = new XDocument();
            using (XmlWriter writer = document.CreateWriter())
            {
                XmlSerializer serializer = this.CreateSerializer();
                serializer.Serialize(writer, context.File);
            }

            FavoritesXmlFile file = FavoritesXmlFile.CreateDocument(document);
            file.AppendUnknownFavorites(context.Unknown);


            document.Save(fileName);
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
            FavoritesXmlFile document = FavoritesXmlFile.LoadXmlDocument(fileLocation);
            List<XElement> unknownFavorites = document.RemoveUnknownFavorites(availableProtocols);
            XmlSerializer serializer = this.CreateSerializer();
            FavoritesFile loaded = DeSerialize(document, serializer);

            if (loaded != null)
                return new SerializationContext(loaded, unknownFavorites);

            return new SerializationContext();
        }

        private static FavoritesFile DeSerialize(FavoritesXmlFile document, XmlSerializer serializer)
        {
            using (XmlReader xmlReader = document.CreateReader())
            {
                return serializer.Deserialize(xmlReader) as FavoritesFile;
            }
        }

        public FavoritesFile Deserialize(string fileLocation)
        {
            XmlAttributeOverrides attributes = this.CreateAttributes();
            object deserialized = Serialize.DeserializeXMLFromDisk(fileLocation, typeof(FavoritesFile), attributes);
            return deserialized as FavoritesFile;
        }

        private XmlSerializer CreateSerializer()
        {
            XmlAttributeOverrides attributes = this.CreateAttributes();
            return new XmlSerializer(typeof(FavoritesFile), attributes);
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
