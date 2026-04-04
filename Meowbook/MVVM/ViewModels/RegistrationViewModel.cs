using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Meowbook.Services;
using Meowbook.Models;

namespace Meowbook.ViewModels
{
    public partial class RegistrationViewModel : ObservableObject
    {
        private readonly ApiService _apiService;

        [ObservableProperty] private string _name;
        [ObservableProperty] private string _username;
        [ObservableProperty] private string _password;

        [ObservableProperty] private string _email;

        [ObservableProperty] private string _birthdate;

        [ObservableProperty] private bool _isBusy;

        public RegistrationViewModel(ApiService apiService)
        {
            _apiService = apiService;
        }

        [RelayCommand]
        private async Task Register()
        {
            // Validation updated to ensure we have the core requirements including Email
            if (string.IsNullOrWhiteSpace(Name) || string.IsNullOrWhiteSpace(Username) || string.IsNullOrWhiteSpace(Email) || string.IsNullOrWhiteSpace(Password) || string.IsNullOrWhiteSpace(Birthdate))
            {
                await Application.Current.MainPage.DisplayAlert("Validation Error", "Please fill in all the required fields.", "OK");
                return;
            }

            IsBusy = true;
            try
            {
                var newUser = new User
                {
                    Name = Name,
                    Username = Username,
                    Email = Email,
                    Password = Password,
                    Birthdate = Birthdate,
                    Bio = "New cat parent on Meowbook!",
                    Avatar = "https://cdn.pixabay.com/photo/2015/10/05/22/37/blank-profile-picture-973460_1280.png",
                    FriendsList = ""
                };

                bool success = await _apiService.RegisterUserAsync(newUser);

                if (success)
                {
                    await Application.Current.MainPage.DisplayAlert("Success", "Account created! Please login.", "OK");
                    await Application.Current.MainPage.Navigation.PopAsync();
                }
                else
                {
                    await Application.Current.MainPage.DisplayAlert("Error", "Registration failed at the server.", "OK");
                }
            }
            catch (Exception ex)
            {
                await Application.Current.MainPage.DisplayAlert("Error", $"Registration failed: {ex.Message}", "OK");
            }
            finally
            {
                IsBusy = false;
            }
        }

        [RelayCommand]
        private async Task GoToLogin()
        {
            await Application.Current.MainPage.Navigation.PopAsync();
        }
    }
}