using System;
using System.Collections.Generic;
using System.IO;
using System.Xml;
using System.Xml.Linq;
using System.Xml.Serialization;
using Terminals.Connections;

namespace Terminals.Data.FilePersisted
{
    internal class FavoritesFileSerializer
    {
        private readonly ConnectionManager connectinManager;

        internal FavoritesFileSerializer(ConnectionManager connectinManager)
        {
            this.connectinManager = connectinManager;
        }

        internal void Serialize(SerializationContext context, string fileName)
        {
            try
            {
                this.TrySerialize(context, fileName);
            }
            catch (Exception exception)
            {
                Logging.Error("Unable to save favorites file." , exception);
            }
        }

        private void TrySerialize(SerializationContext context, string fileName)
        {
            var document = new XDocument();
            this.Serialize(context, document);
            FavoritesXmlFile file = FavoritesXmlFile.CreateDocument(document);
            file.AppendUnknownFavorites(context.Unknown);
            document.Save(fileName);
        }

        private void Serialize(SerializationContext context, XDocument document)
        {
            using (XmlWriter writer = document.CreateWriter())
            {
                XmlSerializer serializer = this.CreateSerializer();
                serializer.Serialize(writer, context.File);
            }
        }

        internal SerializationContext Deserialize(string fileLocation)
        {
            try
            {
                if (!File.Exists(fileLocation))
                    return new SerializationContext();

                return this.TryDeserialize(fileLocation);
            }
            catch
            {
                return new SerializationContext();
            }
        }

        private SerializationContext TryDeserialize(string fileLocation)
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
