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
    }
    public class AdService: IAdService
    {
        private readonly IRepository<Ad> adRepository;
        private readonly IServiceHelper _serviceHelper;
        private readonly IUserService _userSvc;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IRepository<Item> itemRepository;
        public AdService(IRepository<Ad> adRepository, IServiceHelper _serviceHelper, IUserService _userSvc, IUnitOfWork unitOfWork, IRepository<Item> itemRepository)
        {
            this.adRepository = adRepository;
            this._serviceHelper = _serviceHelper;
            this._userSvc = _userSvc;
            this._unitOfWork = unitOfWork;
            this.itemRepository = itemRepository;
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
                Ad ad = model;
                //audit props
                ad.CreationTime = DateTime.Now;
                ad.CreatorUserId = user.Id;
                ad.DeleterUserId = null;
                ad.DeletionTime = null;
                ad.Id = 0;
                ad.IsDeleted = false;
                ad.LastModificationTime = DateTime.Now;
                ad.LastModifierUserId = user.Id;

                //others
                ad.IsActive = true;
                adRepository.InsertAsync(ad);
                _unitOfWork.Commit();
                _unitOfWork.SaveChangesAsync();
                #endregion
                return true;
            }
            catch (Exception ex)
            {
                _unitOfWork.Rollback();
                //logger
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
                var user = await _userSvc.FindFirstAsync(x => x.UserName == username);
                if (user == null)
                {
                    throw await _serviceHelper.GetExceptionAsync("Unauthorized access! Please login");
                }
                var ad = adRepository.FirstOrDefault(x => x.Id == adid);
                if(ad == null)
                {
                    throw await _serviceHelper.GetExceptionAsync("could not find ad! please try refreshing");
                }

                #endregion
                _unitOfWork.BeginTransaction();
                ad.IsActive = false;
                adRepository.UpdateAsync(ad);
                _unitOfWork.Commit();
                _unitOfWork.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                _unitOfWork.Rollback();
                //logger;
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
                var user = await _userSvc.FindFirstAsync(x => x.UserName == username);
                if (user == null)
                {
                    throw await _serviceHelper.GetExceptionAsync("Unauthorized access! Please login");
                }
                var ad = adRepository.FirstOrDefault(x => x.Id == adid);
                if (ad == null)
                {
                    throw await _serviceHelper.GetExceptionAsync("could not find ad! please try refreshing");
                }

                #endregion

                #region make the necessary edit on a property by property basis
                _unitOfWork.BeginTransaction();
                //name
                if(!string.IsNullOrWhiteSpace(adDto.Name) && !string.Equals(ad.Name, adDto.Name))
                {
                    ad.Name = adDto.Name;
                }
                //video path
                if (!string.IsNullOrWhiteSpace(adDto.VideoPath) && !string.Equals(ad.VideoPath, adDto.VideoPath))
                {
                    ad.VideoPath = adDto.VideoPath;
                }
                //amount
                if (!string.IsNullOrWhiteSpace(adDto.Amount.ToString()) && !string.Equals(ad.Amount.ToString(), adDto.Amount.ToString()))
                {
                    ad.Amount = adDto.Amount;
                }
                //ad class
                if (adDto.AdClass != null && !string.Equals(ad.AdClass.ToString(), adDto.AdClass.ToString()))
                {
                    ad.AdClass = adDto.AdClass;
                }
                //ad status
                if (adDto.AdsStatus != null && !string.Equals(ad.AdsStatus.ToString(), adDto.AdsStatus.ToString()))
                {
                    ad.AdsStatus = adDto.AdsStatus;
                }
                //skipped ad image, subcategory
                //key words
                if (!string.IsNullOrWhiteSpace(adDto.Keywords) &&  !string.Equals(ad.Keywords, adDto.Keywords))
                {
                    ad.Keywords = adDto.Keywords;
                }
                //location
                if(!string.IsNullOrWhiteSpace(adDto.Location) && !string.Equals(ad.Location, adDto.Location))
                {
                    ad.Location = adDto.Location;
                }
                //is active
                if(adDto.IsActive != null && !string.Equals(ad.IsActive.ToString(), adDto.IsActive.ToString()))
                {
                    ad.IsActive = adDto.IsActive;
                }
                //skip ad items
                if(adDto.Room != null && !string.Equals(ad.Room, adDto.Room))
                {
                    ad.Room = adDto.Room;
                }
                //furniture
                if(adDto.Furniture != null && !string.Equals(ad.Furniture, adDto.Furniture))
                {
                    ad.Furniture = adDto.Furniture;
                }
                //Parking
                if(adDto.Parking != null && !string.Equals(ad.Parking, adDto.Parking))
                {
                    ad.Parking = adDto.Parking;
                }
                //bedroom
                if(adDto.Bedroom != null && !string.Equals(ad.Bedroom, adDto.Bedroom))
                {
                    ad.Bedroom = adDto.Bedroom;
                }
                //Bath room
                if(adDto.Bathroom != null && !string.Equals(ad.Bathroom, adDto.Bathroom))
                {
                    ad.Bathroom = adDto.Bathroom;
                }
                //land type
                if(adDto.LandType != null && !string.Equals(ad.LandType,adDto.LandType))
                {
                    ad.LandType = adDto.LandType;
                }
                //SquareMeters
                if (adDto.SquareMeters != null && !string.Equals(ad.SquareMeters.ToString(), adDto.SquareMeters.ToString()))
                {
                    ad.SquareMeters = adDto.SquareMeters;
                }
                if (!string.IsNullOrWhiteSpace(adDto.ExchangePossible) && !string.Equals(ad.ExchangePossible, adDto.ExchangePossible))
                {
                    ad.ExchangePossible = adDto.ExchangePossible;
                }

                if (!string.IsNullOrWhiteSpace(adDto.BrokerFee) && !string.Equals(ad.BrokerFee, adDto.BrokerFee))
                {
                    ad.BrokerFee = adDto.BrokerFee;
                }
                if (!string.IsNullOrWhiteSpace(adDto.Condition) && !string.Equals(ad.Condition, adDto.Condition))
                {
                    ad.Condition = adDto.Condition;
                }

                if (!string.IsNullOrWhiteSpace(adDto.Quality) && !string.Equals(ad.Quality, adDto.Quality))
                {
                    ad.Quality = adDto.Quality;
                }
                ////
                if (!string.IsNullOrWhiteSpace(adDto.CompanyName) && !string.Equals(ad.CompanyName, adDto.CompanyName))
                {
                    ad.CompanyName = adDto.CompanyName;
                }
                if (!string.IsNullOrWhiteSpace(adDto.ServiceArea) && !string.Equals(ad.ServiceArea, adDto.ServiceArea))
                {
                    ad.ServiceArea = adDto.ServiceArea;
                }
                if (!string.IsNullOrWhiteSpace(adDto.ServiceFeature) && !string.Equals(ad.ServiceFeature, adDto.ServiceFeature))
                {
                    ad.ServiceFeature = adDto.ServiceFeature;
                }
                if (!string.IsNullOrWhiteSpace(adDto.TypeOfService) && !string.Equals(ad.TypeOfService, adDto.TypeOfService))
                {
                    ad.TypeOfService = adDto.TypeOfService;
                }
                if (!string.IsNullOrWhiteSpace(adDto.Topic) && !string.Equals(ad.Topic, adDto.Topic))
                {
                    ad.Topic = adDto.Topic;
                }
                if (!string.IsNullOrWhiteSpace(adDto.Requirements) && !string.Equals(ad.Requirements, adDto.Requirements))
                {
                    ad.Requirements = adDto.Requirements;
                }
                if (!string.IsNullOrWhiteSpace(adDto.ResumePath) && !string.Equals(ad.ResumePath, adDto.ResumePath))
                {
                    ad.ResumePath = adDto.ResumePath;
                }
                ///
                if (!string.IsNullOrWhiteSpace(adDto.Title) && !string.Equals(ad.Title, adDto.Title))
                {
                    ad.Title = adDto.Title;
                }
                if (!string.IsNullOrWhiteSpace(adDto.PhoneNumber) && !string.Equals(ad.PhoneNumber, adDto.PhoneNumber))
                {
                    ad.PhoneNumber = adDto.PhoneNumber;
                }
                if (!string.IsNullOrWhiteSpace(adDto.Region) && !string.Equals(ad.Region, adDto.Region))
                {
                    ad.Region = adDto.Region;
                }
                if (!string.IsNullOrWhiteSpace(adDto.Place) && !string.Equals(ad.Place, adDto.Place))
                {
                    ad.Place = adDto.Place;
                }
                if (!string.IsNullOrWhiteSpace(adDto.Address) && !string.Equals(ad.Address, adDto.Address))
                {
                    ad.Address = adDto.Address;
                }
                if (!string.IsNullOrWhiteSpace(adDto.JobType) && !string.Equals(ad.JobType, adDto.JobType))
                {
                    ad.JobType = adDto.JobType;
                }
                if (!string.IsNullOrWhiteSpace(adDto.EmploymentStatus) && !string.Equals(ad.EmploymentStatus, adDto.EmploymentStatus))
                {
                    ad.EmploymentStatus = adDto.EmploymentStatus;
                }
                if (!string.IsNullOrWhiteSpace(adDto.Gender) && !string.Equals(ad.Gender, adDto.Gender))
                {
                    ad.Gender = adDto.Gender;
                }
                if (!string.IsNullOrWhiteSpace(adDto.Age.ToString()) && !string.Equals(ad.Age.ToString(), adDto.Age.ToString()))
                {
                    ad.Age = adDto.Age;
                }
                if (!string.IsNullOrWhiteSpace(adDto.Skills) && !string.Equals(ad.Skills, adDto.Skills))
                {
                    ad.Skills = adDto.Skills;
                }
                if (!string.IsNullOrWhiteSpace(adDto.ExpectedSalary) && !string.Equals(ad.ExpectedSalary, adDto.ExpectedSalary))
                {
                    ad.ExpectedSalary = adDto.ExpectedSalary;
                }
                if (!string.IsNullOrWhiteSpace(adDto.Education) && !string.Equals(ad.Education, adDto.Education))
                {
                    ad.Education = adDto.Education;
                }
                if (!string.IsNullOrWhiteSpace(adDto.HighestLevelOfEducation) && !string.Equals(ad.HighestLevelOfEducation, adDto.HighestLevelOfEducation))
                {
                    ad.HighestLevelOfEducation = adDto.HighestLevelOfEducation;
                }
                if (!string.IsNullOrWhiteSpace(adDto.Certification) && !string.Equals(ad.Certification, adDto.Certification))
                {
                    ad.Certification = adDto.Certification;
                }
                if (!string.IsNullOrWhiteSpace(adDto.SaveData.ToString()) && !string.Equals(ad.SaveData.ToString(), adDto.SaveData.ToString()))
                {
                    ad.SaveData = adDto.SaveData;
                }
                //skip work experience
                if (!string.IsNullOrWhiteSpace(adDto.Maker) && !string.Equals(ad.Maker, adDto.Maker))
                {
                    ad.Maker = adDto.Maker;
                }
                if (!string.IsNullOrWhiteSpace(adDto.Year) && !string.Equals(ad.Year, adDto.Year))
                {
                    ad.Year = adDto.Year;
                }
                if (!string.IsNullOrWhiteSpace(adDto.Color) && !string.Equals(ad.Color, adDto.Color))
                {
                    ad.Color = adDto.Color;
                }
                if (!string.IsNullOrWhiteSpace(adDto.Seats.ToString()) && !string.Equals(ad.Seats.ToString(), adDto.Seats.ToString()))
                {
                    ad.Seats = adDto.Seats;
                }
                if (!string.IsNullOrWhiteSpace(adDto.CarType) && !string.Equals(ad.CarType, adDto.CarType))
                {
                    ad.CarType = adDto.CarType;
                }
                if (!string.IsNullOrWhiteSpace(adDto.FuelType) && !string.Equals(ad.FuelType, adDto.FuelType))
                {
                    ad.FuelType = adDto.FuelType;
                }
                if (!string.IsNullOrWhiteSpace(adDto.Mileage) && !string.Equals(ad.Mileage, adDto.Mileage))
                {
                    ad.Mileage = adDto.Mileage;
                }
                //ad image, subcategory, ad items, work experience
                adRepository.UpdateAsync(ad);
                _unitOfWork.Commit();
                _unitOfWork.SaveChangesAsync();
                #endregion
                return true;
            }
            catch (Exception ex)
            {
                _unitOfWork.Rollback();
                //logger;
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
                var user = await _userSvc.FindFirstAsync(x => x.UserName == username);
                if (user == null)
                {
                    throw await _serviceHelper.GetExceptionAsync("Unauthorized access! Please login");
                }
                var ad = adRepository.FirstOrDefault(x => x.Id == adid);
                if (ad == null)
                {
                    throw await _serviceHelper.GetExceptionAsync("could not find ad! please try refreshing");
                }

                #endregion
                _unitOfWork.BeginTransaction();
                ad.AdClass = AdClass.Premium;
                adRepository.UpdateAsync(ad);
                _unitOfWork.Commit();
                _unitOfWork.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                _unitOfWork.Rollback();
                //logger;
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
                var user = await _userSvc.FindFirstAsync(x => x.UserName == username);
                if (user == null)
                {
                    throw await _serviceHelper.GetExceptionAsync("Unauthorized access! Please login");
                }
                #endregion
                _unitOfWork.BeginTransaction();
                itemRepository.InsertAsync(model);
                _unitOfWork.Commit();
                return true;
            }
            catch (Exception ex)
            {
                _unitOfWork.Rollback();
                //logger
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
                var user = await _userSvc.FindFirstAsync(x => x.UserName == username);
                if (user == null)
                {
                    throw await _serviceHelper.GetExceptionAsync("Unauthorized access! Please login");
                }
                var item = itemRepository.FirstOrDefault(x => x.Id == ItemId);
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
                itemRepository.UpdateAsync(item);
                _unitOfWork.Commit();
                return true;
            }
            catch (Exception ex)
            {
                _unitOfWork.Rollback();
                //logger
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
                var user = await _userSvc.FindFirstAsync(x => x.UserName == username);
                if (user == null)
                {
                    throw await _serviceHelper.GetExceptionAsync("Unauthorized access! Please login");
                }

                #endregion
                _unitOfWork.BeginTransaction();
                itemRepository.GetAll().Where(x => ItemIds.Contains(x.Id)).AsTracking().ToList().ForEach(y => y.GroupName = groupname);
                _unitOfWork.Commit();
                return true;
            }
            catch (Exception ex)
            {
                _unitOfWork.Rollback();
                //logger
                return false;
            }
        }

        public async Task<bool> AddAdToCart(long Id, string username)
        {
            try
            {
                return true;
            }
            catch (Exception ex)
            {

                return false;
            }
        }

        public async Task<bool> RemoveAdFromCart(long Id, string username)
        {
            try
            {
                return true;
            }
            catch (Exception ex)
            {

                return false;
            }
        }

        public async Task<IEnumerable<AdDTO>> Search(SearchVM model, string username)
        {
            try
            {
                List<AdDTO> result = new List<AdDTO>();

                return result;
            }
            catch (Exception ex)
            {
                return null;
            }
        }
    }
}
//show premium ads first