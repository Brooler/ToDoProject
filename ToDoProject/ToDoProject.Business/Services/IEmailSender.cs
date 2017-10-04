using System.Threading.Tasks;
using ToDoProject.Business.Models;

namespace ToDoProject.Business.Services
{
    public interface IEmailSender
    {
        Task SendEmail(EmailModel model);
    }
}
