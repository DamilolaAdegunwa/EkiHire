using System;
using EkiHire.Core.Domain.Entities.Auditing;

namespace EkiHire.Core.Domain.Entities
{
    public class Wallet : FullAuditedEntity
    {
        public string WalletNumber { get; set; }
        public decimal Balance { get; set; }
        public string UserType { get; set; }
        public string UserId { get; set; }
        public bool IsReset { get; set; }
        public DateTime? LastResetDate { get; set; }
    }
}