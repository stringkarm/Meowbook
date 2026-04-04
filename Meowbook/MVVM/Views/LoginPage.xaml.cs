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

    private void OnToggleLoginPasswordClicked(object sender, EventArgs e)
    {
        if (LoginPasswordEntry != null && PasswordVisibilityButton != null)
        {
            // 1. Toggle the password visibility
            LoginPasswordEntry.IsPassword = !LoginPasswordEntry.IsPassword;

            // 2. Adjust Opacity based on visibility
            if (LoginPasswordEntry.IsPassword)
            {
                // PASSWORD IS UNSEEN (Hidden) -> Set Opacity to 100%
                PasswordVisibilityButton.Opacity = 1.0;
            }
            else
            {
                // PASSWORD IS SEEN (Visible) -> Set Opacity to 50% (or 0.5)
                PasswordVisibilityButton.Opacity = 0.5;
            }
        }
    }
}