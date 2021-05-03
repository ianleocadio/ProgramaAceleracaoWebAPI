using Domain.Enums;
using Microsoft.AspNetCore.Authorization;
using System;
using System.Linq;

namespace WebAPI.Attributes
{
    [AttributeUsage(AttributeTargets.Method | AttributeTargets.Class, AllowMultiple = true, Inherited = false)]
    public class HasPermission : AuthorizeAttribute
    {
        public HasPermission(params PermissionEnum[] permissions) : base("Bearer")
        {
            if (permissions == null || !permissions.Any())
            {
                return;
            }

            this.Roles = string.Join(",", permissions.Select(p => (int)p));
        }
    }
}
