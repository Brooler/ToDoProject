using System.Threading.Tasks;
using ToDoProject.Business.Models;

namespace ToDoProject.Business.Services
{
    public class DebugEmailSender : IEmailSender
    {
        public Task SendEmail(EmailModel model)
        {
            //TODO: Implement output for testing (for example in file)
            throw new System.NotImplementedException();
        }
    }
}
