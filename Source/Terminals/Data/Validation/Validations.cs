using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using FluentValidation;
using Terminals.Connections;
using Terminals.Data.DB;
using ValidationContext = System.ComponentModel.DataAnnotations.ValidationContext;

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

        static Validations()
        {
            RegisterSqlValidations();
            RegisterFilePersistedValidations();
        }

        private static void RegisterSqlValidations()
        {
            RegisterProvider(typeof(DbFavorite), typeof(DbFavoriteMetadata));
            RegisterProvider(typeof(DbBeforeConnectExecute), typeof(DbBeforeConnectExecuteMetadata));
            RegisterProvider(typeof(DbGroup), typeof(DbGroupMetadata));
            RegisterProvider(typeof(DbCredentialSet), typeof(DbCredentialSetMetadata));
        }

        private static void RegisterFilePersistedValidations()
        {
            RegisterProvider(typeof(Favorite), typeof(FavoriteMetadata));
            RegisterProvider(typeof(Group), typeof(GroupMetadata));
            RegisterProvider(typeof(CredentialSet), typeof(CredentialSetMetadata));
        }

        private static void RegisterProvider(Type itemType, Type metadataType)
        {
            var association = new AssociatedMetadataTypeTypeDescriptionProvider(itemType, metadataType);
            TypeDescriptor.AddProviderTransparent(association, itemType);
        }

        internal static ValidationStates Validate(IFavorite favorite)
        {
            AbstractValidator<IFavorite> validator;

            if (favorite is DbFavorite)
                validator = new DbFavoriteValidator(ConnectionManager.Instance);
            else
                validator = new FavoriteValidator(ConnectionManager.Instance);

            FluentValidation.Results.ValidationResult toConvert = validator.Validate(favorite);
            var results = ConvertResultsToStates(toConvert);

            return new ValidationStates(results);
        }

        internal static ValidationStates Validate(ICredentialSet credentialSet)
        {
            AbstractValidator<ICredentialSet> validator;

            if (credentialSet is DbCredentialSet)
                validator = new DbCredentialSetValidator();
            else
                validator = new CredentialSetValidator();

            FluentValidation.Results.ValidationResult toConvert = validator.Validate(credentialSet);
            var results = ConvertResultsToStates(toConvert);

            return new ValidationStates(results);
        }

        internal static List<ValidationState> Validate(IGroup group)
        {
            AbstractValidator<IGroup> validator;

            if (group is DbGroup)
                validator = new DbGroupValidator();
            else
                validator = new GroupValidator();

            FluentValidation.Results.ValidationResult results = validator.Validate(group);
            return ConvertResultsToStates(results);
        }

        internal static ValidationStates ValidateNameProperty(INamedItem toValidate)
        {
            var results = new List<ValidationResult>();
            var context = new ValidationContext(toValidate, null, null);
            context.MemberName = NAME_PROPERTY;
            Validator.TryValidateProperty(toValidate.Name, context, results);
            var states = ConvertResultsToStates(results);
            return new ValidationStates(states);
        }

        private static List<ValidationState> ConvertResultsToStates(FluentValidation.Results.ValidationResult results)
        {
            return results.Errors
                .Select(ToState)
                .ToList();
        }

        private static ValidationState ToState(FluentValidation.Results.ValidationFailure result)
        {
            return new ValidationState()
                {
                    PropertyName = result.PropertyName,
                    Message = result.ErrorMessage
                };
        }

        private static List<ValidationState> ConvertResultsToStates(List<ValidationResult> results)
        {
            return results.Where(result => result.MemberNames.Any())
                .Select(ToState)
                .ToList();
        }

        private static ValidationState ToState(ValidationResult result)
        {
            return new ValidationState()
                {
                    PropertyName = result.MemberNames.First(),
                    Message = result.ErrorMessage
                };
        }
    }
}
