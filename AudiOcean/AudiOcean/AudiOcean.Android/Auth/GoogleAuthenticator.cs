﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Xamarin.Auth;
namespace AudiOcean.Droid.Auth
{
    public class GoogleAuthenticator
    {
        private const string AuthorizeUrl = "https://accounts.google.com/o/oauth2/v2/auth";
        private const string AccessTokenUrl = "https://www.googleapis.com/oauth2/v4/token";
        private const bool IsUsingNativeUI = true;

        private OAuth2Authenticator _auth;
        private IGoogleAuthenticatorDelegate _authenticationDelegate;
        
        public GoogleAuthenticator(string clientId, string scope, string redirectUrl, IGoogleAuthenticatorDelegate authenticationDelegate)
        {
            _authenticationDelegate = authenticationDelegate;

            _auth = new OAuth2Authenticator(clientId, string.Empty, scope,
                                            new Uri(AuthorizeUrl),
                                            new Uri(redirectUrl),
                                            new Uri(AccessTokenUrl),
                                            null, IsUsingNativeUI);

            _auth.Completed += OnAuthenticationCompleted;
            _auth.Error += OnAuthenticationFailed;


        }

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
            _authenticationDelegate.OnAuthenticationFailed(e.Message, e.Exception);
        }

    }
}