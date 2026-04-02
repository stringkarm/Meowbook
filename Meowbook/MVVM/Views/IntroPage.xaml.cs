namespace Meowbook.Views;

public partial class IntroPage : ContentPage
{
    public IntroPage()
    {
        InitializeComponent();
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();

        // 5-second interval as requested
        await Task.Delay(5000);

        // Direct to LandingPage inside a NavigationPage so we can push/pop other screens
        if (Application.Current != null)
        {
            Application.Current.MainPage = new NavigationPage(new LandingPage());
        }
    }
}