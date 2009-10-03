using System;
using System.Collections.Generic;
using System.Text;

namespace Terminals.Credentials
{
    [Serializable()]
    public class CredentialSet : ICloneable
    {
        public string Domain { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }

        public override string ToString()
        {
            return string.Format(@"\\{0}\{1}", Domain, Username);
        }

        #region ICloneable Members

        public object Clone()
        {
            CredentialSet s = new CredentialSet();
            s.Username = this.Username;
            s.Domain = this.Domain;
            s.Password = this.Password;
            return s;
        }

        #endregion
    }
}
