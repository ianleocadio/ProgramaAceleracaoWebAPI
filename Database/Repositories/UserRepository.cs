using Database.Contexts;
using Database.Repositories.Common;
using Domain.Models;
using Microsoft.EntityFrameworkCore;
using Repository.Interfaces;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Database.Repositories
{
    public class UserRepository : Repository<User>, IUserRepository
    {
        public UserRepository(DbContextBase context) : base(context)
        {
        }

        public async override Task<User?> GetByIdAsync(long id, CancellationToken cancellationToken)
        {
            return await this.DbSet
                .Include(u => u.Permissions)
                .FirstOrDefaultAsync(u => u.ID == id, cancellationToken);
        }

        public async override Task<IEnumerable<User>> GetAllAsync(CancellationToken cancellationToken)
        {
            return await this.DbSet
               .Include(u => u.Permissions)
               .ToListAsync(cancellationToken);
        }

        public async Task<bool> ExistsByUsernameAsync(string username, CancellationToken cancellationToken)
        {
            return await this.DbSet.AnyAsync(u => u.Username == username, cancellationToken);
        }

        public async Task<User?> GetByUsernameAsync(string username, CancellationToken cancellationToken)
        {
            return await this.DbSet.FirstOrDefaultAsync(u => u.Username == username, cancellationToken);
        }
    }
}
