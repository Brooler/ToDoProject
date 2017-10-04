using System;
using System.Threading.Tasks;
using ToDoProject.Web.Models;
using ToDoProject.Web.ViewModels;

namespace ToDoProject.Web.Repository
{
    public class TaskRepository : ITaskRepository
    {
        public Task AddTask(TaskAddEditViewModel model)
        {
            throw new NotImplementedException();
        }

        public Task<bool> CompleteTask(int taskId)
        {
            throw new NotImplementedException();
        }

        public Task<bool> DeleteTask(int taskId)
        {
            throw new NotImplementedException();
        }

        public Task<TaskModel> GetAllUserTasks(string userId)
        {
            throw new NotImplementedException();
        }

        public Task<TaskModel> GetTask(int taskId)
        {
            throw new NotImplementedException();
        }

        public Task<bool> SaveChanges()
        {
            throw new NotImplementedException();
        }
    }
}
