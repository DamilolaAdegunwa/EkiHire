﻿using EkiHire.Core.Configuration;
using EkiHire.Core.Domain.Entities;
using EkiHire.Core.Domain.Entities.Enums;
using EkiHire.Core.Domain.Model;
using EkiHire.Core.Exceptions;
using EkiHire.Core.Messaging.Email;
using EkiHire.Core.Model;
using EkiHire.Core.Utils;
using EkiHire.Data.Repository;
using EkiHire.Data.UnitOfWork;
using log4net;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Threading.Tasks;
using EkiHire.Core.Messaging.Sms;
using System.Text.RegularExpressions;

namespace EkiHire.Business.Services
{
    public interface IUserService
    {
        Task SignUp(LoginViewModel model);
        Task<User> FindByNameAsync(string username);
        Task<User> FindByEmailAsync(string email);
        Task<bool> CheckPasswordAsync(User user, string password);
        Task UpdateAsync(User user);
        Task<IList<string>> GetUserRoles(User user);
        Task<IdentityResult> CreateAsync(User user);
        //public Task<IdentityResult> CreateAsync(User user);
        Task<User> FindFirstAsync(Expression<Func<User, bool>> predicate);
        Task<bool> ExistAsync(Expression<Func<User, bool>> predicate);
        Task<IdentityResult> CreateAsync(User user, string password);
        //public Task<IdentityResult> CreateAsync(User user, string password);
        Task<IdentityResult> AddToRoleAsync(User user, string role);
        string HashPassword(User user, string password);
        Task<IList<User>> GetUsersInRoleAsync(string role);
        Task<string> GenerateEmailConfirmationTokenAsync(User user);
        Task<User> ActivateAccount(string username, string activationCode);
        Task<User> GetProfile(string username);
        Task<bool> ForgotPassword(string username);
        Task<bool> ResetPassword(PassordReset model);
        Task<bool> ChangePassword(string userName, ChangePassord model);
        Task<IList<string>> GetUserRolesAsync(string username);
        Task<bool> UpdateProfile(string userName, UserProfileDTO model);
        Task<bool> IsInRoleAsync(User user, string role);
        Task<IdentityResult> RemoveFromRolesAsync(User user, IEnumerable<string> roles);
        Task<bool> ResendVerificationCode(string username);
        //Task<bool> TestEmail();
        Task<bool> ValidatePassordResetCode(PassordReset model);
        Task<bool> ChangeName(Name name, string username);
        Task<bool> ChangeGender(Gender gender, string username);
        Task<bool> ChangeBirthday(DateTime birthdate, string username);
        Task<bool> ChangeEmail(string userEmail, string username);
        Task<bool> ChangePhoneNumber(string userPhoneNumber, string username);
        //Task<IDictionary<DateTime, List<PostDTO>>> PostTimeGraph();
        Task<bool> ChangeProfileImage(string profileImageString, string username);
        Task<bool> ChangeIsActive(bool isActive, string username);
        Task<bool> ChangeUserType(UserType userType, long receipientId, string username);
        Task<bool> AddUser(LoginViewModel model, string username);
        Task<bool> DeleteUser(long receipientId, string username);
    }

    public class UserService : IUserService
    {

