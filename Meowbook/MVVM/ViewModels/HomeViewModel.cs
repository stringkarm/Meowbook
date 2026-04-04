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

        // Full collections from API
        private List<Post> _allPosts = new();
        private List<User> _allUsers = new();

        // Collections displayed in the UI
        public ObservableCollection<Post> Posts { get; set; } = new();
        public ObservableCollection<User> DisplayedUsers { get; set; } = new();

        [ObservableProperty]
        private string _searchText;

        [ObservableProperty]
        private bool _isSearching;

        public HomeViewModel(ApiService apiService)
        {
            _apiService = apiService;
            LoggedInUserProfileImage = "profileplaceholder.png";
            CurrentUser = new User { Id = "0", Name = "Meow User", FriendsList = "" };
            _ = InitializeData();
        }

        private async Task InitializeData()
        {
            if (GlobalState.CurrentUser != null)
            {
                CurrentUser = GlobalState.CurrentUser;
                LoggedInUserProfileImage = string.IsNullOrEmpty(CurrentUser.Avatar) ? "profileplaceholder.png" : CurrentUser.Avatar;
            }

            await LoadPosts();
            await LoadAllUsers();
        }

        [RelayCommand]
        public async Task LoadPosts()
        {
            if (IsBusy) return;
            IsBusy = true;
            try
            {
                var posts = await _apiService.GetPostsAsync();
                _allPosts = posts ?? new List<Post>();

                MainThread.BeginInvokeOnMainThread(() =>
                {
                    Posts.Clear();
                    foreach (var post in _allPosts)
                    {
                        Posts.Add(post);
                    }
                });
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"[HomeViewModel] Error loading posts: {ex.Message}");
            }
            finally
            {
                IsBusy = false;
            }
        }

        private async Task LoadAllUsers()
        {
            try
            {
                _allUsers = await _apiService.GetUsersAsync();

                MainThread.BeginInvokeOnMainThread(() =>
                {
                    DisplayedUsers.Clear();
                    foreach (var user in _allUsers)
                    {
                        DisplayedUsers.Add(user);
                    }
                });
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"[HomeViewModel] Error loading users: {ex.Message}");
            }
        }

        [RelayCommand]
        public void PerformSearch()
        {
            if (string.IsNullOrWhiteSpace(SearchText))
            {
                LoadAllUsers();
                IsSearching = false;
                return;
            }

            IsSearching = true;
            var filteredUsers = _allUsers.Where(u => u.Name.Contains(SearchText, StringComparison.OrdinalIgnoreCase)).ToList();

            MainThread.BeginInvokeOnMainThread(() =>
            {
                DisplayedUsers.Clear();
                foreach (var user in filteredUsers)
                {
                    DisplayedUsers.Add(user);
                }
            });
        }

        [RelayCommand]
        public async Task ToggleSearch()
        {
            await Shell.Current.GoToAsync("//SearchPage");
        }

        [RelayCommand]
        public async Task NavigateProfile()
        {
            await Shell.Current.GoToAsync("//MyProfilePage");
        }

        [RelayCommand]
        public async Task NavigateMenu()
        {
            await Shell.Current.GoToAsync("//MenuPage");
        }

        [RelayCommand]
        public async Task NavigateAddPost()
        {
            await Shell.Current.GoToAsync("//AddPostPage");
        }
    }
}