using EkiHire.Core.Domain.Entities.Auditing;
using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel.DataAnnotations;
namespace EkiHire.Core.Domain.Entities
{
    public class Category : FullAuditedEntity
    {
        [DataType(DataType.Text)]
        public string Name { get; set; }
        [DataType(DataType.Text)]
        public string ImagePath { get; set; }
        public string ImageString { get; set; }
        public ICollection<Subcategory> Subcategories { get; set; }
    }
}
