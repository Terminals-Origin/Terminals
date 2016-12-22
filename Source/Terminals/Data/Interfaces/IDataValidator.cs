using System.Collections.Generic;
using Terminals.Data.Validation;

namespace Terminals.Data.Interfaces
{
    internal interface IDataValidator
    {
        ValidationStates Validate(ICredentialSet credentialSet);

        List<ValidationState> Validate(IGroup group);

        ValidationStates Validate(IFavorite favorite);

        ValidationStates ValidateNameProperty(INamedItem toValidate);
    }
}