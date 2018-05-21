using Authentication;
using GoogleAuthentication;
using System;
using System.Collections.Generic;
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

        public AudiOceanHttpServer(string[] prefixes)
        {
            listener = new HttpListener();
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
                    //Post User Information uses token

                    //Post Comment ?songId commentBody

                    //Post Song ?name:genre byteBody

                    //Post Rating ?id ratingBody

                    //Post new subscription ?id

                    break;
                case "DELETE":
                    HandleDeleteRequest(context);
                    //Delete User information uses token

                    //Delete Subscription ?id

                    //Delete Song ?id must own resource

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

            throw new NotImplementedException();
        }

        private void HandleGetRequest(HttpListenerContext context)
        {
            //Get music stream ?id

            //Get User information ?id

            //Get SongsUploadedByUser ?id

            //Get most recent song uploads ?numOfSongs
            AuthenticateRequest(context);
            if (context.Response.StatusCode == (int)HttpStatusCode.Unauthorized) { return; }

            throw new NotImplementedException();
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


            throw new NotImplementedException();
        }

        private void HandleDeleteRequest(HttpListenerContext context)
        {
            throw new NotImplementedException();
        }

        public void StopServer()
        {
            listener.Stop();
        }
    }
}
