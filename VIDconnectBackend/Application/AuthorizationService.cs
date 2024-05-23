using VIDconnectBackend.Infrastructure;

namespace VIDconnectBackend.Application;

public interface IAuthorizationService
{
    Task<object> Execute(AuthorizationServiceRequest request);
}

public class AuthorizationService : IAuthorizationService
{
    private readonly IViDconnectApi _viDconnectApi;
    
    public AuthorizationService(IViDconnectApi viDconnectApi)
    {
        _viDconnectApi = viDconnectApi;
    }

    public async Task<object> Execute(AuthorizationServiceRequest request)
    {
        return await _viDconnectApi.GetVerifiableCredentialAsJson(request.Code, request.RedirectUri);
    }
}