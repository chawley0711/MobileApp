using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Xamarin.Auth;

namespace AudiOcean
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class GoogleAuthPage : ContentPage
	{
		public GoogleAuthPage ()
		{
			InitializeComponent ();
        }

	}
}