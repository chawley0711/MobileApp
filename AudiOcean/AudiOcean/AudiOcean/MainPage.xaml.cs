using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace AudiOcean
{
	public partial class MainPage : ContentPage
	{
		public MainPage()
		{
			InitializeComponent();

            List<Song> songs = new List<Song>();
            //needs a way to change title
            SetUpSongs(songs);
		}

        public void SetUpSongs(List<Song> songs)
        {
            List<Song> songs =
            //new List<Song>();
            //foreach(var mi in App.HttpClient.GetMostRecentMusicUploadedInformation(20))
            //{
            //    songs.Add(new Song(mi.NAME, App.HttpClient.GetUserInformation(mi.OWNER_ID).DISPLAY_NAME, 0, mi.RATING));
            //}

            new List<Song>() {
                new Song("Run", "Ski Mask", 107, 4.6, new List<string>() {"It's aight", "Filler", "Filler"}),
                new Song("Massacre", "Dodge & Fuski", 181, 4.1, new List<string>() { "It's aight", "Filler", "Filler"}),
                new Song("Bounce Out With That", "YBN Nahmir", 108, 3.9, new List<string>() {"It's aight", "Filler", "Filler"})
            };
            SongList.ItemsSource = songs;
        }
        private void SongNameLink_Tapped(object sender, EventArgs e)
        {
            var l = sender as Grid;
            Navigation.PushAsync(new SongPage());
        }
    }
}
