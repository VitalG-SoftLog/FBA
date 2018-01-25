using System;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;

namespace FBA.Shared.Interfaces
{
    public interface IDataContext : IDisposable
    {
        Database Database { get; }
        
        int SaveChanges();
        DbEntityEntry<TEntity> Entry<TEntity>(TEntity entity) where TEntity : class;

        DbEntityEntry Entry(object entity);
    }
}
