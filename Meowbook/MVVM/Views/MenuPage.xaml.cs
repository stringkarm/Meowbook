namespace Meowbook.Views;

public partial class MenuPage : ContentPage
{
    public MenuPage()
    {
        InitializeComponent();
    }

    private void OnLogoutClicked(object sender, EventArgs e)
    {
        // Go back to Login Page
        Application.Current.MainPage = new NavigationPage(new LoginPage());
    }
}