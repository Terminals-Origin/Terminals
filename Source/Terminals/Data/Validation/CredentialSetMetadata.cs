using System.ComponentModel.DataAnnotations;

namespace Terminals.Data.Validation
{
    internal class CredentialSetMetadata
    {
        internal const string  NAME_MIN_LENGTH = "Name cant be empty.";

        internal const string USERNAME_MIN_LENGTH = "UserName cant be empty.";

        [Required(ErrorMessage = NAME_MIN_LENGTH)]
        public string Name { get; set; }

        [Required(ErrorMessage = USERNAME_MIN_LENGTH)]
        public string UserName { get; set; }
    }
}