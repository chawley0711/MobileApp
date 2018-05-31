using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace AudiOcean
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class ProfilePage : ContentPage
	{
		public ProfilePage()
		{
			InitializeComponent();

            SetUpSongs();
		}

        public void SetUpSongs()
        {
            List<Song> songs = new List<Song>();
            foreach (var mi in App.HttpClient.GetMusicInformationCollection(App.HttpClient.GetUserInformation(0).ID))
            {
                songs.Add(new Song(mi.NAME, App.HttpClient.GetUserInformation(mi.OWNER_ID).DISPLAY_NAME, 0, mi.RATING));
            }
            SongList.ItemsSource = songs;
            //Replace 0 with logged in user

            //    new List<Song>()
            //    {
            //        new Song("Run", "Ski Mask", 107, 4.6),
            //        new Song("Massacre", "Dodge & Fuski", 181, 4.1),
            //        new Song("Bounce Out With That", "YBN Nahmir", 108, 3.9),
            //        new Song("Run", "Ski Mask", 107, 4.6),
            //        new Song("Massacre", "Dodge & Fuski", 181, 4.1),
            //        new Song("Run", "Ski Mask", 107, 4.6),
            //        new Song("Massacre", "Dodge & Fuski", 181, 4.1)
            //    };
        }

        private void SongNameLink_Tapped(object sender, EventArgs e)
        {
            Label l = (Label)sender;
            //Song s = (Song)l.BindingContext;
            Navigation.PushAsync(new SongPage());
        }
    }
}