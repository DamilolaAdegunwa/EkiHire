using EkiHire.Core.Domain.Entities;
using EkiHire.Data.Repository;
using System.Threading.Tasks;
using EkiHire.Core.Domain.Entities;

namespace EkiHire.Business.Services
{
    public interface IErrorCodeService
    {
        Task<ErrorCode> GetErrorByCodeAsync(string errorCode);
    }

    public class ErrorCodeService : IErrorCodeService
    {
        readonly IRepository<ErrorCode> _repository;

        public ErrorCodeService(IRepository<ErrorCode> repository)
        {
            _repository = repository;
        }

        public Task<ErrorCode> GetErrorByCodeAsync(string errorCode)
        {
            return _repository.FirstOrDefaultAsync(e => e.Code.ToLower() == errorCode.ToLower());
        }
    }
}