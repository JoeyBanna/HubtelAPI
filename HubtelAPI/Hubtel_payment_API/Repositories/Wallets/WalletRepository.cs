using AutoMapper;
using Hubtel_payment_API.Data;
using Microsoft.EntityFrameworkCore;
using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;

namespace Hubtel_payment_API.Repositories.Wallets
{
    public class WalletRepository : IWalletRepository
    {
        private readonly WalletDbContext _context;
       private IConnectionMultiplexer _multiplexer;


        public WalletRepository(WalletDbContext context,IConnectionMultiplexer redis )
        {
            _context = context;
            _multiplexer = redis;
           
            
        }

        public async Task AddWalletAsync(Entities.Wallets entities)
        {
            await _context.AddAsync(entities);

            await _context.SaveChangesAsync();
           

        }

        public async Task<bool> CheckIfWalletAccountExistsAsync(string AccountNumber)
            => await _context.Wallets.AnyAsync(item => item.AccountNumber == AccountNumber);

        public async Task<Entities.Wallets> checkRedisForData(string WalletId)
        {
            var db =  _multiplexer.GetDatabase();

            var serial = db.StringGet(WalletId);

            if (!string.IsNullOrEmpty(serial))
            {
                return JsonSerializer.Deserialize<Entities.Wallets>(serial);
            }
            return null;
            


            //db.StringSet(entities.WalletId, serial);
        }

        public async Task DeleteWalletAsync(Guid Id)
        {
            var wallet = await _context.Wallets.SingleOrDefaultAsync(w => w.WalletId == Id.ToString());
            _context.Remove(wallet);
            await _context.SaveChangesAsync();
        }

        public async Task<IEnumerable<Entities.Wallets>> GetAllWalletsAsync()
        {

           var redisDb = _multiplexer.GetDatabase();
            var completeSet = redisDb.SetMembers("HubtelWallets");
            if (completeSet.Length > 0)
            {
                var obj = Array.ConvertAll(completeSet,val=> JsonSerializer.Deserialize<Entities.Wallets>(val)).ToList();
                Console.WriteLine(obj);

                return obj;
             // return JsonSerializer.Deserialize<Entities.Wallets>(completeSet);

            }

            var results = await _context.Wallets.ToListAsync();
            var rest = JsonSerializer.Serialize(results);
            redisDb.SetAdd("HubtelWallets", rest);
            
            return results;

        }

        public async Task<Entities.Wallets> GetSingleWalletAsync(string Id)
            => await _context.Wallets.Where(w => w.WalletId == Id).SingleOrDefaultAsync();

        public async Task<IEnumerable<Entities.Wallets>> GetWalletByUserIdAsync(Guid userId)
        {
          var obj =  await _context.Wallets.Where(item => item.UserId == userId).ToListAsync();
            return obj;

        }

        public async Task<IEnumerable<Entities.Wallets>> GetWalletCountAsync(Guid id)
        {
            var obj = await _context.Wallets.Where(w => w.UserId == id).ToListAsync();

            return obj;
        }
    }
}
