using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace ToDoProject.Web.ViewModels
{
    public class LoginViewModel
    {
        [Required]
        [DisplayName("Login")]
        public string UserName { get; set; }
        
        [Required]
        public string Password { get; set; }

        [DisplayName("Remember me")]
        public bool RememberMe { get; set; }
    }
}
