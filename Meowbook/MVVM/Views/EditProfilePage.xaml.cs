using Microsoft.Maui.Controls;
using Meowbook.Services;
using System;

namespace Meowbook.Views
{
    public partial class EditProfilePage : ContentPage
    {
        private readonly ApiService _apiService;

        public EditProfilePage()
        {
            InitializeComponent();
            _apiService = new ApiService();
        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();
            this.Opacity = 0;
            this.TranslationY = 25;
            if (GlobalState.CurrentUser != null)
            {
                NameEntry.Text = GlobalState.CurrentUser.Name;
                UsernameEntry.Text = GlobalState.CurrentUser.Username;
                AvatarEntry.Text = GlobalState.CurrentUser.Avatar;
                BioEditor.Text = GlobalState.CurrentUser.Bio;
                
                AvatarEntry.TextChanged += (s, e) => UpdateAvatarPreview(e.NewTextValue);
                UpdateAvatarPreview(GlobalState.CurrentUser.Avatar);

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
                    AvatarEntry.Text = result.FullPath;
                    UpdateAvatarPreview(result.FullPath);
                }
            }
            catch (Exception ex)
            {
                await DisplayAlert("Error", $"Could not select image: {ex.Message}", "OK");
            }
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
            GlobalState.CurrentUser.Avatar = AvatarEntry.Text;
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
