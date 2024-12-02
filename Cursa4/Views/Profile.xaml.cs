
using Cursa4.Models;
using Cursa4.Views.Users;
using System.Collections.ObjectModel;
using System.Net.Http.Json;
using System.Text.Json;

namespace Cursa4.Views;

public partial class Profile : ContentPage
{
    private readonly HttpClient _httpClient = new HttpClient();
    private User _user;
    private string _token;
    public ObservableCollection<User> Users { get; set; } = new ObservableCollection<User>();


    public Profile(User user, string token)
    {
        InitializeComponent();

        _user = user;
        _token = token;

        UsersCollectionView.ItemsSource = Users;
        LoadUsers();
    }
    private async void LoadUsers()
    {
        try
        {
            var token = Preferences.Get("UserToken", string.Empty);

            _httpClient.DefaultRequestHeaders.Authorization =
                new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);

            var response = await _httpClient.GetAsync("http://courseproject4/api/profile");

            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();
                var users = JsonSerializer.Deserialize<List<User>>(content);

                Users.Clear();
                foreach (var user in users)
                {
                    Users.Add(user);
                }
            }
            else
            {
                var errorContent = await response.Content.ReadAsStringAsync();
                await DisplayAlert("Îøèáêà", $"Êîä: {response.StatusCode}, Îòâåò: {errorContent}", "ÎÊ");
            }
        }
        catch (Exception ex)
        {
            await DisplayAlert("Îøèáêà", $"Ïðîèçîøëà îøèáêà: {ex.Message}", "ÎÊ");
        }
    }
    private async void ButtonUserEdit(object sender, EventArgs e)
    {
        await Navigation.PushAsync(new EditProfile(_user, _token));
    }

}