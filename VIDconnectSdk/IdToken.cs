using Newtonsoft.Json;

namespace VIDconnectSdk;

using System;
using System.Text;

public class IdToken
{
    private readonly string _idToken;
    private readonly dynamic _idTokenParsed;

    public IdToken(string idToken)
    {
        _idToken = idToken;
        _idTokenParsed = ParseJwt();
    }

    private dynamic ParseJwt()
    {
        string[] tokenParts = _idToken.Split('.');
        string base64Payload = tokenParts[1];
        byte[] payloadBytes = Convert.FromBase64String(AddPadding(base64Payload));
        string payloadJson = Encoding.UTF8.GetString(payloadBytes);
        var payload = JsonConvert.DeserializeObject(payloadJson);
        if (payload == null) throw new NullReferenceException("Payload cannot be deserialized");
        return payload;
    }

    private string AddPadding(string base64String)
    {
        int missingPadding = 4 - (base64String.Length % 4);
        
        if (missingPadding != 4)
        {
            return base64String + new string('=', missingPadding);
        }
        else
        {
            return base64String;
        }
    }
    public object GetVerifiableCredential()
    {
        if (_idTokenParsed.vp != null &&
            _idTokenParsed.vp.verifiableCredential != null &&
            _idTokenParsed.vp.verifiableCredential.Count > 0)
        {
            dynamic verifiableCredential = _idTokenParsed.vp.verifiableCredential[0];
            if (verifiableCredential.payload != null &&
                verifiableCredential.payload.vc != null)
            {
                return verifiableCredential.payload.vc;
            }
            return verifiableCredential;
        }
        return false;
    }
}
