using Carbon.Json;
using System;
using System.Collections.Generic;

namespace AudiOceanServer
{
    public class CommentInformation
    {
        public CommentInformation(string result)
        {
            /*
                id = comment.CommentID,
                datePosted = comment.DatePosted,
                text = comment.Text,
                userId = comment.UserID,
                ownerName = comment.User.DisplayName
            */
            var mi = JsonObject.Parse(result);
            ID = (int)mi["id"];
            DATE_POSTED = (DateTime)mi["datePosted"];
            TEXT = (string)mi["text"];
            USER_ID = (int)mi["userId"];
            OWNER_NAME = (string)mi["ownerName"];
        }

        public int ID { get; }
        public DateTime DATE_POSTED { get; }
        public string TEXT { get; }
        public int USER_ID { get; }
        public string OWNER_NAME { get; }

        public static ICollection<CommentInformation> ParseJSONCollection(string result)
        {
            JsonArray comments = (JsonArray)JsonObject.Parse(result)["comments"];
            ICollection<CommentInformation> ci = new List<CommentInformation>();
            foreach (var comment in comments)
                ci.Add(new CommentInformation(comment.ToString()));
            return ci;
        }
    }
}