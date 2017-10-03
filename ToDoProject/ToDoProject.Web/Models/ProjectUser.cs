using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;

namespace ToDoProject.Web.Models
{
    public class ProjectUser : IdentityUser
    {
        public ICollection<TaskModel> Tasks { get; set; }
    }
}
