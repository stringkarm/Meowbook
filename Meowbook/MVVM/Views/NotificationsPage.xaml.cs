using Microsoft.Maui.Controls;
using Meowbook.Services;
using System.Collections.ObjectModel;
using System;
using System.Linq;

namespace Meowbook.Views
{
    public partial class NotificationsPage : ContentPage
    {
        private readonly ApiService _apiService;
        public ObservableCollection<string> NotificationItems { get; set; } = new ObservableCollection<string>();

        public NotificationsPage()
        {
            InitializeComponent();
            _apiService = new ApiService();
            NotificationsList.ItemsSource = NotificationItems;
        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();
            if (GlobalState.CurrentUser != null)
            {
                var users = await _apiService.GetUsersAsync();
                var updatedUser = users.FirstOrDefault(u => u.Id == GlobalState.CurrentUserId);
                
                if (updatedUser != null)
                {
                    GlobalState.CurrentUser = updatedUser;
                    NotificationItems.Clear();
                    if (!string.IsNullOrEmpty(updatedUser.Notifications))
                    {
                        var notifs = updatedUser.Notifications.Split(new[] { '|' }, StringSplitOptions.RemoveEmptyEntries);
                        foreach (var n in notifs)
                        {
                            NotificationItems.Add(n);
                        }
                    }
                }
                
                EmptyState.IsVisible = NotificationItems.Count == 0;
            }
        }
    }
}
