using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace VDCore.DBContext.Core.Models
{
    public class User
    {
        [Key]
        public long UserId { get; set; }
        
        [Required(ErrorMessage = "CoreId is required!")]
        [MinLength(36)]
        public Guid CoreId { get; set; }
        
        [Required(ErrorMessage = "Login is required!")]
        [MaxLength(64, ErrorMessage = "Login length should be less than 64 chars.")]
        public string Login { get; set; }
        
        [Required(ErrorMessage = "Password is required!")]
        [MaxLength(128, ErrorMessage = "Password length should be less than 128 chars.")]
        public string Password { get; set; }
        
        private ICollection<UserRole> UserRoles { get; set; }
        
        [Required(ErrorMessage = "UserStatusId is required!")]
        public int UserStatusId { get; set; }
        
        private UserStatus UserStatus { get; set; }
    }
}