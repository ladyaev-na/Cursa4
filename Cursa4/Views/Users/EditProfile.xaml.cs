using Cursa4.Models;
using System.Text.Json;
using System.Text;
using System.Xml;

namespace Cursa4.Views.Users;

public partial class EditProfile : ContentPage
{
    private readonly HttpClient _httpClient = new HttpClient();
    private User _user;
    private string _token;

    public EditProfile(User user, string token)
    {
        InitializeComponent();

        _user = user;
        _token = token;

        surnameLabel.Text = user.Surname;
        nameLabel.Text = user.Name;
        patronymicLabel.Text = user.Patronymic ?? "";
        loginLabel.Text = user.Login;
        passwordLabel.Text = user.Password;

    }

    private async void OnSaveButtonClicked(object sender, EventArgs e)
    {
        // �������� �� ������ ����
        if (string.IsNullOrWhiteSpace(surnameLabel.Text) ||
            string.IsNullOrWhiteSpace(nameLabel.Text) ||
            string.IsNullOrWhiteSpace(loginLabel.Text) ||
            string.IsNullOrWhiteSpace(passwordLabel.Text))
        {
            await DisplayAlert("������", "��� ������������ ���� ������ ���� ���������", "��");
            return;
        }

        if (passwordLabel.Text != confirmPassword.Text)
        {
            await DisplayAlert("������", "������ �� ���������", "��");
            return;
        }

        // ��������� ������ ��� ����������
        var updatedUser = new
        {
            Surname = surnameLabel.Text,
            Name = nameLabel.Text,
            Patronymic = string.IsNullOrEmpty(patronymicLabel.Text) ? null : patronymicLabel.Text,
            Login = loginLabel.Text,
            Password = passwordLabel.Text
        };

        // ����������� ������������ ��� �������������� ������ � ������ �������
        var options = new JsonSerializerOptions
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase // ����������� ����� � camelCase
        };
        var jsonContent = new StringContent(JsonSerializer.Serialize(updatedUser, options), Encoding.UTF8, "application/json");

        // ����������� ������ ����� ���������
        Console.WriteLine($"Sending data: {JsonSerializer.Serialize(updatedUser, options)}");

        // ������ �������
        try
        {
            // ������������� ��������� �����������
            _httpClient.DefaultRequestHeaders.Authorization =
                new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", _token);

            // ���������� PUT-������
            HttpResponseMessage response = await _httpClient.PutAsync($"http://courseproject4/api/profile/{_user.Id}", jsonContent);

            // ����������� ������
            var responseContent = await response.Content.ReadAsStringAsync();
            Console.WriteLine($"Response: {responseContent}");

            if (response.StatusCode == System.Net.HttpStatusCode.OK)
            {
                // ��������� ������ ������������
                _user.Surname = updatedUser.Surname;
                _user.Name = updatedUser.Name;
                _user.Patronymic = updatedUser.Patronymic;
                _user.Login = updatedUser.Login;
                _user.Password = updatedUser.Password;

                // ��������� ������ � Preferences
                Preferences.Set("UserSurname", updatedUser.Surname);
                Preferences.Set("UserName", updatedUser.Name);
                Preferences.Set("UserPatronymic", updatedUser.Patronymic ?? "");
                Preferences.Set("UserLogin", updatedUser.Login);
                Preferences.Set("UserPassword", updatedUser.Password);

                // ��������� UI
                surnameLabel.Text = updatedUser.Surname;
                nameLabel.Text = updatedUser.Name;
                patronymicLabel.Text = updatedUser.Patronymic ?? "";
                loginLabel.Text = updatedUser.Login;
                passwordLabel.Text = updatedUser.Password;

                await DisplayAlert("�����", "������� ������� ��������!", "OK");
                await Navigation.PushAsync(new Profile(_user, _token));
            }
            else if (response.StatusCode == System.Net.HttpStatusCode.Unauthorized)
            {
                await DisplayAlert("������", "������ �������. ������������� �����.", "OK");
                await Navigation.PushAsync(new Views.Login());
            }
            else
            {
                await DisplayAlert("������", $"�� ������� �������� �������: {responseContent}", "OK");
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Exception: {ex.Message}");
            await DisplayAlert("������", $"��������� ������: {ex.Message}", "OK");
        }
    }
}