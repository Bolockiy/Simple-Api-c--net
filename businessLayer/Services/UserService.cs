using ApiToDo.Infrastructure.Data;
using Helper.Security;
using Microsoft.EntityFrameworkCore;
using NLog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using toDoList.Entities.UserAccount;

namespace BusinessLayer.Services
{
    public class UserService : ICrudService<UserAccount>
    {
        private readonly AppDbContext dbContext;
        private static readonly Logger logger = LogManager.GetCurrentClassLogger();

        public UserService(AppDbContext _dbContext) => dbContext = _dbContext;

        public async Task<bool> CreateAsync(UserAccount userAccount)
        {
            try
            {
                logger.Info("Попытка создания нового пользователя");

                if (string.IsNullOrWhiteSpace(userAccount.FullName) ||
                   string.IsNullOrWhiteSpace(userAccount.UserName) ||
                   string.IsNullOrWhiteSpace(userAccount.Password))
                {
                    logger.Warn("Попытка создания пользователя с пустыми полями.");
                    return false;
                }

                if (await dbContext.UserAccounts.AnyAsync(x => x.UserName == userAccount.UserName))
                {
                    logger.Warn($"Попытка создать дубликат пользователя с именем пользователя: {userAccount.UserName}");
                    return false;
                }

                userAccount.Password = PasswordHasher.HashPassword(userAccount.Password);
                await dbContext.UserAccounts.AddAsync(userAccount);
                await dbContext.SaveChangesAsync();
                logger.Info($"Пользователь с именем {userAccount.UserName} успешно создан.");
                return true;
            }
            catch (Exception ex)
            {
                logger.Error(ex, "Ошибка при создании пользователя.");
                return false;
            }
        }

        public async Task<bool> DeleteAsync(int id)
        {
            try
            {
                logger.Info($"Попытка удаления пользователя с ID: {id}");
                var user = await dbContext.UserAccounts.FindAsync(id);
                if (user == null)
                {
                    logger.Warn($"Пользователь с ID {id} не найден.");
                    return false;
                }

                dbContext.UserAccounts.Remove(user);
                await dbContext.SaveChangesAsync();
                logger.Info($"Пользователь с ID {id} успешно удален.");
                return true;
            }
            catch (Exception ex)
            {
                logger.Error(ex, $"Ошибка при удалении пользователя с ID {id}.");
                return false;
            }
        }

        public async Task<List<UserAccount>> GetAllAsync()
        {
            try
            {
                logger.Info("Получение списка всех пользователей.");
                var users = await dbContext.UserAccounts.ToListAsync();
                logger.Info($"Количество найденных пользователей: {users.Count}");
                return users;
            }
            catch (Exception ex)
            {
                logger.Error(ex, "Ошибка при получении списка пользователей.");
                return new List<UserAccount>();
            }
        }

        public async Task<UserAccount?> GetByIdAsync(int id)
        {
            try
            {
                logger.Info($"Получение пользователя с ID: {id}");
                var user = await dbContext.UserAccounts.FirstOrDefaultAsync(x => x.Id == id);
                if (user == null)
                {
                    logger.Warn($"Нет пользователя с ID: {id}");
                    return null;
                }
                logger.Info($"Пользователь с ID {id} найден.");
                return user;
            }
            catch (Exception ex)
            {
                logger.Error(ex, $"Ошибка при получении пользователя с ID: {id}");
                return null;
            }
        }

        public async Task<bool> UpdateAsync(int id, UserAccount userAccount)
        {
            try
            {
                logger.Info($"Попытка обновления пользователя с ID: {id}");

                var existingUser = await dbContext.UserAccounts.FindAsync(id);
                if (existingUser == null)
                {
                    logger.Warn($"Пользователь с ID {id} не найден.");
                    return false;
                }

                bool isUpdated = false;

                if (!string.IsNullOrWhiteSpace(userAccount.FullName) && existingUser.FullName != userAccount.FullName)
                {
                    existingUser.FullName = userAccount.FullName;
                    isUpdated = true;
                    logger.Info($"Обновлено поле FullName для пользователя с ID {id}. Новое значение: {userAccount.FullName}");
                }

                if (!string.IsNullOrWhiteSpace(userAccount.UserName) && existingUser.UserName != userAccount.UserName)
                {
                    existingUser.UserName = userAccount.UserName;
                    isUpdated = true;
                    logger.Info($"Обновлено поле UserName для пользователя с ID {id}. Новое значение: {userAccount.UserName}");
                }

                if (!string.IsNullOrWhiteSpace(userAccount.Password))
                {
                    string hashedPassword = PasswordHasher.HashPassword(userAccount.Password);
                    if (existingUser.Password != hashedPassword)
                    {
                        existingUser.Password = hashedPassword;
                        isUpdated = true;
                        logger.Info($"Обновлен пароль пользователя с ID {id}");
                    }
                }

                if (userAccount.Role.HasValue && existingUser.Role != userAccount.Role)
                {
                    existingUser.Role = userAccount.Role;
                    isUpdated = true;
                    logger.Info($"Обновлена роль пользователя с ID {id}. Новая роль: {userAccount.Role}");
                }

                if (isUpdated)
                {
                    existingUser.UpdateDate();
                    await dbContext.SaveChangesAsync();
                    logger.Info($"Пользователь с ID {id} успешно обновлен.");
                }
                else
                {
                    logger.Info($"Данные пользователя с ID {id} не изменились.");
                }

                return true;
            }
            catch (Exception ex)
            {
                logger.Error(ex, $"Ошибка при обновлении пользователя с ID {id}");
                return false;
            }
        }
    }
}
