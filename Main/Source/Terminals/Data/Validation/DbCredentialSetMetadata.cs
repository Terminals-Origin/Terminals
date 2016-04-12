using System.ComponentModel.DataAnnotations;

namespace Terminals.Data.Validation
{
    internal class DbCredentialSetMetadata
    {
        [Required(ErrorMessage = CredentialSetMetadata.NAME_MIN_LENGTH)]
        [StringLength(255, ErrorMessage = Validations.MAX_255_CHARACTERS)]
        public string Name { get; set; }
    }
}
