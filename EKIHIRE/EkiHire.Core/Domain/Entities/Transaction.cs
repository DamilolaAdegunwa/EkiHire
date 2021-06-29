using EkiHire.Core.Domain.Entities.Auditing;
using System;
using System.Collections.Generic;
using System.Text;
using EkiHire.Core.Domain.Entities.Enums;
using System.ComponentModel.DataAnnotations.Schema;

namespace EkiHire.Core.Domain.Entities
{
    public class Transaction : FullAuditedEntity
    {
        public virtual decimal Amount { get; set; }
        [ForeignKey("UserId")]
        public virtual long? UserId { get; set; }
        [NotMapped]
        public virtual User User { get; set; }
        public virtual TransactionStatus TransactionStatus { get; set; }
        public virtual TransactionType? TransactionType { get; set; }

    }
}
