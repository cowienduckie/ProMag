using IdentityServer.Models;
using IdentityServer.Options;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Promag.Protobuf.Commons.V1;
using Promag.Protobuf.Identity.V1;
using Shared;

namespace IdentityServer.UseCases.Commands.Handlers;

public class CreateLogInUserHandler : IRequestHandler<CreateLogInUserRequest, CreateLogInUserResponse>
{
    private readonly IHttpContextAccessor _accessor;
    private readonly IConfiguration _configuration;
    private readonly LinkGenerator _linkGenerator;
    private readonly ILogger<CreateLogInUserHandler> _logger;
    private readonly RoleManager<ApplicationRole> _roleManager;
    private readonly UserManager<ApplicationUser> _userManager;

    public CreateLogInUserHandler(
        UserManager<ApplicationUser> userManager,
        RoleManager<ApplicationRole> roleManager,
        IHttpContextAccessor accessor,
        LinkGenerator generator,
        IConfiguration configuration,
        ILogger<CreateLogInUserHandler> logger)
    {
        _userManager = userManager;
        _roleManager = roleManager;
        _accessor = accessor;
        _linkGenerator = generator;
        _configuration = configuration;
        _logger = logger;
    }

    public async Task<CreateLogInUserResponse> Handle(CreateLogInUserRequest request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("{HandlerName} - {UserId} - Start", nameof(CreateLogInUserHandler), request.UserId);

        var identityResult = await _userManager.CreateAsync(new ApplicationUser
        {
            Id = request.UserId,
            Email = request.Email,
            UserName = request.UserName
        });

        if (!identityResult.Succeeded || identityResult.Errors.Any())
        {
            _logger.LogError("{HandlerName} - {UserId} - Create user failed!", nameof(CreateLogInUserHandler), request.UserId);

            var errorResult = new CreateLogInUserResponse
            {
                Succeeded = false,
                ActivateUrl = string.Empty,
                UserName = request.UserName
            };

            foreach (var error in identityResult.Errors)
            {
                errorResult.Errors.Add(new ErrorDto
                {
                    Code = error.Code,
                    Description = error.Description
                });
            }

            return errorResult;
        }

        var appUser = await _userManager.FindByIdAsync(request.UserId);

        if (appUser is null)
        {
            _logger.LogError("{HandlerName} - {UserId} - User not found!", nameof(CreateLogInUserHandler), request.UserId);

            return new CreateLogInUserResponse
            {
                Succeeded = false
            };
        }

        foreach (var roleId in request.RoleIds)
        {
            var role = await _roleManager.FindByIdAsync(roleId);

            if (role?.NormalizedName is null)
            {
                continue;
            }

            await _userManager.AddToRoleAsync(appUser, role.NormalizedName);
        }

        var tokenGenerated = await _userManager.GenerateEmailConfirmationTokenAsync(appUser);

        if (_accessor.HttpContext is null)
        {
            _logger.LogError("{HandlerName} - {UserId} - No active HttpContext!", nameof(CreateLogInUserHandler), request.UserId);

            return new CreateLogInUserResponse
            {
                Succeeded = false
            };
        }

        var activateUrl = _linkGenerator.GetPathByPage(
            _accessor.HttpContext,
            "ConfirmEmail",
            "Account",
            new
            {
                id = appUser.Id,
                token = tokenGenerated
            });

        var serviceOption = _configuration.GetOptions<IdentityServiceOptions>("IdentityServiceOptions");

        if (serviceOption.ExternalIdentityBaseUrl is null)
        {
            _logger.LogError("{HandlerName} - {UserId} - External IdentityServer base URL not found!",
                nameof(CreateLogInUserHandler),
                request.UserId);

            return new CreateLogInUserResponse
            {
                Succeeded = false
            };
        }


        _logger.LogInformation("{HandlerName} - {UserId} - Finish", nameof(CreateLogInUserHandler), request.UserId);

        var result = new CreateLogInUserResponse
        {
            Succeeded = identityResult.Succeeded,
            ActivateUrl = new Uri(new Uri(serviceOption.ExternalIdentityBaseUrl), activateUrl).ToString(),
            UserName = appUser.UserName
        };

        foreach (var error in identityResult.Errors)
        {
            result.Errors.Add(new ErrorDto
            {
                Code = error.Code,
                Description = error.Description
            });
        }

        return result;
    }
}