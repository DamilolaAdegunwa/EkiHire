using System;
using System.Collections.Generic;
using System.Text;
using EkiHire.Core.Configuration;
using EkiHire.Core.Domain.DataTransferObjects;
using EkiHire.Core.Domain.Entities;
using EkiHire.Core.Exceptions;
using EkiHire.Core.Messaging.Email;
using EkiHire.Core.Messaging.Sms;
using EkiHire.Core.Model;
using EkiHire.Core.Utils;
using EkiHire.Data.Repository;
using EkiHire.Data.UnitOfWork;
using IPagedList;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using System.Collections.Specialized;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using System.Text.RegularExpressions;
using EkiHire.Core.Domain.Entities.Enums;
using EkiHire.Core.Collections.Extensions;
using EkiHire.Core.Domain.Extensions;
using log4net;
using System.Reflection;
using Microsoft.Extensions.Configuration;
using PagedList;
//using FirebaseAdmin;
//using FirebaseAdmin.Messaging;
using System.Collections;
using MoreLinq;
using Microsoft.AspNetCore.Http;
using System.Threading;
using System.Collections.Concurrent;
using EkiHire.Data.efCore.Context;
using Newtonsoft.Json;
using EkiHire.Business.Payload;
namespace EkiHire.Business.Services
{
    public interface IAdService
    {
        Task<long?> AddAd(AdDTO model, /*IFormFileCollection images,*/ string username);
        Task<bool> CloseAd(long model, string username);
        Task<bool> EditAd(AdDTO adDto, long model, string username);
        Task<bool> PromoteAd(long model, string username);
        Task<bool> CreateItem(ItemDTO model, string username);
        Task<bool> EditItemKeywords(List<string> keywords, long ItemId, string username);
        Task<bool> GroupAdItems(long[] ItemIds, string groupname, string username);
        Task<bool> AddAdToCart(long Id, string username);
        Task<bool> RemoveAdFromCart(long Id, string username);
        //Task<IEnumerable<AdDTO>> GetAd(AdFilter model, string username, bool allowanonymous = false);
        //Task<IEnumerable<AdFeedback>> AdFeedbackByUser(string username, long adId = 0);
        Task<IEnumerable<AdFeedbackDTO>> ReviewsGivenByUser(string username/*, long[] adIds = null*/);
        //Task<IEnumerable<AdFeedback>> AdFeedbackForUser(string username, long adId = 0);
        Task<IEnumerable<AdFeedbackDTO>> ReviewsForAd(long AdId, string username/*, long[] adIds = null*/);
        Task<IEnumerable<Follow>> GetFollowers(string username);
        Task<IEnumerable<Follow>> GetFollowing(string username);
        Task<bool> AddKeywords(List<string> keywords, long subid, string username);
        Task<bool> EditKeywords(long kwId, string correctedWord, string username);
        Task<bool> DeleteKeywords(long kwId, string username);
        Task<IEnumerable<Keyword>> GetKeywords(string username, long[] kwIds = null, long? subid = null);
        Task<IEnumerable<AdProperty>> GetAdPropertiesBySubcategory(long subId, string username);
        Task<IEnumerable<AdPropertyValue>> GetAdPropertiesWithValue(long adid, string username);
        Task<bool> AddAdProperty(AdPropertyDTO model, string username);
        Task<bool> AddOrUpdateAdPropertyValue(AdPropertyValue model, string username);
        Task<bool> UpdateAdProperty(AdProperty model, string username);
        Task<IEnumerable<AdDTO>> Trending(long count = 10, string username = null, bool allowanonymous = false);
        //Task<AdDTO> GetAd(long Id);
        Task<bool> UpdateAdStatus(long AdId, AdsStatus adsStatus);
        Task<bool> AddAdImage(AdImageDTO model, string username);
        Task<IEnumerable<CartItemDTO>> GetCartItems(string username);
        Task<bool> SaveRequestQuote(RequestQuote model, string username);
        Task<bool> SaveReview(AdFeedback model, string username);
        Task<bool> ApplyForJob(JobApplicationDTO model, string username, bool allowAnonymous = false);
        Task<IEnumerable<AdDTO>> TopAvailable(int count = 8, bool allowAnonymous = false);
        Task<IEnumerable<AdDTO>> SimilarAd(long subcategoryId, int count = 8, bool allowAnonymous = false);
        //Task<string> SendNotification(List<string> clientToken, string title, string body);
        Task<AdResponse>/*Task<IEnumerable<Ad>>*/ GetAds(AdFilter model, string username, bool allowanonymous = false, int page = 1, int size = 25);
        Task<List<string>> AddAdImage(long AdId, IFormFileCollection images, string username);
        Task<string> UploadFile(IFormFile file, string username);
        Task<IEnumerable<Transaction>> GetTransactions(string username, int page = 1, int size = 25);
        Task<Transaction> GetTransactionById(long Id, string username);
        Task<IEnumerable<User>> GetUsers(string username, int page = 1, int size = 25);
        Task<User> GetUserById(long Id, string username);
        Task<bool> DeletetUserById(long Id, string username);
        Task<IEnumerable<SubscriptionPackage>> GetSubscriptionPackages(string username, int page = 1, int size = 25);
        Task<SubscriptionPackage> GetSubscriptionPackageById(long Id, string username);
        Task<bool> UpdateSubscriptionPackage(SubscriptionPackage model, string username);
        Task<bool> DeletetSubscriptionPackageById(long Id, string username);
        Task<bool> AddTransaction(Transaction model, string username);
        
