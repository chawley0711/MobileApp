using System;
using System.Collections.Generic;
using System.Text;

namespace AudiOcean
{
    public class Song
    {
        public string Name { get; set; }
        public string Artist { get; set; }
        public int Length { get; set; }

        public string LengthFormat
        {
            get { return (Length / 60) + ":" + (Length % 60).ToString("00"); }
        }

        public double Rating { get; set; }

        public Song(string n, string a, int l, double r)
        {
            this.Name = n;
            this.Artist = a;
            this.Length = l;
            this.Rating = r;


        }
    }
}
