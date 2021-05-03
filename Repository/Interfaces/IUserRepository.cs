using Domain.Models;
using Repository.Interfaces.Common;
using System.Threading;
using System.Threading.Tasks;

namespace Repository.Interfaces
{
    public interface IUserRepository : IRepository<User>
    {
        Task<bool> ExistsByUsernameAsync(string username, CancellationToken cancellationToken);
        Task<User?> GetByUsernameAsync(string username, CancellationToken cancellationToken);
    }
}
