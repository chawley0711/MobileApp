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
    public class AndroidAudiOceanPlayer
    {
        private readonly int SAMPLE_RATE = 44100;
        public System.IO.Stream Stream { get; private set; }
        public AudioTrack Track { get; private set; }
        public int Duration { get; private set; }

        private int SAMPLE_BUFFER_SIZE;

        public event Action<int> TrackDurationPositionNotifier;

        public AndroidAudiOceanPlayer(System.IO.Stream s)
        {
            this.Stream = s;
            SetTrack();
        }
        public void Pause()
        {
            this.Track.Pause();
        }
        public void Stop()
        {
            this.Track.Stop();
        }
        public void Release()
        {
            this.Track.Release();
        }
        public void Write(byte[] bytes, int start, int size)
        {
            this.Track.Write(bytes, start, size);
        }
        public void SetTrack()
        {
            SAMPLE_BUFFER_SIZE = AudioTrack.GetMinBufferSize(SAMPLE_RATE, ChannelOut.Stereo, Android.Media.Encoding.Pcm16bit);
            if (SAMPLE_BUFFER_SIZE == (int)TrackStatus.Error || SAMPLE_BUFFER_SIZE == (int)TrackStatus.ErrorBadValue)
            {
                SAMPLE_BUFFER_SIZE = SAMPLE_RATE * 2 * 2;
            }
            this.Track = new AudioTrack(
                  // Stream type
                  Android.Media.Stream.Music,
                  // Frequency
                  SAMPLE_RATE,
                  // Mono or stereo
                  ChannelOut.Stereo,
                  // Audio encoding
                  Android.Media.Encoding.Pcm16bit,
                  // Length of the audio clip.
                  SAMPLE_BUFFER_SIZE,
                  // Mode. Stream or static.
                  AudioTrackMode.Stream);

            Track.SetPositionNotificationPeriod(SAMPLE_RATE);
            Duration =  (int)(Stream.Length / SAMPLE_BUFFER_SIZE);
            Track.PeriodicNotification += Track_PeriodicNotification;
        }

        private void Track_PeriodicNotification(object sender, AudioTrack.PeriodicNotificationEventArgs e)
        {
            TrackDurationPositionNotifier?.Invoke(e.Track.PlaybackHeadPosition / SAMPLE_RATE);
        }

        public void BeginWriting()
        {
            Java.Lang.Thread t = new Java.Lang.Thread(() =>
            {
                var audioBuffer = new byte[SAMPLE_BUFFER_SIZE];
                for (int i = 0, step = SAMPLE_BUFFER_SIZE; i < Stream.Length; i += step)
                {
                    if (i + step > Stream.Length)
                    {
                        step = (int)(Stream.Length - i);
                    }
                    Stream.Write(audioBuffer, 0, step);
                    Write(audioBuffer, 0, step);
                }
            });
            t.Start();
        }

        public void Play()
        {
            this.Track.Play();
        }

    }
}
