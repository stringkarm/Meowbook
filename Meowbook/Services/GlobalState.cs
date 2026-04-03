namespace Meowbook.Services
{
    public static class GlobalState
    {
        public static string CurrentUserId { get; set; } = string.Empty;
        public static Meowbook.Models.User? CurrentUser { get; set; }
    }
}
