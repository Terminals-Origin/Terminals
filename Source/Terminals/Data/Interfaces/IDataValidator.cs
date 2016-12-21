using Terminals.Data.Validation;

namespace Terminals.Data.Interfaces
{
    internal interface IDataValidator
    {
        ValidationStates Validate(ICredentialSet credentialSet);
    }
}