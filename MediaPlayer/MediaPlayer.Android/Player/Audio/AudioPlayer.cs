using System;
using Xamarin.Forms;
using MediaPlayer;

[assembly: Dependency(typeof(AudioPlayer))]
namespace MediaPlayer
{
    /// <summary>
    /// Cross platform SimpleAudioPlayer implemenations
    /// </summary>
#if __ANDROID__
    [Android.Runtime.Preserve(AllMembers = true)]
#endif
    public class AudioPlayer: IAudioPlayerManager
    {
        static Lazy<IAudioPlayer> Implementation = new Lazy<IAudioPlayer>(() => CreateAudioPlayer(), System.Threading.LazyThreadSafetyMode.PublicationOnly);

        public static IAudioPlayer Current
        {
            get
            {
                var ret = Implementation.Value;
                if (ret == null)
                {
                    throw NotImplementedInReferenceAssembly();
                }
                return ret;
            }
        }

        public IAudioPlayer CurrentPlayer => Current;

        ///<Summary>
        /// Create a new AudioPlayer object
        ///</Summary>
        public static IAudioPlayer CreateAudioPlayer()
        {
#if NETSTANDARD1_0
            return null;
#else
            return new AudioPlayerImplementation();
#endif
        }

        internal static Exception NotImplementedInReferenceAssembly()
        {
            return new NotImplementedException("This functionality is not implemented in the .NET standard version of this assembly. Reference the NuGet package from your platform-specific (head) application project in order to reference the platform-specific implementation.");
        }
    }
}