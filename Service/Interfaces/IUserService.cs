using Domain.Models;
using Domain.Transfers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Service.Interfaces
{
    public interface IUserService
    {
        Task<User> CreateAsync(UserCreateTransfer newUser, CancellationToken cancellationToken);

        Task<User?> GetByIdAsync(long id, CancellationToken cancellationToken);

        Task<IEnumerable<User>> GetAllAsync(CancellationToken cancellationToken);

        Task<User> UpdateAsync(UserUpdateTransfer updateUser, CancellationToken cancellationToken);

        Task DeleteAsync(long id, CancellationToken cancellationToken);
    }
}
