using ApiToDo.Application.Repositories;
using ApiToDo.Domain.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NLog;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ApiToDo.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TaskController : ControllerBase
    {
        private static readonly Logger logger = LogManager.GetCurrentClassLogger();
        private readonly ITaskRepository _taskRepository;

        public TaskController(ITaskRepository taskRepository)
        {
            _taskRepository = taskRepository;
            logger.Info("TaskController инициализирован.");
        }

        [HttpGet]
        public async Task<ActionResult<List<ToDoTask>>> Get()
        {
            logger.Info("Запрос на получение всех задач.");
            return Ok(await _taskRepository.GetAllAsync());
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ToDoTask>> GetById(int id)
        {
            logger.Info($"Запрос на получение задачи с ID: {id}");
            var task = await _taskRepository.GetByIdAsync(id);
            if (task == null)
            {
                logger.Warn($"Задача с ID {id} не найдена.");
                return NotFound();
            }
            return Ok(task);
        }

        [Authorize(Roles = "1")]
        [HttpPost]
        public async Task<ActionResult> Create([FromBody] ToDoTask task)
        {
            task = new ToDoTask
            {
                Title = task.Title,
                Description = task.Description,
                CreatedAt = DateTime.UtcNow
            };

            await _taskRepository.AddAsync(task);
            return CreatedAtAction(nameof(GetById), new { id = task.Id }, task);
        }


        [Authorize(Roles = "1")]
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] ToDoTask updatedTask)
        {
            if (id != updatedTask.Id)
            {
                logger.Warn($"Несоответствие ID задачи: ID в маршруте {id} не совпадает с ID задачи {updatedTask.Id}.");
                return BadRequest("ID задачи в маршруте не совпадает с ID в теле запроса.");
            }

            var existingTask = await _taskRepository.GetByIdAsync(id);
            if (existingTask == null)
            {
                logger.Warn($"Попытка обновления несуществующей задачи с ID: {id}");
                return NotFound($"Задача с ID {id} не найдена.");
            }

            existingTask.Update(updatedTask.Title, updatedTask.Description, updatedTask.IsCompleted);

            logger.Info($"Обновление задачи с ID: {id}");
            await _taskRepository.UpdateAsync(existingTask);

            logger.Info($"Задача с ID {id} успешно обновлена.");
            return Ok(existingTask);
        }

        [Authorize(Roles = "1")]
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            logger.Info($"Удаление задачи с ID: {id}");
            await _taskRepository.DeleteAsync(id);
            logger.Info($"Задача с ID {id} успешно удалена.");
            return NoContent();
        }
    }
}
