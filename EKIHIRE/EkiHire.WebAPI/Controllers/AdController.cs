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
using Microsoft.AspNetCore.Cors;
using EkiHire.Core.Domain.Entities.Enums;

namespace EkiHire.WebAPI.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class AdController : BaseController
    {
        private readonly IAdService adService;
        private readonly ICategoryService categoryService;
        private readonly IServiceHelper serviceHelper;
        public AdController(IAdService adService, ICategoryService categoryService, IServiceHelper serviceHelper)
        {
            this.adService = adService;
            this.categoryService = categoryService;
            this.serviceHelper = serviceHelper;
        }

        [HttpPost, Route("AddAd")]
        public async Task<IServiceResponse<bool>> AddAd(AdDTO model)
        {//working
            return await HandleApiOperationAsync(async () =>
            {
                bool result = await adService.AddAd(model, serviceHelper.GetCurrentUserEmail());
                var response = new ServiceResponse<bool>(result);
                return response;
            });
        }

        [HttpPost, Route("CloseAd")]
        public async Task<IServiceResponse<bool>> CloseAd(long adId)
        {//working
            return await HandleApiOperationAsync(async () =>
            {
                bool result = await adService.CloseAd(adId, serviceHelper.GetCurrentUserEmail());
                var response = new ServiceResponse<bool>(result);
                return response;
            });
        }

        [HttpPost, Route("EditAd/{adId}")]
        public async Task<IServiceResponse<bool>> EditAd(AdDTO dto, long adId)
        {//working
            return await HandleApiOperationAsync(async () =>
            {
                bool result = await adService.EditAd(dto, adId, serviceHelper.GetCurrentUserEmail());
                var response = new ServiceResponse<bool>(result);
                return response;
            });
        }

        [HttpPost, Route("PromoteAd")]
        public async Task<IServiceResponse<bool>> PromoteAd(long adId)
        {//working
            return await HandleApiOperationAsync(async () =>
            {
                bool result = await adService.PromoteAd(adId, serviceHelper.GetCurrentUserEmail());
                var response = new ServiceResponse<bool>(result);
                return response;
            });
        }

        [HttpPost, Route("CreateItem")]
        public async Task<IServiceResponse<bool>> CreateItem(ItemDTO model)
        {
            return await HandleApiOperationAsync(async () =>
            {
                bool result = await adService.CreateItem(model, serviceHelper.GetCurrentUserEmail());
                var response = new ServiceResponse<bool>(result);
                return response;
            });
        }

        [HttpPost, Route("EditItemKeywords")]
        public async Task<IServiceResponse<bool>> EditItemKeywords(List<string> keywords, long ItemId)
        {//working
            return await HandleApiOperationAsync(async () =>
            {
                bool result = await adService.EditItemKeywords(keywords, ItemId, serviceHelper.GetCurrentUserEmail());
                var response = new ServiceResponse<bool>(result);
                return response;
            });
        }

        [HttpPost, Route("GroupAdItem")]
        public async Task<IServiceResponse<bool>> GroupAdItem(long[] ItemIds, string groupname)
        {//working
            return await HandleApiOperationAsync(async () =>
            {
                bool result = await adService.GroupAdItems(ItemIds, groupname, serviceHelper.GetCurrentUserEmail());
                var response = new ServiceResponse<bool>(result);
                return response;
            });
        }
        //cart
        [HttpPost, Route("AddToCart")]
        public async Task<IServiceResponse<bool>> AddToCart(long adId)
        {//working
            return await HandleApiOperationAsync(async () =>
            {
                bool result = await adService.AddAdToCart(adId, serviceHelper.GetCurrentUserEmail());
                var response = new ServiceResponse<bool>(result);
                return response;
            });
        }
        [HttpPost, Route("RemoveFromCart")]
        public async Task<IServiceResponse<bool>> RemoveFromCart(long adId)
        {//woring
            return await HandleApiOperationAsync(async () =>
            {
                bool result = await adService.RemoveAdFromCart(adId, serviceHelper.GetCurrentUserEmail());
                var response = new ServiceResponse<bool>(result);
                return response;
            });
        }
        //filter, search and scan ads
        [HttpPost, Route("Search")]
        public async Task<IServiceResponse<IEnumerable<Ad>>> Search(SearchVM model)
        {
            return await HandleApiOperationAsync(async () =>
            {
                IEnumerable<Ad> result = await adService.SearchTest(model, serviceHelper.GetCurrentUserEmail());
                var response = new ServiceResponse<IEnumerable<Ad>>(result);
                return response;
            });
        }

        //[AllowAnonymous]
        [HttpGet]
        [Route("GetCategories")]
        [Route("GetCategories/{withOtherData:bool}")]
        public async Task<IServiceResponse<IEnumerable<CategoryDTO>>> GetCategories(bool withOtherData = false)
        {//IEnumerable<Category>
            return await HandleApiOperationAsync(async () => {
                var response = new ServiceResponse<IEnumerable<CategoryDTO>>();//
                var data = await categoryService.GetCategories(null,withOtherData);
                response.Object = data;
                return response;
            });
        }

        [HttpGet]
        [Route("GetCategory/{id}")]
        [Route("GetCategory/{id}/{withOtherData:bool}")]
        public async Task<IServiceResponse<CategoryDTO>> GetCategory(long id, bool withOtherData = false)
        {
            return await HandleApiOperationAsync(async () => {
                var response = new ServiceResponse<CategoryDTO>();
                var data = await categoryService.GetCategory(id, withOtherData);
                response.Object = data;
                return response;
            });
        }

        [HttpGet]
        [Route("GetSubcategoriesByCategoryId/{categoryId}")]
        public async Task<IServiceResponse<IEnumerable<SubcategoryDTO>>> GetSubcategoriesByCategoryId(long categoryId)
        {//working
            return await HandleApiOperationAsync(async () => {
                var response = new ServiceResponse<IEnumerable<SubcategoryDTO>>();
                var data = await categoryService.GetSubcategoriesByCategoryId(categoryId);
                response.Object = data;
                return response;
            });
        }
        //review
        [HttpPost]
        [Route("ReviewsGivenByUser")]
        public async Task<IServiceResponse<IEnumerable<AdFeedbackDTO>>> ReviewsGivenByUser(string username = null/*long[] adIds = null*/)
        {//working - no data
            var urn = username ?? serviceHelper.GetCurrentUserEmail();
            return await HandleApiOperationAsync(async () => {
                var response = new ServiceResponse<IEnumerable<AdFeedbackDTO>>();
                var data = await adService.ReviewsGivenByUser(urn/*, adIds*/);
                response.Object = data;
                return response;
            });
        }
        [HttpPost]
        [Route("ReviewsForAd/{adId}")]
        public async Task<IServiceResponse<IEnumerable<AdFeedbackDTO>>> ReviewsForAd(long adId/*long[] adIds = null*/)
        {//working - no data
            return await HandleApiOperationAsync(async () => {
                var response = new ServiceResponse<IEnumerable<AdFeedbackDTO>>();
                var data = await adService.ReviewsForAd(adId, serviceHelper.GetCurrentUserEmail()/*, adIds*/);
                response.Object = data;
                return response;
            });
        }

        [HttpGet]
        [Route("GetFollowers")]
        public async Task<IServiceResponse<IEnumerable<Follow>>> GetFollowers()
        {
            return await HandleApiOperationAsync(async () => {
                var response = new ServiceResponse<IEnumerable<Follow>>();
                var data = await adService.GetFollowers(serviceHelper.GetCurrentUserEmail());
                response.Object = data;
                return response;
            });
        }

        [HttpGet]
        [Route("GetFollowing")]
        public async Task<IServiceResponse<IEnumerable<Follow>>> GetFollowing()
        {
            return await HandleApiOperationAsync(async () => {
                var response = new ServiceResponse<IEnumerable<Follow>>();
                var data = await adService.GetFollowing(serviceHelper.GetCurrentUserEmail());
                response.Object = data;
                return response;
            });
        }

        [HttpPost]
        [Route("AddCategory")]
        public async Task<IServiceResponse<bool>> AddCategory(CategoryDTO model)
        {//tested!
            return await HandleApiOperationAsync(async () => {
                var response = new ServiceResponse<bool>();
                var data = await categoryService.AddCategory(model, serviceHelper.GetCurrentUserEmail());
                response.Object = data;
                return response;
            });
        }

        [HttpPost]
        [Route("AddSubcategory")]
        public async Task<IServiceResponse<bool>> AddSubcategory(SubcategoryDTO model)
        {//tested - working
            return await HandleApiOperationAsync(async () => {
                var response = new ServiceResponse<bool>();
                var data = await categoryService.AddSubcategory(model, serviceHelper.GetCurrentUserEmail());
                response.Object = data;
                return response;
            });
        }

        [HttpPost]
        [Route("GetAllItemGroupsForSubcategory")]
        public async Task<IServiceResponse<List<List<Item>>>> GetAllItemGroupsForSubcategory(long subId)
        {//working - no data returned
            return await HandleApiOperationAsync(async () => {
                var response = new ServiceResponse<List<List<Item>>>();
                var data = await categoryService.GetAllItemGroupsForSubcategory(subId, serviceHelper.GetCurrentUserEmail());
                response.Object = data;
                return response;
            });
        }

        [HttpPost]
        [Route("AddKeywords")]
        public async Task<IServiceResponse<bool>> AddKeywords(List<string> keywords, long subid)
        {//working
            return await HandleApiOperationAsync(async () => {
                var response = new ServiceResponse<bool>();
                var data = await adService.AddKeywords(keywords, subid, serviceHelper.GetCurrentUserEmail());
                response.Object = data;
                return response;
            });
        }

        [HttpPost]
        [Route("EditKeywords")]
        public async Task<IServiceResponse<bool>> EditKeywords(long kwId, string correctedWord)
        {//worked
            return await HandleApiOperationAsync(async () => {
                var response = new ServiceResponse<bool>();
                var data = await adService.EditKeywords(kwId, correctedWord,  serviceHelper.GetCurrentUserEmail());
                response.Object = data;
                return response;
            });
        }

        [HttpPost]
        [Route("DeleteKeywords")]
        public async Task<IServiceResponse<bool>> DeleteKeywords(long kwId)
        {
            return await HandleApiOperationAsync(async () => {
                var response = new ServiceResponse<bool>();
                var data = await adService.DeleteKeywords(kwId,serviceHelper.GetCurrentUserEmail());
                response.Object = data;
                return response;
            });
        }

        [HttpPost]
        [Route("GetKeywords")]
        public async Task<IServiceResponse<IEnumerable<Keyword>>> GetKeywords( long[] kwIds = null, long? subid = null)
        {//worked
            return await HandleApiOperationAsync(async () => {
                var response = new ServiceResponse<IEnumerable<Keyword>>();
                var data = await adService.GetKeywords(serviceHelper.GetCurrentUserEmail(), kwIds, subid);
                response.Object = data;
                return response;
            });
        }

        [HttpPost]
        [Route("GetAdPropertiesBySubcategory")]
        public async Task<IServiceResponse<IEnumerable<AdProperty>>> GetAdPropertiesBySubcategory(long subId)
        {//working
            return await HandleApiOperationAsync(async () => {
                var response = new ServiceResponse<IEnumerable<AdProperty>>();
                var data = await adService.GetAdPropertiesBySubcategory(subId, serviceHelper.GetCurrentUserEmail());
                response.Object = data;
                return response;
            });
        }

        [HttpPost]
        [Route("GetAdPropertiesWithValue")]
        public async Task<IServiceResponse<IEnumerable<AdPropertyValue>>> GetAdPropertiesWithValue(long adid)
        {//working
            return await HandleApiOperationAsync(async () => {
                var response = new ServiceResponse<IEnumerable<AdPropertyValue>>();
                var data = await adService.GetAdPropertiesWithValue(adid, serviceHelper.GetCurrentUserEmail());
                response.Object = data;
                return response;
            });
        }

        [HttpPost]
        [Route("AddAdProperty")]
        public async Task<IServiceResponse<bool>> AddAdProperty(AdPropertyDTO model)
        {//working
            return await HandleApiOperationAsync(async () => {
                var response = new ServiceResponse<bool>();
                var data = await adService.AddAdProperty(model,serviceHelper.GetCurrentUserEmail());
                response.Object = data;
                return response;
            });
        }

        [HttpPost]
        [Route("AddOrUpdateAdPropertyValue")]
        public async Task<IServiceResponse<bool>> AddOrUpdateAdPropertyValue(AdPropertyValueDTO model)
        {
            return await HandleApiOperationAsync(async () => {
                var response = new ServiceResponse<bool>();
                var data = await adService.AddOrUpdateAdPropertyValue(model, serviceHelper.GetCurrentUserEmail());
                response.Object = data;
                return response;
            });
        }

        [HttpPost]
        [Route("Trending")]
        [Route("Trending/{count}")]
        public async Task<IServiceResponse<IEnumerable<AdDTO>>> Trending(long count = 10)
        {
            var allowanonymous = string.IsNullOrWhiteSpace(serviceHelper.GetCurrentUserEmail()) ? true : false;
            return await HandleApiOperationAsync(async () => {
                var response = new ServiceResponse<IEnumerable<AdDTO>>();
                var data = await adService.Trending(count, serviceHelper.GetCurrentUserEmail(), allowanonymous);
                response.Object = data;
                return response;
            });
        }

        [HttpPost]
        [Route("GetAd/{Id}")]
        public async Task<IServiceResponse<AdDTO>> GetAd(long Id)
        {
            return await HandleApiOperationAsync(async () => {
                var response = new ServiceResponse<AdDTO>();
                //var data = await adService.GetAd(Id);
                var data = (await adService.Search(new SearchVM { AdId = Id }, serviceHelper.GetCurrentUserEmail())).FirstOrDefault();
                response.Object = data;
                return response;
            });
        }

        [HttpPost]
        [Route("UpdateAdStatus")]
        public async Task<IServiceResponse<bool>> UpdateAdStatus(long AdId, AdsStatus adsStatus)
        {
            return await HandleApiOperationAsync(async () => {
                var response = new ServiceResponse<bool>();
                var data = await adService.UpdateAdStatus(AdId, adsStatus);
                response.Object = data;
                return response;
            });
        }

        [HttpPost]
        [Route("GetUserCart")]
        public async Task<ServiceResponse<IEnumerable<CartItemDTO>>> GetUserCart()
        {
            return await HandleApiOperationAsync(async () => {
                var response = new ServiceResponse<IEnumerable<CartItemDTO>>();
                var data = await adService.GetCartItems(serviceHelper.GetCurrentUserEmail());
                response.Object = data;
                return response;
            });
        }
        [HttpPost]
        [Route("SaveRequestQuote")]
        public async Task<ServiceResponse<bool>> SaveRequestQuote(RequestQuoteDTO model)
        {
            return await HandleApiOperationAsync(async () => {
                var response = new ServiceResponse<bool>();
                var data = await adService.SaveRequestQuote(model, serviceHelper.GetCurrentUserEmail());
                response.Object = data;
                return response;
            });
        }

        [HttpPost]
        [Route("SaveReview")]
        public async Task<ServiceResponse<bool>> SaveReview(AdFeedbackDTO model/*, string username*/)
        {
            return await HandleApiOperationAsync(async () => {
                var response = new ServiceResponse<bool>();
                var data = await adService.SaveReview(model, serviceHelper.GetCurrentUserEmail());
                response.Object = data;
                return response;
            });
        }

        [HttpPost]
        [Route("ApplyForJob")]
        public async Task<ServiceResponse<bool>> ApplyForJob(JobApplicationDTO model, bool allowAnonymous = false)
        {
            return await HandleApiOperationAsync(async () => {
                var response = new ServiceResponse<bool>();
                var data = await adService.ApplyForJob(model, serviceHelper.GetCurrentUserEmail(), allowAnonymous);
                response.Object = data;
                return response;
            });
        }

        [HttpPost]
        [Route("TopAvailable")]
        public async Task<ServiceResponse<IEnumerable<AdDTO>>> TopAvailable()
        {
            return await HandleApiOperationAsync(async () => {
                var response = new ServiceResponse<IEnumerable<AdDTO>>();
                var data = await adService.TopAvailable(/*model, serviceHelper.GetCurrentUserEmail()*/);
                response.Object = data;
                return response;
            });
        }

        [HttpPost]
        [Route("SimilarAd/{subcategoryId}")]
        public async Task<ServiceResponse<IEnumerable<AdDTO>>> SimilarAd(long subcategoryId)
        {
            return await HandleApiOperationAsync(async () => {
                var response = new ServiceResponse<IEnumerable<AdDTO>>();
                var data = await adService.SimilarAd(subcategoryId/*model, serviceHelper.GetCurrentUserEmail(), allowAnonymous*/);
                response.Object = data;
                return response;
            });
        }

        //[HttpPost]
        //[Route("SendNotification")]
        //Task<string> SendNotification(string title, string body)
        //{
        //    return await HandleApiOperationAsync(async () => {
        //        var response = new ServiceResponse<IEnumerable<AdDTO>>();
        //        var data = await adService.SimilarAd(subcategoryId/*model, serviceHelper.GetCurrentUserEmail(), allowAnonymous*/);
        //        response.Object = data;
        //        return response;
        //    });
        //}
        //[HttpPost]
        //[Route("UpdateAdProperty")]
        //public async Task<IServiceResponse<bool>> UpdateAdProperty(AdProperty model)
        //{
        //    return await HandleApiOperationAsync(async () => {
        //        var response = new ServiceResponse<bool>();
        //        var data = await adService.UpdateAdProperty(model, serviceHelper.GetCurrentUserEmail());
        //        response.Object = data;
        //        return response;
        //    });
        //}
        #region APIs in progress
        //update profile 
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
        //postgraph
        #endregion
    }
}
//ad, categories