using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Win32;
using System.Windows.Forms;

namespace Terminals
{
    class ProtocolHandler
    {
        public static void Register()
        {
            try
            {
                RegistryKey trmKey = Registry.ClassesRoot.OpenSubKey("TRM");
                if (trmKey != null)
                {
                    trmKey.Close();
                    return;
                }
                using (trmKey = Registry.ClassesRoot.CreateSubKey("TRM"))
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
            catch(Exception ex)
            {
                //ignore any security errors and such
                Terminals.Logging.Log.Error("Error accessing registry", ex);
            }
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
