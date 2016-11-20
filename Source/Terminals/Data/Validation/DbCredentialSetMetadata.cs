using System.ComponentModel.DataAnnotations;
using FluentValidation;
using Terminals.Data.DB;

namespace Terminals.Data.Validation
{
    internal class DbCredentialSetMetadata
    {
        [Required(ErrorMessage = CredentialSetMetadata.NAME_MIN_LENGTH)]
        [StringLength(255, ErrorMessage = Validations.MAX_255_CHARACTERS)]
        public string Name { get; set; }

        [Required(ErrorMessage = CredentialSetMetadata.USERNAME_MIN_LENGTH)]
        public string EncryptedUserName { get; set; }
    }

    internal class DbCredentialSetValidator : AbstractValidator<ICredentialSet>
    {
        public DbCredentialSetValidator()
        {
            this.RuleFor(g => g.Name).NotEmpty().WithMessage(CredentialSetMetadata.NAME_MIN_LENGTH);
            this.RuleFor(g => g.Name).Length(0, 255).WithMessage(Validations.MAX_255_CHARACTERS);
            this.RuleFor(g => g.EncryptedUserName).NotEmpty().WithMessage(CredentialSetMetadata.USERNAME_MIN_LENGTH);
        }
    }
}
