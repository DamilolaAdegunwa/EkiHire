using EkiHire.Core.Domain.Entities.Enums;
using EkiHire.Core.Utils;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using EkiHire.Core.Domain.Entities.Auditing;
using EkiHire.Core.Domain.Entities.Common;
using EkiHire.Core.Domain.DataTransferObjects;
using System.ComponentModel.DataAnnotations.Schema;
using EkiHire.Core.Domain.Extensions;
namespace EkiHire.Core.Domain.Entities
{
    public class User : IdentityUser<long>, IFullAudited, IAudited, ICreationAudited, IHasCreationTime, IHasDeletionTime, ISoftDelete, IHasModificationTime, IEntity
    {
        #region all user property
        public string FirstName { get; set; }
        public string MiddleName { get; set; }
        public string LastName { get; set; }
        public string Address { get; set; }
        public bool IsFirstTimeLogin { get; set; }
        public string OptionalPhoneNumber { get; set; }
        public UserType UserType { get; set; }
        public string ImagePath { get; set; }
        public DateTime CreationTime { get; set; }
        public DateTime? DeletionTime { get; set; }
        public bool IsDeleted { get; set; }
        public string RefreshToken { get; set; }

        public string Title { get; set; }
        public string DeviceToken { get; set; }
        public string Referrer { get; set; }
        public string ReferralCode { get; set; }
        public string NextOfKinName { get; set; }
        public string NextOfKinPhone { get; set; }
        public DeviceType LoginDeviceType { get; set; }
        //public int? WalletId { get; set; }
        public virtual Wallet Wallet { get; set; }
        public Gender Gender { get; set; }
        public DateTime? DateOfBirth { get; set; }
        public DateTime? LastModificationTime { get; set; }
        public string AccountConfirmationCode { get; set; }
        public string OTP { get; set; }
        [NotMapped]
        public IEnumerable<UserRole> UserRoles { get; set; }
        //public ICollection<Post> Posts { get; set; }
        //added
        public bool IsActive { get; set; }
        //public long UserId { get; set; }
        public bool AccountIsDeleted { get; set; }
        public Enums.SubscriptionPlan SubscriptionPlan { get; set; }
        [NotMapped]
        public IEnumerable<CartItem> CartItems { get; set; }
        public long? CreatorUserId { get; set; }
        public long? LastModifierUserId { get; set; }
        public long? DeleterUserId { get; set; }
        [NotMapped]
        public virtual ICollection<Message> SentMessages { get; set; }
        [NotMapped]
        public virtual ICollection<Message> ReceivedMessages { get; set; }
        [NotMapped]
        public virtual ICollection<Notification> Notifications { get; set; }
        #endregion
        public bool IsTransient()
        {
            if (EqualityComparer<long>.Default.Equals(Id, default(int)))
            {
                return true;
            }

            //Workaround for EF Core since it sets int/long to min value when attaching to dbcontext
            if (typeof(int) == typeof(int))
            {
                return Convert.ToInt32(Id) <= 0;
            }

            if (typeof(int) == typeof(long))
            {
                return Convert.ToInt64(Id) <= 0;
            }

            return false;
        }

        //implicit conversion
        public static implicit operator User(UserDTO user)
        {
            if (user != null)
            {
                var userDto = new User
                {
                    AccessFailedCount = user.AccessFailedCount,
                    AccountConfirmationCode = user.AccountConfirmationCode,
                    AccountIsDeleted = user.AccountIsDeleted,
                    Address = user.Address,
                    ConcurrencyStamp = user.ConcurrencyStamp,
                    CreationTime = user.CreationTime,
                    DateOfBirth = user.DateOfBirth,
                    DeletionTime = user.DeletionTime,
                    DeviceToken = user.DeviceToken,
                    Email = user.Email,
                    IsActive = user.IsActive,
                    EmailConfirmed = user.EmailConfirmed,
                    FirstName = user.FirstName,
                    Gender = user.Gender,
                    Id = user.Id,
                    ImagePath = user.ImagePath,
                    IsDeleted = user.IsDeleted,
                    IsFirstTimeLogin = user.IsFirstTimeLogin,
                    LastModificationTime = user.LastModificationTime,
                    LastName = user.LastName,
                    LockoutEnabled = user.LockoutEnabled,
                    LockoutEnd = user.LockoutEnd,
                    LoginDeviceType = user.LoginDeviceType,
                    MiddleName = user.MiddleName,
                    NextOfKinName = user.NextOfKinName,
                    NextOfKinPhone = user.NextOfKinPhone,
                    NormalizedEmail = user.NormalizedEmail,
                    NormalizedUserName = user.NormalizedUserName,
                    OptionalPhoneNumber = user.OptionalPhoneNumber,
                    OTP = user.OTP,
                    PasswordHash = user.PasswordHash,
                    PhoneNumber = user.PhoneNumber,
                    PhoneNumberConfirmed = user.PhoneNumberConfirmed,
                    ReferralCode = user.ReferralCode,
                    Referrer = user.Referrer,
                    RefreshToken = user.RefreshToken,
                    SecurityStamp = user.SecurityStamp,
                    Title = user.Title,
                    TwoFactorEnabled = user.TwoFactorEnabled,
                    UserName = user.UserName,
                    UserRoles = user.UserRoles,
                    UserType = user.UserType,
                    Wallet = user.Wallet,
                    //WalletId = user.WalletId,
                    CartItems = user.CartItems.ToEntity(),
                    SubscriptionPlan = user.SubscriptionPlan,
                    
                };
                return userDto;
            }
            return null;
        }
    }

    public static class UserExtensions
    {
        public static bool IsDefaultAccount(this User user)
        {
            return CoreConstants.DefaultAccount == user.UserName;
        }

        public static bool IsNull(this User user)
        {
            return user == null;
        }

        public static bool IsConfirmed(this User user)
        {
            return user.EmailConfirmed || user.PhoneNumberConfirmed;
        }

        public static bool AccountLocked(this User user)
        {
            return user.LockoutEnabled == true;
        }

        public static bool HasNoPassword(this User user)
        {
            return string.IsNullOrWhiteSpace(user.PasswordHash);
        }
    }
}