using System.ComponentModel.DataAnnotations;
using FluentValidation;

namespace Terminals.Data.Validation
{
    internal class GroupMetadata
    {
        [Required(ErrorMessage = CredentialSetMetadata.NAME_MIN_LENGTH)]
        public string Name { get; set; }
    }

    internal class GroupValidator : NamedItemValidator<IGroup>
    { }

    internal class NamedItemValidator<TNamedItem> : AbstractValidator<TNamedItem>
        where TNamedItem : INamedItem
    {
        public NamedItemValidator()
        {
            this.RuleFor(g => g.Name).NotEmpty().WithMessage(CredentialSetMetadata.NAME_MIN_LENGTH);
        }
    }
}