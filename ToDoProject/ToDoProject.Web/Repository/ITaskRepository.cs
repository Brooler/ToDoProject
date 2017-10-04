using System.Threading.Tasks;
using ToDoProject.Web.Models;
using ToDoProject.Web.ViewModels;

namespace ToDoProject.Web.Repository
{
    public interface ITaskRepository
    {
        Task<TaskModel> GetAllUserTasks(string userId);

        Task<TaskModel> GetTask(int taskId);

        Task AddTask(TaskAddEditViewModel model);

        Task<bool> CompleteTask(int taskId);

        Task<bool> DeleteTask(int taskId);

        Task<bool> SaveChanges();
    }
}
