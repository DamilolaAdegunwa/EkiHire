using EkiHire.Core.Domain.Entities.Auditing;

namespace EkiHire.Core.Domain.Entities
{
    public class SubscriptionPackage : FullAuditedEntity<long>
    {
        public string Name { get; set; }
        public decimal Amount { get; set; }
    }
}
