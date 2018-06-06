using Authentication;
using static Google.Apis.Auth.GoogleJsonWebSignature;
using System.Net;
using System.IO;
using Carbon.Json;

namespace GoogleAuthentication
{
    public class GoogleTokenValidator : ITokenValidator<string, Payload>
    {
        public bool ValidateToken(Token<string, Payload> token)
        {
            var req = WebRequest.CreateHttp("https://www.googleapis.com/oauth2/v2/userinfo");
            req.Headers.Add(HttpRequestHeader.Authorization, $"Bearer {token.Value}");
            req.Method = "GET";
            req.ContentLength = 0;
            var res = req.GetResponse() as HttpWebResponse;
            if (res.StatusCode != HttpStatusCode.OK)
            {
                return false;
            }
            var json = new StreamReader(res.GetResponseStream()).ReadToEnd();
            JsonObject value = JsonObject.Parse(json);
            token.Payload = new Payload()
            {
                Email = value["email"],
                Name = value["name"],
                GivenName = value["given_name"],
                Picture = value["picture"],
                FamilyName = value["family_name"]
            };
            return true;
            
        }
    }
}
