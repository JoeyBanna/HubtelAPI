using System.Collections.Generic;
using System.Threading.Tasks;

namespace Hubtel_payment_API.Repositories.Users
{
    public interface IUserRepository
    {
        Task AddUsersAsync(Entities.Users entities);
        Task<IEnumerable<Entities.Users>> GetAllUsersAsync();
    }
}
