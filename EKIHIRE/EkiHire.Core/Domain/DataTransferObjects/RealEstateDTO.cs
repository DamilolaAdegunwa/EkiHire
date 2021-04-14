using EkiHire.Core.Domain.Entities;
using EkiHire.Core.Domain.Entities.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace EkiHire.Core.Domain.DataTransferObjects
{
    public class RealEstateDTO
    {
        //added properties
        public long Id { get; set; }
        // properties from the entities
        public string Name { get; set; }
        public RealEstateAdsStatus RealEstateAdsStatus { get; set; }
        //implicit  conversion
        public static implicit operator RealEstateDTO(RealEstate realEstate)
        {
            if(realEstate != null)
            {
                return new RealEstateDTO
                {
                    Id = realEstate.Id,
                    Name = realEstate.Name,
                    RealEstateAdsStatus = realEstate.RealEstateAdsStatus
                };
            }
            return null;
        }
    }
}
