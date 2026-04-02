using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Collections.ObjectModel;
using Meowbook.Models;
using Meowbook.Services;
using System.Diagnostics;

namespace Meowbook.ViewModels
{
    public partial class HomeViewModel : ObservableObject
    {
        private readonly ApiService _apiService;

        [ObservableProperty]
        private bool _isBusy;

        [ObservableProperty]
        private string _loggedInUserProfileImage;

        // The current logged-in user's data
        [ObservableProperty]
        private User _currentUser;

        // Using ObservableCollection for the feed
        public ObservableCollection<Post> Posts { get; set; } = new();

        // ADDED: ObservableCollection for the Live Stories/Users header
        public ObservableCollection<User> Users { get; set; } = new();

        public HomeViewModel(ApiService apiService)
        {
            _apiService = apiService;

            // Initialize default values
            LoggedInUserProfileImage = "profileplaceholder.png";

            // We initialize a dummy user if your Login logic hasn't passed one yet
            // This prevents "Add Friend" from crashing/doing nothing.
            CurrentUser = new User { Id = "0", Name = "Meow User", FriendsList = "" };

            // Initial load
            _ = LoadPosts();
            _ = LoadUsers(); // ADDED: Call the method to load users on startup
        }

        [RelayCommand]
        public async Task LoadPosts()
        {
            if (IsBusy) return;
            IsBusy = true;

            try
            {
                var items = await _apiService.GetPostsAsync();

                // Always wrap collection updates in MainThread for UI stability
                MainThread.BeginInvokeOnMainThread(() =>
                {
                    Posts.Clear();
                    foreach (var item in items)
                    {
                        Posts.Add(item);
                    }
                });
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Fetch error: {ex.Message}");
            }
            finally
            {
                IsBusy = false;
            }
        }

        // ADDED: Method to load users from your MockAPI
        [RelayCommand]
        public async Task LoadUsers()
        {
            try
            {
                var users = await _apiService.GetUsersAsync();

                MainThread.BeginInvokeOnMainThread(() =>
                {
                    Users.Clear();
                    if (users != null)
                    {
                        foreach (var user in users)
                        {
                            Users.Add(user);
                        }
                    }
                });
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"[HomeViewModel] Error loading users: {ex.Message}");
            }
        }

        [RelayCommand]
        public async Task AddFriend(Post post)
        {
            if (post == null || CurrentUser == null) return;

            // 1. Prevent adding yourself
            if (post.UserId == CurrentUser.Id)
            {
                await Application.Current.MainPage.DisplayAlert("Oops", "You can't add yourself!", "OK");
                return;
            }

            // 2. Check for existing friends
            var currentFriends = CurrentUser.FriendsList ?? "";
            var friendIds = currentFriends.Split(',', StringSplitOptions.RemoveEmptyEntries);

            if (friendIds.Contains(post.UserId))
            {
                await Application.Current.MainPage.DisplayAlert("Notice", $"{post.UserName} is already in your list!", "OK");
                return;
            }

            // 3. Update the local string
            if (string.IsNullOrWhiteSpace(currentFriends))
                CurrentUser.FriendsList = post.UserId;
            else
                CurrentUser.FriendsList += $",{post.UserId}";

            // 4. Push update to API
            try
            {
                bool success = await _apiService.UpdateUserAsync(CurrentUser.Id, CurrentUser);
                if (success)
                {
                    await Application.Current.MainPage.DisplayAlert("Meow!", $"Friend request sent to {post.UserName}!", "OK");
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"[HomeViewModel] AddFriend Error: {ex.Message}");
            }
        }
    }
}