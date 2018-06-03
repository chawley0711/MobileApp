using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.Content.PM;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;

namespace AudiOcean.Droid
{
    [Activity(Label = "SignInActivity", NoHistory = true, LaunchMode = LaunchMode.SingleTop)]
    [IntentFilter(
    new[] { Intent.ActionView },
    Categories = new[] { Intent.CategoryDefault, Intent.CategoryBrowsable },
    DataSchemes = new[] { "com.googleusercontent.apps.877386569820-18p3tqbqo7b07og15dkjhe7vfnbjtlkm" },
    DataPath = "/oauth2redirect")]
    public class SignInActivity : Activity
    {
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            var uri = new Uri(Intent.Data.ToString());

            // Load redirectUrl page
            App.OAuth2Authenticator.OnPageLoading(uri);

            Finish();
            // Create your application here
        }
    }
}