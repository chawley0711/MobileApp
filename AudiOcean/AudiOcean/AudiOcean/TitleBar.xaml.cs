using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using xf = Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace AudiOcean
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class TitleBar : ContentView
    {
        public String Title { get { return this.TitleLabel.Text; } set { this.TitleLabel.Text = value; } }

        public String ImageSource
        {
            get { return null; }
            set => Logo.Source = xf.ImageSource.FromResource(value);
        }

        public TitleBar()
        {
            InitializeComponent();
        }
    }
}