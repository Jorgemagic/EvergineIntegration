namespace EvergineIntegration.Maui;

public partial class MainPage : ContentPage
{
    public MainPage()
	{
		InitializeComponent();
	}

    protected override void OnAppearing()
    {
        base.OnAppearing();

        // Create app
        MyApplication application = new MyApplication();
        EvergineViewl.Application = application;
    }

    void OnEvergineViewPointerPressed(object sender, EventArgs e)
    {
        ////((FrameworkElement)sender).ReleasePointerCaptures();
    }

    void OnResetCameraClicked(object sender, EventArgs e)
    {        
    }

    void OnDisplacementChanged(object sender, ValueChangedEventArgs e)
    {
        ////_interactionService.Displacement = (float)e.NewValue;
    }
}

