using Domain.Enums;
using System.Collections.Generic;
using WebAPI.ViewModels.Common;

namespace WebAPI.ViewModels
{
    public class UserDisplayViewModel : EntityViewModel
    {
        public string Username { get; set; } = null!;
        public string? Name { get; set; }
        public string? Lastname { get; set; }
        public string? Email { get; set; }
        public IEnumerable<PermissionEnum>? Permissions { get; set; }
    }
}
