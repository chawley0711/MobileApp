using AudiOcean.Auth;
using System;
using Xamarin.Auth;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

[assembly: XamlCompilation(XamlCompilationOptions.Compile)]
namespace AudiOcean
{
    public partial class App : Application
    {
        public AudiOceanHttpClient HttpClient { get; private set; }
        private const string AuthorizeUrl = "https://accounts.google.com/o/oauth2/v2/auth";
        private const string AccessTokenUrl = "https://www.googleapis.com/oauth2/v4/token";
        private const bool IsUsingNativeUI = true;
        private IGoogleAuthenticatorDelegate _authenticationDelegate;

        private static OAuth2Authenticator _auth;

        public static OAuth2Authenticator OAuth2Authenticator { get => _auth; }
        public GoogleOAuthToken Token { get; private set; }

        public OAuth2Authenticator GetAuthenticator()
        {
            return _auth;
        }

        public void OnPageLoading(Uri uri)
        {

            _auth.OnPageLoading(uri);
        }

        private void OnAuthenticationCompleted(object sender, AuthenticatorCompletedEventArgs e)
        {

            if (e.IsAuthenticated)
            {
                this.Token = new GoogleOAuthToken
                {
                    TokenType = e.Account.Properties["token_type"],
                    AccessToken = e.Account.Properties["access_token"]
                };
                _authenticationDelegate.OnAuthenticationCompleted(Token);
            }
            else
            {
                _authenticationDelegate.OnAuthenticationCancelled();
            }
        }

        private void OnAuthenticationFailed(object sender, AuthenticatorErrorEventArgs e)
        {

        }
        public App(GoogleOAuthToken token = null)
        {
            InitializeComponent();
            if (token == null)
            {
                LogInPage logInPage = new LogInPage();
                MainPage = new NavigationPage(logInPage);
                ((NavigationPage)MainPage).SetValue(NavigationPage.BarBackgroundColorProperty, Color.CornflowerBlue);
                _auth = new OAuth2Authenticator("877386569820-18p3tqbqo7b07og15dkjhe7vfnbjtlkm.apps.googleusercontent.com", string.Empty, "email profile",
                       new Uri("https://accounts.google.com/o/oauth2/auth"),
                       new Uri("com.googleusercontent.apps.877386569820-18p3tqbqo7b07og15dkjhe7vfnbjtlkm:/oauth2redirect"),
                       new Uri("https://accounts.google.com/o/oauth2/token"),
                       null, IsUsingNativeUI);
            }
            else
            {
                this.Token = token;
                this.HttpClient = new AudiOceanHttpClient(Token.AccessToken);
                MainPage = new NavigationPage(new HomePage() { BarBackgroundColor = Color.CornflowerBlue });
                ((NavigationPage)MainPage).SetValue(NavigationPage.BarBackgroundColorProperty, Color.CornflowerBlue);
            }
        }


        protected override void OnStart()
        {

            MainPage.Navigation.PushModalAsync(new SplashPage());
            Device.StartTimer(TimeSpan.FromMilliseconds(5000), () =>
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
