using Authentication;
using Carbon.Json.Parser;
using DatabaseInterface;
using DatabaseInterface.Services;
using GoogleAuthentication;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using Newtonsoft.Json;
using System.Threading.Tasks;

namespace AudiOceanServer
{
    public class AudiOceanHttpServer
    {
        HttpListener listener;
        private GoogleTokenValidator tokenValidator = new GoogleTokenValidator();
        private readonly string localAddress = "http://" + IPAddress.Parse("10.10.18.46").ToString() + "/";
        IAudiOceanServices audiOceanServices = new AudiOceanServices();
        string DIR_PATH_TO_MUSIC_FOLDER;

        public AudiOceanHttpServer()
        {
            listener = new HttpListener();

            string[] prefixes = new[]
            {
                localAddress + "music/",
                localAddress + "users/",
                localAddress + "list-music/",
                localAddress + "comments/",
                localAddress + "rate/",
                localAddress + "subscriptions/"
            };

            foreach (var prefix in prefixes)
            {
                listener.Prefixes.Add(prefix);
            }
        }

        public void StartServer()
        {
            listener.Start();
            listener.GetContextAsync().ContinueWith((context) => HandleClientRequest(context.Result)).Start();
        }




        private void HandleClientRequest(HttpListenerContext context)
        {
            var request = context.Request;

            switch (request.HttpMethod)
            {
                case "GET":
                    HandleGetRequest(context);
                    break;
                case "POST":
                    HandlePostRequest(context);
                    break;
                case "DELETE":
                    HandleDeleteRequest(context);
                    break;
                default:
                    context.Response.StatusCode = 405;
                    context.Response.ProtocolVersion = new Version("1.1");
                    context.Response.ContentLength64 = 0;
                    context.Response.KeepAlive = false;
                    context.Response.StatusDescription = "Method type cannot be performed.";
                    context.Response.OutputStream.Flush();
                    context.Response.Close();
                    break;
            }
        }

        private void HandleGetRequest(HttpListenerContext context)
        {
            AuthenticateRequest(context);
            if (context.Response.StatusCode == (int)HttpStatusCode.Unauthorized) { return; }

            var dir = context.Request.RawUrl.Split('?')[0].Replace("/", "");

            if (dir == "music")
            {
                GetSong(context);
            }
            else if (dir == "users")
            {
                GetUser(context);
            }
            else if (dir == "list-music")
            {
                //Need to do the numOfSongs bit
                GetListOfMusic(context);
            }
            else if (dir == "comments")
            {
                GetComments(context);
            }
            else if (dir == "subscriptions")
            {
                GetSubscriptions(context);
            }

            context.Response.Close();
        }

        private void HandlePostRequest(HttpListenerContext context)
        {
            //Post User Information uses token user/

            GoogleToken token = AuthenticateRequest(context);
            if (context.Response.StatusCode == (int)HttpStatusCode.Unauthorized) { return; }

            string dir = context.Request.RawUrl.Split('?')[0].Replace("/", "");

            if (dir == "user") { PostNewUser(context, token); }
            //Post Comment ?songId commentBody
            else if (dir == "comments") { PostNewComment(context, token); }
            //Post Song ?name; genre   - byteBody
            else if (dir == "music") { PostNewSong(context, token); }
            //Post Rating ?id ratingBody
            else if (dir == "rate") { RateSong(context, token); }
            //Post new subscription ?id
            else if (dir == "subscriptions") { AddNewSubscription(context, token); }

            context.Response.Close();
        }

        private void HandleDeleteRequest(HttpListenerContext context)
        {
            GoogleToken token = AuthenticateRequest(context);
            if (context.Response.StatusCode == (int)HttpStatusCode.Unauthorized) { return; }

            string dir = context.Request.RawUrl.Split('?')[0].Replace("/", "");
            //Delete Subscription ?id
            if (dir == "subscriptions")
            {
                User subscriber = audiOceanServices.GetUserWithEmail(token.Payload.Email);
                User subscription = audiOceanServices.GetUser(int.Parse(context.Request.QueryString["id"]));
                audiOceanServices.DeleteSubscription(subscriber, subscription);
                context.Response.StatusCode = (int)HttpStatusCode.NoContent;
                context.Response.StatusDescription = "Successfully Deleted Subscription";
            }
            //Delete Song ?id must own resource
            if (dir == "music")
            {
                Song song = audiOceanServices.GetSong(int.Parse(context.Request.QueryString["id"]));
                if (audiOceanServices.GetUserWithEmail(token.Payload.Email).ID == song.OwnerID)
                {
                    audiOceanServices.DeleteSong(song);
                    context.Response.StatusCode = 204;
                    context.Response.StatusDescription = "Successfully Deleted Song";
                }
                else
                {
                    context.Response.StatusCode = (int)HttpStatusCode.Forbidden;
                    context.Response.StatusDescription = "Request was not made by the owner of the resource to delete.";
                }
            }

            context.Response.Close();
        }




