using Microsoft.Maui.Controls;
using Meowbook.Services;
using System;

namespace Meowbook.Views
{
    public partial class MyProfilePage : ContentPage
    {
        private readonly ApiService _apiService;

        public MyProfilePage()
        {
            InitializeComponent();
            _apiService = new ApiService();
        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();
            this.Opacity = 0;
            this.TranslationY = 20;

            if (GlobalState.CurrentUser != null)
            {
                NameLabel.Text = GlobalState.CurrentUser.Name;
                UsernameLabel.Text = $"@{GlobalState.CurrentUser.Username}";
                BioLabel.Text = string.IsNullOrEmpty(GlobalState.CurrentUser.Bio) ? "Hello! This is my bio." : GlobalState.CurrentUser.Bio;

                if (!string.IsNullOrEmpty(GlobalState.CurrentUser.Avatar))
                {
                    ProfileCoverImage.Source = ImageSource.FromUri(new Uri(GlobalState.CurrentUser.Avatar));
                }

                // Dynamically fetch and filter user posts
                try 
                {
                    var posts = await _apiService.GetPostsAsync();
                    if (posts != null)
                    {
                        var userPosts = posts.Where(p => 
                            (p.UserId != null && p.UserId == GlobalState.CurrentUserId) || 
                            (p.UserName != null && (p.UserName == GlobalState.CurrentUser.Name || p.UserName == GlobalState.CurrentUser.Username))
                        ).ToList();
                        
                        PostsCollectionView.ItemsSource = userPosts;
                    }
                }
                catch 
                {
                    // Fallback to empty if fetch fails
                    PostsCollectionView.ItemsSource = null;
                }
            }

            await Task.WhenAll(
                this.FadeTo(1, 400, Easing.CubicOut),
                this.TranslateTo(0, 0, 400, Easing.CubicOut)
            );
        }

        private async void OnSettingsClicked(object sender, EventArgs e)
        {
            await Shell.Current.GoToAsync("EditProfilePage");
        }
    }
}

