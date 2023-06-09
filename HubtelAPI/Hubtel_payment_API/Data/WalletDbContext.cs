using Hubtel_payment_API.Entities;
using Microsoft.EntityFrameworkCore;

namespace Hubtel_payment_API.Data
{
    public class WalletDbContext : DbContext
    {
        public WalletDbContext(DbContextOptions<WalletDbContext> options) : base(options)
        {
        }

        public DbSet<Wallets> Wallets { get; set; }
        public DbSet<Users> Users { get; set; }
        public DbSet<Roles> Roles { get; set; }
    }
}
