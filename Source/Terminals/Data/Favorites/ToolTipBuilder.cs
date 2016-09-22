using System;
using Terminals.Configuration;
using Terminals.Data.Credentials;
using Terminals.Network;

namespace Terminals.Data
{
    internal class ToolTipBuilder
    {
        private readonly PersistenceSecurity persistenceSecurity;

        public ToolTipBuilder(PersistenceSecurity persistenceSecurity)
        {
            this.persistenceSecurity = persistenceSecurity;
        }

        internal string BuildTooTip(IFavorite selected)
        {
            var guarded = new GuardedCredential(selected.Security, this.persistenceSecurity);
            string userDisplayName = HelperFunctions.UserDisplayName(guarded.Domain, guarded.UserName);
            return GetToolTipText(selected, userDisplayName);
        }

        internal static String GetToolTipText(IFavorite selected, string userDisplayName)
        {
            String toolTip = String.Format("Computer: {1}{0}Port: {2}{0}User: {3}{0}",
                Environment.NewLine, selected.ServerName, selected.Port, userDisplayName);

            if (Settings.Instance.ShowFullInformationToolTips)
            {
                var extendedToolTip = CreateExtendedToolTip(selected);
                toolTip += extendedToolTip;
            }

            return toolTip;
        }

        private static string CreateExtendedToolTip(IFavorite selected)
        {
            var rdp = selected.ProtocolProperties as RdpOptions;
            bool console = false;
            if (rdp != null)
                console = rdp.ConnectToConsole;

            return String.Format("Groups: {1}{0}Connect to Console: {2}{0}Notes: {3}{0}",
                                 Environment.NewLine, selected.GroupNames, console, selected.Notes);
        }
    }
}