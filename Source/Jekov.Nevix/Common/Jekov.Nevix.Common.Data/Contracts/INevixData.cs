namespace Jekov.Nevix.Common.Data.Contracts
{
    using Jekov.Nevix.Common.Data.Repositories.Contracts;
    using Jekov.Nevix.Common.Models;
    using System;

    public interface INevixData : IDisposable
    {
        IRepository<MediaFolder> Folders { get; }

        IRepository<MediaFile> Files { get; }

        IRepository<NevixUser> Users { get; }

        int SaveChanges();
    }
}