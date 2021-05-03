using Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Transfers
{
    public class UserPermissionUpdateTransfer
    {
        public long UserID { get; set; }
        public IEnumerable<PermissionEnum> Permissions { get; set; } = null!;
    }
}
