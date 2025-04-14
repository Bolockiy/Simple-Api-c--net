using ApiToDo.Domain.Entities;
using ApiToDo.Infrastructure.Data;
using BusinessLayer.Services;
using Helper.Security;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NLog;
using System.Data;
using System.Threading.Tasks;
using toDoList.Entities.UserAccount;

namespace toDoList.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class UserController : ControllerBase
    {
        private static readonly Logger logger = LogManager.GetCurrentClassLogger();
        private readonly ICrudService<UserAccount> _UserService;

        public UserController(ICrudService<UserAccount> dbContext)
        {
            logger.Info("UserController инициализирован.");
            _UserService = dbContext;
        }

        [HttpGet]
        public async Task<ActionResult<List<UserAccount>>> Get()
        {
            var resp = await _UserService.GetAllAsync();
            return Ok(resp);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<UserAccount?>> GetById(int id)
        {
            var user = await _UserService.GetByIdAsync(id);
            if (user == null) return BadRequest("Пользователь не найден");
            return user;
        }

        [Authorize(Roles = "1")]
        [HttpPost]
        public async Task<ActionResult> Create([FromBody] UserAccount userAccount)
        {
            if (!await _UserService.CreateAsync(userAccount))
                return BadRequest("Произошла ошибка при добавлении пользователя");
            return CreatedAtAction(nameof(GetById), new { id = userAccount.Id }, userAccount);
        }

        [Authorize(Roles = "1")]
        [HttpPut("{id}")]
        public async Task<ActionResult> Update(int id, [FromBody] UserAccount updatedUser)
        {
            if (!await _UserService.UpdateAsync(id, updatedUser))
                return BadRequest("Не получилось обновить запись");
            return Ok();
        }


        [Authorize(Roles = "1")]
        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(int id)
        {
            await _UserService.DeleteAsync(id);
            return NoContent();
        }
    }
}
