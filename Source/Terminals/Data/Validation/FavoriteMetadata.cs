using System;
using System.ComponentModel.DataAnnotations;
using FluentValidation;
using Terminals.Connections;

namespace Terminals.Data.Validation
{
    internal class FavoriteMetadata
    {
        [Required]
        [CustomValidation(typeof(CustomValidationRules), CustomValidationRules.METHOD_ISKNOWNPROTOCOL)]
        public string Protocol { get; set; }

        [Required]
        public string Name { get; set; }

        // Servername is not required, because for web based protocols URL property may be defined
        [CustomValidation(typeof(CustomValidationRules), CustomValidationRules.METHOD_ISVALIDSERVERNAME)]
        public string ServerName { get; set; }

        [Range(0, 65535, ErrorMessage = Validations.PORT_RANGE)]
        public int Port { get; set; }
    }

    internal class FavoriteValidator : AbstractValidator<IFavorite>
    {
        private readonly ConnectionManager connectionManager;

        public FavoriteValidator(ConnectionManager connectionManager)
        {
            this.connectionManager = connectionManager;

            this.RuleFor(g => g.Name).NotEmpty().WithMessage(CredentialSetMetadata.NAME_MIN_LENGTH);
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