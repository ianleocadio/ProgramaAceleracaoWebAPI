using Database.Configurations.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Reflection;

namespace Database.Contexts
{
    public class DbContextBase : DbContext
    {
        private static bool ContextModelTypesFilter(Type type) => typeof(IEntityConfig).IsAssignableFrom(type) && type.IsClass && !type.IsAbstract && type.IsPublic;

        public DbContextBase(DbContextOptions options) : base(options)
        {
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly(), t => ContextModelTypesFilter(t));
        }
    }
}
