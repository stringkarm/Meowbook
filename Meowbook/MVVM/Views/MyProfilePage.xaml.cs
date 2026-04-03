using Microsoft.Maui.Controls;
using Meowbook.Services;
using System;

namespace Meowbook.Views
{
    public partial class MyProfilePage : ContentPage
    {
        public MyProfilePage()
        {
            InitializeComponent();
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
                if (!string.IsNullOrEmpty(GlobalState.CurrentUser.Avatar))
                {
                    ProfileImage.Source = ImageSource.FromUri(new Uri(GlobalState.CurrentUser.Avatar));
                }
            }
            await Task.WhenAll(
                this.FadeTo(1, 400, Easing.CubicOut),
                this.TranslateTo(0, 0, 400, Easing.CubicOut)
            );
        }

        private async void OnSettingsClicked(object sender, EventArgs e)
        {
            await Shell.Current.GoToAsync("SettingsPage");
        }
    }
}

