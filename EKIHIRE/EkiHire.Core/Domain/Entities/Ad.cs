using EkiHire.Core.Domain.Entities.Auditing;
using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel.DataAnnotations;
using EkiHire.Core.Domain.Entities.Enums;
using EkiHire.Core.Domain.DataTransferObjects;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using EkiHire.Core.Domain.Extensions;
namespace EkiHire.Core.Domain.Entities
{
    public class Ad: FullAuditedEntity
    {
        #region all ad properties
        [DataType(DataType.Text)] public virtual string Name { get; set; }
        [DataType(DataType.Text)] public virtual string Address { get; set; }
        public virtual decimal? Amount { get; set; }
        public virtual AdClass? AdClass { get; set; }
        
        
        [DataType(DataType.Text)] public virtual string PhoneNumber { get; set; }
        [DataType(DataType.Text)] public virtual string Location { get; set; }//coordinates
        [DataType(DataType.Text)] public virtual string AdReference { get; set; }
        
        [DataType(DataType.Text)] public virtual string Description { get; set; }
        [DataType(DataType.Text)] public virtual string VideoPath { get; set; }
        [DataType(DataType.Text)] public virtual string Keywords { get; set; }
        public virtual bool? IsActive { get; set; }
        public virtual AdsStatus? AdsStatus { get; set; }
        [ForeignKey("SubcategoryId")]
        public virtual long? SubcategoryId { get; set; }
        [NotMapped]
        public virtual Subcategory Subcategory { get; set; }
        [ForeignKey("UserId")]
        public virtual long? UserId { get; set; }
        [NotMapped]
        public virtual User User { get; set; }

        #endregion

        #region other ad properties 
        //[NotMapped]
        public virtual IEnumerable<AdImage> AdImages { get; set; }
        [NotMapped]
        public virtual IEnumerable<AdFeedback> AdFeedback { get; set; }
        [NotMapped]
        public virtual IEnumerable<AdPropertyValue> AdPropertyValue { get; set; }
        [NotMapped]
        public virtual double Rank { get; set; }
        [NotMapped]
        public virtual double Rating { get; set; }
        [NotMapped]
        public virtual long Likes { get; set; }
        [NotMapped]
        public virtual long Reviews { get; set; }
        [NotMapped]
        public virtual bool InUserCart { get; set; }
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
                    Amount = model.Amount,
                    Keywords = model.Keywords,
                    Location = model.Location,
                    PhoneNumber = model.PhoneNumber,
                    Subcategory = model.Subcategory,
                    SubcategoryId = model.SubcategoryId,
                    VideoPath = model.VideoPath,
                    User = model.User,
                    UserId = model.UserId,
                    Description = model.Description,
                    AdReference = model.AdReference,
                    Id = model.Id,
                    InUserCart = model.InUserCart,
                    IsActive = model.IsActive,
                    AdImages = model.AdImages.ToEntity(),
                    AdPropertyValue = model.AdPropertyValue.ToEntity(),
                    AdFeedback = model.AdFeedback.ToEntity(),
                    Rank = model.Rank,
                    AdsStatus = model.AdsStatus,
                    //Promotion = model.Promotion,
                    Reviews = model.Reviews,
                    Rating = model.Rating,
                    Likes = model.Likes,
                };
                return output;
            }
            return null;
        }
    }
}
