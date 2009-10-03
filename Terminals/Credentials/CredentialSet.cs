using System;
using System.Collections.Generic;
using System.Text;

namespace Terminals.Credentials
{
    [Serializable()]
    public class CredentialSet
    {
        public string Domain { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }

        public override string ToString()
        {
            return string.Format(@"\\{0}\{1}", Domain, Username);
        }
    }
}
