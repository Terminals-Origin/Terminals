using FluentValidation;
using Terminals.Connections;

namespace Terminals.Data.Validation
{
    internal class DbFavoriteValidator : DbNamedItemValidator<IFavorite>
    {
        private readonly ConnectionManager connectionManager;

        public DbFavoriteValidator(ConnectionManager connectionManager)
        {
            this.connectionManager = connectionManager;

            this.RuleFor(g => g.Protocol).NotEmpty();
            this.RuleFor(g => g.Protocol).Length(0, 10).WithMessage(Validations.UNKNOWN_PROTOCOL);
            this.RuleFor(g => g.Protocol).Must(this.IsKnownProtocol)
                .WithMessage(Validations.UNKNOWN_PROTOCOL);

            this.RuleFor(g => g.ServerName).NotEmpty();
            this.RuleFor(g => g.ServerName).Length(0, 255).WithMessage(Validations.MAX_255_CHARACTERS);
            this.RuleFor(g => g.ServerName).Must(g => !CustomValidationRules.IsValidServerNameB(g))
                .WithMessage(CustomValidationRules.SERVER_NAME_IS_NOT_IN_THE_CORRECT_FORMAT);

            this.RuleFor(g => g.Notes).Length(0, 255).WithMessage("Property maximum lenght is 500 characters.");

            this.RuleFor(g => g.Port).InclusiveBetween(0, 65535).WithMessage(Validations.PORT_RANGE);

            this.RuleFor(g => g.ExecuteBeforeConnect).SetValidator(new DbBeforeConnectExecuteValidator());
        }

        private bool IsKnownProtocol(string protocol)
        {
            return this.connectionManager.IsKnownProtocol(protocol);
        }
    }
}