using System;
using System.ComponentModel.DataAnnotations;

namespace ToDoProject.Web.Models
{
    public class TaskModel
    {
        [Key]
        public int TaskId { get; set; }

        public string Name { get; set; }

        public DateTime DueDate { get; set; }

        public bool IsCompleted { get; set; }

        public bool IsDeleted { get; set; }

        public DateTime? CompletedDate { get; set; }

        public string Comment { get; set; }

        public ProjectUser User { get; set; }

        public Priority PriorityId { get; set; }
    }

    public enum Priority
    {
        Lowest = 0,
        Low = 1,
        Middle = 2,
        High = 3,
        Critical = 4
    }
}
