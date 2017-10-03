using System.ComponentModel.DataAnnotations;

namespace ToDoProject.Web.ViewModels
{
    public class SignUpViewModel
    {
        [Required]
        public string UserName { get; set; }

        [Required]
        public string Email { get; set; }

        public string PhoneNumber { get; set; }

        [Required]
        public string Password { get; set; }

        [Required]
        public string ConfirmPassword { get; set; }
    }
}
