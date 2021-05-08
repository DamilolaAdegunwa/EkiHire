using EkiHire.Core.Domain.Entities.Auditing;
using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel.DataAnnotations;
using EkiHire.Core.Domain.Entities.Enums;
using EkiHire.Core.Domain.Entities;
using EkiHire.Core.Domain.DataTransferObjects;
using System.ComponentModel.DataAnnotations.Schema;

namespace EkiHire.Core.Domain.Entities
{
    public class UserCart: FullAuditedEntity
    {
        #region user cart
        [ForeignKey("AdId")]
        public virtual long? AdId { get; set; }
        public virtual Ad Ad { get; set; }
        [ForeignKey("UserId")]
        public virtual long? UserId { get; set; }
        public virtual User User { get; set; }
        #endregion
        public static implicit operator UserCart(UserCartDTO model)
        {
            if(model != null)
            {
                var output = new UserCart
                {
                    AdId = model.AdId,
                    Ad = model.Ad,
                    User = model.User,
                    UserId = model.UserId,
                    Id = model.Id
                };
                return output;
            }
            return null;
        }
    }
}
