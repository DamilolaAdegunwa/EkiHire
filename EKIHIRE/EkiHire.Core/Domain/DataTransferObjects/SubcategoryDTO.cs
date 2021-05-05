using EkiHire.Core.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;
namespace EkiHire.Core.Domain.DataTransferObjects
{
    public class SubcategoryDTO
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

        #endregion

        #region other props
        public long Id { get; set; }
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
