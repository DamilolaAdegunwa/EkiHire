using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EkiHire.Business.Services;
using EkiHire.Core.Domain.DataTransferObjects;
using Microsoft.AspNetCore.Http;
using EkiHire.WebAPI.ViewModels;
using EkiHire.Core.Model;
using Microsoft.AspNetCore.Authorization;

namespace EkiHire.WebAPI.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class AdController : BaseController
    {
        private readonly IAdService adService;
        public AdController(IAdService adService)
        {
            this.adService = adService;
        }

        [HttpPost, Route("AddAd")]
        public async Task<IServiceResponse<bool>> AddAd(AdDTO model)
        {
            var x = System.Threading.Thread.CurrentPrincipal.Identity.Name;
            var y = "";
            return await HandleApiOperationAsync(async () =>
            {
                bool result = await adService.AddAd(model);
                var response = new ServiceResponse<bool>(result);
                return response;
            });
        }

        [HttpPost, Route("CloseAd")]
        public async Task<IServiceResponse<bool>> CloseAd(long adId)
        {
            return await HandleApiOperationAsync(async () =>
            {
                bool result = await adService.CloseAd(adId);
                var response = new ServiceResponse<bool>(result);
                return response;
            });
        }

        [HttpPost, Route("EditAd")]
        public async Task<IServiceResponse<bool>> EditAd(long adId)
        {
            return await HandleApiOperationAsync(async () =>
            {
                bool result = await adService.EditAd(adId);
                var response = new ServiceResponse<bool>(result);
                return response;
            });
        }

        [HttpPost, Route("PromoteAd")]
        public async Task<IServiceResponse<bool>> PromoteAd(long adId)
        {
            return await HandleApiOperationAsync(async () =>
            {
                bool result = await adService.PromoteAd(adId);
                var response = new ServiceResponse<bool>(result);
                return response;
            });
        }

        [HttpPost, Route("CreateAdItem")]
        public async Task<IServiceResponse<bool>> CreateAdItem(ItemDTO model)
        {
            return await HandleApiOperationAsync(async () =>
            {
                bool result = await adService.CreateAdItem(model);
                var response = new ServiceResponse<bool>(result);
                return response;
            });
        }

        [HttpPost, Route("AddAdToItem")]
        public async Task<IServiceResponse<bool>> AddAdToItem(string[] keywords, long ItemId)
        {
            return await HandleApiOperationAsync(async () =>
            {
                bool result = await adService.AddAdToItem(keywords, ItemId);
                var response = new ServiceResponse<bool>(result);
                return response;
            });
        }

        [HttpPost, Route("GroupAdItem")]
        public async Task<IServiceResponse<bool>> GroupAdItem(long[] ItemIds, string groupname)
        {
            return await HandleApiOperationAsync(async () =>
            {
                bool result = await adService.GroupAdItem(ItemIds, groupname);
                var response = new ServiceResponse<bool>(result);
                return response;
            });
        }
        //cart
        [HttpPost, Route("AddToCart")]
        public async Task<IServiceResponse<bool>> AddToCart(long adId)
        {
            return await HandleApiOperationAsync(async () =>
            {
                bool result = await adService.AddToCart(adId);
                var response = new ServiceResponse<bool>(result);
                return response;
            });
        }
        [HttpPost, Route("RemoveFromCart")]
        public async Task<IServiceResponse<bool>> RemoveFromCart(long adId)
        {
            return await HandleApiOperationAsync(async () =>
            {
                bool result = await adService.RemoveFromCart(adId);
                var response = new ServiceResponse<bool>(result);
                return response;
            });
        }
        //filter, search and scan ads
        [HttpPost, Route("Search")]
        public async Task<IServiceResponse<IEnumerable<AdDTO>>> Search(SearchVM model)
        {
            return await HandleApiOperationAsync(async () => {
                IEnumerable<AdDTO> result = await adService.Search(model);
                var response = new ServiceResponse<IEnumerable<AdDTO>>(result);
                return response;
            });
        }
    }
}
