using System;
using System.ComponentModel.DataAnnotations;

namespace Hubtel_payment_API.Entities
{
    public class Roles
    {
        [Key]
        public Guid RoleId { get; set; } = Guid.NewGuid();
        public string RoleName { get; set; }
    }
}
