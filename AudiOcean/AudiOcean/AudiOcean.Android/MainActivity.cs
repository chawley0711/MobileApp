using System;

using Android.App;
using Android.Content.PM;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.OS;
using AudiOcean.Droid.Auth;
using Android.Content;

namespace AudiOcean.Droid
{
    [Activity(Label = "AudiOcean", Icon = "@mipmap/icon", Theme = "@style/MainTheme", MainLauncher = true, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation)]
    public class MainActivity : global::Xamarin.Forms.Platform.Android.FormsAppCompatActivity
    {
        protected override void OnCreate(Bundle bundle)
        {
            TabLayoutResource = Resource.Layout.Tabbar;
            ToolbarResource = Resource.Layout.Toolbar;

            base.OnCreate(bundle);
            global::Xamarin.Forms.Forms.Init(this, bundle);

            global::Xamarin.Auth.Presenters.XamarinAndroid.AuthenticationConfiguration.Init(this, bundle);

            if (Intent.HasExtra("net.audiocean.token"))
            {
                LoadApplication(new App(new AudiOcean.Auth.GoogleOAuthToken() { AccessToken = Intent.GetStringExtra("net.audiocean.token") }));
            }
            else
                LoadApplication(new App());
        }
    }
}