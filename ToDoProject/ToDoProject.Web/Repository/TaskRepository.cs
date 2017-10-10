using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ToDoProject.Web.Context;
using ToDoProject.Web.Models;
using ToDoProject.Web.Services;
using ToDoProject.Web.ViewModels;

namespace ToDoProject.Web.Repository
{
    public class TaskRepository : ITaskRepository
    {
        private readonly IProjectContext _projectContext;
        private readonly IUserService _userService;

        public TaskRepository(IProjectContext projectContext, IUserService userService)
        {
            _projectContext = projectContext;
            _userService = userService;
        }

        public async Task<bool> AddEditTask(TaskAddEditViewModel model)
        {
            var user = await _userService.GetUserById(model.UserId);
            bool result = false;
            if (model.TaskId == 0)
            {
                result = AddTask(model, user);
            }
            else
            {
                result = EditTask(model);
            }

            return result;
        }

        public async Task<bool> CompleteTask(int taskId)
        {
            TaskModel entity;
            try
            {
                entity = _projectContext.Tasks.SingleOrDefault(x => x.TaskId == taskId);
            }
            catch (Exception)
            {
                //TODO: Implement logging
                return false;
            }
            if (entity != null && !entity.IsDeleted)
            {
                entity.IsCompleted = true;
                entity.CompletedDate = DateTime.UtcNow;

                return true;
            }

            return false;
        }

        public async Task<bool> DeleteTask(int taskId)
        {
            TaskModel entity;
            try
            {
                entity = _projectContext.Tasks.SingleOrDefault(x => x.TaskId == taskId);
            }
            catch (Exception ex)
            {
                //TODO: Implement logging
                return false;
            }
            if(entity != null && !entity.IsDeleted)
            {
                entity.IsDeleted = true;

                return true;
            }

            return false;
        }

        public async Task<IEnumerable<TaskCollectionViewModel>> GetAllUserTasks(string userId)
        {
            var entities = _projectContext.Tasks
                .Include(x => x.User)
                .Where(x => x.User.Id == userId)
                .ProjectTo<TaskCollectionViewModel>();

            return entities.ToList();
        }

        public async Task<TaskAddEditViewModel> GetTask(int taskId)
        {
            TaskModel entity;
            try
            {
                entity = _projectContext.Tasks.SingleOrDefault(x => x.TaskId == taskId);
            }
            catch (Exception ex)
            {
                //TODO: Implement logging
                throw ex;
            }

            return Mapper.Map<TaskAddEditViewModel>(entity);
        }

        public async Task<bool> SaveChanges()
        {
            var result = await _projectContext.SaveChangesAsync();

            return result > 0;
        }

        private bool AddTask(TaskAddEditViewModel task, ProjectUser user)
        {
            try
            {
                //await _projectContext.Tasks.AddAsync(task); - not working
                var entity = Mapper.Map<TaskModel>(task);
                entity.User = user;
                _projectContext.Tasks.Add(entity);

                return true;
            }
            catch (Exception ex)
            {
                //TODO: Implement logging
                return false;
            }
        }

        private bool EditTask(TaskAddEditViewModel task)
        {
            try
            {
                var entity = _projectContext.Tasks.FirstOrDefault(x => x.TaskId == task.TaskId);
                entity = Mapper.Map(task, entity);

                return true;
            }
            catch (Exception ex)
            {
                //TODO: Implement logging
                return false;
            }
        }
    }
}
