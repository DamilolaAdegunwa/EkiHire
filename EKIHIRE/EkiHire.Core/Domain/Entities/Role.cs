using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;

namespace EkiHire.Core.Domain.Entities
{
    public class Role : IdentityRole<long>
    {
        public virtual bool IsActive { get; set; }
        public virtual DateTime? CreationTime { get; set; }
        public virtual bool IsDefaultRole { get; set; }

        //public virtual ICollection<UserRole> UserRoles { get; set; }
    }
}