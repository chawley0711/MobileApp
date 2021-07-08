using System;
using System.Collections.Generic;
using System.Text;

namespace AudiOcean
{
    public class AudiOceanUser
    {
        public string userName { get; set; }

        public string displayName { get; set; }

        public string profilePicture { get; set; }

        public List<Song> userSongs { get; set; }

        public List<AudiOceanUser> subscribedList { get; set; }

        public AudiOceanUser(string u, string d, string p, List<Song> s, List<AudiOceanUser> sub)
        {
            this.userName = u;
            this.displayName = d;
            this.profilePicture = p;
            this.subscribedList = sub;
            this.userSongs = s;
        }
    }
}
