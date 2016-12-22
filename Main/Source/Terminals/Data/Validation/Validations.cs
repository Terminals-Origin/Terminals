using System.Collections.Generic;
using System.Linq;
using FluentValidation;
using FluentValidation.Results;
using Terminals.Connections;
using Terminals.Data.Interfaces;

namespace Terminals.Data.Validation
{
    /// <summary>
    /// Stupid tranformations to validate input values when storing to the database
    /// </summary>
    internal abstract class Validations : IDataValidator
    {
        internal const string MAX_255_CHARACTERS = "Property maximum lenght is 255 characters.";

        internal const string UNKNOWN_PROTOCOL = "Protocol is unknown";

        internal const string PORT_RANGE = "Port has to be a number in range 0-65535.";

        /// <summary>
        /// Gets name of the "Name" property
        /// </summary>
        internal const string NAME_PROPERTY = "Name";

        private readonly AbstractValidator<ICredentialSet> credentialSetValidator;

        private readonly AbstractValidator<IFavorite> favoriteValidator;

        private readonly NamedItemValidator<INamedItem> namedItemValidator;

        protected Validations(AbstractValidator<ICredentialSet> credentialSetValidator,
            AbstractValidator<IFavorite> favoriteValidator, NamedItemValidator<INamedItem> namedItemValidator)
        {
            this.credentialSetValidator = credentialSetValidator;
            this.favoriteValidator = favoriteValidator;
            this.namedItemValidator = namedItemValidator;
        }

        public ValidationStates Validate(ConnectionManager connectionManager, IFavorite favorite)
        {
            return ValidateToStates(this.favoriteValidator, favorite);
        }

        public ValidationStates Validate(ICredentialSet credentialSet)
        {
            return ValidateToStates(this.credentialSetValidator, credentialSet);
        }

        public List<ValidationState> Validate(IGroup group)
        {
            return Validate(this.namedItemValidator, group);
        }

        public ValidationStates ValidateNameProperty(INamedItem toValidate)
        {
            return ValidateToStates(this.namedItemValidator, toValidate);
        }

        private static ValidationStates ValidateToStates<TItem>(AbstractValidator<TItem> validator, TItem toValidate)
        {
            List<ValidationState> results = Validate(validator, toValidate);
            return new ValidationStates(results);
        }

        private static List<ValidationState> Validate<TItem>(AbstractValidator<TItem> validator, TItem toValidate)
        {
            ValidationResult results = validator.Validate(toValidate);
            return ConvertResultsToStates(results);
        }

        private static List<ValidationState> ConvertResultsToStates(ValidationResult results)
        {
            return results.Errors
                .Select(ToState)
                .ToList();
        }

        private static ValidationState ToState(ValidationFailure result)
        {
            return new ValidationState()
                {
                    PropertyName = result.PropertyName,
                    Message = result.ErrorMessage
                };
        }
    }
}
