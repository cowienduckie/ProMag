using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PersonalData.Data.Exceptions;
using PersonalData.UseCases.Commands;
using PersonalData.UseCases.Queries;
using Shared;
using Shared.Storage;
using Shared.Storage.Extensions;

namespace PersonalData.Boundaries.Controller;

[Route("api/[controller]")]
[ApiController]
public class PersonalDataController : ControllerBase
{
    private readonly IWebHostEnvironment _env;
    private readonly IStore _fileStore;
    private readonly IMediator _mediator;

    public PersonalDataController(IStorageFactory storageFactory, IMediator mediator, IWebHostEnvironment env)
    {
        _env = env;
        _mediator = mediator;
        _fileStore = storageFactory.GetStore("filesystem")!;
    }

    private string CurrentUserId => HttpContext.User.FindFirst("sub")?.Value ?? throw new ArgumentNullException();
    private static string AvatarFolder => "avatar";

    [HttpGet("ping")]
    public ActionResult<string> Ping()
    {
        return "pong";
    }

    [HttpPost("avatar")]
    [Authorize]
    public async Task<IActionResult> UploadAvatar(IFormFile avatar)
    {
        Guard.NotNull(avatar);

        var avatarFileName = $"{DateTime.UtcNow.Ticks}_{avatar.FileName}";

        if (avatar.Length > 0)
        {
            await UploadPicture(avatar, avatarFileName);

            await _mediator.Send(new UpdateUserAvatarCommand
            {
                PersonId = Guid.Parse(CurrentUserId),
                AvatarFileName = avatarFileName
            });
        }
        else
        {
            return BadRequest();
        }

        return Ok(new
        {
            AvatarFileName = avatarFileName
        });
    }

    [HttpGet("avatar")]
    public async Task<FileContentResult> GetAvatar()
    {
        var person = await _mediator.Send(new GetPersonByIdQuery(Guid.Parse(CurrentUserId)));

        Guard.NotNull(person);

        try
        {
            if (person?.PhotoPath is null)
            {
                throw new ArgumentNullException();
            }

            var path = Path.Combine(AvatarFolder, person.PhotoPath);
            var fileContents = await _fileStore.ReadAllBytesAsync(path);

            return File(fileContents, MimeMapping.GetMimeMapping(person.PhotoPath));
        }
        catch
        {
            var buffer = await System.IO.File.ReadAllBytesAsync(Path.Combine(_env.WebRootPath, "images/user.png"));

            return File(buffer, "image/png");
        }
    }

    private async Task UploadPicture(IFormFile picture, string fileName)
    {
        const int fileLength = 1024 * 1024 * 5; // 5MB

        if (picture.Length > fileLength)
        {
            throw new PersonalDataException("Image exceeds the request content length.");
        }

        using var memoryStream = new MemoryStream();
        await picture.CopyToAsync(memoryStream);

        if (!memoryStream.ToArray().IsValidFile(FileExtensions.FileType.Image, picture.ContentType))
        {
            throw new PersonalDataException("Image is invalid format.");
        }

        await _fileStore.SaveAsync(memoryStream, Path.Combine(AvatarFolder, fileName));
    }
}