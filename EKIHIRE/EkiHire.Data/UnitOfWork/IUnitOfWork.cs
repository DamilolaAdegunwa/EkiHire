using System;
using System.Threading.Tasks;

namespace EkiHire.Data.UnitOfWork
{
    public interface IUnitOfWork : IDisposable
    {
        void SaveChanges();
        Task<int> SaveChangesAsync();

        void BeginTransaction();
        void Commit();
        void Rollback();
    }
}