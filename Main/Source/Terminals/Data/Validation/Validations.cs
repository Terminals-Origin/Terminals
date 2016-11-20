using System.Collections.Generic;
using System.Linq;
using FluentValidation;
using FluentValidation.Results;
using Terminals.Connections;
using Terminals.Data.DB;

namespace Terminals.Data.Validation
{
    /// <summary>
    /// Stupid tranformations to validate input values when storing to the database
    /// </summary>
    internal static class Validations
    {
        internal const string MAX_255_CHARACTERS = "Property maximum lenght is 255 characters.";

        internal const string UNKNOWN_PROTOCOL = "Protocol is unknown";

        internal const string PORT_RANGE = "Port has to be a number in range 0-65535.";

        /// <summary>
        /// Gets name of the "Name" property
        /// </summary>
        internal const string NAME_PROPERTY = "Name";

        internal static ValidationStates Validate(IFavorite favorite)
        {
            AbstractValidator<IFavorite> validator;

            if (favorite is DbFavorite)
                validator = new DbFavoriteValidator(ConnectionManager.Instance);
            else
                validator = new FavoriteValidator(ConnectionManager.Instance);

            ValidationResult toConvert = validator.Validate(favorite);
            List<ValidationState> results = ConvertResultsToStates(toConvert);
            return new ValidationStates(results);
        }

        internal static ValidationStates Validate(ICredentialSet credentialSet)
        {
            AbstractValidator<ICredentialSet> validator;

            if (credentialSet is DbCredentialSet)
                validator = new DbCredentialSetValidator();
            else
                validator = new CredentialSetValidator();

            ValidationResult toConvert = validator.Validate(credentialSet);
            List<ValidationState> results = ConvertResultsToStates(toConvert);
            return new ValidationStates(results);
        }

        internal static List<ValidationState> Validate(IGroup group)
        {
            NamedItemValidator<IGroup> validator;

            if (group is DbGroup)
                validator = new DbNamedItemValidator<IGroup>();
            else
                validator = new NamedItemValidator<IGroup>();

            ValidationResult results = validator.Validate(group);
            return ConvertResultsToStates(results);
        }

        internal static ValidationStates ValidateNameProperty(INamedItem toValidate)
        {
            NamedItemValidator<INamedItem> validator;

            if (toValidate is DbGroup || toValidate is DbFavorite)
                validator = new DbNamedItemValidator<INamedItem>();
            else 
                validator = new NamedItemValidator<INamedItem>();

            ValidationResult results = validator.Validate(toValidate);
            List<ValidationState> states = ConvertResultsToStates(results);
            return new ValidationStates(states);
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
