using Carbon.Json;
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
            JsonObject mi = JsonObject.Parse(result);
            NAME = (string)mi["name"];
            RATING = (int)mi["rating"];
            ID = (int)mi["id"];
            GENRE = (string)mi["genre"];
            DATE_UPLOADED = (DateTime)mi["dateUploaded"];
            OWNER_ID = (int)mi["ownerId"];


        }

        public string NAME { get; }
        public int RATING { get; }
        public int ID { get; }
        public string GENRE { get; }
        public DateTime DATE_UPLOADED { get; }
        public int OWNER_ID { get; }

        public static ICollection<MusicInformation> ParseJSONCollection(string result)
        {
            JsonArray array = (JsonArray)JsonObject.Parse(result)["songs"];
            ICollection<MusicInformation> mi = new List<MusicInformation>();
            foreach (var song in array)
                mi.Add(new MusicInformation(song.ToString()));
            return mi;
        }
    }
}