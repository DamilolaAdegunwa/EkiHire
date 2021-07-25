using EkiHire.Core.Domain.Entities.Auditing;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.IO;

namespace EkiHire.Core.Domain.Entities
{
    public class JobApplication : FullAuditedEntity
    {
        #region properties
        //public virtual long? CategoryId { get; set; } //Job
        //public virtual long? CategoryName { get; set; } //Job
        //public virtual long? SubcategoryId { get; set; }
        //public virtual long? SubcategoryName { get; set; }
        //public virtual string CompanyName { get; set; }
        //public virtual string Currency { get; set; }
        //public virtual decimal? PriceLower { get; set; }
        //public virtual decimal? PriceUpper { get; set; }
        //[ForeignKey("UserId")]
        //public virtual long? UserId { get; set; }
        //[NotMapped]
        //public virtual User User { get; set; }
        //[ForeignKey("AdId")]
        //public virtual long? AdId { get; set; }
        //public virtual Ad Ad { get; set; }
        //public virtual IFormFile Resume { get; set; }
        //public virtual byte[] Resume { get; set; }
        public virtual string ResumePath { get; set; }
        #region contact details
        public virtual string JobTitle { get; set; }
        public virtual string ContactPhoneNumber { get; set; }
        public virtual string ContactEmail { get; set; }
        public virtual string Region { get; set; }//statename
        public virtual string Place { get; set; }
        public virtual string Address { get; set; }

        #endregion

        #region Personal Details
        public virtual string JobType { get; set; }//JobType
        public virtual string EmploymentStatus { get; set; }//EmploymentStatus
        public virtual string Gender { get; set; }
        public virtual int Age { get; set; }
        public virtual string Skills { get; set; }
        public virtual string ExpectedSalary { get; set; }
        //[NotMapped]
        public virtual IEnumerable<PreviousWorkExperience> PreviousWorkExperiences { get; set; }
        public virtual string EducationDetails { get; set; }
        public virtual string HighestLevelOfEducation { get; set; }
        public virtual string Certification { get; set; }
        public virtual bool? SaveMyData { get; set; }
        public virtual string FullName { get; set; }

        #endregion
        public virtual string CompanyEmail { get; set; }
        #endregion

        #region other properties

        #endregion
        public byte[] ConvertIFormFileToByteArray(IFormFile file)
        {
            byte[] fileBytes = null;
            //foreach (var file in files)
            {
                if (file.Length > 0)
                {
                    using (var ms = new MemoryStream())
                    {
                        file.CopyTo(ms);
                        fileBytes = ms.ToArray();
                        string s = Convert.ToBase64String(fileBytes);
                        // act on the Base64 data
                    }
                }
            }
            return fileBytes;
        }
        //#region implicit casting between entity & dto 
        //public static implicit operator JobApplication(JobApplicationDTO model)
        //{
        //    try
        //    {
        //        if (model == null)
        //        {
        //            return null;
        //        }
        //        //var resume = new JobApplication().ConvertIFormFileToByteArray(model.Resume);
        //        JobApplication response = new JobApplication {
        //            ResumePath = model.ResumePath,
        //            JobTitle = model.JobTitle,
        //            ContactPhoneNumber = model.ContactPhoneNumber,
        //            Address = model.Address,
        //            Place = model.Place,
        //            Region = model.Region,
        //            Age = model.Age,
        //            Certification = model.Certification,
        //            EducationDetails = model.EducationDetails,
        //            EmploymentStatus = model.EmploymentStatus,
        //            ExpectedSalary = model.ExpectedSalary,
        //            Gender = model.Gender,
        //            HighestLevelOfEducation = model.HighestLevelOfEducation,
        //            Id = model.Id,
        //            JobType = model.JobType,
        //            Skills = model.Skills, 
        //            SaveMyData = model.SaveMyData,
        //            PreviousWorkExperiences = null,
        //            CompanyEmail = model.CompanyEmail,
        //            FullName = model.FullName,
        //        };
        //        return response;
        //    }
        //    catch (Exception ex)
        //    {
        //        return null;
        //        //throw;
        //    }
        //}
        //#endregion
    }
}
