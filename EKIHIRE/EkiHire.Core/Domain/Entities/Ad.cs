using EkiHire.Core.Domain.Entities.Auditing;
using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel.DataAnnotations;
using EkiHire.Core.Domain.Entities.Enums;

namespace EkiHire.Core.Domain.Entities
{
    public class Ad: FullAuditedEntity
    {
        [DataType(DataType.Text)]
        public string Name { get; set; }
        public string VideoPath { get; set; }
        public decimal? Amount { get; set; }
        public AdClass? AdClass { get; set; }
        public ICollection<AdImage> AdImages { get; set; }
        public Subcategory Subcategory { get; set; }
        public string Keywords { get; set; }
        public string Location { get; set; }
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
        public  { get; set; }
    }
}
