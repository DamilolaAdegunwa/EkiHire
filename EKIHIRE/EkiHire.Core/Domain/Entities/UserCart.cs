using EkiHire.Core.Domain.Entities.Auditing;
using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel.DataAnnotations;
using EkiHire.Core.Domain.Entities.Enums;
using EkiHire.Core.Domain.Entities;
using EkiHire.Core.Domain.DataTransferObjects;

namespace EkiHire.Core.Domain.Entities
{
    public class UserCart: FullAuditedEntity
    {
        public Ad Ad { get; set; }
        public User User { get; set; }
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
