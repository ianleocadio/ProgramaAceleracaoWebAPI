using Domain.Core.Models;
using Domain.Enums;

namespace Domain.Models
{
    public class UserPermission : Entity<UserPermission>
    {
        public PermissionEnum Permission { get; set; }
        public long? UserID { get; set; }

        #region Navigations
        public User? User { get; set; }
        #endregion
    }
}
