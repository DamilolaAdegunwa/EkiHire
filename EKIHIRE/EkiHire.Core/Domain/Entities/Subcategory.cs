using EkiHire.Core.Domain.DataTransferObjects;
using EkiHire.Core.Domain.Entities.Auditing;
using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;

namespace EkiHire.Core.Domain.Entities
{
    public class Subcategory : FullAuditedEntity
    {
        public string Name { get; set; }
        [DataType(DataType.Text)]
        public string ImagePath { get; set; }
        [DataType(DataType.Text)]
        public string ImageString { get; set; }
        public Category Category { get; set; }
        
        public static implicit operator Subcategory(SubcategoryDTO model)
        {
            if (model != null)
            {
                var response = new Subcategory
                {
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
