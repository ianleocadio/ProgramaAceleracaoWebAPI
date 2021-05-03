using Domain.Core.Models.Interfaces;

namespace Domain.Core.Models
{
    public class Entity<TEntity> : IEntity 
        where TEntity : IEntity, new()
    {
        public long? ID { get; set; }
    }
}
