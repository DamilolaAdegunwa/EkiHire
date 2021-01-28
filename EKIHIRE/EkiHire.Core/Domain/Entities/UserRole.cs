using Microsoft.AspNetCore.Identity;

namespace EkiHire.Core.Domain.Entities
{
    public class UserRole : IdentityUserRole<long>
    {
        public virtual User User { get; set; }
        public virtual Role Role { get; set; }
    }
}