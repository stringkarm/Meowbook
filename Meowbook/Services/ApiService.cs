using System.Net.Http.Json;
using System.Diagnostics;
using Meowbook.Models;

namespace Meowbook.Services
{
    public class ApiService
    {
        private readonly HttpClient _httpClient;
        // Updated to your current project URL from the screenshot
        private const string BaseUrl = "https://69c509418a5b6e2dec2bacca.mockapi.io/";

        public ApiService()
        {
            _httpClient = new HttpClient
            {
                BaseAddress = new Uri(BaseUrl),
                Timeout = TimeSpan.FromSeconds(15)
            };
        }

        #region User Operations

        public async Task<List<User>> GetUsersAsync()
        {
            try
            {
                // Returns all users to populate the "Stories" bar or friend lists
                return await _httpClient.GetFromJsonAsync<List<User>>("users") ?? new List<User>();
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Error fetching users: {ex.Message}");
                return new List<User>();
            }
        }

        public async Task<User?> LoginAsync(string username, string password)
        {
            try
            {
                var users = await GetUsersAsync();
                // Validates credentials against the data in MockAPI
                return users.FirstOrDefault(u => u.Username == username && u.Password == password);
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Login Error: {ex.Message}");
                return null;
            }
        }

        public async Task<bool> RegisterUserAsync(User newUser)
        {
            try
            {
                var response = await _httpClient.PostAsJsonAsync("users", newUser);
                return response.IsSuccessStatusCode;
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Error registering: {ex.Message}");
                return false;
            }
        }

        public async Task<bool> UpdateUserAsync(string userId, User updatedUser)
        {
            try
            {
                var response = await _httpClient.PutAsJsonAsync($"users/{userId}", updatedUser);
                return response.IsSuccessStatusCode;
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Error updating user: {ex.Message}");
                return false;
            }
        }

        public async Task<bool> DeleteUserAsync(string userId)
        {
            try
            {
                var response = await _httpClient.DeleteAsync($"users/{userId}");
                return response.IsSuccessStatusCode;
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Error deleting user: {ex.Message}");
                return false;
            }
        }

        #endregion

        #region Post Operations

        public async Task<List<Post>> GetPostsAsync()
        {
            try
            {
                return await _httpClient.GetFromJsonAsync<List<Post>>("posts") ?? new List<Post>();
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"[ApiService] GetPostsAsync error: {ex.Message}");
                return new List<Post>();
            }
        }

        public async Task<bool> CreatePostAsync(Post newPost)
        {
            try
            {
                var response = await _httpClient.PostAsJsonAsync("posts", newPost);
                return response.IsSuccessStatusCode;
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Error creating post: {ex.Message}");
                return false;
            }
        }

        #endregion

        // Soft Delete (Deactivate) - Usually involves setting an "IsActive" flag to false
        public async Task<bool> DeactivateUserAsync(string userId, User user)
        {
            // MockAPI doesn't have a 'soft delete' toggle, so we update a property
            user.Bio = "DEACTIVATED_ACCOUNT";
            var response = await _httpClient.PutAsJsonAsync($"users/{userId}", user);
            return response.IsSuccessStatusCode;
        }

        // Hard Delete - Completely removes user from MockAPI
        public async Task<bool> DeleteUserPermanentlyAsync(string userId)
        {
            var response = await _httpClient.DeleteAsync($"users/{userId}");
            return response.IsSuccessStatusCode;
        }
    }
}