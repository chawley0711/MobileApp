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

            var task = App.HttpClient.GetCurrentlyLoggedInUsersID();
            task.ContinueWith((t) =>
            {
                if (t.IsFaulted)
                {

                }
                else
                {
                    var thisUsersID = t.Result;

                    //var au = App.HttpClient.GetUserInformation(thisUsersID);
                    //au.ContinueWith((a) =>
                    //{
                    //    if(a.IsFaulted)
                    //    {
                            
                    //    }
                    //    else
                    //    {
                    //        new AudiOceanUser(a.Result.DISPLAY_NAME, a.Result.DISPLAY_NAME, a.Result.PROFILE_URL, new List<Song>(), new List<AudiOceanUser>());
                    //    }
                    //});
                }
            });

            //SetUpSongs();
        }

        public void SetUpSongs()
        {
            List<Song> songs =
                    new List<Song>();
            var task = App.HttpClient.GetMusicInformationCollection(0);
            task.ContinueWith((t) =>
            {
                if (t.IsFaulted)
                {
                    Device.BeginInvokeOnMainThread(() =>
                    {
                        SongList.ItemTemplate = null;
                        SongList.ItemsSource = new[] { "Couldn't load page." };
                    });
                    return;
                }
                foreach (var mi in t.Result)
                {
                    songs.Add(new Song(mi.NAME, "Blah", 0, mi.RATING, new List<AudiOceanClient.CommentInformation>()));
                }

                Device.BeginInvokeOnMainThread(() =>
                {
                    SongList.ItemsSource = songs;
                });
            });
            //Replace 0 with logged in user

            //new List<Song>()
            //{
            //    new Song("Run", "Ski Mask", 107, 4.6),
            //    new Song("Massacre", "Dodge & Fuski", 181, 4.1),
            //    new Song("Bounce Out With That", "YBN Nahmir", 108, 3.9),
            //    new Song("Run", "Ski Mask", 107, 4.6),
            //    new Song("Massacre", "Dodge & Fuski", 181, 4.1),
            //    new Song("Run", "Ski Mask", 107, 4.6),
            //    new Song("Massacre", "Dodge & Fuski", 181, 4.1)
            //};
        }

        private void SongNameLink_Tapped(object sender, EventArgs e)
        {
            var l = sender as Grid;
            Navigation.PushAsync(new SongPage(l.BindingContext as Song));
        }

        private void MySongs_Tapped(object sender, EventArgs e)
        {
            SongList.IsVisible = true;
            SubscribeList.IsVisible = false;
        }

        private void SubbedTo_Tapped(object sender, EventArgs e)
        {
            SongList.IsVisible = false;
            SubscribeList.IsVisible = true;
        }
    }
}