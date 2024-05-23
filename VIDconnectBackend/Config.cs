namespace VIDconnectBackend;

public class Config
{
    private readonly IConfiguration _configuration;
    public string ClientSecret => LoadValue<string>("vidconnect.openid_client_secret");
    public string OpenIdUrl => LoadValue<string>("vidconnect.openid_url");
    public string OpenidClientId => LoadValue<string>("vidconnect.openid_client_id");
    
    public Config(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    private T LoadValue<T>(string key)
    {
        var value = _configuration.GetValue<T>(key);
        if (value == null) throw new NullReferenceException("Cannot read key from configuration: " + key);
        return value;
    }
}