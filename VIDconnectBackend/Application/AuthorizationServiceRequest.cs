namespace VIDconnectBackend.Application;

public record AuthorizationServiceRequest(string Code, string RedirectUri)
{
    public readonly string Code = Code;
    public readonly string RedirectUri = RedirectUri;
}