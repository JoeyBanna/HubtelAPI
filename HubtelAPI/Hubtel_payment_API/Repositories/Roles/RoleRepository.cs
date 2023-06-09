using Hubtel_payment_API.Data;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Hubtel_payment_API.Repositories.Roles
{
    public class RoleRepository : IRoleRepository
    {
        private readonly WalletDbContext _context;

        public RoleRepository(WalletDbContext context)
        {
            _context = context;
        }

        public async Task AddRoleAsync(Entities.Roles entities)
        {
            await _context.AddAsync(entities);

            await _context.SaveChangesAsync();
        }

        public async Task<IEnumerable<Entities.Roles>> GetAllRolesAsync() 
            => await _context.Roles.ToListAsync();
    }
}
