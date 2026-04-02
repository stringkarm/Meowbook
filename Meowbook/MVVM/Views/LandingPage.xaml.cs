using Meowbook.ViewModels;

namespace Meowbook.Views;

public partial class LandingPage : ContentPage
{
    // Option A: Standard constructor (Manual Binding)
    public LandingPage()
    {
        InitializeComponent();

        // Bind the UI to the ViewModel so our Commands work
        BindingContext = new LandingViewModel();
    }

    // Option B: Dependency Injection constructor 
    // (Use this if you are registering your pages and ViewModels in MauiProgram.cs)
    public LandingPage(LandingViewModel viewModel)
    {
        InitializeComponent();
        BindingContext = viewModel;
    }

    private async void OnGetStartedClicked(object sender, EventArgs e)
    {
        try
        {
            // Ensure Shell is actually ready before navigating
            if (Shell.Current != null)
            {
                await Shell.Current.GoToAsync("LoginPage");
            }
            else
            {
                // Fallback if Shell isn't loaded yet
                Application.Current.MainPage = new AppShell();
            }
        }
        catch (Exception ex)
        {
            await DisplayAlert("Error", "Could not reach the login screen.", "OK");
        }
    }
}