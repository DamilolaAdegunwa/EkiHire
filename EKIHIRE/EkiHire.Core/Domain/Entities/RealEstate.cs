using System;
using System.Collections.Generic;
using System.Text;
using EkiHire.Core.Domain.Entities.Auditing;
namespace EkiHire.Core.Domain.Entities
{
    public class RealEstate: FullAuditedEntity
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }
        public City City { get; set; }
        public Subcategory Subcategory { get; set; }
        public RealEstateStatus Status { get; set; }

    }
}
