using Android.Media;
using System;
using System.Collections.Generic;
using System.Text;

namespace AudiOcean.playbackStuff
{
    public class myPlaylist
    {
        public int index { get; set; }
        public int maxindex { get; set; }
        public List<AudioTrack> tracks { get; set; }
        public myPlaylist(List<AudioTrack> t)
        {
            this.tracks = t;
        }
    }
}
