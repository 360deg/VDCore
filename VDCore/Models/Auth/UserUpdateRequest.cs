using System;
using System.ComponentModel.DataAnnotations;

namespace VDCore.Models.Auth
{
    public class UserUpdateRequest: UserRequest
    {
        [Required(ErrorMessage = "CoreId is required!")]
        [MinLength(36)]
        public Guid CoreId { get; set; }
    }
}