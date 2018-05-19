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
                    //Get music stream ?id

                    //Get User information ?id

                    //Get SongsUploadedByUser ?id

                    //Get most recent song uploads ?numOfSongs

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
                    break;
            }


          


            throw new NotImplementedException();
        }

        private void HandleGetRequest(HttpListenerContext context)
        {

            throw new NotImplementedException();
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
