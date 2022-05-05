using System.ComponentModel.DataAnnotations;

namespace Collecting.Data.Models
{
    public class Login
    {
        public Login(string userEmail, string password)
        {
            UserEmail = userEmail;
            Password = password;
        }

        [Required]
        public string UserEmail { get; set; }
        [Required]
        public string Password { get; set; }
    }
}
