﻿using EkiHire.Core.Domain.Entities.Auditing;
using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel.DataAnnotations;
using EkiHire.Core.Domain.Entities.Enums;
using EkiHire.Core.Domain.DataTransferObjects;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using EkiHire.Core.Domain.Extensions;
using Microsoft.AspNetCore.Http;
using EkiHire.Core.Domain.Entities;
using System.IO;
using Microsoft.AspNetCore.Http.Internal;

namespace EkiHire.Core.Domain.DataTransferObjects
{
    public class JobApplicationDTO : EntityDTO<long>
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
        public virtual string ResumePath { get; set; }
        #region contact details
        public virtual string JobTitle { get; set; }
        public virtual string ContactPhoneNumber { get; set; }
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
        public string CompanyEmail { get; set; }
        #endregion

        #region other properties

        #endregion
        public IFormFile ConvertByteArrayToIFormFile(byte[] byteArray)
        {
            var stream = new MemoryStream(byteArray);
            var formFile = new FormFile(stream, 0, byteArray.Length, "name", "fileName");
            return formFile;
        }
        #region implicit casting between entity & dto 
        public static implicit operator JobApplicationDTO(JobApplication model)
        {
            try
            {
                if (model == null)
                {
                    return null;
                }
                //var resume = new JobApplicationDTO().ConvertByteArrayToIFormFile(model.Resume);
                JobApplicationDTO response = new JobApplicationDTO
                {
                    ResumePath = model.ResumePath,
                    JobTitle = model.JobTitle,
                    ContactPhoneNumber = model.ContactPhoneNumber,
                    Address = model.Address,
                    Place = model.Place,
                    Region = model.Region,
                    Age = model.Age,
                    Certification = model.Certification,
                    EducationDetails = model.EducationDetails,
                    EmploymentStatus = model.EmploymentStatus,
                    ExpectedSalary = model.ExpectedSalary,
                    Gender = model.Gender,
                    HighestLevelOfEducation = model.HighestLevelOfEducation,
                    Id = model.Id,
                    JobType = model.JobType,
                    Skills = model.Skills,
                    SaveMyData = model.SaveMyData,
                    PreviousWorkExperiences = null,
                    CompanyEmail = model.CompanyEmail,
                    FullName = model.FullName,
                };
                return response;
            }
            catch (Exception ex)
            {
                return null;
                //throw;
            }
        }
        #endregion
    }
}
