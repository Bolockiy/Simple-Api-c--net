using System.Net.Http.Json;
using System.Threading.Tasks;

namespace testToDo
{
    namespace ToDoApiClient
    {
        public class ToDoTask
        {
            public int Id { get; set; }
            public string? Title { get; set; }
            public string? Description { get; set; }
            public bool IsCompleted { get; set; }
            public DateTime CreatedAt { get; set; }
            public DateTime? CompletedAt { get; set; }
        }

        public class ToDoApiClient
        {
            private readonly HttpClient _httpClient;

            public ToDoApiClient()
            {
                _httpClient = new HttpClient { BaseAddress = new Uri("https://localhost:7107/api/") };
            }

            // Получить все задачи
            public async Task<List<ToDoTask>> GetTasksAsync()
            {
                var response = await _httpClient.GetFromJsonAsync<List<ToDoTask>>("Task");
                return response ?? new List<ToDoTask>();
            }

            // Создать задачу
            public async Task CreateTaskAsync(ToDoTask task)
            {
                var response = await _httpClient.PostAsJsonAsync("Task", task);
                response.EnsureSuccessStatusCode();
            }

            // Обновить задачу
            public async Task UpdateTaskAsync(int id, ToDoTask task)
            {
                var response = await _httpClient.PutAsJsonAsync($"Task/{id}", task);
                response.EnsureSuccessStatusCode();
            }

            // Удалить задачу
            public async Task DeleteTaskAsync(int id)
            {
                var response = await _httpClient.DeleteAsync($"Task/{id}");
                response.EnsureSuccessStatusCode();
            }
        }
    }
}
