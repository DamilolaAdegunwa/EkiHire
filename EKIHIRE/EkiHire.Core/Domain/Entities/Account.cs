//using EkiHire.Core.Domain.Entities.Auditing;
//using System.ComponentModel.DataAnnotations;
//using System.ComponentModel.DataAnnotations.Schema;
//namespace EkiHire.Core.Domain.Entities
//{
//    public class Account : FullAuditedEntity
//    {
//        [DataType(DataType.Text)]
//        public virtual string Email { get; set; }
//        [DataType(DataType.Text)]
//        public virtual string Otp { get; set; }
//        [ForeignKey("UserId")]
//        public virtual long? UserId { get; set; }
//        public virtual User User { get; set; }
//        #region comments
//        //public string FirstName { get; set; }
//        //public string LastName { get; set; }
//        //public string MiddleName { get; set; }
//        //public Gender Gender { get; set; }
//        //public string PhoneNumber { get; set; }
//        //public string Address { get; set; }
//        //public string NextOfKin { get; set; }
//        //public string NextOfKinPhone { get; set; }
//        //public long RoleId { get; set; }
//        //public string RoleName { get; set; }
//        //public string Email { get; set; }
//        //public long Id { get; set; }
//        //public string FirstName { get; set; }
//        //public string LastName { get; set; }
//        //public string EmployeeCode { get; set; }
//        //public DateTime? DateOfEmployment { get; set; }
//        //public string MiddleName { get; set; }
//        //public Gender Gender { get; set; }
//        //public string PhoneNumber { get; set; }
//        //public string Address { get; set; }
//        //public string EmployeePhoto { get; set; }
//        //public string NextOfKin { get; set; }
//        //public string NextOfKinPhone { get; set; }
//        //public long RoleId { get; set; }
//        //public string RoleName { get; set; }
//        //public string Otp { get; set; }
//        ////public long UserId { get; set; }
//        //public virtual User User { get; set; }
//        ////public string UserName { get; set; }
//        ////other dto-related properties
//        //public string FullName => FirstName + " " + LastName;
//        //public string Password { get; set; }
//        #endregion
//    }
//}