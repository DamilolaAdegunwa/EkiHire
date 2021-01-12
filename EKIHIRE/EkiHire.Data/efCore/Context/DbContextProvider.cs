using EkiHire.Data.UnitOfWork;
using Microsoft.EntityFrameworkCore;
using EkiHire.Data.Utils.Extensions;

namespace EkiHire.Data.efCore.Context
{
    public interface IDbContextProvider<out TDbContext>
       where TDbContext : DbContext
    {
        TDbContext GetDbContext();
    }

    public sealed class UnitOfWorkDbContextProvider<TDbContext> : IDbContextProvider<TDbContext>
      where TDbContext : DbContext
    {
        private readonly IUnitOfWork _unitOfWork;


        public UnitOfWorkDbContextProvider(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public TDbContext GetDbContext()
        {
            return _unitOfWork.GetDbContext<TDbContext>();
        }
    }
}