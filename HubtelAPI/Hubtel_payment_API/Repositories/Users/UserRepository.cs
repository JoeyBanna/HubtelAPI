using Hubtel_payment_API.Data;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Hubtel_payment_API.Repositories.Users
{
    public class UserRepository : IUserRepository
    {
        private readonly WalletDbContext _context;

        public UserRepository(WalletDbContext context)
        {
            _context = context;
        }

        public async Task AddUsersAsync(Entities.Users entities)
        {
            await _context.AddAsync(entities);

            await _context.SaveChangesAsync();
        }

        public async Task<IEnumerable<Entities.Users>> GetAllUsersAsync() 
            => await _context.Users.ToListAsync();
    }
}
