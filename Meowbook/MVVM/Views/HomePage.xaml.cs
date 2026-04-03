using Meowbook.ViewModels;

namespace Meowbook.Views;

public partial class HomePage : ContentPage
{
    public HomePage(HomeViewModel viewModel)
    {
        InitializeComponent();
        BindingContext = viewModel; // This connects the C# data to the XAML UI
    }
}