using Terminals.Connections;
using Terminals.Data.Validation;

namespace Terminals.Data.Interfaces
{
    internal interface IDataValidator
    {
        ValidationStates Validate(ICredentialSet credentialSet);

        ValidationStates Validate(ConnectionManager connectionManager, IFavorite favorite);
    }
}