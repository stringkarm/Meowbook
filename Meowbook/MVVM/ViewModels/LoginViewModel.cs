using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Meowbook.Services;
using Meowbook.Models;

namespace Meowbook.ViewModels
{
    public partial class LoginViewModel : ObservableObject
    {
        private readonly ApiService _apiService;

        // Changed from _email to _username to match your new schema
        [ObservableProperty]
        private string _username;

        [ObservableProperty]
        private string _password;

        [ObservableProperty]
        private bool _isBusy;

        public LoginViewModel(ApiService apiService)
        {
            _apiService = apiService;
        }

        [RelayCommand]
        private async Task Login()
        {
            // Updated check for Username instead of Email
            if (string.IsNullOrWhiteSpace(Username) || string.IsNullOrWhiteSpace(Password))
            {
                await Application.Current.MainPage.DisplayAlert("Error", "Please enter your username and password!", "OK");
                return;
            }

            IsBusy = true;

            try
            {
                var users = await _apiService.GetUsersAsync();

                // Look for a match using Username
                var user = users?.FirstOrDefault(u =>
                    u.Username.Equals(Username, StringComparison.OrdinalIgnoreCase) &&
                    u.Password == Password);

                if (user != null)
                {
                    // Success! Save globally
                    GlobalState.CurrentUserId = user.Id;
                    GlobalState.CurrentUser = user;

                    Application.Current.MainPage = new AppShell();
                }
                else
                {
                    await Application.Current.MainPage.DisplayAlert("Login Failed", "Invalid username or password.", "Try Again");
                }
            }
            catch (Exception ex)
            {
                await Application.Current.MainPage.DisplayAlert("Error", "Connection error. Try again later.", "OK");
            }
            finally
            {
                IsBusy = false;
            }
        }

        [RelayCommand]
        private async Task GoToRegister()
        {
            await Application.Current.MainPage.Navigation.PushAsync(new Meowbook.Views.RegistrationPage(new RegistrationViewModel(_apiService)));
        }

        [RelayCommand]
        private async Task ForgotPassword()
        {
            await Application.Current.MainPage.Navigation.PushAsync(new Meowbook.Views.ForgotPassword());
        }
    }
}