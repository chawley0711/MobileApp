using Android.Media;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Reflection;
using Xamarin.Forms.Internals;
using Android.Content.Res;
using Xamarin.Forms;

namespace AudiOcean
{
    public class MyPlayer
    {
        Assembly _assembly;

        public void init()
        {
            _assembly = IntrospectionExtensions.GetTypeInfo(typeof(MyPlayer)).Assembly;
            myLoader l = new myLoader();
            byte[] x = l.GetEmbeddedResourceBytes(_assembly, "test.raw");

            PlayAudioTrack(x);
        }
        public void PlayAudioTrack(byte[] audioBuffer)
        {
            AudioTrack audioTrack = new AudioTrack(
              // Stream type
              Android.Media.Stream.Music,
              // Frequency
              11025,
              // Mono or stereo
              ChannelOut.Mono,
              // Audio encoding
              Android.Media.Encoding.Default,
              // Length of the audio clip.
              audioBuffer.Length,
              // Mode. Stream or static.
              AudioTrackMode.Stream);

            audioTrack.Play();

            audioTrack.Write(audioBuffer, 0, audioBuffer.Length);
            audioTrack.Flush();


        }
    }
}
