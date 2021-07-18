using EkiHire.Core.Domain.Entities.Auditing;
using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel.DataAnnotations;
using EkiHire.Core.Domain.Entities.Enums;
using EkiHire.Core.Domain.DataTransferObjects;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using EkiHire.Core.Domain.Extensions;

namespace EkiHire.Core.Domain.Entities
{
    public class Notification : FullAuditedEntity
    {
        [Required]
        public string Message { get; set; }
        public string Title { get; set; }
        public NotificationType? NotificationType { get; set; }
        [NotMapped]
        public User Recipient { get; set; }
        [Required]
        public long RecipientId { get; set; }
        public bool? Delivered { get; set; }
        public bool? IsBroadCast { get; set; }
    }
}
