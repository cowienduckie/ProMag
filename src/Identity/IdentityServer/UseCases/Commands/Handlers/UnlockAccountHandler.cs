using IdentityServer.Models;
using IdentityServer.Options;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Promag.Protobuf.Identity.V1;
using Shared;

namespace IdentityServer.UseCases.Commands.Handlers;

public class UnlockAccountHandler : IRequestHandler<AccountRequest, AccountResponse>
{
    private readonly IConfiguration _configuration;
    private readonly ILogger<UnlockAccountHandler> _logger;
    private readonly UserManager<ApplicationUser> _userManager;

    public UnlockAccountHandler(UserManager<ApplicationUser> userManager, IConfiguration configuration, ILogger<UnlockAccountHandler> logger)
    {
        _userManager = userManager;
        _configuration = configuration;
        _logger = logger;
    }

    public async Task<AccountResponse> Handle(AccountRequest request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("{HandlerName} - {UserId} - Start", nameof(UnlockAccountHandler), request.UserId);

        var appUser = await _userManager.FindByIdAsync(request.UserId);

        if (appUser is null)
        {
            _logger.LogError("{HandlerName} - {UserId} - User not found!", nameof(UnlockAccountHandler), request.UserId);

            return new AccountResponse
            {
                Success = false
            };
        }

        await _userManager.ResetAccessFailedCountAsync(appUser);
        await _userManager.SetLockoutEndDateAsync(appUser, null);

        var serviceOption = _configuration.GetOptions<IdentityServiceOptions>("IdentityServiceOptions");

        if (serviceOption.ExternalIdentityBaseUrl is null)
        {
            _logger.LogError("{HandlerName} - {UserId} - External IdentityServer base URL not found!", nameof(UnlockAccountHandler), request.UserId);

            return new AccountResponse
            {
                Success = false
            };
        }

        _logger.LogInformation("{HandlerName} - {UserId} - Finish", nameof(UnlockAccountHandler), request.UserId);

        return new AccountResponse
        {
            Success = true,
            UserName = appUser.UserName,
            ResetPasswordUrl = new Uri(new Uri(serviceOption.ExternalIdentityBaseUrl), "/Account/ForgetPassword").ToString()
        };
    }
}