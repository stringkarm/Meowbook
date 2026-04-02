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
        // Wait for 3 seconds
        await Task.Delay(3000);

        // Swap to AppShell
        // This will automatically load the first page defined in AppShell.xaml
        Application.Current.MainPage = new AppShell();
    }
}