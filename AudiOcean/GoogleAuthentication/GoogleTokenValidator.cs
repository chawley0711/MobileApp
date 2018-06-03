using Authentication;
using Google.Apis.Auth;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Google.Apis.Auth.GoogleJsonWebSignature;
using Google.Apis.Auth.OAuth2;

namespace GoogleAuthentication
{
    public class GoogleTokenValidator : ITokenValidator<string, Payload>
    {
        public bool ValidateToken(Token<string, Payload> token)
        {
            Task<Payload> validationTask = null;
            try
            {
                validationTask = ValidateAsync(token.Value);
                validationTask.RunSynchronously();
                token.Payload = validationTask.Result;
                return true;
            }
            catch (InvalidJwtException) { return false; }
        }
    }
}
