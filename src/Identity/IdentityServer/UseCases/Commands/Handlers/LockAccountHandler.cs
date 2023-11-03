using IdentityServer.Models.DbModel;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Promag.Protobuf.Commons.V1;
using Promag.Protobuf.Identity.V1;
using Shared;

namespace IdentityServer.UseCases.Commands.Handlers;

public class LockAccountHandler : IRequestHandler<LockUserRequest, LockUserResponse>
{
    private readonly IConfiguration _configuration;
    private readonly ILogger<LockAccountHandler> _logger;
    private readonly UserManager<ApplicationUser> _userManager;

    public LockAccountHandler(UserManager<ApplicationUser> userManager,
        IConfiguration configuration, ILogger<LockAccountHandler> logger)
    {
        _userManager = userManager;
        _configuration = configuration;
        _logger = logger;
    }

    public async Task<LockUserResponse> Handle(LockUserRequest request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("{HandlerName} - {UserId} - Start", nameof(LockAccountHandler), request.UserId);

        var appUser = await _userManager.FindByIdAsync(request.UserId);

        if (appUser is null)
        {
            _logger.LogError("{HandlerName} - {UserId} - User not found!", nameof(LockAccountHandler), request.UserId);

            return new LockUserResponse
            {
                Success = false
            };
        }

        await _userManager.SetLockoutEnabledAsync(appUser, true);

        var resetAccessFailedResult = await _userManager.ResetAccessFailedCountAsync(appUser);

        if (!resetAccessFailedResult.Succeeded || resetAccessFailedResult.Errors.Any())
        {
            _logger.LogError("{HandlerName} - {UserId} - Reset user failed!", nameof(LockAccountHandler), request.UserId);

            var errorResult = new LockUserResponse
            {
                Success = false
            };

            foreach (var error in resetAccessFailedResult.Errors)
            {
                errorResult.Errors.Add(new ErrorDto
                {
                    Code = error.Code,
                    Description = error.Description
                });
            }

            return errorResult;
        }

        var lockoutOption = _configuration.GetOptions<LockoutOptions>("IdentityServiceOptions:Lockout");
        var lockoutEnd = new DateTimeOffset(DateTime.Now.AddTicks(lockoutOption.DefaultLockoutTimeSpan.Ticks));

        var setLockoutEndDateResult = await _userManager.SetLockoutEndDateAsync(appUser, lockoutEnd);

        if (!setLockoutEndDateResult.Succeeded || setLockoutEndDateResult.Errors.Any())
        {
            _logger.LogError("{HandlerName} - {UserId} - Set lockout end date failed!", nameof(LockAccountHandler), request.UserId);

            var errorResult = new LockUserResponse
            {
                Success = false
            };

            foreach (var error in setLockoutEndDateResult.Errors)
            {
                errorResult.Errors.Add(new ErrorDto
                {
                    Code = error.Code,
                    Description = error.Description
                });
            }

            return errorResult;
        }

        _logger.LogInformation("{HandlerName} - {UserId} - Finish", nameof(LockAccountHandler), request.UserId);

        return new LockUserResponse
        {
            Success = true
        };
    }
}