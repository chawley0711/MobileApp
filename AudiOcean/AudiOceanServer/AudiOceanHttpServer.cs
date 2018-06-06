﻿using Authentication;
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
using System.Text.RegularExpressions;
using CSCore.MediaFoundation;
using CSCore;
using System.Threading;

namespace AudiOceanServer
{
    public class AudiOceanHttpServer
    {
        HttpListener listener;
        private GoogleTokenValidator tokenValidator = new GoogleTokenValidator();
        private readonly string localAddress = "http://" + IPAddress.Parse("192.168.0.100").ToString() + "/";
        IAudiOceanServices audiOceanServices = new AudiOceanServices();
        private bool serverStarted;
        private readonly string DIR_PATH_TO_MUSIC_FOLDER = @"C:\Users\Jeffrey\Desktop\AudiOceanMusicFolder";
        private readonly int SONG_BUFFER_SIZE = 44100;

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
            serverStarted = true;
            var t = new Thread(() =>
            {
                while (serverStarted)
                {
                    listener.GetContextAsync().ContinueWith((context) => HandleClientRequest(context.Result)); 
                }
            });
            t.Start();
        }
        public void StopServer()
        {
            serverStarted = false;
            listener.Stop();
            listener.Close();
        }




        private void HandleClientRequest(HttpListenerContext context)
        {
            var request = context.Request;

            if (QueryStringHasKey(context, "id"))
            {
                bool canParse = int.TryParse(context.Request.QueryString["id"], out int id);
                if (!canParse)
                {
                    CreateResponseMessage((int)HttpStatusCode.BadRequest, "The value for id in the query string is not an integer", context);
                    return;
                }
            }

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
                    break;
            }

        }




        private void HandleGetRequest(HttpListenerContext context)
        {
            GoogleToken token = AuthenticateRequest(context);
            if (context.Response.StatusCode == (int)HttpStatusCode.Unauthorized) { return; }

            var dir = context.Request.RawUrl.Split('?')[0].Replace("/", "");

            if (dir == "music")
            {
                GetSong(context);
            }
            else if (dir == "users")
            {
                GetUser(context, token);
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
            else if (dir == "rate")
            {
                GetAvgRating(context);
            }
        }

        private void HandleDeleteRequest(HttpListenerContext context)
        {
            GoogleToken token = AuthenticateRequest(context);
            if (context.Response.StatusCode == (int)HttpStatusCode.Unauthorized) { return; }

            string dir = context.Request.RawUrl.Split('?')[0].Replace("/", "");
            //Delete Subscription ?id
            if (dir == "subscriptions")
            {
                DeleteSubscription(context, token);
            }
            //Delete Song ?id must own resource
            if (dir == "music")
            {
                DeleteSong(context, token);
            }
        }

        private void HandlePostRequest(HttpListenerContext context)
        {
            //Post User Information uses token user/

            GoogleToken token = AuthenticateRequest(context);
            if (context.Response.StatusCode == (int)HttpStatusCode.Unauthorized) { return; }

            string dir = context.Request.RawUrl.Split('?')[0].Replace("/", "");

            if (dir == "users") { PostNewUser(context, token); }
            //Post Comment ?songId commentBody
            else if (dir == "comments") { PostNewComment(context, token); }
            //Post Song ?name; genre   - byteBody
            else if (dir == "music") { PostNewSong(context, token); }
            //Post Rating ?id ratingBody
            else if (dir == "rate") { RateSong(context, token); }
            //Post new subscription ?id
            else if (dir == "subscriptions") { AddNewSubscription(context, token); }

        }


        private void DeleteSong(HttpListenerContext context, GoogleToken token)
        {
            var queryStringKey = "id";


            if (!QueryStringHasKey(context, queryStringKey))
            {
                CreateResponseMessage((int)HttpStatusCode.BadRequest, "\"id\" is required in query string for song deletion.", context);
                return;
            }

            Song song = audiOceanServices.GetSong(int.Parse(context.Request.QueryString[queryStringKey]));
            if (song == null)
            {
                CreateResponseMessage((int)HttpStatusCode.NotFound, "Couldn't find the song with the specified id.", context);
                return;
            }

            User user = audiOceanServices.GetUserWithEmail(token.Payload.Email);
            if (user == null)
            {
                CreateResponseMessage((int)HttpStatusCode.Unauthorized, "User requesting delete does not have an account.", context);
                return;
            }

            if (user.ID != song.OwnerID)
            {
                CreateResponseMessage((int)HttpStatusCode.Forbidden, "Request not made by the owner of the resource to delete.", context);
                return;
            }

            audiOceanServices.DeleteSong(song);
            context.Response.StatusCode = 204;
            context.Response.StatusDescription = "Successfully Deleted Song";
        }

        private void DeleteSubscription(HttpListenerContext context, GoogleToken token)
        {
            if (!QueryStringHasKey(context, "id"))
            {
                CreateResponseMessage((int)HttpStatusCode.BadRequest, "The id field is required in the query string in order to complete request.", context);
            }

            User subscriber = audiOceanServices.GetUserWithEmail(token.Payload.Email);
            if (subscriber == null)
            {
                CreateResponseMessage((int)HttpStatusCode.Unauthorized, "The user who made the request does not have an account with us.", context);
                return;
            }

            User subscription = audiOceanServices.GetUser(int.Parse(context.Request.QueryString["id"]));
            if (subscription == null)
            {
                CreateResponseMessage((int)HttpStatusCode.NotFound, "Couldn't find the user with the specified id.", context);
                return;
            }

            audiOceanServices.DeleteSubscription(subscriber, subscription);
            context.Response.StatusCode = (int)HttpStatusCode.OK;
            context.Response.StatusDescription = "Successfully Deleted Subscription";
        }



        private void GetAvgRating(HttpListenerContext context)
        {
            if (!QueryStringHasKey(context, "id"))
            {
                CreateResponseMessage((int)HttpStatusCode.BadRequest, "Cannot provide an average rating if id is not specified.", context);
                return;
            }

            Song song = audiOceanServices.GetSong(int.Parse(context.Request.QueryString["id"]));

            if (song == null)
            {
                CreateResponseMessage((int)HttpStatusCode.NotFound, "Couldn't find a song with the specified id.", context);
                return;
            }

            byte avgRating = (byte)song.Ratings.Average((r) => (byte)r.Rating1);

            CreateResponseMessage(200, "OK", context, "text/plain-text", body: new[] { avgRating });
        }

        private void GetSubscriptions(HttpListenerContext context)
        {

            if (!QueryStringHasKey(context, "id"))
            {
                CreateResponseMessage((int)HttpStatusCode.BadRequest, "The request's query string must contain a integer value for id.", context);
                return;
            }

            var subscriptions = audiOceanServices.GetSubscriptionsForUser(audiOceanServices.GetUser(int.Parse(context.Request.QueryString["id"])));

            if (subscriptions.Count == 0)
            {
                CreateResponseMessage((int)HttpStatusCode.OK, "OK", context);
                return;
            }

            string json = "{ subscriptions: [";
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

            CreateResponseMessage(200, "OK", context, "application/json", body: jsonBytes);
        }

        private void GetComments(HttpListenerContext context)
        {
            if (!QueryStringHasKey(context, "id"))
            {
                CreateResponseMessage((int)HttpStatusCode.BadRequest, "Request's query string requires an integer to be mapped to \"id\"", context);
                return;
            }

            var song = audiOceanServices.GetSong(int.Parse(context.Request.QueryString["id"]));

            if (song == null)
            {
                CreateResponseMessage((int)HttpStatusCode.NotFound, "Song with the id provided could not be found.", context);
                return;
            }

            var comments = audiOceanServices.GetCommentsForSong(song);
            if (comments.Count == 0)
            {
                CreateResponseMessage((int)HttpStatusCode.OK, "OK", context, contentType: "application/json", body: Encoding.UTF8.GetBytes("{comments: []}"));
                return;
            }

            string json = "{ comments: [";
            var lastComment = comments.ElementAt(comments.Count - 1);
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

            CreateResponseMessage(200, "OK", context, "application/json", body: jsonBytes);
        }

        private void GetListOfMusic(HttpListenerContext context)
        {
            Console.WriteLine("Getting list of songs");
            ICollection<Song> songs = null;

            if (!QueryStringHasKey(context, "id") && !QueryStringHasKey(context, "numOfSongs"))
            {
                CreateResponseMessage((int)HttpStatusCode.BadRequest, "A request for music lists must contain either \"id\" or \"numOfSongs\" in query string.", context);
                return;
            }

            if (context.Request.QueryString["numOfSongs"] != null)
            {
                songs = audiOceanServices.GetMostRecentSongsUploads(int.Parse(context.Request.QueryString["numOfSongs"]));
            }
            else if (context.Request.QueryString["id"] != null)
            {
                songs = audiOceanServices.GetSongsUploadedByUser(audiOceanServices.GetUser(int.Parse(context.Request.QueryString["id"])));

            }
            string json = "{ songs: [";
            List<string> songArray = new List<string>();
            foreach (var song in songs)
            {
                var obj = new
                {
                    name = song.SongName,
                    rating = audiOceanServices.GetAverageRating(song),
                    id = song.ID,
                    ownerId = song.OwnerID,
                    genre = song.Genre?.Name ?? "",
                    dateUploaded = song.DateUploaded
                };
                songArray.Add(JsonConvert.SerializeObject(obj));
            }

            json += string.Join(",", songArray);

            json += "] }";

            Console.WriteLine(json);

            byte[] jsonbytes = Encoding.UTF8.GetBytes(json);

            CreateResponseMessage(200, "OK", context, "application/json", body: jsonbytes);

        }

        private void GetUser(HttpListenerContext context, GoogleToken token)
        {
            Console.WriteLine("Getting user");
            User user;
            if (!QueryStringHasKey(context, "id"))
            {
                user = audiOceanServices.GetUserWithEmail(token.Payload.Email);
            }
            else
            {
                user = audiOceanServices.GetUser(int.Parse(context.Request.QueryString["id"]));
            }
            if (user == null)
            {
                CreateResponseMessage((int)HttpStatusCode.NotFound, "Could not find the user with the specified id.", context);
                return;
            }

            Console.WriteLine(user.Email);
            string json = null;
            byte[] jsonBytes = Encoding.UTF8.GetBytes(json = JsonConvert.SerializeObject(new
            {
                displayName = user.DisplayName,
                profilePictureURL = user.ProfilePictureURL,
                id = user.ID
            }));

            Console.WriteLine(json);

            CreateResponseMessage((int)HttpStatusCode.OK, "OK", context, "application/json", body: jsonBytes);
        }

        private void GetSong(HttpListenerContext context)
        {
            Console.WriteLine("Getting song");
            if (!QueryStringHasKey(context, "id"))
            {
                Console.WriteLine("Bad Request no id");
                CreateResponseMessage((int)HttpStatusCode.BadRequest, "Query string must contain an \"id\" key in order to fulfill request.", context);
                return;
            }

            Song song = audiOceanServices.GetSongByID(int.Parse(context.Request.QueryString["id"]));
            if (song == null)
            {
                Console.WriteLine("Couldn't find song");
                CreateResponseMessage((int)HttpStatusCode.NotFound, "Could not find the requested song", context);
                return;
            }
            Console.WriteLine($"{song.SongName} retrieved");

            Console.WriteLine("Opened file");
            byte[] songBytes = File.ReadAllBytes(Path.Combine(DIR_PATH_TO_MUSIC_FOLDER, song.URL));
            Console.WriteLine($"Read {songBytes.Length} bytes");
            context.Response.SendChunked = true;
            context.Response.ContentLength64 = songBytes.Length;
            context.Response.ContentType = "audio/raw";
            context.Response.StatusCode = 200;
            context.Response.StatusDescription = "OK";

            Console.WriteLine("Sending...");
            for (int i = 0, step = SONG_BUFFER_SIZE; i < songBytes.Length; i += step)
            {

                if (i + step >= songBytes.Length)
                {
                    step = songBytes.Length - i;
                }
                context.Response.OutputStream.Write(songBytes, i, step);
            }
            Console.WriteLine("Done");
        }




        private void AddNewSubscription(HttpListenerContext context, GoogleToken token)
        {
            if (!QueryStringHasKey(context, "id"))
            {
                CreateResponseMessage((int)HttpStatusCode.BadRequest, "Query string must contain value for \"id\".", context);
                return;
            }

            User subscriber = audiOceanServices.GetUserWithEmail(token.Payload.Email);
            if (subscriber == null)
            {
                CreateResponseMessage((int)HttpStatusCode.Unauthorized, "User does not have an account and therefore cannot post a subscription.", context);
                return;
            }

            User subscription = audiOceanServices.GetUser(int.Parse(context.Request.QueryString["id"]));
            if (subscription == null)
            {
                CreateResponseMessage((int)HttpStatusCode.NotFound, "No user was found with the specified id.", context);
                return;
            }

            audiOceanServices.AddSubscription(subscriber, subscription);
            CreateResponseMessage((int)HttpStatusCode.OK, "Successfully Posted Subscription", context);
        }

        private void RateSong(HttpListenerContext context, GoogleToken token)
        {

            if (!QueryStringHasKey(context, "id"))
            {
                CreateResponseMessage((int)HttpStatusCode.BadRequest, "Must specify id in query string", context);
                return;
            }

            int rating = context.Request.InputStream.ReadByte();

            if (context.Request.ContentLength64 == 0)
            {
                CreateResponseMessage((int)HttpStatusCode.BadRequest, "Must send a single byte rating 1-5 in body to process request.", context);
                return;
            }

            if (1 <= rating && rating <= 5)
            {
                User user = audiOceanServices.GetUserWithEmail(token.Payload.Email);
                if (user == null)
                {
                    CreateResponseMessage((int)HttpStatusCode.Unauthorized, "User does not have an account", context);
                    return;
                }

                Song song = audiOceanServices.GetSong(int.Parse(context.Request.QueryString["id"]));
                if (song == null)
                {
                    CreateResponseMessage((int)HttpStatusCode.NotFound, "Couldn't find the song with the specified id", context);
                    return;
                }

                audiOceanServices.RateSong(user, song, rating);
                CreateResponseMessage((int)HttpStatusCode.OK, "Successfully rated song", context);
            }
        }

        private void PostNewSong(HttpListenerContext context, GoogleToken token)
        {
            if (!(QueryStringHasKey(context, "genre") && QueryStringHasKey(context, "name")))
            {
                CreateResponseMessage((int)HttpStatusCode.BadRequest, "Query string must specify values for genre and name", context);
                return;
            }

            string ext = Regex.Match(context.Request.ContentType, @"audio/([a-zA-Z)").Groups[0].Value;

            if (ext == "mpeg3") { ext = "mp3"; }
            else if (!(ext == "wav" || ext == "mp3"))
            {
                CreateResponseMessage((int)HttpStatusCode.UnsupportedMediaType, "Only mpeg3 and wav are supported for upload", context);
                return;
            }


            User songOwner = audiOceanServices.GetUserWithEmail(token.Payload.Email);
            if (songOwner == null)
            {
                CreateResponseMessage((int)HttpStatusCode.Unauthorized, "User posting song does not have an account", context);
                return;
            }

            string fileName = $"{Guid.NewGuid().ToString()}.raw";
            byte[] songBytes = new byte[context.Request.ContentLength64];
            context.Request.InputStream.Read(songBytes, 0, songBytes.Length);
            new MediaFoundationDecoder(new MFByteStream(new MemoryStream(songBytes), true)).WriteToFile(Path.Combine(DIR_PATH_TO_MUSIC_FOLDER, fileName));
            audiOceanServices.AddSong(songOwner,
                new Song()
                {
                    SongName = context.Request.QueryString["name"],
                    GenreID = audiOceanServices.GetGenreWithName(context.Request.QueryString["genre"]).ID,
                    URL = fileName
                });
        }

        private void PostNewComment(HttpListenerContext context, GoogleToken token)
        {
            if (!QueryStringHasKey(context, "id"))
            {
                CreateResponseMessage((int)HttpStatusCode.BadRequest, "Query string must contain value for id.", context);
                return;
            }

            User poster = audiOceanServices.GetUserWithEmail(token.Payload.Email);
            if (poster == null)
            {
                CreateResponseMessage((int)HttpStatusCode.Unauthorized, "User does not have an account", context);
                return;
            }

            var sr = new StreamReader(context.Request.InputStream);

            Song song = audiOceanServices.GetSong(int.Parse(context.Request.QueryString["id"]));
            if (song == null)
            {
                CreateResponseMessage((int)HttpStatusCode.NotFound, "Could not find the song with the specified id.", context);
                return;
            }

            audiOceanServices.AddComment(poster, song, sr.ReadToEnd());
            sr.Dispose();

            CreateResponseMessage((int)HttpStatusCode.OK, "Successfully posted comment.", context);
        }

        private void PostNewUser(HttpListenerContext context, GoogleToken token)
        {
            if (audiOceanServices.GetUserWithEmail(token.Payload.Email) != null)
            {
                CreateResponseMessage((int)HttpStatusCode.Conflict, "Already a user with that email registered.", context);
                return;
            }

            audiOceanServices.AddUser(new User()
            {
                Name = token.Payload.Name,
                Email = token.Payload.Email,
                DisplayName = token.Payload.GivenName,
                ProfilePictureURL = token.Payload.Picture,
            });

            CreateResponseMessage((int)HttpStatusCode.OK, "Successfully created new user account.", context);
        }

        public static string MakeJson(string name, string value)
        {
            return $"\"{name}\": \"{value}\"";
        }

        private static void CreateResponseMessage(int statusCode, string description, HttpListenerContext context, string contentType = "", IEnumerable<KeyValuePair<HttpResponseHeader, string>> headers = null, byte[] body = null)
        {
            context.Response.StatusCode = statusCode;
            context.Response.ProtocolVersion = new Version("1.1");
            if (headers != null)
                foreach (var header in headers)
                {
                    context.Response.Headers.Add(header.Key, header.Value);
                }
            context.Response.ContentType = contentType;
            context.Response.ContentLength64 = body == null ? 0 : body.Length;
            if (body != null)
                context.Response.OutputStream.Write(body, 0, body.Length);
            context.Response.KeepAlive = false;
            context.Response.StatusDescription = description;
            context.Response.OutputStream.Flush();
        }

        private static bool QueryStringHasKey(HttpListenerContext context, string queryStringKey)
        {
            return context.Request.QueryString.AllKeys.Contains(queryStringKey);
        }



        private GoogleToken AuthenticateRequest(HttpListenerContext context)
        {
            string authType = context.Request.Headers["Authentication"].Split(' ')[0];
            GoogleToken googToken = null;
            if (authType == "Bearer")
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

    }
}
