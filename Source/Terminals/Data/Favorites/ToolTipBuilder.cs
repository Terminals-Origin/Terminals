using System;
using System.Text;
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

        /// <summary>
        /// Here we use the modified favorite used in connection, 
        /// becaue e.g. credentials may differ from the saved credentials.
        /// </summary>
        internal string BuildTooTip(IFavorite selected)
        {
            var guarded = new GuardedCredential(selected.Security, this.persistenceSecurity);
            string userDisplayName = HelperFunctions.UserDisplayName(guarded.Domain, guarded.UserName);
            return GetToolTipText(selected, userDisplayName);
        }

        private static String GetToolTipText(IFavorite selected, string userDisplayName)
        {
            var builder = new StringBuilder();
            builder.AppendFormat("Computer: {1}{0}Port: {2}{0}User: {3}{0}",
                Environment.NewLine, selected.ServerName, selected.Port, userDisplayName);

            if (Settings.Instance.ShowFullInformationToolTips)
                AppendExtendedToolTip(builder, selected);

            return builder.ToString();
        }

        private static void AppendExtendedToolTip(StringBuilder builder, IFavorite selected)
        {
            var rdp = selected.ProtocolProperties as IForceConsoleOptions;
            bool console = false;
            if (rdp != null)
                console = rdp.ConnectToConsole;

            builder.AppendFormat("Groups: {1}{0}Connect to Console: {2}{0}Notes: {3}{0}",
                                 Environment.NewLine, selected.GroupNames, console, selected.Notes);
        }
    }
}