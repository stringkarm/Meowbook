using System.Windows.Input;
using Meowbook.Models;
using Meowbook.Services;

namespace Meowbook.ViewModels
{
    public class PostViewModel : BindableObject
    {
        private readonly ApiService _apiService;
        private string _content;
        private string _imageUrl;
        private bool _isBusy;

        public string Content
        {
            get => _content;
            set { _content = value; OnPropertyChanged(); }
        }

        public string ImageUrl
        {
            get => _imageUrl;
            set { _imageUrl = value; OnPropertyChanged(); }
        }

        public bool IsBusy
        {
            get => _isBusy;
            set { _isBusy = value; OnPropertyChanged(); }
        }

        public ICommand SharePostCommand { get; }
        public ICommand CancelCommand { get; }

        public PostViewModel(ApiService apiService)
        {
            _apiService = apiService;

            SharePostCommand = new Command(async () => await ExecuteSharePost());
            CancelCommand = new Command(async () => await Shell.Current.GoToAsync(".."));
        }

        private async Task ExecuteSharePost()
        {
            if (string.IsNullOrWhiteSpace(Content))
            {
                await Application.Current.MainPage.DisplayAlert("Wait!", "Your cat post needs a caption!", "Meow");
                return;
            }

            IsBusy = true;

            try
            {
                // Create the new Post object based on your Model
                var newPost = new Post
                {
                    UserName = "Cat Parent", // In a real app, you'd pull the logged-in user's name
                    Content = Content,
                    ImageUrl = string.IsNullOrWhiteSpace(ImageUrl) ? "default_cat.png" : ImageUrl,
                    Likes = 0
                };

                // Send to MockAPI (The 'C' in CRUD)
                bool success = await _apiService.CreatePostAsync(newPost);

                if (success)
                {
                    await Application.Current.MainPage.DisplayAlert("Success!", "Your cat moment is now live!", "Yay");

                    // Clear fields and go back
                    Content = string.Empty;
                    ImageUrl = string.Empty;
                    await Shell.Current.GoToAsync("..");
                }
                else
                {
                    await Application.Current.MainPage.DisplayAlert("Error", "Could not post. The cat pulled the plug.", "OK");
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Posting Error: {ex.Message}");
            }
            finally
            {
                IsBusy = false;
            }
        }
    }
}