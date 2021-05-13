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
        public static IEnumerable<Category> ToEntity(this IEnumerable<CategoryDTO> categories)
        {
            if (categories != null)
            {
                List<Category> entities = new List<Category>();
                foreach (var c in categories)
                {
                    Category entity = c;
                    entities.Add(entity);
                }
                return entities;
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
        public static IEnumerable<Ad> ToEntity(this IEnumerable<AdDTO> input)
        {
            if (input != null)
            {
                List<Ad> output = new List<Ad>();
                foreach (var row in input)
                {
                    Ad data = row;
                    output.Add(data);
                }
                return output;
            }
            return null;
        }
        #endregion

        #region ad image
        public static IEnumerable<AdImageDTO> ToDTO(this IEnumerable<AdImage> input)
        {
            if (input != null)
            {
                List<AdImageDTO> output = new List<AdImageDTO>();
                foreach (var row in input)
                {
                    AdImageDTO data = row;
                    output.Add(data);
                }
                return output;
            }
            return null;
        }
        public static IEnumerable<AdImage> ToEntity(this IEnumerable<AdImageDTO> input)
        {
            if (input != null)
            {
                List<AdImage> output = new List<AdImage>();
                foreach (var row in input)
                {
                    AdImage data = row;
                    output.Add(data);
                }
                return output;
            }
            return null;
        }
        #endregion

        #region ad property value
        public static IEnumerable<AdPropertyValueDTO> ToDTO(this IEnumerable<AdPropertyValue> input)
        {
            if (input != null)
            {
                List<AdPropertyValueDTO> output = new List<AdPropertyValueDTO>();
                foreach (var row in input)
                {
                    AdPropertyValueDTO data = row;
                    output.Add(data);
                }
                return output;
            }
            return null;
        }
        public static IEnumerable<AdPropertyValue> ToEntity(this IEnumerable<AdPropertyValueDTO> input)
        {
            if (input != null)
            {
                List<AdPropertyValue> output = new List<AdPropertyValue>();
                foreach (var row in input)
                {
                    AdPropertyValue data = row;
                    output.Add(data);
                }
                return output;
            }
            return null;
        }
        #endregion

        #region ad feed back
        public static IEnumerable<AdFeedbackDTO> ToDTO(this IEnumerable<AdFeedback> input)
        {
            if (input != null)
            {
                List<AdFeedbackDTO> output = new List<AdFeedbackDTO>();
                foreach (var row in input)
                {
                    AdFeedbackDTO data = row;
                    output.Add(data);
                }
                return output;
            }
            return null;
        }
        public static IEnumerable<AdFeedback> ToEntity(this IEnumerable<AdFeedbackDTO> input)
        {
            if (input != null)
            {
                List<AdFeedback> output = new List<AdFeedback>();
                foreach (var row in input)
                {
                    AdFeedback data = row;
                    output.Add(data);
                }
                return output;
            }
            return null;
        }
        #endregion

        #region ad property
        public static IEnumerable<AdPropertyDTO> ToDTO(this IEnumerable<AdProperty> input)
        {
            if (input != null)
            {
                List<AdPropertyDTO> output = new List<AdPropertyDTO>();
                foreach (var row in input)
                {
                    AdPropertyDTO data = row;
                    output.Add(data);
                }
                return output;
            }
            return null;
        }
        public static IEnumerable<AdProperty> ToEntity(this IEnumerable<AdPropertyDTO> input)
        {
            if (input != null)
            {
                List<AdProperty> output = new List<AdProperty>();
                foreach (var row in input)
                {
                    AdProperty data = row;
                    output.Add(data);
                }
                return output;
            }
            return null;
        }
        #endregion

        #region follow
        public static IEnumerable<FollowDTO> ToDTO(this IEnumerable<Follow> input)
        {
            if (input != null)
            {
                List<FollowDTO> output = new List<FollowDTO>();
                foreach (var row in input)
                {
                    FollowDTO data = row;
                    output.Add(data);
                }
                return output;
            }
            return null;
        }
        public static IEnumerable<Follow> ToEntity(this IEnumerable<FollowDTO> input)
        {
            if (input != null)
            {
                List<Follow> output = new List<Follow>();
                foreach (var row in input)
                {
                    Follow data = row;
                    output.Add(data);
                }
                return output;
            }
            return null;
        }
        #endregion

        #region item
        public static IEnumerable<ItemDTO> ToDTO(this IEnumerable<Item> input)
        {
            if (input != null)
            {
                List<ItemDTO> output = new List<ItemDTO>();
                foreach (var row in input)
                {
                    ItemDTO data = row;
                    output.Add(data);
                }
                return output;
            }
            return null;
        }
        public static IEnumerable<Item> ToEntity(this IEnumerable<ItemDTO> input)
        {
            if (input != null)
            {
                List<Item> output = new List<Item>();
                foreach (var row in input)
                {
                    Item data = row;
                    output.Add(data);
                }
                return output;
            }
            return null;
        }
        #endregion

        #region keywords
        public static IEnumerable<KeywordDTO> ToDTO(this IEnumerable<Keyword> input)
        {
            if (input != null)
            {
                List<KeywordDTO> output = new List<KeywordDTO>();
                foreach (var row in input)
                {
                    KeywordDTO data = row;
                    output.Add(data);
                }
                return output;
            }
            return null;
        }
        public static IEnumerable<Keyword> ToEntity(this IEnumerable<KeywordDTO> input)
        {
            if (input != null)
            {
                List<Keyword> output = new List<Keyword>();
                foreach (var row in input)
                {
                    Keyword data = row;
                    output.Add(data);
                }
                return output;
            }
            return null;
        }
        #endregion

        #region user cart
        public static IEnumerable<UserCartDTO> ToDTO(this IEnumerable<UserCart> input)
        {
            if (input != null)
            {
                List<UserCartDTO> output = new List<UserCartDTO>();
                foreach (var row in input)
                {
                    UserCartDTO data = row;
                    output.Add(data);
                }
                return output;
            }
            return null;
        }
        public static IEnumerable<UserCart> ToEntity(this IEnumerable<UserCartDTO> input)
        {
            if (input != null)
            {
                List<UserCart> output = new List<UserCart>();
                foreach (var row in input)
                {
                    UserCart data = row;
                    output.Add(data);
                }
                return output;
            }
            return null;
        }
        #endregion
    }
}
