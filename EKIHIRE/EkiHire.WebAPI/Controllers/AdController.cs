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
        {//System.Threading.Thread.CurrentPrincipal.Identity.Name;

            return await HandleApiOperationAsync(async () =>
            {
                bool result = await adService.AddAd(model, User.Identity.Name);
                var response = new ServiceResponse<bool>(result);
                return response;
            });
        }

        [HttpPost, Route("CloseAd")]
        public async Task<IServiceResponse<bool>> CloseAd(long adId)
        {
            return await HandleApiOperationAsync(async () =>
            {
                bool result = await adService.CloseAd(adId, User.Identity.Name);
                var response = new ServiceResponse<bool>(result);
                return response;
            });
        }

        [HttpPost, Route("EditAd")]
        public async Task<IServiceResponse<bool>> EditAd(AdDTO dto, long adId)
        {
            return await HandleApiOperationAsync(async () =>
            {
                bool result = await adService.EditAd(dto, adId, User.Identity.Name);
                var response = new ServiceResponse<bool>(result);
                return response;
            });
        }

        [HttpPost, Route("PromoteAd")]
        public async Task<IServiceResponse<bool>> PromoteAd(long adId)
        {
            return await HandleApiOperationAsync(async () =>
            {
                bool result = await adService.PromoteAd(adId, User.Identity.Name);
                var response = new ServiceResponse<bool>(result);
                return response;
            });
        }

        [HttpPost, Route("CreateAdItem")]
        public async Task<IServiceResponse<bool>> CreateAdItem(ItemDTO model)
        {
            return await HandleApiOperationAsync(async () =>
            {
                bool result = await adService.CreateItem(model, User.Identity.Name);
                var response = new ServiceResponse<bool>(result);
                return response;
            });
        }

        [HttpPost, Route("AddAdToItem")]
        public async Task<IServiceResponse<bool>> AddAdToItem(List<string> keywords, long ItemId)
        {
            return await HandleApiOperationAsync(async () =>
            {
                bool result = await adService.EditItemKeywords(keywords, ItemId, User.Identity.Name);
                var response = new ServiceResponse<bool>(result);
                return response;
            });
        }

        [HttpPost, Route("GroupAdItem")]
        public async Task<IServiceResponse<bool>> GroupAdItem(long[] ItemIds, string groupname)
        {
            return await HandleApiOperationAsync(async () =>
            {
                bool result = await adService.GroupAdItems(ItemIds, groupname, User.Identity.Name);
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
                bool result = await adService.AddAdToCart(adId, User.Identity.Name);
                var response = new ServiceResponse<bool>(result);
                return response;
            });
        }
        [HttpPost, Route("RemoveFromCart")]
        public async Task<IServiceResponse<bool>> RemoveFromCart(long adId)
        {
            return await HandleApiOperationAsync(async () =>
            {
                bool result = await adService.RemoveAdFromCart(adId, User.Identity.Name);
                var response = new ServiceResponse<bool>(result);
                return response;
            });
        }
        //filter, search and scan ads
        [HttpPost, Route("Search")]
        public async Task<IServiceResponse<IEnumerable<AdDTO>>> Search(SearchVM model)
        {
            return await HandleApiOperationAsync(async () => {
                IEnumerable<AdDTO> result = await adService.Search(model, User.Identity.Name);
                var response = new ServiceResponse<IEnumerable<AdDTO>>(result);
                return response;
            });
        }
    }
}
