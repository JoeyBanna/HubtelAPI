using AutoMapper;
using Hubtel_payment_API.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Hubtel_payment_API.Repositories.Wallets
{
    public class WalletRepository : IWalletRepository
    {
        private readonly WalletDbContext _context;
        private readonly IMapper _mapper;

        public WalletRepository(WalletDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task AddWalletAsync(Entities.Wallets entities)
        {
            await _context.AddAsync(entities);

            await _context.SaveChangesAsync();
        }

        public async Task<bool> CheckIfWalletAccountExistsAsync(string AccountNumber)
            => await _context.Wallets.AnyAsync(item => item.AccountNumber == AccountNumber);

        public async Task DeleteWalletAsync(Guid Id)
        {
            var wallet = await _context.Wallets.SingleOrDefaultAsync(w => w.WalletId == Id);
            _context.Remove(wallet);
            await _context.SaveChangesAsync();
        }

        public async Task<IEnumerable<Entities.Wallets>> GetAllWalletsAsync()
            => await _context.Wallets.ToListAsync();

        public async Task<Entities.Wallets> GetSingleWalletAsync(Guid Id)
            => await _context.Wallets.Where(w => w.WalletId == Id).SingleOrDefaultAsync();

        public async Task<IEnumerable<Entities.Wallets>> GetWalletCountAsync(string AccountNumber)
        {
            var obj = await _context.Wallets.Where(w => w.WalletAccountNumber == AccountNumber).ToListAsync();

            return obj;
        }
    }
}
