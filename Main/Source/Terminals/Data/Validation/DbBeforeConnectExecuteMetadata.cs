using System.ComponentModel.DataAnnotations;
using FluentValidation;
using Terminals.Data.DB;

namespace Terminals.Data.Validation
{
    internal class DbBeforeConnectExecuteMetadata
    {
        [StringLength(255, ErrorMessage = Validations.MAX_255_CHARACTERS)]
        public string Command { get; set; }

        [StringLength(255, ErrorMessage = Validations.MAX_255_CHARACTERS)]
        public string CommandArguments { get; set; }

        [StringLength(255, ErrorMessage = Validations.MAX_255_CHARACTERS)]
        public string InitialDirectory { get; set; }
    }

    internal class DbBeforeConnectExecuteValidator : AbstractValidator<IBeforeConnectExecuteOptions>
    {
        public DbBeforeConnectExecuteValidator()
        {
            this.RuleFor(g => g.Command).Length(0, 255).WithMessage(Validations.MAX_255_CHARACTERS);
            this.RuleFor(g => g.CommandArguments).Length(0, 255).WithMessage(Validations.MAX_255_CHARACTERS);
            this.RuleFor(g => g.InitialDirectory).Length(0, 255).WithMessage(Validations.MAX_255_CHARACTERS);
        }
    }
}