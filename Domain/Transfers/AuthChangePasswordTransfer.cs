using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Domain.Transfers
{
    public class AuthChangePasswordTransfer
    {
        public string CurrentPassword { get; set; } = null!;
        public string NewPassword { get; set; } = null!;
        public string RepeatNewPassword { get; set; } = null!;
    }
}
