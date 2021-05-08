using EkiHire.Core.Domain.Entities.Auditing;
using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel.DataAnnotations;
using EkiHire.Core.Domain.DataTransferObjects;
using System.ComponentModel.DataAnnotations.Schema;

namespace EkiHire.Core.Domain.Entities
{
    public class Keyword : FullAuditedEntity
    {
        #region keywords
        [DataType(DataType.Text)]
        public virtual string Name { get; set; }
        [ForeignKey("SubcategoryId")]
        public virtual long? SubcategoryId { get; set; }
        public virtual Subcategory Subcategory { get; set; }
        #endregion

        public static implicit operator Keyword(KeywordDTO model)
        {
            try
            {
                if(model!=null)
                {
                    Keyword response = new Keyword
                    {
                        Name = model.Name,
                        SubcategoryId = model.SubcategoryId,
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
