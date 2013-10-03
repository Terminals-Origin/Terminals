using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
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
        /// Gets name of the groups Name property
        /// </summary>
        internal const string GROUP_NAME = "Name";

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
            List<ValidationState> results = ValidateObject(favorite);
            var executeResults = ValidateObject(favorite.ExecuteBeforeConnect);
            results.AddRange(executeResults);
            return new ValidationStates(results);
        }

        internal static ValidationStates Validate(ICredentialSet credentialSet)
        {
            var results = ValidateObject(credentialSet);
            return new ValidationStates(results);
        }

        internal static List<ValidationState> Validate(IGroup group)
        {
            return ValidateObject(group);
        }

        private static List<ValidationState> ValidateObject(object toValidate)
        {
            var results = new List<ValidationResult>();
            Validator.TryValidateObject(toValidate, new ValidationContext(toValidate, null, null), results, true);
            return ConvertResultsToStates(results);
        }

        internal static ValidationStates ValidateGroupName(IGroup toValidate)
        {
            var results = new List<ValidationResult>();
            var context = new ValidationContext(toValidate, null, null);
            context.MemberName = GROUP_NAME;
            Validator.TryValidateProperty(toValidate.Name, context, results);
            var states = ConvertResultsToStates(results);
            return new ValidationStates(states);
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
