using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using AudiOcean.Droid.DependencyServices;
using AudiOcean.Interfaces;
using Xamarin.Forms;

[assembly: Dependency(typeof(AndroidMusicDependencyService))]
namespace AudiOcean.Droid.DependencyServices
{
    public class AndroidMusicDependencyService : MusicService
    {
        public void Next()
        {
            throw new NotImplementedException();
        }

        public void Pause()
        {
            throw new NotImplementedException();
        }

        public void Play()
        {
            throw new NotImplementedException();
        }

        public void Previous()
        {
            throw new NotImplementedException();
        }

        public void Release()
        {
            throw new NotImplementedException();
        }

        public void SetSongs(ICollection<Song> songs)
        {
            throw new NotImplementedException();
        }

        public void Start()
        {
            throw new NotImplementedException();
        }
    }
}