using EkiHire.Core.Domain.Entities.Auditing;
using System;
using EkiHire.Core.Domain.DataTransferObjects;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace EkiHire.Core.Domain.Entities
{
    public class AdProperty : FullAuditedEntity
    {
        #region AdProperty
        [DataType(DataType.Text)]
        public virtual string Name { get; set; }
        [DataType(DataType.Text)]
        public virtual string PropertyType { get; set; }
        [DataType(DataType.Text)]
        public virtual string Range { get; set; }
        [ForeignKey("SubcategoryId")]
        public virtual long? SubcategoryId { get; set; }
        public virtual Subcategory Subcategory { get; set; }
        #endregion

        public static implicit operator AdProperty(AdPropertyDTO model)
        {
            try
            {
                if (model != null)
                {
                    AdProperty response = new AdProperty
                    {
                        Id = model.Id,
                        Name = model.Name,
                        PropertyType = model.PropertyType,
                        Range = model.Range,
                        Subcategory = model.Subcategory,
                        SubcategoryId = model.SubcategoryId,
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
