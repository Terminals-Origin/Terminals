using FluentValidation;

namespace Terminals.Data.Validation
{
    internal class CredentialSetValidator : AbstractValidator<ICredentialSet>
    {
        internal const string NAME_MIN_LENGTH = "Name cant be empty.";
        internal const string USERNAME_MIN_LENGTH = "UserName cant be empty.";

        public CredentialSetValidator()
        {
            this.RuleFor(g => g.Name).NotEmpty().WithMessage(NAME_MIN_LENGTH);
            this.RuleFor(g => g.EncryptedUserName).NotEmpty().WithMessage(USERNAME_MIN_LENGTH);
        }
    }
}