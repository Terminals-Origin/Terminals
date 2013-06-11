using System.ComponentModel.DataAnnotations;

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
}