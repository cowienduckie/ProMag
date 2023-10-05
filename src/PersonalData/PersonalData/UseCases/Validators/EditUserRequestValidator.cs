using FluentValidation;
using PersonalData.UseCases.Commands;

namespace PersonalData.UseCases.Validators;

public class EditUserCommandValidator : AbstractValidator<EditUserCommand>
{
    public EditUserCommandValidator()
    {
        ClassLevelCascadeMode = CascadeMode.Stop;

        RuleFor(x => x.FirstName)
            .NotNull()
            .NotEmpty()
            .WithMessage("The first name could not be null or empty");

        RuleFor(x => x.LastName)
            .NotNull()
            .NotEmpty()
            .WithMessage("The last name could not be null or empty");
    }
}