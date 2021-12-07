using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace VDCore.DBContext.Core.Models
{
    public class Role
    {
        [Key]
        [Required(ErrorMessage = "RoleId is required!")]
        public int RoleId { get; set; }
        
        [Required(ErrorMessage = "RoleName is required!")]
        [MaxLength(64, ErrorMessage = "RoleName length should be less than 64 chars.")]
        public string Name { get; set; }
        private ICollection<UserRole> UserRoles { get; set; }
    }
}
