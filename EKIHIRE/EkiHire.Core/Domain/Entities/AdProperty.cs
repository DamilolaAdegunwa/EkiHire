using EkiHire.Core.Domain.Entities.Auditing;
using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel.DataAnnotations;
using EkiHire.Core.Domain.DataTransferObjects;

namespace EkiHire.Core.Domain.Entities
{
    public class AdProperty : FullAuditedEntity
    {
        #region AdProperty
        public string Name { get; set; }
        public string PropertyType { get; set; }
        public string Range { get; set; }
        public Subcategory Subcategory { get; set; }
        #endregion

        public static implicit operator AdProperty(AdPropertyDTO model)
        {
            try
            {
                if(model != null)
                {
                    AdProperty response = new AdProperty
                    {
                        Name = model.Name,
                        PropertyType = model.PropertyType,
                        Subcategory = model.Subcategory
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
