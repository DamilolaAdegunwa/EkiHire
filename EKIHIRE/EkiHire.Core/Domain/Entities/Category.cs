﻿using EkiHire.Core.Domain.Entities.Auditing;
using System;
using System.Collections.Generic;
using System.Text;

namespace EkiHire.Core.Domain.Entities
{
    public class Category : FullAuditedEntity
    {
        public string Name { get; set; }
        public ICollection<Subcategory> Subcategories { get; set; }
    }
}
