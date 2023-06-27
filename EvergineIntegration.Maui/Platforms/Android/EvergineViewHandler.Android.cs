using EvergineIntegration.Maui.Controls;
using Microsoft.Maui.Handlers;
using AView = Android.Views.View;

namespace EvergineIntegration.Maui.Handlers
{
    public partial class EvergineViewHandler : ViewHandler<EvergineView, AView>
    {
        protected override AView CreatePlatformView() => throw new NotImplementedException();

        public static void MapApplication(EvergineViewHandler handler, EvergineView evergineView) => throw new NotImplementedException();
    }
}