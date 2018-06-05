using Org.Json;
using System;
using System.Collections.Generic;

namespace AudiOceanServer
{
    public class MusicInformation
    {
        public MusicInformation(string result)
        {
            /*
                type = "Song",
                name = song.SongName,
                rating = song.Ratings.Average((r) => r.Rating1),
                id = song.ID,
                ownerId = song.OwnerID,
                genre = song.Genre.Name,
                dateUploaded = song.DateUploaded */
            JSONObject mi = new JSONObject(result);
            NAME = mi.GetString("name");
            RATING = mi.GetInt("rating");
            ID = mi.GetInt("id");
            GENRE = mi.GetString("genre");
            DATE_UPLOADED = DateTime.Parse(mi.GetString("dateUploaded"));
            OWNER_ID = mi.GetInt("ownerId");


        }

        public string NAME { get; }
        public int RATING { get; }
        public int ID { get; }
        public string GENRE { get; }
        public DateTime DATE_UPLOADED { get; }
        public int OWNER_ID { get; }

        public static ICollection<MusicInformation> ParseJSONCollection(string result)
        {
            JSONObject obj = new JSONObject(result);
            JSONArray array = obj.GetJSONArray("songs");
            ICollection<MusicInformation> mi = new List<MusicInformation>();
            for(int i = 0; i < array.Length(); i++)
                mi.Add(new MusicInformation(array.GetJSONObject(i).ToString()));
            return mi;
        }
    }
}