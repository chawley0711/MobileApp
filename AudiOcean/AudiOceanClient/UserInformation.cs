
using Org.Json;
using System;
using System.Collections.Generic;

namespace AudiOceanServer
{
    public class UserInformation
    {
        public int ID { get; }
        public string DISPLAY_NAME { get; }
        public string PROFILE_URL { get; }

        public UserInformation(string result)
        {
            var user = new JSONObject(result);
            ID = user.GetInt("id");
            DISPLAY_NAME = user.GetString("displayName");
            PROFILE_URL = user.GetString("profileURL");
        }


        public static ICollection<UserInformation> ParseJSONCollection(string result)
        {

            JSONArray userCollection = new JSONObject(result).GetJSONArray("subscriptions");
            ICollection<UserInformation> users = new List<UserInformation>();

            for (int i = 0; i < userCollection.Length(); i++)
            {
                users.Add(new UserInformation(userCollection.GetJSONObject(i).ToString()));
            }

            return users;
        }
    }
}