﻿using AudiOceanClient;
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
        private object defaultItemTemplate;
        public MainPage()
        {
            InitializeComponent();

            SetUpSongs();
            //needs a way to change title
		}

        public void SetUpSongs()
        {
            List<Song> songs =
            new List<Song>();
            var task = App.HttpClient.GetMostRecentMusicUploadedInformation(20);
            task.ContinueWith((t) =>
            {
                if (t.Status == TaskStatus.Faulted)
                {

                    Device.BeginInvokeOnMainThread(() =>
                    {
                        defaultItemTemplate = SongList.ItemTemplate;
                        SongList.ItemTemplate = null;
                        SongList.ItemsSource = new[] { "Could not load page" };

                    });
                }
                else
                {
                    foreach (var mi in t.Result)
                    {
                        songs.Add(new Song(mi.NAME, "Blah", 0, mi.RATING, new List<CommentInformation>()));
                    }
                    SongList.ItemsSource = songs;
                }

            });
            //new List<Song>()
            //{
            //    new Song("Run", "Ski Mask", 107, 4.6),
            //    new Song("Massacre", "Dodge & Fuski", 181, 4.1),
            //    new Song("Bounce Out With That", "YBN Nahmir", 108, 3.9)
            //};
        }
        private void SongNameLink_Tapped(object sender, EventArgs e)
        {
            var l = sender as Grid;
            Navigation.PushAsync(new SongPage(l.BindingContext as Song));
        }
    }
}
