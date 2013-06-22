using System.ComponentModel.DataAnnotations;

namespace Terminals.Data.Validation
{
    internal class GroupMetadata
    {
        [Required(ErrorMessage = CredentialSetMetadata.NAME_MIN_LENGTH)]
        public string Name { get; set; }
    }
}