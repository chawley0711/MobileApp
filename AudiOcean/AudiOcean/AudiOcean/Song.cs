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
        public int length { get; set; }

        public string lengthFormat
        {
            get { return (length / 60) + ":" + (length % 60).ToString("00"); }
        }

        public double rating { get; set; }
        public List<CommentInformation> comments { get; set; }

        public Song(string n, string a, int l, double r, List<CommentInformation> c)
        {
            this.name = n;
            this.artist = a;
            this.length = l;
            this.rating = r;
            this.comments = c;
        }
    }
}
