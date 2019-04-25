
using MediaPlayer.Models;
using MediaPlayer.Permissions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace MediaPlayer.Views.MediaPlayers
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class AudioMediaPlayerPage : ContentPage
    {
        private readonly IAudioPlayerManager _audioPlayerManager;
        private readonly IDevicePermissionServices _devicePermissionServices;
        private int TrackIndex = -1;
        public AudioMediaPlayerPage()
        {
            try
            {
                InitializeComponent();
                _audioPlayerManager = DependencyService.Get<IAudioPlayerManager>();
                buttonPlay.Clicked += (object sender, EventArgs e) =>
                {
                    SetPlay();
                };

                _audioPlayerManager.CurrentPlayer.PlaybackEnded += (object sender, EventArgs e) =>
                {
                    SetPlay();
                };

                void SetPlay()
                {
                    if (Files?.Count == 0)
                        return;
                    TrackIndex++;
                    _audioPlayerManager.CurrentPlayer.Load(Files[TrackIndex].Path);
                    _audioPlayerManager.CurrentPlayer.Play();
                }
                _devicePermissionServices = DependencyService.Get<IDevicePermissionServices>();
                GetFiles();
            }
            catch (Exception ex)
            {
                var x = ex;
            }
        }

        private async void GetFiles()
        {
            if (!await _devicePermissionServices.RequestReadStoragePermission())
            {
                return;
            }
            Files = await DependencyService.Get<IDirectoryPath>().GetFiles();
            //Files.Add("https://storage.googleapis.com/wvmedia/clear/h264/tears/tears.mpd");
            //Files.Add("https://commondatastorage.googleapis.com/gtv-videos-bucket/CastVideos/hls/TearsOfSteel.m3u8");
            //Files.Add("https://html5demos.com/assets/dizzy.mp4");
            //Files.Add("https://storage.googleapis.com/exoplayer-test-media-1/ogg/play.ogg");
        }

        public List<FileDetail> Files { get; set; }
    }
}