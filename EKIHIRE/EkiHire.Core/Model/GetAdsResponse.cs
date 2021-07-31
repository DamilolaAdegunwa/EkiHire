using EkiHire.Core.Domain.Entities;
using EkiHire.Core.Domain.Entities.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace EkiHire.Core.Model
{
    public class GetAdsResponse
    {
        public List<Ad> Ads { get; set; }
        public long Total { get; set; }
        public long Pages { get; set; }
        public long Page { get; set; }
        public long Size { get; set; }
    }
    #region ad response
    public class GetAdsAd
    {
        #region all ad properties
        [DataType(DataType.Text)] [MaxLength(150)] public virtual string Name { get; set; }
        [DataType(DataType.Text)] [MaxLength(150)] public virtual string Address { get; set; }
        public virtual decimal? Amount { get; set; }
        public virtual AdClass? AdClass { get; set; }


        [DataType(DataType.Text)] [MaxLength(150)] public virtual string PhoneNumber { get; set; }
        [DataType(DataType.Text)] [MaxLength(150)] public virtual string Location { get; set; }//coordinates
        [DataType(DataType.Text)] [MaxLength(150)] public virtual string AdReference { get; set; }

        [DataType(DataType.Text)] [MaxLength(400)] public virtual string Description { get; set; }
        [DataType(DataType.Text)] [MaxLength(150)] public virtual string VideoPath { get; set; }
        [DataType(DataType.Text)] [MaxLength(150)] public virtual string Keywords { get; set; }
        public virtual bool? IsActive { get; set; }
        public virtual AdsStatus? AdsStatus { get; set; }
        public virtual long? SubcategoryId { get; set; }
        
        public virtual GetAdsSubcategoryResponse Subcategory { get; set; }
        public virtual long? UserId { get; set; }
        
        public virtual GetAdsUserResponse User { get; set; }
        public bool? Negotiable { get; set; }
        public bool? ContactForPrice { get; set; }

        #endregion

        #region other ad properties 
        
        public virtual IEnumerable<GetAdsAdImageResponse> AdImages { get; set; }
        
        public virtual IEnumerable<GetAdsAdFeedbackResponse> AdFeedback { get; set; }
        
        public virtual IEnumerable<GetDasAdPropertyValueResponse> AdPropertyValue { get; set; }
        
        public virtual double Rank { get; set; }
        
        public virtual double Rating { get; set; }
        
        public virtual long Likes { get; set; }
        
        public virtual long Reviews { get; set; }
        
        public virtual bool InUserCart { get; set; }
        #endregion
    }
    public class GetAdsUserResponse
    {
        
        public long Id { get; set; }
        public long UserName { get; set; }
        public string FirstName { get; set; }
        public string MiddleName { get; set; }
        public string LastName { get; set; }
        public string Address { get; set; }
        public UserType UserType { get; set; }
        public string ImagePath { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public Gender Gender { get; set; }
        public SubscriptionPlan SubscriptionPlan { get; set; }
    }
    public class GetAdsSubcategoryResponse
    {
        #region Subcategory
        [DataType(DataType.Text)]
        public virtual string Name { get; set; }
        [DataType(DataType.Text)]
        public virtual string ImagePath { get; set; }
        [DataType(DataType.Text)]
        public virtual string ImageString { get; set; }
        public virtual long? CategoryId { get; set; }
        public virtual GetAdsCategoryResponse Category { get; set; }

        #endregion
    }
    public class GetAdsCategoryResponse
    {
        #region category
        [DataType(DataType.Text)]
        public virtual string Name { get; set; }
        [DataType(DataType.Text)]
        public virtual string ImagePath { get; set; }
        public virtual string ImageString { get; set; }
        #endregion
    }
    public class GetAdsAdImageResponse
    {
        #region ad image properties
        [DataType(DataType.Text)] public virtual string ImagePath { get; set; }
        public virtual long? AdId { get; set; }
        #endregion
    }
    public class GetAdsAdFeedbackResponse
    {
        #region AdFeedback
        public virtual long? UserId { get; set; }
        public virtual GetAdsUserResponse User { get; set; }
        public virtual long? AdId { get; set; }
        public virtual bool? Like { get; set; }
        [DataType(DataType.Text)]
        public virtual string Review { get; set; }
        public virtual Rating? Rating { get; set; }
        #endregion
    }
    public class GetDasAdPropertyValueResponse
    {
        #region AdPropertyValue
        [DataType(DataType.Text)]
        public virtual string Value { get; set; }
        public virtual long? AdId { get; set; }
        public virtual long? AdPropertyId { get; set; }
        #endregion
    }
    #endregion
}
