using EkiHire.Core.Domain.Entities.Auditing;
using System;
using System.Collections.Generic;
using System.Text;
using EkiHire.Core.Domain.Entities.Enums;
namespace EkiHire.Core.Domain.Entities
{
    public class Transaction : FullAuditedEntity
    {
        public decimal Amount { get; set; }
        public TransactionType TransactionType { get; set; }
    }
}