        Task<bool> AddSubscriptionPackage(SubscriptionPackage model, string username);
        Task<bool> AddNewsletterSubscriber(NewsletterSubscriber model, string username);
        Task<IEnumerable<NewsletterSubscriber>> GetNewsletterSubscriber(string username, int page = 1, int size = 25);
        Task<NewsletterSubscriber> GetNewsletterSubscriberById(long Id, string username);
        Task<Ad> GetAd(long Id, string username, bool allowAnonymous = false);
        Task<IEnumerable<Ad>> GetAdBulk(long[] Ids, string username, bool allowanonymous = false);
        Task<bool> UpdateNewsletterSubscriber(NewsletterSubscriber model, string username);
        Task<bool> DeleteNewsletterSubscriber(long id, string username);
        Task<bool> ChangeUserType(UserType userType, long clientId, string username);
        //Task<bool> AddLocalGovernmentAreaOnce(List<LocalGovernmentArea> model);
        //Task<IEnumerable<Ad>> GetAdsTest(AdFilter model, string username, bool allowanonymous = false, int page = 1, int size = 25);
        Task<IEnumerable<State>> GetStates();
        Task<IEnumerable<LGAData>> GetLGAs();
        //Task<IEnumerable<Message>> GetMessages(long otherPersonId, string username);
        Task<IEnumerable<GetMessagesResponse>> GetMessages(string username);
        Task<IEnumerable<GetNotificationResponse>> GetNotifications(string username);
    }
    public class AdService: IAdService
    {
        #region AdService properties
        private readonly IRepository<Ad> adRepository;
        private readonly IRepository<AdFeedback> adFeedbackRepository;
        private readonly IServiceHelper _serviceHelper;
        private readonly IUserService _userSvc;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IRepository<Item> itemRepository;
        private readonly IRepository<CartItem> CartItemRepository;
        private static readonly ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType.Name);
        private readonly IRepository<Follow> followRepository;
        private readonly IRepository<Subcategory> _subcategoryRepo;
        private readonly IRepository<Keyword> _keywordRepo;
        private readonly IRepository<AdProperty> _adPropertyRepo;
        private readonly IRepository<AdPropertyValue> _adPropertyValueRepo;
        private readonly IRepository<User> _userRepo;
        private readonly IRepository<AdImage> _AdImageRepo;
        private readonly AppConfig appConfig;
        private readonly IRepository<AdLookupLog> AdLookupLogRepository;
        private readonly IRepository<Category> CategoryRepository;
        private readonly IRepository<User> UserRepository;
        private readonly IRepository<RequestQuote> RequestQuoteRepository;
        private readonly IRepository<JobApplication> JobApplicationRepository;
        private readonly SmtpConfig _smtpsettings;
        private readonly IHostingEnvironment _hostingEnvironment;
        //private readonly IMailService _mailSvc;
        private readonly IRepository<Transaction> TransactionRepository;
        private readonly IRepository<SubscriptionPackage> SubscriptionPackageRepository;
        private readonly IRepository<NewsletterSubscriber> NewsletterSubscriberRepository;
        private readonly IRepository<PreviousWorkExperience> PreviousWorkExperienceRepository;
        private readonly IRepository<LocalGovernmentArea> LocalGovernmentAreaRepository;
        private readonly ApplicationDbContext _applicationDbContext;
        private readonly IChatHub _chatHub;
        #endregion
        public AdService(IRepository<Ad> adRepository, IServiceHelper _serviceHelper, IUserService _userSvc, IUnitOfWork unitOfWork, IRepository<Item> itemRepository, IRepository<CartItem> CartItemRepository, IRepository<AdFeedback> adFeedbackRepository, IRepository<Follow> followRepository, IRepository<Subcategory> _subcategoryRepo, IRepository<Keyword> _keywordRepo, IRepository<AdProperty> _adPropertyRepo, IRepository<AdPropertyValue> _adPropertyValueRepo, IRepository<User> _userRepo,
            IRepository<AdImage> _AdImageRepo, IOptions<AppConfig> _appConfig, IRepository<AdLookupLog> AdLookupLogRepository,
            IRepository<Category> CategoryRepository, IRepository<User> UserRepository, IRepository<RequestQuote> RequestQuoteRepository, IRepository<JobApplication> JobApplicationRepository, IOptions<SmtpConfig> settingSvc, IHostingEnvironment _hostingEnvironment, /*IMailService mailSvc,*/ IRepository<Transaction> TransactionRepository, IRepository<SubscriptionPackage> SubscriptionPackageRepository, IRepository<NewsletterSubscriber> NewsletterSubscriberRepository, IRepository<PreviousWorkExperience> PreviousWorkExperienceRepository, IRepository<LocalGovernmentArea> LocalGovernmentAreaRepository, ApplicationDbContext applicationDbContext, IChatHub chatHub)
        {
            this.adRepository = adRepository;
            this._serviceHelper = _serviceHelper;
            this._userSvc = _userSvc;
            this._unitOfWork = unitOfWork;
            this.itemRepository = itemRepository;
            this.CartItemRepository = CartItemRepository;
            this.adFeedbackRepository = adFeedbackRepository;
            this.followRepository = followRepository;
            this._subcategoryRepo = _subcategoryRepo;
            this._keywordRepo = _keywordRepo;
            this._adPropertyRepo = _adPropertyRepo;
            this._adPropertyValueRepo = _adPropertyValueRepo;
            this._userRepo = _userRepo;
            this._AdImageRepo = _AdImageRepo;
            this.appConfig = _appConfig.Value;
            this.AdLookupLogRepository = AdLookupLogRepository;
            this.CategoryRepository = CategoryRepository;
            this.UserRepository = UserRepository;
            this.RequestQuoteRepository = RequestQuoteRepository;
            this.JobApplicationRepository = JobApplicationRepository;
            this._smtpsettings = settingSvc.Value;
            this._hostingEnvironment = _hostingEnvironment;
            //this._mailSvc = mailSvc;
            this.TransactionRepository = TransactionRepository;
            this.SubscriptionPackageRepository = SubscriptionPackageRepository;
            this.NewsletterSubscriberRepository = NewsletterSubscriberRepository;
            this.PreviousWorkExperienceRepository = PreviousWorkExperienceRepository;
            this.LocalGovernmentAreaRepository = LocalGovernmentAreaRepository;
            _applicationDbContext = applicationDbContext;
            _chatHub = chatHub;
        }
        public async Task<long?> AddAd(AdDTO model, string username)
        {
            int retry = 0; int retries = 2;
            retry:
            try
            {
                #region validate credential

                //check that the model carries data
                if (model == null)
                {
                    retry = retries;//don't retry
                    throw await _serviceHelper.GetExceptionAsync("no input provided!");
                }
                //check for non-empty username 
                if (string.IsNullOrWhiteSpace(username))
                {
                    retry = retries;//don't retry
                    throw await _serviceHelper.GetExceptionAsync("Please login and retry");
                }

                //check that the user exist
                var user = await _userSvc.FindFirstAsync(x => x.UserName == username);
                if (user == null)
                {
                    retry = retries;//don't retry
                    throw await _serviceHelper.GetExceptionAsync("User does not exist");
                }

                //check that the username is a valid email ( the password would be validate by the Identity builder)
                if (!Regex.IsMatch(username, @"^[^@\s]+@[^@\s]+\.[^@\s]+$", RegexOptions.IgnoreCase))
                {
                    retry = retries;//don't retry
                    throw await _serviceHelper.GetExceptionAsync("The username isn't a valid email");
                }

                //check for valid subcategory
                if(model.SubcategoryId == null || model.SubcategoryId < 1)
                {
                    retry = retries;//don't retry
                    throw new Exception("Invalid SubcategoryId");
                }
                var subcategory = _subcategoryRepo.Get(model.SubcategoryId??0);
                if(subcategory == null)
                {
                    retry = retries;//don't retry
                    throw new Exception("Cannot find Subcategory");
                }

                //validate usertype, validate the adtype if premium whether user can put premium ad
                #endregion

                #region prepare data
                Ad ad = new Ad();
                ad = model;
                
                ad.AdsStatus = AdsStatus.INREVIEW;
                ad.UserId = user.Id;
                ad.AdReference = $"EH{new Random().Next(1_000_000_000, int.MaxValue)}{new Random().Next(1_000_000_000, int.MaxValue)}";
                ad.IsActive = true;
                //basic properties
                ad.CreationTime = DateTime.Now;
                ad.CreatorUserId = user.Id;
                ad.IsDeleted = false;
                ad.LastModificationTime = DateTime.Now;
                ad.LastModifierUserId = user.Id;
                ad.DeleterUserId = null;
                ad.DeletionTime = null;
                ad.Id = 0;
                #endregion

                #region first send images to cloudinary (old impl, just get the imagePath)
                var images = model.AdImages?.ToList();

                //single thread
                //List<string> imgPaths = new List<string>();
                //if (images != null && images.Count > 0)
                //{
                //    foreach (var i in images)
                //    {
                //        var byteArray = Convert.FromBase64String(i.ImageString);
                //        var stream = new MemoryStream(byteArray);
                //        var imgPath = _serviceHelper.UploadPhoto(stream);
                //        imgPaths.Add(imgPath);
                //    }
                //}
                //multi-thread
                //ConcurrentBag<string> imgPaths = new ConcurrentBag<string>();
                //if (images != null && images.Count > 0)
                //{
                //    Parallel.ForEach(images, (i) => {
                //        var byteArray = Convert.FromBase64String(i.ImageString);
                //        var stream = new MemoryStream(byteArray);
                //        var imgPath = _serviceHelper.UploadPhoto(stream);
                //        imgPaths.Add(imgPath);
                //    });
                //}
                #endregion

                #region first clear adimages before saving
                ad.AdImages = null;
                ad.Subcategory = null;
                ad.User = null;
                ad.AdFeedback = null;
                ad.AdPropertyValue = null;
                _unitOfWork.BeginTransaction();
                var insertedData = await adRepository.InsertAsync(ad);
                _unitOfWork.Commit();
                #endregion

                #region save the property values 
                var adpv = model.AdPropertyValue?.ToList();
                if (adpv != null && adpv.Count > 0)
                {
                    //single thread
                    //_unitOfWork.BeginTransaction();
                    //foreach (var p in adpv)
                    //{
                    //    p.Ad = insertedData; p.AdId = insertedData?.Id;
                    //    await AddOrUpdateAdPropertyValue(p, username);
                    //}
                    //_unitOfWork.Commit();

                    //concurrent
                    _unitOfWork.BeginTransaction();
                    //Parallel.ForEach(adpv, (p) => {
                    foreach(var p in adpv)
                    {
                        _ = _adPropertyValueRepo.InsertAsync(
                            new AdPropertyValue
                            {
                                Ad = null,
                                AdId = insertedData?.Id,
                                AdProperty = null,
                                AdPropertyId = p.AdPropertyId,
                                Value = p.Value,
                                //basic properties
                                CreationTime = DateTime.Now,
                                CreatorUserId = user.Id,
                                IsDeleted = false,
                                LastModificationTime = DateTime.Now,
                                LastModifierUserId = user.Id,
                                DeleterUserId = null,
                                DeletionTime = null,
                                Id = 0,
                            }
                        );
                    }  
                    //});
                    _unitOfWork.Commit();
                }
                #endregion

                #region save the image
                if (images != null && images.Count > 0)
                {
                    //_unitOfWork.BeginTransaction();
                    //_ = Parallel.ForEach(imgPaths, (p) =>
                    //  {
                    //      _ = _AdImageRepo.InsertAsync(
                    //          new AdImage
                    //          {
                    //              AdId = insertedData?.Id,
                    //              ImagePath = p,
                    //              //basic properties
                    //              CreationTime = DateTime.Now,
                    //              CreatorUserId = user.Id,
                    //              IsDeleted = false,
                    //              LastModificationTime = DateTime.Now,
                    //              LastModifierUserId = user.Id,
                    //              DeleterUserId = null,
                    //              DeletionTime = null,
                    //              Id = 0,
                    //          }
                    //      );
                    //  });
                    //_unitOfWork.Commit();

                    _unitOfWork.BeginTransaction();
                    foreach (var p in images)
                    {
                        _ = _AdImageRepo.InsertAsync(
                            new AdImage
                            {
                                AdId = insertedData?.Id,
                                ImagePath = p.ImagePath,
                                //basic properties
                                CreationTime = DateTime.Now,
                                CreatorUserId = user.Id,
                                IsDeleted = false,
                                LastModificationTime = DateTime.Now,
                                LastModifierUserId = user.Id,
                                DeleterUserId = null,
                                DeletionTime = null,
                                Id = 0,
                            }
                        );
                    }
                    _unitOfWork.Commit();
                }
                #endregion

                #region comment
                //if(images.Count > 0)
                //{
                //    List<AdImage> imgList = null;
                //    foreach(var image in images)
                //    {

                //        //byte[] byteArray = System.IO.File.ReadAllBytes(@"C:\images\cow.png");

                //        using(System.IO.MemoryStream stream = new System.IO.MemoryStream())
                //        {
                //            image.CopyTo(stream);
                //            var imgPath = _serviceHelper.UploadPhoto(stream);
                //            await AddAdImage(new AdImage { AdId = savedAd?.Id, ImagePath = imgPath }, username);
                //        }
                //    }

                //}
                #endregion

                return insertedData?.Id;
            }
            catch (Exception ex)
            {
                _unitOfWork.Rollback();
                log.Error($"user ({username}) could not add a new ad {ex.Message} :: {MethodBase.GetCurrentMethod().Name} :: {ex.StackTrace} :: payload {JsonConvert.SerializeObject(model)}");
                if(retry < retries)
                {
                    retry++;
                    goto retry;
                }
                throw ex;
            }
        }
        public async Task<AdResponse> /*Task<IEnumerable<Ad>>*/ GetAds(AdFilter model, string username, bool allowanonymous = false, int page = 1, int size = 25)
        {
            try
            {
                #region validation
                if (string.IsNullOrWhiteSpace(username) && allowanonymous == false)
                {
                    throw await _serviceHelper.GetExceptionAsync("Please input a username!");
                }
                var user = await _userSvc.FindFirstAsync(x => x.UserName == username && x.IsDeleted == false);
                if (user == null && allowanonymous == false)
                {
                    throw await _serviceHelper.GetExceptionAsync("Unauthorized access! Please login");
                }
                if (model == null)
                {
                    throw await _serviceHelper.GetExceptionAsync("Invalid search entry!");
                }
                var userId = user?.Id;
                #endregion

                var query = (from ad in adRepository.GetAll().DefaultIfEmpty()
                             join s in _subcategoryRepo.GetAll().DefaultIfEmpty() on ad.SubcategoryId equals s.Id into subcategory

                             from sub in subcategory.DefaultIfEmpty()
                             join c in CategoryRepository.GetAll().DefaultIfEmpty() on sub.CategoryId equals c.Id into category

                             from cat in category.DefaultIfEmpty()
                             join adp in _adPropertyRepo.GetAll().DefaultIfEmpty() on sub.Id equals adp.SubcategoryId into adproperty

                             from adprop in adproperty.DefaultIfEmpty()
                             join adpv in _adPropertyValueRepo.GetAll().DefaultIfEmpty() on ad.Id equals adpv.AdId into adpropertyvalue

                             from adpropval in adpropertyvalue.DefaultIfEmpty()
                             where !ad.IsDeleted
                             && (ad.Id == model.AdId || model.AdId == null || model.AdId < 1)
                             && (ad.Name.Contains(model.SearchText) || string.IsNullOrWhiteSpace(model.SearchText))
                             && (ad.SubcategoryId == model.SubcategoryId || model.SubcategoryId == null || model.SubcategoryId < 1)
                             && (cat.Id == model.CategoryId || model.CategoryId == null || model.CategoryId < 1)
                             && (ad.Amount >= model.min_amount || model.min_amount == null || model.min_amount < 0)
                             && (ad.Amount <= model.max_amount || model.max_amount == null || model.max_amount < 0)
                             && (ad.Name.Contains(model.Address) || string.IsNullOrWhiteSpace(model.Address))
                             && (ad.AdClass == model.AdClass || model.AdClass == null)
                             && (ad.PhoneNumber.Contains(model.PhoneNumber) || string.IsNullOrWhiteSpace(model.PhoneNumber))
                             && (ad.AdReference.Contains(model.AdReference) || string.IsNullOrWhiteSpace(model.AdReference))
                             && (ad.Description.Contains(model.Description) || string.IsNullOrWhiteSpace(model.Description))
                             && (ad.AdsStatus == model.AdsStatus || model.AdsStatus == null)
                             && (ad.UserId == model.UserId || model.UserId == null || model.UserId < 1)
                             && (model.PropertyValuePairs.Any(pvp => pvp.PropertyId == adprop.Id && adpropval.Value.Contains(pvp.Value)) || model.PropertyValuePairs == null || model.PropertyValuePairs == new List<PropertyValuePair>())

                             let rtnData = adFeedbackRepository.GetAll().DefaultIfEmpty().Where(a => ad.Id == a.Id && !a.IsDeleted && ((int)a.Rating) >= 1 && ((int)a.Rating) <= 5).DefaultIfEmpty().ToList()
                             let rtn = rtnData.DefaultIfEmpty().Average(sf => (int)(sf.Rating ?? 0))

                             select new Ad
                             {
                                 AdClass = ad.AdClass,
                                 Address = ad.Address,
                                 AdFeedback = adFeedbackRepository.GetAll().DefaultIfEmpty().Where(a => a.AdId == ad.Id && a.IsDeleted == false).DefaultIfEmpty().ToList(),
                                 AdImages = _AdImageRepo.GetAll().Where(a => a.AdId == ad.Id && a.IsDeleted == false).ToList(),
                                 //AdPropertyValue = _adPropertyValueRepo.GetAll().DefaultIfEmpty().Where(a => a.AdId == ad.Id && a.IsDeleted == false).DefaultIfEmpty().ToList(),
                                 //AdPropertyValue = _adPropertyValueRepo.GetAll().Include("AdProperty").DefaultIfEmpty().Where(a => a.AdId == ad.Id && a.IsDeleted == false).DefaultIfEmpty().ToList(),
                                 AdPropertyValue = _adPropertyValueRepo.GetAll().Include(u => u.AdProperty).DefaultIfEmpty().Where(a => a.AdId == ad.Id && a.IsDeleted == false).DefaultIfEmpty().Select<AdPropertyValue, AdPropertyValue>(adpropv => new AdPropertyValue
                                 {
                                     Id = adpropv.Id,
                                     AdId = adpropv.AdId,
                                     AdPropertyId = adpropv.AdPropertyId,
                                     Value = adpropv.Value,

                                     AdProperty = new AdProperty
                                     {
                                         Id = adpropv.AdProperty.Id,
                                         Name = adpropv.AdProperty.Name,
                                         PropertyType = adpropv.AdProperty.PropertyType,
                                         Range = adpropv.AdProperty.Range,
                                         SubcategoryId = adpropv.AdProperty.SubcategoryId,
                                     }
                                 }).ToList(),
                                 AdReference = ad.AdReference,
                                 AdsStatus = ad.AdsStatus,
                                 Amount = ad.Amount,
                                 CreationTime = ad.CreationTime,
                                 CreatorUserId = ad.CreatorUserId,
                                 DeleterUserId = ad.DeleterUserId,
                                 DeletionTime = ad.DeletionTime,
                                 Description = ad.Description,
                                 Id = ad.Id,
                                 InUserCart = CartItemRepository.GetAll().DefaultIfEmpty().Any(c => c.UserId == userId && c.AdId == ad.Id && c.IsDeleted == false),
                                 IsActive = ad.IsActive,
                                 IsDeleted = ad.IsDeleted,
                                 Keywords = ad.Keywords,
                                 LastModificationTime = ad.LastModificationTime,
                                 LastModifierUserId = ad.LastModifierUserId,
                                 Likes = adFeedbackRepository.GetAll().DefaultIfEmpty().Where(a => ad.Id == a.Id && !a.IsDeleted && (a.Like ?? false)).DefaultIfEmpty().Count(),
                                 Location = ad.Location,
                                 Name = ad.Name,
                                 PhoneNumber = ad.PhoneNumber,
                                 Rank = default,
                                 Rating = rtn,
                                 Reviews = adFeedbackRepository.GetAll().DefaultIfEmpty().Where(a => ad.Id == a.Id && !a.IsDeleted && !string.IsNullOrWhiteSpace(a.Review)).DefaultIfEmpty().Count(),
                                 UserId = ad.UserId,
                                 SubcategoryId = ad.SubcategoryId,
                                 VideoPath = ad.VideoPath,
                                 User = UserRepository.GetAll().Where(u => u.Id == ad.UserId).FirstOrDefault(),
                                 Subcategory = sub,
                                 Negotiable = ad.Negotiable,
                                 ContactForPrice = ad.ContactForPrice,

                             }).DistinctBy(x => x.Id);
                var data = query.Skip((page - 1) * size).Take(size).ToList();
                var total = query.Count();
                long pages = (long)Math.Ceiling(((double)total / (double)size));
                //var returnVal = data;
                //var filteredval = returnVal;
                //var integer = filteredval.ToList().Count;

                //save searches
                if (data != null && data.Count() > 0)
                {
                    //1 task .run
                    //_ = Task.Run(() =>
                    //{
                    //    try
                    //    {
                    //        _unitOfWork.BeginTransaction();
                    //        foreach (var s in data)
                    //        {
                    //            try
                    //            {
                    //                var sdata = new AdLookupLog
                    //                {
                    //                    AdId = s.Id,
                    //                    Ad = null,
                    //                    //basic properties
                    //                    CreationTime = DateTime.Now,
                    //                    CreatorUserId = user.Id,
                    //                    IsDeleted = false,
                    //                    LastModificationTime = DateTime.Now,
                    //                    LastModifierUserId = user.Id,
                    //                    DeleterUserId = null,
                    //                    DeletionTime = null,
                    //                    Id = 0,

                    //                };
                    //                AdLookupLogRepository.Insert(sdata);
                    //            }
                    //            catch (Exception ex)
                    //            {
                    //                log.Error($"{ex.Message} :: {MethodBase.GetCurrentMethod().Name} :: {ex.StackTrace} ");
                    //            }
                    //        }
                    //        _unitOfWork.Commit();
                    //    }
                    //    catch (Exception ex)
                    //    {
                    //        log.Error($"{ex.Message} :: {MethodBase.GetCurrentMethod().Name} :: {ex.StackTrace} ");
                    //    }
                    //});

                    //2 using parallel foreach
                    //_unitOfWork.BeginTransaction();
                    //Parallel.ForEach(data, (s) => {
                    //    try
                    //    {
                    //        var sdata = new AdLookupLog
                    //        {
                    //            AdId = s.Id,
                    //            Ad = null,
                    //            //basic properties
                    //            CreationTime = DateTime.Now,
                    //            CreatorUserId = user.Id,
                    //            IsDeleted = false,
                    //            LastModificationTime = DateTime.Now,
                    //            LastModifierUserId = user.Id,
                    //            DeleterUserId = null,
                    //            DeletionTime = null,
                    //            Id = 0,

                    //        };
                    //        AdLookupLogRepository.Insert(sdata);
                    //    }
                    //    catch (Exception ex)
                    //    {
                    //        log.Error($"{ex.Message} :: {MethodBase.GetCurrentMethod().Name} :: {ex.StackTrace} ");
                    //    }
                    //});
                    //_unitOfWork.Commit();

                    //3 using ordinary foreach
                    _unitOfWork.BeginTransaction();
                    foreach (var s in data)
                    {
                        try
                        {
                            var sdata = new AdLookupLog
                            {
                                AdId = s.Id,
                                Ad = null,
                                //basic properties
                                CreationTime = DateTime.Now,
                                CreatorUserId = user?.Id,
                                IsDeleted = false,
                                LastModificationTime = DateTime.Now,
                                LastModifierUserId = user?.Id,
                                DeleterUserId = null,
                                DeletionTime = null,
                                Id = 0,

                            };
                            AdLookupLogRepository.InsertAsync(sdata);
                        }
                        catch (Exception ex)
                        {
                            log.Error($"{ex.Message} :: {MethodBase.GetCurrentMethod().Name} :: {ex.StackTrace} ");
                        }
                    }
                    _unitOfWork.Commit();
                }
                return new AdResponse { Ads = data, Total = total, Pages = pages, Page = page, Size = size};
            }
            catch (Exception ex)
            {
                log.Error($"{ex.Message} :: {MethodBase.GetCurrentMethod().Name} :: {ex.StackTrace} ");
                throw ex;
            }
        }

