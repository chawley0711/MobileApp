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
	}
}