using System.Text;
using System.Text.Json;
using Meowbook.Models;

namespace Meowbook.Views;

public partial class ForgotPassword : ContentPage
{
    private readonly HttpClient _client = new HttpClient();
    private const string ApiUrl = "https://69c509418a5b6e2dec2baccb.mockapi.io/";

    public ForgotPassword() => InitializeComponent();

    private async void OnUpdatePasswordClicked(object sender, EventArgs e)
    {
        if (string.IsNullOrWhiteSpace(EmailEntry.Text) || string.IsNullOrWhiteSpace(NewPasswordEntry.Text))
        {
            await DisplayAlert("Error", "All fields are required", "OK");
            return;
        }

        try
        {
            // 1. Fetch user by email
            var response = await _client.GetStringAsync($"{ApiUrl}?email={EmailEntry.Text}");
            var users = JsonSerializer.Deserialize<List<User>>(response, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

            if (users?.Count > 0)
            {
                var user = users[0];
                user.Password = NewPasswordEntry.Text;

                // 2. Update MockAPI data
                var json = JsonSerializer.Serialize(user);
                var content = new StringContent(json, Encoding.UTF8, "application/json");
                var updateResult = await _client.PutAsync($"{ApiUrl}/{user.Id}", content);

                if (updateResult.IsSuccessStatusCode)
                {
                    await DisplayAlert("Success", "Password updated!", "OK");
                    await Shell.Current.GoToAsync("..");
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