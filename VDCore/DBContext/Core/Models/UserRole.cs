using System.ComponentModel.DataAnnotations;

namespace VDCore.DBContext.Core.Models
{
    public class UserRole
    {
        [Key]
        public long Id { get; set; }
        public int RoleId { get; set; }
        public long UserId { get; set; }
        public virtual User User { get; set; }
        public virtual Role Role { get; set; }
    }
}
