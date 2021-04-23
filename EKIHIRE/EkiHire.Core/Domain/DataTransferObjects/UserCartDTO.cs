using EkiHire.Core.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace EkiHire.Core.Domain.DataTransferObjects
{
    public class UserCartDTO
    {
        public Ad Ad { get; set; }
        public User User { get; set; }
        public static implicit operator UserCartDTO(UserCart model)
        {
            if (model != null)
            {
                var output = new UserCartDTO
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
