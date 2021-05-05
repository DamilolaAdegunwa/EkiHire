using EkiHire.Core.Domain.Entities.Auditing;
using System;
using EkiHire.Core.Domain.DataTransferObjects;
using System.ComponentModel.DataAnnotations.Schema;
namespace EkiHire.Core.Domain.Entities
{
    public class Subcategory : FullAuditedEntity
    {
        #region Subcategory
        public string Name { get; set; }
        //[DataType(DataType.Text)]
        public string ImagePath { get; set; }
        //[DataType(DataType.Text)]
        public string ImageString { get; set; }

        [ForeignKey("CategoryId")]
        public virtual Category Category { get; set; }

      
        public long CategoryId { get; set; }

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
                    Category = model.Category
                };
                return response;
            }
            return null;
        }
    }
}
