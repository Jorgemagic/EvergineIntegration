#if IOS || MACCATALYST
using PlatformView = VideoDemos.Platforms.MaciOS.MauiVideoPlayer;
#elif WINDOWS
using PlatformView = EvergineIntegration.Maui.Platforms.Windows.MauiVideoPlayer;
#elif (NETSTANDARD || !PLATFORM) || (NET6_0_OR_GREATER && !IOS && !ANDROID)
using PlatformView = System.Object;
#endif
using EvergineIntegration.Maui.Controls;
using Microsoft.Maui.Handlers;

namespace EvergineIntegration.Maui.Handlers
{
    public partial class VideoHandler
    {
        public static IPropertyMapper<Video, VideoHandler> PropertyMapper = new PropertyMapper<Video, VideoHandler>(ViewHandler.ViewMapper)
        {
            [nameof(Video.AreTransportControlsEnabled)] = MapAreTransportControlsEnabled,
            [nameof(Video.Source)] = MapSource,
            [nameof(Video.IsLooping)] = MapIsLooping,
        };

        public static CommandMapper<Video, VideoHandler> CommandMapper = new(ViewCommandMapper)
        {
            //[nameof(Video.UpdateStatus)] = MapUpdateStatus,
            //[nameof(Video.PlayRequested)] = MapPlayRequested,
            //[nameof(Video.PauseRequested)] = MapPauseRequested,
            //[nameof(Video.StopRequested)] = MapStopRequested
        };

        public VideoHandler() : base(PropertyMapper, CommandMapper)
        {
        }
    }
}
