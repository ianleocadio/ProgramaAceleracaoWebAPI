using Domain.Core.Models;

namespace Service.Interfaces.Common
{
    public interface IEntityConverterService<TEntity, TTransfer>
        where TEntity : Entity<TEntity>, new()
    {
        TEntity TransferToEntity(TTransfer transfer, TEntity entity);
        TEntity TransferToEntity(TTransfer transfer);
    }
}
