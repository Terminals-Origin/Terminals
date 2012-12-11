using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization.Json;
using System.IO;

namespace SqlScriptRunner.Serialization
{
    public class JsonSerializer<T> : ISerialize<T>
    {
        //string SerializeToString(T Input);
        //byte[] SerializeToBytes(T Input);
        //T DeserializeString(string Input);
        //T DeserializeBytes(byte[] Input);
        //string ContentType { get; }

        public string ContentType { get { return "application/json"; } }

        public string SerializeToString(T Input)
        {
            return System.Text.Encoding.UTF8.GetString(ToBytes(Input));
        }
        public byte[] SerializeToBytes(T Input)
        {
            return ToBytes(Input);
        }

        public T DeserializeString(string Input)
        {
            return FromBytes(System.Text.Encoding.UTF8.GetBytes(Input as string));
        }
        public T DeserializeBytes(byte[] Input)
        {
            return FromBytes((Input as byte[]));
        }

        private T FromBytes(byte[] Input)
        {
            using (var stm = new MemoryStream())
            {
                stm.Write(Input, 0, Input.Length);
                if (stm.CanSeek && stm.Position > 0) stm.Seek(0, SeekOrigin.Begin);
                var ser = new DataContractJsonSerializer(typeof(T));
                return (T)ser.ReadObject(stm);
            }
        }
        private byte[] ToBytes(T Input)
        {
            using (var stm = new MemoryStream())
            {
                var ser = new DataContractJsonSerializer(typeof(T));
                ser.WriteObject(stm, Input);
                if (stm.CanSeek && stm.Position > 0) stm.Seek(0, SeekOrigin.Begin);
                byte[] body = new byte[stm.Length];
                stm.Read(body, 0, (int)stm.Length);
                return body;
            }
        }
    }
}