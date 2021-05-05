using EkiHire.Core.Domain.Entities.Auditing;
using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel.DataAnnotations;
using EkiHire.Core.Domain.Entities;

namespace EkiHire.Core.Domain.DataTransferObjects
{
    public class ItemDTO
    {
        #region all item
        [DataType(DataType.Text)]
        public string Name { get; set; }
        [DataType(DataType.Text)]
        public string Keywords { get; set; }
        [DataType(DataType.Text)]
        public string ImagePath { get; set; }
        [DataType(DataType.Text)]
        public string GroupName { get; set; }
        public long Order { get; set; }
        //[ForeignKey("SubcategoryId")]
        public long? SubcategoryId { get; set; }
        public Subcategory Subcategory { get; set; }
        #endregion

        #region other props
        public long Id { get; set; }
        #endregion

        public static implicit operator ItemDTO(Item model)
        {
            if (model != null)
            {
                var dto = new ItemDTO
                {
                    GroupName = model.GroupName,
                    ImagePath = model.ImagePath,
                    Keywords = model.Keywords,
                    Name = model.Name,
                    Order = model.Order,
                    SubcategoryId = model.SubcategoryId,
                    Subcategory = model.Subcategory,
                    Id = model.Id
                };
                return dto;
            }
            return null;
        }
    }
}
