using Database.Configurations.Common;
using Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Database.Configurations
{
    public class UserPermissionConfig : EntityBaseConfig<UserPermission>
    {
        public override void ConfigureEntity(EntityTypeBuilder<UserPermission> builder)
        {
            builder.Property(p => p.Permission)
                .IsRequired();

            builder.HasOne(p => p.User)
                .WithMany(u => u!.Permissions)
                .HasForeignKey(p => p.UserID)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasIndex(p => p.UserID);
        }
    }
}
