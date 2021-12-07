using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace VDCore.DBContext.Core.Models
{
    public class UserStatus
    {
        [Key]
        public int UserStatusId { get; set; }
        [MaxLength(64, ErrorMessage = "StatusName length should be less than 64 chars.")]
        public string StatusName { get; set; }
        private ICollection<User> User { get; set; }
    }
}
