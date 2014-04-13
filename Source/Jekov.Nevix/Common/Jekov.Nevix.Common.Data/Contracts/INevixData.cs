namespace Jekov.Nevix.Common.Data.Contracts
{
    using Jekov.Nevix.Common.Data.Repositories.Contracts;
    using Jekov.Nevix.Common.Models;
    using System;

    public interface INevixData : IDisposable
    {
        IRepository<NevixUser> Users { get; }

        IRepository<ForgottenPasswordRequest> ForgottenPasswordRequests { get; }

        IRepository<ConfirmUser> ConfirmUser { get; }

        int SaveChanges();
    }
}