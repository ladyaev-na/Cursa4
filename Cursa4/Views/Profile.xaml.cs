using Cursa4.Models;

namespace Cursa4.Views;

public partial class Profile : ContentPage
{
    private User _user;
    private string _token;

    public Profile(User user, string token)
    {
        InitializeComponent();

        if (token == null)
        {
            throw new ArgumentNullException(nameof(user), "������������ �� ���������");
        }

        _user = user;
        _token = token;

        name.Text = user.Name;
    }
}