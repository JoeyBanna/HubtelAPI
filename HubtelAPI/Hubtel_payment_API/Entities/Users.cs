using System;
using System.ComponentModel.DataAnnotations;

namespace Hubtel_payment_API.Entities
{
    public class Users
    {
        [Key] 
        public Guid UserId { get; set; } = Guid.NewGuid();
        [Required]
        public string Name { get; set; }
        [Required]
        public Guid RoleId { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.Now;
    }
}
