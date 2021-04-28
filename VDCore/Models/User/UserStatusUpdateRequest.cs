using System;
using System.ComponentModel.DataAnnotations;

namespace VDCore.Models.User
{
    public class UserStatusUpdateRequest
    {
        [Required(ErrorMessage = "CoreId is required!")]
        [MinLength(36)]
        public Guid CoreId { get; set; }
        
        [Required(ErrorMessage = "UserStatusId is required!")]
        public int UserStatusId { get; set; }
    }
}