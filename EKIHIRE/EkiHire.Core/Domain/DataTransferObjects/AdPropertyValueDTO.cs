using EkiHire.Core.Domain.Entities;
using EkiHire.Core.Domain.Entities.Auditing;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace EkiHire.Core.Domain.DataTransferObjects
{
    public class AdPropertyValueDTO : EntityDTO<long>
    {
        #region AdPropertyValue
        [DataType(DataType.Text)]
        public virtual string Value { get; set; }
        //[ForeignKey("AdId")]
        public virtual long? AdId { get; set; }
        public virtual Ad Ad { get; set; }
        //[ForeignKey("AdPropertyId")]
        public virtual long? AdPropertyId { get; set; }
        public virtual AdProperty AdProperty { get; set; }
        #endregion

        public static implicit operator AdPropertyValueDTO(AdPropertyValue model)
        {
            try
            {
                if (model != null)
                {
                    AdPropertyValueDTO response = new AdPropertyValueDTO
                    {
                        Id = model.Id,
                        Value = model.Value,
                        Ad = model.Ad,
                        AdId = model.AdId,
                        AdProperty = model.AdProperty,
                        AdPropertyId = model.AdPropertyId,
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
