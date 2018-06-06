using AudiOcean.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace AudiOcean.Static_Helper_Classes
{
    public class MusicServiceHelper : MusicService
    {
        public static MusicServiceHelper Helper { get; } = new MusicServiceHelper();
        public void Next()
        {
            DependencyService.Get<MusicService>().Next();
        }

        public void Pause()
        {
            DependencyService.Get<MusicService>().Pause();
        }

        public void Play()
        {
            DependencyService.Get<MusicService>().Play();
        }

        public void Previous()
        {
            DependencyService.Get<MusicService>().Previous();
        }

        public void Release()
        {
            DependencyService.Get<MusicService>().Release();
        }

        public void SetSong(Song songs)
        {
            DependencyService.Get<MusicService>().SetSong(songs);
        }

        public void Start()
        {
            DependencyService.Get<MusicService>().Start();
        }
    }
}
