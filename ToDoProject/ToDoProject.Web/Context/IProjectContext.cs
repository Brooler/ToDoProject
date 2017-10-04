using Microsoft.EntityFrameworkCore;
using ToDoProject.Web.Models;

namespace ToDoProject.Web.Context
{
    public interface IProjectContext
    {
        DbSet<TaskModel> Tasks { get; set; }
    }
}