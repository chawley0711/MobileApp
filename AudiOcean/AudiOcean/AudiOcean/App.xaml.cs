using AudiOceanClient;
using AudiOcean.Auth;
using System;
using Xamarin.Auth;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using System.Threading.Tasks;
using System.Net.Http;
using System.Net.Http.Headers;

[assembly: XamlCompilation(XamlCompilationOptions.Compile)]
namespace AudiOcean
{
    public partial class App : Application
    {
        public static event Action UpdateUser;

        public static AudiOceanHttpClient HttpClient { get; private set; }
        //private const string AuthorizeUrl = "https://accounts.google.com/o/oauth2/v2/auth";
        //private const string AccessTokenUrl = "https://www.googleapis.com/oauth2/v4/token";
        //private const bool IsUsingNativeUI = true;

        public static OAuth2Authenticator OAuth2Authenticator { get; private set; }
        public GoogleOAuthToken Token { get; private set; }
        public static UserInformation CURRENT_USER { get; private set; }

        public App(GoogleOAuthToken token = null)
        {
            InitializeComponent();
            if (token == null)
            {
                LogInPage logInPage = new LogInPage();
                MainPage = new NavigationPage(logInPage);
                ((NavigationPage)MainPage).SetValue(NavigationPage.BarBackgroundColorProperty, Color.CornflowerBlue);
                OAuth2Authenticator = new OAuth2Authenticator("877386569820-18p3tqbqo7b07og15dkjhe7vfnbjtlkm.apps.googleusercontent.com", string.Empty, $"email profile",
                       new Uri("https://accounts.google.com/o/oauth2/auth"),
                       new Uri("com.googleusercontent.apps.877386569820-18p3tqbqo7b07og15dkjhe7vfnbjtlkm:/oauth2redirect"),
                       new Uri("https://accounts.google.com/o/oauth2/token"),
                       null, true);
            }
            else
            {
                this.Token = token;
                HttpClient = new AudiOceanHttpClient(Token.AccessToken);
                MainPage = new NavigationPage(new HomePage() { BarBackgroundColor = Color.CornflowerBlue });
                ((NavigationPage)MainPage).SetValue(NavigationPage.BarBackgroundColorProperty, Color.CornflowerBlue);
                HttpClient.GetCurrentlyLoggedInUserInformation().ContinueWith((t) =>
                {
                    if (t.IsCompletedSuccessfully)
                    {
                        CURRENT_USER = t.Result;
                        UpdateUser?.Invoke();
                    }
                });
            }
        }


        protected override void OnStart()
        {
            MainPage.Navigation.PushModalAsync(new SplashPage());
            Device.StartTimer(TimeSpan.FromMilliseconds(3000), () =>
            {
                MainPage.Navigation.PopModalAsync();
                return false;
            });
        }

        protected override void OnSleep()
        {
            // Handle when your app sleeps
        }

        protected override void OnResume()
        {
            // Handle when your app resumes
        }
    }
}
