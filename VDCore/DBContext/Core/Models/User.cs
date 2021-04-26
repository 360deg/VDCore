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
        public string Login { get; set; }
        public string Password { get; set; }
        public virtual ICollection<UserRole> UserRoles { get; set; }
    }
}