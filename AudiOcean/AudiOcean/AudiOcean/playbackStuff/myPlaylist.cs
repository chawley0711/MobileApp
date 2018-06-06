using System;
using System.Collections.Generic;
using System.Text;

namespace AudiOcean.playbackStuff
{
    public class myPlaylist
    {
        public int index { get; set; }
        public int maxindex { get; set; }
        public List<MyPlayer> tracks { get; set; }
        public myPlaylist(List<MyPlayer> t)
        {
            this.tracks = t;
            this.maxindex = t.Count;
        }
        public MyPlayer Next()
        {
            if(index + 1 != maxindex)
            {
                tracks[index].myRelease();
                index++;
                return tracks[index];
            }
            else
            {
                return null;
            }
        }
        public MyPlayer Previous()
        {
            if (index - 1 != 0)
            {
                tracks[index].myRelease();
                index--;
                return tracks[index];
            }
            else
            {
                return null;
            }
        }
    }
}
