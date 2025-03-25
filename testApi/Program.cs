using testToDo.ToDoApiClient;

class Program
{
    static async Task Main(string[] args)
    {
        var client = new ToDoApiClient();

        var TasksResp = await client.GetTasksAsync();

        if (TasksResp != null)
        {
            var task = TasksResp[0];
            Console.WriteLine($"ID: {task.Id}, Title: {task.Title}, Description: {task.Description}, Completed: {task.IsCompleted}");
        }

        var newTask = new ToDoTask
        {
            Title = "Задача утром",
            Description = "Сделать всё",
            IsCompleted = false,
            CreatedAt = DateTime.UtcNow,
            CompletedAt = null
        };

        await client.CreateTaskAsync(newTask);
        Console.WriteLine("Task created successfully!");


        var tasks = await client.GetTasksAsync();
        Console.WriteLine("Tasks from API:");
        foreach (var task in tasks)
        {
            Console.WriteLine($"ID: {task.Id}, Title: {task.Title}, Description: {task.Description}, Completed: {task.IsCompleted}");
        }
        if (tasks.Count > 0)
        {
            var taskToUpdate = tasks[0];
            taskToUpdate.IsCompleted = true;
            taskToUpdate.CompletedAt = DateTime.UtcNow;
            await client.UpdateTaskAsync(taskToUpdate.Id, taskToUpdate);
            Console.WriteLine("Task updated successfully!");
        }

        if (tasks.Count > 0)
        {
            await client.DeleteTaskAsync(tasks[0].Id);
            Console.WriteLine("Task deleted successfully!");
        }
    }
}