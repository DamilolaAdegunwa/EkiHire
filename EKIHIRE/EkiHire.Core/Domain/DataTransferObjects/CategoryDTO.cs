using EkiHire.Core.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace EkiHire.Core.Domain.DataTransferObjects
{
    public class CategoryDTO
    {
        public string Name { get; set; }

        public static implicit operator CategoryDTO(Category category)
        {
            if(category != null)
            {
                var categoryDto = new CategoryDTO
                {
                    Name = category.Name
                };
                return categoryDto;
            }
            return null; 
        }
    }
}
