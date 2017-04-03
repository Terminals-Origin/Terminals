using System;

namespace Terminals.Data
{
    [Serializable]
    public class SecurityOptions : CredentialBase, ISecurityOptions
    {
        private Guid credential = Guid.Empty;

        /// <summary>
        /// Gets or sets the credential unique identifier in text form.
        /// Only for serialization to prevent serialization of empty ids.
        /// </summary>
        public string Credential
        {
            get
            {
                if (this.credential == Guid.Empty)
                    return null;

                return this.credential.ToString();
            }
            set
            {
                if (string.IsNullOrEmpty(value))
                    this.credential = Guid.Empty;
                else
                    this.credential = new Guid(value);
            }
        }

        Guid ISecurityOptions.Credential
        {
            get { return this.credential; }
            set { this.credential = value; }
        }

        public SecurityOptions()
        {
        }

        public ISecurityOptions Copy()
        {
            var copy = new SecurityOptions{ Credential = this.Credential };
            copy.UpdateFrom(this);
            return copy;
        }

        public override string ToString()
        {
            return string.Format("SecurityOptions:Credential='{0}'", this.Credential);
        }
    }
}
