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
            var grey = Color.FromArgb("#BBBBBB");
            HomeIcon.TextColor    = grey;
            HomeText.TextColor    = grey;
            SearchIcon.TextColor  = grey;
            SearchText.TextColor  = grey;
            ProfileIcon.TextColor = grey;
            ProfileText.TextColor = grey;
            MenuIcon.TextColor    = grey;
            MenuText.TextColor    = grey;

            // Highlight active
            var activeColor = Color.FromArgb("#A64BFF");
            switch (activeTab)
            {
                case "Home":
                    HomeIcon.TextColor  = activeColor;
                    HomeText.TextColor  = activeColor;
                    break;
                case "Search":
                    SearchIcon.TextColor = activeColor;
                    SearchText.TextColor = activeColor;
                    break;
                case "Profile":
                    ProfileIcon.TextColor = activeColor;
                    ProfileText.TextColor = activeColor;
                    break;
                case "Menu":
                    MenuIcon.TextColor = activeColor;
                    MenuText.TextColor = activeColor;
                    break;
            }
        }

        // ── Tap Handlers ──────────────────────────────────────────────────
        private async void OnHomeTapped(object sender, TappedEventArgs e)
        {
            await AnimateIcon(HomeIcon);
            await Shell.Current.GoToAsync("///HomePage");
        }

        private async void OnSearchTapped(object sender, TappedEventArgs e)
        {
            await AnimateIcon(SearchIcon);
            await Shell.Current.GoToAsync("///SearchPage");
        }

        private async void OnProfileTapped(object sender, TappedEventArgs e)
        {
            await AnimateIcon(ProfileIcon);
            await Shell.Current.GoToAsync("///MyProfilePage");
        }

        private async void OnMenuTapped(object sender, TappedEventArgs e)
        {
            await AnimateIcon(MenuIcon);
            await Shell.Current.GoToAsync("///MenuPage");
        }

        private async void OnFabTapped(object sender, TappedEventArgs e)
        {
            // Scale the FAB for tactile feedback
            var fab = (View)sender;
            await fab.ScaleTo(0.88, 80, Easing.CubicIn);
            await fab.ScaleTo(1.0, 100, Easing.SpringOut);
            await Shell.Current.GoToAsync("///AddPostPage");
        }

        // ── Micro-animation helper ────────────────────────────────────────
        private static async Task AnimateIcon(View icon)
        {
            await icon.ScaleTo(0.80, 70, Easing.CubicIn);
            await icon.ScaleTo(1.0,  80, Easing.CubicOut);
        }
    }
}
