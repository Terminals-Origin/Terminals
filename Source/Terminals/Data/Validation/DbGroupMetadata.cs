using System.ComponentModel.DataAnnotations;
using FluentValidation;

namespace Terminals.Data.Validation
{
    internal class DbGroupMetadata
    {
        // minimum lenght of string with separate error message
        [Required(ErrorMessage = CredentialSetMetadata.NAME_MIN_LENGTH)]
        [StringLength(255, ErrorMessage = Validations.MAX_255_CHARACTERS)]
        public string Name { get; set; }
    }

    internal class DbNamedItemValidator<TNamedItem> : NamedItemValidator<TNamedItem>
    where TNamedItem : INamedItem
    {
        public DbNamedItemValidator()
        {
            this.RuleFor(g => g.Name).Length(0, 255).WithMessage(Validations.MAX_255_CHARACTERS);
        }
    }
}
