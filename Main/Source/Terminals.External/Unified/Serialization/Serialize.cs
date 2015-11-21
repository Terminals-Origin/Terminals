
using System;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Xml.Serialization;

namespace Unified
{
    public class Serialize
    {
        public static MemoryStream SerializeBinary(object request)
        {
            BinaryFormatter binaryFormatter = new BinaryFormatter();
            MemoryStream memoryStream1 = new MemoryStream();
            binaryFormatter.Serialize(memoryStream1, request);
            return memoryStream1;
        }
        public static byte[] SerializeBinaryAsBytes(object request)
        {
            using (MemoryStream stm = SerializeBinary(request))
            {
                return StreamHelper.StreamToBytes(stm);
            }
        }
        public static object DeSerializeBinary(MemoryStream memStream)
        {
            memStream.Position = (long)0;
            object local1 = new BinaryFormatter().Deserialize(memStream);
            memStream.Close();
            return local1;
        }

        public static object DeSerializeXML(MemoryStream memStream, Type type, bool ThrowException)
        {
            object local2;

            if (memStream.Position > (long)0 && memStream.CanSeek)
            {
                memStream.Position = (long)0;
            }

            try
            {
                XmlSerializer xmlSerializer = new XmlSerializer(type);
                local2 = xmlSerializer.Deserialize(memStream);
            }
            catch
            {
                local2 = null;
                if (ThrowException)
                    throw;
            }

            return local2;
        }

        public static object DeSerializeXML(MemoryStream memStream, Type type)
        {
            object local2;

            if (memStream.Position > (long)0 && memStream.CanSeek)
            {
                memStream.Position = (long)0;
            }

            try
            {
                XmlSerializer xmlSerializer = new XmlSerializer(type);
                local2 = xmlSerializer.Deserialize(memStream);
            }
            catch (Exception)
            {
                local2 = null;
            }

            return local2;
        }

        public static byte[] SerializeXMLAsBytes(object request)
        {
            using (MemoryStream stm = SerializeXML(request))
            {
                return StreamHelper.StreamToBytes(stm);
            }
        }

        public static void SerializeXMLToDisk(object request, string Filename)
        {
            var xmlSerializer = new XmlSerializer(request.GetType());
            SerializeXMLToDisk(request, Filename, xmlSerializer);
        }

        public static void SerializeXMLToDisk(object request, string Filename, XmlAttributeOverrides attributes)
        {
            var xmlSerializer = new XmlSerializer(request.GetType(), attributes);
            SerializeXMLToDisk(request, Filename, xmlSerializer);
        }

        public static void SerializeXMLToDisk(object request, string Filename, XmlSerializer xmlSerializer)
        {
            if (System.IO.File.Exists(Filename))
                System.IO.File.Delete(Filename);

            string serialized = SerializeXMLAsString(request, xmlSerializer);
            System.IO.File.WriteAllText(Filename, serialized, Encoding.UTF8);
        }

        public static string SerializeXMLAsString(object request)
        {
            return SerializeXMLAsString(request, new XmlSerializer(request.GetType()));
        }

        public static string SerializeXMLAsString(object request, XmlSerializer serializer)
        {
            using (MemoryStream stm = SerializeXML(request, serializer))
            {
                return StreamHelper.StreamToString(stm);
            }
        }

        public static MemoryStream SerializeXML(object request)
        {
            var xmlSerializer = new XmlSerializer(request.GetType());
            return SerializeXML(request, xmlSerializer);
        }

        public static MemoryStream SerializeXML(object request, Type type, bool ThrowException)
        {
            MemoryStream memoryStream2;

            try
            {
                XmlSerializer xmlSerializer = new XmlSerializer(type);
                MemoryStream memoryStream1 = new MemoryStream();
                xmlSerializer.Serialize(memoryStream1, request);
                memoryStream2 = memoryStream1;
            }
            catch
            {
                memoryStream2 = null;
                if (ThrowException) 
                    throw;
            }

            return memoryStream2;
        }

        public static MemoryStream SerializeXML(object request, XmlSerializer xmlSerializer)
        {
            MemoryStream memoryStream2;

            try
            {
                MemoryStream memoryStream1 = new MemoryStream();
                xmlSerializer.Serialize(memoryStream1, request);
                memoryStream2 = memoryStream1;
            }
            catch (Exception)
            {
                memoryStream2 = null;
            }

            return memoryStream2;
        }

        public static object DeserializeXMLFromDisk(string Filename, Type type)
        {
            return DeserializeXMLFromDisk(Filename, type, new XmlSerializer(type));
        }

        public static object DeserializeXMLFromDisk(string Filename, Type type, XmlAttributeOverrides attributes)
        {
            var xmlSerializer = new XmlSerializer(type, attributes);
            return DeserializeXMLFromDisk(Filename, type, xmlSerializer);
        }

        public static object DeserializeXMLFromDisk(string Filename, Type type, XmlSerializer xmlSerializer)
        {
            if (File.Exists(Filename))
            {
                string contents = File.ReadAllText(Filename);
                return DeSerializeXML(contents, xmlSerializer);
            }

            return Activator.CreateInstance(type);
        }

        public static object DeSerializeXML(string envelope, Type type, bool ThrowException)
        {
            object local2;
            try
            {
                XmlSerializer xmlSerializer = new XmlSerializer(type);
                MemoryStream memoryStream = new MemoryStream(Encoding.UTF8.GetBytes(envelope));
                object local1 = xmlSerializer.Deserialize(memoryStream);
                memoryStream.Close();
                local2 = local1;
            }
            catch
            {
                local2 = null;
                if (ThrowException) 
                    throw;
            }

            return local2;
        }

        public static object DeSerializeXML(string envelope, Type type)
        {
            return DeSerializeXML(envelope, new XmlSerializer(type));
        }

        public static object DeSerializeXML(string envelope, XmlSerializer xmlSerializer)
        {
            object local2;
            try
            {

                MemoryStream memoryStream = new MemoryStream(Encoding.UTF8.GetBytes(envelope));
                object local1 = xmlSerializer.Deserialize(memoryStream);
                memoryStream.Close();
                local2 = local1;
            }
            catch (Exception)
            {
                local2 = null;
            }

            return local2;
        }
    }
}