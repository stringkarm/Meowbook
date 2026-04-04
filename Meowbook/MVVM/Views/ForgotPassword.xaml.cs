using Meowbook.Services;
using System.Linq;

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
        if (string.IsNullOrWhiteSpace(EmailEntry.Text) ||
            string.IsNullOrWhiteSpace(NewPasswordEntry.Text) ||
            string.IsNullOrWhiteSpace(ConfirmPasswordEntry.Text))
        {
            await DisplayAlert("Error", "All fields are required", "OK");
            return;
        }

        if (NewPasswordEntry.Text != ConfirmPasswordEntry.Text)
        {
            await DisplayAlert("Error", "Passwords do not match", "OK");
            return;
        }

        try
        {
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
                    await Navigation.PopAsync();
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

    private void OnToggleNewPasswordClicked(object sender, EventArgs e)
    {
        NewPasswordEntry.IsPassword = !NewPasswordEntry.IsPassword;
        ToggleNewPasswordBtn.Rotation = NewPasswordEntry.IsPassword ? 0 : 180;
        ToggleNewPasswordBtn.Opacity = NewPasswordEntry.IsPassword ? 1.0 : 0.6;
    }

    private void OnToggleConfirmPasswordClicked(object sender, EventArgs e)
    {
        ConfirmPasswordEntry.IsPassword = !ConfirmPasswordEntry.IsPassword;
        ToggleConfirmPasswordBtn.Rotation = ConfirmPasswordEntry.IsPassword ? 0 : 180;
        ToggleConfirmPasswordBtn.Opacity = ConfirmPasswordEntry.IsPassword ? 1.0 : 0.6;
    }

    private async void OnBackClicked(object sender, EventArgs e)
    {
        await Navigation.PopAsync();
    }
}