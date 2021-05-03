using Domain.Core.Models;
using System.Collections.Generic;

namespace Domain.Models
{
    public class User : Entity<User>
    {
        public string Username { get; set; } = null!;
        public string Password { get; set; } = null!;
        public string Hash { get; set; } = null!;

        public string? Name { get; set; }
        public string? Lastname { get; set; }
        public string? Email { get; set; }

        #region Collections
        public ICollection<UserPermission>? Permissions { get; set; }
        #endregion
    }
}
