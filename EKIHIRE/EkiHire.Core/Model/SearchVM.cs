using EkiHire.Core.Domain.DataTransferObjects;
using EkiHire.Core.Domain.Entities.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EkiHire.Core.Model
{
    public class SearchVM
    {
        public long? AdId { get; set; }
        public string SearchText { get; set; }
        public long? SubcategoryId { get; set; }
        public long? CategoryId { get; set; }
        
        //min_amount, max_amount, locations and properties
        public decimal? min_amount { get; set; }
        public decimal? max_amount { get; set; }
        public long? StateId { get; set; }
        public long? LGAId { get; set; }
        public string Address { get; set; }
        public AdClass? AdClass { get; set; }
        public string PhoneNumber { get; set; }
        //public virtual string Location { get; set; }
        public string AdReference { get; set; }
        public string Description { get; set; }
        public AdsStatus? AdsStatus { get; set; }
        public long? UserId { get; set; }
        public List<string> Keywords { get; set; }
        public List<PropertyValuePair> PropertyValuePairs { get; set; }
    }
    public class PropertyValuePair
    {
        public long PropertyId { get; set; }
        public string Value { get; set; }
    }
}
