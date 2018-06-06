using System;
using AudiOceanClient;
using System.Collections.Generic;
using System.Text;

namespace AudiOcean
{
    public class Song
    {
        public string name { get; set; }
        public string artist { get; set; }

        public double rating { get; set; }

        public Song(string name, string artist, double rating)
        {
            this.name = name;
            this.artist = artist;
            this.rating = rating;
        }
    }
}
