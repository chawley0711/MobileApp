using Android.Media;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Reflection;
using Xamarin.Forms.Internals;
using Android.Content.Res;
using Xamarin.Forms;
using Java.Nio;

namespace AudiOcean
{
    public class MyPlayer
    {
        Assembly _assembly;
        private readonly int SAMPLE_RATE = 44100;
        byte[] mSamples; // the samples to play
        int mNumSamples;
        
        public void init()
        {
            _assembly = IntrospectionExtensions.GetTypeInfo(typeof(MyPlayer)).Assembly;
            myLoader l = new myLoader();
            byte[] x = l.GetEmbeddedResourceBytes(_assembly, "Retrograde-_Mastered_.wav");

            PlayAudioTrack(x);
        }
        public void PlayAudioTrack(byte[] audioBuffer)
        {
            Java.Lang.Thread t = new Java.Lang.Thread(() =>
            {

                int mBufferSize = AudioTrack.GetMinBufferSize(SAMPLE_RATE, ChannelOut.Stereo,
                Android.Media.Encoding .Pcm16bit);
                if (mBufferSize == (int)TrackStatus.Error || mBufferSize == (int)TrackStatus.ErrorBadValue)
                {
                    // For some readon we couldn't obtain a buffer size
                    mBufferSize = SAMPLE_RATE * 2 * 2;
                }
                AudioTrack audioTrack = new AudioTrack(
                  // Stream type
                  Android.Media.Stream.Music,
                  // Frequency
                  SAMPLE_RATE,
                  // Mono or stereo
                  ChannelOut.Stereo,
                  // Audio encoding
                  Android.Media.Encoding.Pcm16bit,
                  // Length of the audio clip.
                  mBufferSize,
                  // Mode. Stream or static.
                  AudioTrackMode.Stream);

                audioTrack.Play();
                bool mShouldContinue = true;
                for (int i = 0, step = 15000; i < audioBuffer.Length; i += step)
                {
                    if (i + step > audioBuffer.Length)
                    {
                        step = audioBuffer.Length - i;
                        mShouldContinue = false;
                    }
                    audioTrack.Write(audioBuffer, i, step);
                }

                if (!mShouldContinue)
                {
                    audioTrack.Release();
                }

            });
            t.Start();


        }
    }
}
