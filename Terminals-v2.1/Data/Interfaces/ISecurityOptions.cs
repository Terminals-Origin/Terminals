using System;

namespace Terminals.Data
{
    internal interface ISecurityOptions : ICredentialBase
    {
        Guid Credential { get; set; }

        ISecurityOptions Copy();

        /// <summary>
        /// Gets this credentails replaced first by Stored credential and then by default
        /// stored credentials for each value, if the value is empty
        /// </summary>
        ISecurityOptions GetResolvedCredentials();

        void UpdateFromCredential(ICredentialSet source);
    }
}