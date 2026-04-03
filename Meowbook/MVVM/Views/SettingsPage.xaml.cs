using Microsoft.Maui.Controls;
using Meowbook.Services;
using System;

namespace Meowbook.Views
{
    public partial class SettingsPage : ContentPage
    {
        private readonly ApiService _apiService;

        public SettingsPage()
        {
            InitializeComponent();
            _apiService = new ApiService();
        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();
            this.Opacity = 0;
            this.TranslationY = 25;
            await Task.WhenAll(
                this.FadeTo(1, 400, Easing.CubicOut),
                this.TranslateTo(0, 0, 400, Easing.CubicOut)
            );
        }

        private async void OnChangePasswordClicked(object sender, EventArgs e)
        {
            if (GlobalState.CurrentUser == null) return;
            string newPass = PasswordEntry.Text;
            string confPass = ConfirmPasswordEntry.Text;

            if (string.IsNullOrWhiteSpace(newPass) || newPass != confPass)
            {
                await DisplayAlert("Error", "Passwords do not match or are empty.", "OK");
                return;
            }

            GlobalState.CurrentUser.Password = newPass; // Assuming MockAPI model has Password field
            bool success = await _apiService.UpdateUserAsync(GlobalState.CurrentUser.Id, GlobalState.CurrentUser);
            if (success)
            {
                await DisplayAlert("Success", "Password updated successfully!", "OK");
                PasswordEntry.Text = "";
                ConfirmPasswordEntry.Text = "";
            }
            else
            {
                await DisplayAlert("Error", "Failed to update password", "OK");
            }
        }

        private async void OnDeactivateClicked(object sender, EventArgs e)
        {
            bool confirm = await DisplayAlert("Deactivate", "Are you sure you want to deactivate your account?", "Yes", "No");
            if (confirm && GlobalState.CurrentUser != null)
            {
                GlobalState.CurrentUser.IsActive = false;
                await _apiService.UpdateUserAsync(GlobalState.CurrentUser.Id, GlobalState.CurrentUser);
                
                GlobalState.CurrentUser = null;
                GlobalState.CurrentUserId = string.Empty;
                await Shell.Current.GoToAsync("//SplashPage");
            }
        }

        private async void OnDeleteClicked(object sender, EventArgs e)
        {
            bool confirm = await DisplayAlert("Delete Permanently", "Are you sure? This cannot be undone.", "Yes", "No");
            if (confirm && GlobalState.CurrentUser != null)
            {
                bool deleted = await _apiService.DeleteUserAsync(GlobalState.CurrentUser.Id);
                if (deleted)
                {
                    await DisplayAlert("Deleted", "Account deleted from system.", "OK");
                    GlobalState.CurrentUser = null;
                    GlobalState.CurrentUserId = string.Empty;
                    await Shell.Current.GoToAsync("//SplashPage");
                }
                else
                {
                    await DisplayAlert("Error", "Failed to delete account.", "OK");
                }
            }
        }
    }
}
