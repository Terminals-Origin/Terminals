using FluentValidation;

namespace Terminals.Data.Validation
{
    internal class DbNamedItemValidator<TNamedItem> : NamedItemValidator<TNamedItem>
        where TNamedItem : INamedItem
    {
        public DbNamedItemValidator()
        {
            this.RuleFor(g => g.Name).Length(0, 255).WithMessage(Validations.MAX_255_CHARACTERS);
        }
    }
}