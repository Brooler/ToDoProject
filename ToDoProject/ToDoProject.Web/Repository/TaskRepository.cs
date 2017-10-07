using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using ToDoProject.Web.Context;
using ToDoProject.Web.Models;
using ToDoProject.Web.ViewModels;

namespace ToDoProject.Web.Repository
{
    public class TaskRepository : ITaskRepository
    {
        private readonly IProjectContext _projectContext;

        public TaskRepository(IProjectContext projectContext)
        {
            _projectContext = projectContext;
        }

        public Task<bool> AddEditTask(TaskAddEditViewModel model)
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

        public Task<IEnumerable<TaskCollectionViewModel>> GetAllUserTasks(string userId)
        {
            throw new NotImplementedException();
        }

        public Task<TaskAddEditViewModel> GetTask(int taskId)
        {
            throw new NotImplementedException();
        }

        public Task<bool> SaveChanges()
        {
            throw new NotImplementedException();
        }
    }
}
