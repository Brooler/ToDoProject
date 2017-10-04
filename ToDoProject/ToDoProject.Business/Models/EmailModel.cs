namespace ToDoProject.Business.Models
{
    public class EmailModel
    {
        public string Subject { get; set; }

        public string EmailTo { get; set; }

        public string EmailFrom { get; set; }

        public string HtmlBody { get; set; }
    }
}
