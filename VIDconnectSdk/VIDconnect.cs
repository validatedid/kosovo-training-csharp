using System.Net.Mime;
using System.Text.Json;

namespace VIDconnectSdk;

public class VIDconnect
{
    private readonly OpenIdConfig _config;

    public VIDconnect(OpenIdConfig config)
    {
        _config = config;
    }

    public async Task<object> GetCredential(string code, string redirectUri)
    {
        var vidconnectApi = new OpenIdApi(_config);
        var oauthToken = await vidconnectApi.GetOauthTokenAsync(code, redirectUri);
        var idToken = new IdToken(oauthToken.id_token);
        return idToken.GetVerifiableCredential();
    }
}

public class OpenIdConfig
{
    public string Url { get; set; }
    public string ClientId { get; set; }
    public string ClientSecret { get; set; }
}

public class OauthToken
{
    public string access_token { get; set; }
    public int expires_in { get; set; }
    public string id_token { get; set; }
    public string scope { get; set; }
    public string token_type { get; set; }
}

public class OpenIdApi
{
    private readonly OpenIdConfig _config;
    private readonly HttpClient _httpClient;

    public OpenIdApi(OpenIdConfig config)
    {
        _config = config ?? throw new ArgumentNullException(nameof(config));
        _httpClient = new HttpClient();
    }

    private readonly Dictionary<string, string> _headers = new Dictionary<string, string>
    {
        { "Content-Type", "application/x-www-form-urlencoded" }
    };

    public async Task<OauthToken> GetOauthTokenAsync(string code, string redirectUri)
    {
        var body = GetBody(code, redirectUri);
        var oauthTokenUrl = $"{_config.Url}/oauth2/token";
        var content = new FormUrlEncodedContent(body);

        var response = await _httpClient.PostAsync(oauthTokenUrl, content);
        response.EnsureSuccessStatusCode();
        var responseBody = await response.Content.ReadAsStringAsync();
        var oauthToken = Newtonsoft.Json.JsonConvert.DeserializeObject<OauthToken>(responseBody);
        if (oauthToken == null) throw new NullReferenceException("Cannot deserialized oauth token");
        return oauthToken;
    }

    private Dictionary<string, string> GetBody(string code, string redirectUri)
    {
        return new Dictionary<string, string>
        {
            { "code", code },
            { "client_id", _config.ClientId },
            { "client_secret", _config.ClientSecret },
            { "redirect_uri", redirectUri },
            { "grant_type", "authorization_code" }
        };
    }
}
