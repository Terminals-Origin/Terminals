using System.ComponentModel.DataAnnotations;

namespace Terminals.Data.Validation
{
    internal class DbFavoriteMetadata
    {
        // Candidate to add validation for RDP url property

        [Required]
        [StringLength(10, ErrorMessage = Validations.UNKNOWN_PROTOCOL)]
        [CustomValidation(typeof(CustomValidationRules), "IsKnownProtocol")]
        public string Protocol { get; set; }

        [Required]
        [StringLength(255, ErrorMessage = Validations.MAX_255_CHARACTERS)]
        public string Name { get; set; }

        [Required]
        [StringLength(255, ErrorMessage = Validations.MAX_255_CHARACTERS)]
        [CustomValidation(typeof(CustomValidationRules), "IsValidServerName")]
        public string ServerName { get; set; }

        [StringLength(500, ErrorMessage = "Property maximum lenght is 500 characters.")]
        public string Notes { get; set; }

        [Range(0, 65535, ErrorMessage = "Port has to be a number in range 0-65535.")]
        public int Port { get; set; }
    }
}