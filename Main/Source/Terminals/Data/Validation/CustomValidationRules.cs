using System;
using System.ComponentModel.DataAnnotations;
using Terminals.Configuration;
using Terminals.Connections;
using Terminals.Data.DB;

namespace Terminals.Data.Validation
{
    public class CustomValidationRules
    {
        public static ValidationResult IsValidServerName(string serverName)
        {
            // todo add ConnectionManager.IsProtocolWebBased(protocol)
            if (Settings.ForceComputerNamesAsURI && Uri.CheckHostName(serverName) == UriHostNameType.Unknown)
                return new ValidationResult("Server name is not in the correct format.", new string[] { "ServerName" });

            return ValidationResult.Success;
        }

        public static ValidationResult IsKnownProtocol(string protocol)
        {
            if (ConnectionManager.IsKnownProtocol(protocol))
                return ValidationResult.Success;

            return new ValidationResult(Validations.UNKNOWN_PROTOCOL, new string[] { "Protocol" });
        }
    }
}