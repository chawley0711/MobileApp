using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace AudiOcean
{
	public partial class MainPage : ContentPage
	{
		public MainPage()
		{
			InitializeComponent();
            playSound();
		}
        public void playSound()
        {
            MyPlayer m = new MyPlayer();
            m.init();
        }
	}
}
