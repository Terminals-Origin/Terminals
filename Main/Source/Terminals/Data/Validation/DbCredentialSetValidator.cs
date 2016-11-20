using FluentValidation;

namespace Terminals.Data.Validation
{
    internal class DbCredentialSetValidator : AbstractValidator<ICredentialSet>
    {
        public DbCredentialSetValidator()
        {
            this.RuleFor(g => g.Name).NotEmpty().WithMessage(CredentialSetValidator.NAME_MIN_LENGTH);
            this.RuleFor(g => g.Name).Length(0, 255).WithMessage(Validations.MAX_255_CHARACTERS);
            this.RuleFor(g => g.EncryptedUserName).NotEmpty().WithMessage(CredentialSetValidator.USERNAME_MIN_LENGTH);
        }
    }
}