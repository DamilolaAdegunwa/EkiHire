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
        [ForeignKey("AdId")]
        public virtual long? AdId { get; set; }
        public virtual Ad Ad { get; set; }
        [ForeignKey("UserId")]
        public virtual long? UserId { get; set; }
        public virtual User User { get; set; }
        public static implicit operator UserCart(UserCartDTO model)
        {
            if(model != null)
            {
                var output = new UserCart
                {
                    Ad = model.Ad,
                    User = model.User,
                };
                return output;
            }
            return null;
        }
    }
}
