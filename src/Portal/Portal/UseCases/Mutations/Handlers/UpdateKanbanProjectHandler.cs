using System.Collections.Frozen;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Portal.Boundaries.GraphQL.Dtos.Tasks;
using Portal.Data;
using Portal.UseCases.Responses;
using Shared.Common.ApiResponse;
using Shared.Common.Helpers;

namespace Portal.UseCases.Mutations.Handlers;

public class UpdateKanbanProjectHandler : IRequestHandler<UpdateKanbanProjectCommand, UpdateKanbanProjectResponse>
{
    private readonly ILogger<UpdateKanbanProjectHandler> _logger;
    private readonly PortalContext _portalContext;
    private readonly IValidator<UpdateKanbanProjectCommand> _validator;

    public UpdateKanbanProjectHandler(
        ILogger<UpdateKanbanProjectHandler> logger,
        PortalContext portalContext,
        IValidator<UpdateKanbanProjectCommand> validator)
    {
        _logger = logger;
        _portalContext = portalContext;
        _validator = validator;
    }

    public async Task<UpdateKanbanProjectResponse> Handle(UpdateKanbanProjectCommand request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("{Handler} - Start", nameof(UpdateKanbanProjectHandler));

        try
        {
            await _validator.ValidateAndThrowAsync(request, cancellationToken);

            var projectExist = await _portalContext.Projects.AnyAsync(p => p.Id.ToString() == request.ProjectId
                                                                           && p.DeletedOn == null, cancellationToken);

            if (projectExist is false)
            {
                _logger.LogError("{Handler} - ProjectId={ProjectId} not found", nameof(UpdateKanbanProjectHandler), request.ProjectId);

                return new UpdateKanbanProjectResponse
                {
                    StatusCode = ResponseStatuses.NotFound.GetCode(),
                    ErrorCode = Errors.COM_001.GetCode(),
                    ErrorMessage = Errors.COM_001.GetMessages("ProjectId")
                };
            }

            // Update column of tasks
            var taskDict = request.Tasks
                .Select(t => t.Value
                    .AsDictionary<string, object?>()
                    .GetObject<KanbanTaskDto>())
                .ToFrozenDictionary(t => Guid.Parse(t.Id), t => t);

            var tasks = await _portalContext.Tasks
                .Where(t => t.ProjectId.ToString() == request.ProjectId && t.DeletedOn == null)
                .ToListAsync(cancellationToken);

            foreach (var task in tasks)
            {
                task.SectionId = Guid.Parse(taskDict[task.Id].Column);
                task.Completed = taskDict[task.Id].IsCompleted;
                task.CompletedOn = DateTime.UtcNow;
            }

            _portalContext.Tasks.UpdateRange(tasks);

            // Update column order
            var sections = await _portalContext.Sections
                .Where(s => s.ProjectId.ToString() == request.ProjectId && s.DeletedOn == null)
                .ToListAsync(cancellationToken);

            foreach (var section in sections)
            {
                section.OrderIndex = request.ColumnOrder.IndexOf(section.Id.ToString());
            }

            _portalContext.Sections.UpdateRange(sections);

            // Remove deleted tasks
            var project = await _portalContext.Projects
                .Include(p => p.Tasks.Where(t => t.DeletedOn == null))
                .Where(p => p.Id.ToString() == request.ProjectId && p.DeletedOn == null)
                .FirstOrDefaultAsync(cancellationToken);

            foreach (var task in project!.Tasks)
            {
                if (!taskDict.ContainsKey(task.Id))
                {
                    _portalContext.Tasks.Remove(task);
                }
            }

            _portalContext.Projects.Update(project);

            // Save changes
            await _portalContext.SaveChangesAsync(cancellationToken);

            _logger.LogInformation("{Handler} - Finish", nameof(UpdateKanbanProjectHandler));

            return new UpdateKanbanProjectResponse
            {
                StatusCode = ResponseStatuses.Success.GetCode(),
                ProjectId = request.ProjectId
            };
        }
        catch (ValidationException ex)
        {
            _logger.LogError("{Handler} - Validation failed: {Message}", nameof(UpdateKanbanProjectHandler), ex.Message);

            return new UpdateKanbanProjectResponse
            {
                StatusCode = ResponseStatuses.BadRequest.GetCode(),
                ErrorCode = Errors.VAL_001.GetCode(),
                ErrorMessage = Errors.VAL_001.GetMessages(ex.Message)
            };
        }
        catch (Exception ex)
        {
            _logger.LogError("{Handler} - Unexpected errors: {Message}", nameof(UpdateKanbanProjectHandler), ex.Message);

            return new UpdateKanbanProjectResponse
            {
                StatusCode = ResponseStatuses.FailWithException.GetCode(),
                ErrorCode = Errors.COM_000.GetCode(),
                ErrorMessage = Errors.COM_000.GetMessages()
            };
        }
    }
}