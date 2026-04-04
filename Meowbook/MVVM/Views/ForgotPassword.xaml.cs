using Meowbook.Models;
using Meowbook.Services;

namespace Meowbook.Views;

public partial class ForgotPassword : ContentPage
{
    private readonly ApiService _apiService;

    public ForgotPassword()
    {
        InitializeComponent();
        _apiService = new ApiService();
    }

    private async void OnUpdatePasswordClicked(object sender, EventArgs e)
    {
        if (string.IsNullOrWhiteSpace(EmailEntry.Text) || string.IsNullOrWhiteSpace(NewPasswordEntry.Text))
        {
            await DisplayAlert("Error", "All fields are required", "OK");
            return;
        }

        try
        {
            // Fetch users from MockAPI
            var users = await _apiService.GetUsersAsync();
            var user = users.FirstOrDefault(u => 
                (u.Email != null && u.Email.Equals(EmailEntry.Text, StringComparison.OrdinalIgnoreCase)) ||
                (u.Username != null && u.Username.Equals(EmailEntry.Text, StringComparison.OrdinalIgnoreCase)));

            if (user != null)
            {
                user.Password = NewPasswordEntry.Text;
                bool success = await _apiService.UpdateUserAsync(user.Id, user);

                if (success)
                {
                    await DisplayAlert("Success", "Password updated successfully!", "OK");
                    await Application.Current.MainPage.Navigation.PopAsync();
                }
                else
                {
                    await DisplayAlert("Error", "Failed to update password across MockAPI", "OK");
                }
            }
            else
            {
                await DisplayAlert("Error", "User not found", "OK");
            }
        }
        catch
        {
            await DisplayAlert("Error", "API connection failed", "OK");
        }
    }
}