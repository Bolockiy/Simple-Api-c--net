using ApiToDo.Domain.Entities;
using Microsoft.AspNetCore.Diagnostics;
using NLog;
using System.Net.Http.Headers;
using System.Net.Http;
using System.Threading.Tasks;
using System.Xml.Linq;
using toDoList.Entities.UserAccount;
using toDoList.User;

namespace ApiLayer.Extensions
{
    public static class Connect
    {
        private const string Url = "https://localhost:7107/api/";
        private const string _apiUrlUser = "https://localhost:7107/api/account/login";
        private static HttpClient _httpClient = new HttpClient();
        private static Logger logger = LogManager.GetCurrentClassLogger();
        public static async Task<LoginResponse?> LoginAsync(string username, string password)
        {
            try
            {
                var request = new LoginRequest
                {
                    UserName = username,
                    Password = password
                };

                var response = await _httpClient.PostAsJsonAsync(_apiUrlUser, request);

                if (response.IsSuccessStatusCode)
                {
                    return await response.Content.ReadFromJsonAsync<LoginResponse>();
                }
                else
                {
                    var errorContent = await response.Content.ReadAsStringAsync();
                    logger.Error($"Ошибка авторизации: {errorContent}");
                    return null;
                }
            }
            catch (Exception ex)
            {
                logger.Error($"Ошибка соединения: {ex.Message}");
                return null;
            }
        }

        public static async Task<List<UserAccount>?> GetAllUsersAsync(string token)
        {
            if (string.IsNullOrEmpty(token))
            {
                logger.Error("Пустой токен");
                return null;
            }
            try
            {
                _httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bareer", token);
                return await _httpClient.GetFromJsonAsync<List<UserAccount>>(Url + "user");
            }
            catch (Exception ex)
            {
                logger.Error($"Ошибка при получении списка пользователей: {ex.Message}");
                return null;
            }
        }

        public static async Task<UserAccount?> GetUserByNameAsync(string name, string token)
        {
            if (string.IsNullOrEmpty(name) || string.IsNullOrEmpty(token))
            {
                logger.Error("Пустой токен или имя");
                return null;
            }
            try
            {
                _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

                return await _httpClient.GetFromJsonAsync<UserAccount>($"{Url}user/name/{name}");
            }
            catch (Exception ex)
            {
                logger.Error($"Ошибка при получении пользователя: {ex.Message}");
                return null;
            }

        }

        public static async Task<UserAccount?> GetUserByIdAsync(int id, string token)
        {
            if (string.IsNullOrEmpty(token))
            {
                logger.Error("Пустой токен");
                return null;
            }
            try
            {
                _httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bareer", token);
                return await _httpClient.GetFromJsonAsync<UserAccount>($"{Url}user/id/{id}");
            }
            catch (Exception ex)
            {
                logger.Error($"Ошибка при получении пользователя: {ex.Message}");
                return null;
            }
        }

        public static async Task<bool> CreateUserAsync(UserAccount user, string token)
        {
            if (string.IsNullOrEmpty(token))
            {
                logger.Error("Отсутствует токен авторизации.");
                return false;
            }
            try
            {
                _httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);
                var response = await _httpClient.PostAsJsonAsync(Url + "user", user);
                return response.IsSuccessStatusCode;
            }
            catch (Exception ex)
            {
                logger.Error($"Ошибка создания пользователя: {ex.Message}");
                return false;
            }
        }

        public static async Task<bool> UpdateUserAsync(int id, UserAccount user, string token)
        {
            if (string.IsNullOrEmpty(token))
            {
                logger.Error("Отсутствует токен авторизации.");
                return false;
            }

            try
            {
                _httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);
                var response = await _httpClient.PutAsJsonAsync($"{Url}user/{id}", user);
                return response.IsSuccessStatusCode;
            }
            catch (Exception ex)
            {
                logger.Error($"Ошибка обновления пользователя: {ex.Message}");
                return false;
            }
        }

        public static async Task<bool> DeleteUserAsync(int id, string token)
        {
            if (string.IsNullOrEmpty(token))
            {
                logger.Error("Отсутствует токен авторизации.");
                return false;
            }

            try
            {
                _httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);
                var response = await _httpClient.DeleteAsync($"{Url}user/{id}");
                return response.IsSuccessStatusCode;
            }
            catch (Exception ex)
            {
                logger.Error($"Ошибка удаления пользователя: {ex.Message}");
                return false;
            }
        }

        public static async Task<List<ToDoTask>?> GetTasksAsync(string token)
        {
            if (string.IsNullOrEmpty(token))
            {
                logger.Error("Отсутствует токен авторизации.");
                return null;
            }

            try
            {
                _httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);
                return await _httpClient.GetFromJsonAsync<List<ToDoTask>>(Url + "task");
            }
            catch (Exception ex)
            {
                logger.Error($"Ошибка при получении списка задач: {ex.Message}");
                return null;
            }
        }

        public static async Task<ToDoTask?> GetTaskByIdAsync(int id, string token)
        {
            if (string.IsNullOrEmpty(token))
            {
                logger.Error("Отсутствует токен авторизации.");
                return null;
            }

            try
            {
                _httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);
                return await _httpClient.GetFromJsonAsync<ToDoTask>($"{Url}task/{id}");
            }
            catch (Exception ex)
            {
                logger.Error($"Ошибка при получении задачи с id={id}: {ex.Message}");
                return null;
            }
        }

        public static async Task<bool> CreateTaskAsync(ToDoTask task, string token)
        {
            if (string.IsNullOrEmpty(token))
            {
                logger.Error("Отсутствует токен авторизации.");
                return false;
            }

            try
            {
                _httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);
                var response = await _httpClient.PostAsJsonAsync(Url + "task", task);
                return response.IsSuccessStatusCode;
            }
            catch (Exception ex)
            {
                logger.Error($"Ошибка при создании задачи: {ex.Message}");
                return false;
            }
        }

        public static async Task<bool> UpdateTaskAsync(int id, ToDoTask task, string token)
        {
            if (string.IsNullOrEmpty(token))
            {
                logger.Error("Отсутствует токен авторизации.");
                return false;
            }

            try
            {
                _httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);
                var response = await _httpClient.PutAsJsonAsync($"{Url}task/{id}", task);
                return response.IsSuccessStatusCode;
            }
            catch (Exception ex)
            {
                logger.Error($"Ошибка при обновлении задачи с id={id}: {ex.Message}");
                return false;
            }
        }

        public static async Task<bool> DeleteTaskAsync(int id, string token)
        {
            if (string.IsNullOrEmpty(token))
            {
                logger.Error("Отсутствует токен авторизации.");
                return false;
            }

            try
            {
                _httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);
                var response = await _httpClient.DeleteAsync($"{Url}task/{id}");
                return response.IsSuccessStatusCode;
            }
            catch (Exception ex)
            {
                logger.Error($"Ошибка при удалении задачи с id={id}: {ex.Message}");
                return false;
            }
        }
    }
}