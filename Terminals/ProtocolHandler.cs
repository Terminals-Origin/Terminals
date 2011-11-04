using System;
using Microsoft.Win32;
using System.Windows.Forms;

namespace Terminals
{
    internal class ProtocolHandler
    {
        private const string TRM_REGISTRY = "TRM";

        internal static void Register()
        {
            try
            {
                if(IsTrmKeyRegistred())
                  return;

                CreateTrmRegistrySubKey();
            }
            catch(Exception ex)
            {
                //ignore any security errors and such
                Logging.Log.Error("Error accessing registry", ex);
            }
        }

        private static void CreateTrmRegistrySubKey()
        {
            using (RegistryKey trmKey = Registry.ClassesRoot.CreateSubKey(TRM_REGISTRY))
            {
                trmKey.SetValue(null, "URL:Terminals Protocol");
                trmKey.SetValue("URL Protocol", "");
                using (RegistryKey shellKey = trmKey.CreateSubKey("shell"))
                {
                    shellKey.SetValue(null, "open");
                    using (RegistryKey commandKey = shellKey.CreateSubKey("open\\command"))
                    {
                        commandKey.SetValue(null, Application.ExecutablePath + " /reuse /url:\"%1\"");
                    }
                }
            }
        }

        private static bool IsTrmKeyRegistred()
        {
            RegistryKey trmKey = Registry.ClassesRoot.OpenSubKey(TRM_REGISTRY);
            if (trmKey != null)
            {
                trmKey.Close();
                return true;
            }

            return false;
        }

        public static void Parse(string url, out string server, out int port)
        {
            server = url.Substring(("trm://").Length);
            if (server.EndsWith("/"))
                server = server.TrimEnd('/');
            port = 0;
            string[] serverParams = server.Split(':');
            if (serverParams.Length == 2)
            {
                server = serverParams[0];
                port = Int32.Parse(serverParams[1]);
            }
        }
    }
}
