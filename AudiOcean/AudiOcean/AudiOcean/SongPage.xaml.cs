using AudiOcean.Static_Helper_Classes;
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
    public partial class SongPage : ContentPage
    {
        private bool isPaused = true;
        public SongPage(Song s)
        {
            BindingContext = new Song(s.id, s.name, s.artist, s.rating);
            MusicServiceHelper.Helper.SetSong(BindingContext as Song);
            MusicServiceHelper.Helper.Start();
            MusicServiceHelper.Helper.Play();
            isPaused = false;
            InitializeComponent();
        }


        private void PlayButton_Clicked(object sender, EventArgs e)
        {
            if (isPaused)
            {
                MusicServiceHelper.Helper.Play();
            }
            else
            {
                MusicServiceHelper.Helper.Pause();
            }
            isPaused = !isPaused;
        }

        private void NextButton_Clicked(object sender, EventArgs e)
        {
            MusicServiceHelper.Helper.Next();
        }

        private void PrevButton_Clicked(object sender, EventArgs e)
        {
            MusicServiceHelper.Helper.Previous();
        }

        private void RateSong1GestureRecognizer_Tapped(object sender, EventArgs e)
        {
            App.HttpClient.RateSong((BindingContext as Song).id, 1);
        }

        private void RateSong2GestureRecognizer_Tapped(object sender, EventArgs e)
        {
            App.HttpClient.RateSong((BindingContext as Song).id, 2);
        }
        private void RateSong3GestureRecognizer_Tapped(object sender, EventArgs e)
        {
            App.HttpClient.RateSong((BindingContext as Song).id, 3);
        }
        private void RateSong4GestureRecognizer_Tapped(object sender, EventArgs e)
        {
            App.HttpClient.RateSong((BindingContext as Song).id, 4);
        }
        private void RateSong5GestureRecognizer_Tapped(object sender, EventArgs e)
        {
            App.HttpClient.RateSong((BindingContext as Song).id, 5);
        }


    }
}