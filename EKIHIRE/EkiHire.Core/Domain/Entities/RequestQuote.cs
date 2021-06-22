using System;
using System.Collections.Generic;
using System.Text;
using EkiHire.Core.Domain.Entities.Auditing;
using System.ComponentModel.DataAnnotations;
using EkiHire.Core.Domain.Entities.Enums;
using EkiHire.Core.Domain.DataTransferObjects;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using EkiHire.Core.Domain.Extensions;
namespace EkiHire.Core.Domain.Entities
{
    public class RequestQuote : FullAuditedEntity
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
        [ForeignKey("RequesterId")]
        public virtual long? RequesterId { get; set; }
        //[NotMapped]
        public virtual User Requester { get; set; }
        #endregion

        #region other properties

        #endregion

        #region implicit & explicit conversion between entity & dto
        public static implicit operator RequestQuote(RequestQuoteDTO model)
        {
            try
            {
                if (model == null)
                {
                    return null;
                }
                RequestQuote response = new RequestQuote() {
                ContactForPrice = model.ContactForPrice,
                Name = model.Name,
                Need = model.Need,
                Region = model.Region,
                RequestDate = model.RequestDate,
                PriceRangeLower = model.PriceRangeLower,
                PriceRangeUpper =model.PriceRangeUpper,
                Requester = model.Requester,
                RequesterId = model.RequesterId,
                UpForNegotiation = model.UpForNegotiation,
                Id = model.Id,
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
