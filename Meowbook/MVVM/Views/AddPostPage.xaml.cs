using Microsoft.Maui.Controls;
using Meowbook.Services;
using Meowbook.Models;
using System;
using System.Threading.Tasks;

namespace Meowbook.Views
{
    public partial class AddPostPage : ContentPage
    {
        private readonly ApiService _apiService;

        public AddPostPage()
        {
            InitializeComponent();
            _apiService = new ApiService();

            // Setup User Data for the Header
            LoadUserData();
        }

        private void LoadUserData()
        {
            // Provides the profile picture and name at the top of the page
            UserNameLabel.Text = GlobalState.CurrentUser?.Name ?? "Meowbook User";

            if (!string.IsNullOrEmpty(GlobalState.CurrentUser?.Avatar))
            {
                UserAvatarImg.Source = GlobalState.CurrentUser.Avatar;
            }
        }

        private void OnContentTextChanged(object sender, TextChangedEventArgs e)
        {
            bool hasContent = !string.IsNullOrWhiteSpace(e.NewTextValue);
            PostBtn.IsEnabled = hasContent;

            // Update button appearance based on state
            if (hasContent)
            {
                PostBtn.BackgroundColor = Color.FromArgb("#007AFF"); // iOS Blue style
                PostBtn.TextColor = Colors.White;
            }
            else
            {
                PostBtn.BackgroundColor = Color.FromArgb("#E0E0E0");
                PostBtn.TextColor = Color.FromArgb("#8E8E8E");
            }
        }

        // Logic for the Photo (Gallery) Icon
        private async void OnChooseImageClicked(object sender, EventArgs e)
        {
            try
            {
                var result = await FilePicker.Default.PickAsync(new PickOptions
                {
                    PickerTitle = "Select a photo",
                    FileTypes = FilePickerFileType.Images
                });

                if (result != null)
                {
                    ImageUrlEntry.Text = result.FullPath;
                    UpdateImagePreview(result.FullPath);
                }
            }
            catch (Exception ex)
            {
                await DisplayAlert("Error", $"Gallery access failed: {ex.Message}", "OK");
            }
        }

        // Logic for the URL Icon (matches OnUrlIconClicked in XAML)
        private async void OnUrlIconClicked(object sender, EventArgs e)
        {
            string result = await DisplayPromptAsync("External Image",
                "Paste the image link here:", "Add", "Cancel", "https://...");

            if (!string.IsNullOrWhiteSpace(result))
            {
                ImageUrlEntry.Text = result;
                UpdateImagePreview(result);
            }
        }

        private void UpdateImagePreview(string path)
        {
            if (string.IsNullOrWhiteSpace(path))
            {
                ImagePreviewContainer.IsVisible = false;
                return;
            }

            try
            {
                if (path.StartsWith("http") || path.StartsWith("https"))
                {
                    PreviewImage.Source = ImageSource.FromUri(new Uri(path));
                }
                else
                {
                    PreviewImage.Source = ImageSource.FromFile(path);
                }
                ImagePreviewContainer.IsVisible = true;
            }
            catch
            {
                ImagePreviewContainer.IsVisible = false;
            }
        }

        // Logic for the 'X' button on the image (matches OnRemoveImageClicked in XAML)
        private void OnRemoveImageClicked(object sender, EventArgs e)
        {
            ImageUrlEntry.Text = string.Empty;
            ImagePreviewContainer.IsVisible = false;
            PreviewImage.Source = null;
        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();

            // "Bottom Sheet" slide-up animation
            this.TranslationY = this.Height;
            this.Opacity = 0;

            await Task.WhenAll(
                this.TranslateTo(0, 0, 400, Easing.CubicOut),
                this.FadeTo(1, 200, Easing.Linear)
            );
        }

        private async void OnCancelClicked(object sender, EventArgs e)
        {
            ContentEditor.Text = string.Empty;
            ImageUrlEntry.Text = string.Empty;

            await Shell.Current.GoToAsync("///HomePage");
        }

        private async void OnPostClicked(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(ContentEditor.Text))
            {
                await DisplayAlert("Meowbook", "Please write something first!", "OK");
                return;
            }

            PostBtn.IsEnabled = false;

            var newPost = new Post
            {
                UserId = GlobalState.CurrentUserId,
                UserName = GlobalState.CurrentUser?.Name ?? "User",
                UserAvatar = GlobalState.CurrentUser?.Avatar ?? "",
                Content = ContentEditor.Text,
                ImageUrl = ImageUrlEntry.Text,
                CreatedAt = DateTime.Now
            };

            bool success = await _apiService.CreatePostAsync(newPost);

            if (success)
            {
                ContentEditor.Text = string.Empty;
                ImageUrlEntry.Text = string.Empty;
                await Shell.Current.GoToAsync("///HomePage");
            }
            else
            {
                await DisplayAlert("Error", "Could not create post.", "OK");
            }

            PostBtn.IsEnabled = true;
        }
    }
}