using Meowbook.ViewModels;

namespace Meowbook.Views;

public partial class RegistrationPage : ContentPage
{
    public RegistrationPage(RegistrationViewModel viewModel)
    {
        InitializeComponent();
        BindingContext = viewModel;
    }

    private async void OnBackClicked(object sender, EventArgs e)
    {
        await Navigation.PopAsync();
    }

    private void OnToggleRegPasswordClicked(object sender, EventArgs e)
    {
        if (RegPasswordEntry != null)
        {
            RegPasswordEntry.IsPassword = !RegPasswordEntry.IsPassword;
        }
    }
}