using ApiToDo.Domain.Entities;
using ApiToDo.Infrastructure.Data;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;
using NLog;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BusinessLayer.Services
{
    public class TaskService : ICrudService<ToDoTask>
    {
        private readonly AppDbContext dbContext;
        private static readonly Logger logger = LogManager.GetCurrentClassLogger();

        public TaskService(AppDbContext _dbContext) => dbContext = _dbContext;

        public async Task<bool> CreateAsync(ToDoTask obj)
        {
            try
            {
                var task = new ToDoTask
                {
                    Title = obj.Title,
                    Description = obj.Description,
                    CreatedAt = DateTime.UtcNow
                };

                if (string.IsNullOrWhiteSpace(task.Title))
                {
                    logger.Warn("Попытка создания задачи с пустым названием.");
                    return false;
                }

                await dbContext.Tasks.AddAsync(task);
                await dbContext.SaveChangesAsync();

                logger.Info($"Задача с названием '{task.Title}' успешно создана.");
                return true;
            }
            catch (Exception ex)
            {
                logger.Error(ex, "Ошибка при создании задачи.");
                return false;
            }
        }


        public async Task<bool> DeleteAsync(int id)
        {
            try
            {
                logger.Info($"Попытка удаления задачи с ID: {id}");
                var task = await dbContext.Tasks.FindAsync(id);
                if (task == null)
                {
                    logger.Warn($"Задача с ID {id} не найдена.");
                    return false;
                }

                dbContext.Tasks.Remove(task);
                await dbContext.SaveChangesAsync();
                logger.Info($"Задача с ID {id} успешно удалена.");
                return true;
            }
            catch (Exception ex)
            {
                logger.Error(ex, $"Ошибка при удалении задачи с ID {id}");
                return false;
            }
        }

        public async Task<List<ToDoTask>> GetAllAsync()
        {
            try
            {
                logger.Info("Получение списка всех задач.");
                var tasks = await dbContext.Tasks.ToListAsync();
                logger.Info($"Количество задач: {tasks.Count}");
                return tasks;
            }
            catch (Exception ex)
            {
                logger.Error(ex, "Ошибка при получении списка задач.");
                return new List<ToDoTask>();
            }
        }

        public async Task<ToDoTask?> GetByIdAsync(int id)
        {
            try
            {
                logger.Info($"Запрос на получение задачи с ID: {id}");
                var task = await dbContext.Tasks.FirstOrDefaultAsync(x => x.Id == id);
                if (task == null)
                {
                    logger.Warn($"Задача с ID {id} не найдена.");
                    return null;
                }
                logger.Info($"Задача с ID {id} найдена.");
                return task;
            }
            catch (Exception ex)
            {
                logger.Error(ex, $"Ошибка при получении задачи с ID: {id}");
                return null;
            }
        }

        public async Task<bool> UpdateAsync(int id, ToDoTask obj)
        {
            try
            {
                if (id != obj.Id)
                {
                    logger.Warn($"Несоответствие ID задачи: ID в маршруте {id} не совпадает с ID задачи {obj.Id}.");
                    return false;
                }

                var existingTask = await dbContext.Tasks.FindAsync(id);
                if (existingTask == null)
                {
                    logger.Warn($"Попытка обновления несуществующей задачи с ID: {id}");
                    return false;
                }

                existingTask.Update(obj.Title, obj.Description, obj.IsCompleted);

                logger.Info($"Обновление задачи с ID: {id}");
                await dbContext.SaveChangesAsync();

                logger.Info($"Задача с ID {id} успешно обновлена.");
                return true;
            }
            catch (Exception ex)
            {
                logger.Error(ex, $"Ошибка при обновлении задачи с ID {id}");
                return false;
            }
        }

        public async Task<ToDoTask?> GetByUserNameAsync(string text)
        {
            return null;
        }
    }
}
