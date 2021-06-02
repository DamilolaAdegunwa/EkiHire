using EkiHire.Core.Domain.Entities.Auditing;
using System;
using EkiHire.Core.Domain.DataTransferObjects;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;
using EkiHire.Core.Domain.Extensions;

namespace EkiHire.Core.Domain.Entities
{
    public class Subcategory : FullAuditedEntity
    {
        #region Subcategory
        [DataType(DataType.Text)]
        public virtual string Name { get; set; }
        [DataType(DataType.Text)]
        public virtual string ImagePath { get; set; }
        [DataType(DataType.Text)]
        public virtual string ImageString { get; set; }

        [ForeignKey("CategoryId")]
        public virtual long? CategoryId { get; set; }
        public virtual Category Category { get; set; }
        //public virtual ICollection<AdProperty> AdProperties { get; set; }

        #endregion

        #region other props
        [NotMapped]
        public virtual IEnumerable<AdProperty> AdProperties { get; set; }
        #endregion

        public static implicit operator Subcategory(SubcategoryDTO model)
        {
            if (model != null)
            {
                var response = new Subcategory
                {
                    Id = model.Id,
                    Name = model.Name,
                    ImagePath = model.ImagePath,
                    ImageString = model.ImageString,
                    CategoryId = model.CategoryId,
                    Category = model.Category,
                    AdProperties = model.AdProperties.ToEntity()
                };
                return response;
            }
            return null;
        }
    }
}
