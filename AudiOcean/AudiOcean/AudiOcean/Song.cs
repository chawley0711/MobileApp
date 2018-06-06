using System;
using AudiOceanClient;
using System.Collections.Generic;
using System.Text;

namespace AudiOcean
{
    public class Song
    {
        public int id { get; set; }
        public string name { get; set; }
        public string artist { get; set; }

        public double rating { get; set; }

        public Song(int id, string name, string artist, double rating)
        {
            this.id = id;
            this.name = name;
            this.artist = artist;
            this.rating = rating;
        }
    }
}
