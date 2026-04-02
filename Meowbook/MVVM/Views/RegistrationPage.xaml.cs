using Meowbook.ViewModels;

namespace Meowbook.Views;

public partial class RegistrationPage : ContentPage
{
    // Using the same ViewModel or a dedicated RegistrationViewModel
    public RegistrationPage(LoginViewModel viewModel)
    {
        InitializeComponent();
        BindingContext = viewModel;
    }

    private async void OnLoginClicked(object sender, EventArgs e)
    {
        await Navigation.PopAsync();
    }
}