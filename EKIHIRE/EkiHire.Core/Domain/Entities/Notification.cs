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
        public string Message { get; set; }
        [Required]
        public string Title { get; set; }
        public NotificationType? NotificationType { get; set; }
        public User Recipient { get; set; }
        public long? UserId { get; set; }
        public bool? Delivered { get; set; }
        public bool? IsBroadCast { get; set; }
    }
}
