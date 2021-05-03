using Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebAPI.ViewModels
{
    public class PermissionUpdateViewModel
    {
        public long UserID { get; set; }
        public IEnumerable<PermissionEnum> Permissions { get; set; } = null!;
    }
}
