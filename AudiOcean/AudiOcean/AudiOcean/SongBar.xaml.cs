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
    public partial class SongBar : ContentView
    {
        public string SongName = "";
        public string SongArtist = "";
        public SongBar()
        {
            //set these things here
            InitializeComponent();
        }

        private void BackwardTapGestureRecognizer_Tapped(object sender, EventArgs e)
        {
            MusicServiceHelper.Helper.Previous();
        }

        private void ForwardTapGestureRecognizer_Tapped(object sender, EventArgs e)
        {
            MusicServiceHelper.Helper.Next();
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