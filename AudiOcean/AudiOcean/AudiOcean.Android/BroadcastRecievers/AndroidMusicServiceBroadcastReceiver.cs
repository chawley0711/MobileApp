using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using AudiOcean.Static_Helper_Classes;

namespace AudiOcean.Droid.BroadcastRecievers
{
    [BroadcastReceiver]
    [IntentFilter(new[] { PLAY_BROADCAST, PAUSE_BROADCAST, STOP_BROADCAST })]
    public class AndroidMusicServiceBroadcastReceiver : BroadcastReceiver
    {
        public const string PLAY_BROADCAST = "net.audiocean.broadcast.play";
        public const string PAUSE_BROADCAST = "net.audiocean.broadcast.pause";
        public const string STOP_BROADCAST = "net.audiocean.broadcast.stop";

        public override void OnReceive(Context context, Intent intent)
        {
            switch (intent.Action)
            {
                case PLAY_BROADCAST: MusicServiceHelper.Helper.Play(); break;
                case PAUSE_BROADCAST: MusicServiceHelper.Helper.Pause(); break;
                case STOP_BROADCAST: MusicServiceHelper.Helper.Release(); break;
            }
        }
    }
}