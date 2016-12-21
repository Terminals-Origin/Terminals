using System.Collections.Generic;
using System.Linq;
using FluentValidation;
using FluentValidation.Results;
using Terminals.Connections;
using Terminals.Data.DB;
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

        protected Validations(AbstractValidator<ICredentialSet> credentialSetValidator)
        {
            this.credentialSetValidator = credentialSetValidator;
        }

        internal static ValidationStates Validate(ConnectionManager connectionManager, IFavorite favorite)
        {
            AbstractValidator<IFavorite> validator;

            if (favorite is DbFavorite)
                validator = new DbFavoriteValidator(connectionManager);
            else
                validator = new FavoriteValidator(connectionManager);

            return ValidateToStates(validator, favorite);
        }

        public ValidationStates Validate(ICredentialSet credentialSet)
        {
            AbstractValidator<ICredentialSet> validator;

            if (credentialSet is DbCredentialSet)
                validator = new DbCredentialSetValidator();
            else
                validator = new CredentialSetValidator();

            return ValidateToStates(validator, credentialSet);
        }

        internal static List<ValidationState> Validate(IGroup group)
        {
            return ValidateNamedItem(group);
        }

        internal static ValidationStates ValidateNameProperty(INamedItem toValidate)
        {
            NamedItemValidator<INamedItem> validator = SelectValidator(toValidate);
            return ValidateToStates(validator, toValidate);
        }

        private static List<ValidationState> ValidateNamedItem(INamedItem toValidate)
        {
            var validator = SelectValidator(toValidate);
            return Validate(validator, toValidate);
        }

        private static NamedItemValidator<INamedItem> SelectValidator(INamedItem toValidate)
        {
            if (toValidate is DbGroup || toValidate is DbFavorite)
                return new DbNamedItemValidator<INamedItem>();

            return new NamedItemValidator<INamedItem>();
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
