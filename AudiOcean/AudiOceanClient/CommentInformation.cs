using Org.Json;
using System;
using System.Collections.Generic;

namespace AudiOceanClient
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
            var mi = new JSONObject(result);
            ID = mi.GetInt("id");
            DATE_POSTED = DateTime.Parse(mi.GetString("datePosted"));
            TEXT = mi.GetString("text");
            USER_ID = mi.GetInt("userId");
            OWNER_NAME = mi.GetString("ownerName");
        }

        public int ID { get; }
        public DateTime DATE_POSTED { get; }
        public string TEXT { get; }
        public int USER_ID { get; }
        public string OWNER_NAME { get; }

        public static ICollection<CommentInformation> ParseJSONCollection(string result)
        {
            JSONArray comments = new JSONObject(result).GetJSONArray("comments");
            ICollection<CommentInformation> ci = new List<CommentInformation>();
            for (int i = 0; i < comments.Length(); i++)
            {
                ci.Add(new CommentInformation(comments.GetJSONObject(i).ToString()));
            }
            return ci;
        }
    }
}