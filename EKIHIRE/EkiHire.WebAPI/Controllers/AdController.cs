using EkiHire.Business.Services;
using EkiHire.Core.Domain.Entities;
using EkiHire.Core.Domain.Entities.Enums;
using EkiHire.Core.Model;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

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
        private readonly IUserService userService;
        public AdController(IAdService adService, ICategoryService categoryService, IServiceHelper serviceHelper, IUserService userService)
        {
            this.adService = adService;
            this.categoryService = categoryService;
            this.serviceHelper = serviceHelper;
            this.userService = userService;
        }

        [HttpPost, Route("AddAd")]
        public async Task<IServiceResponse<long?>> AddAd(AddAdRequest model)
        {
            return await HandleApiOperationAsync(async () =>
            {
                var result = await adService.AddAd(model, serviceHelper.GetCurrentUserEmail());
                var response = new ServiceResponse<long?>(result);
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
        public async Task<IServiceResponse<bool>> EditAd(Ad dto, long adId)
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
        public async Task<IServiceResponse<bool>> CreateItem(Item model)
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
        [AllowAnonymous]
        [HttpPost, Route("Search"), Route("Search/{page}"), Route("Search/{page}/{size}")]
        public async Task<IServiceResponse<GetAdsResponse>> Search(AdFilter model, int page = 1, int size = 25)
        {
            var allowanonymous = string.IsNullOrWhiteSpace(serviceHelper.GetCurrentUserEmail()) || serviceHelper.GetCurrentUserEmail() == "Anonymous" ? true : false;
            return await HandleApiOperationAsync(async () =>
            {
                GetAdsResponse result = (await adService.GetAds(model, serviceHelper.GetCurrentUserEmail(), allowanonymous, page:page,size:size));
                var response = new ServiceResponse<GetAdsResponse>(result);
                return response;
            });
        }

        [HttpGet]
        [Route("GetCategories")]
        [Route("GetCategories/{withOtherData:bool}")]
        public async Task<IServiceResponse<IEnumerable<Category>>> GetCategories(bool withOtherData = false)
        {//IEnumerable<Category>
            return await HandleApiOperationAsync(async () => {
                var response = new ServiceResponse<IEnumerable<Category>>();//
                var data = await categoryService.GetCategories(null,withOtherData);
                response.Object = data;
                return response;
            });
        }

        [HttpGet]
        [Route("GetCategory/{id}")]
        [Route("GetCategory/{id}/{withOtherData:bool}")]
        public async Task<IServiceResponse<Category>> GetCategory(long id, bool withOtherData = false)
        {
            return await HandleApiOperationAsync(async () => {
                var response = new ServiceResponse<Category>();
                var data = await categoryService.GetCategory(id, withOtherData);
                response.Object = data;
                return response;
            });
        }

        [HttpGet]
        [Route("GetSubcategoriesByCategoryId/{categoryId}")]
        public async Task<IServiceResponse<IEnumerable<Subcategory>>> GetSubcategoriesByCategoryId(long categoryId)
        {//working
            return await HandleApiOperationAsync(async () => {
                var response = new ServiceResponse<IEnumerable<Subcategory>>();
                var data = await categoryService.GetSubcategoriesByCategoryId(categoryId);
                response.Object = data;
                return response;
            });
        }
        //review
        [HttpPost]
        [Route("ReviewsGivenByUser")]
        public async Task<IServiceResponse<IEnumerable<AdFeedback>>> ReviewsGivenByUser(string username = null/*long[] adIds = null*/)
        {//working - no data
            var urn = username ?? serviceHelper.GetCurrentUserEmail();
            return await HandleApiOperationAsync(async () => {
                var response = new ServiceResponse<IEnumerable<AdFeedback>>();
                var data = await adService.ReviewsGivenByUser(urn/*, adIds*/);
                response.Object = data;
                return response;
            });
        }
        [HttpPost]
        [Route("ReviewsForAd/{adId}")]
        public async Task<IServiceResponse<IEnumerable<AdFeedback>>> ReviewsForAd(long adId/*long[] adIds = null*/)
        {//working - no data
            return await HandleApiOperationAsync(async () => {
                var response = new ServiceResponse<IEnumerable<AdFeedback>>();
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
        public async Task<IServiceResponse<bool>> AddCategory(Category model)
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
        public async Task<IServiceResponse<bool>> AddSubcategory(Subcategory model)
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
        public async Task<IServiceResponse<bool>> AddAdProperty(AdProperty model)
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
        public async Task<IServiceResponse<bool>> AddOrUpdateAdPropertyValue(AdPropertyValue model)
        {
            return await HandleApiOperationAsync(async () => {
                var response = new ServiceResponse<bool>();
                var data = await adService.AddOrUpdateAdPropertyValue(model, serviceHelper.GetCurrentUserEmail());
                response.Object = data;
                return response;
            });
        }

        [AllowAnonymous]
        [HttpPost]
        [Route("Trending")]
        [Route("Trending/{count}")]
        public async Task<IServiceResponse<IEnumerable<Ad>>> Trending(long count = 10)
        {
            var allowanonymous = string.IsNullOrWhiteSpace(serviceHelper.GetCurrentUserEmail()) || serviceHelper.GetCurrentUserEmail() == "Anonymous" ? true : false;
            return await HandleApiOperationAsync(async () => {
                var response = new ServiceResponse<IEnumerable<Ad>>();
                var data = await adService.Trending(count, serviceHelper.GetCurrentUserEmail(), allowanonymous);
                response.Object = data;
                return response;
            });
        }

        [HttpPost]
        [Route("GetAd/{Id}")]
        [AllowAnonymous]
        public async Task<IServiceResponse<Ad>> GetAd(long Id)
        {
            var allowanonymous = string.IsNullOrWhiteSpace(serviceHelper.GetCurrentUserEmail()) || serviceHelper.GetCurrentUserEmail() == "Anonymous" ? true : false;
            return await HandleApiOperationAsync(async () => {
                var response = new ServiceResponse<Ad>();
                var data = await adService.GetAd(Id, serviceHelper.GetCurrentUserEmail(), allowanonymous);
                //var data = (await adService.GetAds(new AdFilter { AdId = Id }, serviceHelper.GetCurrentUserEmail())).FirstOrDefault();
                response.Object = data;
                return response;
            });
        }

        [HttpPost]
        [Route("GetAd/bulk")]
        [AllowAnonymous]
        public async Task<IServiceResponse<IEnumerable<Ad>>> GetAdBulk(long[] Ids)
        {
            var allowanonymous = string.IsNullOrWhiteSpace(serviceHelper.GetCurrentUserEmail()) || serviceHelper.GetCurrentUserEmail() == "Anonymous" ? true : false;
            return await HandleApiOperationAsync(async () => {
                var response = new ServiceResponse<IEnumerable<Ad>>();
                var data = await adService.GetAdBulk(Ids, serviceHelper.GetCurrentUserEmail(), allowanonymous);
                //var data = (await adService.GetAds(new AdFilter { AdId = Id }, serviceHelper.GetCurrentUserEmail())).FirstOrDefault();
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
        public async Task<ServiceResponse<IEnumerable<CartItem>>> GetUserCart()
        {
            return await HandleApiOperationAsync(async () => {
                var response = new ServiceResponse<IEnumerable<CartItem>>();
                var data = await adService.GetCartItems(serviceHelper.GetCurrentUserEmail());
                response.Object = data;
                return response;
            });
        }
        [HttpPost]
        [Route("SaveRequestQuote")]
        public async Task<ServiceResponse<bool>> SaveRequestQuote(RequestQuote model)
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
        public async Task<ServiceResponse<bool>> SaveReview(AdFeedback model/*, string username*/)
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
        
        public async Task<ServiceResponse<bool>> ApplyForJob(JobApplication model, bool allowAnonymous = false)
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
        [Route("TopAvailable/{count}")]
        [AllowAnonymous]
        public async Task<ServiceResponse<IEnumerable<Ad>>> TopAvailable(int count = 8)
        {
            var allowanonymous = string.IsNullOrWhiteSpace(serviceHelper.GetCurrentUserEmail()) || serviceHelper.GetCurrentUserEmail() == "Anonymous" ? true : false;
            return await HandleApiOperationAsync(async () => {
                var response = new ServiceResponse<IEnumerable<Ad>>();
                var data = await adService.TopAvailable(count,allowanonymous, serviceHelper.GetCurrentUserEmail()/*model, serviceHelper.GetCurrentUserEmail()*/);
                response.Object = data;
                return response;
            });
        }

        [HttpPost]
        [Route("SimilarAd/{subcategoryId}")]
        [Route("SimilarAd/{subcategoryId}/{count}")]
        [AllowAnonymous]
        public async Task<ServiceResponse<IEnumerable<Ad>>> SimilarAd(long subcategoryId, int count = 8)
        {
            var allowanonymous = string.IsNullOrWhiteSpace(serviceHelper.GetCurrentUserEmail()) || serviceHelper.GetCurrentUserEmail() == "Anonymous" ? true : false;
            return await HandleApiOperationAsync(async () => {
                var response = new ServiceResponse<IEnumerable<Ad>>();
                var data = await adService.SimilarAd(subcategoryId,count, allowanonymous, serviceHelper.GetCurrentUserEmail()/*model, serviceHelper.GetCurrentUserEmail(), allowAnonymous*/);
                response.Object = data;
                return response;
            });
        }

        [HttpPost]
        [Route("AddAdImage/{AdId}")]
        [Route("UploadFile/Bulk")]
        //[AllowAnonymous]
        public async Task<ServiceResponse<List<string>>> AddAdImage(long AdId = 0)
        {
            IFormFileCollection images = Request.Form.Files;
            return await HandleApiOperationAsync(async () => {
                var response = new ServiceResponse<List<string>>();
                var data = await adService.AddAdImage(AdId, images, serviceHelper.GetCurrentUserEmail()/*model, serviceHelper.GetCurrentUserEmail(), allowAnonymous*/);
                response.Object = data;
                return response;
            });
        }

        [HttpPost]
        [Route("UploadFile")]
        public async Task<ServiceResponse<string>> UploadFile()
        {
            IFormFile file = Request.Form.Files[0];
            return await HandleApiOperationAsync(async () => {
                var response = new ServiceResponse<string>();
                var data = await adService.UploadFile(file, serviceHelper.GetCurrentUserEmail());
                response.Object = data;
                return response;
            });
        }

        [HttpPost]
        [Route("GetTransactions")]
        [Route("GetTransactions/{page}")]
        [Route("GetTransactions/{page}/{size}")]
        public async Task<ServiceResponse<GetTransactionResponse>> GetTransactions(int page = 1, int size = 25)
        {
            return await HandleApiOperationAsync(async () => {
                var response = new ServiceResponse<GetTransactionResponse>();
                var data = await adService.GetTransactions(serviceHelper.GetCurrentUserEmail(), page, size);
                response.Object = data;
                return response;
            });
        }

        [HttpPost]
        [Route("GetTransaction/{Id}")]
        public async Task<ServiceResponse<Transaction>> GetTransactionById(long Id)
        {
            return await HandleApiOperationAsync(async () => {
                var response = new ServiceResponse<Transaction>();
                var data = await adService.GetTransactionById(Id,serviceHelper.GetCurrentUserEmail());
                response.Object = data;
                return response;
            });
        }

        [HttpPost]
        [Route("GetUsers")]
        [Route("GetUsers/{page}")]
        [Route("GetUsers/{page}/{size}")]
        public async Task<ServiceResponse<IEnumerable<User>>> GetUsers( int page = 1, int size = 25)
        {
            return await HandleApiOperationAsync(async () => {
                var response = new ServiceResponse<IEnumerable<User>>();
                var data = await adService.GetUsers(serviceHelper.GetCurrentUserEmail(), page, size);
                response.Object = data;
                return response;
            });
        }

        [HttpPost]
        [Route("GetUser/{Id}")]
        public async Task<IServiceResponse<User>> GetUserById(long Id)
        {
            return await HandleApiOperationAsync(async () => {
                var response = new ServiceResponse<User>();
                var data = await adService.GetUserById(Id, serviceHelper.GetCurrentUserEmail());
                response.Object = data;
                return response;
            });
        }

        [HttpPost]
        [Route("DeletetUser/{Id}")]
        public async Task<ServiceResponse<bool>> DeletetUserById(long Id)
        {
            return await HandleApiOperationAsync(async () => {
                var response = new ServiceResponse<bool>();
                var data = await adService.DeletetUserById(Id, serviceHelper.GetCurrentUserEmail());
                response.Object = data;
                return response;
            });
        }

        [HttpPost]
        [Route("GetSubscriptionPackages")]
        [Route("GetSubscriptionPackages/{page}")]
        [Route("GetSubscriptionPackages/{page}/{size}")]
        public async Task<ServiceResponse<IEnumerable<SubscriptionPackage>>> GetSubscriptionPackages(int page = 1, int size = 25)
        {
            return await HandleApiOperationAsync(async () => {
                var response = new ServiceResponse<IEnumerable<SubscriptionPackage>>();
                var data = await adService.GetSubscriptionPackages(serviceHelper.GetCurrentUserEmail(), page, size);
                response.Object = data;
                return response;
            });
        }

        [HttpPost]
        [Route("GetSubscriptionPackage/{Id}")]
        public async Task<ServiceResponse<SubscriptionPackage>> GetSubscriptionPackageById(long Id)
        {
            return await HandleApiOperationAsync(async () => {
                var response = new ServiceResponse<SubscriptionPackage>();
                var data = await adService.GetSubscriptionPackageById(Id, serviceHelper.GetCurrentUserEmail());
                response.Object = data;
                return response;
            });
        }

        [HttpPost]
        [Route("UpdateSubscriptionPackage")]
        public async Task<ServiceResponse<bool>> UpdateSubscriptionPackage(SubscriptionPackage model)
        {
            return await HandleApiOperationAsync(async () => {
                var response = new ServiceResponse<bool>();
                var data = await adService.UpdateSubscriptionPackage(model, serviceHelper.GetCurrentUserEmail());
                response.Object = data;
                return response;
            });
        }

        [HttpPost]
        [Route("DeletetSubscriptionPackage/{Id}")]
        public async Task<ServiceResponse<bool>> DeletetSubscriptionPackageById(long Id)
        {
            return await HandleApiOperationAsync(async () => {
                var response = new ServiceResponse<bool>();
                var data = await adService.DeletetSubscriptionPackageById(Id, serviceHelper.GetCurrentUserEmail());
                response.Object = data;
                return response;
            });
        }

        [HttpPost]
        [Route("AddTransaction")]
        public async Task<ServiceResponse<bool>> AddTransaction(Transaction model)
        {
            return await HandleApiOperationAsync(async () => {
                var response = new ServiceResponse<bool>();
                var data = await adService.AddTransaction(model, serviceHelper.GetCurrentUserEmail());
                response.Object = data;
                return response;
            });
        }

        [HttpPost]
        [Route("UpdateAdProperty")]
        public async Task<ServiceResponse<bool>> UpdateAdProperty(AdProperty model)
        {
            return await HandleApiOperationAsync(async () => {
                var response = new ServiceResponse<bool>();
                var data = await adService.UpdateAdProperty(model, serviceHelper.GetCurrentUserEmail());
                response.Object = data;
                return response;
            });
        }

        [HttpPost]
        [Route("AddAdImage")]
        public async Task<ServiceResponse<bool>> AddAdImage(AdImage model)
        {
            return await HandleApiOperationAsync(async () => {
                var response = new ServiceResponse<bool>();
                var data = await adService.AddAdImage(model, serviceHelper.GetCurrentUserEmail());
                response.Object = data;
                return response;
            });
        }

        

        [HttpPost]
        [Route("AddSubscriptionPackage")]
        public async Task<ServiceResponse<bool>> AddSubscriptionPackage(SubscriptionPackage model)
        {
            return await HandleApiOperationAsync(async () => {
                var response = new ServiceResponse<bool>();
                var data = await adService.AddSubscriptionPackage(model, serviceHelper.GetCurrentUserEmail());
                response.Object = data;
                return response;
            });
        }

        [HttpPost]
        [Route("AddNewsletterSubscriber")]
        public async Task<ServiceResponse<bool>> AddNewsletterSubscriber(NewsletterSubscriber model)
        {
            return await HandleApiOperationAsync(async () => {
                var response = new ServiceResponse<bool>();
                var data = await adService.AddNewsletterSubscriber(model, serviceHelper.GetCurrentUserEmail());
                response.Object = data;
                return response;
            });
        }

        [HttpPost]
        [Route("GetNewsletterSubscriber")]
        [Route("GetNewsletterSubscriber/{page}")]
        [Route("GetNewsletterSubscriber/{page}/{size}")]
        public async Task<ServiceResponse<IEnumerable<NewsletterSubscriber>>> GetNewsletterSubscriber(int page = 1, int size = 25)
        {
            return await HandleApiOperationAsync(async () => {
                var response = new ServiceResponse<IEnumerable<NewsletterSubscriber>>();
                var data = await adService.GetNewsletterSubscriber(serviceHelper.GetCurrentUserEmail(), page, size);
                response.Object = data;
                return response;
            });
        }

        [HttpPost]
        [Route("GetNewsletterSubscriber/{Id}")]
        public async Task<ServiceResponse<NewsletterSubscriber>> GetNewsletterSubscriberById(long Id)
        {
            return await HandleApiOperationAsync(async () => {
                var response = new ServiceResponse<NewsletterSubscriber>();
                var data = await adService.GetNewsletterSubscriberById(Id, serviceHelper.GetCurrentUserEmail());
                response.Object = data;
                return response;
            });
        }

        [HttpPost]
        [Route("UpdateNewsletterSubscriber")]
        public async Task<ServiceResponse<bool>> UpdateNewsletterSubscriber(NewsletterSubscriber model)
        {
            return await HandleApiOperationAsync(async () => {
                var response = new ServiceResponse<bool>();
                var data = await adService.UpdateNewsletterSubscriber(model, serviceHelper.GetCurrentUserEmail());
                response.Object = data;
                return response;
            });
        }

        [HttpPost]
        [Route("DeleteNewsletterSubscriber/{id}")]
        public async Task<ServiceResponse<bool>> DeleteNewsletterSubscriber(long id)
        {
            return await HandleApiOperationAsync(async () => {
                var response = new ServiceResponse<bool>();
                var data = await adService.DeleteNewsletterSubscriber(id, serviceHelper.GetCurrentUserEmail());
                response.Object = data;
                return response;
            });
        }

        [HttpPost]
        [Route("ChangeUserType/{userType}/{clientId}")]
        public async Task<ServiceResponse<bool>> ChangeUserType(UserType userType, long clientId)
        {
            return await HandleApiOperationAsync(async () => {
                var response = new ServiceResponse<bool>();
                var data = await adService.ChangeUserType(userType, clientId, serviceHelper.GetCurrentUserEmail());
                response.Object = data;
                return response;
            });
        }

        [AllowAnonymous]
        [HttpGet]
        [Route("[action]")]
        public async Task<IServiceResponse<IEnumerable<State>>> GetStates()
        {
            return await HandleApiOperationAsync(async () => {
                var response = new ServiceResponse<IEnumerable<State>>();
                var data = await adService.GetStates();
                response.Object = data;
                return response;
            });
        }

        [AllowAnonymous]
        [HttpGet]
        [Route("[action]")]
        public async Task<IServiceResponse<IEnumerable<LGAData>>> GetLGAs()
        {
            return await HandleApiOperationAsync(async () => {
                var response = new ServiceResponse<IEnumerable<LGAData>>();
                var data = await adService.GetLGAs();
                response.Object = data;
                return response;
            });
        }

        [HttpGet]
        [Route("[action]")]
        public async Task<IServiceResponse<IDictionary<long, IEnumerable<GetMessagesResponse>>>> GetMessages()
        {
            return await HandleApiOperationAsync(async () => {
                var response = new ServiceResponse<IDictionary<long, IEnumerable<GetMessagesResponse>>>();
                var data = await adService.GetMessages(serviceHelper.GetCurrentUserEmail());
                response.Object = data;
                return response;
            });
        }
        [HttpGet]
        [Route("[action]")]
        public async Task<IServiceResponse<IEnumerable<GetNotificationResponse>>> GetNotifications()
        {
            return await HandleApiOperationAsync(async () => {
                var response = new ServiceResponse<IEnumerable<GetNotificationResponse>>();
                var data = await adService.GetNotifications(serviceHelper.GetCurrentUserEmail());
                response.Object = data;
                return response;
            });
        }
        #region comments
        //[AllowAnonymous]
        //[HttpPost]
        //[Route("[action]")]
        //public async Task<ServiceResponse<bool>> AddLGA(List<LocalGovernmentArea> model)
        //{
        //    return await HandleApiOperationAsync(async () => {
        //        var response = new ServiceResponse<bool>();
        //        var data = await adService.AddLocalGovernmentAreaOnce(model);
        //        response.Object = data;
        //        return response;
        //    });
        //}
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