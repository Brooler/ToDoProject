using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using ToDoProject.Web.Context;
using Microsoft.Extensions.Configuration;
using Microsoft.EntityFrameworkCore;
using ToDoProject.Web.Services;
using AutoMapper;
using ToDoProject.Web.ViewModels;
using ToDoProject.Web.Models;
using ToDoProject.Business.Services;
using ToDoProject.Web.Repository;
using ToDoProject.Web.Helpers;

namespace ToDoProject.Web
{
    public class Startup
    {
        private readonly IHostingEnvironment _env;
        private readonly IConfigurationRoot _config;

        public Startup(IHostingEnvironment env)
        {
            _env = env;

            var configBuilder = new ConfigurationBuilder()
                .SetBasePath(_env.ContentRootPath)
                .AddJsonFile("config.json")
                .AddEnvironmentVariables();

            _config = configBuilder.Build();
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddSingleton(_config);
            services.AddMvc();

            services.AddDbContext<ProjectContext>();

            services.AddIdentity<ProjectUser, IdentityRole>(options =>
            {
                options.Password.RequireDigit = true;
                options.Password.RequiredLength = 8;
                options.Password.RequireLowercase = true;
                options.Password.RequireUppercase = true;
                options.Password.RequireNonAlphanumeric = true;

                options.User.RequireUniqueEmail = true;
            })
                .AddEntityFrameworkStores<ProjectContext>()
                .AddDefaultTokenProviders();
            services.ConfigureApplicationCookie(options =>
            {
                options.Cookie.Name = "TaskAppCookies";
                options.LoginPath = "/auth/login";
                options.LogoutPath = "/auth/logout";
            });

            services.AddScoped<IUserService, UserService>();
            services.AddScoped<ITaskRepository, TaskRepository>();
            if (_env.IsEnvironment("Development"))
            {
                services.AddScoped<IEmailSender, DebugEmailSender>();
            }
            else
            {
                services.AddScoped<IEmailSender, EmailSender>();
            }
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            MapperHelper.InitializeMapper();

            loggerFactory.AddConsole();

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseStaticFiles();

            app.UseAuthentication();

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=App}/{action=Index}/{id?}");
            });
        }
    }
}
