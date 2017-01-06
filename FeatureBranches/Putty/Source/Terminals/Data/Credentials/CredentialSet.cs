using System;
using System.Xml.Serialization;

namespace Terminals.Data
{
    /// <summary>
    /// Container of stored user authentication.
    /// </summary>
    [Serializable]
    public class CredentialSet : CredentialBase, ICredentialSet
    {
        private Guid id = Guid.NewGuid();
        [XmlAttribute("id")]
        public Guid Id
        {
            get
            {
                return id;
            }
            set
            {
                id = value;
            }
        }

        private string name;
        public string Name
        {
            get { return name; }
            set
            {
                if (String.IsNullOrEmpty(value))
                    return;

                this.name = value;
            }
        }

        public override string ToString()
        {
            return String.Format(@"{0}:{1}\{2}", this.Name, "", "");
        }
    }
}
