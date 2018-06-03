using System;
using System.Collections.Generic;
using System.Text;

namespace AudiOcean.Auth
{

    public interface IGoogleAuthenticatorDelegate
    {
        void OnAuthenticationCompleted(GoogleOAuthToken token);
        void OnAuthenticationFailed(string message, Exception exception);
        void OnAuthenticationCancelled();
    }
}
