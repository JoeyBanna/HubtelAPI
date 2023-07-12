using System;
using System.ComponentModel.DataAnnotations;

namespace Hubtel_payment_API.Entities
{
    public class Wallets
    {
        [Key]
        public Guid WalletId { get; set; } = Guid.NewGuid();
        [Required]
        public string Name { get; set; }
        [Required]
        public string Type { get; set; } //momo or card only
        [Required]
        public string AccountNumber { get; set; } //momo number or card number
        [Required]
        public string AccountScheme { get; set; } //visa|mastercard|mtn|vodafone|airteltigo
        [Required]
        public string OwnerPhoneNumber { get; set; } //Owner phone number
        public DateTime CreatedAt { get; set; } = DateTime.Now;
        [Required]
        public Guid UserId { get; set; }
        public string WalletAccountNumber { get; set; }
    }
}
