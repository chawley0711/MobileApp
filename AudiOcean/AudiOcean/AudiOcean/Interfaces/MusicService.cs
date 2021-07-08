using System;
using System.Collections.Generic;
using System.Text;

namespace AudiOcean.Interfaces
{
    public interface MusicService
    {
        void SetSong(Song songs);
        void Play();
        void Pause();
        void Release();
        void Start();
        void Next();
        void Previous();
    }
}
