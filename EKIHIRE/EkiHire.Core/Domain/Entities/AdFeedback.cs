using EkiHire.Core.Domain.Entities.Auditing;
using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel.DataAnnotations;
using EkiHire.Core.Domain.Entities.Enums;
using System.ComponentModel.DataAnnotations.Schema;
using EkiHire.Core.Domain.DataTransferObjects;

namespace EkiHire.Core.Domain.Entities
{
    public class AdFeedback: FullAuditedEntity
    {
        #region AdFeedback
        [ForeignKey("UserId")]
        public virtual long? UserId { get; set; }
        public virtual User User { get; set; }
        [ForeignKey("AdId")]
        public virtual long? AdId { get; set; }
        public virtual Ad Ad { get; set; }
        public virtual bool? Like { get; set; }
        [DataType(DataType.Text)]
        public virtual string Review { get; set; }
        public virtual Rating? Rating { get; set; }
        #endregion

        public static implicit operator AdFeedback(AdFeedbackDTO model)
        {
            try
            {
                if (model != null)
                {
                    AdFeedback response = new AdFeedback
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
