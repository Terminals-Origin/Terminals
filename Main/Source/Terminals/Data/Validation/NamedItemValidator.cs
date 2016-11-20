using FluentValidation;

namespace Terminals.Data.Validation
{
    internal class NamedItemValidator<TNamedItem> : AbstractValidator<TNamedItem>
        where TNamedItem : INamedItem
    {
        public NamedItemValidator()
        {
            this.RuleFor(g => g.Name).NotEmpty().WithMessage(CredentialSetValidator.NAME_MIN_LENGTH);
        }
    }
}