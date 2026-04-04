namespace Meowbook.Services
{
    public static class GlobalState
    {
        public static string CurrentUserId { get; set; } = string.Empty;
        public static Meowbook.Models.User? CurrentUser { get; set; }

        public static async Task NavigateToProfile(string userId)
        {
            if (string.IsNullOrEmpty(userId)) return;

            if (userId == CurrentUserId)
            {
                await Microsoft.Maui.Controls.Shell.Current.GoToAsync("///MyProfilePage");
            }
            else
            {
                await Microsoft.Maui.Controls.Shell.Current.GoToAsync($"UserProfilePage?userId={userId}");
            }
        }
    }
}
