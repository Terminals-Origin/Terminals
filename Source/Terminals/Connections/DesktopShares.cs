using System;
namespace Terminals.Connections
{
    /// <summary>
    /// Resolves desktop share path do be able drag and drop files into the connection user control.
    /// </summary>
    internal class DesktopShares
    {
        private readonly IConnectionExtra connection;
        private readonly string defaultDesktopShare;

        internal DesktopShares(IConnectionExtra connection, string defaultDesktopShare)
        {
            this.connection = connection;
            this.defaultDesktopShare = defaultDesktopShare;
        }

        internal string EvaluateDesktopShare(string currentDesktopShare)
        {
            if (String.IsNullOrEmpty(currentDesktopShare))
                return EvaluateDesktopShare();

            return currentDesktopShare;
        }

        internal string EvaluateDesktopShare()
        {
            if (this.connection == null)
                return String.Empty;

            return this.defaultDesktopShare.Replace("%SERVER%", this.connection.Server)
                .Replace("%USER%", this.connection.UserName)
                .Replace("%server%", this.connection.Server)
                .Replace("%user%", this.connection.UserName);
        }
    }
}
