using Cursa4.Models;

namespace Cursa4.Views;

public partial class Home : ContentPage
{
    private string _token;
    private User _user;

    public Home(User user, string token)
    {
        InitializeComponent();
        
       

        if (user == null)
        {
            throw new ArgumentNullException(nameof(user), "Пользователь не определен");
        }

         _user = user;
        _token = token;

        UserNameLabel.Text = user.Name;

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