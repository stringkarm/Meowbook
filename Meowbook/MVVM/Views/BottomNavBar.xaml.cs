using Microsoft.Maui.Controls;

namespace Meowbook.Views
{
    public partial class BottomNavBar : ContentView
    {
        // ── Bindable Property ─────────────────────────────────────────────
        public static readonly BindableProperty ActiveTabProperty =
            BindableProperty.Create(
                nameof(ActiveTab),
                typeof(string),
                typeof(BottomNavBar),
                "Home",
                propertyChanged: OnActiveTabChanged);

        public string ActiveTab
        {
            get => (string)GetValue(ActiveTabProperty);
            set => SetValue(ActiveTabProperty, value);
        }

        // ── Constructor ───────────────────────────────────────────────────
        public BottomNavBar()
        {
            InitializeComponent();
        }

        // ── Active Tab Highlighting ───────────────────────────────────────
        private static void OnActiveTabChanged(BindableObject bindable, object oldVal, object newVal)
        {
            var bar = (BottomNavBar)bindable;
            bar.UpdateActiveState(newVal as string);
        }

        private void UpdateActiveState(string activeTab)
        {
            // Reset all to grey
            HomeIcon.TextColor    = Color.FromArgb("#BBBBBB");
            FriendsIcon.TextColor = Color.FromArgb("#BBBBBB");
            NotifIcon.TextColor   = Color.FromArgb("#BBBBBB");
            ProfileIcon.TextColor = Color.FromArgb("#BBBBBB");
            HomeDot.IsVisible     = false;
            FriendsDot.IsVisible  = false;
            NotifDot.IsVisible    = false;
            ProfileDot.IsVisible  = false;

            // Highlight active
            var blue = Color.FromArgb("#4A90D9");
            switch (activeTab)
            {
                case "Home":
                    HomeIcon.TextColor  = blue;
                    HomeDot.IsVisible   = true;
                    break;
                case "Friends":
                    FriendsIcon.TextColor = blue;
                    FriendsDot.IsVisible  = true;
                    break;
                case "Notifications":
                    NotifIcon.TextColor = blue;
                    NotifDot.IsVisible  = true;
                    break;
                case "Profile":
                    ProfileIcon.TextColor = blue;
                    ProfileDot.IsVisible  = true;
                    break;
            }
        }

        // ── Tap Handlers ──────────────────────────────────────────────────
        private async void OnHomeTapped(object sender, TappedEventArgs e)
        {
            await AnimateIcon(HomeIcon);
            await Shell.Current.GoToAsync("//HomePage");
        }

        private async void OnFriendsTapped(object sender, TappedEventArgs e)
        {
            await AnimateIcon(FriendsIcon);
            await Shell.Current.GoToAsync("//MessagesPage");
        }

        private async void OnNotifTapped(object sender, TappedEventArgs e)
        {
            await AnimateIcon(NotifIcon);
            await Shell.Current.GoToAsync("//NotificationsPage");
        }

        private async void OnProfileTapped(object sender, TappedEventArgs e)
        {
            await AnimateIcon(ProfileIcon);
            await Shell.Current.GoToAsync("//MyProfilePage");
        }

        private async void OnFabTapped(object sender, TappedEventArgs e)
        {
            // Scale the FAB for tactile feedback
            var fab = (View)sender;
            await fab.ScaleTo(0.88, 80, Easing.CubicIn);
            await fab.ScaleTo(1.0, 100, Easing.SpringOut);
            await Shell.Current.GoToAsync("AddPostPage");
        }

        // ── Micro-animation helper ────────────────────────────────────────
        private static async Task AnimateIcon(View icon)
        {
            await icon.ScaleTo(0.80, 70, Easing.CubicIn);
            await icon.ScaleTo(1.0,  80, Easing.CubicOut);
        }
    }
}
