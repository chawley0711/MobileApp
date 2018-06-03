using System;
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
using static Android.Media.AudioManager;
using static Android.Net.Wifi.WifiManager;

namespace AudiOcean.Droid.Services
{
    [Service]
    [IntentFilter(new[] { ActionPlay, ActionPause, ActionStop })]
    public class AndroidAudioService : Service, IOnAudioFocusChangeListener
    {
        private bool startedForgroundService = false;
        public const string ActionPlay = "net.audiocean.action.play";
        public const string ActionPause = "net.audiocean.action.pause";
        public const string ActionStop = "net.audiocean.action.stop";
        private const int FORGROUND_SERVICE_ID = 123545363;
        private const string WIFI_LOCK_TAG = "audiocean_wifi_lock";

        //Player goes here.
        object player;


        public override StartCommandResult OnStartCommand(Intent intent, StartCommandFlags flags, int startId)
        {
            switch (intent.Action)
            {
                case ActionPlay: Play(); break;
                case ActionStop: Stop(); break;
                case ActionPause: Pause(); break;
            }

            //Set sticky as we are a long running operation

            return StartCommandResult.Sticky;
        }

        private void Pause()
        {
           // player.Pause();
        }

        private void Stop()
        {
            StopForeground(true);
            startedForgroundService = false;
            ReleaseWifiLock();
          //  player.Release();
        }

        private void Play()
        {

            var pendingIntent = PendingIntent.GetActivity(ApplicationContext, 0,
                    new Intent(ApplicationContext, typeof(MainActivity)),
                    PendingIntentFlags.UpdateCurrent);
            //TODO Fill notification with song info.
            var notification = new Notification.Builder(Android.App.Application.Context)
                .SetVisibility(NotificationVisibility.Public)
                .SetCategory(Notification.CategoryService)
                .SetSmallIcon(Resource.Drawable.ic_audiotrack_light)
                .SetLargeIcon(BitmapFactory.DecodeResource(Resources, Resource.Drawable.ic_audiotrack_dark))
                .SetTicker("Song Playing")
                .SetContentIntent(pendingIntent).Build();

            notification.Flags |= NotificationFlags.OngoingEvent;

            if (!startedForgroundService)
            {
                StartForeground(FORGROUND_SERVICE_ID, notification);
            }

            InitilizePlayer();
            AquireWifiLock();

        }

        WifiManager wifiManager;
        WifiLock wifiLock;

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
                    if (player == null)
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

        private async void InitilizePlayer()
        {
            //Initilize Player here.
            wifiManager = WifiManager.FromContext(ApplicationContext);

        }
    }
}