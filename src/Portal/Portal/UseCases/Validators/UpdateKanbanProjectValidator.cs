using FluentValidation;
using Portal.UseCases.Mutations;

namespace Portal.UseCases.Validators;

public class UpdateKanbanProjectValidator : AbstractValidator<UpdateKanbanProjectCommand>
{
    public UpdateKanbanProjectValidator()
    {
        ClassLevelCascadeMode = CascadeMode.Stop;
    }
}