        private void GetSubscriptions(HttpListenerContext context)
        {
            var subscriptions = audiOceanServices.GetSubscriptionsForUser(audiOceanServices.GetUser(int.Parse(context.Request.QueryString["id"])));
            string json = "{ [";
            var lastSub = subscriptions.ElementAt(subscriptions.Count - 1);
            foreach (var sub in subscriptions)
            {
                json += JsonConvert.SerializeObject(new
                {
                    id = sub.ID,
                    displayName = sub.DisplayName,
                    profileURL = sub.ProfilePictureURL
                });

                if (sub != lastSub)
                {
                    json += ",";
                }
            }
            json += "] }";

            byte[] jsonBytes = Encoding.UTF8.GetBytes(json);
            context.Response.ContentLength64 = jsonBytes.Length;
            context.Response.ContentType = "application/json";
            context.Response.StatusCode = 200;
            context.Response.StatusDescription = "OK";
            context.Response.OutputStream.Write(jsonBytes, 0, jsonBytes.Length);
            context.Response.OutputStream.Close();
        }

        private void GetComments(HttpListenerContext context)
        {
            var comments = audiOceanServices.GetCommentsForSong(audiOceanServices.GetSong(int.Parse(context.Request.QueryString["id"])));
            string json = "{ [";
            var lastComment = comments.ElementAt(comments.Count);
            foreach (var comment in comments)
            {
                json += JsonConvert.SerializeObject(new
                {
                    id = comment.CommentID,
                    datePosted = comment.DatePosted,
                    text = comment.Text,
                    userId = comment.UserID,
                    ownerName = comment.User.DisplayName
                });
                if (comment != lastComment)
                {
                    json += ",";
                }
            }
            json += "] }";

            byte[] jsonBytes = Encoding.UTF8.GetBytes(json);
            context.Response.ContentLength64 = jsonBytes.Length;
            context.Response.ContentType = "application/json";
            context.Response.StatusCode = 200;
            context.Response.StatusDescription = "OK";
            context.Response.OutputStream.Write(jsonBytes, 0, jsonBytes.Length);
            context.Response.OutputStream.Close();
        }

        private void GetListOfMusic(HttpListenerContext context)
        {
            ICollection<Song> songs = null;
            if (context.Request.QueryString["numOfSongs"] != null)
            {
                songs = audiOceanServices.GetMostRecentSongsUploads(int.Parse(context.Request.QueryString["numOfSongs"]));
            }
            else if(context.Request.QueryString["id"] != null)
            {
                songs = audiOceanServices.GetSongsUploadedByUser(audiOceanServices.GetUser(int.Parse(context.Request.QueryString["id"])));
            }
            context.Response.ContentType = "application/json";
            context.Response.StatusCode = 200;
            context.Response.StatusDescription = "OK";
            string json = "{ songs: [";
            foreach (var song in songs)
            {
                json += JsonConvert.SerializeObject(SongToJSong(song));
            }

            json += "] }";

            byte[] jsonbytes = Encoding.UTF8.GetBytes(json);
            context.Response.ContentLength64 = jsonbytes.Length;
            context.Response.OutputStream.Write(jsonbytes, 0, jsonbytes.Length);
            context.Response.OutputStream.Close();
        }

        private void GetUser(HttpListenerContext context)
        {
            User user = audiOceanServices.GetUser(int.Parse(context.Request.QueryString["id"]));
            context.Response.ContentType = "application/json";
            context.Response.StatusCode = 200;
            context.Response.StatusDescription = "OK";
            byte[] jsonBytes = Encoding.UTF8.GetBytes($"{{" +
                $"\"displayName\": \"{user.DisplayName}\" " +
                $"\"profileURL\": \"{user.ProfilePictureURL}\" " +
                $"}}");
            context.Response.ContentLength64 = jsonBytes.Length;
            context.Response.OutputStream.Write(jsonBytes, 0, jsonBytes.Length);
            context.Response.OutputStream.Close();
        }

        private void GetSong(HttpListenerContext context)
        {
            Song song = audiOceanServices.GetSongByID(int.Parse(context.Request.QueryString["id"]));
            byte[] songBytes = File.ReadAllBytes(song.URL);
            context.Response.ContentLength64 = songBytes.Length;
            context.Response.ContentType = "audio/mpeg3";
            context.Response.StatusCode = 200;
            context.Response.StatusDescription = "OK";
            //Send Chunked later
            context.Response.OutputStream.Write(songBytes, 0, songBytes.Length);
            context.Response.OutputStream.Close();
        }

