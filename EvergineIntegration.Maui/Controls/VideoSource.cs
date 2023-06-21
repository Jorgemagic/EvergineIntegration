using System.ComponentModel;

namespace EvergineIntegration.Maui.Controls
{
    [TypeConverter(typeof(VideoSourceConverter))]
    public abstract class VideoSource : Element
    {
        public static VideoSource FromResource(string path)
        {
            return new ResourceVideoSource { Path = path };
        }
    }
}
