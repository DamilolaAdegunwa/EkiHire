﻿using EkiHire.Core.Domain.Entities.Auditing;
using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel.DataAnnotations;
using EkiHire.Core.Domain.Entities.Enums;

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
        public string Location { get; set; }
        public ICollection<AdItem> AdItems { get; set; }
        //specifics
        public long? Room { get; set; }
        public string Furniture { get; set; }
        public string Parking { get; set; }
        public long? Bedroom { get; set; }
        public int? Bathroom { get; set; }
        //
        public string LandType { get; set; }//residentiial land, commercial
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

        #endregion
    }
}
