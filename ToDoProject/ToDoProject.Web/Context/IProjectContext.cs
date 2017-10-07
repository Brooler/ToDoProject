using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using ToDoProject.Web.Models;

namespace ToDoProject.Web.Context
{
    public interface IProjectContext
    {
        DbSet<TaskModel> Tasks { get; set; }

        Task<int> SaveChangesAsync();
    }
}