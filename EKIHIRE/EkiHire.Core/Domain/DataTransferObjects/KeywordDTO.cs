using EkiHire.Core.Domain.Entities.Auditing;
using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel.DataAnnotations;
using EkiHire.Core.Domain.DataTransferObjects;
using EkiHire.Core.Domain.Entities;

namespace EkiHire.Core.Domain.DataTransferObjects
{
    public class KeywordDTO : EntityDTO<long>
    {
        #region keywords
        [DataType(DataType.Text)]
        public virtual string Name { get; set; }
        //[ForeignKey("SubcategoryId")]
        public virtual long? SubcategoryId { get; set; }
        public virtual SubcategoryDTO Subcategory { get; set; }
        #endregion

        public static implicit operator KeywordDTO(Keyword model)
        {
            try
            {
                if (model != null)
                {
                    KeywordDTO response = new KeywordDTO
                    {
                        Name = model.Name,
                        SubcategoryId = model.SubcategoryId,
                        Subcategory = model.Subcategory,

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
