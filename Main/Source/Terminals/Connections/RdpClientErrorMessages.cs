using System;
using AxMSTSCLib;

namespace Terminals.Connections
{
    /// <summary>
    /// Translates error codes to client messages
    /// </summary>
    internal class RdpClientErrorMessages
    {
        internal static string ToDisconnectMessage(AxMsRdpClient6 client, int reason)
        {
            switch (reason)
            {
                case 1:
                case 2:
                case 3:
                    // These are normal disconnects and not considered errors.
                    return String.Empty;

                default:
                    return client.GetErrorDescription((uint)reason, (uint)client.ExtendedDisconnectReason);
            }
        }

        internal static string ToFatalErrorMessage(int errorCode)
        {
            switch (errorCode)
            {
                case 0:
                    return "An unknown error has occurred.";
                case 1:
                    return "Internal error code 1.";
                case 2:
                    return "An out-of-memory error has occurred.";
                case 3:
                    return "A window-creation error has occurred.";
                case 4:
                    return "Internal error code 2.";
                case 5:
                    return "Internal error code 3. This is not a valid state.";
                case 6:
                    return "Internal error code 4.";
                case 7:
                    return "An unrecoverable error has occurred during client connection.";
                case 100:
                    return "Winsock initialization error.";
                default:
                    return "An unknown error.";
            }
        }

        internal static string ToLogonMessage(int errorCode)
        {
            switch (errorCode)
            {
                case -5:
                    return "Winlogon is displaying the Session Contention dialog box.";
                case -2:
                    return "Winlogon is continuing with the logon process.";
                case -3:
                    return "Winlogon is ending silently.";
                case -6:
                    return "Winlogon is displaying the No Permissions dialog box.";
                case -7:
                    return "Winlogon is displaying the Disconnect Refused dialog box.";
                case -4:
                    return "Winlogon is displaying the Reconnect dialog box.";
                case -1:
                    return "The user was denied access.";
                case 0:
                    return "The logon failed because the logon credentials are not valid.";
                case 2:
                    return "Another logon or post-logon error occurred. The Remote Desktop client displays a logon screen to the user.";
                case 1:
                    return "The password is expired. The user must update their password to continue logging on.";
                case 3:
                    return "The Remote Desktop client displays a dialog box that contains important information for the user.";
                case -1073741714:
                    return "The user name and authentication information are valid, but authentication was blocked due to restrictions on the user account, such as time-of-day restrictions.";
                case -1073741715:
                    return "The attempted logon is not valid. This is due to either an incorrect user name or incorrect authentication information.";
                case -1073741276:
                    return "The password is expired. The user must update their password to continue logging on.";
                default:
                    return "An unknown error.";
            }
        }

        internal static string ToWarningMessage(int warningCode)
        {
            switch (warningCode)
            {
                case 1:
                    return "Bitmap cache is corrupt.";
                default:
                    return "An unknown warning";
            }
        }
    }
}