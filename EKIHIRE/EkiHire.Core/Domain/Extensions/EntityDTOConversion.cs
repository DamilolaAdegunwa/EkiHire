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
        #region Category
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
        #endregion

        #region sub-categories
        public static IEnumerable<SubcategoryDTO> ToDTO(this IEnumerable<Subcategory> entities)
        {
            if (entities != null)
            {
                List<SubcategoryDTO> result = new List<SubcategoryDTO>();
                foreach (var entity in entities)
                {
                    SubcategoryDTO dto = entity;
                    result.Add(dto);
                }
                return result;
            }
            return null;
        }
        #endregion

        #region ad
        public static IEnumerable<AdDTO> ToDTO(this IEnumerable<Ad> entities)
        {
            if (entities != null)
            {
                List<AdDTO> result = new List<AdDTO>();
                foreach (var entity in entities)
                {
                    AdDTO dto = entity;
                    result.Add(dto);
                }
                return result;
            }
            return null;
        }
        #endregion

        #region real estate
        //public static IEnumerable<RealEstateDTO> ToDTO(this IEnumerable<RealEstate> entities)
        //{
        //    try
        //    {
        //        if(entities != null)
        //        {
        //            var result = new List<RealEstateDTO>();
        //            foreach(var entity in entities)
        //            {
        //                RealEstateDTO dto = entity;
        //                result.Add(dto);

        //            }
        //            return result;
        //        }
        //        return null;
        //    }
        //    catch(Exception ex)
        //    {
        //        return null;
        //    }
        //}
        #endregion

        #region User
        //public static 
        #endregion
    }
}
