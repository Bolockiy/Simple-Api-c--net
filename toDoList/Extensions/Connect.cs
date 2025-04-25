using ApiToDo.Domain.Entities;
using Microsoft.AspNetCore.Diagnostics;
using NLog;
using System.Threading.Tasks;
using toDoList.Entities.UserAccount;
using toDoList.User;

namespace ApiLayer.Extensions
{
    public static class Connect
    {
        private static readonly Logger logger = LogManager.GetCurrentClassLogger();
        private const string _apiUrlUser = "https://localhost:7107/api/account/login";

        private static readonly HttpClient _httpClient = new HttpClient();
        private const string _baseApiUrl = "https://localhost:7107/api/";

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
                logger.Error("Отсутствует токен авторизации.");
                return null;
            }

            try
            {
                _httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);
                return await _httpClient.GetFromJsonAsync<List<UserAccount>>(_baseApiUrl + "user");
            }
            catch (Exception ex)
            {
                logger.Error($"Ошибка при получении списка пользователей: {ex.Message}");
                return null;
            }
        }
        public static async Task<UserAccount?> GetUserByNameAsync(string name, string token)
        {
            if (string.IsNullOrEmpty(token))
            {
                logger.Error("Отсутствует токен авторизации.");
                return null;
            }

            try
            {
                _httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);
                return await _httpClient.GetFromJsonAsync<UserAccount>($"{_baseApiUrl}user/name/{name}");
            }
            catch (Exception ex)
            {
                logger.Error($"Ошибка соединения: {ex.Message}");
                return null;
            }
        }

        public static async Task<UserAccount?> GetUserByIdAsync(int id, string token)
        {
            if (string.IsNullOrEmpty(token))
            {
                logger.Error("Отсутствует токен авторизации.");
                return null;
            }

            try
            {
                _httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);
                return await _httpClient.GetFromJsonAsync<UserAccount>($"{_baseApiUrl}user/id/{id}");
            }
            catch (Exception ex)
            {
                logger.Error($"Ошибка соединения: {ex.Message}");
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
                var response = await _httpClient.PostAsJsonAsync(_baseApiUrl + "user", user);
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
                var response = await _httpClient.PutAsJsonAsync($"{_baseApiUrl}user/{id}", user);
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
                var response = await _httpClient.DeleteAsync($"{_baseApiUrl}user/{id}");
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
                return await _httpClient.GetFromJsonAsync<List<ToDoTask>>(_baseApiUrl + "task");
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
                return await _httpClient.GetFromJsonAsync<ToDoTask>($"{_baseApiUrl}task/{id}");
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
                var response = await _httpClient.PostAsJsonAsync(_baseApiUrl + "task", task);
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
                var response = await _httpClient.PutAsJsonAsync($"{_baseApiUrl}task/{id}", task);
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
                var response = await _httpClient.DeleteAsync($"{_baseApiUrl}task/{id}");
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
