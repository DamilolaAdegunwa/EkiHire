using EkiHire.Core.Domain.Entities.Auditing;
using System.ComponentModel.DataAnnotations.Schema;

namespace EkiHire.Core.Domain.Entities
{
    public class CartItem: FullAuditedEntity
    {
        #region user cart
        [ForeignKey("AdId")]
        public virtual long? AdId { get; set; }
        public virtual Ad Ad { get; set; }
        [ForeignKey("UserId")]
        public virtual long? UserId { get; set; }
        public virtual User User { get; set; }
        #endregion

        //public static implicit operator CartItem(CartItemDTO model)
        //{
        //    if(model != null)
        //    {
        //        var output = new CartItem
        //        {
        //            AdId = model.AdId,
        //            Ad = model.Ad,
        //            User = model.User,
        //            UserId = model.UserId,
        //            Id = model.Id
        //        };
        //        return output;
        //    }
        //    return null;
        //}
    }
}
