using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using EkiHire.Core.Domain.Entities.Auditing;
namespace EkiHire.Core.Domain.Entities
{
    public class Attachment : FullAuditedEntity
    {
        [DataType(DataType.Text)] public virtual string FilePath { get; set; }
    }
}
