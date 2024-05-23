using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using VIDconnectBackend.Application;

namespace VIDconnectBackend.Controllers;

[ApiController]
[Route("vidconnect-back")]
public class AuthorizationController : ControllerBase
{
    private readonly IAuthorizationService _service;

    public AuthorizationController(IAuthorizationService service)
    {
        _service = service;
    }

    [HttpPost]
    [Route("authorize")]
    public async Task<ContentResult> Authorize(AuthorizationRequestDto requestDto)
    {
        var verifiableCredential = await _service.Execute(new AuthorizationServiceRequest(requestDto.Code, requestDto.RedirectUri));
        return new ContentResult
        {
            Content = JsonConvert.SerializeObject(new AuthorizationResponseDto(verifiableCredential)),
            ContentType = "application/json",
            StatusCode = 200
        };
    }
}