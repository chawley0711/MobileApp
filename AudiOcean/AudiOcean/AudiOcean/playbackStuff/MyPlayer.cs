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
        public System.IO.Stream stream { get; set; }
        public AudioTrack track { get; set; }

        public MyPlayer (System.IO.Stream s)
        {
            this.stream = s;
            byte[] x;
            using (var memoryStream = new MemoryStream())
            {
                s.CopyTo(memoryStream);
                x =  memoryStream.ToArray();
            }
            setTrack();
        }
        public void myPause()
        {
            this.track.Pause();
        }
        public void myStop()
        {
            this.track.Stop();
        }
        public void myRelease()
        {
            this.track.Release();
        }
        public void myWrite(byte[] bytes, int start, int size)
        {
            this.track.Write(bytes, start, size);
        }
        public void setTrack()
        {
            int mBufferSize = AudioTrack.GetMinBufferSize(SAMPLE_RATE, ChannelOut.Stereo, Android.Media.Encoding.Pcm16bit);
            if (mBufferSize == (int)TrackStatus.Error || mBufferSize == (int)TrackStatus.ErrorBadValue)
            {
                mBufferSize = SAMPLE_RATE * 2 * 2;
            }
            this.track = new AudioTrack(
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
        }
        public void Play(byte[] audioBuffer)
        {
            Java.Lang.Thread t = new Java.Lang.Thread(() =>
            {
                this.track.Play();
                bool mShouldContinue = true;
                for (int i = 0, step = 15000; i < audioBuffer.Length; i += step)
                {
                    if (i + step > audioBuffer.Length)
                    {
                        step = audioBuffer.Length - i;
                        mShouldContinue = false;
                    }
                    myWrite(audioBuffer, i, step);
                }

                if (!mShouldContinue)
                {
                    myRelease();
                }

            });
            t.Start();
        }
        
    }
}
