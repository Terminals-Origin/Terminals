using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SqlScriptRunner.Serialization
{
    public class Serializer<T>
    {

        public static string SerializeToString(T Input, ISerialize<T> Serializer = null)
        {
            if (Serializer == null) Serializer = new JsonSerializer<T>();
            return Serializer.SerializeToString(Input);
        }

        public static byte[] SerializeToBytes(T Input, ISerialize<T> Serializer = null)
        {
            if (Serializer == null) Serializer = new JsonSerializer<T>();
            return Serializer.SerializeToBytes(Input);
        }

        public static T DeserializeString(string Input, ISerialize<T> Serializer = null)
        {
            if (Serializer == null) Serializer = new JsonSerializer<T>();
            return Serializer.DeserializeString(Input);
        }

        public static T DeserializeBytes(byte[] Input, ISerialize<T> Serializer = null)
        {
            if (Serializer == null) Serializer = new JsonSerializer<T>();
            return Serializer.DeserializeBytes(Input);
        }

    }
}
