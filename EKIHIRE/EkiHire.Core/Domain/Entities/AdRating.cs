using EkiHire.Core.Domain.Entities.Auditing;
using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel.DataAnnotations;
using EkiHire.Core.Domain.Entities.Enums;

namespace EkiHire.Core.Domain.Entities
{
    public class AdRating: FullAuditedEntity
    {
        public long UserId { get; set; }
        public long AdId { get; set; }
        public bool Like { get; set; }
        public string Review { get; set; }
        public Rating Rating { get; set; }
    }
}
