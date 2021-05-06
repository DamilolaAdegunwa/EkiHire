using EkiHire.Core.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel.DataAnnotations;
using EkiHire.Core.Domain.Entities.Auditing;

namespace EkiHire.Core.Domain.DataTransferObjects
{
    public class CategoryDTO : EntityDTO<long>
    {
        #region category
        [DataType(DataType.Text)]
        public virtual string Name { get; set; }
        [DataType(DataType.Text)]
        public virtual string ImagePath { get; set; }
        public virtual string ImageString { get; set; }
        //public ICollection<Subcategory> Subcategories { get; set; }
        #endregion

        #region other props
        #endregion
        public static implicit operator CategoryDTO(Category category)
        {
            if(category != null)
            {
                var categoryDto = new CategoryDTO
                {
                    Name = category.Name,
                    ImagePath = category.ImagePath,
                    ImageString = category.ImageString,
                    //Subcategories = category.Subcategories,
                    Id = category.Id
                };
                return categoryDto;
            }
            return null; 
        }
    }
}
