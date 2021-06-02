using EkiHire.Core.Domain.Entities.Auditing;
using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel.DataAnnotations;
using EkiHire.Core.Domain.DataTransferObjects;
using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using EkiHire.Core.Domain.Extensions;
namespace EkiHire.Core.Domain.Entities
{
    public class Category : FullAuditedEntity
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
        [NotMapped]
        public IEnumerable<Subcategory> Subcategories { get; set; }
        #endregion
        public static implicit operator Category(CategoryDTO categoryDto)
        {
            if (categoryDto != null)
            {
                var category = new Category
                {
                    Name = categoryDto.Name,
                    ImagePath = categoryDto.ImagePath,
                    ImageString = categoryDto.ImageString,
                    Subcategories = categoryDto.Subcategories.ToEntity(),
                    Id = categoryDto.Id,
                };
                return category;
            }
            return null;
        }
    }
}
