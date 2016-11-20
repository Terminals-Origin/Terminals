using System.ComponentModel.DataAnnotations;
using FluentValidation;
using Terminals.Data.DB;

namespace Terminals.Data.Validation
{
    internal class DbGroupMetadata
    {
        // minimum lenght of string with separate error message
        [Required(ErrorMessage = CredentialSetMetadata.NAME_MIN_LENGTH)]
        [StringLength(255, ErrorMessage = Validations.MAX_255_CHARACTERS)]
        public string Name { get; set; }
    }


    internal class DbGroupValidator : AbstractValidator<IGroup>
    {
        public DbGroupValidator()
        {
            this.RuleFor(g => g.Name).NotEmpty().WithMessage(CredentialSetMetadata.NAME_MIN_LENGTH);
            this.RuleFor(g => g.Name).Length(0, 255).WithMessage(Validations.MAX_255_CHARACTERS);
        }
    }
}
