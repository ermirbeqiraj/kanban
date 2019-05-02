using System.ComponentModel.DataAnnotations;

namespace Issue.Web.ViewModels
{
    public class LoginViewModel
    {
        [Required]
        [MaxLength(200)]
        public string Username { get; set; }

        [Required]
        [MaxLength(200)]
        public string Password { get; set; }
    }

    public class LoginResult
    {
        public bool Success { get; set; }
        public string[] Roles { get; set; }
        public int UserId { get; set; }
    }
}
