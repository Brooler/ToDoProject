using System;

namespace ToDoProject.Web.ViewModels
{
    public class TaskCollectionViewModel
    {
        public int TaskId { get; set; }

        public string Name { get; set; }

        public DateTime? DueDate { get; set; }

        public bool IsOverdue { get; set; }
    }
}
