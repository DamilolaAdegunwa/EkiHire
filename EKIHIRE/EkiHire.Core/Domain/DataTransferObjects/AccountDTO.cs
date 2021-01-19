using EkiHire.Core.Domain.Entities;
using EkiHire.Core.Domain.Entities.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace EkiHire.Core.Domain.DataTransferObjects
{
    public class AccountDTO
    {
        public string Email { get; set; }
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string EmployeeCode { get; set; }
        public DateTime? DateOfEmployment { get; set; }
        public string FullName => FirstName + " " + LastName;
        public string MiddleName { get; set; }
        public Gender Gender { get; set; }
        public string PhoneNumber { get; set; }
        public string Address { get; set; }
        public string EmployeePhoto { get; set; }
        public string NextOfKin { get; set; }
        public string NextOfKinPhone { get; set; }
        public int RoleId { get; set; }
        public string RoleName { get; set; }
        public string Otp { get; set; }
        public int UserId { get; set; }
        public virtual User User { get; set; }
        public string UserName { get; set; }
    }
}
