using EkiHire.Core.Domain.Entities.Auditing;
using System;
using System.Collections.Generic;
using System.Text;

namespace EkiHire.Core.Domain.Entities
{
    public class Subcategory : FullAuditedEntity
    {
        public string Name { get; set; }
        public Category Category { get; set; }

    }
}
