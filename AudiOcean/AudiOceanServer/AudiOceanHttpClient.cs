using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace AudiOceanServer
{
    public class AudiOceanHttpClient
    {
        HttpClient httpClient;
        private static readonly string remoteAddress = "http://" + "10.10.48.47" + "/";

        public AudiOceanHttpClient()
        {
            this.httpClient = new HttpClient();
        }


        public Stream GetMusic(int id)
        {
            Task<Stream> task = httpClient.GetStreamAsync($"{remoteAddress}music?id={id}");
            task.RunSynchronously();
            return task.Result;
        }

        public UserInformation GetUserInformation(int id)
        {
            Task<string> task = httpClient.GetStringAsync($"{remoteAddress}users?id={id}");
            task.RunSynchronously();
            return new UserInformation(task.Result);
        }

        public ICollection<MusicInformation> GetMusicInformationCollection(int userId)
        {
            Task<string> task = httpClient.GetStringAsync($"{remoteAddress}list-music?id={userId}");
            task.RunSynchronously();
            return MusicInformation.ParseJSONCollection(task.Result);
        }

        public ICollection<MusicInformation> GetMostRecentMusicUploadedInformation(int numOfSongs)
        {
            Task<string> task = httpClient.GetStringAsync($"{remoteAddress}list-music?numOfSongs={numOfSongs}");
            task.RunSynchronously();
            return MusicInformation.ParseJSONCollection(task.Result);
        }

        public ICollection<CommentInformation> GetComments(int songId)
        {
            Task<string> task = httpClient.GetStringAsync($"{remoteAddress}comments?id={songId}");
            task.RunSynchronously();
            return CommentInformation.ParseJSONCollection(task.Result);
        }

        public ICollection<UserInformation> GetUserSubscriptions(int userId)
        {
            Task<string> task = httpClient.GetStringAsync($"{remoteAddress}subscriptions?id={userId}");
            task.RunSynchronously();
            return UserInformation.ParseJSONCollection(task.Result);
        }

        public int GetAverageRatingForSong(int songID)
        {
            Task<Stream> task = httpClient.GetStreamAsync($"{remoteAddress}rate?id={songID}");
            task.RunSynchronously();
            return task.Result.ReadByte();
        }

       


        /*      localAddress + "music/",
                localAddress + "users/",
                localAddress + "list-music/",
                localAddress + "comments/",
                localAddress + "rate/",
                localAddress + "subscriptions/" 
        */




    }
}
