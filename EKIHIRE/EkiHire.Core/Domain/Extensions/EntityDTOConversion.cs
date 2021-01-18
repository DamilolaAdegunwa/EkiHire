using EkiHire.Core.Domain.Entities;
using EkiHire.Core.Domain.DataTransferObjects;
using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
namespace EkiHire.Core.Domain.Extensions
{
    public static class EntityDTOConversion
    {
        public static IEnumerable<CategoryDTO> ToDTO(this IEnumerable<Category> categories)
        {
            if(categories != null)
            {
                List<CategoryDTO> categoryDtos = new List<CategoryDTO>();
                foreach(var c in categories)
                {
                    CategoryDTO categoryDto = c;
                    categoryDtos.Add(categoryDto);
                }
                return categoryDtos;
            }
            return null;
        }
    }
}
