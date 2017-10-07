using System;
using ToDoProject.Web.Models;

namespace ToDoProject.Web.ViewModels
{
    public class TaskAddEditViewModel
    {
        public int TaskId { get; set; }
        
        public string Name { get; set; }

        public string Comment { get; set; }

        public DateTime? DueDate { get; set; }

        public Priority PriorityId { get; set; }
    }
}
