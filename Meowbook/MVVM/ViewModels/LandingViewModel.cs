using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Meowbook.Views;

namespace Meowbook.ViewModels;

public partial class LandingViewModel : ObservableObject
{
    [RelayCommand]
    private async Task GoToLogin()
    {
        // Pushes the LoginPage onto the navigation stack
        if (Application.Current?.MainPage != null)
        {
            await Application.Current.MainPage.Navigation.PushAsync(new LoginPage());
        }
    }

    [RelayCommand]
    private async Task GoToRegister()
    {
        // Pushes the RegistrationPage onto the navigation stack
        // (Assuming you will create a RegisterPage next)
        if (Application.Current?.MainPage != null)
        {
            // Uncomment the line below once RegisterPage.xaml is created
            // await Application.Current.MainPage.Navigation.PushAsync(new RegisterPage());

            // Temporary placeholder alert for testing:
            await Application.Current.MainPage.DisplayAlert("Purr-fect!", "Registration page coming next.", "OK");
        }
    }
}