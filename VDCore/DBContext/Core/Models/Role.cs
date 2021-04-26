using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace VDCore.DBContext.Core.Models
{
    public class Role
    {
        [Key]
        public int RoleId { get; set; }
        public string Name { get; set; }
        public virtual ICollection<UserRole> UserRoles { get; set; }
    }
}