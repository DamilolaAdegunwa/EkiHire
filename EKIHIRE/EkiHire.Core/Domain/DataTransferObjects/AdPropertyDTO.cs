using EkiHire.Core.Domain.Entities.Auditing;
using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel.DataAnnotations;
using EkiHire.Core.Domain.DataTransferObjects;
using EkiHire.Core.Domain.Entities;

namespace EkiHire.Core.Domain.DataTransferObjects
{
    public class AdPropertyDTO
    {
        #region AdProperty
        public string Name { get; set; }
        public string PropertyType { get; set; }
        public long SubcategoryId { get; set; }
        public string Range { get; set; }
        #endregion

        //public static implicit operator AdPropertyDTO(AdProperty model)
        //{
        //    try
        //    {
        //        if (model != null)
        //        {
        //            AdPropertyDTO response = new AdPropertyDTO
        //            {
        //                Name = model.Name,
        //                PropertyType = model.PropertyType,
        //                SubcategoryDTO = model.Subcategory
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
