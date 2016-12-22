using Terminals.Connections;
using Terminals.Data.Interfaces;

namespace Terminals.Data.Validation
{
    internal class FileValidations : Validations, IDataValidator
    {
        public FileValidations(ConnectionManager connectionManager)
            : base(new CredentialSetValidator(), new FavoriteValidator(connectionManager), new NamedItemValidator<INamedItem>())
        {
        }
    }
}