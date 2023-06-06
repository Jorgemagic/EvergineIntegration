using EvergineIntegration.Maui.Controls;
using EvergineIntegration.Maui.Handlers;

namespace EvergineIntegration.Maui.Extensions
{
    public static class AppBuilderExtensions
    {
        public static MauiAppBuilder UseMauiEvergine(this MauiAppBuilder builder)
        {
            builder.ConfigureMauiHandlers(h =>
            {
                h.AddHandler<EvergineView, EvergineViewHandler>();
            });

            return builder;
        }
    }
}
