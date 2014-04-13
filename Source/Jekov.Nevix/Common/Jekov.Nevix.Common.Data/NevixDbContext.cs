namespace Jekov.Nevix.Common.Data
{
    using Jekov.Nevix.Common.Data.Contracts;
    using Jekov.Nevix.Common.Models;
    using System.Data.Entity;

    public class NevixDbContext : DbContext, INevixDbContext
    {
        public NevixDbContext()
            : this("DefaultConnection")
        {
        }

        public NevixDbContext(string nameOrConnectionString)
            : base(nameOrConnectionString)
        {
        }

        public IDbSet<NevixUser> Users { get; set; }

        public IDbSet<ConfirmUser> ConfirmUser { get; set; }

        public IDbSet<ForgottenPasswordRequest> ForgottenPassword { get; set; }

        public DbContext DbContext
        {
            get
            {
                return this;
            }
        }

        public override int SaveChanges()
        {
            return base.SaveChanges();
        }

        public new IDbSet<T> Set<T>() where T : class
        {
            return base.Set<T>();
        }
    }
}