﻿using System;
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

namespace EkiHire.Business.Services
{
    public interface IAdService
    {
        Task<bool> AddAd(AdDTO model, string username);
        Task<bool> CloseAd(long model, string username);
        Task<bool> EditAd(AdDTO adDto, long model, string username);
        Task<bool> PromoteAd(long model, string username);
        Task<bool> CreateItem(ItemDTO model, string username);
        Task<bool> EditItemKeywords(List<string> keywords, long ItemId, string username);
        Task<bool> GroupAdItems(long[] ItemIds, string groupname, string username);
        Task<bool> AddAdToCart(long Id, string username);
        Task<bool> RemoveAdFromCart(long Id, string username);
        Task<IEnumerable<AdDTO>> Search(SearchVM model, string username);
        //Task<IEnumerable<AdFeedback>> AdFeedbackByUser(string username, long adId = 0);
        Task<IEnumerable<AdFeedback>> AdFeedbackByUser(string username/*, long[] adIds = null*/);
        //Task<IEnumerable<AdFeedback>> AdFeedbackForUser(string username, long adId = 0);
        Task<IEnumerable<AdFeedback>> AdFeedbackForUser(string username/*, long[] adIds = null*/);
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
        Task<IEnumerable<AdDTO>> Trending(long count = 0);
        //Task<AdDTO> GetAd(long Id);
        Task<bool> UpdateAdStatus(long AdId, AdsStatus adsStatus);
        Task<bool> AddAdImage(AdImageDTO model, string username);
        Task<IEnumerable<CartItemDTO>> GetCartItems(string username);
    }
    public class AdService: IAdService
    {
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
        private readonly IRepository<Search> SearchRepository;
        public AdService(IRepository<Ad> adRepository, IServiceHelper _serviceHelper, IUserService _userSvc, IUnitOfWork unitOfWork, IRepository<Item> itemRepository, IRepository<CartItem> CartItemRepository, IRepository<AdFeedback> adFeedbackRepository, IRepository<Follow> followRepository, IRepository<Subcategory> _subcategoryRepo, IRepository<Keyword> _keywordRepo, IRepository<AdProperty> _adPropertyRepo, IRepository<AdPropertyValue> _adPropertyValueRepo, IRepository<User> _userRepo,
            IRepository<AdImage> _AdImageRepo, IOptions<AppConfig> _appConfig, IRepository<Search> SearchRepository)
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
            appConfig = _appConfig.Value;
            this.SearchRepository = SearchRepository;
        }
        public async Task<bool> AddAd(AdDTO model, string username)
        {
            
            try
            {
                #region validate credential

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

                //check for validate usertype, validate the adtype if premium whether user can put premium ad

                #endregion

                #region add ad to the db
                _unitOfWork.BeginTransaction();
                Ad ad = new Ad();
                ad = model;
                //basic properties
                ad.CreationTime = DateTime.Now;
                ad.CreatorUserId = user.Id;
                ad.IsDeleted = false;
                ad.LastModificationTime = DateTime.Now;
                ad.LastModifierUserId = user.Id;
                ad.DeleterUserId = null;
                ad.DeletionTime = null;
                ad.Id = 0;

                //others
                ad.AdReference = $"EH{new Random().Next(1_000_000_000, int.MaxValue)}{new Random().Next(1_000_000_000, int.MaxValue)}";
                ad.AdsStatus = AdsStatus.INREVIEW;
                ad.IsActive = true;

                var inserted = await adRepository.InsertAsync(ad);
                _unitOfWork.Commit();
                //await _unitOfWork.SaveChangesAsync();

                var savedAd = await adRepository.GetAll().Where(a => a.AdReference == ad.AdReference && a.IsDeleted == false).FirstOrDefaultAsync();
                var adpv = model.AdPropertyValue?.ToList();
                if (adpv != null && adpv.Count > 0)
                {
                    foreach (var p in adpv)
                    {
                        p.Ad = savedAd; p.AdId = savedAd?.Id;
                        await AddOrUpdateAdPropertyValue(p, username);
                    }
                }

                var images = model.AdImages?.ToList();
                if(images != null && images.Count > 0)
                {
                    foreach(var i in images)
                    {
                        i.Ad = savedAd; i.AdId = savedAd?.Id;
                        await AddAdImage(i,username);
                    }
                }
                
                #endregion
                return true;
            }
            catch (Exception ex)
            {
                _unitOfWork.Rollback();
                log.Error($"user ({username}) could not add a new ad {ex.Message} :: {MethodBase.GetCurrentMethod().Name} :: {ex.StackTrace}");
                return false;
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
                await adRepository .UpdateAsync(ad);
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
                //check that it is the owner of the ad that is editing it
                if(user.Id != adDto.UserId)
                {
                    throw await _serviceHelper.GetExceptionAsync("you are not authorized to edit this ad!");
                }
                #endregion

                #region make the necessary edit on a property by property basis
                
                //name
                if(!string.IsNullOrWhiteSpace(adDto.Name) && !string.Equals(ad.Name, adDto.Name))
                {
                    ad.Name = adDto.Name;
                }
                //address
                if (!string.IsNullOrWhiteSpace(adDto.Address) && !string.Equals(ad.Address, adDto.Address))
                {
                    ad.Address = adDto.Address;
                }
                //amount
                if (!string.IsNullOrWhiteSpace(adDto.Amount.ToString()) && !string.Equals(ad.Amount.ToString(), adDto.Amount.ToString()) && adDto.Amount >= 0)
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
                //remove sub, cat
                //images
                if(adDto.AdImages != null && adDto.AdImages.Count() > 0)
                {
                    var imgs = _AdImageRepo.GetAll().Where(i => i.AdId == ad.Id && i.IsDeleted == false).ToList() ?? new List<AdImage>();
                    foreach (var img in adDto.AdImages)
                    {
                        if(imgs.Any(i => i.Id == img.Id))
                        {
                            //update an imaage
                            var imgData = imgs.Where(i => i.Id == img.Id).FirstOrDefault();
                            imgData.ImagePath = string.IsNullOrWhiteSpace(img.ImagePath)? imgData.ImagePath: img.ImagePath;
                            imgData.ImageString = img.ImageString;
                            _AdImageRepo.Update(imgData);
                        }
                        else
                        {
                            //insert
                            img.AdId = ad.Id;
                            _AdImageRepo.Insert(img);
                        }
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
                await itemRepository .InsertAsync(model);
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
                var user = await _userSvc.FindFirstAsync(x => x.UserName == username);
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

        public async Task<IEnumerable<AdDTO>> Search(SearchVM model, string username)
        {
            try
            {
                List<AdDTO> result = new List<AdDTO>();
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
                    throw await _serviceHelper.GetExceptionAsync("Invalid search entry!");
                }
                #endregion
                #region filter based on the search entry

                //var ad = adRepository.GetAllIncluding(a => a.Subcategory).Where(a => 
                //(a.Id == model.AdId || model.AdId == null)
                //&& (a.Name.Contains(model.SearchText) 
                ///* experimental */
                ////|| model.SearchText.Contains(a.Name) || Split(a.Name, " ").Contains(model.SearchText) || Split(model.SearchText, " ").Contains(a.Name)
                //|| string.IsNullOrWhiteSpace(model.SearchText))
                //&& (a.SubcategoryId == model.SubcategoryId || (model.SubcategoryId == null || model.SubcategoryId == 0))
                //&& (a.Subcategory.CategoryId == model.CategoryId || (model.CategoryId == null || model.CategoryId == 0))
                //).ToList();
                ////var hasAnyKeyword = Split("House", ",").Any(k => (model.Keywords).Contains(k));
                
                var ads = adRepository.GetAllIncluding(x => x.Subcategory).Where(x => x.IsDeleted == false);
                if (model.AdId != null && model.AdId != 0)
                {
                    ads = ads.Where(a => a.Id == model.AdId);
                }
                if (!string.IsNullOrWhiteSpace(model.SearchText))
                {
                    ads = ads.Where(a => a.Name.Contains(model.SearchText));
                }
                if (model.SubcategoryId != null && model.SubcategoryId != 0)
                {
                    ads = ads.Where(a => a.SubcategoryId == model.SubcategoryId);
                }
                if (model.CategoryId != null && model.CategoryId != 0)
                {
                    ads = ads.Where(a => a.Subcategory.CategoryId == model.CategoryId);
                }
                if (model.min_amount != null)
                {
                    ads = ads.Where(a => a.Amount >= model.min_amount);
                }
                if (model.max_amount != null)
                {
                    ads = ads.Where(a => a.Amount <= model.max_amount);
                }
                //if(model.StateId != null && model.StateId != 0)
                //{
                //    ads = ads.Where(a => a.StateId == model.StateId);
                //}
                //if (model.LGAId != null && model.LGAId != 0)
                //{
                //    ads = ads.Where(a => a.LGAId == model.LGAId);
                //}
                if (!string.IsNullOrWhiteSpace(model.Address))
                {
                    ads = ads.Where(a => a.Address.Contains(model.Address));
                }
                if (model.AdClass != null)
                {
                    ads = ads.Where(a => a.AdClass == model.AdClass);
                }
                if (!string.IsNullOrWhiteSpace(model.PhoneNumber))
                {
                    ads = ads.Where(a => a.PhoneNumber.Contains(model.PhoneNumber));
                }
                //if (!string.IsNullOrWhiteSpace(model.Location))
                //{
                //    ads = ads.Where(a => a.Location.Contains(model.Location));
                //}
                if (!string.IsNullOrWhiteSpace(model.AdReference))
                {
                    ads = ads.Where(a => a.AdReference.Contains(model.AdReference));
                }
                if (!string.IsNullOrWhiteSpace(model.Description))
                {
                    ads = ads.Where(a => a.Description.Contains(model.Description));
                }
                if (model.AdsStatus != null)
                {
                    ads = ads.Where(a => a.AdsStatus == model.AdsStatus);
                }
                if (model.UserId != null && model.UserId != 0)
                {
                    ads = ads.Where(a => a.UserId == model.UserId);
                }
                if (model.PropertyValuePairs != null && model.PropertyValuePairs.Count > 0)
                {
                    foreach (var pv in model.PropertyValuePairs)
                    {
                        if(pv.PropertyId != 0)
                        {
                            var PropertyValues = _adPropertyValueRepo.GetAll().Where(v => v.Value.Contains(pv.Value) && v.AdPropertyId == pv.PropertyId && v.IsDeleted == false);
                            ads = ads.Where(a => PropertyValues.Any(x => x.AdId == a.Id));
                        }
                    }
                }
                var adk = ads.AsEnumerable().Where(a => (model.Keywords == null || Split(a.Keywords,",").Any(k => model.Keywords.Contains(k))));
                var r2 = adk.Where(a => a.IsDeleted == false).ToDTO();
                var r = r2.ToArray();
                
                for(var i=0; i < r.Length; i++)
                {
                    var images = _AdImageRepo.GetAll().Where(a => a.AdId == r[i].Id && a.IsDeleted == false).ToDTO().ToList();
                    var adPropValues = _adPropertyValueRepo.GetAll().Where(a => a.AdId == r[i].Id && a.IsDeleted == false).ToDTO().ToList();
                    var adfeedback = adFeedbackRepository.GetAll().Where(a => a.AdId == r[i].Id && a.IsDeleted == false).ToDTO().ToList();
                    r[i].AdImages = images;
                    r[i].AdPropertyValue = adPropValues;
                    r[i].AdFeedback = adfeedback;
                    r[i].Likes = adfeedback.Where(l => l.Like).Count();

                    var rating = adfeedback.Where(a => ((int)a.Rating) >= 1 && ((int)a.Rating) <= 5);
                    double ratingSum = rating.Sum(x => ((int)x.Rating));
                    double ratingCount = rating.Count();
                    r[i].Rating = (ratingCount > 0 && ratingSum > 0) ? (double)(ratingSum / ratingCount) : 0;
                    r[i].Reviews = adfeedback.Where(a => string.IsNullOrWhiteSpace(a.Review)).Count();
                    r[i].InUserCart = CartItemRepository.GetAll().Any(c => c.UserId == user.Id && c.AdId == r[i].Id && c.IsDeleted == false);
                }
                result = r.ToList();
                #endregion
                //save searches
                //_ = Task.Run<IRepository<Search>>((IRepository<Search> ssy) =>
                {
                    _unitOfWork.BeginTransaction();
                    foreach (var s in result)
                    {
                        try
                        {
                            var sdata = new Search
                            {
                                AdId = s.Id
                            };
                            await SearchRepository.InsertAsync(sdata);
                        }
                        catch (Exception ex)
                        {
                        }
                    }
                    _unitOfWork.Commit();
                }
                //);
                return result;
            }
            catch (Exception ex)
            {
                _unitOfWork.Rollback();
                log.Error($"{ex.Message} :: {MethodBase.GetCurrentMethod().Name} :: {ex.StackTrace} ");
                return null;
            }
        }
        public static List<string> Split(string str, string separator)
        {
            var resp = str.Split(new[] { separator }, StringSplitOptions.None).ToList();
            return resp;
        }

        #region reviews
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
        public async Task<IEnumerable<AdFeedback>> AdFeedbackByUser(string username/*, long[] adIds = null*/)
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
                result = await adFeedbackRepository.GetAll().Where(a => a.UserId == user.Id && a.IsDeleted == false
                //&& (adIds.Contains(a.AdId) || adIds == null)
                ).ToListAsync();
                return result;
            }
            catch (Exception ex)
            {
                log.Error($"A error occured while trying to get reviews - error - {ex.Message} - stackTraack - {ex.StackTrace} :: {MethodBase.GetCurrentMethod().Name}", ex);
                return null;
            }
        }
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
        private long? getUserFromAd(long? adid)
        {
            var resp = adRepository.FirstOrDefault(x => x.Id == adid && x.IsDeleted == false).UserId;
            return resp;
        }
        public async Task<IEnumerable<AdFeedback>> AdFeedbackForUser(string username/*, long[] adIds = null*/)
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
                var result = (from af in adFeedbackRepository.GetAll()
                        join ad in adRepository.GetAll() on af.AdId equals ad.Id
                        where ad.UserId == user.Id && af.IsDeleted == false
                        select af).ToList();
                //var result = adFeedbackRepository.GetAll().AsEnumerable().Where(a => getUserFromAd(a.AdId) == user.Id
                ////&& (adIds.Contains(a.AdId) || adIds == null)
                //).ToList();
                return result;
            }
            catch (Exception ex)
            {
                log.Error($"A error occured while trying to get reviews - error - {ex.Message} - stackTraack - {ex.StackTrace} :: {MethodBase.GetCurrentMethod().Name}", ex);
                return null;
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
                result = await followRepository.GetAll().Where(f => f.Follower.UserName == username && f.IsDeleted == false).ToListAsync();
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
                var user = await _userSvc.FindFirstAsync(x => x.UserName == username);
                if (user == null)
                {
                    throw await _serviceHelper.GetExceptionAsync("Unauthorized access! Please login");
                }
                if (model == null || string.IsNullOrWhiteSpace(model.Name))
                {
                    throw await _serviceHelper.GetExceptionAsync("Invalid data!");
                }
                var subcat = _subcategoryRepo.FirstOrDefault(s => s.Id == model.SubcategoryId);
                if (subcat == null)
                {
                    throw await _serviceHelper.GetExceptionAsync("Invalid subcategory!");
                }
                var adprop = _adPropertyRepo.FirstOrDefault(a => a.Subcategory.Id == model.SubcategoryId && a.Name.ToLower() == model.Name.ToLower());
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
                var subcat = _subcategoryRepo.FirstOrDefault(s => s.Id == model.Subcategory.Id && s.IsDeleted == false);
                if (subcat == null)
                {
                    throw await _serviceHelper.GetExceptionAsync("Invalid subcategory!");
                }
                var adprop = _adPropertyRepo.FirstOrDefault(a => a.Subcategory.Id == model.Subcategory.Id && a.Name == model.Name && a.IsDeleted == false);
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
                return false;
            }
        }
        //create new ad property, update ad prop, delete,
        #endregion
        public async Task<IEnumerable<Ad>> GetActiveAds()
        {
            try
            {
                List<Ad> result = new List<Ad>();
                result = await adRepository.GetAll().Where(a => a.IsActive == true && a.IsDeleted == false).ToListAsync();
                return result;
            }
            catch (Exception ex)
            {
                log.Error($"{ex.Message} :: {MethodBase.GetCurrentMethod().Name} :: {ex.StackTrace} ");
                return null;
            }
        }
        public async Task<IEnumerable<AdDTO>> Trending(long count = 0)
        {
            try
            {
                List<AdDTO> result = new List<AdDTO>();
                //result = adRepository.GetAll().Where(a => TrendingRank(a, out a) > 0).OrderByDescending(ax => ax.Rank).Take((int)count).ToDTO().ToList();

                //_ = "inter-commission";

                //result = adRepository.GetAll().Select(model => TrendingRank(model)).AsEnumerable().OrderByDescending(ab => ab.Rank).Take((int)count).ToDTO().ToList();

                //result = (from a in adRepository.GetAll() select TrendingRank(a)).OrderByDescending(ab => ab.Rank).Take((int)count).ToDTO().ToList();
                List<Ad> ar = new List<Ad>();
                var aList = adRepository.GetAll().Where(x => x.IsDeleted == false).ToList();
                foreach (var a in aList)
                {
                    var data = TrendingRank(a);
                    ar.Add(data);
                }
                result = ar.OrderByDescending(ab => ab.Rank).Take((int)count).ToDTO().ToList();
                return result;
            }
            catch (Exception ex)
            {
                log.Error($"{ex.Message} :: {MethodBase.GetCurrentMethod().Name} :: {ex.StackTrace} ");
                return null;
            }
        }
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

        private Ad TrendingRank(Ad model)
        {
            try
            {
                var reviews = 0; var likes = 0; var searches = 0; var daysSincePosts = 0;
                reviews = adFeedbackRepository.GetAll().Where(a => a.AdId == model.Id && !string.IsNullOrWhiteSpace(a.Review) && a.IsDeleted == false).Count();
                likes = adFeedbackRepository.GetAll().Where(a => a.AdId == model.Id && a.Like && a.IsDeleted == false).Count();
                searches = SearchRepository.GetAll().Where(s => s.AdId == model.Id && s.IsDeleted == false).Count();
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

        public async Task<bool> UpdateAdStatus(long AdId, AdsStatus adsStatus)
        {
            try
            {
                var ad = await adRepository.GetAll().FirstOrDefaultAsync(a => a.Id == AdId && a.IsDeleted == false);
                if(ad == null)
                {
                    throw new Exception("Ad not found!");
                }
                _unitOfWork.BeginTransaction();
                ad.AdsStatus = adsStatus;
                adRepository.Update(ad);
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
                    ImageString = model.ImageString,
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

    }
}
//show premium ads first