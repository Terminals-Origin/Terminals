using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SqlScriptRunner.Serialization
{
    public interface ISerialize<T>
    {
        string SerializeToString(T Input);
        byte[] SerializeToBytes(T Input);
        T DeserializeString(string Input);
        T DeserializeBytes(byte[] Input);
        string ContentType { get; }
    }
}
