using System.ComponentModel.DataAnnotations;
using FluentValidation;

namespace Terminals.Data.Validation
{
    internal class CredentialSetMetadata
    {
        internal const string  NAME_MIN_LENGTH = "Name cant be empty.";
        internal const string USERNAME_MIN_LENGTH = "UserName cant be empty.";

        [Required(ErrorMessage = NAME_MIN_LENGTH)]
        public string Name { get; set; }

        [Required(ErrorMessage = USERNAME_MIN_LENGTH)]
        public string EncryptedUserName { get; set; }
    }


    internal class CredentialSetValidator : AbstractValidator<ICredentialSet>
    {
        public CredentialSetValidator()
        {
            this.RuleFor(g => g.Name).NotEmpty().WithMessage(CredentialSetMetadata.NAME_MIN_LENGTH);
            this.RuleFor(g => g.EncryptedUserName).NotEmpty().WithMessage(CredentialSetMetadata.USERNAME_MIN_LENGTH);
        }
    }
}