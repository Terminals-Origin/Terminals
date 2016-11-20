using FluentValidation;

namespace Terminals.Data.Validation
{
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