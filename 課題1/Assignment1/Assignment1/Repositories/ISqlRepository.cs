using Assignment1.Models;

namespace Assignment1.Repositories;

public interface ISqlRepository
{
    Task<IEnumerable<TaskModel>> GetIndex();
    Task<TaskModel> GetEdit(int TaskId);
    Task UpdateTask(TaskModel Task);
}
