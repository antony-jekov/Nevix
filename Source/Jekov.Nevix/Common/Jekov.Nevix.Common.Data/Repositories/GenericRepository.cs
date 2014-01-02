namespace Jekov.Nevix.Common.Data.Repositories
{
    using Jekov.Nevix.Common.Data.Contracts;
    using Jekov.Nevix.Common.Data.Repositories.Contracts;
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    using System.Linq;

    public class GenericRepository<T> : IRepository<T> where T : class
    {
        public GenericRepository(INevixDbContext context)
        {
            if (context == null)
            {
                throw new ArgumentNullException("context", "The context must not be null!");
            }

            this.Context = context;
            this.DbSet = this.Context.Set<T>();
        }

        protected INevixDbContext Context { get; set; }

        protected IDbSet<T> DbSet { get; set; }

        public IQueryable<T> All()
        {
            return this.DbSet.AsQueryable();
        }

        public T GetById(int id)
        {
            return this.DbSet.Find(id);
        }

        public void Add(T entity)
        {
            DbEntityEntry entry = this.Context.Entry(entity);
            if (entry.State != EntityState.Detached)
            {
                entry.State = EntityState.Added;
            }
            else
            {
                this.DbSet.Add(entity);
            }
        }

        public void Update(T entity)
        {
            DbEntityEntry entry = this.Context.Entry(entity);
            if (entry.State == EntityState.Detached)
            {
                this.DbSet.Attach(entity);
            }

            entry.State = EntityState.Modified;
        }

        public void Delete(T entity)
        {
            DbEntityEntry entry = this.Context.Entry(entity);
            if (entry.State != EntityState.Deleted)
            {
                entry.State = EntityState.Deleted;
            }
            else
            {
                this.DbSet.Attach(entity);
                this.DbSet.Remove(entity);
            }
        }

        public void Delete(int id)
        {
            var entity = this.GetById(id);

            if (entity != null)
            {
                this.Delete(entity);
            }
        }

        public void Detach(T entity)
        {
            DbEntityEntry entry = this.Context.Entry(entity);

            entry.State = EntityState.Detached;
        }
    }
}