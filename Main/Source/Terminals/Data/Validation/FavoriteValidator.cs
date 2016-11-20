using FluentValidation;
using Terminals.Connections;

namespace Terminals.Data.Validation
{
    internal class FavoriteValidator : NamedItemValidator<IFavorite>
    {
        private readonly ConnectionManager connectionManager;

        public FavoriteValidator(ConnectionManager connectionManager)
        {
            this.connectionManager = connectionManager;

            this.RuleFor(g => g.Port).InclusiveBetween(0, 65535).WithMessage(Validations.PORT_RANGE);
            this.RuleFor(g => g.ServerName).Must(g => !CustomValidationRules.IsValidServerNameB(g))
                .WithMessage(CustomValidationRules.SERVER_NAME_IS_NOT_IN_THE_CORRECT_FORMAT);
            this.RuleFor(g => g.Protocol).NotEmpty();
            this.RuleFor(g => g.Protocol).Must(this.IsKnownProtocol)
                .WithMessage(Validations.UNKNOWN_PROTOCOL);
        }

        private bool IsKnownProtocol(string protocol)
        {
            return this.connectionManager.IsKnownProtocol(protocol);
        }
    }
}