using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Hubtel_payment_API.Repositories.Wallets
{
    public interface IWalletRepository
    {
        Task<IEnumerable<Entities.Wallets>> GetWalletCountAsync(string AccountNumber);
        Task<bool> CheckIfWalletAccountExistsAsync(string AccountNumber);
        Task<IEnumerable<Entities.Wallets>> GetAllWalletsAsync();
        Task<Entities.Wallets> GetSingleWalletAsync(Guid Id);
        Task DeleteWalletAsync(Guid Id);
        Task AddWalletAsync(Entities.Wallets entities);
    }
}
