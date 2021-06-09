using EkiHire.Core.Domain.Entities.Auditing;
using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel.DataAnnotations;
using EkiHire.Core.Domain.DataTransferObjects;
using System.ComponentModel.DataAnnotations.Schema;

namespace EkiHire.Core.Domain.Entities
{
    public class AdPropertyValue : FullAuditedEntity
    {
        #region AdPropertyValue
        [DataType(DataType.Text)]
        public virtual string Value { get; set; }
        [ForeignKey("AdId")]
        public virtual long? AdId { get; set; }
        //[NotMapped]
        public virtual Ad Ad { get; set; }
        [ForeignKey("AdPropertyId")]
        public virtual long? AdPropertyId { get; set; }
        public virtual AdProperty AdProperty { get; set; }
        #endregion

        public static implicit operator AdPropertyValue(AdPropertyValueDTO model)
        {
            try
            {
                if(model != null)
                {
                    AdPropertyValue response = new AdPropertyValue { 
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
