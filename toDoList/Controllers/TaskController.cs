using ApiToDo.Domain.Entities;
using BusinessLayer.Services;
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
        private readonly ICrudService<ToDoTask> _taskRepository;

        public TaskController(ICrudService<ToDoTask> taskRepository)
        {
            _taskRepository = taskRepository;
            logger.Info("TaskController инициализирован.");
        }

        [HttpGet]
        public async Task<ActionResult<List<ToDoTask>>> Get()
        {
            var resp = await _taskRepository.GetAllAsync();
            return Ok(resp);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ToDoTask>> GetById(int id)
        {
            var task = await _taskRepository.GetByIdAsync(id);
            if (task == null)
            {
                return NotFound();
            }
            return Ok(task);
        }

        [Authorize(Roles = "1")]
        [HttpPost]
        public async Task<ActionResult> Create([FromBody] ToDoTask task)
        {
            if (!await _taskRepository.CreateAsync(task))
                return BadRequest("Невозможно создать задачу, возможно отсутсвует поле Название");
           return CreatedAtAction(nameof(GetById), new { id = task.Id }, task);
        }


        [Authorize(Roles = "1")]
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] ToDoTask updatedTask)
        {
            if (!await _taskRepository.UpdateAsync(id, updatedTask))
                return BadRequest("Не получилось обновить запись");
            return Ok();
        }

        [Authorize(Roles = "1")]
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {   
            await _taskRepository.DeleteAsync(id);
            return NoContent();
        }
    }
}
