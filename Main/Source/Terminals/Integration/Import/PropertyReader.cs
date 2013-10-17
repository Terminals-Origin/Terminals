using System;
using System.Xml;

namespace Terminals.Integration.Import
{
    /// <summary>
    /// Wrapped XmlTextReader to provide facade to the property parsing
    /// </summary>
    internal class PropertyReader
    {
        private readonly XmlTextReader innerReader;

        internal XmlNodeType NodeType { get { return this.innerReader.NodeType; } }

        internal string NodeName { get { return this.innerReader.Name; } }

        public PropertyReader(XmlTextReader innerReader)
        {
            this.innerReader = innerReader;
        }

        public bool Read()
        {
            return this.innerReader.Read();
        }

        public string ReadString()
        {
            return this.innerReader.ReadString().Trim();
        }

        internal bool ReadBool()
        {
            bool tmp = false;
            bool.TryParse(this.ReadString(), out tmp);
            return tmp;
        }

        internal int ReadInt()
        {
            int tmp = 0;
            int.TryParse(this.ReadString(), out tmp);
            return tmp;
        }

        internal DesktopSize ReadDesktopSize()
        {
            DesktopSize tmp = DesktopSize.AutoScale;
            string str = this.ReadString();
            if (!String.IsNullOrEmpty(str))
                tmp = (DesktopSize)Enum.Parse(typeof(DesktopSize), str);
            return tmp;
        }

        internal Colors ReadColors()
        {
            Colors tmp = Colors.Bit16;
            string str = this.ReadString();
            if (!String.IsNullOrEmpty(str))
                tmp = (Colors)Enum.Parse(typeof(Colors), str);
            return tmp;
        }

        internal RemoteSounds ReadRemoteSounds()
        {
            RemoteSounds tmp = RemoteSounds.DontPlay;
            string str = this.ReadString();
            if (!String.IsNullOrEmpty(str))
                tmp = (RemoteSounds)Enum.Parse(typeof(RemoteSounds), str);
            return tmp;
        }

        internal SSHClient.AuthMethod ReadAuthMethod()
        {
            SSHClient.AuthMethod tmp = SSHClient.AuthMethod.Password;
            string str = this.ReadString();
            if (!String.IsNullOrEmpty(str))
                tmp = (SSHClient.AuthMethod)Enum.Parse(typeof(SSHClient.AuthMethod), str);
            return tmp;
        }

        public override string ToString()
        {
            return string.Format("PropertyReader:NodeType={0},NodeName={1}", this.NodeType, this.NodeName);
        }
    }
}