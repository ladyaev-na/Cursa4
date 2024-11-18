namespace Cursa4.Views;

public partial class Home : ContentPage
{
    private string _token;
    private readonly HttpClient _httpClient = new HttpClient();
    public Home(Models.User user, string token)
	{
		InitializeComponent();
        _token = token;
    }
}