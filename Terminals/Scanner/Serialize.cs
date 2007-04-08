
using System;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Runtime.Serialization.Formatters.Soap;
using System.Text;
using System.Xml.Serialization;

namespace Unified {
	public class Serialize {

		public static MemoryStream SerializeBinary(object request) {
			BinaryFormatter binaryFormatter = new BinaryFormatter();
			MemoryStream memoryStream1 = new MemoryStream();
			binaryFormatter.Serialize(memoryStream1, request);
			return memoryStream1;
		}

		public static object DeSerializeBinary(MemoryStream memStream) {
			memStream.Position = (long)0;
			object local1 = new BinaryFormatter().Deserialize(memStream);
			memStream.Close();
			return local1;
		}

		public static MemoryStream SerializeSOAP(object request) {
			SoapFormatter soapFormatter = new SoapFormatter();
			MemoryStream memoryStream1 = new MemoryStream();
			soapFormatter.Serialize(memoryStream1, request);
			return memoryStream1;
		}

		public static object DeSerializeSOAP(MemoryStream memStream) {
			if (memStream.Position > (long)0 && memStream.CanSeek) memStream.Position = (long)0;
			SoapFormatter soapFormatter = new SoapFormatter();
			object local1 = soapFormatter.Deserialize(memStream);
			memStream.Close();
			return local1;
		}

		public static object DeSerializeXML(MemoryStream memStream, Type type, bool ThrowException) {
			object local2;

			if (memStream.Position > (long)0 && memStream.CanSeek) {
				memStream.Position = (long)0;
			}
			try {
				XmlSerializer xmlSerializer = new XmlSerializer(type);
				local2 = xmlSerializer.Deserialize(memStream);
			}
			catch (Exception exc) {
				local2 = null;
				if(ThrowException) throw exc;
			}
			return local2;
		}

		public static object DeSerializeXML(MemoryStream memStream, Type type) {
			object local2;

			if (memStream.Position > (long)0 && memStream.CanSeek) {
				memStream.Position = (long)0;
			}
			try {
				XmlSerializer xmlSerializer = new XmlSerializer(type);
				local2 = xmlSerializer.Deserialize(memStream);
			}
			catch (Exception) {
				local2 = null;
			}
			return local2;
		}

		public static MemoryStream SerializeXML(object request) {
			return SerializeXML(request, request.GetType());
		}

		public static MemoryStream SerializeXML(object request, Type type, bool ThrowException) {
			MemoryStream memoryStream2;

			try {
				XmlSerializer xmlSerializer = new XmlSerializer(type);
				MemoryStream memoryStream1 = new MemoryStream();
				xmlSerializer.Serialize(memoryStream1, request);
				memoryStream2 = memoryStream1;
			}
			catch (Exception exc) {
				memoryStream2 = null;
				if(ThrowException) throw exc;
			}
			return memoryStream2;
		}
		public static MemoryStream SerializeXML(object request, Type type) {
			MemoryStream memoryStream2;

			try {
				XmlSerializer xmlSerializer = new XmlSerializer(type);
				MemoryStream memoryStream1 = new MemoryStream();
				xmlSerializer.Serialize(memoryStream1, request);
				memoryStream2 = memoryStream1;
			}
			catch (Exception) {
				memoryStream2 = null;
			}
			return memoryStream2;
		}

		public static object DeSerializeXML(string envelope, Type type, bool ThrowException) {
			object local2;
			try {
				XmlSerializer xmlSerializer = new XmlSerializer(type);
				MemoryStream memoryStream = new MemoryStream(Encoding.ASCII.GetBytes(envelope));
				object local1 = xmlSerializer.Deserialize(memoryStream);
				memoryStream.Close();
				local2 = local1;
			}
			catch (Exception exc) {
				local2 = null;
				if(ThrowException) throw exc;
			}
			return local2;
		}

		public static object DeSerializeXML(string envelope, Type type) {
			object local2;
			try {
				XmlSerializer xmlSerializer = new XmlSerializer(type);
				MemoryStream memoryStream = new MemoryStream(Encoding.ASCII.GetBytes(envelope));
				object local1 = xmlSerializer.Deserialize(memoryStream);
				memoryStream.Close();
				local2 = local1;
			}
			catch (Exception) {
				local2 = null;
			}
			return local2;
		}
	}

}
