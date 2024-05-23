using VIDconnectSdk;

namespace VIDconnectBackend.Infrastructure;

public interface IViDconnectApi
{
    Task<object> GetVerifiableCredentialAsJson(string authorizationCode, string redirectUrl);
}

public class ViDconnectApi : IViDconnectApi
{
    private readonly VIDconnect _viDconnectApi;

    public ViDconnectApi(IConfiguration configuration)
    {
        var config = new Config(configuration);
        _viDconnectApi = new VIDconnect(new OpenIdConfig
        {
            Url = config.OpenIdUrl,
            ClientId = config.OpenidClientId,
            ClientSecret = config.ClientSecret
        });
    }

    public async Task<object> GetVerifiableCredentialAsJson(string authorizationCode, string redirectUrl)
    {
        return await _viDconnectApi.GetCredential(authorizationCode, redirectUrl);
    }
}