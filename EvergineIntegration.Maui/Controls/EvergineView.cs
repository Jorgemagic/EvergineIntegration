namespace EvergineIntegration.Maui.Controls
{
    public class EvergineView : View
    {
        public static readonly BindableProperty ApplicationProperty =
                BindableProperty.Create(nameof(Application), typeof(Evergine.Framework.Application), typeof(EvergineView), null);

        public static readonly BindableProperty DisplayNameProperty =
            BindableProperty.Create(nameof(DisplayName), typeof(string), typeof(EvergineView), string.Empty);

        public Evergine.Framework.Application Application
        {
            get { return (Evergine.Framework.Application)GetValue(ApplicationProperty); }
            set { SetValue(ApplicationProperty, value); }
        }

        public string DisplayName
        {
            get { return (string)GetValue(DisplayNameProperty); }
            set { SetValue(DisplayNameProperty, value); }
        }

        public event EventHandler PointerPressed;
        public event EventHandler PointerMoved;
        public event EventHandler PointerReleased;

        internal void StartInteraction() => PointerPressed?.Invoke(this, EventArgs.Empty);

        internal void MovedInteraction() => PointerMoved?.Invoke(this, EventArgs.Empty);

        internal void EndInteraction() => PointerReleased?.Invoke(this, EventArgs.Empty);
    }
}
