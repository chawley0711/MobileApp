using System;
using System.Collections.Generic;
using System.Text;

namespace AudiOcean.playbackStuff
{
    public class AndroidAudiOceanPlaylist
    {
        public int Index { get; private set; }
        public int Count { get; private set; }
        public List<AndroidAudiOceanPlayer> Tracks { get; set; }
        public AndroidAudiOceanPlaylist(List<AndroidAudiOceanPlayer> t)
        {
            this.Tracks = t;
            this.Count = t.Count;
        }
        public AndroidAudiOceanPlayer Next()
        {
            if(Index + 1 != Count)
            {
                Tracks[Index].Release();
                Index++;
                return Tracks[Index];
            }
            else
            {
                return null;
            }
        }
        public AndroidAudiOceanPlayer Previous()
        {
            if (Index - 1 != 0)
            {
                Tracks[Index].Release();
                Index--;
                return Tracks[Index];
            }
            else
            {
                return null;
            }
        }
    }
}
