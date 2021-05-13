using EkiHire.Core.Domain.Entities;
using EkiHire.Core.Domain.Entities.Auditing;
using System;
using System.Collections.Generic;
using System.Text;

namespace EkiHire.Core.Domain.DataTransferObjects
{
    public class UserCartDTO : EntityDTO<long>
    {
        #region user cart
        //[ForeignKey("AdId")]
        public virtual long? AdId { get; set; }
        public virtual AdDTO Ad { get; set; }
        //[ForeignKey("UserId")]
        public virtual long? UserId { get; set; }
        public virtual User User { get; set; }
        #endregion
        public static implicit operator UserCartDTO(UserCart model)
        {
            if (model != null)
            {
                var output = new UserCartDTO
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
