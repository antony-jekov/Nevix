namespace Jekov.Nevix.Common.Data
{
    using Jekov.Nevix.Common.Data.Contracts;
    using Jekov.Nevix.Common.Data.Repositories;
    using Jekov.Nevix.Common.Data.Repositories.Contracts;
    using Jekov.Nevix.Common.Models;
    using System;
    using System.Collections.Generic;

    public class NevixData : INevixData
    {
        private readonly INevixDbContext context;

        private readonly Dictionary<Type, object> repositories = new Dictionary<Type, object>();

        public NevixData()
            : this(new NevixDbContext())
        {
        }

        public NevixData(INevixDbContext context)
        {
            this.context = context;
        }

        public IRepository<NevixUser> Users
        {
            get { return this.GetRepository<NevixUser>(); }
        }

        public INevixDbContext Context
        {
            get { return this.context; }
        }

        public int SaveChanges()
        {
            return this.context.SaveChanges();
        }

        public void Dispose()
        {
            if (this.context != null)
            {
                this.context.Dispose();
            }
        }

        private IRepository<T> GetRepository<T>() where T : class
        {
            if (!this.repositories.ContainsKey(typeof(T)))
            {
                var type = typeof(GenericRepository<T>);

                this.repositories.Add(typeof(T), Activator.CreateInstance(type, this.context));
            }

            return (IRepository<T>)this.repositories[typeof(T)];
        }
    }
}