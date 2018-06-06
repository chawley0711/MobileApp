using AudiOcean.Static_Helper_Classes;
using AudiOceanClient;
using Plugin.FilePicker;
using Plugin.FilePicker.Abstractions;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AudiOceanClient;
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
            ObservableCollection<Song> songs =
                    new ObservableCollection<Song>();
            var task = App.HttpClient.GetMusicInformationCollection(1);
            task.ContinueWith((t) =>
            {
                if (t.IsFaulted)
                {

                    SongList.ItemTemplate = null;
                    SongList.ItemsSource = new[] { "Couldn't load page." };
                    return;
                }
                foreach (var mi in t.Result)
                {
                    Task<UserInformation> userInformation = App.HttpClient.GetUserInformation(mi.OWNER_ID);
                    userInformation.ContinueWith((a) =>
                    {
                        if (a.IsCompletedSuccessfully)
                        {
                            AddSong(songs, mi, a.Result);
                        }
                    });
                }

                Device.BeginInvokeOnMainThread(() => SongList.ItemsSource = songs);
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

        private void AddSong(ObservableCollection<Song> songs, MusicInformation mi, UserInformation result)
        {
            songs.Add(new Song(mi.ID, mi.NAME, result.DISPLAY_NAME, mi.RATING));
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

        private async void Upload_Tapped(object sender, EventArgs e)
        {
            try
            {
                FileData fd = await CrossFilePicker.Current.PickFile();
            } catch(Exception) { }
        }

        private bool isPaused = true;
        private void PlayTapGestureRecognizer_Tapped(object sender, EventArgs e)
        {
            var img = sender as Image;
            if (isPaused)
            {
                img.Source = ImageSource.FromResource("drawable/PauseButton");
                MusicServiceHelper.Helper.Play();
                isPaused = false;
            }
            else
            {
                img.Source = ImageSource.FromResource("drawable/Playbutton");
                MusicServiceHelper.Helper.Pause();
                isPaused = true;
            }

        }
    }
}