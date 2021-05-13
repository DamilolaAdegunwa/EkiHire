using EkiHire.Core.Domain.Entities;
using EkiHire.Core.Domain.Entities.Auditing;
using EkiHire.Core.Domain.Entities.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace EkiHire.Core.Domain.DataTransferObjects
{
    public class AdFeedbackDTO : EntityDTO<long>
    {
        #region AdFeedback
        //[ForeignKey("UserId")]
        public virtual long? UserId { get; set; }
        public virtual User User { get; set; }
        //[ForeignKey("AdId")]
        public virtual long? AdId { get; set; }
        public virtual AdDTO Ad { get; set; }
        public virtual bool Like { get; set; }
        [DataType(DataType.Text)]
        public virtual string Review { get; set; }
        public virtual Rating Rating { get; set; }
        #endregion

        public static implicit operator AdFeedbackDTO(AdFeedback model)
        {
            try
            {
                if(model != null)
                {
                    AdFeedbackDTO response = new AdFeedbackDTO
                    {
                        Id = model.Id,
                        User = model.User,
                        UserId = model.UserId,
                        Ad = model.Ad,
                        AdId = model.AdId,
                        Like = model.Like,
                        Rating = model.Rating,
                        Review = model.Review,
                    };
                    return response;
                }
                return null;
            }
            catch (Exception ex)
            {
                return null;
            }
        }
    }
}
