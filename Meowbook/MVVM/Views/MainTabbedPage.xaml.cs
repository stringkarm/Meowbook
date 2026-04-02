namespace Meowbook.Views;

public partial class MainTabbedPage : TabbedPage
{
    public MainTabbedPage()
    {
        InitializeComponent();

        // This removes the navigation bar from the TabbedPage itself 
        // so the child pages can have their own bars
        NavigationPage.SetHasNavigationBar(this, false);
    }
}