        private readonly UserManager<User> _userManager;
        private readonly AppConfig appConfig;
        private readonly IRepository<User> _userRepository;
        private readonly IServiceHelper _svcHelper;
        //private readonly IMailService _mailSvc;
        private readonly IHostingEnvironment _hostingEnvironment;
        private readonly ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().Name);
        private readonly IUnitOfWork _unitOfWork;
        readonly SmtpConfig _smtpsettings;
        private readonly IChatHub _chatHub;
        public UserService(
            UserManager<User> userManager, 
            IServiceHelper svcHelper,
            IRepository<User> userRepository,
            //IMailService mailSvc,
            IHostingEnvironment hostingEnvironment,
            IOptions<AppConfig> _appConfig,
            IUnitOfWork unitOfWork
            , IOptions<SmtpConfig> settingSvc
            , IChatHub chatHub
            )
        {
            _userManager = userManager;
            _svcHelper = svcHelper;
            //_mailSvc = mailSvc;
            appConfig = _appConfig.Value;
            _hostingEnvironment = hostingEnvironment;
            _userRepository = userRepository;
            _unitOfWork = unitOfWork;
            _smtpsettings = settingSvc.Value;
            _chatHub = chatHub;
        }
        public async Task SignUp(LoginViewModel model)
        {
            # region validate credential

            //check that the model carries data
            if (model == null)
            {
                throw await _svcHelper.GetExceptionAsync("Invalid parameter");
            }
            //check that the model carries a password 
            if (string.IsNullOrWhiteSpace(model.Password))
            {
                throw await _svcHelper.GetExceptionAsync("Please input a password");
            }

            //check that the user does not already exist
            var user = await FindFirstAsync(x => x.UserName == model.UserName);
            if (user != null)
            {
                throw await _svcHelper.GetExceptionAsync("User already exist");
            }

            //check that the username is a valid email ( the password would be validate by the Identity builder)
            if (!Regex.IsMatch(model.UserName, @"^[^@\s]+@[^@\s]+\.[^@\s]+$", RegexOptions.IgnoreCase))
            {
                throw await _svcHelper.GetExceptionAsync("The UserName isn't Invalid Email");
            }

            //check for validate usertype
            #endregion

            #region sign up a new user
            if (model.UserType == UserType.Customer)
            {
                try
                {
                    _unitOfWork.BeginTransaction();
                    string fn = null;
                    try { fn = model.FullName.Split(' ')[0]; } catch { }
                    string ln = null;
                    try { ln = model.FullName.Split(' ')[1]; } catch { }
                    user = new User
                    {
                        FirstName = fn,
                        LastName = ln,
                        UserName = model.UserName,
                        Email = model.UserName,
                        AccountConfirmationCode = CommonHelper.GenerateRandonAlphaNumeric(),
                        EmailConfirmed = false,
                        PhoneNumberConfirmed = false,
                        UserType = UserType.Customer,//model.UserType,
                        IsActive = true,
                    };
                    var creationStatus = await CreateAsync(user, model.Password);

                    if (creationStatus.Succeeded)
                    {
                        try
                        {
                            #region send notification
                            //first compose Welcome notification
                            Notification welcomeNotification = new Notification
                            {
                                Delivered = false,
                                IsBroadCast = false,
                                Message = "Thank you for registering on our platform. You are one step away from getting amazing shopping deals.",
                                Title = "Welcome To EkiHire.com",
                                RecipientId = user.Id,
                                NotificationType = NotificationType.Welcome,
                                Recipient = null,
                                //basic properties
                                CreationTime = DateTime.Now,
                                CreatorUserId = user.Id,
                                IsDeleted = false,
                                LastModificationTime = DateTime.Now,
                                LastModifierUserId = user.Id,
                                DeleterUserId = null,
                                DeletionTime = null,
                                Id = 0,
                                RecipientUserName = user.UserName
                            };
                            try
                            {
                                await _chatHub.SendNotification(welcomeNotification);
                            }
                            catch (Exception)
                            {

                            }

                            #endregion
                            #region old email implementation
                            //var replacement = new StringDictionary
                            //{
                            //    //["FirstName"] = user.FirstName,
                            //    ["ActivationCode"] = user.AccountConfirmationCode
                            //};

                            //var mail = new Mail(_smtpsettings.UserName, "EkiHire.com: Account Verification Code", user.Email)
                            //{
                            //    BodyIsFile = true,
                            //    BodyPath = Path.Combine(_hostingEnvironment.ContentRootPath, CoreConstants.Url.ActivationCodeEmail),
                            //    SenderDisplayName = _smtpsettings.SenderDisplayName,

                            //};
                            //await _mailSvc.SendMailAsync(mail, replacement);
                            #endregion

                            //first get the file
                            var filePath = Path.Combine(_hostingEnvironment.ContentRootPath, CoreConstants.Url.ActivationCodeEmail);
                            if (File.Exists(filePath))
                            {
                                var fileString = File.ReadAllText(filePath);
                                if (!string.IsNullOrWhiteSpace(fileString))
                                {
                                    fileString = fileString.Replace("{{ActivationCode}}", $"{user.AccountConfirmationCode}");

                                    _svcHelper.SendEMail(user.UserName, fileString, "EkiHire.com: Account Verification Code");
                                }
                            }
                        }
                        catch (Exception ex)
                        {
                            log.Error($"{ex.Message} :: {MethodBase.GetCurrentMethod().Name} :: {ex.StackTrace}");
                        }
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
                    }
                    else
                    {
                        _unitOfWork.Rollback();

                        throw await _svcHelper.GetExceptionAsync(creationStatus.Errors.FirstOrDefault()?.Description);
                    }
                    _unitOfWork.Commit();
                }
                catch (Exception ex)
                {
                    _unitOfWork.Rollback();
                    var errMsg = $"an error occured while trying to signup. Please try again!";
                    log.Error($"{errMsg} :: stack trace - {ex.StackTrace} :: exception message - {ex.Message}", ex);
                    throw new Exception(errMsg);
                    //throw await _svcHelper.GetExceptionAsync("an error occured!"); ;
                }
            }

            //the sign up will be adapted for different users types
            #endregion
        }
        protected virtual Task<IdentityResult> CreateAsync(User user)
        {
            return _userManager.CreateAsync(user);
        }

        protected virtual Task<IdentityResult> AddToRoleAsync(User user, string role)
        {
            return _userManager.AddToRoleAsync(user, role);
        }

        protected virtual string HashPassword(User user, string password)
        {
            return _userManager.PasswordHasher.HashPassword(user, password);
        }

        protected virtual Task<IdentityResult> CreateAsync(User user, string password)
        {
            return _userManager.CreateAsync(user, password);
        }

        protected virtual Task<User> FindFirstAsync(Expression<Func<User, bool>> filter)
        {
            return _userManager.Users.Where(filter).FirstOrDefaultAsync();
        }

