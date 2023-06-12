using System;
using System.ComponentModel.DataAnnotations;

namespace Hubtel_payment_API.Entities
{
    public class Wallets
    {
        [Key]
        public Guid WalletId { get; set; } = Guid.NewGuid();
        public string Name { get; set; }
        public string Type { get; set; } //momo or card only
        public string AccountNumber { get; set; } //momo number or card number
        public string AccountScheme { get; set; } //visa|mastercard|mtn|vodafone|airteltigo
        public string OwnerPhoneNumber { get; set; } //Owner phone number
        public DateTime CreatedAt { get; set; } = DateTime.Now;
        public Guid UserId { get; set; }
        public string WalletAccountNumber { get; set; }
    }
}
