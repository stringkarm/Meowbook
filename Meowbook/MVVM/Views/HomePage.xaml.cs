using Meowbook.ViewModels;

namespace Meowbook.Views;

public partial class HomePage : ContentPage
{
        public HomePage(HomeViewModel viewModel)
        {
            InitializeComponent();
            BindingContext = viewModel; // This connects the C# data to the XAML UI
        }

        private async void OnUserAvatarTapped(object sender, EventArgs e)
        {
            if (sender is VisualElement element && element.BindingContext is Meowbook.Models.Post post)
            {
                await Meowbook.Services.GlobalState.NavigateToProfile(post.UserId);
            }
        }
}