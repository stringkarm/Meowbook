using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Meowbook.Models;
using Meowbook.Services;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;

namespace Meowbook.ViewModels
{
    public partial class SearchViewModel : ObservableObject
    {
        private readonly ApiService _apiService;
        private List<User> _allUsers = new();

        [ObservableProperty]
        private string _searchText = string.Empty;

        [ObservableProperty]
        private bool _isBusy;

        public ObservableCollection<User> SearchResults { get; set; } = new();

        public SearchViewModel()
        {
            _apiService = new ApiService();
            _ = LoadUsersAsync();
        }

        private async Task LoadUsersAsync()
        {
            if (IsBusy) return;
            IsBusy = true;
            try
            {
                var users = await _apiService.GetUsersAsync();
                _allUsers = users ?? new List<User>();
                PerformSearch();
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"[SearchViewModel] Error loading users: {ex.Message}");
            }
            finally
            {
                IsBusy = false;
            }
        }

        [RelayCommand]
        public void PerformSearch()
        {
            MainThread.BeginInvokeOnMainThread(() =>
            {
                SearchResults.Clear();
                var results = string.IsNullOrWhiteSpace(SearchText) 
                    ? _allUsers 
                    : _allUsers.Where(u => u.Name.Contains(SearchText, StringComparison.OrdinalIgnoreCase) || 
                                           u.Username.Contains(SearchText, StringComparison.OrdinalIgnoreCase));
                
                foreach (var u in results)
                {
                    SearchResults.Add(u);
                }
            });
        }
    }
}
