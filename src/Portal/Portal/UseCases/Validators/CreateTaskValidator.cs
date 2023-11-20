using FluentValidation;
using Portal.UseCases.Mutations;

namespace Portal.UseCases.Validators;

public class CreateTaskValidator : AbstractValidator<CreateTaskCommand>
{
    public CreateTaskValidator()
    {
        ClassLevelCascadeMode = CascadeMode.Stop;
    }
}