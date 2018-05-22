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

			MainPage = new ProfilePage();
		}

		protected override void OnStart()
		{
            //Device.StartTimer(TimeSpan.FromMilliseconds(5000), () =>
            //{
            //    App.Current.MainPage.Navigation.PushAsync(new HomePage());
            //    return false;
            //});
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
