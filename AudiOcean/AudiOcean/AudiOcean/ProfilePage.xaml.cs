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
		}

        private void SongNameLink_Tapped(object sender, EventArgs e)
        {
            Label l = (Label)sender;
            //Song s = (Song)l.BindingContext;
            Navigation.PushAsync(new SongPage());
        }
    }
}