using Meowbook.Views; // Ensure you have this namespace to see your pages

namespace Meowbook
{
    public partial class AppShell : Shell
    {
        public AppShell()
        {
            InitializeComponent();

            // Register routes for pages that are NOT in the TabBar
            // This allows the ViewModel to navigate to them by name
            Routing.RegisterRoute(nameof(LandingPage), typeof(LandingPage));
            Routing.RegisterRoute(nameof(LoginPage), typeof(LoginPage));
            Routing.RegisterRoute(nameof(ForgotPassword), typeof(ForgotPassword));
            Routing.RegisterRoute(nameof(RegistrationPage), typeof(RegistrationPage));  
            // If you have a separate Detail page for posts, register it here too:
            // Routing.RegisterRoute("PostDetailsPage", typeof(PostDetailsPage));
        }
    }
}