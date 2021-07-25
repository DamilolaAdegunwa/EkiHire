using EkiHire.Core.Domain.Entities.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace EkiHire.Core.Domain.Model
{
    public class PassordReset
    {
        public string UserName { get; set; }
        public string Code { get; set; }
        public string NewPassword { get; set; }
    }


    public class ChangePassord
    {
        public string CurrentPassword { get; set; }
        public string NewPassword { get; set; }
    }

    public class ProfileImage
    {
        public string profileImageString { get; set; }
    }
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
        public SubscriptionPlan SubscriptionPlan { get; set; }
    }
}