        protected virtual Task<bool> ExistAsync(Expression<Func<User, bool>> filter)
        {
            return _userManager.Users.AnyAsync(filter);
        }

        protected virtual Task<User> FindByNameAsync(string username)
        {
            return _userManager.FindByNameAsync(username);
        }

        protected virtual Task<IList<string>> GetUserRoles(User user)
        {
            return _userManager.GetRolesAsync(user);
            //return _userManager.GetRolesAsync(user);
        }
        public async Task<IList<string>> GetUserRolesAsync(string username)
        {
            var user = await _userManager.FindByNameAsync(username);
            return await _userManager.GetRolesAsync(user);
        }
        protected virtual Task<User> FindByEmailAsync(string email)
        {
            return _userManager.FindByEmailAsync(email);
        }

        protected virtual Task<IList<User>> GetUsersInRoleAsync(string role)
        {
            return _userManager.GetUsersInRoleAsync(role);
        }

        protected virtual Task<string> GenerateEmailConfirmationTokenAsync(User user)
        {
            return _userManager.GenerateEmailConfirmationTokenAsync(user);
        }

        protected virtual Task<bool> CheckPasswordAsync(User user, string password)
        {
            return _userManager.CheckPasswordAsync(user, password);
        }

        protected virtual Task UpdateAsync(User user)
        {
            return _userManager.UpdateAsync(user);
        }

        Task<User> IUserService.FindByNameAsync(string username)
        {
            return FindByNameAsync(username);
        }

        Task<User> IUserService.FindByEmailAsync(string email)
        {
            return FindByEmailAsync(email);
        }

        Task<bool> IUserService.CheckPasswordAsync(User user, string password)
        {
            return CheckPasswordAsync(user, password);
        }

        Task IUserService.UpdateAsync(User user)
        {
            return UpdateAsync(user);
        }

        Task<IList<string>> IUserService.GetUserRoles(User user)
        {
            return GetUserRoles(user);
        }

        Task<IdentityResult> IUserService.CreateAsync(User user)
        {
            return CreateAsync(user);
        }
        //public Task<IdentityResult> CreateAsync(User user)
        //{
        //    return _userManager.CreateAsync(user);
        //}
        Task<User> IUserService.FindFirstAsync(Expression<Func<User, bool>> filter)
        {
            return FindFirstAsync(filter);
        }

        Task<bool> IUserService.ExistAsync(Expression<Func<User, bool>> predicate)
        {
            return ExistAsync(predicate);
        }

        Task<IdentityResult> IUserService.CreateAsync(User user, string password)
        {
            return CreateAsync(user, password);
        }
        //public Task<IdentityResult> CreateAsync(User user, string password)
        //{
        //    return _userManager.CreateAsync(user);
        //}
        Task<IdentityResult> IUserService.AddToRoleAsync(User user, string role)
        {
            return AddToRoleAsync(user, role);
        }

        string IUserService.HashPassword(User user, string password)
        {
            return HashPassword(user, password);
        }

        Task<IList<User>> IUserService.GetUsersInRoleAsync(string role)
        {
            return GetUsersInRoleAsync(role);
        }

        Task<string> IUserService.GenerateEmailConfirmationTokenAsync(User user)
        {
            return GenerateEmailConfirmationTokenAsync(user);
        }

