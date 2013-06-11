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

        static Validations()
        {
            var favorteAssociation = new AssociatedMetadataTypeTypeDescriptionProvider(typeof(DbFavorite), typeof(DbFavoriteMetadata));
            TypeDescriptor.AddProviderTransparent(favorteAssociation, typeof(DbFavorite));
            var executeAssociation = new AssociatedMetadataTypeTypeDescriptionProvider(typeof(DbBeforeConnectExecute), typeof(DbBeforeConnectExecuteMetadata));
            TypeDescriptor.AddProviderTransparent(executeAssociation, typeof(DbBeforeConnectExecute));
            var groupAssociation = new AssociatedMetadataTypeTypeDescriptionProvider(typeof(DbGroup), typeof(DbGroupMetadata));
            TypeDescriptor.AddProviderTransparent(groupAssociation, typeof(DbGroup));
            var credentialsAssociation = new AssociatedMetadataTypeTypeDescriptionProvider(typeof(DbCredentialSet), typeof(DbCredentialSetMetadata));
            TypeDescriptor.AddProviderTransparent(credentialsAssociation, typeof(DbCredentialSet));

            // todo replace the validation in NewFavorite Form
            // todo add file persisted favorite validations
        }

        internal static List<ValidationState> ValidateFavorite(IFavorite favorite)
        {
            var results = new List<ValidationResult>();
            Validator.TryValidateObject(favorite, new ValidationContext(favorite, null, null), results, true);
            var beforeExcute = favorite.ExecuteBeforeConnect;
            Validator.TryValidateObject(beforeExcute, new ValidationContext(beforeExcute, null, null), results, true);
            return ConvertResultsToStates(results);
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
