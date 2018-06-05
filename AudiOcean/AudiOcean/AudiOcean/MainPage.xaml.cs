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
        private object defaultItemTemplate;
        public MainPage()
        {
            InitializeComponent();

            SetUpSongs();
        }

        public void SetUpSongs()
        {
            List<Song> songs =
            new List<Song>();
            foreach (var mi in App.HttpClient.GetMostRecentMusicUploadedInformation(20))
            {
                songs.Add(new Song(mi.NAME, App.HttpClient.GetUserInformation(mi.OWNER_ID).DISPLAY_NAME, 0, mi.RATING));
            }

            //new List<Song>()
            //{
            //    new Song("Run", "Ski Mask", 107, 4.6),
            //    new Song("Massacre", "Dodge & Fuski", 181, 4.1),
            //    new Song("Bounce Out With That", "YBN Nahmir", 108, 3.9)
            //};
            if (songs.Count != 0)
                SongList.ItemsSource = songs;
            else {
                defaultItemTemplate = SongList.ItemTemplate;
                SongList.ItemTemplate = null;
                SongList.ItemsSource = new[] { "Could not load page" };
            }
        }
        private void SongNameLink_Tapped(object sender, EventArgs e)
        {
            var l = sender as Grid;
            Navigation.PushAsync(new SongPage());
        }
    }
}
