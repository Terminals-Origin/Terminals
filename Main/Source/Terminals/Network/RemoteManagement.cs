using System;
using System.Management;
using System.Net;
using Microsoft.Win32;
using Terminals.Data;
using Terminals.Data.Credentials;

namespace Terminals.Network
{
    internal enum ShutdownCommands
    {
        LogOff = 0,
        ForcedLogOff = 4,
        Shutdown = 1,
        ForcedShutdown = 5,
        Reboot = 2,
        ForcedReboot = 6,
        PowerOff = 8,
        ForcedPowerOff = 12
    }

    /// <summary>
    /// Wrapper for remote windows machines management.
    /// </summary>
    internal static class RemoteManagement
    {
        internal static bool ForceShutdown(PersistenceSecurity persistenceSecurity, IFavorite favorite, ShutdownCommands shutdownCommand)
        {
            var guarded = new GuardedSecurity(persistenceSecurity, favorite.Security);
            var security = guarded.GetResolvedCredentials();
            var credentials = new NetworkCredential(security.UserName, security.Password, security.Domain);
            return ForceShutdown(favorite.ServerName, shutdownCommand, credentials) == 0;
        }

        /// <summary>
        /// Send a shutdown command to a (remote) computer.
        /// </summary>
        /// <param name="machineName">The machinename or ip-address of computer to send shutdown command to.</param>
        /// <param name="shutdownCommand">Shutdown type command.</param>
        /// <param name="credentials">Optional network credentials for the (remote) computer.</param>
        /// <returns>0 if the shutdown was succesfully send, else another integer value.</returns>
        /// <exception cref="ManagementException">An unhandled managed error occured.</exception>
        /// <exception cref="UnauthorizedAccessException">Access was denied.</exception>
        private static int ForceShutdown(String machineName, ShutdownCommands shutdownCommand, NetworkCredential credentials)
        {
            ConnectionOptions options = CreateOptions(credentials);
            var scope = new ManagementScope(String.Format("\\\\{0}\\root\\cimv2", machineName), options);
            scope.Connect();

            var query = new SelectQuery("Win32_OperatingSystem");
            using (var searcher = new ManagementObjectSearcher(scope, query))
            {
                foreach (ManagementObject os in searcher.Get())
                {
                    ManagementBaseObject inParams = os.GetMethodParameters("Win32Shutdown");
                    inParams["Flags"] = (Int32)shutdownCommand;

                    using (ManagementBaseObject outParams = os.InvokeMethod("Win32Shutdown", inParams, null))
                    {
                        return Convert.ToInt32(outParams.Properties["returnValue"].Value);
                    }
                }
            }

            return -1;
        }

        private static ConnectionOptions CreateOptions(NetworkCredential credentials)
        {
            var options = new ConnectionOptions();
            if (credentials != null)
            {
                options.EnablePrivileges = true;
                options.Username = String.IsNullOrEmpty(credentials.Domain) ? credentials.UserName : FormatUserName(credentials);
                options.Password = credentials.Password;
            }
            return options;
        }

        private static string FormatUserName(NetworkCredential credentials)
        {
            return String.Format("{0}\\{1}", credentials.Domain, credentials.UserName);
        }

        /// <summary>
        /// Tryes to enable the RDP protocol on remote computer.
        /// This action requires remote access to the registry and admin priviledges.
        /// </summary>
        /// <param name="favorite">Not null favorite to use as target machine of the operation</param>
        /// <returns>Null, if was not able to perform the action, True in case of success, False when the protocol is already enabled.</returns>
        internal static bool? EnableRdp(IFavorite favorite)
        {
            using (var reg = RegistryKey.OpenRemoteBaseKey(RegistryHive.LocalMachine, favorite.ServerName))
            {
                RegistryKey ts = reg.OpenSubKey(@"SYSTEM\CurrentControlSet\Control\Terminal Server", true);
                Object denyValue = ts.GetValue("fDenyTSConnections");
                if (denyValue != null)
                {
                    Int32 isdenied = Convert.ToInt32(denyValue);
                    if (isdenied == 1)
                    {
                        ts.SetValue("fDenyTSConnections", 0);
                        return true;
                    }

                    return false;
                }

                return null;
            }
        }
    }
}
