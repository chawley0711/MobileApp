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
using AudiOcean.Droid.Services;

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
            Intent intent = new Intent(Android.App.Application.Context, typeof(AndroidAudioService));
            intent.SetAction(AndroidAudioService.ActionPause);
            //intent.GetIntExtra("id", 0)
            Android.App.Application.Context.StartService(intent);
        }

        public void Play()
        {
            Intent intent = new Intent(Android.App.Application.Context, typeof(AndroidAudioService));
            intent.SetAction(AndroidAudioService.ActionPlay);
            //intent.GetIntExtra("id", 0)
            Android.App.Application.Context.StartService(intent);
        }

        public void Previous()
        {
            throw new NotImplementedException();
        }

        public void Release()
        {
            Intent intent = new Intent(Android.App.Application.Context, typeof(AndroidAudioService));
            intent.SetAction(AndroidAudioService.ActionStop);
            //intent.GetIntExtra("id", 0)
            Android.App.Application.Context.StartService(intent);
        }

        public void SetSong(Song songs)
        {
            Intent intent = new Intent(Android.App.Application.Context, typeof(AndroidAudioService));
            intent.SetAction(AndroidAudioService.ActionSetSource);
            //intent.GetIntExtra("id", 0)
            intent.PutExtra("id", songs.id);
            Android.App.Application.Context.StartService(intent);

        }

        public void Start()
        {
  
        }
    }
}