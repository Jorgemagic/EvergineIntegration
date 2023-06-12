using Evergine.Common.Graphics;
using Evergine.DirectX11;
using Evergine.Framework.Graphics;
using Evergine.Framework.Services;
using Evergine.WinUI;
using Microsoft.Maui.Handlers;
using Microsoft.UI.Xaml.Controls;
using System.Diagnostics;
using EvergineIntegration.Maui.Controls;

namespace EvergineIntegration.Maui.Handlers
{
    public partial class EvergineViewHandler : ViewHandler<EvergineView, SwapChainPanel>
    {
        bool _loaded;

        public EvergineViewHandler(IPropertyMapper mapper, CommandMapper commandMapper = null) 
            : base(mapper, commandMapper)
        {
        }

        protected override SwapChainPanel CreatePlatformView()
        {
            var swapChainPanel = new SwapChainPanel
            {
                IsHitTestVisible = true
            };

            return swapChainPanel;
        }

        protected override void ConnectHandler(SwapChainPanel platformView)
        {
            base.ConnectHandler(platformView);

            _loaded = false;

            platformView.Loaded += OnPlatformViewLoaded;
            platformView.PointerPressed += OnPlatformViewPointerPressed;
            platformView.PointerMoved += OnPlatformViewPointerMoved;
            platformView.PointerReleased += OnPlatformViewPointerReleased;
        }

        protected override void DisconnectHandler(SwapChainPanel platformView)
        {
            base.DisconnectHandler(platformView);

            platformView.Loaded -= OnPlatformViewLoaded;
            platformView.PointerPressed -= OnPlatformViewPointerPressed;
            platformView.PointerMoved -= OnPlatformViewPointerMoved;
            platformView.PointerReleased -= OnPlatformViewPointerReleased;
        }

        public static void MapApplication(EvergineViewHandler handler, EvergineView evergineView)
        {
            //if (!handler._loaded)
            //    return;            
            handler.UpdateApplication(handler.PlatformView, evergineView.Application, evergineView.DisplayName);
        }

        void OnPlatformViewLoaded(object sender, Microsoft.UI.Xaml.RoutedEventArgs e)
        {
            _loaded = true;
            UpdateValue(nameof(EvergineView.Application));
            //this.UpdateApplication(PlatformView, VirtualView.Application, VirtualView.DisplayName);
        }
        
        void OnPlatformViewPointerReleased(object sender, Microsoft.UI.Xaml.Input.PointerRoutedEventArgs e)
        {
            VirtualView.StartInteraction();            
        }

        void OnPlatformViewPointerMoved(object sender, Microsoft.UI.Xaml.Input.PointerRoutedEventArgs e)
        {
            VirtualView.MovedInteraction();
        }

        void OnPlatformViewPointerPressed(object sender, Microsoft.UI.Xaml.Input.PointerRoutedEventArgs e)
        {
            VirtualView.EndInteraction();
        }

        void UpdateApplication(SwapChainPanel swapChainPanel, Evergine.Framework.Application application, string displayName)
        {
            GraphicsContext graphicsContext = new DX11GraphicsContext();
            application.Container.RegisterInstance(graphicsContext);
            graphicsContext.CreateDevice(new ValidationLayer(ValidationLayer.NotifyMethod.Trace));

            // Create Services
            WinUIWindowsSystem windowsSystem = new WinUIWindowsSystem();
            application.Container.RegisterInstance(windowsSystem);

            var surface = (WinUISurface)windowsSystem.CreateSurface(swapChainPanel);

            ConfigureGraphicsContext(application, surface, displayName);
     
            // Creates XAudio device
            var xaudio = new Evergine.XAudio2.XAudioDevice();
            application.Container.RegisterInstance(xaudio);

            Stopwatch clockTimer = Stopwatch.StartNew();
            windowsSystem.Run(
            application.Initialize,
            () =>
            {
                var gameTime = clockTimer.Elapsed;
                clockTimer.Restart();

                application.UpdateFrame(gameTime);
                application.DrawFrame(gameTime);
            });
        }

        void ConfigureGraphicsContext(Evergine.Framework.Application application, WinUISurface surface, string displayName)
        {
            GraphicsContext graphicsContext = application.Container.Resolve<GraphicsContext>();

            SwapChainDescription swapChainDescription = new SwapChainDescription()
            {
                SurfaceInfo = surface.SurfaceInfo,
                Width = 200, // surface.Width,
                Height = 200, //surface.Height,
                ColorTargetFormat = PixelFormat.R8G8B8A8_UNorm,
                ColorTargetFlags = TextureFlags.RenderTarget | TextureFlags.ShaderResource,
                DepthStencilTargetFormat = PixelFormat.D24_UNorm_S8_UInt,
                DepthStencilTargetFlags = TextureFlags.DepthStencil,
                SampleCount = TextureSampleCount.None,
                IsWindowed = true,
                RefreshRate = 60
            };

            var swapChain = graphicsContext.CreateSwapChain(swapChainDescription);
            swapChain.VerticalSync = true;
            surface.NativeSurface.SwapChain = swapChain;

            var graphicsPresenter = application.Container.Resolve<GraphicsPresenter>();
            var firstDisplay = new Display(surface, swapChain);
            graphicsPresenter.AddDisplay(displayName, firstDisplay);
        }
    }
}