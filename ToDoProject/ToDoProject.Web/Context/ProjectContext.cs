using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using ToDoProject.Web.Models;

namespace ToDoProject.Web.Context
{
    public class ProjectContext : IdentityDbContext<ProjectUser>
    {
        private readonly IConfigurationRoot _config;
        public ProjectContext(IConfigurationRoot config, DbContextOptions options)
            : base(options)
        {
            _config = config;
        }

        public DbSet<TaskModel> Tasks { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);

            optionsBuilder.UseSqlServer(_config["ConnectionStrings:ProjectContextConnection"]);
        }
    }
}
