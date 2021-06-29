using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using EkiHire.Core.Caching;
using EkiHire.Core.Configuration;
using EkiHire.Core.Exceptions;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
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
        string UploadPhoto(Stream stream);
        byte[] ObjectToByteArray(Object obj);
        Object ByteArrayToObject(byte[] arrBytes);
    }
    public class ServiceHelper : IServiceHelper
    {
        readonly IErrorCodeService _errorCodesSvc;
        readonly ICacheManager _cacheManager;
        readonly IHttpContextAccessor _httpContext;
        private readonly AppConfig appConfig;
        public ServiceHelper(IErrorCodeService errorCodesSvc, ICacheManager cacheManager, IHttpContextAccessor httpContext, IOptions<AppConfig> _appConfig)
        {
            _errorCodesSvc = errorCodesSvc;
            _cacheManager = cacheManager;
            _httpContext = httpContext;
            appConfig = _appConfig.Value;
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

            //if (error is null)
            //    throw new EkiHireGenericException(errorCode, errorCode);

            //return new EkiHireGenericException(error.Message, error.Code);
            return new EkiHireGenericException(errorCode, errorCode);
        }

        public T GetOrUpdateCacheItem<T>(string key, Func<T> update, int? cacheSeconds = null)
        {
            var item = cacheSeconds is null ? _cacheManager.Get(key, update) : _cacheManager.Get(key, cacheSeconds.Value, update);
            return (T) item;
        }
        public string UploadPhoto(Stream stream)
        {
            stream.Position = 0;
            Account account = new Account(
             appConfig.CLOUDINARY_CLOUD_NAME,
             appConfig.CLOUDINARY_API_KEY,
             appConfig.CLOUDINARY_API_SECRET);

            Cloudinary cloudinary = new Cloudinary(account);
            var uploadParams = new CloudinaryDotNet.Actions.ImageUploadParams()
            {
                File = new CloudinaryDotNet.FileDescription(Guid.NewGuid().ToString(), stream),
            };

            ImageUploadResult uploadResult = cloudinary.Upload(uploadParams);
            //var result = cloudinary.Api.UrlImgUp.BuildUrl(String.Format("ekihire_img{0}.{1}", uploadResult.PublicId, uploadResult.Format));
            var result = uploadResult.Url.ToString();
            return result;
        }

        // Convert an object to a byte array
        public byte[] ObjectToByteArray(Object obj)
        {
            BinaryFormatter bf = new BinaryFormatter();
            using (var ms = new MemoryStream())
            {
                bf.Serialize(ms, obj);
                return ms.ToArray();
            }
        }

        // Convert a byte array to an Object
        public Object ByteArrayToObject(byte[] arrBytes)
        {
            using (var memStream = new MemoryStream())
            {
                var binForm = new BinaryFormatter();
                memStream.Write(arrBytes, 0, arrBytes.Length);
                memStream.Seek(0, SeekOrigin.Begin);
                var obj = binForm.Deserialize(memStream);
                return obj;
            }
        }
    }

    public class EkiHireEqualityComparer : IEqualityComparer<long>
    {
        public new bool Equals(long x, long y)
        {
            return x == y;
        }

        public int GetHashCode(long obj)
        {
            return 10;
        }
    }
}