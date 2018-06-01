using Android.Media;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Reflection;
using Xamarin.Forms.Internals;
using Android.Content.Res;

namespace AudiOcean
{
    public class MyPlayer
    {
        Assembly _assembly;
        
        public void init()
        {
            myLoader l = new myLoader();
            byte[] x = l.GetEmbeddedResourceBytes(_assembly, "android/content/res/Resources/allen_arrogh.wav");
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
              Android.Media.Encoding.Pcm16bit,
              // Length of the audio clip.
              audioBuffer.Length,
              // Mode. Stream or static.
              AudioTrackMode.Stream);

            audioTrack.Play();
            audioTrack.Write(audioBuffer, 0, audioBuffer.Length);
        }
    }
}
