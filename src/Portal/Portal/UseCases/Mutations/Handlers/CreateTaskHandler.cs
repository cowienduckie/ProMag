using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Portal.Data;
using Portal.UseCases.Responses;
using Shared.Common.ApiResponse;
using Task = Portal.Domain.Task;

namespace Portal.UseCases.Mutations.Handlers;

public class CreateTaskHandler : IRequestHandler<CreateTaskCommand, CreateTaskResponse>
{
    private readonly ILogger<CreateTaskHandler> _logger;
    private readonly PortalContext _portalContext;
    private readonly IValidator<CreateTaskCommand> _validator;

    public CreateTaskHandler(
        ILogger<CreateTaskHandler> logger,
        IValidator<CreateTaskCommand> validator,
        PortalContext portalContext)
    {
        _logger = logger;
        _validator = validator;
        _portalContext = portalContext;
    }

    public async Task<CreateTaskResponse> Handle(CreateTaskCommand request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("{Handler} - Start", nameof(CreateTaskHandler));

        try
        {
            await _validator.ValidateAndThrowAsync(request, cancellationToken);

            var project = await _portalContext.Projects
                .Include(p => p.Sections.Where(s => s.Id == Guid.Parse(request.SectionId)))
                .FirstOrDefaultAsync(p => p.Id == Guid.Parse(request.ProjectId), cancellationToken);

            if (project is null)
            {
                _logger.LogError("{Handler} - ProjectId={ProjectId} not found", nameof(CreateTaskHandler), request.ProjectId);

                return new CreateTaskResponse
                {
                    StatusCode = ResponseStatuses.BadRequest.GetCode(),
                    ErrorCode = Errors.COM_001.GetCode(),
                    ErrorMessage = Errors.COM_001.GetMessages("ProjectId")
                };
            }

            if (project.Sections.Count == 0)
            {
                _logger.LogError("{Handler} - SectionId={SectionId} not found", nameof(CreateTaskHandler), request.SectionId);

                return new CreateTaskResponse
                {
                    StatusCode = ResponseStatuses.BadRequest.GetCode(),
                    ErrorCode = Errors.COM_001.GetCode(),
                    ErrorMessage = Errors.COM_001.GetMessages("SectionId")
                };
            }

            var newTask = new Task
            {
                Name = request.Name,
                Notes = request.Notes,
                StartOn = request.StartOn,
                DueOn = request.DueOn,
                WorkspaceId = project.WorkspaceId,
                Section = project.Sections.Single(),
                Project = project
            };

            await _portalContext.Tasks.AddAsync(newTask, cancellationToken);
            await _portalContext.SaveChangesAsync(cancellationToken);

            _logger.LogInformation("{Handler} - Finish", nameof(CreateTaskHandler));

            return new CreateTaskResponse
            {
                StatusCode = ResponseStatuses.Success.GetCode(),
                TaskId = Guid.NewGuid().ToString(),
                ProjectId = request.ProjectId
            };
        }
        catch (ValidationException ex)
        {
            _logger.LogError("{Handler} - Validation failed: {Message}", nameof(CreateTaskHandler), ex.Message);

            return new CreateTaskResponse
            {
                StatusCode = ResponseStatuses.BadRequest.GetCode(),
                ErrorCode = Errors.VAL_001.GetCode(),
                ErrorMessage = Errors.VAL_001.GetMessages(ex.Message)
            };
        }
        catch (Exception ex)
        {
            _logger.LogError("{Handler} - Unexpected errors: {Message}", nameof(CreateTaskHandler), ex.Message);

            return new CreateTaskResponse
            {
                StatusCode = ResponseStatuses.FailWithException.GetCode(),
                ErrorCode = Errors.COM_000.GetCode(),
                ErrorMessage = Errors.COM_000.GetMessages()
            };
        }
    }
}