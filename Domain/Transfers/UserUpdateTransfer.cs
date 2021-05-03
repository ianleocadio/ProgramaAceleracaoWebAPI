
using Domain.Transfers.Common;

namespace Domain.Transfers
{
    public class UserUpdateTransfer : EntityTransfer
    {
        public string? Name { get; set; }
        public string? Lastname { get; set; }
        public string? Email { get; set; }
    }
}
