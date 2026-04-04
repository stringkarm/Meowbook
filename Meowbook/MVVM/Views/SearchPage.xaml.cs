using Microsoft.Maui.Controls;
using Meowbook.ViewModels;
using System;

namespace Meowbook.Views
{
    public partial class SearchPage : ContentPage
    {
        public SearchPage()
        {
            InitializeComponent();
            BindingContext = new SearchViewModel();
        }

        private async void OnUserSelected(object sender, EventArgs e)
        {
            if (sender is VisualElement element && element.BindingContext is Meowbook.Models.User user)
            {
                await Meowbook.Services.GlobalState.NavigateToProfile(user.Id);
            }
        }
    }
}