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

            BindingContext = new AudiOceanUser("@me", "Me", "ProfilePicture", new List<Song>()
            {
                new Song("Run", "Ski Mask", 107, 4.6, new List<string>() {"It's aight", "Filler", "Filler"}),
                new Song("Massacre", "Dodge & Fuski", 181, 4.1, new List<string>() {"It's aight", "Filler", "Filler"}),
                new Song("Bounce Out With That", "YBN Nahmir", 108, 3.9, new List<string>() {"It's aight", "Filler", "Filler"}),
                new Song("Run", "Ski Mask", 107, 4.6, new List<string>() {"It's aight", "Filler", "Filler"}),
                new Song("Massacre", "Dodge & Fuski", 181, 4.1, new List<string>() {"It's aight", "Filler", "Filler"}),
                new Song("Run", "Ski Mask", 107, 4.6, new List<string>() {"It's aight", "Filler", "Filler"}),
                new Song("Massacre", "Dodge & Fuski", 181, 4.1, new List<string>() {"It's aight", "Filler", "Filler"})
            }, new List<AudiOceanUser>()
            {
                new AudiOceanUser("@you", "You", "ProfilePicture", null, null),
                new AudiOceanUser("@notme", "Not Me", "ProfilePicture", null, null),
                new AudiOceanUser("@notyou", "Not You", "ProfilePicture", null, null)
            });
                //    new List<Song>();
                //{
                //foreach (var mi in App.HttpClient.GetMusicInformationCollection(App.HttpClient.GetUserInformation(0).ID))
                //    songs.Add(new Song(mi.NAME, App.HttpClient.GetUserInformation(mi.OWNER_ID).DISPLAY_NAME, 0, mi.RATING));
                //}
                //Replace 0 with logged in user
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