using EkiHire.Core.Domain.Entities.Auditing;
using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel.DataAnnotations;
using EkiHire.Core.Domain.Entities.Enums;
using EkiHire.Core.Domain.DataTransferObjects;

namespace EkiHire.Core.Domain.Entities
{
    public class Ad: FullAuditedEntity
    {
        #region all ad properties
        [DataType(DataType.Text)]
        public string Name { get; set; }
        public string VideoPath { get; set; }
        public decimal? Amount { get; set; }
        public AdClass? AdClass { get; set; }
        public AdsStatus? AdsStatus { get; set; }
        public ICollection<AdImage> AdImages { get; set; }
        public Subcategory Subcategory { get; set; }
        public string Keywords { get; set; }
        public string Location { get; set; }//coordinates
        public bool? IsActive { get; set; }
        //public ICollection<AdItem> AdItems { get; set; }
        //specifics
        public long? Room { get; set; }
        public string Furniture { get; set; }
        public string Parking { get; set; }
        public long? Bedroom { get; set; }
        public long? Bathroom { get; set; }
        //
        public string LandType { get; set; }//residential land, commercial
        public decimal? SquareMeters { get; set; }
        public string ExchangePossible { get; set; }
        public string BrokerFee { get; set; }
        //
        public string Condition { get; set; }//Brand New
        public string Quality { get; set; }//Standard
        //LandType, Condition, Quality, BrokerFee
        public string CompanyName { get; set; }
        public string ServiceArea { get; set; }
        public string ServiceFeature { get; set; }
        public string TypeOfService { get; set; }//inspection, Repair
        public string Topic { get; set; }
        //job
        public string Requirements { get; set; }
        public string ResumePath { get; set; }
        public string Title { get; set; }
        public string PhoneNumber { get; set; }
        public string Region { get; set; }
        public string Place { get; set; }
        public string Address { get; set; }
        public string JobType { get; set; }
        public string EmploymentStatus { get; set; }
        public string Gender { get; set; }
        public long? Age { get; set; }
        public string Skills { get; set; }
        public string ExpectedSalary { get; set; }
        public string Education { get; set; }
        public string HighestLevelOfEducation { get; set; }
        public string Certification { get; set; }
        public bool SaveData { get; set; } = true;
        public ICollection<WorkExperience> WorkExperiences { get; set; }
        //cars
        public string Maker { get; set; }
        public string Year { get; set; }
        public string Color { get; set; }
        public long? Seats { get; set; }
        public string CarType { get; set; }
        public string FuelType { get; set; }
        public string Mileage { get; set; }
        //
        public User User { get; set; }

        #endregion

        public static implicit operator Ad(AdDTO model)
        {
            if (model != null)
            {
                Ad output = new Ad
                {
                    Name = model.Name,
                    AdClass = model.AdClass,
                    Address = model.Address,
                    AdImages = model.AdImages,
                    //AdItems = model.AdItems,
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
                    Subcategory = model.Subcategory,
                    Title = model.Title,
                    Topic = model.Topic,
                    TypeOfService = model.TypeOfService,
                    VideoPath = model.VideoPath,
                    WorkExperiences = model.WorkExperiences,
                    Year = model.Year,
                    User = model.User
                };
                return output;
            }
            return null;
        }
    }
}
