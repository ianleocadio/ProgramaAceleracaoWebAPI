using Database.Contexts;
using Database.Repositories.Common;
using Domain.Models;
using Microsoft.EntityFrameworkCore;
using Repository.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Database.Repositories
{
    public class UserPermissionRepository : Repository<UserPermission>, IUserPermissionRepository
    {
        public UserPermissionRepository(DbContextBase context) : base(context)
        {
        }

        public async Task<IEnumerable<UserPermission>> GetByUserIdAsync(long userId, CancellationToken cancellationToken)
        {
            return await this.DbSet.Where(p => p.UserID == userId).ToListAsync(cancellationToken);
        }
    }
}
