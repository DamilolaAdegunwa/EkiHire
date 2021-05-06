using System;
using System.ComponentModel.DataAnnotations;
using EkiHire.Core.Domain.Entities.Auditing;

namespace EkiHire.Core.Domain.Entities
{
    public class Wallet : FullAuditedEntity
    {
        [DataType(DataType.Text)] public virtual string WalletNumber { get; set; }
        public virtual decimal Balance { get; set; }
        [DataType(DataType.Text)] public virtual string UserType { get; set; }
        [DataType(DataType.Text)] public virtual string UserId { get; set; }
        public virtual bool IsReset { get; set; }
        public virtual DateTime? LastResetDate { get; set; }
    }
}