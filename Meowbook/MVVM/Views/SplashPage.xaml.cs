namespace Meowbook.Views;

public partial class SplashPage : ContentPage
{
    public SplashPage()
    {
        InitializeComponent();
        RunSplashScreen();
    }

    private async void RunSplashScreen()
    {
        // Wait for 2.5 seconds
        await Task.Delay(2500);

        // Fade out before transitioning
        await this.FadeTo(0, 400, Easing.CubicIn);

        // Swap to AppShell – first page in AppShell.xaml (LandingPage) will fade in
        Application.Current.MainPage = new AppShell();
    }
}