using Meowbook.ViewModels;

namespace Meowbook.Views;

public partial class LandingPage : ContentPage
{
    public LandingPage()
    {
        InitializeComponent();
        BindingContext = new LandingViewModel();
    }

    public LandingPage(LandingViewModel viewModel)
    {
        InitializeComponent();
        BindingContext = viewModel;
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();
        // Fade the page in from transparent
        this.Opacity = 0;
        await this.FadeTo(1, 500, Easing.CubicOut);
    }

    private async void OnGetStartedClicked(object sender, EventArgs e)
    {
        try
        {
            // Animate button press: scale down then up
            var btn = (Button)sender;
            await btn.ScaleTo(0.93, 80, Easing.CubicIn);
            await btn.ScaleTo(1.0, 80, Easing.CubicOut);

            // Fade the whole page out before navigating
            await this.FadeTo(0, 300, Easing.CubicIn);

            // Navigate using the absolute route (// prefix required for root ShellContent routes)
            await Shell.Current.GoToAsync("//LoginPage");

            // Reset opacity for when we come back
            this.Opacity = 1;
        }
        catch (Exception)
        {
            this.Opacity = 1;
            await DisplayAlert("Error", "Could not reach the login screen.", "OK");
        }
    }
}
