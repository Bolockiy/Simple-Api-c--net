using ApiToDo.Domain.Entities;
using ApiToDo.Infrastructure.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NLog;
using System.Data;
using toDoList.Entities.UserAccount;
using toDoList.Security;

namespace toDoList.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class UserController : ControllerBase
    {
        private static readonly Logger logger = LogManager.GetCurrentClassLogger();
        private readonly AppDbContext _dbContext;

        public UserController(AppDbContext dbContext)
        {
            logger.Info("UserController инициализирован.");
            _dbContext = dbContext;
        }

        [HttpGet]
        public async Task<List<UserAccount>> Get()
        {
            logger.Info("Получение списка пользователей.");
            return await _dbContext.UserAccounts.ToListAsync();
        }

        [HttpGet("{id}")]
        public async Task<UserAccount?> GetById(int id)
        {
            logger.Info($"Получение пользователя по ID: {id}");
            return await _dbContext.UserAccounts.FirstOrDefaultAsync(x => x.Id == id);
        }

        [Authorize(Roles = "1")]
        [HttpPost]
        public async Task<ActionResult> Create([FromBody] UserAccount userAccount)
        {
            if (string.IsNullOrWhiteSpace(userAccount.FullName) ||
                string.IsNullOrWhiteSpace(userAccount.UserName) ||
                string.IsNullOrWhiteSpace(userAccount.Password))
            {
                logger.Warn("Попытка создания пользователя с пустыми полями.");
                return BadRequest("Все поля должны быть заполнены.");
            }

            if (await _dbContext.UserAccounts.AnyAsync(x => x.UserName == userAccount.UserName))
            {
                logger.Warn($"Попытка создать дубликат пользователя: {userAccount.UserName}");
                return Conflict("Пользователь уже существует.");
            }

            userAccount.Password = PasswordHasher.HashPassword(userAccount.Password);
            await _dbContext.UserAccounts.AddAsync(userAccount);
            await _dbContext.SaveChangesAsync();

            logger.Info($"Создан новый пользователь: {userAccount.UserName}");
            return CreatedAtAction(nameof(GetById), new { id = userAccount.Id }, userAccount);
        }

        [Authorize(Roles = "1")]
        [HttpPut("{id}")]
        public async Task<ActionResult> Update(int id, [FromBody] UserAccount updatedUser)
        {
            logger.Info($"Попытка обновления пользователя с ID: {id}");

            var existingUser = await _dbContext.UserAccounts.FindAsync(id);
            if (existingUser == null)
            {
                logger.Warn($"Пользователь с ID {id} не найден.");
                return NotFound($"Пользователь с ID {id} не найден.");
            }

            bool isUpdated = false;
            if (!string.IsNullOrWhiteSpace(updatedUser.FullName) && existingUser.FullName != updatedUser.FullName)
            {
                existingUser.FullName = updatedUser.FullName;
                isUpdated = true;
                logger.Info($"Обновлено FullName пользователя с ID {id}. Новое значение: {updatedUser.FullName}");
            }

            if (!string.IsNullOrWhiteSpace(updatedUser.UserName) && existingUser.UserName != updatedUser.UserName)
            {
                existingUser.UserName = updatedUser.UserName;
                isUpdated = true;
                logger.Info($"Обновлено UserName пользователя с ID {id}. Новое значение: {updatedUser.UserName}");
            }

            if (!string.IsNullOrWhiteSpace(updatedUser.Password))
            {
                string hashedPassword = PasswordHasher.HashPassword(updatedUser.Password);
                if (existingUser.Password != hashedPassword)
                {
                    existingUser.Password = hashedPassword;
                    isUpdated = true;
                    logger.Info($"Обновлен пароль пользователя с ID {id}");
                }
            }

            if (updatedUser.Role.HasValue && existingUser.Role != updatedUser.Role)
            {
                existingUser.Role = updatedUser.Role;
                isUpdated = true;
                logger.Info($"Обновлена роль пользователя с ID {id}. Новая роль: {updatedUser.Role}");
            }

            if (isUpdated)
            {
                existingUser.UpdateDate();
                await _dbContext.SaveChangesAsync();
                logger.Info($"Пользователь с ID {id} успешно обновлен.");
            }
            else
            {
                logger.Info($"Обновление пользователя с ID {id} не требуется — данные не изменились.");
            }

            return Ok(existingUser);
        }


        [Authorize(Roles = "1")]
        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(int id)
        {
            logger.Info($"Удаление пользователя с ID: {id}");
            var userAccount = await GetById(id);
            if (userAccount == null)
            {
                logger.Warn($"Попытка удаления несуществующего пользователя с ID {id}");
                return NotFound();
            }

            _dbContext.UserAccounts.Remove(userAccount);
            await _dbContext.SaveChangesAsync();
            logger.Info($"Пользователь с ID {id} удален.");
            return Ok();
        }
    }
}
