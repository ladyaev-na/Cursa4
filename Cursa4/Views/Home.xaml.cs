using Cursa4.Models;

namespace Cursa4.Views;

public partial class Home : ContentPage
{
    private string _token;
    private User _user;

    public Home(User user, string token)
    {
        InitializeComponent();

        _user = user;
        _token = token;
    }

    private async void OnImageTapped(object sender, TappedEventArgs e)
    {
        await Navigation.PushAsync(new Profile(_user, _token));
    }

    private async void OnLinkTapped(object sender, TappedEventArgs e)
    {
        await Launcher.OpenAsync(new Uri("https://lk.mysbertips.ru/"));
    }
}