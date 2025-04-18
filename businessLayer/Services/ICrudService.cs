using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using toDoList.Entities.UserAccount;

namespace BusinessLayer.Services
{
    public interface ICrudService<T>
    {
        Task<List<T>> GetAllAsync();
        Task<T?> GetByIdAsync(int id);
        Task<bool> CreateAsync(T obj);
        Task<bool> UpdateAsync(int id, T obj);
        Task<bool> DeleteAsync(int id);
        Task<T?> GetByUserNameAsync(string text);
    }
}
