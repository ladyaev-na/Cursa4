using System.Text.Json;
using System.Text;
using Cursa4.Models;

namespace Cursa4.Views;

public partial class Login : ContentPage
{
    private readonly HttpClient _httpClient = new HttpClient();

    public Login()
    {
        InitializeComponent();
    }

    private async void OnLoginButtonClicked(object sender, EventArgs e)
    {
        string login = LoginEntry.Text;
        string password = PasswordEntry.Text;

        if (string.IsNullOrEmpty(login) || string.IsNullOrEmpty(password))
        {
            await DisplayAlert("������", "������� ����� � ������", "OK");
            return;
        }

        var loginResponse = await AuthenticateUserAsync(login, password);
        if (loginResponse != null)
        {
            await Navigation.PushAsync(new Home(loginResponse.User, loginResponse.Token));
        }
    }

    private async Task<AuthResponse> AuthenticateUserAsync(string login, string password)
    {
        // ������������ ���� ��� ��������
        var loginData = new { login, password };
        // �������������� ������ � JSON ��� ��������
        var jsonContent = new StringContent(JsonSerializer.Serialize(loginData), Encoding.UTF8, "application/json");

        // ������ �������
        try
        {
            // ���������� ������ � ���������� ����� � response
            HttpResponseMessage response = await _httpClient.PostAsync("http://courseproject4/api/login", jsonContent);

            // ���� ����� ����� 200
            if (response.StatusCode == System.Net.HttpStatusCode.OK)
            {
                var content = await response.Content.ReadAsStringAsync();
                var result = JsonSerializer.Deserialize<AuthResponse>(content);

                if (result?.Token != null)
                {
                    // ���������� ���� ������, ������� ������ ������������ � �����
                    return result;
                }
                else
                {
                    await DisplayAlert("������", "�� ������� �������� �����", "OK");
                }
            }
            // ���� ����� 401
            else if (response.StatusCode == System.Net.HttpStatusCode.Unauthorized)
            {
                await DisplayAlert("������ �����", "������������ ����� ��� ������", "OK");
            }
            else
            {
                await DisplayAlert("������", "��������� ������ �� �������", "OK");
            }
        }
        catch (Exception ex)
        {
            await DisplayAlert("������ ����", ex.Message, "��");
        }
        return null;
    }
}