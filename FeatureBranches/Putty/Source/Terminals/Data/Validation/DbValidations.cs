using Terminals.Connections;
using Terminals.Data.Interfaces;

namespace Terminals.Data.Validation
{
    internal class DbValidations : Validations, IDataValidator
    {
        public DbValidations(ConnectionManager connectionManager)
            :base(new DbCredentialSetValidator(), new DbFavoriteValidator(connectionManager), new DbNamedItemValidator<INamedItem>())
        {
        }
    }
}