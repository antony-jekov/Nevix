namespace Jekov.Nevix.Common.Data.Contracts
{
    using Jekov.Nevix.Common.Models;
    using System;
    using System.Collections.Generic;
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    public interface INevixDbContext : IDisposable
    {
        IDbSet<NevixUser> Users { get; set; }

        DbContext DbContext { get; }

        int SaveChanges();

        DbEntityEntry<TEntity> Entry<TEntity>(TEntity entity) where TEntity : class;

        IDbSet<T> Set<T>() where T : class;
    }
}