        /// <summary>
        /// Use the given OTP to confirm that the user is authentic
        /// </summary>
        /// <param name="username"></param>
        /// <param name="activationCode"></param>
        /// <returns></returns> /*checked*/
        public async Task<User> ActivateAccount(string username, string activationCode)
        {
            try
            {
                if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(activationCode))
                {
                    throw await _svcHelper.GetExceptionAsync("Please input a valid username and activation code");
                }

                var user = await FindByNameAsync(username);

                //await ValidateUser(user);
                User userDto = new User();
                if (user.IsConfirmed())
                {
                    userDto = user;
                    return userDto;
                    //return user == null ? null : new UserDTO
                    //{
                    //    PhoneNumber = user.PhoneNumber,
                    //    Email = user.Email,
                    //    FirstName = user.FirstName,
                    //    LastName = user.LastName,
                    //    Id = user.Id,
                    //    Gender = user.Gender,
                    //    IsActive = user.IsConfirmed(),
                    //    AccountIsDeleted = user.IsDeleted,
                    //};
                    //throw new EkiHireGenericException("Your account was activated earlier.");
                }


                if (activationCode == user.AccountConfirmationCode)
                {

                    user.EmailConfirmed = true;
                    user.PhoneNumberConfirmed = true;

                    await UpdateAsync(user);

                }
                else if (activationCode != user.AccountConfirmationCode)
                {
                    //throw new EkiHireGenericException("Invalid OTP");
                    throw new Exception("Invalid OTP");
                    //await _svcHelper.GetExceptionAsync(ErrorConstants.USER_ACCOUNT_INVALID_OTP);
                }
                
                userDto = user;
                return userDto;
                //return user == null ? null : new UserDTO
                //{
                //    PhoneNumber = user.PhoneNumber,
                //    Email = user.Email,
                //    FirstName = user.FirstName,
                //    LastName = user.LastName,
                //    Id = user.Id,
                //    Gender = user.Gender,
                //    IsActive = user.IsConfirmed(),
                //    AccountIsDeleted = user.IsDeleted,
                //};
            }
            catch (Exception ex)
            {
                log.Error($"{ex.Message} :: {MethodBase.GetCurrentMethod().Name} :: {ex.StackTrace} ");
                //return null;
                throw ex;
            }
        }

        public async Task<User> GetProfile(string username)
        {
            if (string.IsNullOrEmpty(username))
            {
                throw await _svcHelper.GetExceptionAsync(ErrorConstants.USER_ACCOUNT_NOT_EXIST);
            }

            var user = await FindByNameAsync(username);

            await ValidateUser(user);

            return user;
            //return user is null ? null : new UserDTO
            //{
            //    Email = user.Email,
            //    FirstName = user.FirstName,
            //    LastName = user.LastName,
            //    Gender = user.Gender,
            //    NextOfKinName = user.NextOfKinName,
            //    NextOfKinPhone = user.NextOfKinPhone,
            //    PhoneNumber = user.PhoneNumber,
            //    ReferralCode = user.ReferralCode,
            //    Address = user.Address, 
            //    MiddleName = user.MiddleName,
            //    CreationTime = user.CreationTime,
            //    DateOfBirth = user.DateOfBirth,
            //    SubscriptionPlan = user.SubscriptionPlan
            //};
        }

        public async Task<bool> ForgotPassword(string username)
        {
            if (string.IsNullOrEmpty(username))
            {
                throw await _svcHelper.GetExceptionAsync(ErrorConstants.USER_ACCOUNT_NOT_EXIST);
            }

            var user = await FindByNameAsync(username);

            await ValidateUser(user);

            user.OTP = CommonHelper.RandomDigits(5);
            await _userManager.UpdateAsync(user);

            #region old email implementation
            //var replacement = new StringDictionary
            //{
            //    ["FirstName"] = user.UserName,
            //    ["Otp"] = user.OTP
            //};

            //var mail = new Mail(_smtpsettings.UserName, "EkiHire.com: Password Reset OTP", user.Email)
            //{
            //    BodyIsFile = true,
            //    BodyPath = Path.Combine(_hostingEnvironment.ContentRootPath, CoreConstants.Url.PasswordResetEmail)
            //};

            //await _mailSvc.SendMailAsync(mail, replacement);
            #endregion

            try
            {
                //first get the file
                var filePath = Path.Combine(_hostingEnvironment.ContentRootPath, CoreConstants.Url.PasswordResetEmail);
                if (File.Exists(filePath))
                {
                    var fileString = File.ReadAllText(filePath);
                    if (!string.IsNullOrWhiteSpace(fileString))
                    {
                        //fileString = fileString.Replace("{{FirstName}}", $"{model.FirstName}");
                        fileString = fileString.Replace("{{FirstName}}", $"{user.UserName}");
                        fileString = fileString.Replace("{{Otp}}", $"{user.OTP}");

                        _svcHelper.SendEMail(user.UserName, fileString, "EkiHire.com: Password Reset OTP");
                    }
                }
            }
            catch (Exception ex)
            {
                log.Error($"{ex.Message} :: {MethodBase.GetCurrentMethod().Name} :: {ex.StackTrace}");
            }

            return await Task.FromResult(true);
        }

        public async Task<bool> ValidatePassordResetCode(PassordReset model)
        {
            if (string.IsNullOrEmpty(model.UserName))
            {
                throw await _svcHelper.GetExceptionAsync(ErrorConstants.USER_ACCOUNT_NOT_EXIST);
            }

            var user = await FindByNameAsync(model.UserName);

            await ValidateUser(user);

            if (user.OTP != model.Code)
            {
                throw new EkiHireGenericException("Invalid Reset OTP");
            }
            return true;
        }

        public async Task<bool> ResetPassword(PassordReset model)
        {
            if (string.IsNullOrEmpty(model.UserName))
            {
                throw await _svcHelper.GetExceptionAsync(ErrorConstants.USER_ACCOUNT_NOT_EXIST);
            }

            var user = await FindByNameAsync(model.UserName);

            await ValidateUser(user);

            if (user.OTP != model.Code)
            {
                throw new EkiHireGenericException("Invalid OTP");
                // throw await _svcHelper.GetExceptionAsync(ErrorConstants.USER_ACCOUNT_WRONG_OTP);
            }

            var changeToken = await _userManager.GeneratePasswordResetTokenAsync(user);

            var result = await _userManager.ResetPasswordAsync(user, changeToken, model.NewPassword);

            if (!result.Succeeded)
            {
                throw await _svcHelper.GetExceptionAsync(result.Errors?.FirstOrDefault().Description);
            }

            user.OTP = null;
            await _userManager.UpdateAsync(user);

            return true;
        }

        public async Task<bool> ChangePassword(string username, ChangePassord model)
        {
            if (string.IsNullOrEmpty(username))
            {
                throw await _svcHelper.GetExceptionAsync(ErrorConstants.USER_ACCOUNT_NOT_EXIST);
            }

            var user = await FindByNameAsync(username);

            await ValidateUser(user);

            var result = await _userManager.ChangePasswordAsync(user, model.CurrentPassword, model.NewPassword);

            if (!result.Succeeded)
            {
                throw await _svcHelper.GetExceptionAsync(result.Errors?.FirstOrDefault().Description);
            }

            return true;
        }

        public Task<bool> IsInRoleAsync(User user, string role)
        {
            if (string.IsNullOrWhiteSpace(role) || user is null)
                throw new ArgumentNullException();

            return _userManager.IsInRoleAsync(user, role);
        }

        public Task<IdentityResult> RemoveFromRolesAsync(User user, IEnumerable<string> roles)
        {
            if (roles == null || user is null)
                throw new ArgumentNullException();

            return _userManager.RemoveFromRolesAsync(user, roles);
        }

        private async Task ValidateUser(User user)
        {
            if (user == null)
                throw await _svcHelper.GetExceptionAsync(ErrorConstants.USER_ACCOUNT_NOT_EXIST);

            if (user.IsDeleted)
                throw await _svcHelper.GetExceptionAsync(ErrorConstants.USER_ACCOUNT_NOT_EXIST);

            if (user.AccountLocked())
                throw await _svcHelper.GetExceptionAsync(ErrorConstants.USER_ACCOUNT_LOCKED);
        }

        public async Task<bool> UpdateProfile(string username, UserProfileDTO model)
        {
            if (string.IsNullOrEmpty(username))
            {
                throw await _svcHelper.GetExceptionAsync(ErrorConstants.USER_ACCOUNT_NOT_EXIST);
            }

            var user = await FindByNameAsync(username);

            await ValidateUser(user);

            if (!string.IsNullOrWhiteSpace(model.FirstName) && !string.Equals(user.FirstName, model.FirstName))
            {
                user.FirstName = model.FirstName;
            }

            if (!string.IsNullOrWhiteSpace(model.LastName) && !string.Equals(user.LastName, model.LastName))
            {
                user.LastName = model.LastName;
            }

            if (!string.IsNullOrWhiteSpace(model.PhoneNumber) && !string.Equals(user.PhoneNumber, model.PhoneNumber))
            {
                user.PhoneNumber = model.PhoneNumber;
            }

            if (!string.IsNullOrWhiteSpace(model.MiddleName) && !string.Equals(user.MiddleName, model.MiddleName))
            {
                user.MiddleName = model.MiddleName;
            }

            if (!string.IsNullOrWhiteSpace(model.Address) && !string.Equals(user.Address, model.Address))
            {
                user.Address = model.Address;
            }

            if (!string.IsNullOrWhiteSpace(model.NextOfKin) && !string.Equals(user.NextOfKinName, model.NextOfKin))
            {
                user.NextOfKinName = model.NextOfKin;
            }

            if (!string.IsNullOrWhiteSpace(model.DateOfBirth) && !string.Equals(user.DateOfBirth, model.DateOfBirth))
            {
                DateTime birthdate;
                if(DateTime.TryParse(model.DateOfBirth, out birthdate))
                {
                    user.DateOfBirth = birthdate;
                }
            }

            if (!string.IsNullOrWhiteSpace(model.NextOfKinPhone) && !string.Equals(user.NextOfKinPhone, model.NextOfKinPhone))
            {
                user.NextOfKinPhone = model.NextOfKinPhone;
            }

            if (!string.IsNullOrWhiteSpace(model.Title) && !string.Equals(user.Title, model.Title))
            {
                user.Title = model.Title;
            }

            if (!string.IsNullOrWhiteSpace(model.Email) && !string.Equals(user.Email, model.Email) && model.Email.IsValidEmail())
            {
                user.NextOfKinName = model.Email;
            }

            var result = await _userManager.UpdateAsync(user);

            return result.Succeeded;
        }

        public async Task<bool> ResendVerificationCode(string username)
        {
            try
            {
                var user = _userRepository.FirstOrDefault(u => u.UserName == username);
                if (user != null)
                {
                    #region old email implementation
                    //var replacement = new StringDictionary
                    //{
                    //    //["FirstName"] = user.FirstName,
                    //    ["ActivationCode"] = user.AccountConfirmationCode
                    //};

                    //var mail = new Mail(appConfig.AppEmail, "EkiHire.com: Account Verification Code", user.Email)
                    //{
                    //    BodyIsFile = true,
                    //    BodyPath = Path.Combine(_hostingEnvironment.ContentRootPath, CoreConstants.Url.ActivationCodeEmail)
                    //};

                    //await _mailSvc.SendMailAsync(mail, replacement);
                    #endregion

                    try
                    {
                        //first get the file
                        var filePath = Path.Combine(_hostingEnvironment.ContentRootPath, CoreConstants.Url.ActivationCodeEmail);
                        if (File.Exists(filePath))
                        {
                            var fileString = File.ReadAllText(filePath);
                            if (!string.IsNullOrWhiteSpace(fileString))
                            {
                                //fileString = fileString.Replace("{{FirstName}}", $"{user.FirstName}");
                                fileString = fileString.Replace("{{ActivationCode}}", $"{user.AccountConfirmationCode}");

                                _svcHelper.SendEMail(user.UserName, fileString, "EkiHire.com: Account Verification Code");
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        log.Error($"{ex.Message} :: {MethodBase.GetCurrentMethod().Name} :: {ex.StackTrace}");
                    }
                    #region old code  snippet
                    //try
                    //{
                    //    //first get the file
                    //    var filePath = Path.Combine(_hostingEnvironment.ContentRootPath, CoreConstants.Url.PasswordResetEmail);
                    //    if (File.Exists(filePath))
                    //    {
                    //        var fileString = File.ReadAllText(filePath);
                    //        if (!string.IsNullOrWhiteSpace(fileString))
                    //        {
                    //            //fileString = fileString.Replace("{{FirstName}}", $"{model.FirstName}");
                    //            fileString = fileString.Replace("{{FirstName}}", $"{user.UserName}");
                    //            fileString = fileString.Replace("{{Otp}}", $"{user.OTP}");

                    //            _svcHelper.SendEMail(user.UserName, fileString, "EkiHire.com: Password Reset OTP");
                    //        }
                    //    }
                    //}
                    //catch (Exception ex)
                    //{
                    //    log.Error($"{ex.Message} :: {MethodBase.GetCurrentMethod().Name} :: {ex.StackTrace}");
                    //}
                    #endregion

                    return true;
                }
                return false;
            }
            catch (Exception ex)
            {
                log.Error($"{ex.Message} :: {ex.StackTrace}", ex);
                return false;
            }
            
        }
		//public async Task<bool> TestEmail()
  //      {
		//	try{
		//		var replacement = new StringDictionary
  //              {
  //                  //["FirstName"] = "Damilola Adegunwa",
  //                  ["ActivationCode"] = "FAKE123456"
  //              };

  //              var mail = new Mail(appConfig.AppEmail, "EkiHire.com: Account Verification Code", "damee1993@gmail.com")
  //              {
  //                  BodyIsFile = true,
  //                  BodyPath = Path.Combine(_hostingEnvironment.ContentRootPath, CoreConstants.Url.ActivationCodeEmail)
  //              };

  //              await _mailSvc.SendMailAsync(mail, replacement);
  //              return true;
		//	}catch(Exception e){
		//		return false;
		//	}
  //      }

        public async Task<bool> ChangeName(Name name, string username)
        {
            if (name == null || string.IsNullOrEmpty(username))
            {
                //throw await _svcHelper.GetExceptionAsync(ErrorConstants.USER_ACCOUNT_NOT_EXIST);
                throw new EkiHireGenericException("Invalid data");
            }

            var user = await FindByNameAsync(username);

            await ValidateUser(user);

            if (!user.IsConfirmed())
            {
                throw new EkiHireGenericException("Your account has not been activated!");
            }
            user.FirstName = name.FirstName;
            user.LastName = name.LastName;

            await UpdateAsync(user);
            return true;
        }

        public async Task<bool> ChangeGender(Gender gender, string username)
        {
            if (string.IsNullOrEmpty(username))
            {
                //throw await _svcHelper.GetExceptionAsync(ErrorConstants.USER_ACCOUNT_NOT_EXIST);
                throw new EkiHireGenericException("Could not identify user");
            }

            var user = await FindByNameAsync(username);

            await ValidateUser(user);

            if (!user.IsConfirmed())
            {
                throw new EkiHireGenericException("Your account has not been activated!");
            }
            user.Gender = gender;

            await UpdateAsync(user);
            return true;
        }

        public async Task<bool> ChangeBirthday(DateTime birthdate, string username)
        {
            if (string.IsNullOrEmpty(username))
            {
                //throw await _svcHelper.GetExceptionAsync(ErrorConstants.USER_ACCOUNT_NOT_EXIST);
                throw new EkiHireGenericException("Could not identify user");
            }
            if (birthdate == new DateTime())
            {
                //throw await _svcHelper.GetExceptionAsync(ErrorConstants.USER_ACCOUNT_NOT_EXIST);
                throw new EkiHireGenericException("Invalid date given");
            }
            var user = await FindByNameAsync(username);

            await ValidateUser(user);

            if (!user.IsConfirmed())
            {
                throw new EkiHireGenericException("Your account has not been activated!");
            }
            user.DateOfBirth = birthdate;

            await UpdateAsync(user);
            return true;
        }

        public async Task<bool> ChangeEmail(string userEmail, string username)
        {
            if (string.IsNullOrEmpty(username)/* || string.IsNullOrEmpty(userEmail)*/)
            {
                //throw await _svcHelper.GetExceptionAsync(ErrorConstants.USER_ACCOUNT_NOT_EXIST);
                throw new EkiHireGenericException("Could not identify user");
            }
            if (string.IsNullOrEmpty(userEmail))
            {
                //throw await _svcHelper.GetExceptionAsync(ErrorConstants.USER_ACCOUNT_NOT_EXIST);
                throw new EkiHireGenericException("Invalid data given");
            }
            var user = await FindByNameAsync(username);

            await ValidateUser(user);

            if (!user.IsConfirmed())
            {
                throw new EkiHireGenericException("Your account has not been activated!");
            }
            user.Email = userEmail;

            await UpdateAsync(user);
            return true;
            //I must warn that it is not a good thing to change the UserEmail, especially without verifying that it is the true owner
            //doing the change (, also you must never allow change to the UserName whatever the reason) so we will need to add OTP 
            //verification for this step.
        }

        public async Task<bool> ChangePhoneNumber(string userPhoneNumber, string username)
        {
            if (string.IsNullOrEmpty(username)/* || string.IsNullOrEmpty(userEmail)*/)
            {
                //throw await _svcHelper.GetExceptionAsync(ErrorConstants.USER_ACCOUNT_NOT_EXIST);
                throw new EkiHireGenericException("Could not identify user");
            }
            if (string.IsNullOrEmpty(userPhoneNumber))
            {
                //throw await _svcHelper.GetExceptionAsync(ErrorConstants.USER_ACCOUNT_NOT_EXIST);
                throw new EkiHireGenericException("Invalid data given");
            }
            var user = await FindByNameAsync(username) ?? await FindByEmailAsync(username);

            await ValidateUser(user);

            if (!user.IsConfirmed())
            {
                throw new EkiHireGenericException("Your account has not been activated!");
            }
            user.PhoneNumber = userPhoneNumber;

            await UpdateAsync(user);
            return true;
            //I must warn that it is not a good thing to change the userPhoneNumber, especially without verifying that it is the true owner
            //doing the change (, also you must never allow change to the UserName whatever the reason) so we will need to add OTP 
            //verification for this step.
        }

        //public Task<IDictionary<DateTime, List<PostDTO>>> PostTimeGraph()
        //{
        //    throw new NotImplementedException();
        //}

        public async Task<bool> ChangeProfileImage(string profileImageString, string username)
        {
            try
            {
                if (string.IsNullOrEmpty(username))
                {
                    //throw await _svcHelper.GetExceptionAsync(ErrorConstants.USER_ACCOUNT_NOT_EXIST);
                    throw new EkiHireGenericException("Could not identify user");
                }
                var user = await FindByNameAsync(username);
                await ValidateUser(user);
                if (!user.IsConfirmed())
                {
                    throw new EkiHireGenericException("Your account has not been activated!");
                }
                user.ImagePath = profileImageString;
                await UpdateAsync(user);
                return true;
            }
            catch (Exception ex)
            {
                _unitOfWork.Rollback();
                log.Error($"{ex.Message} :: {MethodBase.GetCurrentMethod().Name} :: {ex.StackTrace} ");
                return false;
            }
            
        }
        public async Task<bool> ChangeIsActive(bool isActive, string username)
        {
            try
            {
                if (string.IsNullOrEmpty(username))
                {
                    //throw await _svcHelper.GetExceptionAsync(ErrorConstants.USER_ACCOUNT_NOT_EXIST);
                    throw new EkiHireGenericException("Could not identify user");
                }
                var user = await FindByNameAsync(username);
                await ValidateUser(user);
                if (!user.IsConfirmed())
                {
                    throw new EkiHireGenericException("Your account has not been activated!");
                }
                user.IsActive = isActive;
                await UpdateAsync(user);
                return true;
            }
            catch (Exception ex)
            {
                _unitOfWork.Rollback();
                log.Error($"{ex.Message} :: {MethodBase.GetCurrentMethod().Name} :: {ex.StackTrace} ");
                return false;
            }

        }
        public async Task<bool> AddUser(LoginViewModel model, string username)
        {
            try
            {
                #region validate credential

                //check that the model carries data
                if (model == null)
                {
                    throw new Exception("no input provided!");
                }
                //check for non-empty username 
                if (string.IsNullOrWhiteSpace(username))
                {
                    throw new Exception("Please login and retry");
                }

                //check that the user exist
                var user = await FindFirstAsync(x => x.UserName == username);
                if (user == null)
                {
                    throw new Exception("User does not exist");
                }
                //check that the privilege of the user
                if(user.UserType == UserType.SuperAdministrator)
                {
                    //fine
                }
                else if (user.UserType == UserType.Administrator)
                {
                    if(model.UserType == UserType.SuperAdministrator)
                    {
                        throw new Exception("you are not authorized to add user with this role");
                    }
                }
                else
                {
                    throw new Exception("you are not authorized to add user");
                }

                //check for valid usertype, validate the adtype if premium whether user can put premium ad
                #endregion
                string fn = null;
                try { fn = model.FullName.Split(' ')[0]; } catch { }
                string ln = null;
                try { ln = model.FullName.Split(' ')[1]; } catch { }
                user = new User
                {
                    FirstName = fn,
                    LastName = ln,
                    UserName = model.UserName,
                    Email = model.UserName,
                    AccountConfirmationCode = CommonHelper.GenerateRandonAlphaNumeric(),
                    EmailConfirmed = true,
                    PhoneNumberConfirmed = true,
                    UserType = model.UserType,
                    IsActive = true,
                };
                var password = CommonHelper.GenerateRandonAlphaNumeric();
                var creationStatus = await CreateAsync(user, password);

                if (!creationStatus.Succeeded)
                {
                    throw new Exception(creationStatus.Errors?.FirstOrDefault().Description);
                }

                //you'd need to email the account credentials to the new user...
                try
                {
                    //first get the file
                    var filePath = Path.Combine(_hostingEnvironment.ContentRootPath, CoreConstants.Url.AccountActivationEmail);
                    if (File.Exists(filePath))
                    {
                        var fileString = File.ReadAllText(filePath);
                        if (!string.IsNullOrWhiteSpace(fileString))
                        {
                            var addUserMsg = $"<br/><span>You have been added to the admin page of Ekihire with {model.UserType.ToString()} Role</span>";
                            //fileString = fileString.Replace("{{FirstName}}", $"{model.FirstName}");
                            fileString = fileString.Replace("{{UserName}}", $"{model.UserName}");
                            fileString = fileString.Replace("{{DefaultPassword}}", $"{password}{addUserMsg}");

                            _svcHelper.SendEMail(model.UserName, fileString, "Ekihire.com: You are welcome on board!");
                        }
                    }
                }
                catch (Exception ex)
                {
                    log.Error($"{ex.Message} :: {MethodBase.GetCurrentMethod().Name} :: {ex.StackTrace} ");
                }
                return true;
            }
            catch (Exception ex)
            {
                log.Error($"{ex.Message} :: {MethodBase.GetCurrentMethod().Name} :: {ex.StackTrace} ");
                //return false;
                throw ex;
            }
        }
        
        public async Task<bool> ChangeUserType(UserType userType, long receipientId, string username)
        {
            try
            {
                if (string.IsNullOrEmpty(username))
                {
                    throw new Exception("Could not identify user");
                }
                var loggedInUser = _userRepository.FirstOrDefault(a => a.UserName == username && !a.IsDeleted);
                await ValidateUser(loggedInUser);
                if (!loggedInUser.IsConfirmed())
                {
                    throw new Exception("Your account has not been activated!");
                }
                //if(loggedInUser.UserType != UserType.SuperAdministrator)
                //{
                //    throw new Exception("Only a user  with Super Admin role is allowed to change user type");
                //}
                if (loggedInUser.UserType == UserType.SuperAdministrator)
                {
                    //fine
                }
                else if (loggedInUser.UserType == UserType.Administrator)
                {
                    if (userType == UserType.SuperAdministrator)
                    {
                        throw new Exception("you are not authorized to change user to this role");
                    }
                }
                else
                {
                    throw new Exception("you are not authorized to change user role");
                }
                var receipient = _userRepository.FirstOrDefault(a => a.Id == receipientId && !a.IsDeleted);
                if(receipient == null)
                {
                    throw new Exception("the user you want to edit does not exist!");
                }
                receipient.UserType = userType;
                receipient.LastModificationTime = DateTime.Now;
                receipient.LastModifierUserId = loggedInUser.Id;
                _unitOfWork.BeginTransaction();
                await UpdateAsync(receipient);
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
        public async Task<bool> DeleteUser(long receipientId, string username)
        {
            try
            {
                if (string.IsNullOrEmpty(username))
                {
                    throw new Exception("Could not identify user");
                }
                var loggedInUser = _userRepository.FirstOrDefault(a => a.UserName == username && !a.IsDeleted);
                await ValidateUser(loggedInUser);
                if (!loggedInUser.IsConfirmed())
                {
                    throw new Exception("Your account has not been activated!");
                }
                var receipient = _userRepository.FirstOrDefault(a => a.Id == receipientId && !a.IsDeleted);
                if (receipient == null)
                {
                    throw new Exception("the user you want to edit does not exist!");
                }
                //if (loggedInUser.UserType != UserType.SuperAdministrator)
                //{
                //    throw new Exception("Only a user  with Super Admin role is allowed to change user type");
                //}
                if (loggedInUser.UserType == UserType.SuperAdministrator)
                {
                    //fine
                }
                else if (loggedInUser.UserType == UserType.Administrator)
                {
                    if (receipient.UserType == UserType.SuperAdministrator)
                    {
                        throw new Exception("you are not authorized to delete user with this role");
                    }
                }
                else
                {
                    throw new Exception("you are not authorized to delete user");
                }
                
                receipient.IsDeleted = true;
                receipient.DeleterUserId = loggedInUser.Id;
                receipient.DeletionTime = DateTime.Now;
                receipient.LastModificationTime = DateTime.Now;
                receipient.LastModifierUserId = loggedInUser.Id;
                await UpdateAsync(receipient);
                return true;
            }
            catch (Exception ex)
            {
                _unitOfWork.Rollback();
                log.Error($"{ex.Message} :: {MethodBase.GetCurrentMethod().Name} :: {ex.StackTrace} ");
                return false;
            }
        }
    }
}