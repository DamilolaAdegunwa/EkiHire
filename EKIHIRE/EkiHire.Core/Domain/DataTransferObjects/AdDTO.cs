using EkiHire.Core.Domain.Entities.Auditing;
using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel.DataAnnotations;
using EkiHire.Core.Domain.Entities.Enums;
using EkiHire.Core.Domain.Entities;
using EkiHire.Core.Domain.Extensions;
using Microsoft.AspNetCore.Http;

namespace EkiHire.Core.Domain.DataTransferObjects
{
    public class AdDTO : EntityDTO<long>
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
        //public virtual bool? Promotion { get; set; }
        //[ForeignKey("SubcategoryId")]
        public virtual long? SubcategoryId { get; set; }
        public virtual SubcategoryDTO Subcategory { get; set; }
        //[ForeignKey("UserId")]
        public virtual long? UserId { get; set; }
        public virtual User User { get; set; }
        //public virtual IFormFile FormFile { get; set; }

        #endregion

        #region other addto property
        public virtual IEnumerable<AdImageDTO> AdImages { get; set; }
        public virtual IEnumerable<AdFeedbackDTO> AdFeedback { get; set; }
        public virtual IEnumerable<AdPropertyValueDTO> AdPropertyValue { get; set; }
        public virtual double Rank { get; set; }
        public virtual double Rating { get; set; }
        public virtual long Likes { get; set; }
        public virtual long Reviews { get; set; }
        public virtual bool InUserCart { get; set; }
        #endregion

        public static implicit operator AdDTO(Ad model)
        {
            if (model != null)
            {
                var dto = new AdDTO
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
                    AdImages = model.AdImages.ToDTO(),
                    AdPropertyValue = model.AdPropertyValue.ToDTO(),
                    AdFeedback = model.AdFeedback.ToDTO(),
                    Rank = model.Rank,
                    AdsStatus = model.AdsStatus,
                    //Promotion = model.Promotion,
                    Reviews = model.Reviews,
                    Rating = model.Rating,
                    Likes = model.Likes,
                };
                return dto;
            }
            return null;
        }
    }
}
