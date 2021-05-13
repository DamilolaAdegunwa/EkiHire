using EkiHire.Core.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;
using EkiHire.Core.Domain.Entities.Auditing;

namespace EkiHire.Core.Domain.DataTransferObjects
{
    public class SubcategoryDTO : EntityDTO<long>
    {
        #region Subcategory
        //[DataType(DataType.Text)]
        public virtual string Name { get; set; }
        //[DataType(DataType.Text)]
        public virtual string ImagePath { get; set; }
        //[DataType(DataType.Text)]
        public virtual string ImageString { get; set; }

        //[ForeignKey("CategoryId")]
        public virtual long? CategoryId { get; set; }
        public virtual CategoryDTO Category { get; set; }
        //public virtual ICollection<AdPropertyDTO> AdProperties { get; set; }

        #endregion

        #region other props
        public virtual ICollection<AdPropertyDTO> AdProperties { get; set; }
        #endregion

        public static implicit operator SubcategoryDTO(Subcategory model)
        {
            if (model != null)
            {
                var response = new SubcategoryDTO
                {
                    Id = model.Id,
                    Name = model.Name,
                    ImagePath = model.ImagePath,
                    ImageString = model.ImageString,
                    CategoryId = model.CategoryId,
                    Category = model.Category
                };
                return response;
            }
            return null;
        }
    }
}