        private void AddNewSubscription(HttpListenerContext context, GoogleToken token)
        {
            User subscriber = audiOceanServices.GetUserWithEmail(token.Payload.Email);
            User subscription = audiOceanServices.GetUser(int.Parse(context.Request.QueryString["id"]));
            audiOceanServices.AddSubscription(subscriber, subscription);
            context.Response.StatusCode = 204;
            context.Response.StatusDescription = "Successfully Posted Subscription";
        }

        private void RateSong(HttpListenerContext context, GoogleToken token)
        {
            int rating = context.Request.InputStream.ReadByte();
            if (1 <= rating && rating <= 5)
            {
                User user = audiOceanServices.GetUserWithEmail(token.Payload.Email);
                Song song = audiOceanServices.GetSong(int.Parse(context.Request.QueryString["id"]));
                audiOceanServices.RateSong(user, song, rating);
                context.Response.StatusCode = 204;
                context.Response.StatusDescription = "Successfully Rated Song";
            }
        }

        private void PostNewSong(HttpListenerContext context, GoogleToken token)
        {
            string fileName = Guid.NewGuid().ToString() + ".mp3";
            byte[] songBytes = new byte[context.Request.ContentLength64];
            context.Request.InputStream.Read(songBytes, 0, songBytes.Length);
            File.WriteAllBytes(Path.Combine(DIR_PATH_TO_MUSIC_FOLDER, fileName), songBytes);
            User songOwner = audiOceanServices.GetUserWithEmail(token.Payload.Email);
            audiOceanServices.AddSong(songOwner,
                new Song()
                {
                    SongName = context.Request.QueryString["name"],
                    Genre = new Genre() { Name = context.Request.QueryString["genre"] },
                    URL = fileName
                });
        }

        private void PostNewComment(HttpListenerContext context, GoogleToken token)
        {
            var sr = new StreamReader(context.Request.InputStream);
            User poster = audiOceanServices.GetUserWithEmail(token.Payload.Email);
            Song song = audiOceanServices.GetSong(int.Parse(context.Request.QueryString["id"]));
            audiOceanServices.AddComment(poster, song, sr.ReadToEnd());
            sr.Close();

            context.Response.StatusCode = 204;
            context.Response.StatusDescription = "Successfully posted comment.";
            context.Response.OutputStream.Close();
        }

        private void PostNewUser(HttpListenerContext context, GoogleToken token)
        {
            audiOceanServices.AddUser(new User()
            {
                Name = token.Payload.Name,
                Email = token.Payload.Email,
                DisplayName = token.Payload.GivenName,
                ProfilePictureURL = token.Payload.Picture,
            });

            context.Response.StatusCode = 204;
            context.Response.StatusDescription = "Successfully created new user account.";
            context.Response.OutputStream.Close();
        }

        public object SongToJSong(Song song)
        {
            var jsong = new
            {
                type = "Song",
                name = song.SongName,
                rating = song.Ratings.Average((r) => r.Rating1),
                id = song.ID,
                ownerId = song.OwnerID,
                genre = song.Genre.Name,
                dateUploaded = song.DateUploaded
            };

            return jsong;
        }

        public static string MakeJson(string name, string value)
        {
            return $"\"{name}\": \"{value}\"";
        }

        private GoogleToken AuthenticateRequest(HttpListenerContext context)
        {
            string authType = context.Request.Headers["Authorization"].Split(' ')[0];
            GoogleToken googToken = null;
            if (authType != "Bearer")
            {
                string token = context.Request.Headers["Authentication"].Split(' ')[1];
                tokenValidator.ValidateToken(googToken = new GoogleToken(token));
            }
            else
            {
                CreateResponseMessage((int)HttpStatusCode.Unauthorized, "Bearer Authentication was not specified.", context);
            }
            return googToken;
        }

        private static void CreateResponseMessage(int statusCode, string description, HttpListenerContext context, IEnumerable<KeyValuePair<HttpResponseHeader, string>> headers = null, byte[] body = null)
        {
            context.Response.StatusCode = statusCode;
            context.Response.ProtocolVersion = new Version("1.1");
            foreach (var header in headers)
            {
                context.Response.Headers.Add(header.Key, header.Value);
            }
            context.Response.ContentLength64 = body == null ? 0 : body.Length;
            context.Response.OutputStream.Write(body, 0, body.Length);
            context.Response.KeepAlive = false;
            context.Response.StatusDescription = description;
            context.Response.OutputStream.Flush();
            context.Response.Close();
        }


        public void StopServer()
        {
            listener.Stop();
            listener.Close();
        }
    }
}
