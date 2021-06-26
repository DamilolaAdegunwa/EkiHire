using EkiHire.Core.Domain.Entities.Auditing;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace EkiHire.Core.Domain.Entities
{
    public class AdLookupLog : FullAuditedEntity
    {
        [ForeignKey("AdId")]
        public virtual long? AdId { get; set; }
        public virtual Ad Ad { get; set; }
    }
}
