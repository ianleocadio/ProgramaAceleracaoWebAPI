using Database.Configurations.Interfaces;
using Domain.Core.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Database.Configurations.Common
{
    public abstract class EntityBaseConfig<TEntity> : IEntityTypeConfiguration<TEntity>, IEntityConfig
        where TEntity : Entity<TEntity>, new()
    {
        public void Configure(EntityTypeBuilder<TEntity> builder)
        {
            ConfigureBase(builder);
            ConfigureEntity(builder);
        }

        public void ConfigureBase(EntityTypeBuilder<TEntity> builder)
        {
            builder.HasKey(c => c.ID);
        }

        public abstract void ConfigureEntity(EntityTypeBuilder<TEntity> builder);
    }
}
