using EkiHire.Core.Domain.Entities.Auditing;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EkiHire.Core.Domain.Entities
{
    public class Item : FullAuditedEntity
    {
        #region all item
        [DataType(DataType.Text)]
        public virtual string Name { get; set; }
        [DataType(DataType.Text)]
        public virtual string Keywords { get; set; }
        [DataType(DataType.Text)]
        public virtual string ImagePath { get; set; }
        [DataType(DataType.Text)]
        public virtual string GroupName { get; set; }
        public virtual long Order { get; set; }
        [ForeignKey("SubcategoryId")]
        public virtual long? SubcategoryId { get; set; }
        public virtual Subcategory Subcategory { get; set; }
        #endregion 

        //public static implicit operator Item(ItemDTO model)
        //{
        //    if (model != null)
        //    {
        //        var output = new Item
        //        {
        //            GroupName = model.GroupName,
        //            ImagePath = model.ImagePath,
        //            Keywords = model.Keywords,
        //            Name = model.Name,
        //            Order = model.Order,
        //            SubcategoryId = model.SubcategoryId,
        //            Subcategory = model.Subcategory,
        //        };
        //        return output;
        //    }
        //    return null;
        //}
    }
}
//category > subcategory > item > class >  subclass > unnit > subunit