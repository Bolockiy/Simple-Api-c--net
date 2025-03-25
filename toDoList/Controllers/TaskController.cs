using ApiToDo.Application.Repositories;
using ApiToDo.Domain.Entities;
using Microsoft.AspNetCore.Mvc;

namespace ApiToDo.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TaskController : ControllerBase
    {
        private readonly ITaskRepository _taskRepository;

        public TaskController(ITaskRepository taskRepository)
        {
            _taskRepository = taskRepository;
        }

        [HttpGet]
        public async Task<ActionResult<List<ToDoTask>>> Get()
        {
            return Ok(await _taskRepository.GetAllAsync());
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ToDoTask>> GetById(int id)
        {
            var task = await _taskRepository.GetByIdAsync(id);
            return task == null ? NotFound() : Ok(task);
        }

        [HttpPost]
        public async Task<ActionResult> Create(ToDoTask task)
        {
            await _taskRepository.AddAsync(task);
            return CreatedAtAction(nameof(GetById), new { id = task.Id }, task);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, ToDoTask updatedTask)
        {
            if (id != updatedTask.Id) return BadRequest();
            await _taskRepository.UpdateAsync(updatedTask);
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            await _taskRepository.DeleteAsync(id);
            return NoContent();
        }
    }
}
