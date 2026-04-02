using Meowbook.ViewModels;
using Microsoft.Extensions.DependencyInjection;

namespace Meowbook.Views
{
    public partial class HomePage : ContentPage
    {
        // 1. Add this default constructor for MainTabbedPage
        public HomePage()
        {
            InitializeComponent();

            // This manually resolves the ViewModel from your DI container
            BindingContext = App.Current.Handler.MauiContext.Services.GetService<HomeViewModel>();
        }

        // 2. Keep this constructor for Shell/Standard DI
        public HomePage(HomeViewModel viewModel)
        {
            InitializeComponent();
            BindingContext = viewModel;
        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();

            if (BindingContext is HomeViewModel vm)
            {
                // Add a small delay to ensure the UI has measured itself
                await Task.Delay(100);

                // Use the command to fetch data
                if (vm.LoadPostsCommand.CanExecute(null))
                {
                    await vm.LoadPostsCommand.ExecuteAsync(null);
                }
            }
        }
    }
}