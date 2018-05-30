using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace AudiOceanServer
{
    public class AudiOceanHttpClient
    {
        HttpClient httpClient;
                                                                       //Point address to ServerApp
        private static readonly string remoteAddress = "http://" +          "10.10.48.47"                     + "/";
        private readonly string token;

        public AudiOceanHttpClient(string token)
        {
            this.token = !string.IsNullOrWhiteSpace(token) ? token : throw new ArgumentException("Token cannot be null or empty");
            this.httpClient = new HttpClient();
            this.httpClient.DefaultRequestHeaders.Add("Authentication", $"Bearer {token}");
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



        public bool PostNewUser()
        {
            Task<HttpResponseMessage> task = httpClient.PostAsync($"{remoteAddress}users", new ByteArrayContent(new byte[0]));
            task.RunSynchronously();
            HttpResponseMessage response = task.Result;
            return response.IsSuccessStatusCode;
        }

        public bool PostNewSong(string name, string genre, string audioType,  byte[] songBytes)
        {
            ByteArrayContent content = new ByteArrayContent(songBytes);
            content.Headers.ContentLength = songBytes.Length;
            content.Headers.ContentType = new MediaTypeHeaderValue($"audio/{audioType}");
            Task<HttpResponseMessage> task = httpClient.PostAsync($"{remoteAddress}music?name={name}&genre={genre}", content);
            task.RunSynchronously();
            HttpResponseMessage response = task.Result;
            return response.IsSuccessStatusCode;
        }

        public bool PostNewSong(string name, string genre, string audioType, Stream songByteStream)
        {
            byte[] songBytes;
            if (songByteStream.CanSeek)
            {
                songBytes = new byte[songByteStream.Length];
                songByteStream.Read(songBytes, 0, songBytes.Length);

            }
            else
            {

                List<byte> bytes = new List<byte>();
                int b = songByteStream.ReadByte();
                if (b != -1)
                {
                    bytes.Add((byte)b);
                }
                songBytes = bytes.ToArray();
            }

            return PostNewSong(name, genre, audioType, songBytes);
        }

        public bool RateSong(int songId, byte rating)
        {
            if (rating < 1 || 5 < rating) { throw new ArgumentException("Rating must be 1-5"); }

            Task<HttpResponseMessage> task = httpClient.PostAsync($"{remoteAddress}rate?id={songId}", new ByteArrayContent(new[] { rating }));
            task.RunSynchronously();
            HttpResponseMessage response = task.Result;
            return response.IsSuccessStatusCode;
        }

        public bool AddNewSubscription(int subscriptionId)
        {
            Task<HttpResponseMessage> task = httpClient.PostAsync($"{remoteAddress}subscriptions?id={subscriptionId}", new ByteArrayContent(new byte[0]));
            task.RunSynchronously();
            HttpResponseMessage response = task.Result;
            return response.IsSuccessStatusCode;
        }

        public bool PostComment(int songId, string comment)
        {
            byte[] commentBytes = Encoding.UTF8.GetBytes(comment);
            ByteArrayContent content = new ByteArrayContent(commentBytes);
            content.Headers.ContentLength = commentBytes.Length;
            content.Headers.ContentType = new MediaTypeHeaderValue("text/plain-text");
            Task<HttpResponseMessage> task = httpClient.PostAsync($"{remoteAddress}comments?id={songId}", content);
            task.RunSynchronously();
            return task.Result.IsSuccessStatusCode;
        }

        public bool DeleteSong(int songId)
        {
            Task<HttpResponseMessage> task = httpClient.DeleteAsync($"{remoteAddress}music?id={songId}");
            task.RunSynchronously();
            return task.Result.IsSuccessStatusCode;
        }

        public bool DeleteSubscription(int subscriptionId)
        {
            Task<HttpResponseMessage> task = httpClient.DeleteAsync($"{remoteAddress}subscriptions?id={subscriptionId}");
            task.RunSynchronously();
            return task.Result.IsSuccessStatusCode;
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
