﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.Graphics;
using Android.Media;
using Android.Net;
using Android.Net.Wifi;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using AudiOcean.Droid.BroadcastRecievers;
using static Android.Media.AudioManager;
using static Android.Net.Wifi.WifiManager;

namespace AudiOcean.Droid.Services
{
    [Service]
    [IntentFilter(new[] { ActionPlay, ActionPause, ActionStop })]
    public class AndroidAudioService : Service, IOnAudioFocusChangeListener
    {
        private bool canPlay = false;
        private bool startedForgroundService = false;
        public const string ActionPlay = "net.audiocean.action.play";
        public const string ActionPause = "net.audiocean.action.pause";
        public const string ActionStop = "net.audiocean.action.stop";
        public const string ActionSetSource = "net.audiocean.action.set_source";
        private const int FORGROUND_SERVICE_ID = 123545363;
        private const string WIFI_LOCK_TAG = "audiocean_wifi_lock";
        private WifiManager wifiManager;
        private WifiLock wifiLock;

        //Player goes here.
        private AndroidAudiOceanPlayer Player { get; set; }




        public override StartCommandResult OnStartCommand(Intent intent, StartCommandFlags flags, int startId)
        {
            switch (intent.Action)
            {
                case ActionPlay: Play(); break;
                case ActionStop: Stop(); break;
                case ActionPause: Pause(); break;
                case ActionSetSource: SetSource(intent.GetIntExtra("id", 0)); break;
            }

            //Set sticky as we are a long running operation

            return StartCommandResult.Sticky;
        }

        private void SetSource(int songId)
        {
            var musicStream = App.HttpClient.GetMusic(songId);
            musicStream.ContinueWith((t) =>
            {
                Player = new AndroidAudiOceanPlayer(t.Result);
                Player.SetTrack();
                canPlay = true;
            });
        }

        private void Pause()
        {
            var playIntent = new Intent(this, typeof(AndroidMusicServiceBroadcastReceiver));
            playIntent.SetAction(AndroidMusicServiceBroadcastReceiver.PLAY_BROADCAST);
            //Resource.Drawable.PauseButton, "Pause"
            UpdateNotification(playIntent, Resource.Drawable.PlayButton, "Play");
            Player.Pause();
            // player.Pause();
        }

        private void Stop()
        {
            StopForeground(true);
            startedForgroundService = false;
            ReleaseWifiLock();
            Player.Release();
            //  player.Release();
        }

        private void Play()
        {
            var pauseIntent = new Intent(this, typeof(AndroidMusicServiceBroadcastReceiver));
            pauseIntent.SetAction(AndroidMusicServiceBroadcastReceiver.PAUSE_BROADCAST);
            //Resource.Drawable.PauseButton, "Pause"
            UpdateNotification(pauseIntent, Resource.Drawable.PauseButton, "Pause");

            InitilizePlayer();
            AquireWifiLock();
            Player.Play();
        }

        private void UpdateNotification(Intent extraAction, int icon, string title)
        {
            if (startedForgroundService) { startedForgroundService = false; }
            var pendingIntent = PendingIntent.GetActivity(ApplicationContext, 0,
                    new Intent(ApplicationContext, typeof(MainActivity)),
                    PendingIntentFlags.UpdateCurrent);
            var pendingIntentExtraAction = PendingIntent.GetBroadcast(this, 0, extraAction, PendingIntentFlags.UpdateCurrent);

            //TODO Fill notification with song info.
            var notification = new Notification.Builder(Android.App.Application.Context)
                .SetVisibility(NotificationVisibility.Public)
                .SetCategory(Notification.CategoryService)
                .SetSmallIcon(Resource.Drawable.ic_audiotrack_light)
                .SetLargeIcon(BitmapFactory.DecodeResource(Resources, Resource.Drawable.ic_audiotrack_dark))
                .SetTicker("Song Playing")
                .SetContentIntent(pendingIntent)
                .AddAction(new Notification.Action(icon, title, pendingIntentExtraAction))
                .Build();

            notification.Flags |= NotificationFlags.OngoingEvent;

            if (!startedForgroundService)
            {
                StartForeground(FORGROUND_SERVICE_ID, notification);
            }
        }


        private void AquireWifiLock()
        {
            if (wifiLock == null)
            {
                wifiLock = wifiManager.CreateWifiLock(WifiMode.Full, WIFI_LOCK_TAG);
            }
            wifiLock.Acquire();
        }

        private void ReleaseWifiLock()
        {
            if (wifiLock == null)
                return;
            wifiLock.Release();
            wifiLock = null;
        }

        public override IBinder OnBind(Intent intent)
        {
            throw new NotImplementedException();
        }

        public void OnAudioFocusChange(AudioFocus focusChange)
        {
            switch (focusChange)
            {
                case AudioFocus.Gain:
                    if (Player == null)
                        InitilizePlayer();

                    //Turn it up!
                    break;
                case AudioFocus.Loss:
                    //We have lost focus stop!
                    Stop();
                    break;
                case AudioFocus.LossTransient:
                    //We have lost focus for a short time, but likely to resume so pause
                    Pause();
                    break;
                case AudioFocus.LossTransientCanDuck:
                    //We have lost focus but should still play at a lower 10% volume
                    //turn it down!
                    break;
            }
        }

        private void InitilizePlayer()
        {
            //Initilize Player here.
            wifiManager = WifiManager.FromContext(ApplicationContext);

        }
    }
}