using AudiOcean.Auth;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Auth;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace AudiOcean
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class LogInPage : ContentPage
    {
        //private const string AuthorizeUrl = "https://accounts.google.com/o/oauth2/v2/auth";
        //private const string AccessTokenUrl = "https://www.googleapis.com/oauth2/v4/token";
        //private const bool IsUsingNativeUI = false;
   //     private IGoogleAuthenticatorDelegate _authenticationDelegate;



        public LogInPage()
        {
            InitializeComponent();
        }

        protected override void OnAppearing()
        {
            //leave this
            base.OnAppearing();

            //initializeAuth
 

             //subscribe two things to oncomplete:
            // method that says if not authenticated dont do anything
            //method if is authenticated this page dissapear other appear;
        }

        protected override bool OnBackButtonPressed()
        {
            //leave alone
            return false;
        }

        protected override void OnDisappearing()
        {
            base.OnDisappearing();
        }

        private void Button_Clicked(object sender, EventArgs e)
        {
            var presenter = new Xamarin.Auth.Presenters.OAuthLoginPresenter();
            presenter.Login(App.OAuth2Authenticator);
        }
     

    
    }
}