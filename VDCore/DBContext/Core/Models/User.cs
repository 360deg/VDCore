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
        
        [Required]
        [MinLength(36)]
        public Guid CoreId { get; set; }
        
        [Required]
        [MaxLength(64, ErrorMessage = "Login length should be less than 64 chars.")]
        public string Login { get; set; }
        
        [Required]
        [MaxLength(128, ErrorMessage = "Password length should be less than 128 chars.")]
        public string Password { get; set; }
        
        public virtual ICollection<UserRole> UserRoles { get; set; }
        
        [Required]
        public int UserStatusId { get; set; }
        
        public virtual UserStatus UserStatus { get; set; }
    }
}