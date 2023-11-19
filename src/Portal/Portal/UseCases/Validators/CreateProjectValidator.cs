using FluentValidation;
using Portal.UseCases.Mutations;

namespace Portal.UseCases.Validators;

public class CreateProjectValidator : AbstractValidator<CreateProjectCommand>
{
    public CreateProjectValidator()
    {
        ClassLevelCascadeMode = CascadeMode.Stop;

        RuleFor(x => x.Name)
            .NotNull()
            .NotEmpty()
            .WithMessage("The first name could not be null or empty");
    }
}