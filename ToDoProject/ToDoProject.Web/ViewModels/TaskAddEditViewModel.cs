using System;
using System.ComponentModel.DataAnnotations;
using ToDoProject.Web.Models;

namespace ToDoProject.Web.ViewModels
{
    public class TaskAddEditViewModel
    {
        [Key]
        public int TaskId { get; set; }
        
        [Required]
        public string Name { get; set; }

        public string Comment { get; set; }
        
        public DateTime? DueDate { get; set; }

        public Priority PriorityId { get; set; }
    }
}
