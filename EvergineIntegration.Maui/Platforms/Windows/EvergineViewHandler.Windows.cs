using Evergine.Common.Graphics;
using Evergine.DirectX11;
using Evergine.Framework.Graphics;
using Evergine.Framework.Services;
using Evergine.WinUI;
using EvergineIntegration.Maui.Controls;
using Microsoft.Maui.Handlers;
using Microsoft.UI.Xaml.Controls;
using System.Diagnostics;
using WGrid = Microsoft.UI.Xaml.Controls.Grid;

namespace EvergineIntegration.Maui.Handlers
{
    public partial class EvergineViewHandler : ViewHandler<EvergineView, WGrid>
    {
        bool _loaded;
        SwapChainPanel _swapChainPanel;

        public EvergineViewHandler(IPropertyMapper mapper, CommandMapper commandMapper = null) 
            : base(mapper, commandMapper)
        {
        }

        protected override WGrid CreatePlatformView()
        {
            var platformView = new WGrid();

            _swapChainPanel = new SwapChainPanel
            {
                IsHitTestVisible = true
            };

            platformView.Children.Add(_swapChainPanel);

            return platformView;
        }

        protected override void ConnectHandler(WGrid platformView)
        {
            base.ConnectHandler(platformView);

            _loaded = false;

            platformView.Loaded += OnPlatformViewLoaded;

            _swapChainPanel.PointerPressed += OnPlatformViewPointerPressed;
            _swapChainPanel.PointerMoved += OnPlatformViewPointerMoved;
            _swapChainPanel.PointerReleased += OnPlatformViewPointerReleased;
        }

        protected override void DisconnectHandler(WGrid platformView)
        {
            base.DisconnectHandler(platformView);

            platformView.Loaded -= OnPlatformViewLoaded;

            _swapChainPanel.PointerPressed -= OnPlatformViewPointerPressed;
            _swapChainPanel.PointerMoved -= OnPlatformViewPointerMoved;
            _swapChainPanel.PointerReleased -= OnPlatformViewPointerReleased;

            _swapChainPanel = null;
        }

        public static void MapApplication(EvergineViewHandler handler, EvergineView evergineView)
        {
            if (!handler._loaded)
                return;

            handler.UpdateApplication(handler._swapChainPanel, evergineView, evergineView.DisplayName);
        }

        void OnPlatformViewLoaded(object sender, Microsoft.UI.Xaml.RoutedEventArgs e)
        {
            _loaded = true;
            UpdateValue(nameof(EvergineView.Application));
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

        void UpdateApplication(SwapChainPanel swapChainPanel, EvergineView view, string displayName)
        {
            if (view.Application is null)
                return;

            GraphicsContext graphicsContext = new DX11GraphicsContext();
            view.Application.Container.RegisterInstance(graphicsContext);
            graphicsContext.CreateDevice(new ValidationLayer(ValidationLayer.NotifyMethod.Trace));

            // Create Services
            WinUIWindowsSystem windowsSystem = new WinUIWindowsSystem();
            view.Application.Container.RegisterInstance(windowsSystem);

            var surface = (WinUISurface)windowsSystem.CreateSurface(swapChainPanel);

            ConfigureGraphicsContext(view.Application, surface, displayName);

            // Creates XAudio device
            var xaudio = new Evergine.XAudio2.XAudioDevice();
            view.Application.Container.RegisterInstance(xaudio);

            Stopwatch clockTimer = Stopwatch.StartNew();
            windowsSystem.Run(
            view.Application.Initialize,
            () =>
            {
                var gameTime = clockTimer.Elapsed;
                clockTimer.Restart();

                view.Application.UpdateFrame(gameTime);
                view.Application.DrawFrame(gameTime);

            });
        }

        void ConfigureGraphicsContext(Evergine.Framework.Application application, WinUISurface surface, string displayName)
        {
            GraphicsContext graphicsContext = application.Container.Resolve<GraphicsContext>();

            SwapChainDescription swapChainDescription = new SwapChainDescription()
            {
                SurfaceInfo = surface.SurfaceInfo,
                Width = surface.Width,
                Height = surface.Height,
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
            swapChain.FrameBuffer.IntermediateBufferAssociated = false;
            surface.NativeSurface.SwapChain = swapChain;

            var graphicsPresenter = application.Container.Resolve<GraphicsPresenter>();
            var firstDisplay = new Display(surface, swapChain);
            graphicsPresenter.AddDisplay(displayName, firstDisplay);
        }
    }
}