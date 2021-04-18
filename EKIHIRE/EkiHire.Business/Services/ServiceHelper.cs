using EkiHire.Core.Caching;
using EkiHire.Core.Exceptions;
using Microsoft.AspNetCore.Http;
using System;
using System.Security.Claims;
using System.Threading.Tasks;

namespace EkiHire.Business.Services
{
    public interface IServiceHelper
    {
        Task<EkiHireGenericException> GetExceptionAsync(string errorCode);
        T GetOrUpdateCacheItem<T>(string key, Func<T> update, int? cacheSeconds = null);
        string GetCurrentUserEmail();
        int? GetCurrentUserId();
        Task<ClaimsPrincipal> GetCurrentUserAsync();
        string GetCurrentUserTerminal();
        Uri GetAbsoluteUri();
    }
    public class ServiceHelper : IServiceHelper
    {
        readonly IErrorCodeService _errorCodesSvc;
        readonly ICacheManager _cacheManager;
        readonly IHttpContextAccessor _httpContext;
        public ServiceHelper(IErrorCodeService errorCodesSvc, ICacheManager cacheManager, IHttpContextAccessor httpContext)
        {
            _errorCodesSvc = errorCodesSvc;
            _cacheManager = cacheManager;
            _httpContext = httpContext;
        }

        public string GetCurrentUserEmail()
        {
            var email = _httpContext.HttpContext?.User?.FindFirst("name")?.Value;
            return !string.IsNullOrEmpty(email) ? email : "Anonymous";
        }
      
        public string GetCurrentUserTerminal()
        {
            var terminal = _httpContext.HttpContext?.User?.FindFirst("location")?.Value;
            return !string.IsNullOrEmpty(terminal) ? terminal : "";
        }
        public Uri GetAbsoluteUri()
        {
            var request = _httpContext.HttpContext.Request;

            var uriBuilder = new UriBuilder
            {
                Scheme = request.Scheme,
                Host = request.Host.Host,
                Path = request.Path.ToString(),
                Query = request.QueryString.ToString()
            };

            return uriBuilder.Uri;
        }

        public int? GetCurrentUserId()
        {
            var id = _httpContext.HttpContext?.User?.FindFirst("id")?.Value;
            return id is null ? (int?) null : int.Parse(id);
        }

        public async Task<ClaimsPrincipal> GetCurrentUserAsync()
        {
            var user = _httpContext.HttpContext?.User;
            return user;
        }

        public async Task<EkiHireGenericException> GetExceptionAsync(string errorCode)
        {
            var error = await GetOrUpdateCacheItem(errorCode, async () => await _errorCodesSvc.GetErrorByCodeAsync(errorCode));

            if (error is null)
                throw new EkiHireGenericException(errorCode, errorCode);

            return new EkiHireGenericException(error.Message, error.Code);
        }

        public T GetOrUpdateCacheItem<T>(string key, Func<T> update, int? cacheSeconds = null)
        {
            var item = cacheSeconds is null ? _cacheManager.Get(key, update) : _cacheManager.Get(key, cacheSeconds.Value, update);
            return (T) item;
        }
    }
}