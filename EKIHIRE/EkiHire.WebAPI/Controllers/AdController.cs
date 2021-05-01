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
using EkiHire.Core.Domain.Entities;

namespace EkiHire.WebAPI.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class AdController : BaseController
    {
        private readonly IAdService adService;
        private readonly ICategoryService categoryService;
        public AdController(IAdService adService, ICategoryService categoryService)
        {
            this.adService = adService;
            this.categoryService = categoryService;
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

        [HttpPost, Route("CreateItem")]
        public async Task<IServiceResponse<bool>> CreateItem(ItemDTO model)
        {
            return await HandleApiOperationAsync(async () =>
            {
                bool result = await adService.CreateItem(model, User.Identity.Name);
                var response = new ServiceResponse<bool>(result);
                return response;
            });
        }

        [HttpPost, Route("EditItemKeywords")]
        public async Task<IServiceResponse<bool>> EditItemKeywords(List<string> keywords, long ItemId)
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
        //[AllowAnonymous]
        [HttpGet]
        [Route("GetCategories")]
        public async Task<IServiceResponse<IEnumerable<CategoryDTO>>> GetCategories()
        {
            return await HandleApiOperationAsync(async () => {
                var response = new ServiceResponse<IEnumerable<CategoryDTO>>();
                var data = await categoryService.GetCategories();
                response.Object = data;
                return response;
            });
        }

        [HttpGet]
        [Route("GetCategory/{id}")]
        public async Task<IServiceResponse<CategoryDTO>> GetCategory(long id)
        {
            return await HandleApiOperationAsync(async () => {
                var response = new ServiceResponse<CategoryDTO>();
                var data = await categoryService.GetCategory(id);
                response.Object = data;
                return response;
            });
        }

        [HttpGet]
        [Route("GetSubcategoriesByCategoryId/{categoryId}")]
        public async Task<IServiceResponse<IEnumerable<SubcategoryDTO>>> GetSubcategoriesByCategoryId(long categoryId)
        {
            return await HandleApiOperationAsync(async () => {
                var response = new ServiceResponse<IEnumerable<SubcategoryDTO>>();
                var data = await categoryService.GetSubcategoriesByCategoryId(categoryId);
                response.Object = data;
                return response;
            });
        }
        //review
        [HttpPost]
        [Route("AdFeedbackByUser")]
        public async Task<IServiceResponse<IEnumerable<AdFeedback>>> AdFeedbackByUser(long[] adIds = null)
        {
            return await HandleApiOperationAsync(async () => {
                var response = new ServiceResponse<IEnumerable<AdFeedback>>();
                var data = await adService.AdFeedbackByUser(User.Identity.Name, adIds);
                response.Object = data;
                return response;
            });
        }
        [HttpPost]
        [Route("AdFeedbackForUser")]
        public async Task<IServiceResponse<IEnumerable<AdFeedback>>> AdFeedbackForUser(long[] adIds = null)
        {
            return await HandleApiOperationAsync(async () => {
                var response = new ServiceResponse<IEnumerable<AdFeedback>>();
                var data = await adService.AdFeedbackForUser(User.Identity.Name, adIds);
                response.Object = data;
                return response;
            });
        }

        [HttpGet]
        [Route("GetFollowers")]
        public async Task<IServiceResponse<IEnumerable<Follow>>> GetFollowers(string username = null)
        {
            return await HandleApiOperationAsync(async () => {
                var response = new ServiceResponse<IEnumerable<Follow>>();
                var data = await adService.GetFollowers(User.Identity.Name);
                response.Object = data;
                return response;
            });
        }

        [HttpGet]
        [Route("GetFollowing")]
        public async Task<IServiceResponse<IEnumerable<Follow>>> GetFollowing(string username = null)
        {
            return await HandleApiOperationAsync(async () => {
                var response = new ServiceResponse<IEnumerable<Follow>>();
                var data = await adService.GetFollowing(username??User.Identity.Name);
                response.Object = data;
                return response;
            });
        }

        [HttpPost]
        [Route("AddCategory")]
        public async Task<IServiceResponse<bool>> AddCategory(CategoryDTO model, string username = null)
        {
            return await HandleApiOperationAsync(async () => {
                var response = new ServiceResponse<bool>();
                var data = await categoryService.AddCategory(model, username??User.Identity.Name);
                response.Object = data;
                return response;
            });
        }

        [HttpPost]
        [Route("AddSubcategory")]
        public async Task<IServiceResponse<bool>> AddSubcategory(SubcategoryDTO model, string username = null)
        {
            return await HandleApiOperationAsync(async () => {
                var response = new ServiceResponse<bool>();
                var data = await categoryService.AddSubcategory(model, username ?? User.Identity.Name);
                response.Object = data;
                return response;
            });
        }

        [HttpPost]
        [Route("GetAllItemGroupsForSubcategory")]
        public async Task<IServiceResponse<List<IGrouping<string, Item>>>> GetAllItemGroupsForSubcategory(long subId, string username = null)
        {
            return await HandleApiOperationAsync(async () => {
                var response = new ServiceResponse<List<IGrouping<string, Item>>>();
                var data = await categoryService.GetAllItemGroupsForSubcategory(subId, username ?? User.Identity.Name);
                response.Object = data;
                return response;
            });
        }
        #region APIs in progress
        //update profile 
        //Adverts
        //Followers
        //Following
        //Basket
        //Statistics
        //Reviews
        //PremiumPackages
        //Messages
        //NewAdsOnSelectedCategory
        //Explore
        //Post
        //Market
        //inviteFriends
        //Help(we run on Intercom)
        //settings
        //faq, t & c, signout
        #endregion
    }
}
//ad, categories