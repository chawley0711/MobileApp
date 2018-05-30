using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;

namespace AudiOceanServer
{
    public class UserInformation
    {
        public string ID { get; }
        public string DISPLAY_NAME { get; }
        public string PROFILE_URL { get; }

        public UserInformation(string result)
        {
            var user = JObject.Parse(result);
            ID = (string)user["id"];
            DISPLAY_NAME = (string)user["displayName"];
            PROFILE_URL = (string)user["profileURL"];
        }


        public static ICollection<UserInformation> ParseJSONCollection(string result)
        {

            JArray userCollection = (JArray)JObject.Parse(result)["subscriptions"];
            ICollection<UserInformation> users = new List<UserInformation>();

            foreach (var user in userCollection)
            {
                users.Add(new UserInformation(user.ToString()));
            }

            return users;
        }
    }
}