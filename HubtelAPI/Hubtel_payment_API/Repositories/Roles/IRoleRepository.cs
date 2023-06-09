using System.Collections.Generic;
using System.Threading.Tasks;

namespace Hubtel_payment_API.Repositories.Roles
{
    public interface IRoleRepository
    {
        Task AddRoleAsync(Entities.Roles entities);
        Task<IEnumerable<Entities.Roles>> GetAllRolesAsync();
    }
}
