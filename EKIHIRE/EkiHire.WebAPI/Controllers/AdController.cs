using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EkiHire.Business.Services;
using EkiHire.Core.Domain.DataTransferObjects;
using Microsoft.AspNetCore.Http;

namespace EkiHire.WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AdController : BaseController
    {

        public AdController()
        {

        }

        [HttpPost, Route("AddAd")]
        public async Task<IServiceResponse<bool>> AddAd(AdDTO model)
        {
            return await HandleApiOperationAsync(async () =>
            {
                bool result = true;
                var response = new ServiceResponse<bool>(result);
                return response;
            });
        }

        [HttpPost, Route("CloseAd")]
        public async Task<IServiceResponse<bool>> CloseAd(long Id)
        {
            return await HandleApiOperationAsync(async () =>
            {
                bool result = true;
                var response = new ServiceResponse<bool>(result);
                return response;
            });
        }

        [HttpPost, Route("EditAd")]
        public async Task<IServiceResponse<bool>> EditAd(long Id)
        {
            return await HandleApiOperationAsync(async () =>
            {
                bool result = true;
                var response = new ServiceResponse<bool>(result);
                return response;
            });
        }

        [HttpPost, Route("PromoteAd")]
        public async Task<IServiceResponse<bool>> PromoteAd(long Id)
        {
            return await HandleApiOperationAsync(async () =>
            {
                bool result = true;
                var response = new ServiceResponse<bool>(result);
                return response;
            });
        }

        public async Task<IServiceResponse<bool>> CreateAdItem(long[] AdIds)
        {
            return await HandleApiOperationAsync(async () =>
            {
                bool result = true;
                var response = new ServiceResponse<bool>(result);
                return response;
            });
        }

        public async Task<IServiceResponse<bool>> AddAdToItem(long[] AdIds, long ItemId)
        {
            return await HandleApiOperationAsync(async () =>
            {
                bool result = true;
                var response = new ServiceResponse<bool>(result);
                return response;
            });
        }

        public async Task<IServiceResponse<bool>> GroupAdItem(long[] ItemIds, string groupname)
        {
            return await HandleApiOperationAsync(async () =>
            {
                bool result = true;
                var response = new ServiceResponse<bool>(result);
                return response;
            });
        }
        //cart
        [HttpPost, Route("AddToCart")]
        public async Task<IServiceResponse<bool>> AddToCart(long Id)
        {
            return await HandleApiOperationAsync(async () =>
            {
                bool result = true;
                var response = new ServiceResponse<bool>(result);
                return response;
            });
        }
        public async Task<IServiceResponse<bool>> RemoveFromCart(long Id)
        {
            return await HandleApiOperationAsync(async () =>
            {
                bool result = true;
                var response = new ServiceResponse<bool>(result);
                return response;
            });
        }
    }
}
