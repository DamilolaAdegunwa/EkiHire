using System;
using System.Collections.Generic;
using System.Text;
using EkiHire.Core.Domain.Entities.Auditing;
using EkiHire.Core.Domain.Entities.Enums;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace EkiHire.Core.Model
{
    #region AddAd request
    public class AddAdRequest
    {
        #region main props
        [DataType(DataType.Text)] [MaxLength(150)] public virtual string Name { get; set; }
        [DataType(DataType.Text)] [MaxLength(150)] public virtual string Address { get; set; }
        public virtual decimal? Amount { get; set; }
        public virtual AdClass? AdClass { get; set; }


        [DataType(DataType.Text)] [MaxLength(150)] public virtual string PhoneNumber { get; set; }
        [DataType(DataType.Text)] [MaxLength(150)] public virtual string Location { get; set; }//coordinates
        [DataType(DataType.Text)] [MaxLength(150)] public virtual string AdReference { get; set; }

        [DataType(DataType.Text)] [MaxLength(150)] public virtual string Description { get; set; }
        [DataType(DataType.Text)] [MaxLength(150)] public virtual string VideoPath { get; set; }
        [DataType(DataType.Text)] [MaxLength(150)] public virtual string Keywords { get; set; }
        public virtual bool? IsActive { get; set; }
        public virtual AdsStatus? AdsStatus { get; set; }
        public virtual long? SubcategoryId { get; set; }
        public virtual long? UserId { get; set; }
        public bool? Negotiable { get; set; }
        public bool? ContactForPrice { get; set; }

        #endregion

        #region other ad properties 
    
        public virtual IEnumerable<AddAd_AdImage> AdImages { get; set; }
    
        public virtual IEnumerable<AddAd_AdPropertyValue> AdPropertyValue { get; set; }
        #endregion
    }
    public class AddAd_AdPropertyValue
    {
        #region AdPropertyValue
        [DataType(DataType.Text)]
        public virtual string Value { get; set; }
        public virtual long? AdId { get; set; }
        public virtual long? AdPropertyId { get; set; }
        #endregion
    }
    public class AddAd_AdImage
    {
        #region ad image properties
        [DataType(DataType.Text)] public virtual string ImagePath { get; set; }
        public virtual long? AdId { get; set; }
        #endregion
    }
    #endregion

}
