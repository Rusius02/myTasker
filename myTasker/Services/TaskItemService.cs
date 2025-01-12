using Domain;
using System.Collections.Generic;
using System.Threading.Tasks;

public class TaskItemService
{
    private readonly GenericRepository<TaskItem> _taskRepository;

    public TaskItemService(GenericRepository<TaskItem> taskRepository)
    {
        _taskRepository = taskRepository;
    }

    public async Task<IEnumerable<TaskItem>> GetAllTasksAsync()
    {
        return await _taskRepository.GetAllAsync();
    }

    public async Task<TaskItem> GetTaskByIdAsync(int id)
    {
        return await _taskRepository.GetByIdAsync(id);
    }

    public async Task AddTaskAsync(TaskItem task)
    {
        await _taskRepository.AddAsync(task);
    }

    public async Task UpdateTaskAsync(TaskItem task)
    {
        await _taskRepository.UpdateAsync(task);
    }

    public async Task DeleteTaskAsync(int id)
    {
        var task = await _taskRepository.GetByIdAsync(id);
        if (task != null)
        {
            await _taskRepository.DeleteAsync(task);
        }
    }
}
