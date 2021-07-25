using EkiHire.Core.Domain.Entities.Auditing;
namespace EkiHire.Core.Domain.Entities
{
    public class NewsletterSubscriber : FullAuditedEntity
    {
        public string Email { get; set; }
    }
}
