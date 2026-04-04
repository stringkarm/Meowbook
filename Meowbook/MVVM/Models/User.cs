using CommunityToolkit.Mvvm.ComponentModel;
using System.Text.Json.Serialization;

namespace Meowbook.Models
{
    public partial class User : ObservableObject
    {
        [ObservableProperty]
        [property: JsonPropertyName("id")]
        private string _id;

        [ObservableProperty]
        [property: JsonPropertyName("name")]
        private string _name;

        [ObservableProperty]
        [property: JsonPropertyName("username")]
        private string _username;

        [ObservableProperty]
        [property: JsonPropertyName("password")]
        private string _password;

        [ObservableProperty]
        [property: JsonPropertyName("avatar")]
        private string _avatar;

        [ObservableProperty]
        [property: JsonPropertyName("bio")]
        private string _bio;

        [ObservableProperty]
        [property: JsonPropertyName("friendsList")]
        private string _friendsList;

        [ObservableProperty]
        [property: JsonPropertyName("birthdate")]
        private string _birthdate;

        [ObservableProperty]
        [property: JsonPropertyName("notifications")]
        private string _notifications;

        [ObservableProperty]
        [property: JsonPropertyName("isActive")]
        private bool _isActive;

        [ObservableProperty]
        [property: JsonPropertyName("email")]
        private string _email;
    }
}