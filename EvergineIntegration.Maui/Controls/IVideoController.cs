namespace EvergineIntegration.Maui.Controls
{
    public enum VideoStatus
    {
        NotReady,
        Playing,
        Paused
    }

    public interface IVideoController
    {
        VideoStatus Status { get; set; }
        TimeSpan Duration { get; set; }
    }
}
