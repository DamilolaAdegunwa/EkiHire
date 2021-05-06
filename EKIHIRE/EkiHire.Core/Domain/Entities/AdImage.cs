using EkiHire.Core.Domain.Entities.Auditing;
using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel.DataAnnotations;

namespace EkiHire.Core.Domain.Entities
{
    public class AdImage: FullAuditedEntity
    {
        [DataType(DataType.Text)] public virtual string ImagePath { get; set; }
        [DataType(DataType.Text)] public virtual string ImageString { get; set; }
    }
}
