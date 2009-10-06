using System;
using System.Collections.Generic;
using System.Text;

namespace Terminals.Credentials
{
    [Serializable()]
    public class CredentialSet : ICloneable
    {
        public string Name { get; set; }
        public string Domain { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }

        public override string ToString()
        {
            return Name;
        }

        public static CredentialSet CredentialByName(string Name)
        {
            List<CredentialSet> list = Settings.SavedCredentials;
            if (string.IsNullOrEmpty(Name) || list == null || list.Count < 1)
                return null;
            foreach (CredentialSet set in list)
            {
                if (set.Name == Name) return set;
            }
            return null;
        }


        #region ICloneable Members

        public object Clone()
        {
            CredentialSet s = new CredentialSet();
            s.Name = this.Name;
            s.Username = this.Username;
            s.Domain = this.Domain;
            s.Password = this.Password;
                return s;
        }

        #endregion
    }
}
