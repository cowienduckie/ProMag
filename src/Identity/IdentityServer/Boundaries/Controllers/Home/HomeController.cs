using Duende.IdentityServer.Services;
using IdentityServer.Common.Attributes;
using IdentityServer.Models.Home;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace IdentityServer.Boundaries.Controllers.Home;

[SecurityHeaders]
[Authorize]
public class HomeController : Controller
{
    private readonly IWebHostEnvironment _environment;
    private readonly IIdentityServerInteractionService _interaction;
    private readonly ILogger<HomeController> _logger;

    public HomeController(
        IIdentityServerInteractionService interaction,
        IWebHostEnvironment environment,
        ILogger<HomeController> logger)
    {
        _interaction = interaction;
        _environment = environment;
        _logger = logger;
    }

    public IActionResult Index()
    {
        if (_environment.IsDevelopment())
        {
            return View();
        }

        _logger.LogInformation("Homepage is disabled in production. Returning 404");

        return NotFound();
    }

    /// <summary>
    ///     Shows the error page
    /// </summary>
    public async Task<IActionResult> Error(string errorId)
    {
        var vm = new ErrorViewModel();

        // retrieve error details from identity server
        var message = await _interaction.GetErrorContextAsync(errorId);

        if (message == null)
        {
            return View("Error", vm);
        }

        vm.Error = message;

        if (!_environment.IsDevelopment())
        {
            // only show in development
            message.ErrorDescription = null;
        }

        return View("Error", vm);
    }
}