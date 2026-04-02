using CommunityToolkit.Mvvm.ComponentModel;
using System.Text.Json.Serialization;

namespace Meowbook.Models
{
    public partial class Post : ObservableObject
    {
        [ObservableProperty]
        [property: JsonPropertyName("id")]
        private string _id;

        [ObservableProperty]
        [property: JsonPropertyName("userId")]
        private string _userId;

        [ObservableProperty]
        [property: JsonPropertyName("userName")]
        private string _userName;

        [ObservableProperty]
        [property: JsonPropertyName("userAvatar")] // Must match MockAPI exactly
        private string _userAvatar;

        [ObservableProperty]
        [property: JsonPropertyName("content")]
        private string _content;

        [ObservableProperty]
        [property: JsonPropertyName("imageUrl")] // Must be lowercase 'i' to match MockAPI
        private string _imageUrl;

        [ObservableProperty]
        [property: JsonPropertyName("likes")]
        private int _likes;
    }
}