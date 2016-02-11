using System;

namespace Terminals.Data
{
    public interface ISecurityOptions : ICredentialBase
    {
        Guid Credential { get; set; }

        /// <summary>
        /// Gets this credentials replaced first by Stored credential and then by default
        /// stored credentials for each value, if the value is empty
        /// </summary>
        ISecurityOptions GetResolvedCredentials();

        void UpdateFromCredential(ICredentialSet source);
    }
}