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
            await DisplayAlert("Ошибка", "Введите логин и пароль", "OK");
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
        // Формирование тела для отправки
        var loginData = new { login, password };
        // Преобразование данных в JSON для отправки
        var jsonContent = new StringContent(JsonSerializer.Serialize(loginData), Encoding.UTF8, "application/json");

        // Запрос серверу
        try
        {
            // Отправляем запрос и записываем ответ в response
            HttpResponseMessage response = await _httpClient.PostAsync("http://courseproject4/api/login", jsonContent);

            // Если ответ успех 200
            if (response.StatusCode == System.Net.HttpStatusCode.OK)
            {
                var content = await response.Content.ReadAsStringAsync();
                var result = JsonSerializer.Deserialize<AuthResponse>(content);

                if (result?.Token != null)
                {
                    // Возвращаем весь объект, включая данные пользователя и токен
                    return result;
                }
                else
                {
                    await DisplayAlert("Ошибка", "Не удалось получить токен", "OK");
                }
            }
            // Если ответ 401
            else if (response.StatusCode == System.Net.HttpStatusCode.Unauthorized)
            {
                await DisplayAlert("Ошибка входа", "Неправильный логин или пароль", "OK");
            }
            else
            {
                await DisplayAlert("Ошибка", "Произошла ошибка на сервере", "OK");
            }
        }
        catch (Exception ex)
        {
            await DisplayAlert("Ошибка сети", ex.Message, "ОК");
        }
        return null;
    }
}