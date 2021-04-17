using EkiHire.Core.Domain.Entities.Auditing;
using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel.DataAnnotations;

namespace EkiHire.Core.Domain.Entities
{
    public class Item : FullAuditedEntity
    {
        #region all item
        [DataType(DataType.Text)]
        public string Name { get; set; }
        [DataType(DataType.Text)]
        public string Keywords { get; set; }
        [DataType(DataType.Text)]
        public string ImagePath { get; set; }
        [DataType(DataType.Text)]
        public string GroupName { get; set; }
        public long Order { get; set; }
        public Subcategory Subcategory { get; set; }
        #endregion 


    }
}
//category > subcategory > item > class >  subclass 