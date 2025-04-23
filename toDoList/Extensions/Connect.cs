using toDoList.Entities.UserAccount;
using toDoList.User;

namespace ApiLayer.Extensions
{
    public static class Connect
    {
        private const string _apiUrlUser = "http://localhost:5215/api/account/login";
        private static readonly HttpClient _httpClient = new HttpClient();
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
                    Console.WriteLine($"Ошибка авторизации: {errorContent}");
                    return null;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ошибка соединения: {ex.Message}");
                return null;
            }
        }
    }
}
