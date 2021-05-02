using EkiHire.Core.Configuration;
using EkiHire.Core.Domain.DataTransferObjects;
using EkiHire.Core.Domain.Entities;
using EkiHire.Core.Domain.Entities.Enums;
using EkiHire.Core.Exceptions;
using EkiHire.Core.Messaging.Email;
using EkiHire.Core.Model;
using EkiHire.Core.Utils;
using EkiHire.Data.Repository;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using log4net;
using System.Reflection;
namespace EkiHire.Business.Services
{
    public interface IUserService
    {
        Task<User> FindByNameAsync(string username);
        Task<User> FindByEmailAsync(string email);
        Task<bool> CheckPasswordAsync(User user, string password);
        Task UpdateAsync(User user);
        Task<IList<string>> GetUserRoles(User user);
        Task<IdentityResult> CreateAsync(User user);
        public Task<IdentityResult> CreateAsync(UserDTO user);
        Task<User> FindFirstAsync(Expression<Func<User, bool>> predicate);
        Task<bool> ExistAsync(Expression<Func<User, bool>> predicate);
        Task<IdentityResult> CreateAsync(User user, string password);
        public Task<IdentityResult> CreateAsync(UserDTO user, string password);
        Task<IdentityResult> AddToRoleAsync(User user, string role);
        string HashPassword(User user, string password);
        Task<IList<User>> GetUsersInRoleAsync(string role);
        Task<string> GenerateEmailConfirmationTokenAsync(User user);
        Task<UserDTO> ActivateAccount(string usernameOrEmail, string activationCode);
        Task<UserDTO> GetProfile(string username);
        Task<bool> ForgotPassword(string usernameOrEmail);
        Task<bool> ResetPassword(PassordResetDTO model);
        Task<bool> ChangePassword(string userName, ChangePassordDTO model);
        Task<IList<string>> GetUserRolesAsync(string username);
        Task<bool> UpdateProfile(string userName, UserProfileDTO model);
        Task<bool> IsInRoleAsync(User user, string role);
        Task<IdentityResult> RemoveFromRolesAsync(User user, IEnumerable<string> roles);
        Task<bool> ResendVerificationCode(string username);
        Task<bool> TestEmail();
        Task<bool> ValidatePassordResetCode(PassordResetDTO model);
        Task<bool> ChangeName(Name name, string username);
        Task<bool> ChangeGender(Gender gender, string username);
        Task<bool> ChangeBirthday(DateTime birthdate, string username);
        Task<bool> ChangeEmail(string userEmail, string username);
        Task<bool> ChangePhoneNumber(string userPhoneNumber, string username);
        //Task<IDictionary<DateTime, List<PostDTO>>> PostTimeGraph();
    }

    public class UserService : IUserService
    {

        private readonly UserManager<User> _userManager;
        private readonly AppConfig appConfig;
        private readonly IRepository<User> _userRepository;
        private readonly IServiceHelper _svcHelper;
        private readonly IMailService _mailSvc;
        private readonly IHostingEnvironment _hostingEnvironment;
        private readonly ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().Name);
        public UserService(
            UserManager<User> userManager, 
            IServiceHelper svcHelper,
            IRepository<User> userRepository,
            IMailService mailSvc,
            IHostingEnvironment hostingEnvironment,
            IOptions<AppConfig> _appConfig)
        {
            _userManager = userManager;
            _svcHelper = svcHelper;
            _mailSvc = mailSvc;
            appConfig = _appConfig.Value;
            _hostingEnvironment = hostingEnvironment;
            _userRepository = userRepository;
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
        public Task<IdentityResult> CreateAsync(UserDTO user)
        {
            return _userManager.CreateAsync(user);
        }
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
        public Task<IdentityResult> CreateAsync(UserDTO user, string password)
        {
            return _userManager.CreateAsync(user);
        }
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
        public async Task<UserDTO> ActivateAccount(string username, string activationCode)
        {
            if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(activationCode))
            {
                throw await _svcHelper.GetExceptionAsync(ErrorConstants.USER_ACCOUNT_NOT_EXIST);
            }

            var user = await FindByNameAsync(username);

            await ValidateUser(user);
            UserDTO userDto = new UserDTO();
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
                throw new EkiHireGenericException("Invalid OTP");
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

        public async Task<UserDTO> GetProfile(string username)
        {
            if (string.IsNullOrEmpty(username))
            {
                throw await _svcHelper.GetExceptionAsync(ErrorConstants.USER_ACCOUNT_NOT_EXIST);
            }

            var user = await FindByNameAsync(username);

            await ValidateUser(user);

            return user is null ? null : new UserDTO
            {
                Email = user.Email,
                FirstName = user.FirstName,
                LastName = user.LastName,
                Gender = user.Gender,
                NextOfKinName = user.NextOfKinName,
                NextOfKinPhone = user.NextOfKinPhone,
                PhoneNumber = user.PhoneNumber,
                ReferralCode = user.ReferralCode,
                Address = user.Address, 
                MiddleName = user.MiddleName,
                CreationTime = user.CreationTime,
                DateOfBirth = user.DateOfBirth,
                SubscriptionPlanType = user.SubscriptionPlanType
            };
        }

        public async Task<bool> ForgotPassword(string usernameOrEmail)
        {
            if (string.IsNullOrEmpty(usernameOrEmail))
            {
                throw await _svcHelper.GetExceptionAsync(ErrorConstants.USER_ACCOUNT_NOT_EXIST);
            }

            var user = await FindByNameAsync(usernameOrEmail);

            ValidateUser(user);

            user.OTP = CommonHelper.RandomDigits(5);
            await _userManager.UpdateAsync(user);

            var replacement = new StringDictionary
            {
                ["FirstName"] = user.UserName,
                ["Otp"] = user.OTP
            };

            var mail = new Mail(appConfig.AppEmail, "EkiHire.com: Password Reset OTP", user.Email)
            {
                BodyIsFile = true,
                BodyPath = Path.Combine(_hostingEnvironment.ContentRootPath, CoreConstants.Url.PasswordResetEmail)
            };

            await _mailSvc.SendMailAsync(mail, replacement);

            return await Task.FromResult(true);
        }

        public async Task<bool> ValidatePassordResetCode(PassordResetDTO model)
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

        public async Task<bool> ResetPassword(PassordResetDTO model)
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

        public async Task<bool> ChangePassword(string username, ChangePassordDTO model)
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
                    var replacement = new StringDictionary
                    {
                        //["FirstName"] = user.FirstName,
                        ["ActivationCode"] = user.AccountConfirmationCode
                    };

                    var mail = new Mail(appConfig.AppEmail, "EkiHire.com: Account Verification Code", user.Email)
                    {
                        BodyIsFile = true,
                        BodyPath = Path.Combine(_hostingEnvironment.ContentRootPath, CoreConstants.Url.ActivationCodeEmail)
                    };

                    await _mailSvc.SendMailAsync(mail, replacement);
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
		public async Task<bool> TestEmail()
        {
			try{
				var replacement = new StringDictionary
                {
                    //["FirstName"] = "Damilola Adegunwa",
                    ["ActivationCode"] = "FAKE123456"
                };

                var mail = new Mail(appConfig.AppEmail, "EkiHire.com: Account Verification Code", "damee1993@gmail.com")
                {
                    BodyIsFile = true,
                    BodyPath = Path.Combine(_hostingEnvironment.ContentRootPath, CoreConstants.Url.ActivationCodeEmail)
                };

                await _mailSvc.SendMailAsync(mail, replacement);
                return true;
			}catch(Exception e){
				return false;
			}
        }

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
    }
}
