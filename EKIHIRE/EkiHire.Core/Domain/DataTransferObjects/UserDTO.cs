using EkiHire.Core.Domain.Entities.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using EkiHire.Core.Utils;
using Microsoft.AspNetCore.Identity;
using EkiHire.Core.Domain.Entities.Auditing;
using EkiHire.Core.Domain.Entities.Common;
using EkiHire.Core.Domain.Entities;

namespace EkiHire.Core.Domain.DataTransferObjects
{
    public class UserDTO : IdentityUser<int>, IHasCreationTime, IHasDeletionTime, ISoftDelete, IHasModificationTime
    {
        public string FirstName { get; set; }
        public string MiddleName { get; set; }
        public string LastName { get; set; }
        public string Address { get; set; }
        public bool IsFirstTimeLogin { get; set; }
        public string OptionalPhoneNumber { get; set; }
        public UserType UserType { get; set; }
        public string Image { get; set; }
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
        public int? WalletId { get; set; }
        public virtual Wallet Wallet { get; set; }
        public Gender Gender { get; set; }
        public string DateOfBirth { get; set; }
        public DateTime? LastModificationTime { get; set; }
        public string AccountConfirmationCode { get; set; }
        public string Photo { get; set; }
        public string OTP { get; set; }
        public ICollection<UserRole> UserRoles { get; set; }
        //added
        public bool IsActive { get; set; }
        public long UserId { get; set; }
        public bool AccountIsDeleted { get; set; }

        //implicit conversion
        public static implicit operator UserDTO(User user)
        {
            if (user != null)
            {
                var userDto = new UserDTO
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
                    Image = user.Image,
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
                    Photo = user.Photo,
                    ReferralCode = user.ReferralCode,
                    Referrer = user.Referrer,
                    RefreshToken = user.RefreshToken,
                    SecurityStamp = user.SecurityStamp,
                    Title = user.Title,
                    TwoFactorEnabled = user.TwoFactorEnabled,
                    UserId = user.UserId,
                    UserName = user.UserName,
                    UserRoles = user.UserRoles,
                    UserType = user.UserType,
                    Wallet = user.Wallet,
                    WalletId = user.WalletId,
                    
                };
                return userDto;
            }
            return null;
        }
    }
    //public class UserDTO
    //{
    //    public long UserId { get; set; }

    //    public bool IsActive { get; set; }

    //    public UserType UserType { get; set; }

    //    [StringLength(50, MinimumLength = 1, ErrorMessage = "{0} must be between {1} and {2} characters long.")]
    //    [Display(Name = "First Name")]
    //    [Required]
    //    public string FirstName { get; set; }

    //    [StringLength(50, MinimumLength = 1, ErrorMessage = "{0} must be between {1} and {2} characters long.")]
    //    [Display(Name = "Last Name")]
    //    [Required]
    //    public string LastName { get; set; }

    //    [Required(ErrorMessage = "Please specify a valid email.")]
    //    [EmailAddress(ErrorMessage = "Please specify a valid email.")]
    //    public string Email { get; set; }

    //    [StringLength(20, MinimumLength = 8, ErrorMessage = "{0} must be between {1} and {2} characters long.")]
    //    [Display(Name = "Phone Number")]
    //    public string PhoneNumber { get; set; }
    //    public string Image { get; set; }
    //    public List<string> Roles { get; set; }
    //    public bool AccountIsDeleted { get; set; }
    //    public Gender Gender { get; set; }
    //    public string ReferralCode { get; set; }
    //}

    public class UserProfileDTO
    {
        public string NextOfKin { get; set; }

        public string NextOfKinPhone { get; set; }
        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string Email { get; set; }

        public string PhoneNumber { get; set; }
        public string Gender { get; set; }
        public string ReferralCode { get; set; }
        public string Address { get; set; }
        public string MiddleName { get; set; }
        public string DateJoined { get; set; }
        public string DateOfBirth { get; set; }
        public string Title { get; set; }
    }
}
