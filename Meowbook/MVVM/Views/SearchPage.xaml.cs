using Microsoft.Maui.Controls;
using Meowbook.ViewModels;

namespace Meowbook.MVVM.Views
{
    public partial class SearchPage : ContentPage
    {
        public SearchPage()
        {
            InitializeComponent();
            BindingContext = new SearchViewModel();
        }
    }
}