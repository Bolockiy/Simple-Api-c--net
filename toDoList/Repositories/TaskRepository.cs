using ApiToDo.Application.Repositories;
using ApiToDo.Domain.Entities;
using ApiToDo.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace ApiToDo.Infrastructure.Repositories
{
    public class TaskRepository : ITaskRepository
    {
        private readonly AppDbContext _context;
        public TaskRepository(AppDbContext context) => _context = context;

        public async Task<List<ToDoTask>> GetAllAsync() => await _context.Tasks.ToListAsync();

        public async Task<ToDoTask?> GetByIdAsync(int id) => await _context.Tasks.FindAsync(id);

        public async Task AddAsync(ToDoTask task)
        {
            _context.Tasks.Add(task);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(ToDoTask task)
        {
            _context.Tasks.Update(task);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var task = await _context.Tasks.FindAsync(id);
            if (task != null)
            {
                _context.Tasks.Remove(task);
                await _context.SaveChangesAsync();
            }
        }
    }
}
