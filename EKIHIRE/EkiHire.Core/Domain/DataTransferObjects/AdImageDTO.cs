using EkiHire.Core.Domain.Entities;
using EkiHire.Core.Domain.Entities.Auditing;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace EkiHire.Core.Domain.DataTransferObjects
{
    public class AdImageDTO: EntityDTO<long>
    {
        #region ad image properties
        [DataType(DataType.Text)] public virtual string ImagePath { get; set; }
        [DataType(DataType.Text)] public virtual string ImageString { get; set; }
        [ForeignKey("AdId")]
        public virtual long? AdId { get; set; }
        public virtual Ad Ad { get; set; }
        #endregion

        #region other property 

        #endregion

        public static implicit operator AdImageDTO(AdImage model)
        {
            try
            {
                if(model != null)
                {
                    AdImageDTO response = new AdImageDTO
                    {
                        ImagePath = model.ImagePath,
                        ImageString = model.ImageString,
                        AdId = model.AdId,
                        Ad = model.Ad,
                        Id = model.Id
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
