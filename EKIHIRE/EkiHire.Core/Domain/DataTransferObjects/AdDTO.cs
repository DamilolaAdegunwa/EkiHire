using EkiHire.Core.Domain.Entities.Auditing;
using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel.DataAnnotations;
using EkiHire.Core.Domain.Entities.Enums;
using EkiHire.Core.Domain.Entities;

namespace EkiHire.Core.Domain.DataTransferObjects
{
    public class AdDTO : EntityDTO<long>
    {
        #region all ad properties
        [DataType(DataType.Text)] public virtual string Name { get; set; }

        [DataType(DataType.Text)] public virtual string VideoPath { get; set; }
        public virtual decimal? Amount { get; set; }
        public virtual AdClass? AdClass { get; set; }
        public virtual AdsStatus? AdsStatus { get; set; }
        //public virtual ICollection<AdImage> AdImages { get; set; }
        //[ForeignKey("SubcategoryId")]
        public virtual long? SubcategoryId { get; set; }
        public virtual SubcategoryDTO Subcategory { get; set; }
        [DataType(DataType.Text)] public virtual string Keywords { get; set; }
        [DataType(DataType.Text)] public virtual string Location { get; set; }//coordinates
        public virtual bool? IsActive { get; set; }
        //public virtual ICollection<AdItem> AdItems { get; set; }
        //public virtual ICollection<AdPropertyValue> AdPropertyValue { get; set; }
        //specifics
        public virtual long? Room { get; set; }
        [DataType(DataType.Text)] public virtual string Furniture { get; set; }
        [DataType(DataType.Text)] public virtual string Parking { get; set; }
        public virtual long? Bedroom { get; set; }
        public virtual long? Bathroom { get; set; }
        //
        [DataType(DataType.Text)] public virtual string LandType { get; set; }//residential land, commercial
        public virtual decimal? SquareMeters { get; set; }
        [DataType(DataType.Text)] public virtual string ExchangePossible { get; set; }
        [DataType(DataType.Text)] public virtual string BrokerFee { get; set; }
        //
        [DataType(DataType.Text)] public virtual string Condition { get; set; }//Brand New
        [DataType(DataType.Text)] public virtual string Quality { get; set; }//Standard
        //LandType, Condition, Quality, BrokerFee
        [DataType(DataType.Text)] public virtual string CompanyName { get; set; }
        [DataType(DataType.Text)] public virtual string ServiceArea { get; set; }
        [DataType(DataType.Text)] public virtual string ServiceFeature { get; set; }
        [DataType(DataType.Text)] public virtual string TypeOfService { get; set; }//inspection, Repair
        [DataType(DataType.Text)] public virtual string Topic { get; set; }
        //job
        [DataType(DataType.Text)] public virtual string Requirements { get; set; }
        [DataType(DataType.Text)] public virtual string ResumePath { get; set; }
        [DataType(DataType.Text)] public virtual string Title { get; set; }
        [DataType(DataType.Text)] public virtual string PhoneNumber { get; set; }
        [DataType(DataType.Text)] public virtual string Region { get; set; }
        [DataType(DataType.Text)] public virtual string Place { get; set; }
        [DataType(DataType.Text)] public virtual string Address { get; set; }
        [DataType(DataType.Text)] public virtual string JobType { get; set; }
        [DataType(DataType.Text)] public virtual string EmploymentStatus { get; set; }
        [DataType(DataType.Text)] public virtual string Gender { get; set; }
        public virtual long? Age { get; set; }
        [DataType(DataType.Text)] public virtual string Skills { get; set; }
        [DataType(DataType.Text)] public virtual string ExpectedSalary { get; set; }
        [DataType(DataType.Text)] public virtual string Education { get; set; }
        [DataType(DataType.Text)] public virtual string HighestLevelOfEducation { get; set; }
        [DataType(DataType.Text)] public virtual string Certification { get; set; }
        public virtual bool SaveData { get; set; } = true;
        //public virtual ICollection<WorkExperience> WorkExperiences { get; set; }
        //cars
        [DataType(DataType.Text)] public virtual string Maker { get; set; }
        [DataType(DataType.Text)] public virtual string Year { get; set; }
        [DataType(DataType.Text)] public virtual string Color { get; set; }
        public virtual long? Seats { get; set; }
        [DataType(DataType.Text)] public virtual string CarType { get; set; }
        [DataType(DataType.Text)] public virtual string FuelType { get; set; }
        [DataType(DataType.Text)] public virtual string Mileage { get; set; }
        //
        //[ForeignKey("UserId")]
        public virtual long? UserId { get; set; }
        public virtual User User { get; set; }

        #endregion

        #region ad dto
        public virtual ICollection<AdImage> AdImages { get; set; }
        public virtual ICollection<AdPropertyValue> AdPropertyValue { get; set; }
        #endregion

        public static implicit operator AdDTO(Ad model)
        {
            if (model != null)
            {
                var dto = new AdDTO
                {
                    
                    Id = model.Id,
                    Name = model.Name,
                    AdClass = model.AdClass,
                    Address = model.Address,
                    //AdImages = model.AdImages,
                    //AdItems = model.AdItems,
                    //AdPropertyValue = model.AdPropertyValue,
                    AdsStatus = model.AdsStatus,
                    Age = model.Age,
                    Amount = model.Amount,
                    ServiceArea = model.ServiceArea,
                    Bathroom = model.Bathroom,
                    Bedroom = model.Bedroom,
                    BrokerFee = model.BrokerFee,
                    CarType = model.CarType,
                    Certification = model.Certification,
                    Color = model.Color,
                    CompanyName = model.CompanyName,
                    Condition = model.Condition,
                    Education = model.Education,
                    EmploymentStatus = model.EmploymentStatus,
                    ExchangePossible = model.ExchangePossible,
                    ExpectedSalary = model.ExpectedSalary,
                    FuelType = model.FuelType,
                    Furniture = model.Furniture,
                    Gender = model.Gender,
                    HighestLevelOfEducation = model.HighestLevelOfEducation,
                    JobType = model.JobType,
                    Keywords = model.Keywords,
                    LandType = model.LandType,
                    Location = model.Location,
                    Maker = model.Maker,
                    Mileage = model.Mileage,
                    Parking = model.Parking,
                    PhoneNumber = model.PhoneNumber,
                    Place = model.Place,
                    Quality = model.Quality,
                    Region = model.Region,
                    Requirements = model.Requirements,
                    ResumePath = model.ResumePath,
                    Room = model.Room,
                    SaveData = model.SaveData,
                    Seats = model.Seats,
                    ServiceFeature = model.ServiceFeature,
                    Skills = model.Skills,
                    SquareMeters = model.SquareMeters,
                    //Subcategory = model.Subcategory,
                    SubcategoryId = model.SubcategoryId,
                    Title = model.Title,
                    Topic = model.Topic,
                    TypeOfService = model.TypeOfService,
                    VideoPath = model.VideoPath,
                    //WorkExperiences = model.WorkExperiences,
                    Year = model.Year,
                    UserId = model.UserId,
                    //User = model.User
                };
                return dto;
            }
            return null;
        }
    }
}
