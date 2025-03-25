using ApiToDo.Domain.Entities;

namespace ApiToDo.Application.Repositories
{
    public interface ITaskRepository
    {
        Task<List<ToDoTask>> GetAllAsync();
        Task<ToDoTask?> GetByIdAsync(int id);
        Task AddAsync(ToDoTask task);
        Task UpdateAsync(ToDoTask task);
        Task DeleteAsync(int id);
    }
}
