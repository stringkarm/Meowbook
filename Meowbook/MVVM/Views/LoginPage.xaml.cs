using Meowbook.ViewModels;
using Meowbook.Services;

namespace Meowbook.Views;

public partial class LoginPage : ContentPage
{
    public LoginPage()
    {
        InitializeComponent();
        BindingContext = new LoginViewModel(new Meowbook.Services.ApiService());
    }

    // Constructor for Dependency Injection (set up in MauiProgram.cs)
    public LoginPage(LoginViewModel viewModel)
    {
        InitializeComponent();
        BindingContext = viewModel;
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();
        // Slide up from slight offset + fade in
        this.Opacity = 0;
        this.TranslationY = 30;
        await Task.WhenAll(
            this.FadeTo(1, 400, Easing.CubicOut),
            this.TranslateTo(0, 0, 400, Easing.CubicOut)
        );
    }
}