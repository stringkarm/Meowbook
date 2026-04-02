using Meowbook.Views;

namespace Meowbook 
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();
            MainPage = new Meowbook.Views.SplashPage();
        }
    }
}