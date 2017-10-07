using System.Collections.Generic;
using System.Threading.Tasks;
using ToDoProject.Web.Models;
using ToDoProject.Web.ViewModels;

namespace ToDoProject.Web.Repository
{
    public interface ITaskRepository
    {
        Task<IEnumerable<TaskCollectionViewModel>> GetAllUserTasks(string userId);

        Task<TaskAddEditViewModel> GetTask(int taskId);

        Task<bool> AddEditTask(TaskAddEditViewModel model);

        Task<bool> CompleteTask(int taskId);

        Task<bool> DeleteTask(int taskId);

        Task<bool> SaveChanges();
    }
}
