using System.Net.Http.Json;
using System.Diagnostics;
using Meowbook.Models;

namespace Meowbook.Services
{
    public class ApiService
    {
        private readonly HttpClient _httpClient;
        private const string BaseUrl = "https://69c509418a5b6e2dec2bacca.mockapi.io/";

        public ApiService()
        {
            _httpClient = new HttpClient { BaseAddress = new Uri(BaseUrl) };
        }

        #region User Operations

        public async Task<List<User>> GetUsersAsync()
        {
            try
            {
                return await _httpClient.GetFromJsonAsync<List<User>>("users") ?? new List<User>();
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Error fetching users: {ex.Message}");
                return new List<User>();
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

        // --- NEW: Update User (Crucial for the Add Friend logic) ---
        public async Task<bool> UpdateUserAsync(string userId, User updatedUser)
        {
            try
            {
                // This sends the updated friendsList back to MockAPI
                var response = await _httpClient.PutAsJsonAsync($"users/{userId}", updatedUser);
                return response.IsSuccessStatusCode;
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Error updating user: {ex.Message}");
                return false;
            }
        }

        #endregion

        #region Post Operations

        public async Task<List<Post>> GetPostsAsync()
        {
            try
            {
                // Ensure the endpoint string matches your MockAPI resource name
                return await _httpClient.GetFromJsonAsync<List<Post>>("posts") ?? new List<Post>();
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Error fetching posts: {ex.Message}");
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

        public async Task<bool> UpdatePostAsync(string id, Post updatedPost)
        {
            try
            {
                var response = await _httpClient.PutAsJsonAsync($"posts/{id}", updatedPost);
                return response.IsSuccessStatusCode;
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Error updating post: {ex.Message}");
                return false;
            }
        }

        public async Task<bool> DeletePostAsync(string id)
        {
            try
            {
                var response = await _httpClient.DeleteAsync($"posts/{id}");
                return response.IsSuccessStatusCode;
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Error deleting post: {ex.Message}");
                return false;
            }
        }

        #endregion
    }
}