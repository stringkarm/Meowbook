using Microsoft.Maui.Controls;
using Meowbook.Services;
using Meowbook.Models;
using System;

namespace Meowbook.Views
{
    public partial class AddPostPage : ContentPage
    {
        private readonly ApiService _apiService;

        public AddPostPage()
        {
            InitializeComponent();
            _apiService = new ApiService();
            ImageUrlEntry.TextChanged += OnImageUrlTextChanged;
        }

        private void OnImageUrlTextChanged(object sender, TextChangedEventArgs e)
        {
            UpdateImagePreview(e.NewTextValue);
        }

        private async void OnChooseImageClicked(object sender, EventArgs e)
        {
            try
            {
                var result = await FilePicker.Default.PickAsync(new PickOptions
                {
                    PickerTitle = "Please select an image",
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
                await DisplayAlert("Error", $"Could not select image: {ex.Message}", "OK");
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
                    // Handle local file path
                    PreviewImage.Source = ImageSource.FromFile(path);
                }
                ImagePreviewContainer.IsVisible = true;
            }
            catch
            {
                ImagePreviewContainer.IsVisible = false;
            }
        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();
            // Slide up like a bottom sheet
            this.TranslationY = this.Height;
            this.Opacity = 0;
            await Task.WhenAll(
                this.TranslateTo(0, 0, 380, Easing.CubicOut),
                this.FadeTo(1, 250, Easing.Linear)
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
                await DisplayAlert("Error", "Post content cannot be empty", "OK");
                return;
            }

            // Button bounce feedback
            await PostBtn.ScaleTo(0.92, 70, Easing.CubicIn);
            await PostBtn.ScaleTo(1.0, 70, Easing.CubicOut);

            PostBtn.IsEnabled = false;

            var newPost = new Post
            {
                UserId = GlobalState.CurrentUserId,
                UserName = GlobalState.CurrentUser?.Name ?? "Unknown",
                UserAvatar = GlobalState.CurrentUser?.Avatar ?? "",
                Content = ContentEditor.Text,
                ImageUrl = ImageUrlEntry.Text,
                Likes = 0,
                Comments = 0,
                Shares = 0
            };

            bool success = await _apiService.CreatePostAsync(newPost);
            
            PostBtn.IsEnabled = true;

            if (success)
            {
                ContentEditor.Text = string.Empty;
                ImageUrlEntry.Text = string.Empty;
                // Fade out before going back
                await this.FadeTo(0, 250, Easing.CubicIn);
                await Shell.Current.GoToAsync("///HomePage");
                this.Opacity = 1;
            }
            else
            {
                await DisplayAlert("Error", "Failed to create post", "OK");
            }
        }
    }
}
