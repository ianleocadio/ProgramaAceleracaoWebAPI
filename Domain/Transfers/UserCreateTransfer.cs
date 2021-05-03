using Domain.Enums;
using System.Collections.Generic;

namespace Domain.Transfers
{
    public class UserCreateTransfer
    {
        public string Username { get; set; } = null!;
        public string Password { get; set; } = null!;
        public string? Name { get; set; }
        public string? Lastname { get; set; }
        public string? Email { get; set; }
        public IEnumerable<PermissionEnum>? Permissions { get; set; }
    }
}
