using Meowbook.Services;

namespace Meowbook.Views;

public partial class MenuPage : ContentPage
{
    public MenuPage()
    {
        InitializeComponent();
    }

    protected override void OnAppearing()
    {
        base.OnAppearing();
        if (GlobalState.CurrentUser != null)
        {
            MenuNameLabel.Text = GlobalState.CurrentUser.Name;
            if (!string.IsNullOrEmpty(GlobalState.CurrentUser.Avatar))
            {
                MenuProfileImage.Source = ImageSource.FromUri(new Uri(GlobalState.CurrentUser.Avatar));
            }
        }
    }

    private async void OnViewProfileClicked(object sender, EventArgs e)
    {
        await Shell.Current.GoToAsync("UserProfilePage"); // Or MyProfilePage. Let's use MyProfilePage
    }

    private async void OnEditProfileClicked(object sender, EventArgs e)
    {
        await Shell.Current.GoToAsync("EditProfilePage");
    }

    private async void OnSettingsClicked(object sender, EventArgs e)
    {
        await Shell.Current.GoToAsync("SettingsPage");
    }

    private async void OnHelpClicked(object sender, EventArgs e)
    {
        await DisplayAlert("Help & Support", "Support feature coming soon.", "OK");
    }

    private async void OnTermsClicked(object sender, EventArgs e)
    {
        await DisplayAlert("Terms of Service", "Terms of Service feature coming soon.", "OK");
    }

    private void OnLogoutClicked(object sender, EventArgs e)
    {
        GlobalState.CurrentUser = null;
        GlobalState.CurrentUserId = string.Empty;
        Application.Current.MainPage = new SplashPage();
    }
}