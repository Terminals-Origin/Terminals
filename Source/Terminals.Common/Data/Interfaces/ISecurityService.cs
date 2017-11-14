using System.Collections.Generic;
using System.Windows.Forms;
using Terminals.Data;

namespace Terminals.Common.Data.Interfaces
{
    public interface ISecurityService
    {
        IGuardedCredential FromCredentialSet(ICredentialSet credentialSet);

        IGuardedCredential FromSecurityOption(ISecurityOptions securityOptions);

        Form GetCredentialForm();

        IEnumerable<ICredentialSet> Credentials { get; }
    }
}
