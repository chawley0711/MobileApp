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

        private OAuth2Authenticator _auth;

      

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
            _auth = new OAuth2Authenticator("877386569820-18p3tqbqo7b07og15dkjhe7vfnbjtlkm.apps.googleusercontent.com​", string.Empty, "email profile",
                                            new Uri(AuthorizeUrl),
                                            new Uri("https://www.google.com"),
                                            new Uri(AccessTokenUrl),
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
