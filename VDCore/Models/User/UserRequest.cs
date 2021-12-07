using System.ComponentModel.DataAnnotations;

namespace VDCore.Models.User
{
    public class UserRequest
    {
        [Required(ErrorMessage = "Login is required!")]
        [MaxLength(64, ErrorMessage = "Login length should be less than 64 chars.")]
        public string Login { get; set; }
        
        [Required(ErrorMessage = "Password is required!")]
        [MaxLength(128, ErrorMessage = "Password length should be less than 128 chars.")]
        public string Password { get; set; }
        
        public int UserStatusId { get; set; }
    }
}
