using EkiHire.Core.Domain.Entities.Auditing;
using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel.DataAnnotations;
using EkiHire.Core.Domain.DataTransferObjects;

namespace EkiHire.Core.Domain.Entities
{
    public class AdPropertyValue : FullAuditedEntity
    {
        #region AdPropertyValue
        public string Value { get; set; }
        public Ad Ad { get; set; }
        public AdProperty AdProperty { get; set; }
        #endregion

        public static implicit operator AdPropertyValue(AdPropertyValueDTO model)
        {
            try
            {
                if(model != null)
                {
                    AdPropertyValue response = new AdPropertyValue { 
                        Value = model.Value,
                        Ad = model.Ad,
                        AdProperty = model.AdProperty
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
