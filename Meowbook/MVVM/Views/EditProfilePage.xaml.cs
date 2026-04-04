using Microsoft.Maui.Controls;
using Meowbook.Services;
using System;

namespace Meowbook.Views
{
    public partial class EditProfilePage : ContentPage
    {
        private readonly ApiService _apiService;
        private string _avatarPath;

        public EditProfilePage()
        {
            InitializeComponent();
            _apiService = new ApiService();
        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();
            
            if (GlobalState.CurrentUser != null)
            {
                NameEntry.Text = GlobalState.CurrentUser.Name;
                UsernameEntry.Text = GlobalState.CurrentUser.Username;
                CurrentNameLabel.Text = GlobalState.CurrentUser.Name;
                CurrentUsernameLabel.Text = $"@{GlobalState.CurrentUser.Username}";
                BioEditor.Text = GlobalState.CurrentUser.Bio;
                _avatarPath = GlobalState.CurrentUser.Avatar;
                
                UpdateAvatarPreview(_avatarPath);

                if (DateTime.TryParse(GlobalState.CurrentUser.Birthdate, out DateTime parsedDate))
                {
                    BirthdatePicker.Date = parsedDate;
                }
            }

            await Task.WhenAll(
                this.FadeTo(1, 400, Easing.CubicOut),
                this.TranslateTo(0, 0, 400, Easing.CubicOut)
            );
        }

        private async void OnChooseAvatarClicked(object sender, EventArgs e)
        {
            try
            {
                var result = await FilePicker.Default.PickAsync(new PickOptions
                {
                    PickerTitle = "Please select a profile photo",
                    FileTypes = FilePickerFileType.Images
                });

                if (result != null)
                {
                    _avatarPath = result.FullPath;
                    UpdateAvatarPreview(_avatarPath);
                }
            }
            catch (Exception ex)
            {
                await DisplayAlert("Error", $"Could not select image: {ex.Message}", "OK");
            }
        }

        private async void OnUrlAvatarClicked(object sender, EventArgs e)
        {
            string result = await DisplayPromptAsync("External Avatar", 
                "Paste the image link here:", "Add", "Cancel", "https://...");

            if (!string.IsNullOrWhiteSpace(result))
            {
                _avatarPath = result;
                UpdateAvatarPreview(_avatarPath);
            }
        }

        private async void OnBackClicked(object sender, EventArgs e)
        {
            await Shell.Current.GoToAsync("..");
        }

        private void UpdateAvatarPreview(string path)
        {
            if (string.IsNullOrWhiteSpace(path))
            {
                AvatarPreviewImage.Source = "profileplaceholder.png";
                return;
            }

            try
            {
                if (path.StartsWith("http") || path.StartsWith("https"))
                {
                    AvatarPreviewImage.Source = ImageSource.FromUri(new Uri(path));
                }
                else
                {
                    AvatarPreviewImage.Source = ImageSource.FromFile(path);
                }
            }
            catch
            {
                AvatarPreviewImage.Source = "profileplaceholder.png";
            }
        }

        private async void OnSaveClicked(object sender, EventArgs e)
        {
            if (GlobalState.CurrentUser == null) return;

            GlobalState.CurrentUser.Name = NameEntry.Text;
            GlobalState.CurrentUser.Username = UsernameEntry.Text;
            GlobalState.CurrentUser.Birthdate = BirthdatePicker.Date.ToString("yyyy-MM-dd");
            GlobalState.CurrentUser.Avatar = _avatarPath;
            GlobalState.CurrentUser.Bio = BioEditor.Text;

            bool success = await _apiService.UpdateUserAsync(GlobalState.CurrentUser.Id, GlobalState.CurrentUser);
            if (success)
            {
                await DisplayAlert("Success", "Profile updated successfully!", "OK");
                await Shell.Current.GoToAsync("..");
            }
            else
            {
                await DisplayAlert("Error", "Failed to update profile.", "OK");
            }
        }
    }
}
