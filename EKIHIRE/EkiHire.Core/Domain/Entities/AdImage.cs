using EkiHire.Core.Domain.Entities.Auditing;
using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using EkiHire.Core.Domain.DataTransferObjects;

namespace EkiHire.Core.Domain.Entities
{
    public class AdImage: FullAuditedEntity
    {
        #region ad image properties
        [DataType(DataType.Text)] public virtual string ImagePath { get; set; }
        [DataType(DataType.Text)] public virtual string ImageString { get; set; }
        [ForeignKey("AdId")]
        public virtual long? AdId { get; set; }
        [NotMapped]
        public virtual Ad Ad { get; set; }
        #endregion

        public static implicit operator AdImage(AdImageDTO model)
        {
            try
            {
                if (model != null)
                {
                    AdImage response = new AdImage
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
