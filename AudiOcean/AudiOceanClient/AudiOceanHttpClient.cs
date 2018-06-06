using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace AudiOceanClient
{
    public class AudiOceanHttpClient
    {
        HttpClient httpClient;
        //Point address to ServerApp
        private static readonly string remoteAddress = "http://" + "192.168.0.100" + "/";
        private readonly string token;

        public AudiOceanHttpClient(string token)
        {
            this.token = !string.IsNullOrWhiteSpace(token) ? token : throw new ArgumentException("Token cannot be null or empty");
            this.httpClient = new HttpClient();
            this.httpClient.DefaultRequestHeaders.Add("Authentication", $"Bearer {this.token}");
        }


        public Stream GetMusic(int id)
        {
            Task<Stream> s = httpClient.GetStreamAsync($"{remoteAddress}music?id={id}");
            return s.Result;
        }

        public async Task<UserInformation> GetUserInformation(int id)
        {
            string task = await httpClient.GetStringAsync($"{remoteAddress}users?id={id}");

            return new UserInformation(task);
        }

        public async Task<ICollection<MusicInformation>> GetMusicInformationCollection(int userId)
        {
            var task = await httpClient.GetStringAsync($"{remoteAddress}list-music?id={userId}");

            return MusicInformation.ParseJSONCollection(task);
        }

        public async Task<ICollection<MusicInformation>> GetMostRecentMusicUploadedInformation(int numOfSongs)
        {
            string task = await httpClient.GetStringAsync($"{remoteAddress}list-music?numOfSongs={numOfSongs}");
            return MusicInformation.ParseJSONCollection(task);
        }

        public async Task<ICollection<CommentInformation>> GetComments(int songId)
        {
            string task = await httpClient.GetStringAsync($"{remoteAddress}comments?id={songId}");
            return CommentInformation.ParseJSONCollection(task);
        }

        public async Task<ICollection<UserInformation>> GetUserSubscriptions(int userId)
        {
            string task = await httpClient.GetStringAsync($"{remoteAddress}subscriptions?id={userId}");
            return UserInformation.ParseJSONCollection(task);
        }

        public async Task<int> GetAverageRatingForSong(int songID)
        {
            Stream task = await httpClient.GetStreamAsync($"{remoteAddress}rate?id={songID}");
            return task.ReadByte();
        }

        public async Task<UserInformation> GetCurrentlyLoggedInUserInformation()
        {
            var task = await httpClient.GetStringAsync($"{remoteAddress}users");
            return new UserInformation(task);
        }

        public async Task<bool> PostNewUser()
        {
            var task = await httpClient.PostAsync($"{remoteAddress}users", new ByteArrayContent(new byte[0]));
            return task.IsSuccessStatusCode;
        }

        public async Task<bool> PostNewSong(string name, string genre, string audioType, byte[] songBytes)
        {
            ByteArrayContent content = new ByteArrayContent(songBytes);
            content.Headers.ContentLength = songBytes.Length;
            content.Headers.ContentType = new MediaTypeHeaderValue($"audio/{audioType}");
            var task = await httpClient.PostAsync($"{remoteAddress}music?name={name}&genre={genre}", content);
            return task.IsSuccessStatusCode;
        }

        public async Task<bool> PostNewSong(string name, string genre, string audioType, Stream songByteStream)
        {
            byte[] songBytes;
            if (songByteStream.CanSeek)
            {
                songBytes = new byte[songByteStream.Length];
                await songByteStream.ReadAsync(songBytes, 0, songBytes.Length);

            }
            else
            {
                byte[] firstByte = new byte[1];
                await songByteStream.ReadAsync(firstByte, 0, 1);
                List<byte> bytes = new List<byte>(firstByte[0]);
                int b;
                while ((b = songByteStream.ReadByte()) != -1)
                {
                    bytes.Add((byte)b);
                }
                songBytes = bytes.ToArray();
            }

            return await PostNewSong(name, genre, audioType, songBytes);
        }

        public async Task<bool> RateSong(int songId, byte rating)
        {
            if (rating < 1 || 5 < rating) { throw new ArgumentException("Rating must be 1-5"); }

            var task = await httpClient.PostAsync($"{remoteAddress}rate?id={songId}", new ByteArrayContent(new[] { rating }));

            return task.IsSuccessStatusCode;
        }

        public async Task<bool> AddNewSubscription(int subscriptionId)
        {
            var task = await httpClient.PostAsync($"{remoteAddress}subscriptions?id={subscriptionId}", new ByteArrayContent(new byte[0]));

            return task.IsSuccessStatusCode;
        }

        public async Task<bool> PostComment(int songId, string comment)
        {
            byte[] commentBytes = Encoding.UTF8.GetBytes(comment);
            ByteArrayContent content = new ByteArrayContent(commentBytes);
            content.Headers.ContentLength = commentBytes.Length;
            content.Headers.ContentType = new MediaTypeHeaderValue("text/plain-text");
            var task = await httpClient.PostAsync($"{remoteAddress}comments?id={songId}", content);
            return task.IsSuccessStatusCode;
        }

        public async Task<bool> DeleteSong(int songId)
        {
            var task = await httpClient.DeleteAsync($"{remoteAddress}music?id={songId}");
            return task.IsSuccessStatusCode;
        }

        public async Task<bool> DeleteSubscription(int subscriptionId)
        {
            var task = await httpClient.DeleteAsync($"{remoteAddress}subscriptions?id={subscriptionId}");
            return task.IsSuccessStatusCode;
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
