using Microsoft.Maui.Controls;
using Meowbook.Models;
using Meowbook.Services;
using System.Text.Json;
using System;

namespace Meowbook.Views
{
    [QueryProperty(nameof(TargetUserId), "userId")]
    public partial class UserProfilePage : ContentPage
    {
        private string _targetUserId;
        public string TargetUserId
        {
            get => _targetUserId;
            set
            {
                _targetUserId = value;
                LoadUserProfile();
            }
        }

        private User _targetUser;
        private readonly ApiService _apiService;

        public UserProfilePage()
        {
            InitializeComponent();
            _apiService = new ApiService();
        }

        private async void LoadUserProfile()
        {
            if (string.IsNullOrEmpty(TargetUserId)) return;
            var users = await _apiService.GetUsersAsync();
            _targetUser = users.FirstOrDefault(u => u.Id == TargetUserId);

            if (_targetUser != null)
            {
                NameLabel.Text = _targetUser.Name;
                UsernameLabel.Text = $"@{_targetUser.Username}";
                if (!string.IsNullOrEmpty(_targetUser.Avatar))
                {
                    ProfileImage.Source = ImageSource.FromUri(new Uri(_targetUser.Avatar));
                }
            }
        }

        private async void OnFollowClicked(object sender, EventArgs e)
        {
            if (_targetUser == null || GlobalState.CurrentUser == null) return;

            FollowBtn.IsEnabled = false;

            // Simple payload
            var notification = $"[New Follow Request] {GlobalState.CurrentUser.Name} wants to follow you!";
            
            if (string.IsNullOrEmpty(_targetUser.Notifications))
            {
                _targetUser.Notifications = notification;
            }
            else
            {
                _targetUser.Notifications += $"|{notification}"; // delimit by pipe for simplicity
            }

            bool success = await _apiService.UpdateUserAsync(_targetUser.Id, _targetUser);
            if (success)
            {
                FollowBtn.Text = "Requested";
                FollowBtn.BackgroundColor = Colors.Gray;
            }
            else
            {
                await DisplayAlert("Error", "Could not send follow request", "OK");
                FollowBtn.IsEnabled = true;
            }
        }
    }
}
