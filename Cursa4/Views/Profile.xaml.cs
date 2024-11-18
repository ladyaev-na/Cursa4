using Cursa4.Models;
using System;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.Maui.Controls;

namespace Cursa4.Views;

public partial class Profile : ContentPage
{
    private readonly HttpClient _httpClient = new HttpClient();
    private User _user;
    private string _token;

    public Profile(User user, string token)
    {
        InitializeComponent();

        _user = user;
        _token = token;

        name.Text = user.Name;
        surname.Text = user.Surname;
        patronymic.Text = user.Patronymic;
        login.Text = user.Login;

        editName.Text = user.Name;
        editSurname.Text = user.Surname;
        editPatronymic.Text = user.Patronymic;
        editLogin.Text = user.Login;
    }

    private async void OnSaveButtonClicked(object sender, EventArgs e)
    {
        _user.Name = editName.Text;
        _user.Surname = editSurname.Text;
        _user.Patronymic = editPatronymic.Text;
        _user.Login = editLogin.Text;

        // Отправка запроса на сервер
        await UpdateProfile(_user);
    }

    private async Task UpdateProfile(User user)
    {
        // Формирование тела для отправки
        var jsonContent = new StringContent(JsonSerializer.Serialize(user), Encoding.UTF8, "application/json");
        Console.WriteLine(JsonSerializer.Serialize(user)); // Отладочный вывод

        try
        {
            // Установка заголовка авторизации
            _httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", _token);

            // Формирование URL
            var url = $"http://courseproject4/api/profile/{user.Id}";

            HttpResponseMessage response = await _httpClient.PostAsync(url, jsonContent);

            if (response.StatusCode == System.Net.HttpStatusCode.OK)
            {
                var responseContent = await response.Content.ReadAsStringAsync();
                var updatedUser = JsonSerializer.Deserialize<User>(responseContent);

                await DisplayAlert("Успех", "Профиль успешно обновлен.", "OK");
            }
            else
            {
                await DisplayAlert("Ошибка", "Не удалось обновить профиль.", "OK");
            }
        }
        catch (Exception ex)
        {
            await DisplayAlert("Ошибка сети", ex.Message, "ОК");
        }
    }
}