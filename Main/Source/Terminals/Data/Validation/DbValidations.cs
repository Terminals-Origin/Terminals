using FluentValidation;
using Terminals.Connections;
using Terminals.Data.Interfaces;

namespace Terminals.Data.Validation
{
    internal class DbValidations : Validations, IDataValidator
    {
        private AbstractValidator<IFavorite> favoriteValidator;

        private DbNamedItemValidator<INamedItem> namedItemValidator;

        public DbValidations(ConnectionManager connectionManager)
            :base(new DbCredentialSetValidator())
        {
            this.favoriteValidator = new DbFavoriteValidator(connectionManager);
            this.namedItemValidator = new DbNamedItemValidator<INamedItem>();
        }
    }
}