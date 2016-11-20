using System;
using System.ComponentModel.DataAnnotations;
using Terminals.Configuration;
using Terminals.Connections;

namespace Terminals.Data.Validation
{
    public class CustomValidationRules
    {
        internal const string METHOD_ISVALIDSERVERNAME = "IsValidServerName";

        internal const string METHOD_ISKNOWNPROTOCOL = "IsKnownProtocol";

        internal const string SERVER_NAME_IS_NOT_IN_THE_CORRECT_FORMAT = "Server name is not in the correct format.";

        public static ValidationResult IsValidServerName(string serverName)
        {
            if (IsValidServerNameB(serverName))
                return new ValidationResult(SERVER_NAME_IS_NOT_IN_THE_CORRECT_FORMAT, new string[] { "ServerName" });

            return ValidationResult.Success;
        }

        internal static bool IsValidServerNameB(string serverName)
        {
            return Settings.Instance.ForceComputerNamesAsURI && Uri.CheckHostName(serverName) == UriHostNameType.Unknown;
        }
    }
}