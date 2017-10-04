using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using ToDoProject.Business.Models;

namespace ToDoProject.Business.Services
{
    public class EmailSender : IEmailSender
    {
        public Task SendEmail(EmailModel model)
        {
            //TODO: Implement real email sender
            throw new NotImplementedException();
        }
    }
}
