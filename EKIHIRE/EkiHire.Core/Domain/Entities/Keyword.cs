using EkiHire.Core.Domain.Entities.Auditing;
using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel.DataAnnotations;
using EkiHire.Core.Domain.DataTransferObjects;

namespace EkiHire.Core.Domain.Entities
{
    public class Keyword : FullAuditedEntity
    {
        #region keywords
        [DataType(DataType.Text)]
        public string Name { get; set; }
        public Subcategory Subcategory { get; set; }
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
                        Subcategory = model.Subcategory
                    };
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
