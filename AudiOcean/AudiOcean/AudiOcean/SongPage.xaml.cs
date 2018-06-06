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
		public SongPage(Song s)
		{
            BindingContext = new Song(s.name, s.artist, s.rating);

			InitializeComponent();
		}

        private bool isPaused = true;

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
    }
}