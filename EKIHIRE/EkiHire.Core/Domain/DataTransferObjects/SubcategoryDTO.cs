using EkiHire.Core.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace EkiHire.Core.Domain.DataTransferObjects
{
    public class SubcategoryDTO
    {
        public string Name { get; set; }
        public Category Category { get; set; }

        public static implicit operator SubcategoryDTO(Subcategory model)
        {
            if (model != null)
            {
                var response = new SubcategoryDTO
                {
                    Name = model.Name,
                    Category = model.Category
                };
                return response;
            }
            return null;
        }
    }
}
