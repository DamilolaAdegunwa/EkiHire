using EkiHire.Core.Domain.Entities.Auditing;
using EkiHire.Core.Domain.Entities.Enums;
using System;
using System.Collections.Generic;
using System.Text;
namespace EkiHire.Core.Domain.Entities
{
    public class Account : FullAuditedEntity
    {
        public long Id { get; set; }
        public string Email { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string MiddleName { get; set; }
        public Gender Gender { get; set; }
        public string PhoneNumber { get; set; }
        public string Address { get; set; }

        public string NextOfKin { get; set; }
        public string NextOfKinPhone { get; set; }
        public long RoleId { get; set; }
        public string RoleName { get; set; }
        public string Otp { get; set; }
        public long UserId { get; set; }
        public virtual User User { get; set; }
    }
}