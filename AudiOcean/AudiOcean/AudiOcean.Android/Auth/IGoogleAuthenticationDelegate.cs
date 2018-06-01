using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;

namespace AudiOcean.Droid.Auth
{
    public interface IGoogleAuthenticatorDelegate
    {
        void OnAuthenticationCompleted(GoogleOAuthToken token);
        void OnAuthenticationFailed(string message, Exception exception);
        void OnAuthenticationCancelled();
    }
}