        public async Task<bool> ApplyForJob(JobApplicationDTO model, string username, bool allowAnonymous = false)
        {
            try
            {
                #region validate credential

                //check that the model carries data
                if (model == null)
                {
                    throw await _serviceHelper.GetExceptionAsync("no input!");
                }
                //check for non-empty username
                if (string.IsNullOrWhiteSpace(username) && allowAnonymous == false)
                {
                    throw await _serviceHelper.GetExceptionAsync("Please login and retry!");
                }

                //check that the user exist
                var user = await _userSvc.FindFirstAsync(x => x.UserName == username && !x.IsDeleted);
                if (user == null && allowAnonymous == false)
                {
                    throw await _serviceHelper.GetExceptionAsync("User does not exist!");
                }

                //check that the username is a valid email ( the password would be validate by the Identity builder)
                if (!Regex.IsMatch(username, @"^[^@\s]+@[^@\s]+\.[^@\s]+$", RegexOptions.IgnoreCase) && allowAnonymous == false)
                {
                    throw await _serviceHelper.GetExceptionAsync("The username isn't a valid email");
                }
                #endregion

                #region save and send data
                JobApplication data = new JobApplication();
                data = model;

                //basic properties
                data.CreationTime = DateTime.Now;
                data.CreatorUserId = user.Id;
                data.IsDeleted = false;
                data.LastModificationTime = DateTime.Now;
                data.LastModifierUserId = user.Id;
                data.DeleterUserId = null;
                data.DeletionTime = null;
                data.Id = 0;

                _unitOfWork.BeginTransaction();
                var savedData = await JobApplicationRepository.InsertAsync(data);
                _unitOfWork.Commit();

                //check if it saved the prev-work-experience
                if(savedData != null && savedData.Id > 0 && data.PreviousWorkExperiences != null && data.PreviousWorkExperiences.Count() > 0)
                {
                    foreach(var p in data.PreviousWorkExperiences)
                    {
                        _unitOfWork.BeginTransaction();
                         await JobApplicationRepository.InsertAsync(data);
                        _unitOfWork.Commit();
                    }
                }
                //send to the job advertizer/hr
                
                _ = Task.Run(async () =>
                {
                    try
                    {
                        //first get the file
                        var filePath = Path.Combine(_hostingEnvironment.ContentRootPath, CoreConstants.Url.JobApplicationEmail);
                        if (File.Exists(filePath))
                        {
                            var fileString = File.ReadAllText(filePath);
                            if (!string.IsNullOrWhiteSpace(fileString))
                            {
                                fileString = fileString.Replace("{{FullName}}", $"{model.FullName}");
                                fileString = fileString.Replace("{{Position}}", $"{model.JobTitle}");
                                fileString = fileString.Replace("{{ContactPhoneNumber}}", $"{model.ContactPhoneNumber}");
                                fileString = fileString.Replace("{{ContactEmail}}", $"{model.CompanyEmail}");
                                fileString = fileString.Replace("{{Address}}", $"{model.Address}");
                                fileString = fileString.Replace("{{Skills}}", $"{model.Skills}");
                                fileString = fileString.Replace("{{ExpectedSalary}}", $"{model.ExpectedSalary}");
                                fileString = fileString.Replace("{{Age}}", $"{model.Age}");
                                fileString = fileString.Replace("{{Certification}}", $"{model.Certification}");
                                fileString = fileString.Replace("{{HighestLevelOfEducation}}", $"{model.HighestLevelOfEducation}");
                                fileString = fileString.Replace("{{Gender}}", $"{model.Gender}");

                                _serviceHelper.SendEMail(model.CompanyEmail, fileString, "EkiHire.com: Account Verification Code");

                                //_serviceHelper.SendEMail("damee1993@gmail.com", fileString, "(Test) EkiHire.com: Account Verification Code");
                                //_serviceHelper.SendEMail("damilola_093425@yahoo.com", fileString, "(Test) EkiHire.com: Account Verification Code");
                                //_serviceHelper.SendEMail("adegunwad@accessbankplc.com", fileString, "(Test) EkiHire.com: Account Verification Code");
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        log.Error($"{ex.Message} :: {MethodBase.GetCurrentMethod().Name} :: {ex.StackTrace} ");
                    }
                });

                #endregion

                return true;
            }
            catch (Exception ex)
            {
                log.Error($"{ex.Message} :: {MethodBase.GetCurrentMethod().Name} :: {ex.StackTrace} ");
                return false;
                //throw ex;
            }
        }
        public async Task<string> UploadFile(IFormFile file, string username)
        {
            try
            {
                MemoryStream stream = new MemoryStream();
                file.CopyTo(stream);
                var filePath = _serviceHelper.UploadPhoto(stream);
                return filePath;
            }
            catch (Exception ex)
            {
                log.Error($"error while trying to close ad for user ({username}) :: stackTrace => {ex.StackTrace}", ex);
                throw ex;
            }
        }
        public async Task<bool> CloseAd(long adid, string username)
        {
            try
            {
                #region validate the data given
                if(string.IsNullOrWhiteSpace(username))
                {
                    throw await _serviceHelper.GetExceptionAsync("Please input a username!");
                }
                var user = await _userSvc.FindFirstAsync(x => x.UserName == username && x.IsDeleted == false);
                if (user == null)
                {
                    throw await _serviceHelper.GetExceptionAsync("Unauthorized access! Please login");
                }
                var ad = adRepository.FirstOrDefault(x => x.Id == adid && x.IsDeleted == false);
                if(ad == null)
                {
                    throw await _serviceHelper.GetExceptionAsync("could not find ad! please try refreshing");
                }
                #endregion

                _unitOfWork.BeginTransaction();
                ad.IsActive = false;
                await adRepository.UpdateAsync(ad);
                _unitOfWork.Commit();
                return true;
            }
            catch (Exception ex)
            {
                _unitOfWork.Rollback();
                log.Error($"error while trying to close ad for user ({username}) :: stackTrace => {ex.StackTrace}", ex);
                return false;
            }
        }

        public async Task<bool> EditAd(AdDTO adDto, long adid, string username)
        {
            try
            {
                #region validate the data given
                if (string.IsNullOrWhiteSpace(username))
                {
                    throw await _serviceHelper.GetExceptionAsync("Please input a username!");
                }
                var loggedInUser = await _userSvc.FindFirstAsync(x => x.UserName == username && x.IsDeleted == false);
                if (loggedInUser == null)
                {
                    throw await _serviceHelper.GetExceptionAsync("Unauthorized access! Please login");
                }
                var ad = adRepository.FirstOrDefault(x => x.Id == adid && x.IsDeleted == false);
                if (ad == null)
                {
                    throw await _serviceHelper.GetExceptionAsync("could not find ad! please try refreshing");
                }
                //check that it is the owner of the ad that is editing it
                if (loggedInUser.Id != adDto.UserId && loggedInUser.UserType != UserType.Administrator)
                {
                    throw await _serviceHelper.GetExceptionAsync("you are not authorized to edit this ad!");
                }
                #endregion

                #region make the necessary edit on a property by property basis

                //name
                if (!string.IsNullOrWhiteSpace(adDto.Name) && !string.Equals(ad.Name, adDto.Name))
                {
                    ad.Name = adDto.Name;
                }
                //address
                if (!string.IsNullOrWhiteSpace(adDto.Address) && !string.Equals(ad.Address, adDto.Address))
                {
                    ad.Address = adDto.Address;
                }
                //amount
                if (adDto.Amount != null && !string.Equals(ad.Amount, adDto.Amount) && adDto.Amount >= 0)
                {
                    ad.Amount = adDto.Amount;
                }
                //ad class
                if (adDto.AdClass != null && !string.Equals(ad.AdClass.ToString(), adDto.AdClass.ToString()))
                {
                    ad.AdClass = adDto.AdClass;
                }
                //phone-number
                if (!string.IsNullOrWhiteSpace(adDto.PhoneNumber) && !string.Equals(ad.PhoneNumber, adDto.PhoneNumber))
                {
                    ad.PhoneNumber = adDto.PhoneNumber;
                }
                //location
                if (!string.IsNullOrWhiteSpace(adDto.Location) && !string.Equals(ad.Location, adDto.Location))
                {
                    ad.Location = adDto.Location;
                }
                // remove AdReference
                //Description
                if (!string.IsNullOrWhiteSpace(adDto.Description) && !string.Equals(ad.Description, adDto.Description))
                {
                    ad.Description = adDto.Description;
                }
                //video path
                if (!string.IsNullOrWhiteSpace(adDto.VideoPath) && !string.Equals(ad.VideoPath, adDto.VideoPath))
                {
                    ad.VideoPath = adDto.VideoPath;
                }
                //key words
                if (!string.IsNullOrWhiteSpace(adDto.Keywords) && !string.Equals(ad.Keywords, adDto.Keywords))
                {
                    ad.Keywords = adDto.Keywords;
                }
                //is active
                if (adDto.IsActive != null && !string.Equals(ad.IsActive.ToString(), adDto.IsActive.ToString()))
                {
                    ad.IsActive = adDto.IsActive;
                }
                //ad status
                if (adDto.AdsStatus != null && !string.Equals(ad.AdsStatus.ToString(), adDto.AdsStatus.ToString()))
                {
                    ad.AdsStatus = adDto.AdsStatus;
                }
                //remove sub, cat, user
                //images
                if(adDto.AdImages != null && adDto.AdImages.Count() > 0)
                {
                    var imgs = _AdImageRepo.GetAll().Where(i => i.AdId == ad.Id && i.IsDeleted == false).ToList() ?? new List<AdImage>();
                    foreach (var img in adDto.AdImages)
                    {
                        _unitOfWork.BeginTransaction();
                        if (imgs.Any(i => i.Id == img.Id))
                        {
                            //update an imaage
                            var imgData = imgs.Where(i => i.Id == img.Id).FirstOrDefault();
                            imgData.ImagePath = string.IsNullOrWhiteSpace(img.ImagePath)? imgData.ImagePath: img.ImagePath;
                            //imgData.ImageString = img.ImageString;
                            _AdImageRepo.Update(imgData);
                        }
                        _unitOfWork.Commit();
                        //else
                        //{
                        //    //insert
                        //    img.AdId = ad.Id;
                        //    _AdImageRepo.Insert(img);
                        //}
                    }
                }
                //property value
                if(adDto.AdPropertyValue != null && adDto.AdPropertyValue.Count() > 0)
                {
                    //check for all the property-value available to the ad
                    //var pvs = _adPropertyValueRepo.GetAll().Where(p => p.AdId == ad.Id).ToList();
                    foreach(var p in adDto.AdPropertyValue)
                    {
                        p.AdId = ad.Id;
                        await AddOrUpdateAdPropertyValue(p, username);
                    }
                }
                _unitOfWork.BeginTransaction();
                await adRepository.UpdateAsync(ad);
                _unitOfWork.Commit();
                #endregion
                return true;
            }
            catch (Exception ex)
            {
                _unitOfWork.Rollback();
                log.Error($"error while editing ad for user ({username}) :: stackTrace => {ex.StackTrace}", ex);
                return false;
            }
        }
        public async Task<bool> PromoteAd(long adid, string username)
        {
            try
            {
                #region validate the data given
                if (string.IsNullOrWhiteSpace(username))
                {
                    throw await _serviceHelper.GetExceptionAsync("Please input a username!");
                }
                var user = await _userSvc.FindFirstAsync(x => x.UserName == username && x.IsDeleted == false);
                if (user == null)
                {
                    throw await _serviceHelper.GetExceptionAsync("Unauthorized access! Please login");
                }
                var ad = adRepository.FirstOrDefault(x => x.Id == adid && x.IsDeleted == false);
                if (ad == null)
                {
                    throw await _serviceHelper.GetExceptionAsync("could not find ad! please try refreshing");
                }

                #endregion
                _unitOfWork.BeginTransaction();
                ad.AdClass = AdClass.Premium;
                await adRepository .UpdateAsync(ad);
                _unitOfWork.Commit();
                return true;
            }
            catch (Exception ex)
            {
                _unitOfWork.Rollback();
                log.Error($"error while promoting ad for user ({username}) :: stackTrace => {ex.StackTrace}", ex);
                return false;
            }
        }

        public async Task<bool> CreateItem(ItemDTO model, string username)
        {//admin function
            try
            {
                #region validation
                if (string.IsNullOrWhiteSpace(username))
                {
                    throw await _serviceHelper.GetExceptionAsync("Please input a username!");
                }
                var user = await _userSvc.FindFirstAsync(x => x.UserName == username && x.IsDeleted == false);
                if (user == null)
                {
                    throw await _serviceHelper.GetExceptionAsync("Unauthorized access! Please login");
                }
                #endregion
                _unitOfWork.BeginTransaction();
                await itemRepository.InsertAsync(model);
                _unitOfWork.Commit();
                return true;
            }
            catch (Exception ex)
            {
                _unitOfWork.Rollback();
                log.Error($"error while trying to create Item :: stackTrace => {ex.StackTrace}", ex);
                return false;
            }
        }

        public async Task<bool> EditItemKeywords(List<string> keywords, long ItemId, string username)
        {//admin function
            try
            {
                #region validation
                if (string.IsNullOrWhiteSpace(username))
                {
                    throw await _serviceHelper.GetExceptionAsync("Please input a username!");
                }
                var user = await _userSvc.FindFirstAsync(x => x.UserName == username && x.IsDeleted == false);
                if (user == null)
                {
                    throw await _serviceHelper.GetExceptionAsync("Unauthorized access! Please login");
                }
                var item = itemRepository.FirstOrDefault(x => x.Id == ItemId && x.IsDeleted == false);
                if (item == null)
                {
                    throw await _serviceHelper.GetExceptionAsync("Item not found!");
                }
                #endregion
                _unitOfWork.BeginTransaction();
                var existingKw = item.Keywords.Split(',').Distinct().ToList();
                var comingKw = keywords.Distinct().ToList();

                var u = existingKw.Union(comingKw).ToList();
                var k = string.Join(",", u);

                item.Keywords = k;
                await itemRepository.UpdateAsync(item);
                _unitOfWork.Commit();
                return true;
            }
            catch (Exception ex)
            {
                _unitOfWork.Rollback();
                log.Error($"{ex.Message} :: {MethodBase.GetCurrentMethod().Name} :: {ex.StackTrace} ");
                return false;
            }
        }

        public async Task<bool> GroupAdItems(long[] ItemIds, string groupname, string username)
        {
            try
            {
                #region validation
                if (string.IsNullOrWhiteSpace(username))
                {
                    throw await _serviceHelper.GetExceptionAsync("Please input a username!");
                }
                var user = await _userSvc.FindFirstAsync(x => x.UserName == username && x.IsDeleted == false);
                if (user == null)
                {
                    throw await _serviceHelper.GetExceptionAsync("Unauthorized access! Please login");
                }

                #endregion
                //_unitOfWork.BeginTransaction();
                itemRepository.GetAll().Where(x => ItemIds.Contains(x.Id) && x.IsDeleted == false).AsTracking().ToList().ForEach(y => y.GroupName = groupname);
                //_unitOfWork.Commit();
                return true;
            }
            catch (Exception ex)
            {
                _unitOfWork.Rollback();
                log.Error($"{ex.Message} :: {MethodBase.GetCurrentMethod().Name} :: {ex.StackTrace} ");
                return false;
            }
        }

        public async Task<bool> AddAdToCart(long adId, string username)
        {
            try
            {
                #region validation
                if (string.IsNullOrWhiteSpace(username))
                {
                    throw await _serviceHelper.GetExceptionAsync("Please input a username!");
                }
                var user = await _userSvc.FindFirstAsync(x => x.UserName == username && !x.IsDeleted);
                if (user == null)
                {
                    throw await _serviceHelper.GetExceptionAsync("Unauthorized access! Please login");
                }
                var ad = adRepository.FirstOrDefault(x => x.Id == adId && x.IsDeleted == false);
                if(ad == null)
                {
                    throw await _serviceHelper.GetExceptionAsync("Cannot find ad!");
                }
                #endregion
                #region add ad to cart
                //check if already exist
                var cartItem = CartItemRepository.FirstOrDefault(x => x.AdId == adId && x.UserId == user.Id && x.IsDeleted == false);
                if(cartItem != null)
                {
                    if(cartItem.IsDeleted)
                    {
                        cartItem.IsDeleted = false;
                        cartItem.LastModificationTime = DateTime.Now;
                        cartItem.LastModifierUserId = user.Id;
                        _unitOfWork.BeginTransaction();
                        await CartItemRepository.UpdateAsync(cartItem);
                        _unitOfWork.Commit();
                    }
                    return true;
                }
                var data = new CartItem
                {
                    AdId = ad.Id,
                    UserId = user.Id,

                    //basic properties
                    CreationTime = DateTime.Now,
                    CreatorUserId = user.Id,
                    IsDeleted = false,
                    LastModificationTime = DateTime.Now,
                    LastModifierUserId = user.Id,
                    DeleterUserId = null,
                    DeletionTime = null,
                    Id = 0
                };
                _unitOfWork.BeginTransaction();
                await CartItemRepository.InsertAsync(data);
                _unitOfWork.Commit();
                #endregion
                return true;
            }
            catch (Exception ex)
            {
                _unitOfWork.Rollback();
                log.Error($"{ex.Message} :: {MethodBase.GetCurrentMethod().Name} :: {ex.StackTrace} ");
                return false;
            }
        }

        public async Task<bool> RemoveAdFromCart(long adId, string username)
        {
            try
            {
                #region validation
                if (string.IsNullOrWhiteSpace(username))
                {
                    throw await _serviceHelper.GetExceptionAsync("Please input a username!");
                }
                var user = await _userSvc.FindFirstAsync(x => x.UserName == username && x.IsDeleted == false);
                if (user == null)
                {
                    throw await _serviceHelper.GetExceptionAsync("Unauthorized access! Please login");
                }
                var ad = adRepository.FirstOrDefault(x => x.Id == adId && x.IsDeleted == false);
                if (ad == null)
                {
                    throw await _serviceHelper.GetExceptionAsync("Cannot find ad!");
                }
                #endregion
                #region add ad to cart
                var data = CartItemRepository.FirstOrDefault(x => x.AdId == adId && x.UserId == user.Id && x.IsDeleted == false);
                if(data == null)
                {
                    return true;
                }

                _unitOfWork.BeginTransaction();
                await CartItemRepository.DeleteAsync(data);
                _unitOfWork.Commit();
                #endregion
                return true;
            }
            catch (Exception ex)
            {
                _unitOfWork.Rollback();
                log.Error($"{ex.Message} :: {MethodBase.GetCurrentMethod().Name} :: {ex.StackTrace} ");
                return false;
            }
        }
        #region reviews
        
        public async Task<IEnumerable<AdFeedbackDTO>> ReviewsGivenByUser(string username/*, long[] adIds = null*/)
        {
            try
            {
                #region validation
                if (string.IsNullOrWhiteSpace(username))
                {
                    throw await _serviceHelper.GetExceptionAsync("Please input a username!");
                }
                var user = await _userSvc.FindFirstAsync(x => x.UserName == username && x.IsDeleted == false);
                if (user == null)
                {
                    throw await _serviceHelper.GetExceptionAsync("Unauthorized access! Please login");
                }
                #endregion
                var result = new List<AdFeedback>();
                result = await adFeedbackRepository.GetAll()?.Where(a => a.UserId == user.Id && a.IsDeleted == false
                //&& (adIds.Contains(a.AdId) || adIds == null)
                )?.ToListAsync();
                return result?.ToDTO();
            }
            catch (Exception ex)
            {
                log.Error($"A error occured while trying to get reviews - error - {ex.Message} - stackTraack - {ex.StackTrace} :: {MethodBase.GetCurrentMethod().Name}", ex);
                return null;
            }
        }
        
        public async Task<IEnumerable<AdFeedbackDTO>> ReviewsForAd(long AdId, string username/*, long[] adIds = null*/)
        {
            try
            {
                #region validation
                if (string.IsNullOrWhiteSpace(username))
                {
                    throw await _serviceHelper.GetExceptionAsync("Please input a username!");
                }
                var user = await _userSvc.FindFirstAsync(x => x.UserName == username && x.IsDeleted == false);
                if (user == null)
                {
                    throw await _serviceHelper.GetExceptionAsync("Unauthorized access! Please login");
                }
                #endregion
                //var result = (from af in adFeedbackRepository.GetAll()
                //        join ad in adRepository.GetAll() on af.AdId equals ad.Id
                //        where ad.UserId == user.Id && af.IsDeleted == false
                //        select af).ToList();
                var result = adFeedbackRepository.GetAll()?.Where(r => r.AdId == AdId)?.ToList();
                //var result = adFeedbackRepository.GetAll().AsEnumerable().Where(a => getUserFromAd(a.AdId) == user.Id
                ////&& (adIds.Contains(a.AdId) || adIds == null)
                //).ToList();
                return result?.ToDTO();
            }
            catch (Exception ex)
            {
                log.Error($"An error occured while trying to get reviews - error - {ex.Message} - stackTraack - {ex.StackTrace} :: {MethodBase.GetCurrentMethod().Name}", ex);
                return null;
            }
        }

        public async Task<bool> SaveReview(AdFeedback model, string username)
        {
            try
            {
                #region validate input

                //check that the model carries data
                if (model == null)
                {
                    throw await _serviceHelper.GetExceptionAsync("no input");
                }
                //check for non-empty username 
                if (string.IsNullOrWhiteSpace(username))
                {
                    throw await _serviceHelper.GetExceptionAsync("Please login and retry");
                }

                //check that the user exist
                var loggedInUser = await _userSvc.FindFirstAsync(x => x.UserName == username);
                if (loggedInUser == null)
                {
                    throw await _serviceHelper.GetExceptionAsync("User does not exist");
                }

                //check that the AdId was passed and that it is valid 
                var ad = adRepository.FirstOrDefault(x => x.Id == model.AdId);
                if(ad == null)
                {
                    throw new Exception("Invalid Ad Id");
                }
                #endregion

                //first check if the user has a row in the db for feedback (that is not deleted)
                AdFeedback data = model;
                data.AdId = model.AdId;
                data.UserId = loggedInUser.Id;

                //basic properties
                data.CreationTime = DateTime.Now;
                data.CreatorUserId = loggedInUser.Id;
                data.IsDeleted = false;
                data.LastModificationTime = DateTime.Now;
                data.LastModifierUserId = loggedInUser.Id;
                data.DeleterUserId = null;
                data.DeletionTime = null;
                data.Id = 0;
                var feedback = adFeedbackRepository.FirstOrDefault(f => f.UserId == loggedInUser.Id && f.AdId == model.AdId);
                _unitOfWork.BeginTransaction();
                if(feedback == null)
                {
                    adFeedbackRepository.Insert(data);
                }
                else
                {
                    if(data.Like != null)
                    {
                        feedback.Like = data.Like;
                    }
                    if (!string.IsNullOrWhiteSpace(data.Review))
                    {
                        feedback.Review = data.Review;
                    }
                    if (data.Rating != null)
                    {
                        feedback.Rating = data.Rating;
                    }
                    adFeedbackRepository.Update(feedback);
                }
                _unitOfWork.Commit();

                return true;
            }
            catch (Exception ex)
            {
                _unitOfWork.Rollback();
                log.Error($"an error occured while trying to get {username} followers:: stack-trace - {ex.StackTrace}", ex);
                throw ex;
            }
        }
        #endregion
        #region follow
        public async Task<IEnumerable<Follow>> GetFollowers(string username)
        {
            try
            {
                #region validation
                if (string.IsNullOrWhiteSpace(username))
                {
                    throw await _serviceHelper.GetExceptionAsync("Please input a username!");
                }
                var user = await _userSvc.FindFirstAsync(x => x.UserName == username && x.IsDeleted == false);
                if (user == null)
                {
                    throw await _serviceHelper.GetExceptionAsync("Unauthorized access! Please login");
                }
                #endregion
                var result = new List<Follow>();
                result = await followRepository.GetAll().Where(f => f.Following.UserName == username && f.IsDeleted == false).ToListAsync();
                return result;
            }
            catch (Exception ex)
            {
                log.Error($"an error occured while trying to get {username} followers:: stack-trace - {ex.StackTrace}", ex);
                return null;
            }
        }
        public async Task<IEnumerable<Follow>> GetFollowing(string username)
        {
            try
            {
                #region validation
                if (string.IsNullOrWhiteSpace(username))
                {
                    throw await _serviceHelper.GetExceptionAsync("Please input a username!");
                }
                var user = await _userSvc.FindFirstAsync(x => x.UserName == username && x.IsDeleted == false);
                if (user == null)
                {
                    throw await _serviceHelper.GetExceptionAsync("Unauthorized access! Please login");
                }
                #endregion
                var result = new List<Follow>();
                result = await followRepository.GetAll().Where(f => f.FollowerId == user.Id && f.IsDeleted == false).ToListAsync();
                return result;
            }
            catch (Exception ex)
            {
                log.Error($"an error occured while trying to get people {username} is following:: stack-trace - {ex.StackTrace}", ex);
                return null;
            }
        }
        #endregion
        #region keywords
        public async Task<bool> AddKeywords(List<string> keywords, long subid, string username)
        {
            try
            {
                #region validation
                if (string.IsNullOrWhiteSpace(username))
                {
                    throw await _serviceHelper.GetExceptionAsync("Please input a username!");
                }
                var user = await _userSvc.FindFirstAsync(x => x.UserName == username && x.IsDeleted == false);
                if (user == null)
                {
                    throw await _serviceHelper.GetExceptionAsync("Unauthorized access! Please login");
                }
                var subcategory = _subcategoryRepo.FirstOrDefault(s => s.Id == subid && s.IsDeleted == false); 
                if(subcategory == null)
                {
                    throw await _serviceHelper.GetExceptionAsync("The subcategory does not exist");
                }
                if(keywords == null || keywords.Count() == 0)
                {
                    throw await _serviceHelper.GetExceptionAsync("Invalid data!");
                }
                #endregion
                _unitOfWork.BeginTransaction();
                foreach(var k in keywords)
                {
                    Keyword keyword = new Keyword
                    {
                        Name = k,
                        SubcategoryId = subcategory.Id,

                        //basic properties
                        CreationTime = DateTime.Now,
                        CreatorUserId = user.Id,
                        IsDeleted = false,
                        LastModificationTime = DateTime.Now,
                        LastModifierUserId = user.Id,
                        DeleterUserId = null,
                        DeletionTime = null,
                        Id = 0
                    };
                    await _keywordRepo.InsertAsync(keyword);
                }
                _unitOfWork.Commit();
                return true;
                
            }
            catch (Exception ex)
            {
                log.Error($"{ex.Message} :: {MethodBase.GetCurrentMethod().Name} :: {ex.StackTrace} ");
                return false;
            }
        }
        public async Task<bool> EditKeywords(long kwId, string correctedWord, string username)
        {
            try
            {
                #region validation
                if (string.IsNullOrWhiteSpace(username))
                {
                    throw await _serviceHelper.GetExceptionAsync("Please input a username!");
                }
                var user = await _userSvc.FindFirstAsync(x => x.UserName == username && x.IsDeleted == false);
                if (user == null)
                {
                    throw await _serviceHelper.GetExceptionAsync("Unauthorized access! Please login");
                }
                if (string.IsNullOrWhiteSpace(correctedWord))
                {
                    throw await _serviceHelper.GetExceptionAsync("Invalid data!!");
                }
                var keyword = _keywordRepo.FirstOrDefault(k => k.Id == kwId && k.IsDeleted == false);
                if (keyword == null)
                {
                    throw await _serviceHelper.GetExceptionAsync("can't find keyword!");
                }
                #endregion

                _unitOfWork.BeginTransaction();
                keyword.Name = correctedWord;
                await _keywordRepo.UpdateAsync(keyword);
                _unitOfWork.Commit();

                return true;
            }
            catch (Exception ex)
            {
                log.Error($"{ex.Message} :: {MethodBase.GetCurrentMethod().Name} :: {ex.StackTrace} ");
                return false;
            }
        }
        public async Task<bool> DeleteKeywords(long kwId, string username)
        {
            try
            {
                #region validation
                if (string.IsNullOrWhiteSpace(username))
                {
                    throw await _serviceHelper.GetExceptionAsync("Please input a username!");
                }
                var user = await _userSvc.FindFirstAsync(x => x.UserName == username && x.IsDeleted == false);
                if (user == null)
                {
                    throw await _serviceHelper.GetExceptionAsync("Unauthorized access! Please login");
                }
                var keyword = _keywordRepo.FirstOrDefault(k => k.Id == kwId && k.IsDeleted == false);
                if (keyword == null)
                {
                    throw await _serviceHelper.GetExceptionAsync("can't find keyword!");
                }
                #endregion
                _unitOfWork.BeginTransaction();
                await _keywordRepo.DeleteAsync(keyword);
                _unitOfWork.Commit();

                return true;
            }
            catch (Exception ex)
            {
                _unitOfWork.Rollback();
                log.Error($"{ex.Message} :: {MethodBase.GetCurrentMethod().Name} :: {ex.StackTrace} ");
                return false;
            }
        }
        public async Task<IEnumerable<Keyword>> GetKeywords(string username, long[] kwIds = null, long? subid= null)
        {
            try
            {
                #region validation
                if (string.IsNullOrWhiteSpace(username))
                {
                    throw await _serviceHelper.GetExceptionAsync("Please input a username!");
                }
                var user = await _userSvc.FindFirstAsync(x => x.UserName == username && x.IsDeleted == false);
                if (user == null)
                {
                    throw await _serviceHelper.GetExceptionAsync("Unauthorized access! Please login");
                }
                #endregion
                List<Keyword> result = new List<Keyword>();
                result = await _keywordRepo.GetAll().Where(k => (kwIds.Contains(k.Id) || kwIds == null)
                && (k.Subcategory.Id == subid || subid == null)
                && k.IsDeleted == false).ToListAsync();

                return result;
            }
            catch (Exception ex)
            {
                log.Error($"{ex.Message} :: {MethodBase.GetCurrentMethod().Name} :: {ex.StackTrace} ");
                return null;
            }
        }
        #endregion

        #region ad property and value
        public async Task<IEnumerable<AdProperty>> GetAdPropertiesBySubcategory(long subId, string username)
        {
            try
            {
                #region validation
                if (string.IsNullOrWhiteSpace(username))
                {
                    throw await _serviceHelper.GetExceptionAsync("Please input a username!");
                }
                var user = await _userSvc.FindFirstAsync(x => x.UserName == username && x.IsDeleted == false);
                if (user == null)
                {
                    throw await _serviceHelper.GetExceptionAsync("Unauthorized access! Please login");
                }
                #endregion
                List<AdProperty> result = new List<AdProperty>();
                result = await _adPropertyRepo.GetAllIncluding(x => x.Subcategory).Where(a => a.SubcategoryId == subId && a.IsDeleted == false).ToListAsync();
                return result;
            }
            catch (Exception ex)
            {
                log.Error($"{ex.Message} :: {MethodBase.GetCurrentMethod().Name} :: {ex.StackTrace} ");
                return null;
            }
        }

        public async Task<IEnumerable<AdPropertyValue>> GetAdPropertiesWithValue(long adid, string username)
        {
            try
            {
                #region validation
                if (string.IsNullOrWhiteSpace(username))
                {
                    throw await _serviceHelper.GetExceptionAsync("Please input a username!");
                }
                var user = await _userSvc.FindFirstAsync(x => x.UserName == username && x.IsDeleted == false);
                if (user == null)
                {
                    throw await _serviceHelper.GetExceptionAsync("Unauthorized access! Please login");
                }
                #endregion
                List<AdPropertyValue> result = new List<AdPropertyValue>();
                result = await _adPropertyValueRepo.GetAllIncluding(x => x.AdProperty).Where(a => a.Ad.Id == adid && a.IsDeleted == false
                //&& (adPropertyIds.Contains(a.AdProperty.Id) || adPropertyIds == null)
                ).ToListAsync();
                return result;
            }
            catch (Exception ex)
            {
                log.Error($"{ex.Message} :: {MethodBase.GetCurrentMethod().Name} :: {ex.StackTrace} ");
                return null;
            }
        }

        public async Task<bool> AddAdProperty(AdPropertyDTO model, string username)
        {
            try
            {
                #region validation
                if (string.IsNullOrWhiteSpace(username))
                {
                    throw await _serviceHelper.GetExceptionAsync("Please input a username!");
                }
                var user = await _userSvc.FindFirstAsync(x => x.UserName == username && x.IsDeleted == false);
                if (user == null)
                {
                    throw await _serviceHelper.GetExceptionAsync("Unauthorized access! Please login");
                }
                if (model == null || string.IsNullOrWhiteSpace(model.Name))
                {
                    throw await _serviceHelper.GetExceptionAsync("Invalid data!");
                }
                var subcat = _subcategoryRepo.FirstOrDefault(s => s.Id == model.SubcategoryId && s.IsDeleted == false);
                if (subcat == null)
                {
                    throw await _serviceHelper.GetExceptionAsync("Invalid subcategory!");
                }
                var adprop = _adPropertyRepo.FirstOrDefault(a => a.Subcategory.Id == model.SubcategoryId && a.Name.ToLower() == model.Name.ToLower() && a.IsDeleted == false);
                if (adprop != null)
                {
                    throw await _serviceHelper.GetExceptionAsync("This property has already been created for this subcategory!");
                    //throw new Exception("This property has already been created for this subcategory!");
                }
                #endregion

                #region add a new property for ad in the selected subcategory
                AdProperty data = new AdProperty
                {
                    Name = model.Name,
                    PropertyType = model.PropertyType,
                    Range = model.Range,
                    Subcategory = subcat,
                    //basic properties
                    CreationTime = DateTime.Now,
                    CreatorUserId = user.Id,
                    LastModificationTime = DateTime.Now,
                    LastModifierUserId = user.Id,
                    
                };
                _unitOfWork.BeginTransaction();
                await _adPropertyRepo.InsertAsync(data);
                _unitOfWork.Commit();
                #endregion

                return true;
            }
            catch (Exception ex)
            {
                _unitOfWork.Rollback();
                log.Error($"{ex.Message} :: {MethodBase.GetCurrentMethod().Name} :: {ex.StackTrace} ");
                return false;
                //throw ex;
            }
        }

        public async Task<bool> AddOrUpdateAdPropertyValue(AdPropertyValue model, string username)
        {
            try
            {
                #region validation
                if (string.IsNullOrWhiteSpace(username))
                {
                    throw await _serviceHelper.GetExceptionAsync("Please input a username!");
                }
                var user = await _userSvc.FindFirstAsync(x => x.UserName == username && x.IsDeleted == false);
                if (user == null)
                {
                    throw await _serviceHelper.GetExceptionAsync("Unauthorized access! Please login");
                }
                if (model == null)
                {
                    throw await _serviceHelper.GetExceptionAsync("Invalid data!");
                }
                var ad = adRepository.FirstOrDefault(s => s.Id == model.AdId && s.IsDeleted == false);
                if (ad == null)
                {
                    throw await _serviceHelper.GetExceptionAsync("Invalid Ad!");
                }
                var adProperty = _adPropertyRepo.FirstOrDefault(a => a.Id == model.AdPropertyId && a.IsDeleted == false);
                if (adProperty == null)
                {
                    throw await _serviceHelper.GetExceptionAsync("Invalid adProperty!");
                }
                var adPropertyValue = _adPropertyValueRepo.FirstOrDefault(a => a.AdId == model.AdId && a.AdPropertyId == model.AdPropertyId && a.IsDeleted == false);

                #endregion

                #region Add Ad Property Value
                _unitOfWork.BeginTransaction();
          
                if (adPropertyValue == null)
                {
                    AdPropertyValue data = new AdPropertyValue
                    {
                        Ad = ad,
                        AdId = model.AdId,
                        //AdProperty = adProperty,
                        AdPropertyId = model.AdPropertyId,
                        Value = model.Value,
                        //basic properties
                        CreationTime = DateTime.Now,
                        CreatorUserId = user.Id,
                        IsDeleted = false,
                        LastModificationTime = DateTime.Now,
                        LastModifierUserId = user.Id,
                        DeleterUserId = null,
                        DeletionTime = null,
                        Id = 0,
                        
                    };
                    await _adPropertyValueRepo.InsertAsync(data);
                }
                else
                {
                    adPropertyValue.Value = model.Value;
                    await _adPropertyValueRepo.UpdateAsync(adPropertyValue);
                }
                _unitOfWork.Commit();
                #endregion
                return true;
            }
            catch (Exception ex)
            {
                _unitOfWork.Rollback();
                log.Error($"{ex.Message} :: {MethodBase.GetCurrentMethod().Name} :: {ex.StackTrace}");
                return false;
            }
        }

        public async Task<bool> UpdateAdProperty(AdProperty model, string username)
        {
            try
            {
                #region validation
                if (string.IsNullOrWhiteSpace(username))
                {
                    throw await _serviceHelper.GetExceptionAsync("Please input a username!");
                }
                var user = await _userSvc.FindFirstAsync(x => x.UserName == username && x.IsDeleted == false);
                if (user == null)
                {
                    throw await _serviceHelper.GetExceptionAsync("Unauthorized access! Please login");
                }
                if (model == null || string.IsNullOrWhiteSpace(model.Name) || model.PropertyType == null)
                {
                    throw await _serviceHelper.GetExceptionAsync("Invalid data!");
                }
                var subcat = _subcategoryRepo.FirstOrDefault(s => s.Id == model.SubcategoryId && s.IsDeleted == false);
                if (subcat == null)
                {
                    throw await _serviceHelper.GetExceptionAsync("Invalid subcategory!");
                }
                var adprop = _adPropertyRepo.FirstOrDefault(a => a.SubcategoryId == model.SubcategoryId && a.Name == model.Name && a.IsDeleted == false);
                if (adprop != null)
                {
                    throw await _serviceHelper.GetExceptionAsync("This property has already been created for this subcategory!");
                }
                var data = _adPropertyRepo.FirstOrDefault(a => a.Id == model.Id && a.IsDeleted == false);
                if(data == null)
                {
                    throw await _serviceHelper.GetExceptionAsync("The property does not exist!");
                }
                #endregion
                data.Name = model.Name;
                data.Range = string.IsNullOrWhiteSpace(model.Range) ? data.Range : model.Range;
                data.PropertyType = model.PropertyType;

                _unitOfWork.BeginTransaction();
                await _adPropertyRepo.UpdateAsync(data);
                _unitOfWork.Commit();

                return true; 
            }
            catch (Exception ex)
            {
                _unitOfWork.Rollback();
                log.Error($"{ex.Message} :: {MethodBase.GetCurrentMethod().Name} :: {ex.StackTrace} ");
                //return false;
                throw ex;
            }
        }
        //create new ad property, update ad prop, delete,
        #endregion

        public async Task<IEnumerable<AdDTO>> Trending(long count = 10, string username = null, bool allowanonymous = false)
        {
            try
            {
                List<AdDTO> result = new List<AdDTO>();
                //result = adRepository.GetAll().Where(a => TrendingRank(a, out a) > 0).OrderByDescending(ax => ax.Rank).Take((int)count).ToDTO().ToList();
                //_ = "inter-commission";
                //result = adRepository.GetAll().Select(model => TrendingRank(model)).AsEnumerable().OrderByDescending(ab => ab.Rank).Take((int)count).ToDTO().ToList();
                //result = (from a in adRepository.GetAll() select TrendingRank(a)).OrderByDescending(ab => ab.Rank).Take((int)count).ToDTO().ToList();
                List<Ad> ar = new List<Ad>();
                var aList = (await GetAds(new AdFilter(), username, allowanonymous)).Ads;//adRepository.GetAll().Where(x => x.IsDeleted == false).ToList();
                Parallel.ForEach(aList, (a) => {
                    var data = TrendingRank(a);
                    ar.Add(data);
                }) ;
                //foreach (var a in aList)
                //{
                //    var data = TrendingRank(a);
                //    ar.Add(data);
                //}
                result = ar.OrderByDescending(ab => ab.Rank).Take((int)count).ToDTO().ToList();
                return result;
            }
            catch (Exception ex)
            {
                log.Error($"{ex.Message} :: {MethodBase.GetCurrentMethod().Name} :: {ex.StackTrace} ");
                return null;
            }
        }
        
        private Ad TrendingRank(Ad model)
        {
            try
            {
                var reviews = 0; var likes = 0; var searches = 0; var daysSincePosts = 0;
                reviews = adFeedbackRepository.GetAll().Where(a => a.AdId == model.Id && !string.IsNullOrWhiteSpace(a.Review) && a.IsDeleted == false).Count();
                likes = adFeedbackRepository.GetAll().Where(a => a.AdId == model.Id && (a.Like??false) && a.IsDeleted == false).Count();
                searches = AdLookupLogRepository.GetAll().Where(s => s.AdId == model.Id && s.IsDeleted == false).Count();
                daysSincePosts = (DateTime.Now.Date - model.CreationTime.Date).Days; daysSincePosts = daysSincePosts < 1 ? 1 : daysSincePosts;

                double rank = ((reviews * appConfig.ReviewsWeight) + (likes * appConfig.LikesWeight) + (searches * appConfig.SearchWeight)) / (daysSincePosts * appConfig.DaysSincePostWeight);

                //model.Rank = rank;
                //ad = model;
                //return rank;

                ////test
                //rank = 5;
                model.Rank = rank;
                return model;
            }
            catch (Exception ex)
            {
                log.Error($"{ex.Message} :: {MethodBase.GetCurrentMethod().Name} :: {ex.StackTrace} ");
                //ad = model;
                //return 0;
                return model;
            }
        }

        public async Task<bool> UpdateAdStatus(long AdId, AdsStatus adsStatus)
        {
            try
            {
                #region validate
                var ad = await adRepository.GetAll().FirstOrDefaultAsync(a => a.Id == AdId && a.IsDeleted == false);
                if(ad == null)
                {
                    throw new Exception("Ad not found!");
                }

                //check that the person is an admin
                #endregion
                _unitOfWork.BeginTransaction();
                ad.AdsStatus = adsStatus;
                adRepository.Update(ad);
                _unitOfWork.Commit();

                #region send notification
                //first compose Welcome notification
                Notification adsStatusNotification = new Notification
                {
                    Delivered = false,
                    IsBroadCast = false,
                    Message = adsStatus == AdsStatus.ACTIVE ? 
                    "This is to let you know that your ad has been accepted on this platform. feel free to leverage all of the exciting offer we have on this platform" : 
                    "Sorry your ad has been declined, this is possibly because it failed to meet some of our criteria. you can read about our policy or contact us for clarification.",
                    Title = adsStatus == AdsStatus.ACTIVE ? "EkiHire.com: Your Ad have been approved!" : "EkiHire.com: Ad declined",
                    RecipientId = ad.UserId.GetValueOrDefault(),
                    NotificationType = adsStatus == AdsStatus.ACTIVE ? NotificationType.AdApproval : NotificationType.AdDenial,
                    Recipient = null,
                    //basic properties
                    CreationTime = DateTime.Now,
                    CreatorUserId = ad.UserId,
                    IsDeleted = false,
                    LastModificationTime = DateTime.Now,
                    LastModifierUserId = ad.UserId,
                    DeleterUserId = null,
                    DeletionTime = null,
                    Id = 0,
                };
                _chatHub.SendNotification(adsStatusNotification);
                #endregion
                return true;
            }
            catch (Exception ex)
            {
                _unitOfWork.Rollback();
                log.Error($"{ex.Message} :: {MethodBase.GetCurrentMethod().Name} :: {ex.StackTrace} ");
                return false;
            }
        }
        public async Task<bool> AddAdImage(AdImageDTO model, string username)
        {
            try
            {
                #region validation
                if (string.IsNullOrWhiteSpace(username))
                {
                    throw await _serviceHelper.GetExceptionAsync("Please input a username!");
                }
                var user = await _userSvc.FindFirstAsync(x => x.UserName == username && x.IsDeleted == false);
                if (user == null)
                {
                    throw await _serviceHelper.GetExceptionAsync("Unauthorized access! Please login");
                }
                var ad = await adRepository.GetAll().FirstOrDefaultAsync(a => a.Id == model.AdId && a.IsDeleted == false);
                if (ad == null)
                {
                    throw new Exception("Ad not found!");
                }
                #endregion
                _unitOfWork.BeginTransaction();
                AdImage data = new AdImage
                {
                    Ad = ad,
                    AdId = model.AdId,
                    ImagePath = model.ImagePath,
                    //ImageString = model.ImageString,
                    //basic properties
                    CreationTime = DateTime.Now,
                    CreatorUserId = user.Id,
                    IsDeleted = false,
                    LastModificationTime = DateTime.Now,
                    LastModifierUserId = user.Id,
                    DeleterUserId = null,
                    DeletionTime = null,
                    Id = 0,

                };
                await _AdImageRepo.InsertAsync(data);
                _unitOfWork.Commit();
                return true;
            }
            catch (Exception ex)
            {
                _unitOfWork.Rollback();
                log.Error($"{ex.Message} :: {MethodBase.GetCurrentMethod().Name} :: {ex.StackTrace} ");
                return false;
            }
        }

        public async Task<List<string>> AddAdImage(long AdId, IFormFileCollection images, string username)
        {
            try
            {
                //images upload
                List<string> imgList = new List<string>();
                if (images.Count > 0)
                {
                    _unitOfWork.BeginTransaction();
                    foreach (var image in images)
                    {
                        try
                        {
                            //byte[] byteArray = System.IO.File.ReadAllBytes(@"C:\images\cow.png");
                            using (System.IO.MemoryStream stream = new System.IO.MemoryStream())
                            {
                                image.CopyTo(stream);
                                var imgPath = _serviceHelper.UploadPhoto(stream);
                                imgList.Add(imgPath);
                                if (AdId != 0)
                                {
                                    _AdImageRepo.Insert(new AdImage { AdId = AdId, ImagePath = imgPath });
                                }

                            }
                        }
                        catch (Exception ex)
                        {
                            log.Error($"image {image.FileName} failed to upload for user {_serviceHelper.GetCurrentUserEmail()}");
                            
                        }
                    }
                    _unitOfWork.Commit();
                }
                return imgList;
                ////////////////////////////////////////
                //var images = model.AdImages?.ToList();
                //if(images != null && images.Count > 0)
                //{
                //    foreach(var i in images)
                //    {
                //        i.Ad = savedAd; i.AdId = savedAd?.Id;
                //        await AddAdImage(i,username);
                //    }
                //}
            }
            catch (Exception ex)
            {
                _unitOfWork.Rollback();
                log.Error($"{ex.Message} :: {MethodBase.GetCurrentMethod().Name} :: {ex.StackTrace} ");
                return null;
            }
        }
        public async Task<IEnumerable<CartItemDTO>> GetCartItems(string username)
        {
            try
            {
                #region validation
                if (string.IsNullOrWhiteSpace(username))
                {
                    throw await _serviceHelper.GetExceptionAsync("Please input a username!");
                }
                var user = await _userSvc.FindFirstAsync(x => x.UserName == username && x.IsDeleted == false);
                if (user == null)
                {
                    throw await _serviceHelper.GetExceptionAsync("Unauthorized access! Please login");
                }
                #endregion
                var cartItems = CartItemRepository.GetAll().Where(uc => uc.UserId == user.Id && uc.IsDeleted == false).ToDTO();
                return cartItems;
            }
            catch (Exception ex)
            {
                log.Error($"{ex.Message} :: {MethodBase.GetCurrentMethod().Name} :: {ex.StackTrace} ");
                return null;
            }
        }
        public async Task<bool> SaveRequestQuote(RequestQuote model, string username)
        {
            try
            {
                RequestQuote request = new RequestQuote();
                #region validation
                if (string.IsNullOrWhiteSpace(username))
                {
                    throw await _serviceHelper.GetExceptionAsync("Please input a username!");
                }
                var user = await _userSvc.FindFirstAsync(x => x.UserName == username && x.IsDeleted == false);
                if (user == null)
                {
                    throw await _serviceHelper.GetExceptionAsync("Unauthorized access! Please login");
                }
                #endregion

                #region format and save data
                request = model;

                //request.Requester = user;
                request.RequesterId = user.Id;
                //basic properties
                request.CreationTime = DateTime.Now;
                request.CreatorUserId = user.Id;
                request.IsDeleted = false;
                request.LastModificationTime = DateTime.Now;
                request.LastModifierUserId = user.Id;
                request.DeleterUserId = null;
                request.DeletionTime = null;
                request.Id = 0;

                _unitOfWork.BeginTransaction();
                RequestQuoteRepository.Insert(request);
                _unitOfWork.Commit();
                #endregion
                return true;
            }
            catch (Exception ex)
            {
                log.Error($"{ex.Message} :: {MethodBase.GetCurrentMethod().Name} :: {ex.StackTrace} ");
                return false;
                //throw ex;
            }
        }
        
        public async Task<IEnumerable<AdDTO>> TopAvailable(int count = 8, bool allowAnonymous = false)
        {
            try
            {
                return adRepository?.GetAll()?.Where(a => !a.IsDeleted).Take(count)?.ToList()?.ToDTO();
            }
            catch (Exception ex)
            {
                log.Error($"{ex.Message} :: {MethodBase.GetCurrentMethod().Name} :: {ex.StackTrace} ");
                //return false;
                throw ex;
            }
        }
        public async Task<IEnumerable<AdDTO>> SimilarAd( long subcategoryId, int count = 8, bool allowAnonymous = false)
        {
            try
            {
                return adRepository?.GetAll().Where(a => a.SubcategoryId == subcategoryId && !a.IsDeleted)?.Take(count)?.ToList()?.ToDTO();
            }
            catch (Exception ex)
            {
                log.Error($"{ex.Message} :: {MethodBase.GetCurrentMethod().Name} :: {ex.StackTrace} ");
                //return false;
                throw ex;
            }
        }
        //public async Task<string> SendNotification(List<string> clientToken, string title, string body)
        //{
        //    var registrationTokens = clientToken;
        //    var message = new MulticastMessage()
        //    {
        //        Tokens = registrationTokens,
        //        Data = new Dictionary<string, string>()
        //         {
        //             {"title", title},
        //             {"body", body},
        //         },
        //    };
        //    var response = await FirebaseMessaging.DefaultInstance.SendMulticastAsync(message).ConfigureAwait(true);
        //    return "";
        //}
        public async Task<bool> AddTransaction(Transaction model, string username)
        {
            try
            {
                #region validation
                if (string.IsNullOrWhiteSpace(username))
                {
                    throw await _serviceHelper.GetExceptionAsync("Please input a username!");
                }
                var user = await _userSvc.FindFirstAsync(x => x.UserName == username && x.IsDeleted == false);
                if (user == null)
                {
                    throw await _serviceHelper.GetExceptionAsync("Unauthorized access! Please login");
                }
                #endregion
                _unitOfWork.BeginTransaction();
                //basic properties
                model.CreationTime = DateTime.Now;
                model.CreatorUserId = user.Id;
                model.IsDeleted = false;
                model.LastModificationTime = DateTime.Now;
                model.LastModifierUserId = user.Id;
                model.DeleterUserId = null;
                model.DeletionTime = null;
                model.Id = 0;
                TransactionRepository.Insert(model);
                _unitOfWork.Commit();

                return true;
            }
            catch (Exception ex)
            {
                log.Error($"{ex.Message} :: {MethodBase.GetCurrentMethod().Name} :: {ex.StackTrace} ");
                //return false;
                throw ex;
            }
        }
        public async Task<IEnumerable<Transaction>> GetTransactions(string username, int page = 1, int size = 25)
        {
            try
            {
                return TransactionRepository.GetAll().Skip((page - 1) * size).Take(size);
            }
            catch (Exception ex)
            {
                log.Error($"{ex.Message} :: {MethodBase.GetCurrentMethod().Name} :: {ex.StackTrace} ");
                //return false;
                throw ex;
            }
        }
        public async Task<Transaction> GetTransactionById(long Id, string username)
        {
            try
            {
                return TransactionRepository.Get(Id);
            }
            catch (Exception ex)
            {
                log.Error($"{ex.Message} :: {MethodBase.GetCurrentMethod().Name} :: {ex.StackTrace} ");
                //return false;
                throw ex;
            }
        }
       
        public async Task<IEnumerable<User>> GetUsers(string username, int page = 1, int size = 25)
        {
            try
            {
                return UserRepository.GetAll().Where(x => !x.IsDeleted).Skip((page - 1) * size).Take(size);
            }
            catch (Exception ex)
            {
                log.Error($"{ex.Message} :: {MethodBase.GetCurrentMethod().Name} :: {ex.StackTrace} ");
                //return false;
                throw ex;
            }
        }
        public async Task<User> GetUserById(long Id, string username)
        {
            try
            {
                return UserRepository.FirstOrDefault(x => x.Id == Id && !x.IsDeleted);
            }
            catch (Exception ex)
            {
                log.Error($"{ex.Message} :: {MethodBase.GetCurrentMethod().Name} :: {ex.StackTrace} ");
                //return false;
                throw ex;
            }
        }
        public async Task<bool> DeletetUserById(long Id, string username)
        {
            try
            {
                var user = UserRepository.Get(Id);
                if(user == null)
                {
                    return false;
                }
                _unitOfWork.BeginTransaction();
                _userRepo.DeleteAsync(user);
                _unitOfWork.Commit();

                return true;
            }
            catch (Exception ex)
            {
                log.Error($"{ex.Message} :: {MethodBase.GetCurrentMethod().Name} :: {ex.StackTrace} ");
                //return false;
                throw ex;
            }
        }
        public async Task<bool> AddSubscriptionPackage(SubscriptionPackage model, string  username)
        {
            try
            {
                _unitOfWork.BeginTransaction();
                SubscriptionPackageRepository.Insert(model);
                _unitOfWork.Commit();

                return true;
            }
            catch (Exception ex)
            {
                log.Error($"{ex.Message} :: {MethodBase.GetCurrentMethod().Name} :: {ex.StackTrace} ");
                //return false;
                throw ex;
            }
        }
        public async Task<IEnumerable<SubscriptionPackage>> GetSubscriptionPackages(string username, int page = 1, int size = 25)
        {
            try
            {
                var result = SubscriptionPackageRepository.GetAll().Where(x => x.Id > 0).Skip((page - 1) * size).Take(size);
                return result;
            }
            catch (Exception ex)
            {
                log.Error($"{ex.Message} :: {MethodBase.GetCurrentMethod().Name} :: {ex.StackTrace} ");
                //return false;
                throw ex;
            }
        }
        public async Task<SubscriptionPackage> GetSubscriptionPackageById(long Id, string username)
        {
            try
            {
                return SubscriptionPackageRepository.FirstOrDefault(x => x.Id == Id && !x.IsDeleted);
            }
            catch (Exception ex)
            {
                log.Error($"{ex.Message} :: {MethodBase.GetCurrentMethod().Name} :: {ex.StackTrace} ");
                //return false;
                throw ex;
            }
        }
        public async Task<bool> UpdateSubscriptionPackage(SubscriptionPackage model, string username)
        {
            try
            {
                _unitOfWork.BeginTransaction();
                SubscriptionPackageRepository.Update(model);
                _unitOfWork.Commit();

                return true;
            }
            catch (Exception ex)
            {
                log.Error($"{ex.Message} :: {MethodBase.GetCurrentMethod().Name} :: {ex.StackTrace} ");
                //return false;
                throw ex;
            }
        }

        public async Task<bool> DeletetSubscriptionPackageById(long Id, string username)
        {
            try
            {
                var package = SubscriptionPackageRepository.FirstOrDefault(x => x.Id == Id && !x.IsDeleted);
                if (package == null)
                {
                    return false;
                }
                _unitOfWork.BeginTransaction();
                SubscriptionPackageRepository.DeleteAsync(package);
                _unitOfWork.Commit();

                return true;
            }
            catch (Exception ex)
            {
                log.Error($"{ex.Message} :: {MethodBase.GetCurrentMethod().Name} :: {ex.StackTrace} ");
                //return false;
                throw ex;
            }
        }
        public async Task<bool> AddNewsletterSubscriber(NewsletterSubscriber model, string username)
        {
            try
            {
                #region validate credential

                //check that the model carries data
                if (model == null)
                {
                    throw await _serviceHelper.GetExceptionAsync("no input provided!");
                }
                //check for non-empty username 
                if (string.IsNullOrWhiteSpace(username))
                {
                    throw await _serviceHelper.GetExceptionAsync("Please login and retry");
                }

                //check that the user exist
                var user = await _userSvc.FindFirstAsync(x => x.UserName == username);
                if (user == null)
                {
                    throw await _serviceHelper.GetExceptionAsync("User does not exist");
                }

                //check that the username is a valid email ( the password would be validate by the Identity builder)
                if (!Regex.IsMatch(username, @"^[^@\s]+@[^@\s]+\.[^@\s]+$", RegexOptions.IgnoreCase))
                {
                    throw await _serviceHelper.GetExceptionAsync("The username isn't a valid email");
                }
                #endregion
                _unitOfWork.BeginTransaction();
                //basic properties
                model.CreationTime = DateTime.Now;
                model.CreatorUserId = user.Id;
                model.IsDeleted = false;
                model.LastModificationTime = DateTime.Now;
                model.LastModifierUserId = user.Id;
                model.DeleterUserId = null;
                model.DeletionTime = null;
                model.Id = 0;
                NewsletterSubscriberRepository.InsertAsync(model);
                _unitOfWork.Commit();

                return true;
            }
            catch (Exception ex)
            {
                log.Error($"{ex.Message} :: {MethodBase.GetCurrentMethod().Name} :: {ex.StackTrace} ");
                //return false;
                throw ex;
            }
        }
        public async Task<IEnumerable<NewsletterSubscriber>> GetNewsletterSubscriber(string username, int page = 1, int size = 25)
        {
            try
            {
                return NewsletterSubscriberRepository.GetAll().Where(n => !n.IsDeleted).Skip((page - 1) * size).Take(size);
            }
            catch (Exception ex)
            {
                log.Error($"{ex.Message} :: {MethodBase.GetCurrentMethod().Name} :: {ex.StackTrace} ");
                //return false;
                throw ex;
            }
        }
        public async Task<NewsletterSubscriber> GetNewsletterSubscriberById(long Id, string username)
        {
            try
            {
                return NewsletterSubscriberRepository.FirstOrDefault(n => n.Id == Id && !n.IsDeleted);
            }
            catch (Exception ex)
            {
                log.Error($"{ex.Message} :: {MethodBase.GetCurrentMethod().Name} :: {ex.StackTrace} ");
                //return false;
                throw ex;
            }
        }
        public async Task<bool> UpdateNewsletterSubscriber(NewsletterSubscriber model, string username)
        {
            try
            {
                _unitOfWork.BeginTransaction();
                NewsletterSubscriberRepository.Update(model);
                _unitOfWork.Commit();

                return true;
            }
            catch (Exception ex)
            {
                log.Error($"{ex.Message} :: {MethodBase.GetCurrentMethod().Name} :: {ex.StackTrace} ");
                //return false;
                throw ex;
            }
        }

        public async Task<bool> DeleteNewsletterSubscriber(long id, string username)
        {
            try
            {
                var model = NewsletterSubscriberRepository.FirstOrDefault(n => n.Id == id && !n.IsDeleted);
                if(model == null)
                {
                    return false;
                }
                _unitOfWork.BeginTransaction();
                NewsletterSubscriberRepository.Delete(model);
                _unitOfWork.Commit();

                return true;
            }
            catch (Exception ex)
            {
                log.Error($"{ex.Message} :: {MethodBase.GetCurrentMethod().Name} :: {ex.StackTrace} ");
                //return false;
                throw ex;
            }
        }

        public async Task<bool> ChangeUserType(UserType userType, long clientId, string username)
        {
            try
            {
                #region validate
                //check that the user exist
                var user = await _userSvc.FindFirstAsync(x => x.UserName == username);
                if (user == null)
                {
                    throw await _serviceHelper.GetExceptionAsync("User does not exist");
                }
                //check that the person is an administrator
                if (user.UserType != UserType.Administrator)
                {
                    throw new Exception("you are not authorized to add user");
                }

                //check that  the client exist
                var client = _userRepo.FirstOrDefault(x => x.Id == clientId);
                if (client == null)
                {
                    throw await _serviceHelper.GetExceptionAsync("Client does not exist");
                }
                #endregion

                client.UserType = userType;
                _unitOfWork.BeginTransaction();
                _userRepo.Update(client);
                _unitOfWork.Commit();

                return true;
            }
            catch (Exception ex)
            {
                log.Error($"{ex.Message} :: {MethodBase.GetCurrentMethod().Name} :: {ex.StackTrace} ");
                //return false;
                throw ex;
            }
        }
        public async Task<Ad> GetAd(long Id, string username, bool allowAnonymous = false)
        {
            try
            {
                
                #region validate
                //check that the user exist
                User loggedInUser = null;
                if (!allowAnonymous)
                {
                    loggedInUser = await _userSvc.FindFirstAsync(x => x.UserName == username);
                    if (loggedInUser == null)
                    {
                        throw await _serviceHelper.GetExceptionAsync("User does not exist");
                    }
                }
                var loggedInUserId = loggedInUser?.Id;
                #endregion
                var ad = adRepository.FirstOrDefault(x => x.Id == Id);
                if(ad == null)
                {
                    throw new Exception("Invalid Ad Id");
                }
                //you can now add other properties
                ad.AdImages = _AdImageRepo.GetAll().Where(x => x.AdId == Id);
                ad.AdFeedback = adFeedbackRepository.GetAll().Where(x => x.AdId == Id);
                ad.AdPropertyValue = _adPropertyValueRepo.GetAll().Where(x => x.AdId == Id);
                //ad.Rank = 0;
                var rating = adFeedbackRepository.GetAll().DefaultIfEmpty().Where(a => a.Id == Id && !a.IsDeleted && (int)(a.Rating??0) >= 1 && (int)(a.Rating??0) <= 5).DefaultIfEmpty().Average(sf => (int)(sf.Rating ?? 0));
                ad.Rating = rating;//adFeedbackRepository.GetAll().Where(x => x.AdId == Id && (int)(x.Rating??0) >= 1 && (int)(x.Rating ?? 0) <= 5).Average(y => (int)y.Rating);
                ad.Likes = adFeedbackRepository.GetAll().DefaultIfEmpty().Where(a => ad.Id == a.Id && !a.IsDeleted && (a.Like ?? false)).DefaultIfEmpty().Count();
                ad.Reviews = adFeedbackRepository.GetAll().DefaultIfEmpty().Where(a => ad.Id == a.Id && !a.IsDeleted && !string.IsNullOrWhiteSpace(a.Review)).DefaultIfEmpty().Count();
                if (!allowAnonymous)
                {
                    ad.InUserCart = CartItemRepository.GetAll().DefaultIfEmpty().Any(c => c.UserId == loggedInUserId && c.AdId == ad.Id && c.IsDeleted == false);
                }
                ad.User = UserRepository.FirstOrDefault(u => u.Id == ad.UserId);//owner
                ad.Subcategory = _subcategoryRepo.FirstOrDefault(s => s.Id == ad.SubcategoryId);
                return ad;
            }
            catch (Exception ex)
            {
                log.Error($"{ex.Message} :: {MethodBase.GetCurrentMethod().Name} :: {ex.StackTrace} ");
                //return false;
                throw ex;
            }
        }
        public async Task<IEnumerable<Ad>> GetAdBulk(long[] Ids, string username, bool allowanonymous = false)
        {
            try
            {
                #region con-current
                //username = "adegunwad@accessbankplc.com"; 
                //ConcurrentBag<Ad> ads = new ConcurrentBag<Ad>();
                //Parallel.ForEach(Ids,(id) =>
                //{
                //    //var ad = GetAd(id, username).Result;
                //    var ad = GetAd(id, username).GetAwaiter().GetResult();
                //    ads.Add(ad);
                //});
                #endregion

                List<Ad> ads = new List<Ad>();
                foreach (var id in Ids)
                {
                    try
                    {
                        var ad = await GetAd(id, username, allowanonymous);
                        ads.Add(ad);
                    }
                    catch (Exception ex)
                    {
                        log.Error($"{ex.Message} :: {MethodBase.GetCurrentMethod().Name} :: {ex.StackTrace} ");
                    }
                }

                return ads;
            }
            catch (Exception ex)
            {
                log.Error($"{ex.Message} :: {MethodBase.GetCurrentMethod().Name} :: {ex.StackTrace} ");
                //return false;
                throw ex;
            }
        }
        //public async Task<bool> AddLocalGovernmentAreaOnce(List<LocalGovernmentArea> model)
        //{
        //    try
        //    {
        //        var data = LocalGovernmentAreaRepository.FirstOrDefault(x => true);
        //        if(data!= null)
        //        {
        //            throw new Exception("Already loaded!!");
        //        }
        //        _unitOfWork.BeginTransaction();
        //        foreach(var row in model)
        //        {
        //            var line = new LocalGovernmentArea {
        //                SerialNumber = row.SerialNumber,
        //                Country = row.Country,
        //                State = row.State,
        //                LGA = row.LGA,
        //                SenDistrict = row.SenDistrict,
        //                SenDistrictCode = row.SenDistrictCode,
        //                Shape_Length = row.Shape_Length,
        //                Shape_Area = row.Shape_Area,
        //                //basic properties
        //                CreationTime = DateTime.Now,
        //                CreatorUserId = null,
        //                IsDeleted = false,
        //                LastModificationTime = DateTime.Now,
        //                LastModifierUserId = null,
        //                DeleterUserId = null,
        //                DeletionTime = null,
        //                Id = 0,
        //            };
        //            LocalGovernmentAreaRepository.Insert(line);
        //        }
        //        _unitOfWork.Commit();
        //        return true;
        //    }
        //    catch (Exception ex)
        //    {
        //        log.Error($"{ex.Message} :: {MethodBase.GetCurrentMethod().Name} :: {ex.StackTrace} ");
        //        //return false;
        //        throw ex;
        //    }
        //}

        public async Task<IEnumerable<State>> GetStates()
        {
            try
            {
                List<State> states = new List<State>();
                states = (from s in LocalGovernmentAreaRepository.GetAll()
                           select new State { 
                                Country = s.Country,
                                Name = s.State
                           }).DistinctBy(s => s.Name).ToList();
                return states;
            }
            catch (Exception ex)
            {
                log.Error($"{ex.Message} :: {MethodBase.GetCurrentMethod().Name} :: {ex.StackTrace} ");
                //return false;
                throw ex;
            }
        }
        public async Task<IEnumerable<LGAData>> GetLGAs()
        {
            try
            {
                List<LGAData> lgas = new List<LGAData>();
                lgas = (from l in LocalGovernmentAreaRepository.GetAll()
                       select new LGAData { 
                            Country = l.Country,
                            Id = l.Id,
                            LGA = l.LGA,
                            SenateDistrict = l.SenDistrict,
                            SenateDistrictCode = l.SenDistrictCode,
                            State = l.State,
                       }).ToList();
                return lgas;
            }
            catch (Exception ex)
            {
                log.Error($"{ex.Message} :: {MethodBase.GetCurrentMethod().Name} :: {ex.StackTrace} ");
                //return false;
                throw ex;
            }
        }

        public async Task<IEnumerable<Message>> GetMessagesOLD(long otherPersonId, string username)
        {
            try
            {
                #region validate
                User loggedInUser = _userRepo.FirstOrDefault(a => a.UserName == username && !a.IsDeleted);
                if(loggedInUser == null)
                {
                    throw new Exception("please login and try again");
                }
                User otherPerson = _userRepo.FirstOrDefault(a => a.Id == otherPersonId && !a.IsDeleted);
                if (otherPerson == null)
                {
                    throw new Exception("can't find the details of the other person");
                }
                #endregion

                List<Message> messages = new List<Message>();
                messages = await _applicationDbContext.Messages.Where(a => (a.SenderId == loggedInUser.Id && a.RecipientId == otherPerson.Id) || (a.SenderId == otherPerson.Id && a.RecipientId == loggedInUser.Id)).ToListAsync();
                return messages;
            }
            catch (Exception ex)
            {
                log.Error($"{ex.Message} :: {MethodBase.GetCurrentMethod().Name} :: {ex.StackTrace} ");
                //return false;
                throw ex;
            }
        }

        public async Task<IEnumerable<GetMessagesResponse>> GetMessages(string username)
        {
            try
            {
                #region validate
                User loggedInUser = _userRepo.FirstOrDefault(a => a.UserName == username && !a.IsDeleted);
                if (loggedInUser == null)
                {
                    throw new Exception("please login and try again");
                }
                #endregion
                loggedInUser.Id = 10042;
                var query = (from m in _applicationDbContext.Messages
                             join s in _applicationDbContext.Users on m.SenderId equals s.Id
                             join r in _applicationDbContext.Users on m.SenderId equals r.Id

                             where m.SenderId == loggedInUser.Id || m.RecipientId == loggedInUser.Id
                             select new GetMessagesResponse
                             {
                                 MessageId = m.Id,

                                 SenderId = s.Id,
                                 SenderFirstName = s.FirstName,
                                 SenderLastName = s.LastName,
                                 SenderImagePath = s.ImagePath,
                                 SenderUserName = s.UserName,

                                 RecipientId = r.Id,
                                 RecipientFirstName = r.FirstName,
                                 RecipientLastName = r.LastName,
                                 RecipientImagePath = r.ImagePath,
                                 RecipientUserName = r.UserName,

                                 Text = m.Text,
                                 When = m.When,
                                 
                             }).DistinctBy(a => a.MessageId);
                return query.ToList();
            }
            catch (Exception ex)
            {
                log.Error($"{ex.Message} :: username {username} :: {MethodBase.GetCurrentMethod().Name} :: {ex.StackTrace} ");
                //return false;
                throw ex;
            }
        }
        public async Task<IEnumerable<GetNotificationResponse>> GetNotifications(string username)
        {
            try
            {
                #region validate
                User loggedInUser = _userRepo.FirstOrDefault(a => a.UserName == username && !a.IsDeleted);
                if (loggedInUser == null)
                {
                    throw new Exception("please login and try again");
                }
                #endregion
                var result = (from n in _applicationDbContext.Notification
                              join r in _applicationDbContext.Users on n.RecipientId equals r.Id

                              where !n.IsDeleted && n.RecipientId == loggedInUser.Id
                              select new GetNotificationResponse
                              {
                                  Delivered = n.Delivered,
                                  Id = n.Id,
                                  IsBroadCast = n.IsBroadCast,
                                  Message = n.Message,
                                  NotificationType = n.NotificationType,
                                  RecipientFirstName = r.FirstName,
                                  RecipientId = r.Id,
                                  RecipientImagePath = r.ImagePath,
                                  RecipientLastName = r.LastName,
                                  RecipientUserName = r.UserName,
                                  Title = n.Title,
                                  
                              }).DistinctBy(a => a.Id);

                return result.ToList();
            }
            catch (Exception ex)
            {
                log.Error($"{ex.Message} :: username {username} :: {MethodBase.GetCurrentMethod().Name} :: {ex.StackTrace} ");
                //return false;
                throw ex;
            }
        }
        #region comments
        //List<Ad> result = new List<Ad>();
        //var ads = adRepository.GetAllIncluding(a => a.AdImages).Where(x => !x.IsDeleted);
        // var images = _AdImageRepo.GetAll().
        //var ads = adRepository.GetAll().Include("AdImages");

        //// var newAds = ads.Join(images, x => x.Id, y => y.AdId, (x, y) => x).ToDTO();

        //var r = ads.Where(a => !a.IsDeleted);
        //result = r.ToList();
        //var _1 = _adPropertyValueRepo.GetAll().Include(u => u.AdProperty).DefaultIfEmpty().Where(a => a.AdId == 46 && a.IsDeleted == false).DefaultIfEmpty().ToList();
        //var _2 = (from a in _adPropertyValueRepo.GetAll()
        //         join ap in _adPropertyRepo.GetAll() on a.AdPropertyId equals ap.Id

        //         where a.AdId == 46 && !a.IsDeleted
        //         select new AdPropertyValue
        //         {
        //             AdId = 46,
        //             Id = a.Id,
        //             AdPropertyId = a.AdPropertyId,
        //             Value = a.Value,
        //             AdProperty = new AdProperty { 
        //                Name = ap.Name,
        //                Id = ap.Id
        //             },
        //         }).ToList();
        //var _3 = "";
        //return default;
        //try
        //{
        //    var replacement = new System.Collections.Specialized.StringDictionary
        //    {
        //        ["FullName"] = data.FullName,
        //        ["Position"] = data.JobTitle,
        //        ["ContactPhoneNumber"] = data.ContactPhoneNumber,
        //        ["ContactEmail"] = data.ContactEmail,
        //        ["Address"] = data.Address,
        //        ["Skills"] = data.Skills,
        //        ["ExpectedSalary"] = data.ExpectedSalary,
        //        ["Age"] = data.Age.ToString(),
        //        ["Certification"] = data.Certification,
        //        ["HighestLevelOfEducation"] = data.HighestLevelOfEducation,
        //        ["Gender"] = data.Gender,

        //    };

        //    var mail = new Mail(_smtpsettings.UserName, "EkiHire.com: Account Verification Code",
        //        user.Email
        //        //"damee1993@gmail.com"
        //        )
        //    {
        //        BodyIsFile = true,
        //        BodyPath = Path.Combine(_hostingEnvironment.ContentRootPath, CoreConstants.Url.ActivationCodeEmail),
        //        SenderDisplayName = _smtpsettings.SenderDisplayName,

        //    };

        //    await _mailSvc.SendMailAsync(mail, replacement);
        //}
        //catch (Exception ex)
        //{

        //}

        //public async Task<IEnumerable<Ad>> GetActiveAds()
        //{
        //    try
        //    {
        //        List<Ad> result = new List<Ad>();
        //        result = await adRepository.GetAll().Where(a => a.IsActive == true && a.IsDeleted == false).ToListAsync();
        //        return result;
        //    }
        //    catch (Exception ex)
        //    {
        //        log.Error($"{ex.Message} :: {MethodBase.GetCurrentMethod().Name} :: {ex.StackTrace} ");
        //        return null;
        //    }
        //}
        //private double TrendingRank(Ad model, out Ad ad)
        //{
        //    try
        //    {
        //        var reviews = 0; var likes = 0; var searches = 0; var daysSincePosts = 0;
        //        reviews = adFeedbackRepository.GetAll().Where(a => a.AdId == model.Id && !string.IsNullOrWhiteSpace(a.Review)).Count();
        //        likes = adFeedbackRepository.GetAll().Where(a => a.AdId == model.Id && a.Like).Count();
        //        searches = SearchRepository.GetAll().Where(s => s.AdId == model.Id).Count();
        //        daysSincePosts = (DateTime.Now.Date - model.CreationTime.Date).Days;

        //        double rank = ((reviews * appConfig.ReviewsWeight) + (likes * appConfig.LikesWeight) + (searches * appConfig.SearchWeight)) / (daysSincePosts * appConfig.DaysSincePostWeight);

        //        //test
        //        rank = 5;

        //        model.Rank = rank;
        //        ad = model;
        //        return rank;
        //    }
        //    catch (Exception ex)
        //    {
        //        log.Error($"{ex.Message} :: {MethodBase.GetCurrentMethod().Name} :: {ex.StackTrace} ");
        //        ad = model;
        //        return 0;
        //    }
        //}
        //public async Task<IEnumerable<AdFeedback>> AdFeedbackForUser(string username, long adId  = 0)
        //{
        //    try
        //    {
        //        #region validation
        //        if (string.IsNullOrWhiteSpace(username))
        //        {
        //            throw await _serviceHelper.GetExceptionAsync("Please input a username!");
        //        }
        //        var user = await _userSvc.FindFirstAsync(x => x.UserName == username);
        //        if (user == null)
        //        {
        //            throw await _serviceHelper.GetExceptionAsync("Unauthorized access! Please login");
        //        }
        //        var ad = adRepository.Get(adId);
        //        if (ad == null)
        //        {
        //            throw await _serviceHelper.GetExceptionAsync("Cannot find ad!");
        //        }
        //        #endregion
        //        var result = await adFeedbackRepository.GetAll().Where(a => adRepository.Get(a.AdId).User.UserName == username
        //        && (a.AdId == adId || adId == 0)
        //        ).ToListAsync();
        //        return result;
        //    }
        //    catch (Exception ex)
        //    {
        //        log.Error($"A error occured while trying to get reviews - error - {ex.Message} - stackTraack - {ex.StackTrace} :: {MethodBase.GetCurrentMethod().Name}", ex);
        //        return null;
        //    }
        //}
        //private long? getUserFromAd(long? adid)
        //{
        //    var resp = adRepository.FirstOrDefault(x => x.Id == adid && x.IsDeleted == false).UserId;
        //    return resp;
        //}
        //public async Task<bool> PostAds()
        //{
        //    try
        //    {

        //    }
        //    catch (Exception ex)
        //    {

        //    }
        //}
        //public async Task<AdDTO> GetAd(long Id)
        //{
        //    try
        //    {
        //        //var ad = await adRepository.GetAll().FirstOrDefaultAsync(a => a.Id == Id);
        //        //return ad;
        //        return default;
        //    }
        //    catch (Exception ex)
        //    {
        //        log.Error($"{ex.Message} :: {MethodBase.GetCurrentMethod().Name} :: {ex.StackTrace} ");
        //        return null;
        //    }
        //}
        //SendAccountCredentials(user, model.Password);
        //the email needs to be worked on and be further simplified in it's process flow
        //#region send emnail
        //try
        //{
        //    //first file
        //    if (File.Exists(Path.Combine(_hostingEnvironment.ContentRootPath, CoreConstants.Url.ActivationCodeEmail)))
        //    {
        //        var fileString = File.ReadAllText(Path.Combine(_hostingEnvironment.ContentRootPath, CoreConstants.Url.ActivationCodeEmail));
        //        if (!string.IsNullOrWhiteSpace(fileString))
        //        {
        //            //fileString = fileString.Replace("{{FirstName}}", user.FirstName);
        //            fileString = fileString.Replace("{{ActivationCode}}", user.AccountConfirmationCode);

        //            _mailSvc.SendMailAsync(user.UserName, "EkiHire.com: Account Verification Code", fileString);
        //        }
        //    }
        //}
        //catch (Exception ex)
        //{

        //    //throw;
        //}

        //#endregion
        //public async Task<IEnumerable<AdDTO>> GetAdOld(AdFilter model, string username, bool allowanonymous = false)
        //{
        //    try
        //    {
        //        List<AdDTO> result = new List<AdDTO>();
        //        #region validation
        //        if (string.IsNullOrWhiteSpace(username) && allowanonymous == false)
        //        {
        //            throw await _serviceHelper.GetExceptionAsync("Please input a username!");
        //        }
        //        var user = await _userSvc.FindFirstAsync(x => x.UserName == username && x.IsDeleted == false);
        //        if (user == null && allowanonymous == false)
        //        {
        //            throw await _serviceHelper.GetExceptionAsync("Unauthorized access! Please login");
        //        }
        //        if (model == null)
        //        {
        //            throw await _serviceHelper.GetExceptionAsync("Invalid search entry!");
        //        }
        //        #endregion
        //        #region filter based on the search entry
        //        var ads = adRepository.GetAll().Where(x => x.IsDeleted == false);
        //        if (model.AdId != null && model.AdId != 0)
        //        {
        //            ads = ads.Where(a => a.Id == model.AdId);
        //        }
        //        if (!string.IsNullOrWhiteSpace(model.SearchText))
        //        {
        //            ads = ads.Where(a => a.Name.Contains(model.SearchText));
        //        }
        //        if (model.SubcategoryId != null && model.SubcategoryId != 0)
        //        {
        //            ads = ads.Where(a => a.SubcategoryId == model.SubcategoryId);
        //        }
        //        if (model.CategoryId != null && model.CategoryId != 0)
        //        {
        //            ads = from a in ads
        //                  join s in _subcategoryRepo.GetAll() on a.SubcategoryId equals s.Id
        //                  join c  in CategoryRepository.GetAll() on s.CategoryId equals c.Id
        //                  where c.Id == model.CategoryId select a;
        //        }
        //        if (model.min_amount != null)
        //        {
        //            ads = ads.Where(a => a.Amount >= model.min_amount);
        //        }
        //        if (model.max_amount != null)
        //        {
        //            ads = ads.Where(a => a.Amount <= model.max_amount);
        //        }
        //        //if(model.StateId != null && model.StateId != 0)
        //        //{
        //        //    ads = ads.Where(a => a.StateId == model.StateId);
        //        //}
        //        //if (model.LGAId != null && model.LGAId != 0)
        //        //{
        //        //    ads = ads.Where(a => a.LGAId == model.LGAId);
        //        //}
        //        if (!string.IsNullOrWhiteSpace(model.Address))
        //        {
        //            ads = ads.Where(a => a.Address.Contains(model.Address));
        //        }
        //        if (model.AdClass != null)
        //        {
        //            ads = ads.Where(a => a.AdClass == model.AdClass);
        //        }
        //        if (!string.IsNullOrWhiteSpace(model.PhoneNumber))
        //        {
        //            ads = ads.Where(a => a.PhoneNumber.Contains(model.PhoneNumber));
        //        }
        //        //if (!string.IsNullOrWhiteSpace(model.Location))
        //        //{
        //        //    ads = ads.Where(a => a.Location.Contains(model.Location));
        //        //}
        //        if (!string.IsNullOrWhiteSpace(model.AdReference))
        //        {
        //            ads = ads.Where(a => a.AdReference.Contains(model.AdReference));
        //        }
        //        if (!string.IsNullOrWhiteSpace(model.Description))
        //        {
        //            ads = ads.Where(a => a.Description.Contains(model.Description));
        //        }
        //        if (model.AdsStatus != null)
        //        {
        //            ads = ads.Where(a => a.AdsStatus == model.AdsStatus);
        //        }
        //        if (model.UserId != null && model.UserId != 0)
        //        {
        //            ads = ads.Where(a => a.UserId == model.UserId);
        //        }
        //        if (model.PropertyValuePairs != null && model.PropertyValuePairs.Count > 0)
        //        {
        //            foreach (var pv in model.PropertyValuePairs)
        //            {
        //                if(pv.PropertyId != 0)
        //                {
        //                    var PropertyValues = _adPropertyValueRepo.GetAll().Where(v => v.Value.Contains(pv.Value) && v.AdPropertyId == pv.PropertyId && v.IsDeleted == false);
        //                    ads = ads.Where(a => PropertyValues.Any(x => x.AdId == a.Id));
        //                }
        //            }
        //        }
        //        //var adk = ads.AsEnumerable().Where(a => (model.Keywords == null || Split(a.Keywords,",").Any(k => model.Keywords.Contains(k))));
        //        var r = ads.Where(a => a.IsDeleted == false).ToDTO().ToArray();
        //        //var r = r2.ToArray();

        //        #region really needs optimization
        //        for (var i = 0; i < r.Length; i++)
        //        {
        //            try
        //            {
        //                if (r[i].User == null)
        //                {
        //                    if (r[i].UserId == null || r[i].UserId == 0)
        //                    {
        //                        continue;
        //                    }
        //                    r[i].User = UserRepository.Get(r[i].UserId ?? 0);
        //                }
        //                var images = _AdImageRepo.GetAll().Where(a => a.AdId == r[i].Id && a.IsDeleted == false)?.ToDTO()?.ToList();
        //                var adPropValues = _adPropertyValueRepo.GetAll().Where(a => a.AdId == r[i].Id && a.IsDeleted == false)?.ToDTO()?.ToList();
        //                var adfeedback = adFeedbackRepository.GetAll().Where(a => a.AdId == r[i].Id && a.IsDeleted == false)?.ToDTO()?.ToList();
        //                r[i].AdImages = images;
        //                r[i].AdPropertyValue = adPropValues;
        //                r[i].AdFeedback = adfeedback;
        //                r[i].Likes = adfeedback.Where(l => l.Like ?? false).Count();

        //                var rating = adfeedback.Where(a => ((int)a.Rating) >= 1 && ((int)a.Rating) <= 5);
        //                double ratingSum = rating.Sum(x => ((int)x.Rating));
        //                double ratingCount = rating.Count();
        //                r[i].Rating = (ratingCount > 0 && ratingSum > 0) ? (double)(ratingSum / ratingCount) : 0;
        //                r[i].Reviews = adfeedback.Where(a => !string.IsNullOrWhiteSpace(a.Review)).Count();
        //                if (user != null)
        //                {
        //                    r[i].InUserCart = CartItemRepository.GetAll().Any(c => c.UserId == user.Id && c.AdId == r[i].Id && c.IsDeleted == false);
        //                }
        //            }
        //            catch (Exception ex)
        //            {
        //                log.Error($"{ex.Message} :: {MethodBase.GetCurrentMethod().Name} :: {ex.StackTrace} ");
        //            }

        //        }
        //        #endregion

        //        result = r.ToList();
        //        #endregion
        //        //save searches
        //        //_ = Task.Run<IRepository<Search>>((IRepository<Search> ssy) =>
        //        {
        //            _unitOfWork.BeginTransaction();
        //            foreach (var s in result)
        //            {
        //                try
        //                {
        //                    var sdata = new AdLookupLog
        //                    {
        //                        AdId = s.Id
        //                    };
        //                    await AdLookupLogRepository.InsertAsync(sdata);
        //                }
        //                catch (Exception ex)
        //                {
        //                }
        //            }
        //            _unitOfWork.Commit();
        //        }
        //        //);
        //        return result;
        //    }
        //    catch (Exception ex)
        //    {
        //        _unitOfWork.Rollback();
        //        log.Error($"{ex.Message} :: {MethodBase.GetCurrentMethod().Name} :: {ex.StackTrace} ");
        //        //return null;
        //        throw ex;
        //    }
        //}

        //public static List<string> Split(string str, string separator)
        //{
        //    var resp = str.Split(new[] { separator }, StringSplitOptions.None).ToList();
        //    return resp;
        //}
        //public async Task<IEnumerable<AdFeedback>> AdFeedbackByUser(string username, long adId = 0)
        //{
        //    try
        //    {
        //        #region validation
        //        if (string.IsNullOrWhiteSpace(username))
        //        {
        //            throw await _serviceHelper.GetExceptionAsync("Please input a username!");
        //        }
        //        var user = await _userSvc.FindFirstAsync(x => x.UserName == username);
        //        if (user == null)
        //        {
        //            throw await _serviceHelper.GetExceptionAsync("Unauthorized access! Please login");
        //        }
        //        var ad = adRepository.Get(adId);
        //        if (ad == null)
        //        {
        //            throw await _serviceHelper.GetExceptionAsync("Cannot find ad!");
        //        }
        //        #endregion

        //        var result = new List<AdFeedback>();

        //        result = await adFeedbackRepository.GetAll().Where(a => a.UserId == user.Id
        //        && (a.AdId == adId || adId == 0)
        //        ).ToListAsync();

        //        return result;
        //    }
        //    catch (Exception ex)
        //    {
        //        log.Error($"A error occured while trying to get reviews - error - {ex.Message} - stackTraack - {ex.StackTrace} :: {MethodBase.GetCurrentMethod().Name}",ex);
        //        return null;
        //    }
        //}
        #endregion
    }
}