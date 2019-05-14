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

    public class UserViewModel
    {
        public int Id { get; set; }
        
        [Required]
        public string Email { get; set; }
        [Required]
        [MinLength(6)]
        public string Password { get; set; }

        public string PhoneNumber { get; set; }

        public string UserName { get; set; }

    }

    public class UserUpdateModel
    {
        public int Id { get; set; }

        [Required]
        public string Email { get; set; }
      
        public string PhoneNumber { get; set; }
        [Required]
        public string UserName { get; set; }
    }

    public class UserForListModel
    {
        public int Id { get; set; }
        public string Email { get; set; }

        public string UserName { get; set; }
    }

    public class UpdateUserPassword
    {
        [Required]
        public int Id { get; set; }

        [Required]
        [StringLength(20, MinimumLength = 6)]
        public string Password { get; set; }

        [Compare("Password")]
        public string ConfirmPassword { get; set; }
    }
}
