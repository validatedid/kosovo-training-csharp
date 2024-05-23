namespace VIDconnectBackend.Controllers;

public record AuthorizationRequestDto(string Code, string RedirectUri);