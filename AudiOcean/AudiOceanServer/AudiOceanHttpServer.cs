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
using System.Threading.Tasks;

namespace AudiOceanServer
{
    public class AudiOceanHttpServer
    {
        HttpListener listener;
        private GoogleTokenValidator tokenValidator = new GoogleTokenValidator();
        private readonly string localAddress = "http://" + IPAddress.Parse("10.10.18.46").ToString() + "/";
        IAudiOceanServices audiOceanServices = new AudiOceanServices();

        public AudiOceanHttpServer()
        {
            listener = new HttpListener();

            string[] prefixes = new[]
            {
                localAddress + "music/",
                localAddress + "users/",
                localAddress + "list-music/",
                localAddress + "home/",
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
            //Get comments ?songID; numOfComments

            //Get music stream ?id

            //Get User information ?id

            //Get SongsUploadedByUser ?id

            //Get most recent song uploads ?numOfSongs
            AuthenticateRequest(context);
            if (context.Response.StatusCode == (int)HttpStatusCode.Unauthorized) { return; }

            if (context.Request.RawUrl == "music")
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

            if(context.Request.RawUrl == "users")
            {
                User user = audiOceanServices.GetUserByID(context.Request.QueryString["id"]);
                context.Response.ContentType = "application/json";
                context.Response.StatusCode = 200;
                context.Response.StatusDescription = "OK";
                byte[] jsonBytes = Encoding.UTF8.GetBytes($"{{" +
                    $"\"displayName\": \"{user.DisplayName}\"" +
                    $"\"profileURL\": \"{user.ProfilePictureURL}\"" +
                    $"}}");
                context.Response.ContentLength64 = jsonBytes.Length;
                context.Response.OutputStream.Write(jsonBytes, 0, jsonBytes.Length);
                context.Response.OutputStream.Close();
            }
            if(context.Request.RawUrl == "list-music")
            {
                var songs = audiOceanServices.GetSongsUploadedByUser(audiOceanServices.GetUserByID(context.Request.QueryString["id"]));

            }

            context.Response.Close();
        }

        private void AuthenticateRequest(HttpListenerContext context)
        {
            string authType = context.Request.Headers["Authorization"].Split(' ')[0];
            if (authType != "Bearer")
            {
                string token = context.Request.Headers["Authentication"].Split(' ')[1];
                tokenValidator.ValidateToken(new GoogleToken(token));
            }
            else
            {
                CreateResponseMessage((int)HttpStatusCode.Unauthorized, "Bearer Authentication was not specified.", context);
            }
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

        private void HandlePostRequest(HttpListenerContext context)
        {
            //Post User Information uses token user/

            //Post Comment ?songId commentBody

            //Post Song ?name; genre   - byteBody

            //Post Rating ?id ratingBody

            //Post new subscription ?id
            throw new NotImplementedException();
        }

        private void HandleDeleteRequest(HttpListenerContext context)
        {
            //Delete User information uses token

            //Delete Subscription ?id

            //Delete Song ?id must own resource
            throw new NotImplementedException();
        }

        public void StopServer()
        {
            listener.Stop();
            listener.Close();
        }
    }
}
