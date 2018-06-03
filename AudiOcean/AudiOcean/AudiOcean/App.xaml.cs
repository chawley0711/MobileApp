using AudiOcean.Auth;
using System;
using Xamarin.Auth;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

[assembly: XamlCompilation (XamlCompilationOptions.Compile)]
namespace AudiOcean
{
	public partial class App : Application
	{

        private const string AuthorizeUrl = "https://accounts.google.com/o/oauth2/v2/auth";
        private const string AccessTokenUrl = "https://www.googleapis.com/oauth2/v4/token";
        private const bool IsUsingNativeUI = true;
        private IGoogleAuthenticatorDelegate _authenticationDelegate;

        private static OAuth2Authenticator _auth;

        public static OAuth2Authenticator OAuth2Authenticator { get => _auth; }

      

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
                var token = new GoogleOAuthToken
                {
                    TokenType = e.Account.Properties["token_type"],
                    AccessToken = e.Account.Properties["access_token"]
                };
                _authenticationDelegate.OnAuthenticationCompleted(token);
            }
            else
            {
                _authenticationDelegate.OnAuthenticationCancelled();
            }
        }

        private void OnAuthenticationFailed(object sender, AuthenticatorErrorEventArgs e)
        {
        }
        public App ()
		{
			InitializeComponent();

			MainPage = new MainPage();
		}

		protected override void OnStart ()
		{
            _auth = new OAuth2Authenticator("877386569820-18p3tqbqo7b07og15dkjhe7vfnbjtlkm.apps.googleusercontent.com", string.Empty, "email profile",
                                            new Uri("https://accounts.google.com/o/oauth2/auth"),
                                            new Uri("com.googleusercontent.apps.877386569820-18p3tqbqo7b07og15dkjhe7vfnbjtlkm:/oauth2redirect"),
                                            new Uri("https://accounts.google.com/o/oauth2/token"),
                                            null, IsUsingNativeUI);

            _auth.Completed += OnAuthenticationCompleted;
            _auth.Error += OnAuthenticationFailed;

            var presenter = new Xamarin.Auth.Presenters.OAuthLoginPresenter();
            presenter.Login(_auth);
        }

		protected override void OnSleep ()
		{
			// Handle when your app sleeps
		}

		protected override void OnResume ()
		{
			// Handle when your app resumes
		}
	}
}
