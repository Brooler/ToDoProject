using AutoMapper;
using System;
using ToDoProject.Web.Models;
using ToDoProject.Web.ViewModels;

namespace ToDoProject.Web.Helpers
{
    public class MapperHelper
    {
        public static void InitializeMapper()
        {
            Mapper.Initialize(config =>
            {
                config.CreateMap<SignUpViewModel, ProjectUser>();
                config.CreateMap<TaskAddEditViewModel, TaskModel>().ReverseMap();
                config.CreateMap<TaskModel, TaskCollectionViewModel>()
                    .ForMember(dest => dest.IsOverdue, opt => opt.MapFrom(src => src.DueDate <= DateTime.UtcNow));
            });
        }
    }
}
