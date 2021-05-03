using Database.Configurations.Common;
using Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Database.Configurations
{
    public class UserConfig : EntityBaseConfig<User>
    {
        public override void ConfigureEntity(EntityTypeBuilder<User> builder)
        {
            builder.Property(u => u.Username)
                .HasMaxLength(60)
                .IsRequired();

            builder.Property(u => u.Password)
                .IsRequired();

            builder.Property(u => u.Hash)
                .IsRequired();

            builder.Property(u => u.Name)
                .HasMaxLength(30);

            builder.Property(u => u.Lastname)
                .HasMaxLength(100);

            builder.Property(u => u.Email);

            builder.HasMany(u => u.Permissions)
                .WithOne()
                .HasForeignKey(p => p.UserID)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasIndex(u => u.Username)
                .IsUnique();
        }
    }
}
