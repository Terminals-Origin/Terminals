using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Terminals.TerminalServices
{
    public class GatewayCredentialsSources
    {
        private static List<GatewayCredentialsSources> lst;
        public static List<GatewayCredentialsSources> Sources
        {
            get
            {
                if (lst != null) return lst;
                lst = new List<GatewayCredentialsSources>();
                lst.Add(new GatewayCredentialsSources() { ID = 0, DisplayName = "Ask for Password (NTLM)" });
                lst.Add(new GatewayCredentialsSources() { ID = 1, DisplayName = "Smart Card" });
                lst.Add(new GatewayCredentialsSources() { ID = 4, DisplayName = "Allow user to select later" });
                return lst;
            }
        }

        public int ID { get; set; }
        public string DisplayName { get; set; }

        public override string ToString()
        {
            return DisplayName;
        }
    }
}
