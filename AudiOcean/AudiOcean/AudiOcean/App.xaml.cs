using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

[assembly: XamlCompilation (XamlCompilationOptions.Compile)]
namespace AudiOcean
{
	public partial class App : Application
	{
		public App()
		{
			InitializeComponent();

            MainPage = new NavigationPage(new HomePage());
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
