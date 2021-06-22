using EkiHire.Core.Domain.Entities.Auditing;
using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel.DataAnnotations;
using EkiHire.Core.Domain.Entities.Enums;
using EkiHire.Core.Domain.Entities;
using EkiHire.Core.Domain.Extensions;
namespace EkiHire.Core.Domain.DataTransferObjects
{
    public class RequestQuoteDTO : EntityDTO<long>
    {
        #region properties
        public string Name { get; set; }
        public string Need { get; set; }
        public string Region { get; set; }
        public DateTime? RequestDate { get; set; }
        public decimal? PriceRangeLower { get; set; }
        public decimal? PriceRangeUpper { get; set; }
        public bool? ContactForPrice { get; set; }
        public bool? UpForNegotiation { get; set; }
        //[ForeignKey("RequesterId")]
        public virtual long? RequesterId { get; set; }
        //[NotMapped]
        public virtual UserDTO Requester { get; set; }
        #endregion

        #region other properties

        #endregion

        #region implicit & explicit conversion between entity & dto
        public static implicit operator RequestQuoteDTO(RequestQuote model)
        {
            try
            {
                if (model == null)
                {
                    return null;
                }
                RequestQuoteDTO response = new RequestQuoteDTO()
                {
                    ContactForPrice = model.ContactForPrice,
                    Name = model.Name,
                    Need = model.Need,
                    Region = model.Region,
                    RequestDate = model.RequestDate,
                    PriceRangeLower = model.PriceRangeLower,
                    PriceRangeUpper = model.PriceRangeUpper,
                    Requester = model.Requester,
                    RequesterId = model.RequesterId,
                    UpForNegotiation = model.UpForNegotiation,
                    Id = model.Id
                };
                return response;
            }
            catch (Exception ex)
            {
                return null;
            }
        }
        #endregion
    }
}
