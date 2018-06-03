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
		public SongPage()
		{
            BindingContext = new Song("That one song", "Some dude", 194, 3.4, new List<string>() { "It's aight", "Filler", "Filler" });

			InitializeComponent();
		}
	}
}