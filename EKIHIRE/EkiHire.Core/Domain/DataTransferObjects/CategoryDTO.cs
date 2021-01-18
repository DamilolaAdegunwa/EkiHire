using EkiHire.Core.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace EkiHire.Core.Domain.DataTransferObjects
{
    public class CategoryDTO
    {

        public static implicit operator CategoryDTO(Category category)
        {
            if(category != null)
            {
                var categoryDto = new CategoryDTO
                {

                };
                return categoryDto;
            }
            return null; 
        }
    }
}
