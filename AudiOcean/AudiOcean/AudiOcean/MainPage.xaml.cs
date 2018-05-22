﻿using System;
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

            SetUpSongs();
		}

        public void SetUpSongs()
        {
            SongList.ItemsSource = new List<Song>()
            {
                new Song("Run", "Ski Mask", 107, 4.6),
                new Song("Massacre", "Dodge & Fuski", 181, 4.1),
                new Song("Bounce Out With That", "YBN Nahmir", 108, 3.9)
            };
        }
	}
}
