using EkiHire.Core.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace EkiHire.Core.Domain.DataTransferObjects
{
    public class AdPropertyValueDTO
    {
        #region AdPropertyValue
        public string Value { get; set; }
        public Ad Ad { get; set; }
        public AdProperty AdProperty { get; set; }
        #endregion

        public static implicit operator AdPropertyValueDTO(AdPropertyValue model)
        {
            try
            {
                if (model != null)
                {
                    AdPropertyValueDTO response = new AdPropertyValueDTO
                    {
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
