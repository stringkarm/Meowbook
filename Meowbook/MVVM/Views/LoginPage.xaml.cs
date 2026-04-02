using Meowbook.ViewModels;
using Meowbook.Services;

namespace Meowbook.Views;

public partial class LoginPage : ContentPage
{
    public LoginPage()
    {
        InitializeComponent();

        // Manual binding if not using DI, 
        // though DI is preferred for professional MVVM:
        // BindingContext = new LoginViewModel(new ApiService());
    }

    // Constructor for Dependency Injection (set up in MauiProgram.cs)
    public LoginPage(LoginViewModel viewModel)
    {
        InitializeComponent();
        BindingContext = viewModel;
    }
}