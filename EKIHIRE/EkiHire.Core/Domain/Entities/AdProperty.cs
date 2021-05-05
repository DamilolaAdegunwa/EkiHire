using EkiHire.Core.Domain.Entities.Auditing;
using System;
using EkiHire.Core.Domain.DataTransferObjects;
using System.ComponentModel.DataAnnotations.Schema;

namespace EkiHire.Core.Domain.Entities
{
    public class AdProperty : FullAuditedEntity
    {
        #region AdProperty
        public string Name { get; set; }
        public string PropertyType { get; set; }
        public string Range { get; set; }
        public virtual Subcategory Subcategory { get; set; }
        
        
       
        #endregion

        //public static implicit operator AdProperty(AdPropertyDTO model)
        //{
        //    try
        //    {
        //        if (model != null)
        //        {
        //            AdProperty response = new AdProperty
        //            {
        //                Name = model.Name,
        //                PropertyType = model.PropertyType,
        //                Range=model.Range,
        //                Subcategory = model.SubcategoryDTO
        //            };
        //            return response;
        //        }
        //        return null;
        //    }
        //    catch (Exception ex)
        //    {
        //        return null;
        //    }
        //}
    }
}
