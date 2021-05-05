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
        public string Name { get; set; }
        [DataType(DataType.Text)]
        public string ImagePath { get; set; }
        [DataType(DataType.Text)]
        public string ImageString { get; set; }

        public Category Category { get; set; }

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
                    Category = model.Category
                };
                return response;
            }
            return null;
        }
    }
}
