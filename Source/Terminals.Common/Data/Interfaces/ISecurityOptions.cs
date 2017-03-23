using System;

namespace Terminals.Data
{
    public interface ISecurityOptions : ICredentialBase
    {
        Guid Credential { get; set; }

        ISecurityOptions Copy();
    }
}