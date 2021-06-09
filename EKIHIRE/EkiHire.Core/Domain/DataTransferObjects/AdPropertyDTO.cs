using EkiHire.Core.Domain.Entities.Auditing;
using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel.DataAnnotations;
using EkiHire.Core.Domain.DataTransferObjects;
using EkiHire.Core.Domain.Entities;
using EkiHire.Core.Domain.Entities.Enums;

namespace EkiHire.Core.Domain.DataTransferObjects
{
    public class AdPropertyDTO : EntityDTO<long>
    {
        #region AdProperty
        [DataType(DataType.Text)]
        public string Name { get; set; }
        public PropertyType? PropertyType { get; set; }
        [DataType(DataType.Text)]
        public string Range { get; set; }
        //[ForeignKey("SubcategoryId")]
        public virtual long? SubcategoryId { get; set; }
        public virtual SubcategoryDTO Subcategory { get; set; }
        #endregion



        public static implicit operator AdPropertyDTO(AdProperty model)
        {
            try
            {
                if (model != null)
                {
                    AdPropertyDTO response = new AdPropertyDTO
                    {
                        Name = model.Name,
                        PropertyType = model.PropertyType,
                        Range = model.Range,
                        SubcategoryId = model.SubcategoryId,
                        Subcategory = model.Subcategory,
                        Id = model.Id
                    };
                    return response;
                }
                return null;
            }
            catch (Exception ex)
            {
                return null;
            }
        }
    }
}
